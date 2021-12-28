using System;
using System.IO;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../../";
            
            MyDirectory directory = new MyDirectory(path);
            Console.WriteLine(directory.GetContent());
            
            Console.ReadLine();

            // while (true)
            // {
            //     string inputText = Console.ReadLine();
            //
            //     switch (inputText)
            //     {
            //         case "cd":
            //             
            //             break;
            //     }
            // }
        }
    }
}
