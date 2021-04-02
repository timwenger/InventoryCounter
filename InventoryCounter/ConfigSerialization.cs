using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

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
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SearchOptions.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, SearchOptions.Instance);
            stream.Close();
        }

        private static SearchOptions DeSerializeSearchOptions()
        {
            IFormatter formatter = new BinaryFormatter();
            SearchOptions startupSearchOptions;
            try
            {
                Stream stream = new FileStream("SearchOptions.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                startupSearchOptions = (SearchOptions)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                startupSearchOptions = LoadDefaultConfig();
            }
            return startupSearchOptions;
        }

        private static SearchOptions LoadDefaultConfig()
        {
            SearchOptions defaultConfig = SearchOptions.Instance;
            defaultConfig.Directory = SearchOptions.defaultDirectoryText;
            defaultConfig.FileExtension = Extension.jpg;
            defaultConfig.fNameFormatDict.Add(ChkBx.value, ChkBxText.value);
            return defaultConfig;
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
