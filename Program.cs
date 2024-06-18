using System;
using System.Collections.Generic;
using System.IO;

namespace se3
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"/Users/ann.adamchuk/calendar/client/node_modules/vue";
            string text = ReadDirectory(directoryPath);
            Metrics metrics = new Metrics(text);
            metrics.Process();
            Console.WriteLine(metrics.Result());
        }

        static string ReadDirectory(string directoryPath)
        {
            string text = "";
            string [] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (extension == ".js" || extension == ".mjs" || extension == ".ts" || extension == ".mts")
                {
                    text += File.ReadAllText(file);
                }
            }

            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDir in subDirectories)
            {
                text += ReadDirectory(subDir);
            }

            return text;
        }
    }
}
