using System;
using System.IO;
using System.Drawing;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using var currentDirectory = new MyDirectory(@"../../../");

            while (true)
            {
                var currentPath = currentDirectory.GetFullPath();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{currentPath.Substring(0, currentPath.Length - 1)} > ");
                Console.ResetColor();

                string inputText = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(inputText)) continue;
                string[] inputTextArr = inputText.Split(' ');
                
                Helper.ParseInputString(inputText, out var inputCommand,out var inputArguments, out var inputFlags);

                if (inputCommand is "view" or "find")
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
                    
                    default:
                        Console.WriteLine("Command not found!");
                        break;
                }
            }
        }
    }
}
