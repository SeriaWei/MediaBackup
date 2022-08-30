using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBackup.IO
{
    internal class LocalMediaManager : IMediaManager
    {
        public LocalMediaManager(string folerPath)
        {
            FolderPath = folerPath;
        }
        public string FolderPath { get; private set; }
        public DateTime GetCreationTime(string filePath)
        {
            var date = DateParser.Parse(filePath);
            if (date.HasValue) return date.Value;

            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.CreationTime;
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories);
        }

        public Stream GetFileStream(string filePath)
        {
            return File.OpenRead(filePath);
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
            var saveFoler = Path.Combine(FolderPath, targetFolder);
            if (!Directory.Exists(saveFoler))
            {
                Directory.CreateDirectory(saveFoler);
            }
            string targetFile= Path.Combine(saveFoler, fileName);
            if (File.Exists(targetFile)) return false;

            stream.Position = 0;
            using (FileStream sf = File.Create(targetFile))
            {
                stream.CopyTo(sf);
            }
            return true;
        }
    }
}
