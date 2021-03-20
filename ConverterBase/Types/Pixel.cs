namespace ConverterBase
{
    public struct Pixel
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;

        public bool IsGrayscale;
        
        public Pixel(byte r, byte g, byte b, byte a, bool isGrayscale)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
            IsGrayscale = isGrayscale;
        }
        
        public Pixel(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = 255;
            IsGrayscale = false;
        }
        
        public Pixel(byte grayscale)
        {
            Red = grayscale;
            Green = grayscale;
            Blue = grayscale;
            Alpha = 255;
            IsGrayscale = true;
        }
    }
}