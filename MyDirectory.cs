using System;
using System.IO;

namespace FileManager
{
    public class MyDirectory : IDisposable
    {
        private readonly DirectoryInfo _directory;
        private readonly string _name;
        private readonly string _fullPath;
        private readonly DateTime _dateCreated;
        private readonly DateTime _dateLastAccess;
        private readonly DateTime _dateLastWriteTime;

        public MyDirectory(string path)
        {
            _directory = new DirectoryInfo(path);
            _name = _directory.Name;
            _fullPath = _directory.FullName;
            _dateCreated = _directory.CreationTime;
            _dateLastAccess = _directory.LastAccessTime;
            _dateLastWriteTime = _directory.LastWriteTime;
        }

        public string GetFullPath()
        {
            return _fullPath;
        }

        public string GetContent()
        {
            string resultStr = "";
            long allFilesSize = 0;

            DirectoryInfo[] directories = _directory.GetDirectories();
            FileInfo[] files = _directory.GetFiles();

            foreach (var directory in directories)
            {
                resultStr += $"{directory.CreationTime}\t<DIR>\t{directory.Name}\n";
            }

            foreach (var file in files)
            {
                resultStr += $"{file.CreationTime}\t{file.Length}\t{file.Name}\n";
                allFilesSize += file.Length;
            }

            resultStr += $"\t\t{files.Length} files";
            resultStr += allFilesSize != 0 ? $" — {allFilesSize}\n" : "\n";

            resultStr += $"\t\t{directories.Length} folders";
            
            return resultStr;
        }
        
        public void Dispose()
        {
            Console.WriteLine("Directory object has been disposed!");    
        }
    }
}