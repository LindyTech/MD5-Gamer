using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using MD5_Pro_Gramer.Properties;
using System.Runtime.Remoting.Channels;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using BrightIdeasSoftware;
using System.Security;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MySqlX.XDevAPI.Relational;

namespace MD5_Pro_Gramer
{
    public partial class Form1 : Form
    {
        List<GameFile> masterList = new List<GameFile>();

        public Form1()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            List<string> alreadyInList = new List<string>();
            runBtn.Enabled = false;
            openBtn.Enabled = false;
            status.Text = "Adding game files...";

            batchProgressBar.Value = 0;
            

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Multiselect = true,

                InitialDirectory = @"G:\",
                Title = "Browse .bin Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bin files (*.bin)|*.bin",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    
                    string directoryPath = Path.GetFullPath(file);
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    long size  = new System.IO.FileInfo(directoryPath).Length;


                    GameFile gf = new GameFile(fileName, directoryPath, size, "");


                    if (checkExist(gf))
                    {
                        alreadyInList.Add(gf.FileName);
                        //batchProgressBar.Value += 1;
                    }
                    else 
                    {
                        masterList.Add(gf);
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.WorkerReportsProgress = true;
                        bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                        bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                        bw.RunWorkerAsync(gf);
                        bw.Dispose();
                    }
                    

                    
                }

                itemsCount.Text = masterList.Count.ToString();
                batchProgressBar.Value = alreadyInList.Count;
                batchProgressBar.Maximum = masterList.Count;

                var message = string.Join(Environment.NewLine, alreadyInList);
                if (alreadyInList.Count != 0)
                {
                    MessageBox.Show("The following items were already imported: " + "\n" + message);
                }

