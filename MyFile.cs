using System;
using System.IO;

namespace FileManager
{
    public class MyFile : IDisposable
    {
        private string _name;
        private long _size;
        private DateTime _dateCreated;
        private DateTime _dateUpdated;

        public MyFile(FileInfo fileInfo)
        {
            _name = fileInfo.Name;
            _size = fileInfo.Length;
            _dateCreated = fileInfo.CreationTime;
            _dateUpdated = fileInfo.LastWriteTime;
        }
        
        public void Dispose()
        {
            Console.WriteLine("File object has been disposed!");    
        }
    }
}