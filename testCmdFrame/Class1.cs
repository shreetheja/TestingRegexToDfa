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
        public char PrevTransitionVal;
        public char NextTransiionVal;
        public Node PrevNode;
        public Node NextNode;
        public static int FIRSTNODE = -1;
        public static int LASTNODE = -1;
        public static char epsilon = '$';
        public static char add = '|';
        public static char kleinStar = '*';
        public static char kleinPlus = '+';
        public static char openBrace = '(';
        public static char closeBrace = ')';
        public static char EndofLine = '~';
    }
    class Class1
    {
        public List<char> TerminalValue = new List<char>();
        public Node solvedNode = new Node();
        Class1(string input)
        {
            if (ValidateAndInitialiseInput(input))
            {
                solvedNode.StateNumber = Node.FIRSTNODE;
                solvedNode.PrevTransitionVal = Node.epsilon;
                solvedNode.NextTransiionVal = Node.epsilon;
                Solve(input,Node.EndofLine);
            }
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
        private void Solve(string input,char operatorAbove)
        {
            char sameSymbol;
            if(IFAllSameSymbols(input.ToCharArray(),out sameSymbol))
            {
                switch(sameSymbol)
                {
                    case '|':Addsymbols(input.ToCharArray(),operatorAbove);return;
                    case '+': KlienPlussmbols(input.ToCharArray(),operatorAbove); return;
                    case '*':KlienStarSymbols(input.ToCharArray(),operatorAbove); return;
                    case 'T':                                                        //terminal value
                    default:ProductSymbols(input.ToCharArray(),operatorAbove);return;
                }
            }
            string[] Splitinput = SplitTokens(input,ref operatorAbove);
            if(Splitinput[0]!=null)
            Solve(Splitinput[0],operatorAbove);
            if (Splitinput[1] != null)
            Solve(Splitinput[1],operatorAbove);

        }
        private void solveBasic(string input,char symbol)
        {
            Console.WriteLine("Solveing" + input);
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
                    returningArray[0] = characterArrayToString(inputCharacters, i, true);
                    returningArray[1] = characterArrayToString(inputCharacters, i, false);
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

            TryBracketSplit(inputCharacters, out returningArray,ref operatorHere);


            return returningArray;
        }
        #endregion


        #region Helper Methods Of Split Tokens
        bool IFAllSameSymbols(char[] characters,out char sameSymb)
        {
            char SameSymbol = 'N';
            bool foundSymbol = false;
            bool foundTerminalOnce = false;
            if(characters.Length<2&&TerminalValue.Contains(characters[0]))              //terminal only
            {
                sameSymb = 'T';
                return true;
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
                        if(SameSymbol=='N')                     //check only product
                        {
                            foundSymbol = true;
                            sameSymb = 'p';
                        }
                        else
                        {
                            sameSymb = 'P';
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
            }
            sameSymb = SameSymbol;
            return true;
        }
        void TryBracketSplit(char[] inputCharacters, out string[] output,ref char operatorabove)
        {
            output = new string[100];
            if(inputCharacters[0] == '(')
            {
               
                if(inputCharacters[inputCharacters.Length-1]==')')
                {
                    //TODO Solve End
                    return;
                }
                else
                {
                    int i;
                    for (i = 0; i < inputCharacters.Length; i++)
                    {
                        if (inputCharacters[i] == ')')
                        {
                            output[0] = characterArrayToString(inputCharacters, i+1,true);
                            break;
                        }

                    }
                    output[1] = characterArrayToString(inputCharacters, i, false);
                    return;
                }
            }
            for (int i = 0; i < inputCharacters.Length; i++)
            {
                if (inputCharacters[i] == '(')
                {
                    output[0] = characterArrayToString(inputCharacters, i, true);
                    output[1] = characterArrayToString(inputCharacters, i-1, false);
                }

            }
        }
    
        #endregion

        #region custom string Methods
        string characterArrayToString(char[] array,int index,bool InclusivebeforeIndex)
        {
            string returnString = "";
            if (InclusivebeforeIndex)
            {
                for (int i = 0; i < index; i++)
                {
                    returnString += array[i];
                }
            }
            else
            {
                for (int i = index+1; i <array.Length; i++)
                {
                    returnString += array[i];
                }
            }
            return returnString;
        }
        #endregion


        #region baseSolvers
        void Addsymbols(char[] inputSymbols, char operatorAbove)
        {
            Console.WriteLine("Adding : " + inputSymbols);
        }
        void ProductSymbols(char[] inputSymbols, char operatorAbove)
        {
            Console.WriteLine("Conscatenating : " + inputSymbols);
        }
        void KlienStarSymbols(char[] inputSymbols, char operatorAbove)
        {
            Console.WriteLine("klienstaring : " + inputSymbols);
        }
        void KlienPlussmbols(char[] inputSymbols, char operatorAbove)
        {
            Console.WriteLine("Klien plusing : " + inputSymbols);
        }
        #endregion

        public static void Main(string[] args)
        {
            Class1 solv = new Class1("a|b|abc");
            Console.ReadKey();
        }
    }
}
