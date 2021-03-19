namespace GifFormat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConverterBase;

    public class LZWGifCompressor
    {
        private Dictionary<int, List<int>> CodeTable { get; set; }
        private string ImageData { get; set; }
        
        public LZWGifCompressor()
        {
            CodeTable = new Dictionary<int, List<int>>();
            ImageData = "";
        }

        // private void InitializeCodeTable(List<RGB> initialCodeTable)
        // {
        //     CodeTable.Clear();
        //     for (var i = 0; i < initialCodeTable.Count; i++)
        //     {
        //         CodeTable.Add(i, initialCodeTable[i].ToString());
        //     }
        //
        //     CodeTable[initialCodeTable.Count] = "CC";
        //     CodeTable[initialCodeTable.Count + 1] = "EOI";
        // }
        private void InitializeCodeTable(int codeTableSize)
        {
            CodeTable.Clear();
            for (var i = 0; i < codeTableSize; i++)
            {
                CodeTable.Add(i, new List<int>(){i});
            }

            CodeTable[codeTableSize] = new List<int>(){codeTableSize};
            CodeTable[codeTableSize + 1] = new List<int>(){codeTableSize + 1};
        }

        private void ConvertBytesToString(byte[] data)
        {
            foreach (var t in data)
            {
                //Console.Write(Convert.ToString(subBlockImageData[i],2).PadLeft(8,'0'));
                //Console.Write(" ");
                ImageData += Reverse(Convert.ToString(t, 2)
                    .PadLeft(8, '0'));
            }
        }
        
        public void Compress()
        {
        }

        public List<int> Decompress(string imageData, int startCodeLength, int colorTableSize)
        {
            var clearCode = colorTableSize;
            var endOfInformation = colorTableSize + 1;

            ImageData = imageData;
            InitializeCodeTable(colorTableSize);
            var result = new List<int>();
            var codeLength = startCodeLength;
            
            //first code from 0 to codeLength is always control code - CC, ignore it
            var pos = codeLength;
            var code = Convert.ToInt32(Reverse(ImageData.Substring(pos, codeLength)),2);
            result.AddRange(CodeTable[code]);
            pos += codeLength;

            var nextCode = 0;
            var codeLengthIncreaseThreshold = 1 << codeLength;

            while (endOfInformation != nextCode)
            {
                if (nextCode == clearCode)
                {
                    InitializeCodeTable(colorTableSize);
                    //skip CC control code
                    pos += codeLength;
                    codeLength = startCodeLength;
                    codeLengthIncreaseThreshold = 1 << codeLength;
                    code = Convert.ToInt32(Reverse(ImageData.Substring(pos, codeLength)),2);
                    result.AddRange(CodeTable[code]);
                    pos += codeLength;
                }
                
                var prevCode = code;
                
                if (CodeTable.Count == codeLengthIncreaseThreshold)
                {
                    codeLength++;
                    codeLengthIncreaseThreshold = 1 << codeLength;
                }
                // if (codeLength == 13)
                // {
                //     codeLength = 12;
                //     
                // }                
                code = Convert.ToInt32(Reverse(ImageData.Substring(pos, codeLength)),2);
                pos += codeLength;
                nextCode = Convert.ToInt32(Reverse(ImageData.Substring(pos, codeLength)),2);
                
                if (CodeTable.ContainsKey(code))
                {
                    result.AddRange(CodeTable[code]);
                    var k = CodeTable[code][0];
                    CodeTable[CodeTable.Count] = CodeTable[prevCode].Concat(new List<int>{k}).ToList();
                }
                else
                {
                    var prevCodeValue = CodeTable[prevCode];
                    var k = prevCodeValue[0];
                    result.AddRange(prevCodeValue.Concat(new List<int>{k}).ToList());
                    CodeTable[CodeTable.Count] = prevCodeValue.Concat(new List<int> {k}).ToList();
                }
            }
            return result;
        }
        private string Reverse( string s )
        {
            var charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }
    
}