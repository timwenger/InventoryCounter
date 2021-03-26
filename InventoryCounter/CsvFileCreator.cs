using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Globalization;

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
            float foldersWorth = _folders.TotalWorth;
            float picsWorth = _pics.TotalWorth;
            AppendFoldersInventory(foldersWorth);
            AppendPicsInventory(picsWorth);
            if (_folders.Count > 0 || _pics.Count > 0)
            {
                AppendGrandTotalToPrintout(picsWorth + foldersWorth);
                return WriteToCsvFile();
            }
            return true;
        }

        private void AppendFoldersInventory(float foldersWorth)
        {
            if (_folders.Count > 0)
            {
                AppendTitleToPrintout("Inventory in subdirectories");
                _printout.AddRange(_folders.GetCollectionCopy());
                AppendSubTotalToPrintout(foldersWorth);
            }
        }

        private void AppendPicsInventory(float picsWorth)
        {
            if (_pics.Count > 0)
            {
                AppendTitleToPrintout("Inventory in this directory");
                _printout.AddRange(_pics.GetCollectionCopy());
                AppendSubTotalToPrintout(picsWorth);
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
            CsvRecord blankRow = new CsvRecord();
            _printout.Add(blankRow);
        }

        private void AppendTotalToPrintout(float total, string msg)
        {
            CsvRecord totalRow = new CsvRecord(total, msg);
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
            try
            {
                using (var writer = new StreamWriter(_folderPath + "\\inventory.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_printout);
                }
            }
            catch (IOException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
    }
}
