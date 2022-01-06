using System;

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

                currentDirectory = Helper.RunCommand(currentDirectory, inputCommand, inputArguments, inputFlags);
            }
        }
    }
}