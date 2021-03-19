namespace PNGFormat
{
    using System;

    public class IncorrectChunkException: Exception
    {
        public IncorrectChunkException() : base() { }

        public IncorrectChunkException(string message): base(message) { }
    }
}