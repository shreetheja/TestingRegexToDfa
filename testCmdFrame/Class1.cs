using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCmdFrame
{
   
    class Class1
    {
        public List<char> TerminalValue = new List<char>();
        public Node solvedNode = new Node();
        public int usedstates = 0;
        Node SolveTheREGEX(string input)
        {
            Node solved;
            if (ValidateAndInitialiseInput(input))
            {
                solved = Solve(input, Node.EndofLine);
                return solved;
            }
            else
                return null;
        }
        #region Main Methods
        private bool ValidateAndInitialiseInput(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) || Char.IsDigit(input[i]))
                {
                    if (!TerminalValue.Contains(input[i]))
                    {
                        TerminalValue.Add(input[i]);
                    }

                }
                else
                {
                    switch (input[i])
                    {
                        case '|':
                            { }
                            break;
                        case '*':
                            { }
                            break;
                        case '+':
                            { }
                            break;
                        case ' ':
                            Console.WriteLine("Whitespaces are invalid"); return false;
                            break;
                        case ')':
                            { }
                            break;
                        case '(':
                            { }
                            break;
                        default:
                            Console.WriteLine("The variable " + input[i] + " is invalid"); return false;
                            break;
                    }
                }
            }
            return true;
        }
        private Node Solve(string input,char operatorAbove)
        {
            char sameSymbol;
            if (IFAllSameOperation(input.ToCharArray(), out sameSymbol))
            {
                switch (sameSymbol)
                {
                    case '|': return ConvertAddsymbolsToNode(input.ToCharArray());
                    case '+': return ConvertKlienPlussmbolsToNode(input.ToCharArray());
                    case '*': return ConvertKlienStarSymbolsToNode(input.ToCharArray());
                    case 'T': return ConvertSinglesymbolToNode(input.ToCharArray()[0]);                                                   //terminal value
                    default: return ConvertProductSymbolsToNode(input.ToCharArray());
                }
            }
            if(input == "()")
            {
                Node Empty = Node.nodes[Node.getaNodeIndex()];
                Node.ConvertToRealList(ref Empty);
                return Empty;
            }
            string[] Splitinput = SplitTokens(input,ref operatorAbove);
            Node LeftNode = null;
            Node RightNode = null;
            LeftNode = Solve(Splitinput[0],operatorAbove);
            RightNode = Solve(Splitinput[1],operatorAbove);
            Node JoinedNode = JoinNodes(LeftNode, RightNode, operatorAbove);
            return JoinedNode;
        }
        private string[] SplitTokens(string input,ref char operatorHere)
        {
            char[] inputCharacters = input.ToCharArray();
            string[] returningArray = new string[100];
            //TODO:startinf ()

            for (int i = 0; i < inputCharacters.Length; i++)
            {

                if (inputCharacters[i] == '|')
                {
                    operatorHere = '|';
                    returningArray[0] = characterArrayToString(inputCharacters, i, true,false);
                    returningArray[1] = characterArrayToString(inputCharacters, i, false,false);
                    return returningArray;
                }
                else if (inputCharacters[i] == '(')
                {
                    for (int j = i; ; j++)
                    {
                        if (inputCharacters[j] == ')')
                        {
                            i = j;
                            break;
                        }
                    }
                }
            }
            
            if(!TryBracketSplit(inputCharacters, out returningArray,ref operatorHere))
            {
                TryKlienSplit(inputCharacters, out returningArray, ref operatorHere);
            }


            return returningArray;
        }
        #endregion

        #region Helper Methods Of Split Tokens
        bool IFAllSameOperation(char[] characters,out char sameSymb)
        {
            char SameSymbol = 'N';//not found yet state
            bool foundSymbol = false;
            bool foundTerminalOnce = false;
            if (characters.Length < 2 && TerminalValue.Contains(characters[0]))              //terminal only
            {
                sameSymb = 'T';
                return true;
            }
            foreach(char c in characters)                                                   //Unique check for concat and asterisk or plus
            {
                if(c=='*'||c=='+')
                {
                    if(characters.Length!=2)                                                //if ther is a klien symbol ther then it shud be of lenght 2(eg:a*) else its multiple operation 
                    {
                        sameSymb = 'N';
                        return false;
                    }
                    else
                    {
                        sameSymb = c;
                        return true;
                    }
                }
            }
            foreach(char c in characters)
            {
                if(TerminalValue.Contains(c))
                {
                    if (!foundTerminalOnce)
                    {
                        foundTerminalOnce = true;
                        continue;
                    }
                    else
                    {
                        if(SameSymbol=='N')     //not found any symbol yet               
                        {
                            foundSymbol = true;
                            sameSymb = 'p';
                        }
                        else
                        {
                            sameSymb = 'N';
                            return false;
                        }
                    }
                }
                else
                {
                    foundTerminalOnce = false;
                    if(!foundSymbol)
                    {
                        SameSymbol = c;
                        foundSymbol = true;
                    }
                    else
                    {
                        if(c == SameSymbol)
                        {
                            continue;
                        }
                        else
                        {
                            sameSymb = 'N';
                            return false;
                        }
                    }
                }
            }                                               //Check for other symbols like | and also concatenation only
            sameSymb = SameSymbol;
            return true;
        }
        bool TryBracketSplit(char[] inputCharacters, out string[] output,ref char operatorabove)
        {
            bool bracketSuccess = false;
            output = new string[100];
            if(inputCharacters[0] == '(')
            {
               
                if(inputCharacters[inputCharacters.Length-1]==')')                              //for Test case (a|b)
                {   
                    bracketSuccess = true;
                    operatorabove = ')';
                    output[0] = InsideRecentBrackets(inputCharacters);
                    output[1] = "()";
                    return bracketSuccess;
                }
                else if(inputCharacters[inputCharacters.Length - 2] == ')')                     //For Test case (a|b)*
                {
                    if(inputCharacters[inputCharacters.Length - 1] == '+'|| inputCharacters[inputCharacters.Length - 1] == '*')//make sure that last symbol is a * or +
                    {
                        operatorabove = inputCharacters[inputCharacters.Length-1];
                        output[0] = InsideRecentBrackets(inputCharacters);
                        output[1] = "()";
                        return true;
                    }
                }
                else
                {
                    int i;
                    for (i = 0; i < inputCharacters.Length; i++)
                    {
                        if (inputCharacters[i] == ')')
                        {
                            if (inputCharacters[i + 1] == '+' || inputCharacters[i + 1] == '*')                 
                            {
                                i++;                                                                //include klien star  or plus
                                operatorabove = 'P';
                                output[0] = characterArrayToString(inputCharacters,i,true,true);
                            }
                            else
                                output[0] = characterArrayToString(inputCharacters,i,true,true);
                            break;
                        }

                    }
                    bracketSuccess = true;
                    output[1] = characterArrayToString(inputCharacters, i,false,false);
                    return bracketSuccess;
                }
            }
            for (int i = 0; i < inputCharacters.Length; i++)
            {
                if (inputCharacters[i] == '(')
                {
                    ///There are 3 cases here
                    ///1.abc | (a|b) Here Operator above is |
                    ///2.abc   (a|b) Here Operator above is P (Concatenation)
                    ///3.abc*  (a|b) Here opearator is actually P but there is * or + so split the case again

                    if(inputCharacters[i-1]=='*'||inputCharacters[i-1]=='+')            //Here testcase 3 resolved that is abc*(a|b)
                    {
                        if(TerminalValue.Contains(inputCharacters[i-2]))
                        {
                            //Concatenation
                            operatorabove = 'P';
                            output[0] = characterArrayToString(inputCharacters, i-1, true, true);
                            output[1] = characterArrayToString(inputCharacters, i, false, true);
                            bracketSuccess = true;
                            return bracketSuccess;
                        }
                        else
                        {
                            //add
                            operatorabove = '|';
                            output[0] = characterArrayToString(inputCharacters, i - 1, true, true);
                            output[1] = characterArrayToString(inputCharacters, i, false, true);
                            bracketSuccess = true;
                            return bracketSuccess;
                        }

                    }
                    else
                    {
                        if (TerminalValue.Contains(inputCharacters[i - 1]))
                        {
                            //Concatenation
                            operatorabove = 'P';
                            output[0] = characterArrayToString(inputCharacters, i, true,false);
                            output[1] = characterArrayToString(inputCharacters, i, false, true);
                            bracketSuccess = true;
                            return bracketSuccess;
                        }
                        else
                        {
                            //add
                            operatorabove = '|';
                            output[0] = characterArrayToString(inputCharacters, i, true,false);
                            output[1] = characterArrayToString(inputCharacters, i, false, true);
                            bracketSuccess = true;
                            return bracketSuccess;
                        }
                    }
                }

            }
            return bracketSuccess;
        }
        void TryKlienSplit(char[] inputCharacters, out string[] output, ref char operatorabove)
        {
            int starOrPlusIndex = 0;
            output = new string[100];
            foreach (char c in inputCharacters)
            {
                if(c=='*'||c=='+')
                {
                    break;
                }
                starOrPlusIndex++;
            }
            if(starOrPlusIndex == 1)                                            //Test case a*bc
            {
                operatorabove = 'P';
                output[0] = characterArrayToString(inputCharacters, starOrPlusIndex, true,true);
                output[1] = characterArrayToString(inputCharacters, starOrPlusIndex, false,false);
            }
            else if(starOrPlusIndex == (inputCharacters.Length-1))              //test case abc*
            {
                operatorabove = 'P';
                output[0] = characterArrayToString(inputCharacters, starOrPlusIndex-1, true,false);
                output[1] = characterArrayToString(inputCharacters, starOrPlusIndex-1, false,true);
            }
            else                                                                //Test case ab*c
            {
                operatorabove = 'P';
                output[0] = characterArrayToString(inputCharacters, starOrPlusIndex-1, true,false);
                output[1] = characterArrayToString(inputCharacters, starOrPlusIndex-1, false,true);

            }

        }

        #endregion

        #region custom string Methods
        string characterArrayToString(char[] array,int symbolIndex,bool isBefore,bool includeSymbol)
        {
            string returnString = "";
            if(isBefore)
            {
                if (includeSymbol)
                {
                    for (int i = 0; i <= symbolIndex; i++)
                    {
                        returnString += array[i];
                    }
                }
                else
                {
                    for (int i = 0; i < symbolIndex; i++)
                    {
                        returnString += array[i];
                    }
                }
            }
            else
            {
                if (includeSymbol)
                {
                    for (int i = symbolIndex; i<array.Length; i++)
                    {
                        returnString += array[i];
                    }
                }
                else
                {
                    for (int i = symbolIndex+1; i <array.Length; i++)
                    {
                        returnString += array[i];
                    }
                }
            }
            return returnString;
        }
        string InsideRecentBrackets(char[] array)
        {
            string returnString = "";
            int FirstBracIndex = -1;
            int LastBracIndex = -1;
            for(int i=0;i<array.Length;i++)
            {
                if(array[i] == '('&&FirstBracIndex==-1)
                {
                    FirstBracIndex = i;
                }
                if(array[i] == ')')
                {
                    LastBracIndex = i;
                }

            }
            for(int i=FirstBracIndex+1;i<LastBracIndex;i++)
            {
                returnString += array[i];
            }
            return returnString;
        }
        #endregion

        #region  BaseSolvers
        Node ConvertAddsymbolsToNode(char[] inputSymbols)
        {
            Console.WriteLine("Add Symbols to nodes " + inputSymbols);
            Node sendNode = Node.nodes[Node.getaNodeIndex()];                           //Just a node
            Node.ConvertToRealList(ref sendNode);                                       //Node wh ts and tf
            Node sendingLastNode = sendNode.NextTransition[Node.NullTransit][0];
            sendNode.NextTransition.Remove(Node.NullTransit);
            Node addFirstNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
            sendNode.AddNodeToNextTransitionList(Node.NullTransit, ref addFirstNode);
            int numberOfPlus = 0;
            foreach(char c in inputSymbols)
            {
                if (c == '|')
                    numberOfPlus++;
            }
            for(int i=0;i<=numberOfPlus*2;i+=2)
            {
                Node addNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
                addFirstNode.AddNodeToNextTransitionList(inputSymbols[i], ref addNode);
                addNode.AddNodeToNextTransitionList(Node.NullTransit,ref sendingLastNode);
            }
            return sendNode;

        }
        Node ConvertProductSymbolsToNode(char[] inputSymbols)
        {
            Console.WriteLine("Product Symbols to nodes " + inputSymbols);
            Node sendNode = Node.nodes[Node.getaNodeIndex()];                           //Just a node
            Node.ConvertToRealList(ref sendNode);
            Node sendingLastNode = sendNode.NextTransition[Node.NullTransit][0];
            int numberOfTerminals=0;
            foreach(char c in inputSymbols)
            {
                numberOfTerminals++;
            }
            Node current = Node.nodes[Node.getaNodeIndex(++usedstates)];
            sendNode.NextTransition.Remove(Node.NullTransit);
            sendNode.AddNodeToNextTransitionList(Node.NullTransit,ref current);
            for (int i=0;i<numberOfTerminals;i++)
            {
                Node nextNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
                current.AddNodeToNextTransitionList(inputSymbols[i], ref nextNode);
                current = nextNode;
            }
            current.AddNodeToNextTransitionList(Node.NullTransit, ref sendingLastNode);
            return sendNode;
        }
        Node ConvertKlienStarSymbolsToNode(char[] inputSymbols)
        {
            ///Tasks to do here
            ///conside a*
            ///here the single node Function will give ts->(&)1->(a)2->tf
            ///1)Now here since its a Klienstar even First symbol is  final state so point it to tf
            ///2)And make 2nd state point itself to the symbol a

            Console.WriteLine("klienstaring : " + inputSymbols);
            Node sendNode = ConvertSinglesymbolToNode(inputSymbols[0]);
            Node lastNode = sendNode.NextTransition[Node.NullTransit][0].NextTransition[inputSymbols[0]][0].NextTransition[Node.NullTransit][0];


            ///Now we have states like ts->1->2->tf 2 is final state yes but this is klien star 
            ///So even epsilon is final so make the state 1 to point at tf too
            
            Node FirstOneNode = sendNode.NextTransition[Node.NullTransit][0];            //First Node
            FirstOneNode.AddNodeToNextTransitionList(Node.NullTransit,ref lastNode);
            

            Node SecondOneNode = FirstOneNode.NextTransition[inputSymbols[0]][0];            //last but One node
            SecondOneNode.AddNodeToNextTransitionList(inputSymbols[0],ref SecondOneNode);    //add a loop back to klien star
            
            return sendNode;

        }
        Node ConvertKlienPlussmbolsToNode(char[] inputSymbols)
        {
            ///Tasks to do here
            ///conside a+
            ///here the single node Function will give ts->(&)1->(a)2->tf
            ///1)And make 2nd state point itself to the symbol a
            Console.WriteLine("Klien plusing : " + inputSymbols);
            Node sendNode = ConvertSinglesymbolToNode(inputSymbols[0]);
            Node FirstOneNode = sendNode.NextTransition[Node.NullTransit][0];            //First Node
            Node LastButOneNode = FirstOneNode.NextTransition[inputSymbols[0]][0];            //last but One node
            LastButOneNode.AddNodeToNextTransitionList(inputSymbols[0], ref LastButOneNode);  //add a loop back to klien star
            return sendNode;
        }
        Node ConvertSinglesymbolToNode(char inputSymbol)
        {
            Console.WriteLine("Single Symbol op : " + inputSymbol);
            Node SendNode = Node.nodes[Node.getaNodeIndex()];
            Node.ConvertToRealList(ref SendNode);
            Node sendingLastNode = SendNode.NextTransition[Node.NullTransit][0];
            Node singleNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
            SendNode.NextTransition.Remove(Node.NullTransit);
            SendNode.AddNodeToNextTransitionList(Node.NullTransit, ref singleNode);
            Node secondNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
            singleNode.AddNodeToNextTransitionList(inputSymbol,ref secondNode);
          //singleNode.NextTransition.Add(inputSymbol, Node.nodes[Node.getaNodeIndex(++usedstates)]);
            singleNode.NextTransition[inputSymbol][0].AddNodeToNextTransitionList(Node.NullTransit,ref sendingLastNode);
          //singleNode.NextTransition[inputSymbol].NextTransition.Add(Node.NullTransit, sendingLastNode);
            return SendNode;
        }
        #endregion

        #region Mid Solvers and Attaching To main
        Node AddNodesAttachMain(Node left,Node right,char operatorabove)
        {
            Console.WriteLine("Solving ADDnodes");

            ///startup Of sending node and adding a head node called
            ///FirstNode (Note this is not head but which will nohave any null transit)
            ///map this first node to null transit with sending node
            ///and start attaching the left and right with sendFirstNode
            Node SendingNode = Node.nodes[Node.getaNodeIndex()];
            Node.ConvertToRealList(ref SendingNode);
            Node SendingLastNode = SendingNode.NextTransition[Node.NullTransit][0];
            Node SendingfirstNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
            SendingNode.NextTransition.Remove(Node.NullTransit);
            SendingNode.AddNodeToNextTransitionList(Node.NullTransit, ref SendingfirstNode);

            ///Now take The left nodes first node and collect all the nodes connected to 
            ///it and attach to the first node of our sendingFirst node
            ///Do it same to the Right node now
            Node leftFirstNode = left.NextTransition[Node.NullTransit][0];
            Node rightFirstNode = right.NextTransition[Node.NullTransit][0];

            foreach(char Transitions in leftFirstNode.NextTransition.Keys)
            {
                foreach(Node nodesConnected in leftFirstNode.NextTransition[Transitions])
                {
                    Node ConnectedNode = nodesConnected;
                    SendingfirstNode.AddNodeToNextTransitionList(Transitions,ref ConnectedNode);
                }
            }
            foreach (char Transitions in rightFirstNode.NextTransition.Keys)
            {
                foreach (Node nodesConnected in rightFirstNode.NextTransition[Transitions])
                {
                    Node ConnectedNode = nodesConnected;
                    SendingfirstNode.AddNodeToNextTransitionList(Transitions, ref ConnectedNode);
                }
            }

            ///Now We have all nodes to be connected to first node conected
            ///Remaining part is to connect alll the nodes which are reaching
            ///To thier respective final nodes to the Final node of sending node
            List<Node> leftFinalNodes = new List<Node>();
            GetAllNodesPointingAtLastNode(left,ref leftFinalNodes);
            List<Node> rightFinalNodes = new List<Node>();
            GetAllNodesPointingAtLastNode(right, ref rightFinalNodes);

            foreach(Node finalNode in leftFinalNodes)
            {
                finalNode.NextTransition.Remove(Node.NullTransit);
                finalNode.AddNodeToNextTransitionList(Node.NullTransit,ref SendingLastNode);
            }
            foreach (Node finalNode in rightFinalNodes)
            {
                finalNode.NextTransition.Remove(Node.NullTransit);
                finalNode.AddNodeToNextTransitionList(Node.NullTransit, ref SendingLastNode);
            }


            return SendingNode;
        }
        Node ConcatNodesAttachMain(Node left, Node right, char operatorabove)
        {
            Console.WriteLine("Solving ConcatNodes");

            ///startup Of sending node and adding a head node called
            ///FirstNode (Note this is not head but which will nohave any null transit)
            ///map this first node to null transit with sending node
            ///and start attaching the left and right with sendFirstNode
            Node SendingNode = Node.nodes[Node.getaNodeIndex()];
            Node.ConvertToRealList(ref SendingNode);
            Node SendingLastNode = SendingNode.NextTransition[Node.NullTransit][0];
            Node SendingfirstNode = Node.nodes[Node.getaNodeIndex(++usedstates)];
            SendingNode.NextTransition.Remove(Node.NullTransit);
            SendingNode.AddNodeToNextTransitionList(Node.NullTransit, ref SendingfirstNode);

            

            //take the first real node (not head node)
            Node leftFirstNode = left.NextTransition[Node.NullTransit][0];
            Node rightFirstNode = right.NextTransition[Node.NullTransit][0];

            //get all nodes which are pointing at last
            List<Node> LeftFinalNodes = new List<Node>();
            GetAllNodesPointingAtLastNode(left, ref LeftFinalNodes);
            List<Node> rightFinalNodes = new List<Node>();
            GetAllNodesPointingAtLastNode(right, ref rightFinalNodes);

            ///So here Concept for Concatenation is Make the usual startup 
            ///1)Then the first node of startup is pointed at leftNode's first's all transition
            ///2)delete the firstnode of left if not required then collect all the nodes pointing at end of left node
            ///3)make them point at rights second node with respective transitions
            ///4) Now we have to cleanup the mess with making all the right Final nodes to point at sending nodes ka final
          
            
            //step 1
            foreach (char Transitions in leftFirstNode.NextTransition.Keys)
            {
                foreach (Node nodesConnected in leftFirstNode.NextTransition[Transitions])
                {
                    Node ConnectedNode = nodesConnected;
                    SendingfirstNode.AddNodeToNextTransitionList(Transitions,ref ConnectedNode);
                }
            }
            //step 2
            foreach(Node FinalNode in LeftFinalNodes)
            {
                FinalNode.NextTransition.Remove(Node.NullTransit);
                foreach (char Transitions in rightFirstNode.NextTransition.Keys)
                {
                    foreach (Node nodesConnected in leftFirstNode.NextTransition[Transitions])
                    {
                        Node conectedNode = nodesConnected;
                        FinalNode.AddNodeToNextTransitionList(Transitions,ref conectedNode);
                    }
                }

            }
            Node.nodes.Remove(rightFirstNode);

            //step 3
            foreach (Node finalNode in rightFinalNodes)
            {
                finalNode.NextTransition.Remove(Node.NullTransit);
                finalNode.AddNodeToNextTransitionList(Node.NullTransit, ref SendingLastNode);
            }

            return SendingNode;
        }
        Node KleinStarNodesAttachMain(Node left, Node right, char operatorabove)
        {
            //TODO Handle ()* in Right
            Console.WriteLine("Solving KlienStarNodes");
            return null;
        }
        Node KleinPlusNodesAttachMain(Node left, Node right, char operatorabove)
        {
            //TODO Handle ()+ in Right
            Console.WriteLine("Solving KlienPlusNodes");
            return null;

        }
        Node SingleAttachMain(Node left, Node right, char operatorabove)
        {
            Console.WriteLine("Solving SingleAttachNodes");
            return null;

        }
        #endregion

        #region Linked List Helper Methods
        Node JoinNodes(Node LeftMost,Node RightMost,char operatorAbove)
        {
            Node Endreslt;
            switch (operatorAbove)
            {
                case '|':Endreslt = AddNodesAttachMain(LeftMost, RightMost, operatorAbove);break;
                case 'P':Endreslt = ConcatNodesAttachMain(LeftMost, RightMost, operatorAbove); break;
                case '*':Endreslt = KleinStarNodesAttachMain(LeftMost, RightMost, operatorAbove); break;
                case '+':Endreslt = KleinPlusNodesAttachMain(LeftMost, RightMost, operatorAbove); break;
                case ')': Endreslt = LeftMost;break;                                                        //Ignore the brackets which are just to group
                default:Endreslt =  SingleAttachMain(LeftMost, RightMost, operatorAbove); break;

            }
            return Endreslt;
        }
        void GetAllNodesPointingAtLastNode(Node MainNode,ref List<Node> ListOfNodes,Node PrevPointNode = null)
        {
            if(MainNode.StateNumber == Node.LASTNODE)
            {
                ListOfNodes.Add(PrevPointNode);
                return;
            }
            
            foreach (char transition in MainNode.NextTransition.Keys)
            {
                foreach(Node TransitionNode in MainNode.NextTransition[transition])
                {
                    GetAllNodesPointingAtLastNode(TransitionNode, ref ListOfNodes, MainNode);
                }
            }
           
        }
        
        #endregion

        public static void Main(string[] args)
        {
            Class1 solv = new Class1();
            Node sol = solv.SolveTheREGEX("abc(a|b)");
            solv.DebugDisplayNode(sol);
            Console.ReadKey();
        }
        public void DebugDisplayNode(Node CurrentNode,Node PrevNode = null)
        {
            if(PrevNode == null)
            {
                Console.WriteLine("Starting Traverse ");
            }
            if(CurrentNode == PrevNode)
            {
                Console.WriteLine("There is a klein transition: ");
                return;
            }
            if(CurrentNode.StateNumber == Node.LASTNODE)
            {
                Console.WriteLine("Last Reached : "+CurrentNode.StateNumber);
                return;
            }
            foreach (char transition in CurrentNode.NextTransition.Keys)
            {
                foreach (Node TransitionNode in CurrentNode.NextTransition[transition])
                {

                    Console.WriteLine("Currently in the Node : "+CurrentNode.StateNumber+" And Transiting to : "+TransitionNode.StateNumber+" By the Character transition: "+transition);
                    DebugDisplayNode(TransitionNode, CurrentNode);
                }
            }

        }
    }
}
