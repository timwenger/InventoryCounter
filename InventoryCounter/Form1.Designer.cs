namespace InventoryCounter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.directoryEntry = new System.Windows.Forms.TextBox();
            this.directoryLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.messagesBox = new System.Windows.Forms.TextBox();
            this.radioButton_jpg = new System.Windows.Forms.RadioButton();
            this.radioButton_png = new System.Windows.Forms.RadioButton();
            this.radioButton_pdf = new System.Windows.Forms.RadioButton();
            this.groupBox_filetypes = new System.Windows.Forms.GroupBox();
            this.textBox_fileNameFormat = new System.Windows.Forms.TextBox();
            this.checkBox_date = new System.Windows.Forms.CheckBox();
            this.checkBox_value = new System.Windows.Forms.CheckBox();
            this.checkBox_description = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox_filetypes.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // directoryEntry
            // 
            this.directoryEntry.ForeColor = System.Drawing.Color.DimGray;
            this.directoryEntry.Location = new System.Drawing.Point(129, 111);
            this.directoryEntry.Name = "directoryEntry";
            this.directoryEntry.Size = new System.Drawing.Size(618, 23);
            this.directoryEntry.TabIndex = 0;
            // 
            // directoryLabel
            // 
            this.directoryLabel.AutoSize = true;
            this.directoryLabel.Location = new System.Drawing.Point(66, 114);
            this.directoryLabel.Name = "directoryLabel";
            this.directoryLabel.Size = new System.Drawing.Size(57, 15);
            this.directoryLabel.TabIndex = 1;
            this.directoryLabel.Text = "directory:";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(66, 140);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(146, 23);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // ResultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(240, 144);
            this.resultLabel.Name = "ResultLabel";
            this.resultLabel.Size = new System.Drawing.Size(67, 15);
            this.resultLabel.TabIndex = 3;
            this.resultLabel.Text = "ResultLabel";
            // 
            // messagesBox
            // 
            this.messagesBox.Location = new System.Drawing.Point(66, 173);
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "messagesBox";
            this.messagesBox.ReadOnly = true;
            this.messagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messagesBox.Size = new System.Drawing.Size(681, 84);
            this.messagesBox.TabIndex = 4;
            // 
            // radioButton_jpg
            // 
            this.radioButton_jpg.AutoSize = true;
            this.radioButton_jpg.Checked = true;
            this.radioButton_jpg.Location = new System.Drawing.Point(14, 22);
            this.radioButton_jpg.Name = "radioButton_jpg";
            this.radioButton_jpg.Size = new System.Drawing.Size(66, 19);
            this.radioButton_jpg.TabIndex = 5;
            this.radioButton_jpg.TabStop = true;
            this.radioButton_jpg.Text = "jpg files";
            this.radioButton_jpg.UseVisualStyleBackColor = true;
            this.radioButton_jpg.CheckedChanged += new System.EventHandler(this.radioButton_jpg_CheckedChanged);
            // 
            // radioButton_png
            // 
            this.radioButton_png.AutoSize = true;
            this.radioButton_png.Location = new System.Drawing.Point(86, 22);
            this.radioButton_png.Name = "radioButton_png";
            this.radioButton_png.Size = new System.Drawing.Size(70, 19);
            this.radioButton_png.TabIndex = 6;
            this.radioButton_png.Text = "png files";
            this.radioButton_png.UseVisualStyleBackColor = true;
            this.radioButton_png.CheckedChanged += new System.EventHandler(this.radioButton_png_CheckedChanged);
            // 
            // radioButton_pdf
            // 
            this.radioButton_pdf.AutoSize = true;
            this.radioButton_pdf.Location = new System.Drawing.Point(162, 22);
            this.radioButton_pdf.Name = "radioButton_pdf";
            this.radioButton_pdf.Size = new System.Drawing.Size(67, 19);
            this.radioButton_pdf.TabIndex = 7;
            this.radioButton_pdf.Text = "pdf files";
            this.radioButton_pdf.UseVisualStyleBackColor = true;
            this.radioButton_pdf.CheckedChanged += new System.EventHandler(this.radioButton_pdf_CheckedChanged);
            // 
            // groupBox_filetypes
            // 
            this.groupBox_filetypes.Controls.Add(this.radioButton_pdf);
            this.groupBox_filetypes.Controls.Add(this.radioButton_jpg);
            this.groupBox_filetypes.Controls.Add(this.radioButton_png);
            this.groupBox_filetypes.Location = new System.Drawing.Point(68, 19);
            this.groupBox_filetypes.Name = "groupBox_filetypes";
            this.groupBox_filetypes.Size = new System.Drawing.Size(251, 53);
            this.groupBox_filetypes.TabIndex = 8;
            this.groupBox_filetypes.TabStop = false;
            this.groupBox_filetypes.Text = "File Type to Search";
            // 
            // textBox_fileNameFormat
            // 
            this.textBox_fileNameFormat.Enabled = false;
            this.textBox_fileNameFormat.ForeColor = System.Drawing.Color.DimGray;
            this.textBox_fileNameFormat.Location = new System.Drawing.Point(16, 46);
            this.textBox_fileNameFormat.Name = "textBox_fileNameFormat";
            this.textBox_fileNameFormat.Size = new System.Drawing.Size(400, 23);
            this.textBox_fileNameFormat.TabIndex = 9;
            // 
            // checkBox_date
            // 
            this.checkBox_date.AutoSize = true;
            this.checkBox_date.Location = new System.Drawing.Point(16, 21);
            this.checkBox_date.Name = "checkBox_date";
            this.checkBox_date.Size = new System.Drawing.Size(92, 19);
            this.checkBox_date.TabIndex = 10;
            this.checkBox_date.Text = "yyyy,mm,dd";
            this.checkBox_date.UseVisualStyleBackColor = true;
            this.checkBox_date.CheckedChanged += new System.EventHandler(this.checkBox_date_CheckedChanged);
            // 
            // checkBox_value
            // 
            this.checkBox_value.AutoSize = true;
            this.checkBox_value.Location = new System.Drawing.Point(114, 21);
            this.checkBox_value.Name = "checkBox_value";
            this.checkBox_value.Size = new System.Drawing.Size(59, 19);
            this.checkBox_value.TabIndex = 11;
            this.checkBox_value.Text = "$99.99";
            this.checkBox_value.UseVisualStyleBackColor = true;
            this.checkBox_value.CheckedChanged += new System.EventHandler(this.checkBox_value_CheckedChanged);
            // 
            // checkBox_description
            // 
            this.checkBox_description.AutoSize = true;
            this.checkBox_description.Checked = true;
            this.checkBox_description.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_description.Enabled = false;
            this.checkBox_description.Location = new System.Drawing.Point(179, 21);
            this.checkBox_description.Name = "checkBox_description";
            this.checkBox_description.Size = new System.Drawing.Size(85, 19);
            this.checkBox_description.TabIndex = 12;
            this.checkBox_description.Text = "description";
            this.checkBox_description.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_fileNameFormat);
            this.groupBox1.Controls.Add(this.checkBox_description);
            this.groupBox1.Controls.Add(this.checkBox_date);
            this.groupBox1.Controls.Add(this.checkBox_value);
            this.groupBox1.Location = new System.Drawing.Point(325, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 78);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Name Format";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 268);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_filetypes);
            this.Controls.Add(this.messagesBox);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.directoryLabel);
            this.Controls.Add(this.directoryEntry);
            this.Name = "Form1";
            this.Text = "Inventory Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_filetypes.ResumeLayout(false);
            this.groupBox_filetypes.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox directoryEntry;
        private System.Windows.Forms.Label directoryLabel;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.TextBox messagesBox;
        private System.Windows.Forms.RadioButton radioButton_jpg;
        private System.Windows.Forms.RadioButton radioButton_png;
        private System.Windows.Forms.RadioButton radioButton_pdf;
        private System.Windows.Forms.GroupBox groupBox_filetypes;
        private System.Windows.Forms.TextBox textBox_fileNameFormat;
        private System.Windows.Forms.CheckBox checkBox_date;
        private System.Windows.Forms.CheckBox checkBox_value;
        private System.Windows.Forms.CheckBox checkBox_description;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

