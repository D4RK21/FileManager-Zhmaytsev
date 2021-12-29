﻿using System;
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

        // public string GetContent()
        // {
        //     string resultStr = "";
        //
        //     DirectoryInfo[] directories = _directory.GetDirectories();
        //     FileInfo[] files = _directory.GetFiles();
        //     
        //     directories = directories.Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
        //     files = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
        //     
        //     foreach (var directory in directories)
        //     {
        //         resultStr += $"{directory.CreationTime}\t<DIR>\t{directory.Name}\n";
        //     }
        //
        //     foreach (var file in files)
        //     {
        //         resultStr += $"{file.CreationTime}\t{file.Length}\t{file.Name}\n";
        //     }
        //     
        //     resultStr += $"\t{files.Length} files\n";
        //     resultStr += $"\t{directories.Length} folders";
        //     
        //     return resultStr;
        // }

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
        
        public void Dispose()
        {
            Console.WriteLine("Directory object has been disposed!");    
        }
    }
}