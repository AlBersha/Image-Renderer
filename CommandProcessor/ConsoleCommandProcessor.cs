using System;

namespace ConsoleProcessor
{
    public class ConsoleCommandProcessor: ICommandProcessor
    {
        public string SourceFile { get; set; }
        private string SourceFileName { get; set; }
        public string SourceFormat { get; set; }
        public string GoalFormat { get; set; }
        public string OutputFile { get; set; }

        
        public ConsoleCommandProcessor()
        {
            SourceFile = "";
            GoalFormat = "";
            OutputFile = "";
        }

        public bool ProcessCommand(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
                
                if (args[i].StartsWith("--source="))
                {
                    SourceFile = args[i][(args[i].IndexOf('=') + 1)..];
                    SourceFormat = SourceFile[(SourceFile.LastIndexOf('.')+1)..];
                    SourceFileName = SourceFile[..^SourceFormat.Length];
                }
                else if (args[i].StartsWith("--goal-format="))
                {
                    GoalFormat = args[i][(args[i].IndexOf('=') + 1)..].ToLower();
                }
                else if (args[i].StartsWith("--output="))
                {
                    OutputFile = args[i][(args[i].IndexOf('=') + 1)..];
                    OutputFile += "." + GoalFormat;
                }
                else
                {
                    Console.WriteLine($"Argument {args[i]} is invalid.");
                    Environment.Exit(1);
                }
            }
            
            if (SourceFile == "")
            {
                Console.WriteLine("Argument --source is either entered incorrectly or is missing");
                Environment.Exit(1);
            }
            
            if (GoalFormat == "" && !SourceFormat.Contains("obj"))
            {
                Console.WriteLine("Argument --goal-format is either entered incorrectly or is missing");
                Environment.Exit(1);
            }
            
            if (OutputFile == "")
            {
                OutputFile = SourceFileName + GoalFormat;
            }
            
            return true;
        }
    }
}