using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Forms;

namespace InventoryCounter
{
    public partial class GUI : Form
    {
        private SearchOptions _searchOptions;        
        private GuiResultsObjects _resultsObjects;
        private GuiConfigObjects _configObjects;


        public Label ResultLabel { get { return resultLabel; } }
        public TextBox MessagesBox {get { return messagesBox; } }

        public GUI()
        {
            InitializeComponent();
            InitializeResultsObjects();
            InitializeConfigObjects();            
        }

        private void InitializeResultsObjects()
        {
            _resultsObjects.ResultLabel = resultLabel;
            _resultsObjects.MessagesBox = messagesBox;
        }

        private void InitializeConfigObjects()
        {
            _configObjects.DirectoryEntry = directoryEntry;
            _configObjects.RadioButton_jpg = radioButton_jpg;
            _configObjects.RadioButton_png = radioButton_png;
            _configObjects.RadioButton_pdf = radioButton_pdf;
            _configObjects.CheckBox_value = checkBox_value;
            _configObjects.CheckBox_date = checkBox_date;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            ResultPrinter.PrintResults(_searchOptions.Directory, _resultsObjects);
        }

        private void DirectoryEntry_TextChanged(object sender, EventArgs e)
        {
            _searchOptions.Directory = directoryEntry.Text;
        }

        private void CheckBox_date_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_date.Checked && !_searchOptions.fNameFormatDict.Contains(ChkBx.date))
                _searchOptions.fNameFormatDict.Add(ChkBx.date, ChkBxText.date);
            else if (!checkBox_date.Checked && _searchOptions.fNameFormatDict.Contains(ChkBx.date))
                _searchOptions.fNameFormatDict.Remove(ChkBx.date);

            UpdateFileNameFormatTextBox();
        }

        private void CheckBox_value_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_value.Checked && !_searchOptions.fNameFormatDict.Contains(ChkBx.value))
                _searchOptions.fNameFormatDict.Add(ChkBx.value, ChkBxText.value);
            else if (!checkBox_value.Checked && _searchOptions.fNameFormatDict.Contains(ChkBx.value))
                _searchOptions.fNameFormatDict.Remove(ChkBx.value);

            UpdateFileNameFormatTextBox();
        }

        private void UpdateFileNameFormatTextBox()
        {
            textBox_fileNameFormat.Text = _searchOptions.FNameFormat;
        }

        private void RadioButton_jpg_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption(_configObjects);
        }

        private void RadioButton_png_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption(_configObjects);
        }

        private void RadioButton_pdf_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFileExtensionSearchOption(_configObjects);
        }

        public static void UpdateFileExtensionSearchOption(GuiConfigObjects configObjects)
        {
            if (configObjects.RadioButton_jpg.Checked)
                SearchOptions.Instance.FileExtension = Extension.jpg;
            else if (configObjects.RadioButton_png.Checked)
                SearchOptions.Instance.FileExtension = Extension.png;
            else if (configObjects.RadioButton_pdf.Checked)
                SearchOptions.Instance.FileExtension = Extension.pdf;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _searchOptions = SearchOptions.Instance; 
            ConfigSerialization.LoadSearchOptions(_configObjects);                        
            resultLabel.Text = string.Empty;
            messagesBox.Visible = false;            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchOptions.Directory = directoryEntry.Text;
            ConfigSerialization.SaveSearchOptions();         
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new()
            {
                InitialDirectory = _searchOptions.Directory,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directoryEntry.Text = dialog.FileName;
            }
        }
    }
}
