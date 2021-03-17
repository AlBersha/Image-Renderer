namespace GifFormat
{
    using System;
    using System.Collections.Generic;

    public class LZWCompressor
    {
        private Dictionary<int, string> CodeTable { get; set; }
        private string ImageData { get; set; }

        public LZWCompressor()
        {
            CodeTable = new Dictionary<int, string>();
            ImageData = "";
        }

        private void InitializeCodeTable(List<RGB> initialCodeTable)
        {
            for (var i = 0; i < initialCodeTable.Count; i++)
            {
                CodeTable.Add(i, initialCodeTable[i].ToString());
            }

            CodeTable[initialCodeTable.Count] = "CC";
            CodeTable[initialCodeTable.Count + 1] = "EOI";
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
        
        public static byte[] Compress()
        {

            return new byte[] { 5};
        }

        public string Decompress(byte[] data, int startCodeLength, List<RGB> initialCodeTable)
        {
            ConvertBytesToString(data);
            Console.WriteLine(ImageData);
            InitializeCodeTable(initialCodeTable);

            var result = "";
            var codeLength = startCodeLength;
            
            //first code from 0 to codeLength always is always control code - CC, ignore it
            var code = Reverse(ImageData.Substring(codeLength, codeLength));
            result += CodeTable[Convert.ToInt32(code,2)];
            
            var pos = 2 * codeLength;
            var nextCode = Reverse(ImageData.Substring(pos, codeLength));
            
            while (CodeTable[Convert.ToInt32(nextCode,2)] != "EOI")
            {
                codeLength = Convert.ToString(CodeTable.Count, 2).Length;
                if (codeLength > 12)
                {
                    InitializeCodeTable(initialCodeTable);
                    //skip CC control code
                    pos += codeLength;
                    codeLength = startCodeLength;
                    code = Reverse(ImageData.Substring(pos, codeLength));
                    pos += codeLength;
                    result += CodeTable[Convert.ToInt32(code,2)];
                    
                }
                var prevCode = code;
                code = nextCode;
                
                var prevCodeValue = CodeTable[Convert.ToInt32(prevCode, 2)];
                var codeValue = CodeTable[Convert.ToInt32(code, 2)];
                
                if (CodeTable.ContainsKey(Convert.ToInt32(code,2)))
                {
                    result += CodeTable[Convert.ToInt32(code,2)];
                    //get first index from code
                    var k = codeValue.Substring(0, 24);
                    // add prevCode + k in codeTable
                    CodeTable[CodeTable.Count] = prevCodeValue + k;
                }
                else
                {
                    var k = prevCodeValue.Substring(0, 24);
                    result += prevCodeValue + k;
                    CodeTable[CodeTable.Count] = prevCodeValue + k;
                }
            
                pos += codeLength;
                nextCode = Reverse(ImageData.Substring(pos, codeLength));
            }
            
            Console.WriteLine(result);
            
            
            foreach (var pair in CodeTable)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}");
            }
            return result;
        }
        private string Reverse( string s )
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }
}