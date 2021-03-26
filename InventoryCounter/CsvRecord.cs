
using System.Collections.Generic;


namespace InventoryCounter
{
    /// <summary>
    /// Information about a single line item to be printed in a CSV file.
    /// </summary>
    public class CsvRecord
    {
        public string Worth
        {
            get
            {
                if (worth != null)
                    return "$" + worth.ToString();
                else
                    return string.Empty;
            }
        }

        public string Description { get; set; }

        public bool IsErrorRow = false;

        // ideally float? worth would be a public get, private set. It should only be set by constructor.
        // But if it is a property, then the CSV writer prints it.
        public float? worth;

        public CsvRecord(float? appraisal, string description)
        {
            worth = appraisal;
            Description = description;
        }

        public CsvRecord(string description)
        {
            Description = description;
        }

        public CsvRecord()
        {
            Description = string.Empty;
        }

        public CsvRecord DeepCopy()
        {
            CsvRecord copy = new CsvRecord(worth, Description);
            copy.IsErrorRow = IsErrorRow;
            return copy;
        }
    }
}

   