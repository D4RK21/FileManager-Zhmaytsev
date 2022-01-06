using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FileManager
{
    public class MyDirectory : IDisposable
    {
        private readonly DirectoryInfo _directory;
        private readonly string _fullPath;

        public MyDirectory(string path)
        {
            _directory = new DirectoryInfo(path);
            _fullPath = _directory.FullName;
        }

        public string GetFullPath()
        {
            return _fullPath;
        }

        public string GetContent(string[] flags)
        {
            string resultStr = string.Empty;
            long allFilesSize = 0;

            DirectoryInfo[] directories = _directory.GetDirectories();
            FileInfo[] files = _directory.GetFiles();

            if (!flags.Contains("h"))
            {
                directories = directories.Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
                files = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
            }

            if (flags.Contains("sn"))
            {
                directories = directories.OrderBy(d => d.Name).ToArray();
                files = files.OrderBy(s => s.Name).ToArray();
            }

            if (flags.Contains("sl"))
            {
                files = files.OrderBy(s => s.Length).ToArray();
            }

            if (flags.Contains("sd"))
            {
                directories = directories.OrderByDescending(d => d.LastWriteTime).ToArray();
                files = files.OrderByDescending(s => s.LastWriteTime).ToArray();
            }

            if (flags.Contains("t"))
            {
                Helper.GetContentInTreeForm(directories, files, out resultStr, out allFilesSize);
            }
            else
            {
                Helper.GetContentInDefaultForm(directories, files, out resultStr, out allFilesSize);
            }

            resultStr += $"{files.Length} files";
            resultStr += allFilesSize != 0 ? $" — {allFilesSize} bytes\n" : "\n";

            resultStr += $"{directories.Length} folders";

            return resultStr;
        }

        public string MakeDirectory(string name)
        {
            var pathToNewDir = Path.Combine(_fullPath, name);

            if (!Directory.Exists(pathToNewDir))
            {
                Directory.CreateDirectory(pathToNewDir);
            }

            return pathToNewDir;
        }

        public bool MakeFile(string name)
        {
            var pathToNewFile = Path.Combine(_fullPath, name);

            if (File.Exists(pathToNewFile))
            {
                return false;
            }

            using var file = File.Create(pathToNewFile);
            
            return true;
        }

        public bool Delete(string path)
        {
            path = Path.Combine(_fullPath, path);

            if (File.Exists(path))
            {
                File.Delete(path);
                
                return true;
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                
                return true;
            }

            return false;
        }

        public bool Rename(string oldName, string newName)
        {
            oldName = Path.Combine(_fullPath, oldName);
            newName = Path.Combine(_fullPath, newName);

            if (File.Exists(oldName))
            {
                File.Move(oldName, newName);
                
                return true;
            }

            if (Directory.Exists(oldName))
            {
                Directory.Move(oldName, newName);
                
                return true;
            }

            return false;
        }

        public void Dispose()
        {
        }
    }
}