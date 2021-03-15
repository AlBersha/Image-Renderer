namespace PPMFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class PPMReader: IPPMReader
    {
        public List<List<Pixel>> ReadPPM(string path)
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
                        ppmFileData.ColorRange = int.Parse(words[2]);
                    }
                    
                    continue;
                }

                if (isNumber && ppmFileData.ColorRange == 0) 
                {
                    ppmFileData.ColorRange = int.Parse(words[0]);
                    
                    continue;
                }

                if (isNumber)
                {
                    List<Pixel> pixels = new List<Pixel>();
                    for (int i = 0; i < ppmFileData.Width; i++)
                    {
                        Pixel item = new Pixel();
                        var tmp = int.TryParse(words[0], out item.R);
                        words.RemoveAt(0);
                        
                        tmp = int.TryParse(words[0], out item.G);
                        words.RemoveAt(0);
                        
                        tmp = int.TryParse(words[0], out item.B);
                        words.RemoveAt(0);

                        pixels.Add(item);
                    }

                    ppmFileData.Data.Add(pixels);
                }
            }

            return ppmFileData.Data;
        }
    }
}