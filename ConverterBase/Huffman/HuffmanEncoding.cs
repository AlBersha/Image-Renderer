namespace ConverterBase.Huffman
{
    using System;
    using System.Collections.Generic;

    public class HuffmanEncoding
    {
        public List<Node> CreateHuffmanNodes(List<byte> bytesArray)
        {
            List<Node> huffmanNodes = new List<Node>();

            foreach (var item in bytesArray)
            {
                string symbol = Convert.ToChar(item).ToString();
                if (huffmanNodes.Exists(x => x.Symbol == symbol))
                {
                    huffmanNodes[huffmanNodes.FindIndex(x => x.Symbol == symbol)].IncreaseFrequency();
                }
                else
                {
                    huffmanNodes.Add(new Node(symbol));
                }
                
                huffmanNodes.Sort();
            }
            
            return huffmanNodes;
        }

        public bool CreateTreeFromList(List<Node> nodes)
        {
            while (nodes.Count > 1)
            {
                Node first = nodes[0];
                nodes.RemoveAt(0);
                Node second = nodes[0];
                nodes.RemoveAt(0);
                nodes.Add(new Node(first, second));
                nodes.Sort();
            }

            return true;
        }

        public void SetCode(string code, Node node)
        {
            if (node == null)
            {
                return;
            }

            if (node.IsLeaf())
            {
                node.Code = code;
                return;
            }

            SetCode(code + "0", node.LeftNode);
            SetCode(code + "1", node.RightNode);
        }
    }
}