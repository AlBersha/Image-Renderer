namespace PPMFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ConverterBase;
    using ConverterBase.Readers;

    public class PPMReader: IImageReader
    {
        public IImage ReadImage(string path)
        {
            string[] lines = File.ReadAllLines(path);
            PPM ppmFileData = new PPM();

            foreach (var line in lines)
            {
                if (line[0] == 'P')
                {
                    string[] tmp = line.Split(' ');
                    ppmFileData.Type = tmp[0];
                    
                    continue;
                }

                if (line[0] == '#')
                {
                    ppmFileData.Comments += line + '\n';
                    
                    continue;
                }

                List<string> words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                bool isNumber = int.TryParse(words[0], out _);
                
                if (isNumber && ppmFileData.Width == 0)
                {
                    ppmFileData.Width = int.Parse(words[0]);
                    ppmFileData.Height = int.Parse(words[1]);

                    if (words.Count > 2)
                    {
                        ppmFileData.BitDepth = byte.Parse(words[2]);
                    }
                    
                    continue;
                }

                if (isNumber && ppmFileData.BitDepth == 0) 
                {
                    ppmFileData.BitDepth = byte.Parse(words[0]);
                    
                    continue;
                }

                if (isNumber)
                {
                    for (int i = 0; i < ppmFileData.Height; i++)
                    {
                        List<Pixel> row = new List<Pixel>();
                        for (int j = 0; j < ppmFileData.Width; j++)
                        {
                            Pixel item = new Pixel();
                            var tmp = byte.TryParse(words[0], out item.Red);
                            words.RemoveAt(0);
                        
                            tmp = byte.TryParse(words[0], out item.Green);
                            words.RemoveAt(0);
                        
                            tmp = byte.TryParse(words[0], out item.Blue);
                            words.RemoveAt(0);
                            
                            row.Add(item);
                        }
                        ppmFileData.Data.Add(row);

                    }
                }
            }

            return ppmFileData;
        }
    }
}