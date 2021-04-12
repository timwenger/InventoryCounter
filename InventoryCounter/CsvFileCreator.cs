using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Dynamic;

namespace InventoryCounter
{
    /// <summary>
    /// For printing CSV records into a CSV file.
    /// Potentially multiple formats possible.
    /// </summary>
    public class CsvFileCreator
    {
        private readonly string _folderPath;
        private readonly CsvRecordCollection _folders;
        private readonly CsvRecordCollection _files;
        private readonly List<CsvRecord> _printout = new();
        public CsvFileCreator(string folderPath, CsvRecordCollection folders, CsvRecordCollection files)
        {
            _folderPath = folderPath;
            _folders = folders;
            _files = files;
        }

        /// <summary>
        /// returns false if there were errors writing to CSV file
        /// </summary>        
        public bool CreateFolderInventoryCsvFile()
        {
            _printout.Clear();
            decimal foldersWorth = _folders.TotalWorth;
            decimal filesWorth = _files.TotalWorth;
            
            AppendAnyFoldersInventory(foldersWorth);
            AppendAnyFilesInventory(filesWorth);
            if (_folders.Count > 0 || _files.Count > 0)
            {
                if (PrintValues)
                    AppendGrandTotalToPrintout(foldersWorth + filesWorth);
                return WriteToCsvFile();
            }
            return true;
        }

        private void AppendAnyFoldersInventory(decimal foldersWorth)
        {
            if (_folders.Count > 0)
            {
                AppendTitleToPrintout("Inventory in subdirectories");
                _printout.AddRange(_folders.GetCollectionCopy());
                if (PrintValues)
                    AppendSubTotalToPrintout(foldersWorth);
            }
        }

        private void AppendAnyFilesInventory(decimal filesWorth)
        {
            if (_files.Count > 0)
            {
                AppendTitleToPrintout("Inventory in this directory");
                _printout.AddRange(_files.GetCollectionCopy());
                if (PrintValues)
                    AppendSubTotalToPrintout(filesWorth);
            }
        }

        private void AppendTitleToPrintout(string titleText)
        {
            AppendBlankRowToPrintout();

            CsvRecord titleRow = new (titleText);
            _printout.Add(titleRow);
            AppendBlankRowToPrintout();
        }

        private void AppendBlankRowToPrintout()
        {
            CsvRecord blankRow = new (string.Empty);
            _printout.Add(blankRow);
        }

        private void AppendTotalToPrintout(decimal total, string msg)
        {
            CsvRecord totalRow = new (msg, total);
            _printout.Add(totalRow);
        }

        private void AppendSubTotalToPrintout(decimal total)
        {
            AppendBlankRowToPrintout();
            AppendTotalToPrintout(total, "SubTotal");
        }

        private void AppendGrandTotalToPrintout(decimal total)
        {
            AppendTotalToPrintout(total, "Grand Total");
        }

        /// <summary>
        /// returns false if there were errors writing to CSV file
        /// </summary>
        private bool WriteToCsvFile()
        {
            List<dynamic> flexPrintout = CopyPrintoutToFlexList();
            try
            {
                using var writer = new StreamWriter(_folderPath + "\\inventory.csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(flexPrintout);
            }
            catch (IOException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        private List<dynamic> CopyPrintoutToFlexList()
        {
            List<dynamic> flexPrintout = new();
            foreach (CsvRecord record in _printout)
            {
                dynamic flexRecord = new ExpandoObject();
                flexPrintout.Add(flexRecord);
                if (PrintValues)
                    flexRecord.Worth = record.WorthS;
                if (PrintDates)
                    flexRecord.Date = record.Date;
                flexRecord.Description = record.Description;
            }
            return flexPrintout;
        }

        private static bool PrintDates { get { return SearchOptions.Instance.SearchForDates; } }
        private static bool PrintValues { get { return SearchOptions.Instance.SearchForValues; } }
    }
}
