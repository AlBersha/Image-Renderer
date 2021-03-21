namespace ConverterBase
{
    using System;
    public struct RGB
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        
        public override string ToString()
        {
            var redByte = Convert.ToString(Red, 2).PadLeft(8, '0');
            var greenByte = Convert.ToString(Green, 2).PadLeft(8, '0');
            var blueByte = Convert.ToString(Blue, 2).PadLeft(8, '0');

            return redByte + greenByte + blueByte;
        }
    }
}