using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MD5_Pro_Gramer
{
    class GameFile
    {
        public GameFile()
        {

        }

        public GameFile(string fileName, string filePath, long sizeInBytes, string md5hash)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
            this.SizeInBytes = sizeInBytes;
            this.MD5Hash = md5hash;
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string fileName;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        public string FileDesc
        {
            get { return FileDesc; }
            set { FileDesc = value; }
        }
        private string fileDesc;

        public string MD5Hash
        { 
            get { return md5Hash; }
            set { md5Hash = value; }
        }
        private string md5Hash;

        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private string company;
        
        public long SizeInBytes
        {
            get { return sizeInBytes; }
            set { sizeInBytes = value; }
        }
        private long sizeInBytes;

        public long Status
        {
            get { return status; }
            set { status = value; }
        }
        private long status;

        public string Match
        {
            get { return match; }
            set { match = value; }
        }
        private string match;

        public double GetSizeInMB()
        {
            return ((double)this.SizeInBytes) / (1024.0 * 1024.0);
        }

       
    }
}
