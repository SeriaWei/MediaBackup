using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBackup.IO
{
    internal interface IMediaManager
    {
        string[] GetFiles();
        Stream GetFileStream(string filePath);
        DateTime GetCreationTime(string filePath);
        string GetMediaType(string filePath);
        bool SaveFile(Stream stream, string targetFolder, string fileName);
    }
}
