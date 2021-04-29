namespace ConsoleProcessor
{
    public interface ICommandProcessor
    {
        public string SourceFile { get; set; }
        public string SourceFormat { get; set; }
        public string GoalFormat { get; set; }
        public string OutputFile { get; set; }
        public bool ProcessCommand(string[] args);
    }
}