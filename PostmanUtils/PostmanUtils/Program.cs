using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PostmanUtils.Models;

namespace PostmanUtils
{
    class Program
    {
        const string ReadPrompt = "console> ";

        static void Main(string[] args)
        {
            Console.Title = typeof(Program).Name;
            // We will add some set-up stuff here later...

            Run();
            ReadFromConsole();
        }


        static void Run()
        {
            //var consoleInput = ReadFromConsole();
            var consoleInpuf = @"C:\Southern\Apps\OCC\test\Postman\PostmanEnvironments.json";

            try
            {
                // Execute the command:
                var result = ExecuteSplitEnvironments(consoleInpuf);

                // Write out the result:
                WriteToConsole(result);
            }
            catch (Exception ex)
            {
                // OOPS! Something went wrong - Write out the problem:
                WriteToConsole(ex.Message);
            }
        }

        static string ExecuteSplitEnvironments(string pathName)
        {
            // We'll make this more interesting shortly:

            var readFile = SharpSee.ReadFile(pathName);
            if (!readFile.Item1)
            {
                return readFile.Item2;
            }

            var environmentsList = JsonConvert.DeserializeObject<List<PostmanEnvironment>>(readFile.Item2);

            var environCnt = environmentsList?.Count ?? 0;

            for (var envIdx = 0; envIdx < environCnt; envIdx++)
            {
                var json = JsonConvert.SerializeObject(environmentsList[envIdx]);
                SharpSee.StringToFile(json, pathName.Replace(".json", $"{envIdx}.json"));
            }

            return $"Split the file {pathName}";
        }


        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }


        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write(ReadPrompt + promptMessage);
            return Console.ReadLine();
        }
    }
}
