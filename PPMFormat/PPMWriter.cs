namespace PPMFormat
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ConverterBase;

    public class PPMWriter: IPPMWriter
    {
        public bool WriteImage(IImage image, string outputPath)
        {
            List<string> fileData = new List<string>();
            fileData.Add("P3");
            int width = image.Width;
            int height = image.Height;
            int colorRange = FindColorRange(image.Data);
            
            fileData.Add(width.ToString() + ' ' + height.ToString());
            fileData.Add(colorRange.ToString());

            foreach (var line in image.Data)
            {
                string row = "";
                foreach (var symbol in line)
                {
                    row += symbol.Red + " " + symbol.Green + " " + symbol.Blue + "   ";
                }
                fileData.Add(row);
            }

            File.WriteAllLines(outputPath, fileData);
            return true;
        }

        private int FindColorRange(List<List<RGB>> array)
        {
            int max = 0;
            foreach (var line in array)
            {
                foreach (var pixel in line)
                {
                    if (pixel.Red > max)
                    {
                        max = pixel.Red;
                    }

                    if (pixel.Blue > max)
                    {
                        max = pixel.Blue;
                    }

                    if (pixel.Green > max)
                    {
                        max = pixel.Green;
                    }
                }
            }
            
            return max;
        }
    }
}