using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;

namespace InventoryCounter
{
    public struct GuiConfigObjects
    {
        public TextBox DirectoryEntry { get; set;
        }
        public RadioButton RadioButton_jpg { get; set; }
        public RadioButton RadioButton_png { get; set; }
        public RadioButton RadioButton_pdf { get; set; }

        public CheckBox CheckBox_value { get; set; }
        public CheckBox CheckBox_date { get; set; }
    }

    public static class ConfigSerialization
    {

        
        public static void SaveSearchOptions()
        {
            SerializeSearchOptions();
        }

        public static void LoadSearchOptions(GuiConfigObjects configObjects)
        {
            SearchOptions startupSearchOptions = DeSerializeSearchOptions();
            InitializeSearchOptions(startupSearchOptions, configObjects);
        }

        private static void SerializeSearchOptions()
        {
            DataContractSerializer serializer = new(typeof(SearchOptions),
                [typeof(string), typeof(bool), typeof(OrderedDictionary), typeof(ChkBx)]);
            Stream stream = new FileStream("SearchOptions.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.WriteObject(stream, SearchOptions.Instance);
            stream.Close();
        }

        private static SearchOptions DeSerializeSearchOptions()
        {
            DataContractSerializer serializer = new (typeof(SearchOptions),
                [typeof(string), typeof(bool), typeof(OrderedDictionary), typeof(ChkBx)]);
            SearchOptions startupSearchOptions = LoadDefaultSearchOptions();
            Stream stream = new FileStream("SearchOptions.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                var obj = serializer.ReadObject(stream);
                if (obj is SearchOptions so)
                    startupSearchOptions = so;
                stream.Close();
            }
            catch (XmlException)
            {
                startupSearchOptions = LoadDefaultSearchOptions();
                stream.Close();

            }
            catch (SerializationException)
            {
                // If there is a serialization problem (unknown types) fall back to defaults
                startupSearchOptions = LoadDefaultSearchOptions();
                stream.Close();
            }
            return startupSearchOptions;
        }

        private static SearchOptions LoadDefaultSearchOptions()
        {
            SearchOptions defaultConfig = SearchOptions.Instance;
            defaultConfig.Directory = GetProgramsParentDirectory();
            defaultConfig.FileExtension = Extension.jpg;
            defaultConfig.fNameFormatDict[ChkBx.value] = ChkBxText.value;
            return defaultConfig;
        }

        private static string GetProgramsParentDirectory()
        {
            string cd = Environment.CurrentDirectory;
            int indexOfLastDirectorySlash = cd.LastIndexOf(@"\");
            string parentCd = cd.Substring(0, indexOfLastDirectorySlash);
            return parentCd;
        }

        private static void InitializeSearchOptions(SearchOptions searchOptionsFromFile, GuiConfigObjects configObjects)
        {
            configObjects.DirectoryEntry.Text = searchOptionsFromFile.Directory;

            if (searchOptionsFromFile.FileExtension == Extension.jpg)
                configObjects.RadioButton_jpg.Checked = true;
            else if (searchOptionsFromFile.FileExtension == Extension.png)
                configObjects.RadioButton_png.Checked = true;
            else if (searchOptionsFromFile.FileExtension == Extension.pdf)
                configObjects.RadioButton_pdf.Checked = true;
            GUI.UpdateFileExtensionSearchOption(configObjects);

            foreach (ChkBx chkBx in searchOptionsFromFile.fNameFormatDict.Keys)
            {
                switch (chkBx)
                {
                    case ChkBx.date:
                        configObjects.CheckBox_date.Checked = true;
                        break;
                    case ChkBx.value:
                        configObjects.CheckBox_value.Checked = true;
                        break;
                }
            }
        }
    }
}
