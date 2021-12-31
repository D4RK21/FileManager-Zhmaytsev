using System;
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

            return "";
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
    }
}