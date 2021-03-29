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
        private CsvRecordCollection _folders;
        private CsvRecordCollection _pics;
        private List<CsvRecord> _printout = new List<CsvRecord>();
        public CsvFileCreator(string folderPath, CsvRecordCollection folders, CsvRecordCollection pics)
        {
            _folderPath = folderPath;
            _folders = folders;
            _pics = pics;
        }

        /// <summary>
        /// returns false if there were errors writing to CSV file
        /// </summary>        
        public bool CreateFolderInventoryCsvFile()
        {
            _printout.Clear();
            float? foldersWorth = _folders.TotalWorth;
            float? picsWorth = _pics.TotalWorth;
            float? grandTotal = null;
            if (foldersWorth != null && foldersWorth != null) grandTotal = foldersWorth + picsWorth;
            else if (foldersWorth != null) grandTotal = foldersWorth;
            else if (picsWorth != null) grandTotal = picsWorth;

            AppendFoldersInventory(foldersWorth);
            AppendPicsInventory(picsWorth);
            if (_folders.Count > 0 || _pics.Count > 0)
            {
                if (grandTotal != null)
                    AppendGrandTotalToPrintout((float)grandTotal);
                return WriteToCsvFile();
            }
            return true;
        }

        private void AppendFoldersInventory(float? foldersWorth)
        {
            if (_folders.Count > 0)
            {
                AppendTitleToPrintout("Inventory in subdirectories");
                _printout.AddRange(_folders.GetCollectionCopy());
                if(foldersWorth != null)
                    AppendSubTotalToPrintout((float)foldersWorth);
            }
        }

        private void AppendPicsInventory(float? picsWorth)
        {
            if (_pics.Count > 0)
            {
                AppendTitleToPrintout("Inventory in this directory");
                _printout.AddRange(_pics.GetCollectionCopy());
                if (picsWorth != null)
                    AppendSubTotalToPrintout((float)picsWorth);
            }
        }

        private void AppendTitleToPrintout(string titleText)
        {
            AppendBlankRowToPrintout();

            CsvRecord titleRow = new CsvRecord(titleText);
            _printout.Add(titleRow);
            AppendBlankRowToPrintout();
        }

        private void AppendBlankRowToPrintout()
        {
            CsvRecord blankRow = new CsvRecord(string.Empty);
            _printout.Add(blankRow);
        }

        private void AppendTotalToPrintout(float total, string msg)
        {
            CsvRecord totalRow = new CsvRecord(msg, total);
            _printout.Add(totalRow);
        }

        private void AppendSubTotalToPrintout(float total)
        {
            AppendBlankRowToPrintout();
            AppendTotalToPrintout(total, "SubTotal");
        }

        private void AppendGrandTotalToPrintout(float total)
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
                using (var writer = new StreamWriter(_folderPath + "\\inventory.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(flexPrintout);
                }
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
            List<dynamic> flexPrintout = new List<dynamic>();
            foreach(CsvRecord record in _printout)
            {
                dynamic flexRecord = new ExpandoObject();
                flexPrintout.Add(flexRecord);
                if(SearchOptions.Instance.SearchForValues)
                    flexRecord.Worth = record.WorthS;
                if (SearchOptions.Instance.SearchForDates)
                    flexRecord.Date = record.Date;
                flexRecord.Description = record.Description;
            }
            return flexPrintout;
        }        
    }
}
