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
        public Dictionary<char, List<Node>> PrevTransition = new Dictionary<char,List<Node>>();
        public Dictionary<char, List<Node>> NextTransition = new Dictionary<char, List<Node>>();


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
                if (CurrentNodeIndex >= Node.nodes.Count)                     ///to refill nodes
                {
                    for (int i = 0; i < 10; i++)
                    {
                        nodes.Add(new Node(0));
                    }
                }
                nodes[CurrentNodeIndex].StateNumber = stateNumber;
                
                
                return CurrentNodeIndex;
            }
            
        }
        public static void ConvertToRealList(ref Node node)
        {
            node.StateNumber = Node.FIRSTNODE;
            Node lastNode = new Node(Node.LASTNODE);
            node.AddNodeToNextTransitionList(Node.NullTransit,ref lastNode);
        }
        public void AddNodeToNextTransitionList(char transition,ref Node addingNode)
        {
            if(!this.NextTransition.ContainsKey(transition))
            {
                this.NextTransition[transition] = new List<Node>();
                this.NextTransition[transition].Add(addingNode);
            }
            else
            {
                this.NextTransition[transition].Add(addingNode);
            }
        }
        public Node(int stateNumber=-1)
        {
            if (stateNumber == Node.FIRSTNODE)
            {
                StateNumber = Node.FIRSTNODE;
                Node lastNode = new Node(Node.LASTNODE);
                this.AddNodeToNextTransitionList(Node.NullTransit,ref lastNode);
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
