
using System.Collections.Generic;


namespace InventoryCounter
{
    /// <summary>
    /// Information about a single line item to be printed in a CSV file.
    /// </summary>
    public class CsvRecord
    {
        public string WorthS
        {
            get
            {
                if (WorthD != null)
                    return ((decimal)WorthD).ToString("C2");
                else
                    return string.Empty;
            }
        }
        
        public decimal? WorthD { get; private set; }
        public string Date { get; private set; }
        public string Description { get; private set; }

        public bool IsErrorRow { get; set; } = false;

        


        public CsvRecord(string description, decimal? worth = null, string date = "")
        {            
            Description = description;
            WorthD = worth;
            Date = date;
        }

        public CsvRecord DeepCopy()
        {
            CsvRecord copy = new (Description, WorthD, Date)
            {
                IsErrorRow = IsErrorRow
            };
            return copy;
        }
    }
}

   