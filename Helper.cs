using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileManager
{
    public static class Helper
    {
        public static void ParseInputString(string input, out string command, out string[] arguments, out string[] flags)
        {
            var regex = new Regex(@"("".*?""|[^ ""]+)+");
            var matchesArray = regex.Matches(input).ToArray();

            int argumentsCounter = 0;
            int flagsCounter = 0;

            for (int i = 1; i < matchesArray.Length; i++)
            {
                if (matchesArray[i].Value[0] != '-')
                {
                    argumentsCounter++;
                }
                else
                {
                    flagsCounter++;
                }
            }

            command = matchesArray[0].Value.ToLower();
            arguments = new string[argumentsCounter];
            flags = new string[flagsCounter];

            argumentsCounter = 0;
            flagsCounter = 0;

            for (int i = 1; i < matchesArray.Length; i++)
            {
                if (matchesArray[i].Value[0] != '-')
                {
                    var argumentText = matchesArray[i].Value;

                    if (argumentText[0] == '"' && argumentText[^1] == '"')
                    {
                        argumentText = argumentText.Substring(1, argumentText.Length - 2);
                    }

                    arguments[argumentsCounter] = argumentText;
                    argumentsCounter++;
                }
                else
                {
                    flags[flagsCounter] = matchesArray[i].Value.Substring(1).ToLower();
                    flagsCounter++;
                }
            }
        }

        public static string CheckPath(string path, string directory)
        {
            path = Path.Combine(directory, path);

            if (Directory.Exists(path) || File.Exists(path))
            {
                return path;
            }

            return string.Empty;
        }

        public static void DisplayHelp()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tAvailable commands:");
            Console.ResetColor();

            Console.WriteLine("—dir - Outputting directory content");
            Console.WriteLine("    flag -h for showing hidden files");
            Console.WriteLine("    flag -sn for sorting by name");
            Console.WriteLine("    flag -sl for sorting by size");
            Console.WriteLine("    flag -sd for sorting by time");
            Console.WriteLine("    flag -t for outputting in the form of tree");
            Console.WriteLine("—view - View first 200 symbols of file content");
            Console.WriteLine("—find - Is substring exist in file");
            Console.WriteLine("—mkdir - Make directory");
            Console.WriteLine("—mkfile - Make file");
            Console.WriteLine("—del - Delete directory or file");
            Console.WriteLine("—ren - Rename directory or file");
        }

        public static void GetContentInTreeForm(DirectoryInfo[] directories, FileInfo[] files, out string resultStr, out long allFilesSize)
        {
            resultStr = string.Empty;
            allFilesSize = 0;
            
            foreach (var directory in directories)
            {
                resultStr +=
                    $"<DIR> {directory.Name}\n\t— Creation time: {directory.CreationTime}\n\t— Last update: {directory.LastWriteTime}\n";
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
        
        public static void GetContentInDefaultForm(DirectoryInfo[] directories, FileInfo[] files, out string resultStr, out long allFilesSize)
        {
            resultStr = string.Empty;
            allFilesSize = 0;
            
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

        public static MyDirectory RunCommand(MyDirectory currentDirectory, string inputCommand, string[] inputArguments, string[] inputFlags)
        {
            bool response;
            
            switch (inputCommand)
            {
                case "dir":
                    Console.WriteLine(currentDirectory.GetContent(inputFlags));
                    break;
                case "view":
                    using (var file = new MyFile(inputArguments[0]))
                    {
                        Console.WriteLine(file.View(200));
                    }
                    break;
                case "find":
                    using (var file = new MyFile(inputArguments[0]))
                    {
                        Console.WriteLine(file.IsSubstrExistInFile(inputArguments[1]));
                    }
                    break;
                case "cd":
                    currentDirectory.Dispose();
                    currentDirectory = new MyDirectory(inputArguments[0]);
                    break;
                case "mkdir":
                    var pathToNewDir = currentDirectory.MakeDirectory(inputArguments[0]);
                    currentDirectory.Dispose();
                    currentDirectory = new MyDirectory(pathToNewDir);
                    break;
                case "mkfile":
                    response = currentDirectory.MakeFile(inputArguments[0]);
                    Console.WriteLine(response
                        ? $"File {inputArguments[0]} successfully created!"
                        : "File already exist!");
                    break;
                case "del":
                    response = currentDirectory.Delete(inputArguments[0]);
                    Console.WriteLine(response ? "Successfully deleted!" : "Object does not exist!");
                    break;
                case "ren":
                    response = currentDirectory.Rename(inputArguments[0], inputArguments[1]);
                    Console.WriteLine(response ? "Successfully renamed!" : "Object does not exist!");
                    break;
                case "help":
                    Helper.DisplayHelp();
                    break;
                default:
                    Console.WriteLine("Command not found!");
                    break;
            }
            
            return currentDirectory;
        }
    }
}