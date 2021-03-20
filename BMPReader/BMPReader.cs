namespace BMPReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ConverterBase;

    public class BMPReader: IBMPRReader
    {
        public IImage ReadImage(string path)
        {
            BMP image = new BMP();
            
            using (var stream = File.OpenRead(path))
            {
                using (var reader = new BinaryReader(stream))
                {
                    image.BmpHeader.bfType = reader.ReadBytes(2);
                    image.BmpHeader.bfSize = reader.ReadInt32();
                    image.BmpHeader.bfReserved[0] = reader.ReadInt16();
                    image.BmpHeader.bfReserved[1] = reader.ReadInt16();
                    image.BmpHeader.bfHeadersize = reader.ReadInt32();
                    image.BmpHeader.biSize = reader.ReadInt32();
                    image.BmpHeader.biWidth = reader.ReadInt32();
                    image.BmpHeader.biHeight = reader.ReadInt32();
                    image.BmpHeader.biPlanes = reader.ReadInt16();
                    image.BmpHeader.biBitCount = reader.ReadInt16();
                    image.BmpHeader.biCompression = reader.ReadInt32();
                    image.BmpHeader.biSizeImage = reader.ReadInt32();
                    
                    image.BmpHeader.biXPelsPerMeter = reader.ReadInt32();
                    image.BmpHeader.biYPelsPerMeter = reader.ReadInt32();
                    image.BmpHeader.biClrUsed = reader.ReadInt32();
                    image.BmpHeader.biClrImportant = reader.ReadInt32();

                    if (image.BmpHeader.biWidth > 0 && image.BmpHeader.biHeight > 0)
                    {
                        for (int i = 0; i < image.BmpHeader.biWidth; i++)
                        {
                            List<Pixel> line = new List<Pixel>();
                            for (int j = 0; j < image.BmpHeader.biHeight; j++)
                            {
                                Pixel item = new Pixel();
                            
                                item.Red = reader.ReadByte();
                                item.Green = reader.ReadByte();
                                item.Blue = reader.ReadByte();

                                image.Data.Add(item);
                            }
                        
                        }
                    }
                    else
                    {
                        Console.WriteLine("The image has incorrect either width or height. Please try to process another image.");
                    }
                }
            }

            return image;
        }
    }
}