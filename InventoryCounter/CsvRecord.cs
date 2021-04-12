
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
                if (WorthF != null)
                    return ((float)WorthF).ToString("C2");
                else
                    return string.Empty;
            }
        }
        
        public float? WorthF { get; private set; }
        public string Date { get; private set; }
        public string Description { get; private set; }

        public bool IsErrorRow { get; set; } = false;

        


        public CsvRecord(string description, float? worth = null, string date = "")
        {            
            Description = description;
            WorthF = worth;
            Date = date;
        }

        public CsvRecord DeepCopy()
        {
            CsvRecord copy = new (Description, WorthF, Date)
            {
                IsErrorRow = IsErrorRow
            };
            return copy;
        }
    }
}

   