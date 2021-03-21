namespace Renderer
{
    using System;
    using GifFormat;

    public class CommandConsoleProcessor: ICommandProcessor
    {
        public string SourceFile { get; set; }
        public string SourceFileName { get; set; }
        public string SourceFormat { get; set; }
        public string GoalFormat { get; set; }
        public string OutputFile { get; set; }

        public CommandConsoleProcessor()
        {
            SourceFile = "";
            GoalFormat = "";
            OutputFile = "";
        }

        public bool ProcessCommand(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
                
                if (args[i].StartsWith("--source="))
                {
                    SourceFile = args[i].Substring(args[i].IndexOf('=') + 1);
                    SourceFormat = SourceFile.Substring(SourceFile.LastIndexOf('.')+1);
                    SourceFileName = SourceFile.Substring(0, SourceFile.Length - SourceFormat.Length);
                }
                else if (args[i].StartsWith("--goal-format="))
                {
                    GoalFormat = args[i].Substring(args[i].IndexOf('=') + 1).ToLower();
                }
                else if (args[i].StartsWith("--output="))
                {
                    OutputFile = args[i].Substring(args[i].IndexOf('=') + 1);
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
            
            if (GoalFormat == "")
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