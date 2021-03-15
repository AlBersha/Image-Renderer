namespace GifFormat
{
    using System.Collections.Generic;
    using ConverterBase;
    
    public class GIF : IImage
    {
        public GifHeader Header;
        public GifImageDescriptor ImageDescriptor;
        public List<RGB> ColorTable;
    }
    
    public class GifHeader
    {
        // Header
        public byte[] Signature = new byte[3]; /* Header Signature (always "GIF") */
        public byte[] Version = new byte[3];   /* GIF format version("87a" or "89a") */

        // Logical Screen Descriptor
        public short ScreenWidth;        /* Width of Display Screen in Pixels */
        public short ScreenHeight;       /* Height of Display Screen in Pixels */
        public byte Packed;              /* Screen and Color Map Information */
        public byte BackgroundColor;     /* Background Color Index */
        public byte AspectRatio;         /* Pixel Aspect Ratio */
    }
    
    public class GifImageDescriptor
    {
        public short Left;         /* X position of image on the display */
        public short Top;          /* Y position of image on the display */
        public short Width;        /* Width of the image in pixels */
        public short Height;       /* Height of the image in pixels */
        public byte Packed;        /* Image and Color Table Data Information */
    }

    public struct RGB
    {
        public byte Red;
        public byte Green;
        public byte Blue;
    }
}