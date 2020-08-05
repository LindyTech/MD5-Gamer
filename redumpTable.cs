using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MD5_Pro_Gramer
{
    public partial class redumpTable : Form
    {
        public redumpTable()
        {
            InitializeComponent();
        }

        private void redumpTest_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            try
            {
                DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.dat"); //Getting dat files

                foreach (FileInfo file in Files)
                {
                    //richTextBox1.AppendText(File.ReadAllText(file.FullName));
                    int totalLines = File.ReadAllLines(file.FullName).Length;
                    progressBar1.Maximum = totalLines + objectListView1.Items.Count +1;
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = true;
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                    bw.RunWorkerAsync(file);
                    bw.Dispose();

                }
                //MessageBox.Show("Done reading ALL files");
            }
            catch (SecurityException sec)
            {
                MessageBox.Show(sec.Message);
            }



        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            FileInfo file = e.Argument as FileInfo;
            int totalLines = File.ReadAllLines(file.FullName).Length;
           

            List<GameFile> gamesList = new List<GameFile>();
            string system =  "";

            foreach (string line in File.ReadAllLines(file.FullName))
            {

                //int currentLine = line.Count();
                //MessageBox.Show(((currentLine * 100) / totalLines  ).ToString());
                (sender as BackgroundWorker).ReportProgress(1);

                GameFile gf = new GameFile();

                Match systemCheck = Regex.Match(line, "<name>(.*?)<\\/name>");
                if (systemCheck.Success)
                {
                    system = systemCheck.Groups[1].Value;
                    
                }
                gf.Company = system;


                Match elements = Regex.Match(line, "<[^<>]+>");
                if (elements.Success)
                {
                    string game = elements.Value;
                    if (game.StartsWith("<rom name="))
                    {
                        Match gameName = Regex.Match(game, "<rom name=\"(.*?)\"");
                        gf.FileName = gameName.Groups[1].Value;
                        //MessageBox.Show(gf.FileName);
                        Match md5 = Regex.Match(game, "md5=\"(.*?)\"");
                        gf.MD5Hash = md5.Groups[1].Value;
                        //MessageBox.Show(gf.MD5Hash);

                        //gf.FileName = game;
                        if (gameName.Success && md5.Success)
                        {
                            gamesList.Add(gf);
                        }

                    }
                    e.Result = gamesList;
                }
                
            }
            //objectListView1.AddObject(gamesList);

        }



        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(progressBar1.Value != progressBar1.Maximum)
            {
                progressBar1.Value += e.ProgressPercentage;
            }
            else
            {
                progressBar1.Maximum += 1;
                progressBar1.Value += 1 ;
            }
            
                

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<GameFile> gamesList = e.Result as List<GameFile>;

            objectListView1.UpdateObjects(gamesList);
        }

        private void createDatabase()
        {


        }

        private void dataTableTest()
        {
             //Data Set for each System Library

            //Gets program directory 
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            //Gets only .dat files from directory
            FileInfo[] Files = d.GetFiles("*.dat");
            
            //Runs work for each .dat file collected
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

        private void button1_Click(object sender, EventArgs e)
        {
            dataTableTest();
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
            e.Result = gamesList; //Sends result as List<> of GameFile

        }

        private void xDocWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void xDocWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<GameFile> gf = e.Result as List<GameFile>;
            MessageBox.Show("done");
            //objectListView1.UpdateObjects(gf);
        }
    }
}


