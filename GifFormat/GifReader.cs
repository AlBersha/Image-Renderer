namespace GifFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ConverterBase;

    public class GifReader : IGifReader
    {
        private GIF GIF { get; set; }

        public GifReader()
        {
            GIF = new GIF();
        }
        
        public IImage Read(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                using (var br = new BinaryReader(stream))
                {
                    //Read GIF header
                    GIF.Header.Signature = br.ReadBytes(3);
                    GIF.Header.Version = br.ReadBytes(3);
                    GIF.Header.ScreenWidth = br.ReadInt16();
                    GIF.Header.ScreenHeight = br.ReadInt16();
                    GIF.Header.Packed = br.ReadByte();
                    GIF.Header.BackgroundColor = br.ReadByte();
                    GIF.Header.AspectRatio = br.ReadByte();

                    var globalColorTableFlag = (GIF.Header.Packed >> 7) & 1;

                    if (globalColorTableFlag == 1)
                    {
                        //Bits 0-2 size of the Global Color Table 2^(N+1)
                        var size = GIF.Header.Packed & ((1 << 3) - 1);
                        var numberOfColorsInGlobalTable = (1 << (size + 1));

                        //Read Global Color Table
                        for (var i = 0; i < numberOfColorsInGlobalTable; i++)
                        {
                            GIF.ColorTable.Add(new RGB
                            {
                                Red = br.ReadByte(),
                                Green = br.ReadByte(),
                                Blue = br.ReadByte()
                            });
                        }
                    }

                    //find image descriptor flag(this indicates the beginning of the frame)
                    byte currentByte = 0;
                    while (currentByte != 0x2C)
                    {
                        currentByte = br.ReadByte();
                    }

                    //Read image descriptor info
                    GIF.ImageDescriptor.Left = br.ReadInt16();
                    GIF.ImageDescriptor.Top = br.ReadInt16();
                    GIF.ImageDescriptor.Width = br.ReadInt16();
                    GIF.ImageDescriptor.Height = br.ReadInt16();
                    GIF.ImageDescriptor.Packed = br.ReadByte();

                    var localColorTableFlag = (GIF.ImageDescriptor.Packed >> 7) & 1;

                    if (localColorTableFlag == 1)
                    {
                        //Bits 0-2 size of the Local Color Table 2^(N+1)
                        var size = GIF.ImageDescriptor.Packed & ((1 << 3) - 1);
                        var numberOfColorsInLocalTable = (1 << (size + 1));

                        GIF.ColorTable.Clear();
                        //Read Global Color Table
                        for (var i = 0; i < numberOfColorsInLocalTable; i++)
                        {
                            GIF.ColorTable.Add(new RGB
                            {
                                Red = br.ReadByte(),
                                Green = br.ReadByte(),
                                Blue = br.ReadByte()
                            });
                        }
                    }

                    var lzwMinCodeLength = br.ReadByte() + 1;
                    var flag = true;

                    // List<string> initialCodeTable = new List<string>();
                    //
                    // for (var i = 0; i < GIF.ColorTable.Count; i++)
                    // {
                    //     initialCodeTable.Add(i.ToString());
                    // }
                    //initialCodeTable.Add("CC");
                    //initialCodeTable.Add("EOI");
                    //read image data
                    var decompressedData = "";
                    while (flag)
                    {
                        var subBlockLength = br.ReadByte();
                        var subBlockImageData = new byte[subBlockLength];

                        subBlockImageData = br.ReadBytes(subBlockLength);
                        
                        var compressor = new LZWCompressor();
                        decompressedData += compressor.Decompress(subBlockImageData, lzwMinCodeLength, GIF.ColorTable);
                        
                        //if this byte is 0, it`s end of frame, otherwise it`s length of next sub block
                        subBlockLength = br.ReadByte();
                        flag = subBlockLength != 0;
                    }
                    ConvertByteToRGB(decompressedData);

                    foreach (var list in GIF.ImageData)
                    {
                        foreach (var pixel in list)
                        {
                            Console.Write($"{pixel.Red:X} {pixel.Green:X} {pixel.Blue:X} | ");
                            
                        }
                        Console.WriteLine();
                    }
                    
                }
            }

            return GIF;
        }

        private void ConvertByteToRGB(string byteData)
        {
            for (var i = 0; i < GIF.ImageDescriptor.Width; i++)
            {
                var pixels = new List<RGB>();
                
                for (var j = 0; j < GIF.ImageDescriptor.Height; j++)
                {
                    var red = byteData.Substring(i * GIF.ImageDescriptor.Width * 24 + j * 24, 8);
                    var green = byteData.Substring(i * GIF.ImageDescriptor.Width * 24 + j * 24 + 8, 8);
                    var blue = byteData.Substring(i * GIF.ImageDescriptor.Width * 24+ j * 24 + 16, 8);
                    pixels.Add(new RGB()
                    {
                        Red = Convert.ToByte(red,2),
                        Green = Convert.ToByte(green,2),
                        Blue = Convert.ToByte(blue,2)
                    });

                }
                GIF.ImageData.Add(pixels);
            }
        }
        
    }
}