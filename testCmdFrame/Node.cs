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
        private static List<Node> VisitedNodes = new List<Node>();


        #region Linked List Helper Methods
        public List<Node> GetAllNodesPointingAtSomeNode(char desiredTransit, int DesiredStateNumber)
        {
            List<Node> listOfNodes = new List<Node>();
            VisitedNodes.Clear();
            GetAllNodesPointingAtSomeNodeRecur(ref listOfNodes, desiredTransit, DesiredStateNumber);
            return listOfNodes;
        }
        private void GetAllNodesPointingAtSomeNodeRecur(ref List<Node> ListofNodes, char desiredTransit,int DesiredStateNumber,Node prevNode = null,char Prevtransit = '~')
        {
            if(prevNode!=null)
            {
                if(Node.VisitedNodes.Contains(this))
                {
                    return;
                }
                else
                {
                    if(DesiredStateNumber == this.StateNumber && Prevtransit==desiredTransit)
                    {
                        ListofNodes.Add(prevNode);
                    }
                }
            }
            foreach (char transition in this.NextTransition.Keys)
            {
                foreach (Node TransitionNode in this.NextTransition[transition])
                {
                    Node.VisitedNodes.Add(this);
                    TransitionNode.GetAllNodesPointingAtSomeNodeRecur(ref ListofNodes, desiredTransit, DesiredStateNumber,this,transition);
                }
            }
        }
        public Node getaNodeBystateNumber(int stateNumber)
        {
            Node foundNode = null;
            VisitedNodes.Clear();
            getaNodeBystateNumberRecur(stateNumber,ref foundNode);
            return foundNode;
        }
        private void getaNodeBystateNumberRecur(int stateNumber,ref Node FoundNode)
        {
            if(FoundNode != null || VisitedNodes.Contains(this))
            {
                return;
            }
            foreach (char transition in this.NextTransition.Keys)
            {
                foreach (Node TransitionNode in this.NextTransition[transition])
                {
                    if (TransitionNode.StateNumber == stateNumber)
                    {
                        FoundNode = TransitionNode;
                        return;
                    }
                    else
                    {
                        VisitedNodes.Add(this);
                        TransitionNode.getaNodeBystateNumberRecur(stateNumber, ref FoundNode);
                    }
                }
            }
        }
        public List<Node> GetEpsilonTransits(bool debug=false)
        {
            List<Node> Nodes = new List<Node>();
            VisitedNodes.Clear();
            GetEpsilonTransitsRecur(ref Nodes,debug);
            return Nodes;
        }
        private void GetEpsilonTransitsRecur(ref List<Node> ToFillList, bool debug = false,Node prevNode = null)
        {
            if (prevNode != null)
            {
                if (Node.VisitedNodes.Contains(this))
                {
                    return;
                }
            }
            foreach (char transition in this.NextTransition.Keys)
            {
                foreach (Node TransitionNode in this.NextTransition[transition])
                {
                    VisitedNodes.Add(this);
                    if (transition == Node.Epsilon)
                    {
                        ToFillList.Add(this);
                        if(debug)
                        Console.WriteLine("Node with epsilon found stateNumber: " + TransitionNode.StateNumber);
                    }
                    TransitionNode.GetEpsilonTransitsRecur(ref ToFillList,debug, this);
                }
            }
        }
        public void DisplayNodes()
        {
            VisitedNodes.Clear();
            Console.WriteLine("Statrting the traverse Now ");
            DebugDisplayNode(this);
        }
        private void DebugDisplayNode(Node PrevNode = null,char lastTransition = '~')
        {
            if (PrevNode != null)
            {
                if (this.StateNumber == Node.LASTNODE)
                {
                    Console.WriteLine("Last Reached : " + this.StateNumber);
                    return;
                }
                else if (VisitedNodes.Contains(this))
                {
                    Console.WriteLine("Reached alredy visited Node with number : " + StateNumber + " By taking a transition of : " +lastTransition);
                    return;
                }
                
            }
            
            
            foreach (char transition in this.NextTransition.Keys)
            {
                foreach (Node TransitionNode in this.NextTransition[transition])
                {
                    VisitedNodes.Add(this);
                    Console.WriteLine("Currently in the Node : " + this.StateNumber + " And Transiting to : " + TransitionNode.StateNumber + " By the Character transition: " + transition);
                    TransitionNode.DebugDisplayNode(this,transition);
                }
            }

        }
        #endregion

        #region Node Adding Conversion Helpers
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
        #endregion

        public void AddNodeToNextTransitionList(char transition,ref Node addingNode)
        {
            if(!this.NextTransition.ContainsKey(transition))
            {
                
                this.NextTransition[transition] = new List<Node>();
                this.NextTransition[transition].Add(addingNode);
            }
            else
            {
                if (!this.NextTransition[transition].Contains(addingNode))
                {
                    this.NextTransition[transition].Add(addingNode);
                }
                else
                {
                    //Found the same node so ignore
                }
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
