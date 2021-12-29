using System;
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
            
            command = matchesArray[0].Value;
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
                    flags[flagsCounter] = matchesArray[i].Value.Substring(1);
                    flagsCounter++;
                }

            }
        }
        
        public static void SortByFlag()
        {
            
        }
    }
}