                this.objectListView1.SetObjects(masterList);

                
                //textBox1.Text = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("Please select .bin files to proceed");
                openBtn.Enabled = true;
            }

        }

        // The cryptographic service provider.
        private SHA256 Sha256 = SHA256.Create();
        private MD5 Md5 = MD5.Create();

        // Compute the file's Sha256 hash.
        private byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }


        // Compute the file's MD5 hash.
        private byte[] GetHashMD5(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Md5.ComputeHash(stream);
            }
        }


        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }


        private void runBtn_click(object sender, EventArgs e)
        {

            if (masterList.Count == 0)
            {
                MessageBox.Show("Please select dabases from settings, then click 'Open' to add .bin files to begin");
            }
            else 
            {
                batchProgressBar.Value = 0;
                batchProgressBar.Maximum = masterList.Count;
                itemsCount.Text = masterList.Count.ToString();

                //var gamesList = new List<GameFile>();

                runBtn.Enabled = false;

                status.Text = "calculating hashes...";
                
                //New Code testing seperate background workers
                foreach (GameFile gf in masterList)
                {
                        if(checkExist(gf) ==true) 
                        {
                            status.Text = "Some iso hashes were already calculated...";
                            batchProgressBar.Value += 1;
                        }
                        else
                        {
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.WorkerReportsProgress = true;
                            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                            bw.RunWorkerAsync(gf);
                            bw.Dispose();
                        } 

                    
                }
                if (batchProgressBar.Value == batchProgressBar.Maximum)
                {
                    runBtn.Enabled = true;
                    openBtn.Enabled = true;
                }
                batchProgressBar.Maximum = masterList.Count;

            }

            
            //Old code looping in single backgroundworker
            //backgroundWorker1.RunWorkerAsync(GamesList);


        }



        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            GameFile gf = e.Argument as GameFile;

            byte[] buffer;
            int bytesRead;
            long size;
            long totalBytesRead = 0;

                using (Stream file = File.OpenRead(gf.FilePath))
            {
                size = file.Length;

                using (HashAlgorithm hasher = MD5.Create())
                {
                    do
                    {

                        buffer = new byte[4096];

                        bytesRead = file.Read(buffer, 0, buffer.Length);

                        totalBytesRead += bytesRead;

                        hasher.TransformBlock(buffer, 0, bytesRead, null, 0);

                        gf.Status = ((int)((double)totalBytesRead / size * 100));

                        
                    }
                    while (bytesRead != 0);

                    hasher.TransformFinalBlock(buffer, 0, 0);
                    gf.MD5Hash = MakeHashString(hasher.Hash);
                    e.Result = gf;

                }
                }
            //}
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //MessageBox.Show(e.ProgressPercentage.ToString());
            //= e.
            //BarRenderer br = new BarRenderer(0,100);
            //this.olvColumn1.Renderer = br;

            GameFile gf = (GameFile)e.UserState;

            objectListView1.UpdateObject(gf);


        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GameFile gf = e.Result as GameFile;
            //updateListMD5(gf);

            objectListView1.UpdateObject(gf);

            compare();

            //objectListView1.RedrawItems(0,toolStripProgressBar1.Maximum-1, false);
            if (gf.Status==100)
            {
                batchProgressBar.Value += 1;
            }
            
            if(batchProgressBar.Value == batchProgressBar.Maximum)
            {
                runBtn.Enabled = true;
                openBtn.Enabled = true;
                status.Text = "All hashes calculated...";
            }
                
        }

        private static string MakeHashString(byte[] hashBytes)
        {
            StringBuilder hash = new StringBuilder(32);


            foreach (byte b in hashBytes)
                hash.Append(b.ToString("x2"));

            return hash.ToString();
        }

        private void clearBtn_click(object sender, EventArgs e)
        {
            objectListView1.Items.Clear();
            batchProgressBar.Value = 0;
            itemsCount.Text = "0";
            md5TextBox.Text = "";
            fileNameTextBox.Text = "";
            status.Text = "Ready...";
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var settingsForm = new Form2();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.Systems))
            {
                Properties.Settings.Default.Systems.Split(',')
                    .ToList()
                    .ForEach(item =>
                    {
                        var index = settingsForm.checkedListBox1.Items.IndexOf(item);
                        settingsForm.checkedListBox1.SetItemChecked(index, true);
                    });
            }


            settingsForm.ShowDialog();

        }

        private bool checkExist(GameFile file)
        {
            bool exists = false;

            foreach(GameFile gf in masterList)
            {
                if (gf.FileName == file.FileName)
                    exists = true;
            }

            return exists;
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var settingsForm = new Form2();

            Properties.Settings.Default.Reload();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.Systems))
            {
                Properties.Settings.Default.Systems.Split(',')
                    .ToList()
                    .ForEach(item =>
                    {
                        var index = settingsForm.checkedListBox1.Items.IndexOf(item);
                        settingsForm.checkedListBox1.SetItemChecked(index, true);
                    });
            }
            else if(string.IsNullOrEmpty(Properties.Settings.Default.Systems))
            {
                MessageBox.Show("Please select game databases to use from settings");
            }

            //settingsForm.Show();

        }

        private void redumpBtn_click(object sender, EventArgs e)
        {
            createRedumpDB();
        }


        private void setSelectedFile()
        {
            GameFile gf = new GameFile();

            gf.FileName = objectListView1.SelectedItem.SubItems[0].Text;
            gf.MD5Hash = objectListView1.SelectedItem.SubItems[3].Text;

            fileNameTextBox.Text = gf.FileName;
            md5TextBox.Text = gf.MD5Hash;
        }


        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form redumpTable = new redumpTable();
            redumpTable.Show();
        }


        private void createRedumpDB()
        {
            //Data Set for each System Library
            DataSet ds = new DataSet();
            //Gets program directory 
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            //Gets only .dat files from directory
            FileInfo[] Files = d.GetFiles("*.dat");

            //Runs work for each .dat file collected

            if (Files.Length > 0)
            {
                foreach (FileInfo file in Files)
                {
                    BackgroundWorker xDocWorker = new BackgroundWorker();
                    xDocWorker.WorkerReportsProgress = true;
                    xDocWorker.DoWork += new DoWorkEventHandler(xDocWorker_DoWork);
                    xDocWorker.ProgressChanged += new ProgressChangedEventHandler(xDocWorker_ProgressChanged);
                    xDocWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(xDocWorker_RunWorkerCompleted);
                    xDocWorker.RunWorkerAsync(file);
                    xDocWorker.Dispose();

                }
            }
            else
            {
                MessageBox.Show("Please select databases from settings to begin.");
            }
        }

        private void xDocWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Retrieves file passed to background worker
            FileInfo file = e.Argument as FileInfo;

            //New Dataset for all tables
            DataSet dataSet = new DataSet();


            string fileName = Path.GetFileNameWithoutExtension(file.FullName); //Extracts only the file name without .dat extension
            fileName = Regex.Replace(fileName, " ", ""); //Removes blank spaces
            string tableName = "table_" + fileName; //Names table after fileName


            //Company tag for all items in file
            string company = "";

            //Creating Table & Columns
            DataTable table1 = new DataTable(tableName);
            table1.Columns.Add("GameName", typeof(String));
            table1.Columns.Add("Company", typeof(String));
            table1.Columns.Add("MD5", typeof(String));

            //Adds current table to dataSet
            dataSet.Tables.Add(table1);

            //Loads .dat file for parsing xml data
            XDocument doc = XDocument.Load(file.FullName);

            //List of GameFile objects for storing redump game info
            List<GameFile> gamesList = new List<GameFile>();

            //Extracts the System Manufactuer from .dat file
            foreach (XElement c in doc.Descendants("name"))
            {
                company = (string)c.Value;
            }

            //Extracts all <game> items from .dat file
            foreach (XElement game in doc.Descendants("game"))
            {
                GameFile gf = new GameFile();

                gf.Company = company;


                //Extracts rom info from game element, inside .dat file
                foreach (XElement rom in game.Elements("rom"))
                {
                    {
                        //Adds info from redump.org to new GameFile
                        gf.FileName = (string)rom.Attribute("name");
                        gf.MD5Hash = (string)rom.Attribute("md5");

                        //Adds GameFile info to table row  
                        DataRow row = table1.NewRow();
                        row["GameName"] = gf.FileName;
                        row["Company"] = gf.Company;
                        row["MD5"] = gf.MD5Hash;

                        //Adds new row to current table
                        table1.Rows.Add(row);
                    }

                    (sender as BackgroundWorker).ReportProgress(1);
                    //gamesList.Add(gf);

                }

            }
            
            e.Result = dataSet; //Sends result as List<> of GameFile

        }

        private void xDocWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void xDocWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataSet ds = e.Result as DataSet;
            dataSet1 = ds;
            status.Text = "Redump.org Databases have been set...";
            //objectListView1.UpdateObjects(gf);
        }

        private void compare()
        {
            //Starts loop for all items in ObjectListView
            foreach (GameFile file in masterList)
            {
                file.Match = "No"; //Automatically sets Match to No instead of applying it multiple times in loop

                //Begins looping each table in dataSet1
                foreach (DataTable t in dataSet1.Tables)
                {
                    //Begins looping for each row in the current table
                    foreach (DataRow row in t.Rows)
                    {
                        //Creates a second GameFile from Redump.org data
                        GameFile gf = new GameFile();
                        gf.FileName = row["GameName"].ToString();
                        gf.MD5Hash = row["MD5"].ToString();

                        //if comparison to check for a matching md5 hash
                        if (file.MD5Hash == gf.MD5Hash)
                        {
                            MessageBox.Show("Match!");
                            file.Match = "Yes";
                            objectListView1.RefreshObject(file);
                        }
                    }

                    int matches = 0;

                    foreach (GameFile gf in masterList)
                    {
                        if (gf.Match == "Yes")
                            matches += 1;
                    }

                    status.Text = "Done parsing table. " + matches + "/" + masterList.Count + " files were verified with Redump.org";

                    objectListView1.BuildList();


                }
            }
        }

        private void compareBtn_Click(object sender, EventArgs e)
        {
            compare();
        }

        private void objectListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            if (e.IsSelected)
                setSelectedFile();
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            if(md5TextBox.Text != "")
            Clipboard.SetText(md5TextBox.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createRedumpDB();
        }
    }


}

