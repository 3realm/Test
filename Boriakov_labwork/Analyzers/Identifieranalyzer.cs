using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boriakov_labwork.Analyzers
{
    public enum State1 { S, A, F, E }

    internal class Identifieranalyzer : Analyzerfields
    {
        public State1 CurrentState { get; set; }
        public int Index { get; set; }

        public Identifieranalyzer(string str)
            : base(str)
        {
            CurrentState = State1.S;
            Index = 0;
        }

        public void Analyze(ref string errorMessage)
        {
            while (CurrentState != State1.F && CurrentState != State1.E) 
            {
                symbol = str[Index];
                Index++;

                switch (CurrentState)
                {
                    case State1.S:
                        if (Char.IsLetter(symbol) == true || symbol == '_')
                        {
                            CurrentState = State1.A;
                        }
                        else
                        {
                            CurrentState = State1.E;
                            errorMessage = "Ожидалась буква или _";
                        }
                        break;
                    case State1.A:
                        if (Char.IsLetter(symbol) == true || Char.IsDigit(symbol) == true && Char.IsPunctuation(symbol) == false)
                        {
                            CurrentState = State1.A;
                        }
                        else if (symbol == _terminalSymbol)
                        {
                            CurrentState = State1.F;
                        }
                        else
                        {
                            CurrentState = State1.E;
                            errorMessage = "Ожидался индетификатор в нормальной форме";
                        }
                        break;
                    case State1.F:
                        break;
                    case State1.E:
                        break;
                }
            }
        }
    }
}
