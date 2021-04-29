using System.Collections.Generic;

namespace ConsoleProcessor
{
    public class FormatValidator
    {
        private readonly List<string> _availableForReading = new List<string>(); 
        private readonly List<string> _availableForWriting = new List<string>();

        public FormatValidator(List<string> readingFormats, List<string> writingFormats)
        {
            _availableForReading.AddRange(readingFormats);
            _availableForWriting.AddRange(writingFormats);
        }

        public void AddReadingFormat(string format)
        {
            _availableForReading.Add(format);
        }
        
        public bool ValidateSourceFileFormat(string format)
        {
            if (!_availableForReading.Contains(format))
            {
                return false;
            }
            
            return true;
        }
        
        public bool ValidateGoalFileFormat(string format)
        {
            return _availableForWriting.Contains(format);
        }

    }
}