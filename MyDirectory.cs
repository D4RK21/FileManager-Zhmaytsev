using System;
using System.Drawing;
using System.IO;
using System.Linq;

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

        public string GetContent(string[] flags)
        {
            string resultStr = "";
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

            if (flags.Contains("ss"))
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
                foreach (var directory in directories)
                {
                    resultStr += $"<DIR> {directory.Name}\n\t— Creation time: {directory.CreationTime}\n\t— Last update: {directory.LastWriteTime}\n";
                }

                resultStr += "\n";
                
                foreach (var file in files)
                {
                    resultStr +=
                        $"{file.Name}\n\t— Size: {file.Length}\n\t— Extension: {file.Extension}\n\t— Creation time: {file.CreationTime}\n\t— Last update: {file.LastWriteTime}\n";
                    
                    if (file.Extension is ".jpg" or ".png" or ".bmp" or ".gif" or ".tif")
                    {
                        var bmp = new Bitmap(file.FullName);
                        resultStr += $"\t— Resolution: {bmp.Width} x {bmp.Height}\n";
                    }
                        
                    allFilesSize += file.Length;
                }
            }
            else
            {
                foreach (var directory in directories)
                {
                    resultStr += $"{directory.LastWriteTime}\t<DIR>\t{directory.Name}\n";
                }

                foreach (var file in files)
                {
                    resultStr += $"{file.LastWriteTime}\t{file.Length}\t{file.Name}\n";
                    allFilesSize += file.Length;
                }
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
            
            File.Create(pathToNewFile);
            return true;
        }

        public bool Delete(string path)
        {
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
            Console.WriteLine("Directory object has been disposed!");    
        }
    }
}