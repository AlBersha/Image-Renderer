namespace PNGFormat
{

    public class Chunk
    {
        public int Length;
        public char[] Name;
        public byte[] Content;
        public byte[] CRC;

        public Chunk()
        {
            Name = new char[4];
            Content = new byte[Length];
            CRC = new byte[4];
        }
    }
}