namespace MD5_Pro_Gramer
{
    partial class redumpTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.gameName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Company = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.size = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.crc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.md5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.sha1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.redumpTest = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.dataSet11 = new MD5_Pro_Gramer.DataSet1();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.gameName);
            this.objectListView1.AllColumns.Add(this.Company);
            this.objectListView1.AllColumns.Add(this.size);
            this.objectListView1.AllColumns.Add(this.crc);
            this.objectListView1.AllColumns.Add(this.md5);
            this.objectListView1.AllColumns.Add(this.sha1);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.gameName,
            this.Company,
            this.size,
            this.crc,
            this.md5,
            this.sha1});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(1295, 116);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // gameName
            // 
            this.gameName.AspectName = "FileName";
            this.gameName.Text = "Game Name";
            this.gameName.Width = 524;
            // 
            // Company
            // 
            this.Company.AspectName = "Company";
            this.Company.Text = "Company";
            this.Company.Width = 206;
            // 
            // size
            // 
            this.size.AspectName = "size";
            this.size.Text = "Size";
            this.size.Width = 75;
            // 
            // crc
            // 
            this.crc.AspectName = "crc";
            this.crc.Text = "CRC";
            // 
            // md5
            // 
            this.md5.AspectName = "MD5Hash";
            this.md5.Text = "MD5";
            this.md5.Width = 277;
            // 
            // sha1
            // 
            this.sha1.AspectName = "sha1";
            this.sha1.Text = "SHA1";
            this.sha1.Width = 342;
            // 
            // redumpTest
            // 
            this.redumpTest.Location = new System.Drawing.Point(13, 422);
            this.redumpTest.Name = "redumpTest";
            this.redumpTest.Size = new System.Drawing.Size(75, 23);
            this.redumpTest.TabIndex = 1;
            this.redumpTest.Text = "redump Test";
            this.redumpTest.UseVisualStyleBackColor = true;
            this.redumpTest.Click += new System.EventHandler(this.redumpTest_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 158);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1294, 99);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(709, 426);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(598, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // redumpTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 461);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.redumpTest);
            this.Controls.Add(this.objectListView1);
            this.Name = "redumpTable";
            this.Text = "redumpTable";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn gameName;
        private BrightIdeasSoftware.OLVColumn Company;
        private BrightIdeasSoftware.OLVColumn size;
        private BrightIdeasSoftware.OLVColumn crc;
        private BrightIdeasSoftware.OLVColumn md5;
        private BrightIdeasSoftware.OLVColumn sha1;
        private System.Windows.Forms.Button redumpTest;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private DataSet1 dataSet11;
    }
}