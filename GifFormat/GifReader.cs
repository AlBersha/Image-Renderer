namespace GifFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ConverterBase;
    using ConverterBase.Readers;

    public class GifReader : IImageReader
    {
        private GIF GIF { get; set; }

        public GifReader()
        {
            GIF = new GIF();
        }

        public IImage ReadImage(string path)
        {
            try
            {
                using (var stream = File.OpenRead(path))
                {
                    using (var br = new BinaryReader(stream))
                    {
                        ReadHeader(br);
                        ReadGlobalColorTable(br);

                        var flag = true;
                        while (flag)
                        {
                            var code = br.ReadByte();
                            switch (code)
                            {
                                case 0x2C:
                                {
                                    ReadImageDescriptor(br);
                                    ReadLocalColorTable(br);
                                    ReadImageData(br);
                                    flag = false;
                                    break;
                                }
                                case 0x21:
                                {
                                    var extensionBlock = br.ReadByte();
                                    switch (extensionBlock)
                                    {
                                        case 0xF9:
                                        {
                                            ReadGraphicControlExtensionBlock(br);
                                            break;
                                        }
                                        case 0x01:
                                        {
                                            ReadPlainTextExtensionBlock(br);
                                            break;
                                        }
                                        case 0xFF:
                                        {
                                            ReadApplicationExtensionBlock(br);
                                            break;
                                        }
                                        case 0xFE:
                                        {
                                            ReadCommentExtensionBlock(br);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    return GIF;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong during .gif file reading. We will handle such kind of exception in the next release");
                throw;
            }
            
        }

        private void ReadHeader(BinaryReader br)
        {
            //Read GIF header
            GIF.Header.Signature = br.ReadBytes(3);
            GIF.Header.Version = br.ReadBytes(3);
            GIF.Header.ScreenWidth = br.ReadInt16();
            GIF.Header.ScreenHeight = br.ReadInt16();
            GIF.Header.Packed = br.ReadByte();
            GIF.Header.BackgroundColor = br.ReadByte();
            GIF.Header.AspectRatio = br.ReadByte();
        }

        private void ReadGlobalColorTable(BinaryReader br)
        {
            var globalColorTableFlag = (GIF.Header.Packed >> 7) & 1;

            if (globalColorTableFlag == 1)
            {
                //Bits 0-2 size of the Global Color Table 2^(N+1)
                var size = GIF.Header.Packed & ((1 << 3) - 1);
                var numberOfColorsInGlobalTable = (1 << (size + 1));

                //Read Global Color Table
                for (var i = 0; i < numberOfColorsInGlobalTable; i++)
                {
                    GIF.ColorTable.Add(new Pixel
                    {
                        Red = br.ReadByte(),
                        Green = br.ReadByte(),
                        Blue = br.ReadByte()
                    });
                }
            }
        }

        private void ReadLocalColorTable(BinaryReader br)
        {
             
            var localColorTableFlag = (GIF.ImageDescriptor.Packed >> 7) & 1;
                    
            if (localColorTableFlag == 1)
            {
                //Bits 0-2 size of the Local Color Table 2^(N+1)
                var size = GIF.ImageDescriptor.Packed & ((1 << 3) - 1);
                var numberOfColorsInLocalTable = (1 << (size + 1));

                GIF.ColorTable.Clear();
                //Read Local Color Table
                for (var i = 0; i < numberOfColorsInLocalTable; i++)
                {
                    GIF.ColorTable.Add(new Pixel
                    {
                        Red = br.ReadByte(),
                        Green = br.ReadByte(),
                        Blue = br.ReadByte()
                    });
                }
            }
        }

        private void ReadGraphicControlExtensionBlock(BinaryReader br)
        {
            var blockSize = br.ReadByte();
            var packed = br.ReadByte();
            var delayTime = br.ReadInt16();
            var colorIndex = br.ReadByte();
            var terminator = br.ReadByte();
        }

        private void ReadPlainTextExtensionBlock(BinaryReader br)
        {
            var blockSize = br.ReadByte();
            var textGridLeft = br.ReadInt16();
            var textGridTop = br.ReadInt16();
            var textGridWidth = br.ReadInt16();
            var textGridHeight = br.ReadInt16();
            var cellWidth = br.ReadByte();
            var cellHeight = br.ReadByte();
            var textFgColorIndex = br.ReadByte();
            var textBgColorIndex = br.ReadByte();
            var plainTextData = new List<byte>();
            
            var subBlockLength = br.ReadByte();
            while (subBlockLength != 0)
            {
                plainTextData.AddRange(br.ReadBytes(subBlockLength));
                subBlockLength = br.ReadByte();
            }
        }

        private void ReadApplicationExtensionBlock(BinaryReader br)
        {
            var blockSize = br.ReadByte();
            var identifier = br.ReadChars(8);
            var authentCode = br.ReadBytes(3);
            var applicationData = new List<byte>();

            var subBlockLength = br.ReadByte();
            while (subBlockLength != 0)
            {
                applicationData.AddRange(br.ReadBytes(subBlockLength));
                subBlockLength = br.ReadByte();
            }
        }

        private void ReadCommentExtensionBlock(BinaryReader br)
        {
            var commentData = new List<byte>();

            var subBlockLength = br.ReadByte();
            while (subBlockLength != 0)
            {
                commentData.AddRange(br.ReadBytes(subBlockLength));
                subBlockLength = br.ReadByte();
            }
        }

        private void ReadImageDescriptor(BinaryReader br)
        {
            GIF.ImageDescriptor.Left = br.ReadInt16();
            GIF.ImageDescriptor.Top = br.ReadInt16();
            GIF.ImageDescriptor.Width = br.ReadInt16();
            GIF.ImageDescriptor.Height = br.ReadInt16();
            GIF.ImageDescriptor.Packed = br.ReadByte();
        }

        private void ReadImageData(BinaryReader br)
        {
            var lzwMinCodeLength = br.ReadByte() + 1;

            var subBlockLength = br.ReadByte();
            var imageDataBytes = new List<byte>();
            while (subBlockLength != 0)
            {
                imageDataBytes.AddRange(br.ReadBytes(subBlockLength));
                //if this byte is 0, it`s end of frame, otherwise it`s length of next sub block
                subBlockLength = br.ReadByte();
            }

            var imageDataBits = "";
            foreach (var currByte in imageDataBytes)
            {
                imageDataBits += Reverse(Convert.ToString(currByte, 2).PadLeft(8, '0'));
            }
            var compressor = new LZWGifCompressor();
            var decompressedData = compressor.Decompress(imageDataBits, lzwMinCodeLength, GIF.ColorTable.Count);
            ConvertIntToRGB(decompressedData);
        }
        
        private string Reverse( string s )
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }

        private void ConvertIntToRGB(List<int> decompressedData)
        {
            for (var i = 0; i < GIF.ImageDescriptor.Height; i++)
            {
                var pixels = new List<Pixel>();

                for (var j = 0; j < GIF.ImageDescriptor.Width; j++)
                {
                    var index = decompressedData[i * GIF.ImageDescriptor.Width + j];
                    pixels.Add(GIF.ColorTable[index]); 
                }
                GIF.Data.Add(pixels);
            }

            var interlace = (GIF.ImageDescriptor.Packed >> 6) & 1;
            if (interlace == 1)
            {
                List<List<Pixel>> nonInterlacedImage = new List<List<Pixel>>();
                for (var i = 0; i < GIF.ImageDescriptor.Height;i++)
                {
                    nonInterlacedImage.Add(new List<Pixel>());
                }
                
                var j = 0;
                for (var i = 0; i < GIF.ImageDescriptor.Height; i+= 8,j++)
                {
                    nonInterlacedImage[i] = GIF.Data[j];
                }
                for (var i = 4; i < GIF.ImageDescriptor.Height; i+= 8,j++)
                {                   
                    nonInterlacedImage[i] = GIF.Data[j];
                }
                for (var i = 2; i < GIF.ImageDescriptor.Height; i+= 4,j++)
                {
                    nonInterlacedImage[i] = GIF.Data[j];
                }
                for (var i = 1; i < GIF.ImageDescriptor.Height; i+= 2,j++)
                {
                    nonInterlacedImage[i] = GIF.Data[j];
                }

                GIF.Data = nonInterlacedImage;
            }
        }
    }
}