using System;
using System.Text;
using System.Collections.Specialized;

namespace InventoryCounter
{
    public enum ChkBx
    {
        date,
        value
    }

    public struct ChkBxText
    {
        public const string date = "yyyy,mm,dd";
        public const string value = "$99.99";
    }

    public struct Extension
    {
        public const string jpg = "*.jpg";
        public const string png = "*.png";
        public const string pdf = "*.pdf";
    }

    [Serializable]
    public class SearchOptions
    {
        public string Directory { get; set; }
        public string FileExtension { get; set; }

        public bool SearchForValues
        {
            get
            {
                return fNameFormatDict.Contains(ChkBx.value);
            }
        }

        public bool SearchForDates
        {
            get
            {
                return fNameFormatDict.Contains(ChkBx.date);
            }
        }

        public OrderedDictionary fNameFormatDict = new ();

        public string FNameFormat
        {
            get
            {
                StringBuilder sb = new (); 
                foreach(string formatElement in fNameFormatDict.Values)
                {
                    sb.Append(formatElement);
                    sb.Append(' ');
                }
                sb.Append("file description");
                return sb.ToString();
            }
        }

        private static SearchOptions _instance;
        private SearchOptions() { }

        public static SearchOptions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SearchOptions();
                return _instance;
            }
        }

    }
}
