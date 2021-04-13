namespace Renderer
{
    using System.Collections.Generic;

    public class FormatValidator
    {
        private List<string> availableForReading = new List<string>(); 
        private List<string> availableForWriting = new List<string>();

        public FormatValidator(List<string> readingFormats, List<string> writingFormats)
        {
            availableForReading.AddRange(readingFormats);
            availableForWriting.AddRange(writingFormats);
        }

        public void AddReadingFormat(string format)
        {
            availableForReading.Add(format);
        }
        
        public bool ValidateSourceFileFormat(string format)
        {
            if (!availableForReading.Contains(format))
            {
                return false;
            }
            
            return true;
        }
        
        public bool ValidateGoalFileFormat(string format)
        {
            if (!availableForWriting.Contains(format))
            {
                return false;
            }
            
            return true;
        }

    }
}