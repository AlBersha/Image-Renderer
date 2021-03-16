namespace BMPReader
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using ConverterBase;
    using ConverterBase.Readers;

    public class BMPReader: IBMPRReader
    {
        public IImage ReadImage(string path)
        {
            BMP image = new BMP();
            
            using (var stream = File.OpenRead(path))
            {
                using (var reader = new BinaryReader(stream))
                {
                    image.BmpHeader.bfType = reader.ReadInt32();
                    image.BmpHeader.bfSize = reader.ReadInt32();
                    image.BmpHeader.bfReserved = reader.ReadInt32();
                    image.BmpHeader.bfOffBits = reader.ReadInt32();
                    image.BmpHeader.biSize = reader.ReadInt32();
                    image.BmpHeader.biWidth = reader.ReadInt32();
                    image.BmpHeader.biHeight = reader.ReadInt32();
                    image.BmpHeader.biPlanes = reader.ReadInt32();
                    image.BmpHeader.biBitCount = reader.ReadInt32();
                    image.BmpHeader.biCompression = reader.ReadInt32();
                    image.BmpHeader.biSizeImage = reader.ReadInt32();
                }
            }

            return image;
        }
    }
}