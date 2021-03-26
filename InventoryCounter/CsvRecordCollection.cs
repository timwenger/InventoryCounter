using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryCounter
{
    /// <summary>
    /// A collection of inventory items and their associated errors in a csv file 
    /// that should be grouped together. That might be all the files in a folder, 
    /// or all the folders in a folder, or some other grouping.
    /// The collection can provide its contents and worth at any given time.
    /// </summary>
    public class CsvRecordCollection
    {
        // the methods in this class will be instance methods, as they will operate on a specific RecordCollection instance.
        private List<CsvRecord> _records = new List<CsvRecord>();
        public int Count { get { return _records.Count; } }

        internal void AddErrorRecord(string error)
        {
            AddErrorToCollection(error);
        }

        internal void AddInventoryRecord(float value, string itemName, params string[] errorMessages)
        {
            _records.Add(new CsvRecord(value, itemName));
            foreach (string error in errorMessages)
                AddErrorToCollection(error);
        }

        private void AddErrorToCollection(string errorMessage)
        {
            CsvRecord errorRecord = new CsvRecord(errorMessage) { IsErrorRow = true };
            _records.Add(errorRecord);
        }

        public List<CsvRecord> GetCollectionCopy()
        {
            List<CsvRecord> printout = new List<CsvRecord>();
            foreach (CsvRecord record in _records)
            {
                printout.Add(record.DeepCopy());
            }
            return printout;
        }

        public List<CsvRecord> GetCollectionErrorsCopy()
        {
            List<CsvRecord> printout = new List<CsvRecord>();
            foreach (CsvRecord record in _records)
            {
                if (record.IsErrorRow)
                    printout.Add(record.DeepCopy());
            }
            return printout;
        }

        public float TotalWorth
        {
            get
            {
                float sum = 0;
                foreach (var record in _records)
                {
                    if (record.worth != null)
                        sum += (float)record.worth;
                }
                return sum;
            }
        }
    }
}
