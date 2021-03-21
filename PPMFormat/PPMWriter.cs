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

            int counter = 0;
            for (int i = 0; i < image.Width; i++)
            {
                string row = "";
                for (int j = 0; j < image.Height; j++)
                {
                    row += image.Data[counter].Red + " " + image.Data[counter].Green + " " + image.Data[counter++].Blue + "   ";
                }
                fileData.Add(row);
            }
            
            File.WriteAllLines(outputPath, fileData);
            return true;
        }

        private int FindColorRange(List<Pixel> array)
        {
            int max = 0;
            foreach (var pixel in array)
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
            
            return max;
        }
    }
}