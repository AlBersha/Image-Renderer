namespace ConverterBase.Huffman
{
    using System;

    public class Node: IComparable<Node>
    {
        public string Symbol;
        public int Frequency;
        public string Code;
        public Node Parent;
        public Node LeftNode;
        public Node RightNode;

        public Node(string value)
        {
            Symbol = value;
            Frequency = 1;
            Code = "";
            Parent = LeftNode = RightNode = null;
        }

        public Node(Node first, Node second)
        {
            Code = "";
            Parent = null;
            Frequency = first.Frequency + second.Frequency;

            if (first.Frequency < second.Frequency)
            {
                RightNode = second;
                LeftNode = first;
                RightNode.Parent = LeftNode.Parent = this;
                Symbol = second.Symbol + first.Symbol;

            }
            else
            {
                RightNode = first;
                LeftNode = second;
                RightNode.Parent = LeftNode.Parent = this;
                Symbol = first.Symbol + second.Symbol;
            }
        }
        
        public bool IsLeaf()
        {
            return LeftNode == null && RightNode == null;
        }
        
        public int CompareTo(Node node)
        {
            return this.Frequency.CompareTo(node.Frequency);
        }

        public void IncreaseFrequency()
        {
            Frequency++;
        }
    }
}