namespace PNGFormat
{
    using System.IO;

    public interface IChunkVisitor
    {
        void Visit(Stream stream, ImageHeader header, ChunkHeader chunkHeader, byte[] data, byte[] crc);
    }
}