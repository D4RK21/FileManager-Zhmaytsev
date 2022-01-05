﻿using System;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Type \"help\" to get list of commands");
            Console.ResetColor();

            var currentDirectory = new MyDirectory(@"../../../");

            while (true)
            {
                var currentPath = currentDirectory.GetFullPath();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{(currentPath[^1] == '\\' ? currentPath[..^1] : currentPath)} > ");
                Console.ResetColor();

                string inputText = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(inputText)) continue;

                Helper.ParseInputString(inputText, out var inputCommand, out var inputArguments, out var inputFlags);

                if (inputCommand is "view" or "find" or "cd" or "del" or "ren")
                {
                    inputArguments[0] = Helper.CheckPath(inputArguments[0], currentDirectory.GetFullPath());
                    if (inputArguments[0] == string.Empty)
                    {
                        Console.WriteLine("Invalid path!");
                        continue;
                    }
                }

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
            }
        }
    }
}