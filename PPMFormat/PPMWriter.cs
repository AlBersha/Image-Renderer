namespace PPMFormat
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ConverterBase;
    using ConverterBase.Writers;

    public class PPMWriter: IImageWriter
    {
        public bool WriteImage(IImage image, string outputPath)
        {
            List<string> fileData = new List<string>();
            fileData.Add("P3");
            int width = image.Data[0].Count;
            int height = image.Data.Count;
            int colorRange = FindColorRange(image.Data);
            
            fileData.Add(width.ToString() + ' ' + height.ToString());
            fileData.Add(colorRange.ToString());

            for (int i = 0; i < height; i++)
            {
                string row = "";
                for (int j = 0; j < width; j++)
                {
                    row += image.Data[i][j].Red + " " + image.Data[i][j].Green + " " + image.Data[i][j].Blue + "   ";
                }
                fileData.Add(row);
            }

            File.WriteAllLines(outputPath, fileData);
            return true;
        }

        private int FindColorRange(List<List<Pixel>> array)
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