using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boriakov_labwork.Analyzers
{
    public enum StateSimple { S, A, F, E }

    internal class Simpletypeanalyzer : Analyzerfields
    {
        public StateSimple CurrentState { get; set; }
        public int Index { get; set; }

        public Simpletypeanalyzer(string str)
            : base(str)
        {
            CurrentState = StateSimple.S;
            Index = 0;
        }



        public void Analyze(ref string errorMessage)
        {
            string text = "";

            string s1 = "byte";
            string s2 = "word";
            string s3 = "integer";
            string s4 = "real";
            string s5 = "char";
            string s6 = "double";

            s1 = s1.ToUpper();
            s2 = s2.ToUpper();
            s3 = s3.ToUpper();
            s6 = s6.ToUpper();
            s4 = s4.ToUpper();
            s5 = s5.ToUpper();

            while (str[Index] != '?')
            {
                text += str[Index];
                Index++;
            }

            if (Index == 0) 
            {
                CurrentState = StateSimple.E;
                errorMessage = "Ожидался простой тип";
            }

            while (CurrentState != StateSimple.F && CurrentState != StateSimple.E)
            {
                symbol = str[Index - 1];
                Index++;

                switch (CurrentState)
                {
                    case StateSimple.S:
                        if (string.Compare(text, s1) == 0 || string.Compare(text, s2) == 0 || string.Compare(text, s3) == 0 || string.Compare(text, s4) == 0 || string.Compare(text, s5) == 0 || string.Compare(text, s6) == 0)
                        {
                            CurrentState = StateSimple.A;
                        }
                        else
                        {
                            CurrentState = StateSimple.E;
                            errorMessage = "Ожидался простой тип";
                        }
                        break;

                    case StateSimple.A:
                        if (symbol == _terminalSymbol)
                        {
                            CurrentState = StateSimple.F;
                        }
                        else
                        {
                            CurrentState = StateSimple.E;
                            errorMessage = "Ожидался простой тип";
                        }
                        break;

                    case StateSimple.F:
                        break;
                    case StateSimple.E:
                        break;
                    default:
                        break;
                }


            }
        }
    }
}
