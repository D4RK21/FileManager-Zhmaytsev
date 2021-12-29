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
                Console.Write($"\n{currentPath.Substring(0, currentPath.Length - 1)} > ");
                Console.ResetColor();

                string inputText = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(inputText)) continue;
                string[] inputTextArr = inputText.Split(' ');
                
                Helper.ParseInputString(inputText, out var inputCommand,out var inputArguments, out var inputFlags);
                
                // if (inputText.Contains('"'))
                // {
                //     
                // }
                // else
                // {
                //     inputCommand = inputTextArr[0].ToLower();
                //     inputArguments = inputTextArr[1];
                //     inputFlags = inputTextArr[2].ToLower();
                // }

                switch (inputCommand)
                {
                    case "dir":
                        Console.WriteLine(currentDirectory.GetContent(inputFlags));
                        break;
                }
            }
        }
    }
}
