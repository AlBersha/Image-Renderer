using System;

namespace Renderer
{
    using BMPReader;
    using ConverterBase;

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFile = "";
            var goalFormat = "";
            var outputFile = "";
            
            foreach (var arg in args)
            {
                if (arg.StartsWith("--source="))
                {
                    sourceFile = arg.Substring(arg.IndexOf('=') + 1);
                };
                
                if (arg.StartsWith("--goal-format"))
                {
                    goalFormat = arg.Substring(arg.IndexOf('=') + 1);
                }

                if (arg.StartsWith("--output"))
                {
                    outputFile = arg.Substring(arg.IndexOf('=') + 1);
                }
            }

            if (sourceFile == "")
            {
                Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            }

            if (goalFormat == "")
            {
                Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            }

            if (outputFile == "")
            {
                outputFile = sourceFile;
            }
            
            
            
        }
    }
}