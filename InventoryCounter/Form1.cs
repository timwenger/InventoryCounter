using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace InventoryCounter
{
    public partial class Form1 : Form
    {
        private SearchOptions _searchOptions;
        private string defaultDirectoryText = "(uses this program's parent folder as default)";

        private FormResultsObjects resultsObjs;


        public Label ResultLabel { get { return resultLabel; } }
        public TextBox MessagesBox {get { return messagesBox; } }

        public Form1()
        {
            InitializeComponent();
            resultsObjs.ResultLabel = resultLabel;
            resultsObjs.MessagesBox = messagesBox;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            string topDirectory = GetUserEnteredDirectory();

            ResultPrinter.PrintResults(topDirectory, resultsObjs);
        }

        string GetUserEnteredDirectory()
        {
            string userGivenDirectory = directoryEntry.Text;
            if (userGivenDirectory == defaultDirectoryText)
            {
                string cd = Environment.CurrentDirectory;
                int indexOfLastDirectorySlash = cd.LastIndexOf(@"\");
                string parentCd = cd.Substring(0, indexOfLastDirectorySlash);
                directoryEntry.Text = parentCd;
                return parentCd;
            }
            return userGivenDirectory;
        }

        private void checkBox_date_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_date.Checked && !_searchOptions.fNameFormatDict.Contains(ChkBx.date))
                _searchOptions.fNameFormatDict.Add(ChkBx.date, checkBox_date.Text);
            else if (!checkBox_date.Checked && _searchOptions.fNameFormatDict.Contains(ChkBx.date))
                _searchOptions.fNameFormatDict.Remove(ChkBx.date);

            UpdateFileNameFormatTextBox();
        }

        private void checkBox_value_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_value.Checked && !_searchOptions.fNameFormatDict.Contains(ChkBx.value))
                _searchOptions.fNameFormatDict.Add(ChkBx.value, checkBox_value.Text);
            else if (!checkBox_value.Checked && _searchOptions.fNameFormatDict.Contains(ChkBx.value))
                _searchOptions.fNameFormatDict.Remove(ChkBx.value);

            UpdateFileNameFormatTextBox();
        }

        private void UpdateFileNameFormatTextBox()
        {
            textBox_fileNameFormat.Text = _searchOptions.FNameFormat;
        }

        private void radioButton_jpg_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption();
        }

        private void radioButton_png_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption();
        }

        private void radioButton_pdf_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption();
        }

        private void UpdateFileExtensionSearchOption()
        {
            if (radioButton_jpg.Checked)
                _searchOptions.FileExtension = Extension.jpg;
            else if (radioButton_png.Checked)
                _searchOptions.FileExtension = Extension.png;
            else if (radioButton_pdf.Checked)
                _searchOptions.FileExtension = Extension.pdf;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _searchOptions = SearchOptions.Instance;
            SearchOptions searchOptionsFromFile = DeSerializeSearchOptions();
            InitializeSearchOptions(searchOptionsFromFile);
            if(directoryEntry.Text == string.Empty)
                directoryEntry.Text = defaultDirectoryText;
            resultLabel.Text = string.Empty;
            messagesBox.Visible = false;            
        }

        private void InitializeSearchOptions(SearchOptions searchOptionsFromFile)
        {
            directoryEntry.Text = searchOptionsFromFile.Directory;

            if (searchOptionsFromFile.FileExtension == Extension.jpg)
                radioButton_jpg.Checked = true;
            else if (searchOptionsFromFile.FileExtension == Extension.png)
                radioButton_png.Checked = true;
            else if (searchOptionsFromFile.FileExtension == Extension.pdf)
                radioButton_pdf.Checked = true;
            UpdateFileExtensionSearchOption();

            foreach (ChkBx chkBx in searchOptionsFromFile.fNameFormatDict.Keys)
            {
                switch(chkBx)
                {
                    case ChkBx.date:
                        checkBox_date.Checked = true;
                        break;
                    case ChkBx.value:
                        checkBox_value.Checked = true;
                        break;
                }                    
            }
        }
        private SearchOptions DeSerializeSearchOptions()
        {
            IFormatter formatter = new BinaryFormatter();
            SearchOptions searchOptionsFromFile;
            try
            {
                Stream stream = new FileStream("SearchOptions.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                searchOptionsFromFile = (SearchOptions)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                //create default options if config file is not present
                searchOptionsFromFile = SearchOptions.Instance;
                searchOptionsFromFile.Directory = defaultDirectoryText;
                searchOptionsFromFile.FileExtension = Extension.jpg;
                searchOptionsFromFile.fNameFormatDict.Add(ChkBx.value, checkBox_value.Text);
            }
            return searchOptionsFromFile;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchOptions.Directory = directoryEntry.Text;
            SerializeSearchOptions();            
        }

        private void SerializeSearchOptions()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SearchOptions.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, _searchOptions);
            stream.Close();
        }
    }
}
