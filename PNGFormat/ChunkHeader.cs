namespace PNGFormat
{
    using System;

    public class ChunkHeader
    {
        public long Position { get; }
        public int Length { get; }
        public string Name { get; }
        public bool IsCritical => char.IsUpper(Name[0]);
        
        public ChunkHeader(long position, int length, string name)
        {
            if (length < 0)
            {
                throw new ArgumentException($"Length less than zero ({length}) encountered when reading chunk at position {position}.");
            }

            Position = position;
            Length = length;
            Name = name;
        }

    }
}