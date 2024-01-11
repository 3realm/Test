using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boriakov_labwork.Analyzers
{
    public enum State { S, A, B, C, F, E }

    internal class Constantanalyzer : Analyzerfields 
    {
        public State CurrentState { get; set; }
        public int Index { get; set; }

        public Constantanalyzer(string str)
            : base(str)
        {
            CurrentState = State.S;
            Index = 0;
        }

        public void Analyze(ref string errorMessage)
        {
            while (CurrentState != State.F && CurrentState != State.E)
            {
                symbol = str[Index];
                Index++;

                switch (CurrentState)
                {
                    case State.S:
                        if (symbol == '+' || symbol == '-')
                        {
                            CurrentState = State.A;
                        }
                        else if (Char.IsDigit(symbol) == true && symbol != '0')
                        {
                            CurrentState = State.B;
                        }
                        else if (symbol == '0')
                        {
                            CurrentState = State.C;
                        }
                        else 
                        {
                            CurrentState = State.E;
                            errorMessage = "Ожидалась цифра или знак +/-";
                        }
                        break;

                    case State.A:
                        if (Char.IsDigit(symbol) == true && symbol != '0')
                        {
                            CurrentState = State.B;
                        }
                        else
                        {
                            CurrentState = State.E;
                            errorMessage = "Ожидалась любая цифра кроме нуля";
                        }
                        break;

                    case State.B:
                        if (Char.IsDigit(symbol) == true)
                        {
                            CurrentState = State.B;
                        }
                        else if (symbol == _terminalSymbol)
                        {
                            CurrentState = State.F;
                        }
                        else
                        {
                            CurrentState = State.E;
                            errorMessage = "Ожидалась цифра";
                        }
                        break;

                    case State.C:
                        if (symbol == _terminalSymbol)
                        {
                            CurrentState = State.F;
                        }
                        else
                        {
                            CurrentState = State.E;
                            errorMessage = "Ожидался конец цепочки";
                        }
                        break;

                    case State.F:
                        break;
                    case State.E:
                        break;                    
                }                
            }
        }
    }
}
