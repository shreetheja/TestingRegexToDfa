using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCmdFrame
{
    class Node
    {
        public int StateNumber;
        public Dictionary<char, Node> PrevTransition = new Dictionary<char, Node>();
        public Dictionary<char, Node> NextTransition = new Dictionary<char, Node>();


        public static int FIRSTNODE = -1;
        public static int LASTNODE = -2;
        public static char NullTransit = '&';
        public static char Epsilon = '$';
        public static char add = '|';
        public static char kleinStar = '*';
        public static char kleinPlus = '+';
        public static char openBrace = '(';
        public static char closeBrace = ')';
        public static char EndofLine = '~';
        public static List<Node> nodes;
        public static int CurrentNodeIndex = -1;
        public static int getaNodeIndex(int stateNumber = 0)
        {
            if(CurrentNodeIndex ==-1)
            {
                CurrentNodeIndex ++;
                nodes[0].StateNumber = stateNumber;
                return 0;
            }
            else
            {
                CurrentNodeIndex++;
                nodes[CurrentNodeIndex].StateNumber = stateNumber;
                if (CurrentNodeIndex>=Node.nodes.Count)                     ///to refill nodes
                {
                    for (int i = 0; i < 10; i++)
                    {
                        nodes.Add(new Node(0));
                    }
                }
                
                return CurrentNodeIndex;
            }
            
        }
        public static void ConvertToRealList(ref Node node)
        {
            node.StateNumber = Node.FIRSTNODE;
            Node lastNode = new Node(Node.LASTNODE);
            node.NextTransition.Add(Node.NullTransit, lastNode);
            lastNode.PrevTransition.Add(Node.NullTransit, node);
        }
        public Node(int stateNumber=-1)
        {
            if (stateNumber == Node.FIRSTNODE)
            {
                StateNumber = Node.FIRSTNODE;
                Node lastNode = new Node(Node.LASTNODE);
                this.NextTransition.Add(Node.NullTransit,lastNode);
                lastNode.PrevTransition.Add(Node.NullTransit, this);
            }
            else if(stateNumber == Node.LASTNODE)
            {
                StateNumber = Node.LASTNODE;
            }
            else
            {
                StateNumber = stateNumber;
            }
            if(Node.nodes == null)
            {
                Node.nodes = new List<Node>();
                for(int i=0;i<10;i++)
                {
                    nodes.Add(new Node(0));
                }
            }
        }

}
}
