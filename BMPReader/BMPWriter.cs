namespace BMPReader
{
    using System.IO;
    using ConverterBase;
    using ConverterBase.Writers;

    public class BMPWriter: IBMPWriter
    {
        public bool WriteImage(IImage image, string outputPath)
        {
            BMPHeader bmpheader = new BMPHeader(image.Width, image.Height);

            using (BinaryWriter writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                writer.Write(bmpheader.bfType);
                writer.Write(bmpheader.bfSize);
                writer.Write(bmpheader.bfReserved[0]);
                writer.Write(bmpheader.bfReserved[1]);
                writer.Write(bmpheader.bfHeadersize);
                writer.Write(bmpheader.biSize);
                writer.Write(bmpheader.biWidth);
                writer.Write(bmpheader.biHeight);
                writer.Write(bmpheader.biPlanes);
                writer.Write(bmpheader.biBitCount);
                writer.Write(bmpheader.biCompression);
                writer.Write(bmpheader.biSizeImage);
                writer.Write(bmpheader.biXPelsPerMeter);
                writer.Write(bmpheader.biYPelsPerMeter);
                writer.Write(bmpheader.biClrUsed);
                writer.Write(bmpheader.biClrImportant);
    
                foreach (var item in image.Data)
                {
                    writer.Write(item.Red);
                    writer.Write(item.Green);
                    writer.Write(item.Blue);
                }
            }

            return true;
        }
    }
}