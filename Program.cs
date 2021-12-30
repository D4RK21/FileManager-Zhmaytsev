using System;
using System.IO;
using System.Drawing;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = new MyDirectory(@"../../../");

            while (true)
            {
                var currentPath = currentDirectory.GetFullPath();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{(currentPath[^1] == '\\' ? currentPath[..^1] : currentPath)} > ");
                Console.ResetColor();

                string inputText = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(inputText)) continue;
                string[] inputTextArr = inputText.Split(' ');
                
                Helper.ParseInputString(inputText, out var inputCommand,out var inputArguments, out var inputFlags);

                if (inputCommand is "view" or "find" or "cd")
                {
                    inputArguments[0] = Helper.CheckPath(inputArguments[0], currentDirectory.GetFullPath());
                    if (inputArguments[0] == "")
                    {
                        Console.WriteLine("Invalid path!");
                        continue;
                    }
                }

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
                    default:
                        Console.WriteLine("Command not found!");
                        break;
                }
            }
        }
    }
}
