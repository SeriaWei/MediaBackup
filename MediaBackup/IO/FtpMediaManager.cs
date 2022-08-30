using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediaBackup.IO
{
    internal class FtpMediaManager : IMediaManager
    {
        public FtpMediaManager(string ftpPath, string userName, string password)
        {
            BasePath = ftpPath;
            Username = userName;
            Password = password;
        }
        public string BasePath { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public DateTime GetCreationTime(string filePath)
        {
            var date = DateParser.Parse(filePath);
            if (date.HasValue) return date.Value;

            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.CreationTime;
        }
        private FtpWebRequest CreateFtpRequest(string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(BasePath);
            request.Method = method;
            if (Username != null)
            {
                request.Credentials = new NetworkCredential(Username, Password);
            }
            return request;
        }
        public string[] GetFiles()
        {
            FtpWebRequest request  = CreateFtpRequest(WebRequestMethods.Ftp.ListDirectory);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();
            reader.Close();
            response.Close();

            return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public Stream GetFileStream(string filePath)
        {
            FtpWebRequest request = CreateFtpRequest(WebRequestMethods.Ftp.DownloadFile);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        public string GetMediaType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".gif":
                case ".bmp":
                case ".png":
                case ".jepg":
                case ".jpg": return "Image";
                case ".mov":
                case ".wmv":
                case ".avi":
                case ".mkv":
                case ".flv":
                case ".rmvb":
                case ".mp4": return "Video";
            }
            return String.Empty;
        }

        public bool SaveFile(Stream stream, string targetFolder, string fileName)
        {
            string saveFoler = BasePath.TrimEnd('/') + "/" + targetFolder;

            throw new NotImplementedException();
        }
    }
}
