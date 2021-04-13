namespace PNGFormat
{
    public class PngReaderSettings
    {
        public IChunkVisitor ChunkVisitor { get; set; }
        public bool DisallowTrailingData { get; set; }
    }
}