namespace PNGFormat
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Transactions;
    using ConverterBase;

    public class PNGReader: IPNGReader
    {
        PNG image;

        public PNGReader()
        {
            image = new PNG();
        }

        public IImage ReadImage(string path)
        {
            
            using (var stream = File.OpenRead(path))
            {
                using (var binreader = new BinaryReader(stream))
                {
                    image.Signature = binreader.ReadBytes(8);
                    
                    Chunk chunk = new Chunk();
                    string name = "";
                    do
                    {
                        var tmp = binreader.ReadBytes(4);
                        Array.Reverse(tmp);
                        chunk.Length = BitConverter.ToInt32(tmp, 0);
                        chunk.Name = binreader.ReadChars(4);

                        chunk.Content = binreader.ReadBytes(chunk.Length);
                        chunk.CRC = binreader.ReadBytes(4);
                        name = new string(chunk.Name);
                        switch (name)
                        {
                            case "IHDR":
                                ProcessIHDR(chunk);
                                break;
                            case "PLTE":
                                ProcessPLTE(chunk);
                                break;
                            case "IDAT":
                                ProcessIDAT(chunk);
                                break;
                            case "gAMA":
                                ProcessgAMA(chunk);
                                break;
                        }
                        
                        
                        image.Chunks.Add(chunk);
                    } while (name != "IEND");
                }
            }

            return image;
        }

        private void ProcessIHDR(Chunk chunk)
        {
            var tmp = chunk.Content[0 .. 4];
            Array.Reverse(tmp);
            image.Width = BitConverter.ToInt32(tmp, 0);

            tmp = chunk.Content[4 .. 8];
            Array.Reverse(tmp);
            image.Height = BitConverter.ToInt32(tmp, 0);

            image.BitDepth = chunk.Content[9];

            if (chunk.Content[10] != 0 || chunk.Content[11] != 0)
            {
                throw new IncorrectChunkException("Incorrect compression algorithm or filtration method.");
            }

            image.Interlace = chunk.Content[12];
        }

        private void ProcessIDAT(Chunk chunk)
        {
            byte flagCodes = chunk.Content[0];
            byte controlBits = chunk.Content[1];

            int indexToRemove = 0;
            chunk.Content = chunk.Content.Where((source, index) => index != indexToRemove).ToArray();
            chunk.Content = chunk.Content.Where((source, index) => index != indexToRemove).ToArray();

            BitArray bitArray = new BitArray(chunk.Content);
            int counter = 0;
            bool isFinal = false;
            do
            {
                isFinal = bitArray.Get(counter++);
                bool[] compression = new bool[2];
                compression[0] = bitArray.Get(counter++);
                compression[1] = bitArray.Get(counter++);


                compression[1] = false;
                
                if (!compression[0] && !compression[1])
                {
                    byte[] chuckContentCopy = new byte[chunk.Content.Length];
                    chuckContentCopy = chunk.Content;

                    int blockLength = Convert.ToInt32(chuckContentCopy[0]);
                    Pixel item = new Pixel();
                    for (int i = 0; i < blockLength; i+=3)
                    {
                        item.Red = chuckContentCopy[i];
                        item.Green = chuckContentCopy[i + 1];
                        item.Blue = chuckContentCopy[i + 2];
                        
                        image.Data.Add(item);
                    }
                }
                else
                {
                    if (compression[0] && !compression[1])
                    {
                        // read representation of code trees (see
                        // subsection below)
                    };
                    
                    if (!compression[0] && compression[1])
                    {
                    
                    
                    };
                    if (compression[0] && compression[1]) return;
                    
                    // loop (until end of block code recognized)
                    // while (expression)
                    // {
                    // decode literal/length value from input stream
                        BitArray literalsAndLength = new BitArray(8, false);
                        for (int i = 0; i < 5; i++)
                        {
                            literalsAndLength[i+3] = bitArray.Get(counter++);
                        }
                        Reverse(literalsAndLength);
                    
                        byte[] literalsAndLengthByte = new byte[1];
                        literalsAndLength.CopyTo(literalsAndLengthByte, 0);
                        if (literalsAndLengthByte[0] < 256)
                        {
                            // copy value (literal byte) to output stream
                        }
                        else
                        {
                            if (literalsAndLengthByte[0] == 256)
                            {
                                break;
                            }
                            if (literalsAndLengthByte[0] > 256 && literalsAndLengthByte[0] < 287)
                            {
                                // decode distance from input stream
                                BitArray distances = new BitArray(8, false);
                                for (int i = 0; i < 5; i++)
                                {
                                    distances[i+3] = bitArray.Get(counter++);
                                }
                                Reverse(distances);
                            
                                byte[] distancesByte = new byte[1];
                                distances.CopyTo(distancesByte, 0);
                            
                                // move backwards distance bytes in the output
                                //     stream, and copy length bytes from this
                                // position to the output stream
                            }
                        }
                    // }

                }
                
                

                
                
                
                
                

            } while (!isFinal);

            BitArray thirdTreeElements = new BitArray(8, false);
            for (int i = 0, j = 3; i < 4; i++, j++ )
            {
                thirdTreeElements[i+4] = bitArray.Get(j);
            }
            Reverse(thirdTreeElements);

            byte[] thirdTreeElementsByte = new byte[1];
            thirdTreeElements.CopyTo(thirdTreeElementsByte, 0);

            byte[] alphabet = {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};
            Dictionary<byte, byte> frequencyMap = new Dictionary<byte, byte>();
            BitArray threeBitsSequence = new BitArray(8, false);
            for (int i = 0; i < thirdTreeElementsByte[0] + 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    threeBitsSequence[j + 5] = bitArray[counter++];
                }
                Reverse(threeBitsSequence);
                byte[] symbolsCount = new byte[1];
                threeBitsSequence.CopyTo(symbolsCount, 0);
                frequencyMap.Add(alphabet[i], symbolsCount[0]);
            }



        }
        
        private void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }    
        }
        
        private void ProcessPLTE(Chunk chunk)
        {
            

        }
        private void ProcessbKGD (Chunk chunk)
        {
            
        }

        private void ProcessgAMA(Chunk chunk)
        {
            Array.Reverse(chunk.Content);
            image.Gama = BitConverter.ToInt32(chunk.Content, 0);
        }
    }
}