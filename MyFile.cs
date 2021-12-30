using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FileManager
{
    public class MyFile : IDisposable
    {
        private readonly FileInfo _file;
        private readonly string _name;
        private readonly string _fullPath;
        private readonly long _size;
        private readonly DateTime _dateCreated;
        private readonly DateTime _dateUpdated;

        public MyFile(string path)
        {
            _file = new FileInfo(path);
            _name = _file.Name;
            _fullPath = _file.FullName;
            _size = _file.Length;
            _dateCreated = _file.CreationTime;
            _dateUpdated = _file.LastWriteTime;
        }

        public string View(int maxCount)
        {
            char[] buffer = new char[maxCount];

            using var reader = new StreamReader(_fullPath, Encoding.UTF8);
            
            reader.Read(buffer, 0, maxCount);

            var resultStr = new string(buffer);
            return resultStr;
        }

        public bool IsSubstrExistInFile(string substr)
        {
            string[] readText = File.ReadAllLines(_fullPath, Encoding.UTF8);

            return readText.Any(s => s.Contains(substr));
        }

        public void Dispose()
        {
            Console.WriteLine("File object has been disposed!");    
        }
    }
}