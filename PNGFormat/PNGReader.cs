namespace PNGFormat
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Transactions;
    using ConverterBase;
    using ConverterBase.Readers;

    public class PNGReader : IPNGReader
    {
        PNG image;

        //         public PNGReader()
        //         {
        //             image = new PNG();
        //         }
        //
        //         public IImage ReadImage(string path)
        //         {
        //             
        //             using (var stream = File.OpenRead(path))
        //             {
        //                 using (var binreader = new BinaryReader(stream))
        //                 {
        //                     image.Signature = binreader.ReadBytes(8);
        //                     
        //                     Chunk chunk = new Chunk();
        //                     string name = "";
        //                     do
        //                     {
        //                         var tmp = binreader.ReadBytes(4);
        //                         Array.Reverse(tmp);
        //                         chunk.Length = BitConverter.ToInt32(tmp, 0);
        //                         chunk.Name = binreader.ReadChars(4);
        //
        //                         chunk.Content = binreader.ReadBytes(chunk.Length);
        //                         chunk.CRC = binreader.ReadBytes(4);
        //                         name = new string(chunk.Name);
        //                         switch (name)
        //                         {
        //                             case "IHDR":
        //                                 ProcessIHDR(chunk);
        //                                 break;
        //                             case "PLTE":
        //                                 ProcessPLTE(chunk);
        //                                 break;
        //                             case "IDAT":
        //                                 ProcessIDAT(chunk);
        //                                 break;
        //                             case "gAMA":
        //                                 ProcessgAMA(chunk);
        //                                 break;
        //                         }
        //                         
        //                         
        //                         image.Chunks.Add(chunk);
        //                     } while (name != "IEND");
        //                 }
        //             }
        //
        //             return image;
        //         }
        //
        //         private void ProcessIHDR(Chunk chunk)
        //         {
        //             var tmp = chunk.Content[0 .. 4];
        //             Array.Reverse(tmp);
        //             image.Width = BitConverter.ToInt32(tmp, 0);
        //
        //             tmp = chunk.Content[4 .. 8];
        //             Array.Reverse(tmp);
        //             image.Height = BitConverter.ToInt32(tmp, 0);
        //
        //             image.BitDepth = chunk.Content[9];
        //
        //             if (chunk.Content[10] != 0 || chunk.Content[11] != 0)
        //             {
        //                 throw new IncorrectChunkException("Incorrect compression algorithm or filtration method.");
        //             }
        //
        //             image.Interlace = chunk.Content[12];
        //         }
        //
        //         private void ProcessIDAT(Chunk chunk)
        //         {
        //             byte flagCodes = chunk.Content[0];
        //             byte controlBits = chunk.Content[1];
        //
        //             int indexToRemove = 0;
        //             chunk.Content = chunk.Content.Where((source, index) => index != indexToRemove).ToArray();
        //             chunk.Content = chunk.Content.Where((source, index) => index != indexToRemove).ToArray();
        //
        //             BitArray bitArray = new BitArray(chunk.Content);
        //             int counter = 0;
        //             bool isFinal = false;
        //             do
        //             {
        //                 isFinal = bitArray.Get(counter++);
        //                 bool[] compression = new bool[2];
        //                 compression[0] = bitArray.Get(counter++);
        //                 compression[1] = bitArray.Get(counter++);
        //
        //
        //                 compression[1] = false;
        //                 
        //                 if (!compression[0] && !compression[1])
        //                 {
        //                     byte[] chuckContentCopy = new byte[chunk.Content.Length];
        //                     chuckContentCopy = chunk.Content;
        //
        //                     int blockLength = Convert.ToInt32(chuckContentCopy[0]);
        //                     Pixel item = new Pixel();
        //                     for (int i = 0; i < blockLength; i+=3)
        //                     {
        //                         item.Red = chuckContentCopy[i];
        //                         item.Green = chuckContentCopy[i + 1];
        //                         item.Blue = chuckContentCopy[i + 2];
        //                         
        //                         image.Data.Add(item);
        //                     }
        //                 }
        //                 else
        //                 {
        //                     if (compression[0] && !compression[1])
        //                     {
        //                         // read representation of code trees (see
        //                         // subsection below)
        //                     };
        //                     
        //                     if (!compression[0] && compression[1])
        //                     {
        //                     
        //                     
        //                     };
        //                     if (compression[0] && compression[1]) return;
        //                     
        //                     // loop (until end of block code recognized)
        //                     // while (expression)
        //                     // {
        //                     // decode literal/length value from input stream
        //                         BitArray literalsAndLength = new BitArray(8, false);
        //                         for (int i = 0; i < 5; i++)
        //                         {
        //                             literalsAndLength[i+3] = bitArray.Get(counter++);
        //                         }
        //                         Reverse(literalsAndLength);
        //                     
        //                         byte[] literalsAndLengthByte = new byte[1];
        //                         literalsAndLength.CopyTo(literalsAndLengthByte, 0);
        //                         if (literalsAndLengthByte[0] < 256)
        //                         {
        //                             // copy value (literal byte) to output stream
        //                         }
        //                         else
        //                         {
        //                             if (literalsAndLengthByte[0] == 256)
        //                             {
        //                                 break;
        //                             }
        //                             if (literalsAndLengthByte[0] > 256 && literalsAndLengthByte[0] < 287)
        //                             {
        //                                 // decode distance from input stream
        //                                 BitArray distances = new BitArray(8, false);
        //                                 for (int i = 0; i < 5; i++)
        //                                 {
        //                                     distances[i+3] = bitArray.Get(counter++);
        //                                 }
        //                                 Reverse(distances);
        //                             
        //                                 byte[] distancesByte = new byte[1];
        //                                 distances.CopyTo(distancesByte, 0);
        //                             
        //                                 // move backwards distance bytes in the output
        //                                 //     stream, and copy length bytes from this
        //                                 // position to the output stream
        //                             }
        //                         }
        //                     // }
        //
        //                 }
        //                 
        //                 
        //
        //                 
        //                 
        //                 
        //                 
        //                 
        //
        //             } while (!isFinal);
        //
        //             BitArray thirdTreeElements = new BitArray(8, false);
        //             for (int i = 0, j = 3; i < 4; i++, j++ )
        //             {
        //                 thirdTreeElements[i+4] = bitArray.Get(j);
        //             }
        //             Reverse(thirdTreeElements);
        //
        //             byte[] thirdTreeElementsByte = new byte[1];
        //             thirdTreeElements.CopyTo(thirdTreeElementsByte, 0);
        //
        //             byte[] alphabet = {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};
        //             Dictionary<byte, byte> frequencyMap = new Dictionary<byte, byte>();
        //             BitArray threeBitsSequence = new BitArray(8, false);
        //             for (int i = 0; i < thirdTreeElementsByte[0] + 4; i++)
        //             {
        //                 for (int j = 0; j < 3; j++)
        //                 {
        //                     threeBitsSequence[j + 5] = bitArray[counter++];
        //                 }
        //                 Reverse(threeBitsSequence);
        //                 byte[] symbolsCount = new byte[1];
        //                 threeBitsSequence.CopyTo(symbolsCount, 0);
        //                 frequencyMap.Add(alphabet[i], symbolsCount[0]);
        //             }
        //
        //
        //
        //         }
        //         
        //         private void Reverse(BitArray array)
        //         {
        //             int length = array.Length;
        //             int mid = (length / 2);
        //
        //             for (int i = 0; i < mid; i++)
        //             {
        //                 bool bit = array[i];
        //                 array[i] = array[length - i - 1];
        //                 array[length - i - 1] = bit;
        //             }    
        //         }
        //         
        //         private void ProcessPLTE(Chunk chunk)
        //         {
        //             
        //
        //         }
        //         private void ProcessbKGD (Chunk chunk)
        //         {
        //             
        //         }
        //
        //         private void ProcessgAMA(Chunk chunk)
        //         {
        //             Array.Reverse(chunk.Content);
        //             image.Gama = BitConverter.ToInt32(chunk.Content, 0);
        //         }
        //     }
        public IImage ReadImage(string path)
        {
            throw new NotImplementedException();
        }

        public PNG ReadImage(Stream stream, PngReaderSettings settings)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException($"The provided stream of type {stream.GetType().FullName} was not readable.");
            }

            var validHeader = HasValidHeader(stream);

            if (!validHeader.IsValid)
            {
                throw new ArgumentException($"The provided stream did not start with the PNG header. Got {validHeader}.");
            }

            var crc = new byte[4];
            var imageHeader = ReadImageHeader(stream, crc);

            var hasEncounteredImageEnd = false;

            Palette palette = null;

            using (var output = new MemoryStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    while (TryReadChunkHeader(stream, out var header))
                    {
                        if (hasEncounteredImageEnd)
                        {
                            if (settings?.DisallowTrailingData == true)
                            {
                                throw new InvalidOperationException($"Found another chunk {header} after already reading the IEND chunk.");
                            }

                            break;
                        }

                        var bytes = new byte[header.Length];
                        var read = stream.Read(bytes, 0, bytes.Length);
                        if (read != bytes.Length)
                        {
                            throw new InvalidOperationException($"Did not read {header.Length} bytes for the {header} header, only found: {read}.");
                        }

                        if (header.IsCritical)
                        {
                            switch (header.Name)
                            {
                                case "PLTE":
                                    if (header.Length % 3 != 0)
                                    {
                                        throw new InvalidOperationException($"Palette data must be multiple of 3, got {header.Length}.");
                                    }

                                    // Ignore palette data unless the header.ColorType indicates that the image is paletted.
                                    if (imageHeader.ColorType.HasFlag(ColorType.PaletteUsed)) {
                                        palette = new Palette(bytes);
                                    }

                                    break;
                                case "IDAT":
                                    memoryStream.Write(bytes, 0, bytes.Length);
                                    break;
                                case "IEND":
                                    hasEncounteredImageEnd = true;
                                    break;
                                default:
                                    throw new NotSupportedException($"Encountered critical header {header} which was not recognised.");
                            }
                        }

                        read = stream.Read(crc, 0, crc.Length);
                        if (read != 4)
                        {
                            throw new InvalidOperationException($"Did not read 4 bytes for the CRC, only found: {read}.");
                        }

                        var result = (int)CRC.Calculate(Encoding.ASCII.GetBytes(header.Name), bytes);
                        var crcActual = (crc[0] << 24) + (crc[1] << 16) + (crc[2] << 8) + crc[3];

                        if (result != crcActual)
                        {
                            throw new InvalidOperationException($"CRC calculated {result} did not match file {crcActual} for chunk: {header.Name}.");
                        }

                        settings?.ChunkVisitor?.Visit(stream, imageHeader, header, bytes, crc);
                    }

                    memoryStream.Flush();
                    memoryStream.Seek(2, SeekOrigin.Begin);

                    using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(output);
                        deflateStream.Close();
                    }
                }

                var bytesOut = output.ToArray();

                var (bytesPerPixel, samplesPerPixel) = Decoder.GetBytesAndSamplesPerPixel(imageHeader);

                bytesOut = Decoder.Decode(bytesOut, imageHeader, bytesPerPixel, samplesPerPixel);

                return new PNG(imageHeader, new RawPngData(bytesOut, bytesPerPixel, palette, imageHeader));
            }
        }
        private static HeaderValidation HasValidHeader(Stream stream)
        {
            return new HeaderValidation(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(),
                stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
        }
        
        private static ImageHeader ReadImageHeader(Stream stream, byte[] crc)
        {
            if (!TryReadChunkHeader(stream, out var header))
            {
                throw new ArgumentException("The provided stream did not contain a single chunk.");
            }

            if (header.Name != "IHDR")
            {
                throw new ArgumentException($"The first chunk was not the IHDR chunk: {header}.");
            }

            if (header.Length != 13)
            {
                throw new ArgumentException($"The first chunk did not have a length of 13 bytes: {header}.");
            }

            var ihdrBytes = new byte[13];
            var read = stream.Read(ihdrBytes, 0, ihdrBytes.Length);

            if (read != 13)
            {
                throw new InvalidOperationException($"Did not read 13 bytes for the IHDR, only found: {read}.");
            }

            read = stream.Read(crc, 0, crc.Length);
            if (read != 4)
            {
                throw new InvalidOperationException($"Did not read 4 bytes for the CRC, only found: {read}.");
            }

            var width = StreamProcessor.ReadBigEndianInt32(ihdrBytes, 0);
            var height = StreamProcessor.ReadBigEndianInt32(ihdrBytes, 4);
            var bitDepth = ihdrBytes[8];
            var colorType = ihdrBytes[9];
            var compressionMethod = ihdrBytes[10];
            var filterMethod = ihdrBytes[11];
            var interlaceMethod = ihdrBytes[12];

            return new ImageHeader(width, height, bitDepth, (ColorType)colorType, (CompressionMethod)compressionMethod, (FilterMethod)filterMethod,
                (InterlaceMethod)interlaceMethod);
        }
        
        private static bool TryReadChunkHeader(Stream stream, out ChunkHeader chunkHeader)
        {
            chunkHeader = default;

            var position = stream.Position;
            if (!StreamProcessor.TryReadHeaderBytes(stream, out var headerBytes))
            {
                return false;
            }

            var length = StreamProcessor.ReadBigEndianInt32(headerBytes, 0);

            var name = Encoding.ASCII.GetString(headerBytes, 4, 4);

            chunkHeader = new ChunkHeader(position, length, name);

            return true;
        }
    }
}