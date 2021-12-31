using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FileManager
{
    public class MyFile : IDisposable
    {
        private readonly FileInfo _file;
        private readonly string _fullPath;

        public MyFile(string path)
        {
            _file = new FileInfo(path);
            _fullPath = _file.FullName;
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
        }
    }
}