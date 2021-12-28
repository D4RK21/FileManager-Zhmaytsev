using System;
using System.IO;

namespace FileManager
{
    public class MyDirectory : IDisposable
    {
        private string _name;
        private string _fullPath;
        private DateTime _dateCreated;
        private DateTime _dateLastAccess;
        private DateTime _dateLastWriteTime;

        public MyDirectory(DirectoryInfo directoryInfo)
        {
            _name = directoryInfo.Name;
            _fullPath = directoryInfo.FullName;
            _dateCreated = directoryInfo.CreationTime;
            _dateLastAccess = directoryInfo.LastAccessTime;
            _dateLastWriteTime = directoryInfo.LastWriteTime;
        }
        
        public void Dispose()
        {
            Console.WriteLine("Directory object has been disposed!");    
        }
    }
}