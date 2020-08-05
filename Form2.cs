using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mime;
using System.IO.Compression;
using System.ComponentModel;
using System.Security.Permissions;
using System.IO;
using System.Diagnostics.Tracing;
using System.Security;

namespace MD5_Pro_Gramer
{

    public partial class Form2 : Form
    {
        public bool shown = true;
        public Form2()
        {
            InitializeComponent();
            System.Net.ServicePointManager.DefaultConnectionLimit = 10;
        }
        private string[] generateScrapeURL()
        {
            string[] scrapeURL = new string[checkedListBox1.CheckedItems.Count];

            List<string> scrapeList = new List<string>();

            string systemABV = "";



            if (checkedListBox1.CheckedItems.Count == 0)
                MessageBox.Show("Please select at least one system");

            if (checkedListBox1.CheckedItems.Count >= 1)
                for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                {
                    if (checkedListBox1.CheckedItems[x].ToString() == "Gamecube")
                        systemABV = "gc";
                    else if (checkedListBox1.CheckedItems[x].ToString() == "Dreamcast")
                        systemABV = "dc";
                    else
                        systemABV = checkedListBox1.CheckedItems[x].ToString();


                    scrapeList.Add("http://redump.org/datfile/" + systemABV + "/");
                    //MessageBox.Show(scrapeList[x]);
                    scrapeURL = scrapeList.ToArray();

                }


            return scrapeURL;
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            
            string[] url_List = new string[checkedListBox1.CheckedItems.Count];

            if(url_List.Length > 0)
            {
                url_List = generateScrapeURL();

                Properties.Settings.Default.SystemsURLS = string.Join(",", url_List);
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Please select at least one database.");
            }


            //backgroundWorker1.RunWorkerAsync(url_List);

            //Delete old .dat files
            try
            {
                DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.dat"); //Getting dat files

                foreach (FileInfo file in Files)
                {
                    File.Delete(file.ToString());
                }
            }
            catch (SecurityException sec)
            {
                MessageBox.Show(sec.Message);
            }

            for (int x = 0; x < url_List.Length; x++)
            {
                //settingsProgressBar.Maximum = url_List.Length;

                BackgroundWorker dbDownloader = new BackgroundWorker();
                dbDownloader.WorkerReportsProgress = true;
                dbDownloader.DoWork += new DoWorkEventHandler(dbDownloader_DoWork);
                //dbDownloader.ProgressChanged += new ProgressChangedEventHandler(dbDownloader_ProgressChanged);
                //dbDownloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dbDownloader_RunWorkerCompleted);
                dbDownloader.RunWorkerAsync(url_List[x]);
                dbDownloader.Dispose();

                //System.Threading.Thread.Sleep(7000);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

            var indices = this.checkedListBox1.CheckedItems.Cast<string>().ToArray();


            Properties.Settings.Default.Systems = string.Join(",", indices);
            Properties.Settings.Default.Save();

        }

        private void downloadDB_btn_Click(object sender, EventArgs e)
        {
            
       
        }

        public  async void DownloadData(string dataURL)
        {
            string filename = null;
            string filedownload = null;
            await Task.Run(() =>
            {

                using (WebClient client = new WebClient())
                {

                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    try
                    {

                        client.OpenRead(dataURL);

                        string header_contentDisposition = client.ResponseHeaders["content-disposition"];
                        filename = new ContentDisposition(header_contentDisposition).FileName;



                        filedownload = AppDomain.CurrentDomain.BaseDirectory + filename;
                        var url = new Uri(dataURL);

                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
                        client.QueryString.Add("zipfile",filedownload);
                        client.DownloadFileAsync(url, filedownload);
                        client.Dispose();
                        Task.Delay(7000);



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }

                }


            });
        }


        private static void webClient_DownloadFileCompleted(object s, AsyncCompletedEventArgs e)
        {
            string zipFile = ((System.Net.WebClient)(s)).QueryString["zipfile"];
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + zipFile;
            string datPath = fullPath.Substring(0,fullPath.Length-4);
            datPath = datPath + ".dat";

            //MessageBox.Show(File.Exists(datPath).ToString());
            //System.Diagnostics.Process.Start(datPath);

            ZipFile.ExtractToDirectory(zipFile, AppDomain.CurrentDomain.BaseDirectory);
            MessageBox.Show(datPath + " file download complete!");

        }
        public void webClient_DownloadProgressChanged(object s, System.Net.DownloadProgressChangedEventArgs e)
        {
            var RecevedBytes = e.BytesReceived;
            var TotalBytes = e.TotalBytesToReceive;

            //settingsProgressBar.Maximum = TotalBytes;
            //settingsProgressBar.Value = e.ProgressPercentage;

         }



        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void dbDownloader_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument as string;

            DownloadData(url);
        }
    }
}
