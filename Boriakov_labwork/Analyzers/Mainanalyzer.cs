using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boriakov_labwork.Analyzers
{
    public enum StateMain { S, A, B, C, D, G, H, I, J, K, L, M, N, O, P, Q, R, T, U, V, W, F, E }

    internal class Mainanalyzer : Analyzerfields
    {

        public StateMain CurrentState { get; set; }
        public int Index { get; set; }
        public List<string> Table { get; set; }

        public int Number_names { get; set; }

        public List<string> Name { get; set; }
        public List<string> Type { get; set; }
        public List<string> Dimension { get; set; }
        public List<string> Volume { get; set; }

        public Mainanalyzer(string str)
            : base(str)
        {
            CurrentState = StateMain.S;
            Index = 0;
        }


        public void MainAnalyze(ref string errorMessage) 
        {
            string text = "";
            string table_text = "";

            int mnogitel = 1;
            int begunok = 0;
            
            Table = new List<string>();

            Name = new List<string>();
            Type = new List<string>();
            Dimension = new List<string>();
            Volume = new List<string>();

            List<string> wortList = new List<string>();

            List<int> countList = new List<int>();

            List<string> list = new List<string>() 
            { 
                "var",
                "word", 
                "integer",
                "real",
                "char",
                "double",
                "array",
                "..",
                "of",
                "byte"
            };

            

            string s1 = "var";            
            string s2 = "word";
            string s3 = "integer";
            string s4 = "real";
            string s5 = "char";
            string s6 = "double";
            string s7 = "array";
            string s8 = "..";
            string s9 = "of";
            string s10 = "byte";



            while (CurrentState != StateMain.F && CurrentState != StateMain.E)
            {
                symbol = str[Index];
                Index++;

                switch (CurrentState)
                {
                    // S
                    case StateMain.S:
                        if (symbol == 'v' && str[Index] == 'a' && str[Index + 1] == 'r')
                        {
                            Index = Index + 2;
                            CurrentState = StateMain.A;
                        }
                        else if (symbol == 'V' && str[Index] == 'A' && str[Index+1] == 'R')
                        {
                            Index = Index + 2;
                            CurrentState = StateMain.A;
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.S;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидается VAR";
                        }

                        text = "";
                        break;

                    
                    // C
                    case StateMain.C:
                        if (symbol == ':')
                        {
                            CurrentState = StateMain.D;
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.C;
                        }
                        else if (symbol == ',')
                        {
                            // счетчик 
                            Number_names++;
                            //
                            CurrentState = StateMain.B;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалось : или ,";
                        }
                        break;

                    // D
                    case StateMain.D:
                        string t_errorMessage_D = "";
                        
                        // ARRAY
                        if (symbol == 'A' && str[Index] == 'R' && str[Index + 1] == 'R' && str[Index + 2] == 'A' && str[Index + 3] == 'Y')
                        {
                            Index = Index + 4;
                            // LABEL
                            table_text += "                    " + "ARRAY ";
                            //
                            begunok = 2;
                            CurrentState = StateMain.G;
                        }
                        // 
                        else if (symbol == 'a' && str[Index] == 'r' && str[Index + 1] == 'r' && str[Index + 2] == 'a' && str[Index + 3] == 'y')
                        {
                            Index = Index + 4;
                            // LABEL
                            table_text += "                    " + "ARRAY ";
                            //
                            begunok = 2;
                            CurrentState = StateMain.G;
                        }
                        // 
                        else
                        {
                            bool key_D = true;
                            // вырез простого типа
                            while (key_D == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ';')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];
                            }
                            //

                            Index--;                           

                            // множитель
                            switch (text)
                            {
                                case ("BYTE"):
                                    mnogitel = 1;
                                    break;

                                case ("WORD"):
                                    mnogitel = 2;
                                    break;

                                case ("INTEGER"):
                                    mnogitel = 4;
                                    break;

                                case ("CHAR"):
                                    mnogitel = 1;
                                    break;

                                case ("REAL"):
                                    mnogitel = 8;
                                    break;

                                default:
                                    break;
                            }

                            // LABEL
                            table_text += "                            " + text + "                                   " + mnogitel;
                            // 

                            for (int i = 0; i <= Number_names; i++)
                            {
                                Type.Add(text);
                                Dimension.Add("---");
                                Volume.Add(mnogitel.ToString());
                            }

                            // простой тип
                            text += _terminalSymbol;

                            Simpletypeanalyzer simpletypeanalyzer_D = new Simpletypeanalyzer(text);

                            simpletypeanalyzer_D.Analyze(ref t_errorMessage_D);

                            if (simpletypeanalyzer_D.CurrentState != StateSimple.E)
                            {

                                CurrentState = StateMain.Q;
                            }
                            else
                            {
                                CurrentState = StateMain.E;
                                errorMessage = "Ожидалось ARRAY или\n" + t_errorMessage_D;
                            }
                            //



                            Number_names = 0;
                            text = "";
                        }
                        break;

                    // G
                    case StateMain.G:
                        if (symbol == '[')
                        {
                            CurrentState = StateMain.I;
                        }
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.H;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалcя пробел или [";
                        }
                        break;

                    // H
                    case StateMain.H:
                        if (symbol == '[')
                        {
                            CurrentState = StateMain.I;
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.H;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалcя пробел или [";
                        }
                        break;

                    // I
                    case StateMain.I:
                        // петля
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.I;
                        }
                        //
                        else
                        {
                            string t_errorMessage_I = "";

                            bool key_I = true;
                            // вырез константы
                            while (key_I == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ']' || symbol == '.')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];

                            }
                            //

                            Index--;

                            string text_copy_I = text;

                            // константа 1
                            text += _terminalSymbol;

                            Constantanalyzer constantanalyzer = new Constantanalyzer(text);

                            constantanalyzer.Analyze(ref t_errorMessage_I);

                            if (constantanalyzer.CurrentState != State.E)
                            {
                                CurrentState = StateMain.J;

                                int r1 = text_copy_I.IndexOf('+');
                                // заносим константу в список для дальнейших проверок и операций
                                if (r1 == -1)
                                {

                                    countList.Add(int.Parse(text_copy_I));
                                }
                                else
                                {
                                    text_copy_I.Remove(0);
                                    countList.Add(int.Parse(text_copy_I));
                                }

                                // проверка на диапазоном
                                if (countList[countList.Count - 1] < -32768 || countList[countList.Count - 1] > 32767)
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Целая константа находится за допустимым диапазоном";
                                    text_copy_I = "0";
                                }
                                //

                            }
                            else
                            {
                                CurrentState = StateMain.E;
                                errorMessage = t_errorMessage_I;
                            }
                            //



                            text = "";                            
                        }                        
                        break;

                    // J
                    case StateMain.J:
                        if (symbol == '.' && str[Index] == '.')
                        {
                            Index = Index + 1;
                            CurrentState = StateMain.K;
                        }
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.J;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалось ..";
                        }

                        text = "";
                        break;

                    // K
                    case StateMain.K:                        
                        // петля
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.K;
                        }
                        else
                        {
                            // константа 2
                            string t_errorMessage_K = "";

                            bool key_K = true;
                            // вырез константы
                            while (key_K == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ']' || symbol == '.' || symbol == ',')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];

                            }
                            //
                            
                            Index--;

                            string text_copy_K = text;

                            text += _terminalSymbol;

                            Constantanalyzer constantanalyzer = new Constantanalyzer(text);

                            constantanalyzer.Analyze(ref t_errorMessage_K);

                            if (constantanalyzer.CurrentState != State.E)
                            {
                                CurrentState = StateMain.L;

                                // проверки констант
                                int r1 = text_copy_K.IndexOf('+');

                                if (r1 == -1)
                                {

                                    countList.Add(int.Parse(text_copy_K));
                                }
                                else
                                {
                                    text_copy_K.Remove(0);
                                    countList.Add(int.Parse(text_copy_K));
                                }

                                if (countList[countList.Count - 1] < -32768 || countList[countList.Count - 1] > 32767)
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Целые константы находятся за допустимым диапазоном";
                                }
                                
                                if (countList[countList.Count - 2] > countList[countList.Count - 1])
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Начальная константа больше конечной";
                                }
                                //
                            }
                            else
                            {
                                CurrentState = StateMain.E;
                                errorMessage = t_errorMessage_K;
                            }

                            

                            text = "";
                        }
                        break;

                    // L --> U
                    case StateMain.L:
                        if (symbol == ']')
                        {
                            CurrentState = StateMain.M;
                        }
                        // петля
                        else if(Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.L;
                        }
                        else if (symbol == ',') 
                        {
                            CurrentState = StateMain.T;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалcя пробел или ] или ,";
                        }
                        break;
                    // M
                    case StateMain.M:
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.N;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидался пробел";
                        }
                        break;
                    // N
                    case StateMain.N:

                        if (symbol == 'O' && str[Index] == 'F')
                        {
                            Index = Index + 1;
                            CurrentState = StateMain.O;
                        }
                        else if (symbol == 'o' && str[Index] == 'f')
                        {
                            Index = Index + 1;
                            CurrentState = StateMain.O;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидался OF";
                        }

                        text = "";
                        break;
                    // O
                    case StateMain.O:
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.P;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидался пробел";
                        }
                        break;

                    // P
                    case StateMain.P:
                        string t_errorMessage_P = "";

                        bool key = true;
                        // вырез простого типа
                        while (key == true)
                        {
                            if (symbol == ' ' || symbol == _terminalSymbol || symbol == ';')
                            {
                                break;
                            }

                            text += symbol;
                            Index++;
                            symbol = str[Index - 1];
                        }
                        //

                        Index--;

                        // заносим в LIST
                        table_text += text + "                      ";
                        //

                        for (int i = 0; i <= Number_names; i++)
                        {

                        }

                        // множитель
                        switch (text)
                        {
                            case ("BYTE"):
                                mnogitel = 1;
                                break;

                            case ("WORD"):
                                mnogitel = 2;
                                break;

                            case ("INTEGER"):
                                mnogitel = 4;
                                break;

                            case ("CHAR"):
                                mnogitel = 1;
                                break;

                            case ("REAL"):
                                mnogitel = 8;
                                break;

                            default:
                                break;
                        }
                        //

                        // рассчёт памяти
                        int summa = 0;

                        if (begunok == 10)
                        {
                            summa = ((countList[countList.Count - 1] - countList[countList.Count - 2]) * (countList[countList.Count - 3] - countList[countList.Count - 4]) * mnogitel);
                            table_text += summa.ToString();
                        }
                        else
                        {
                            begunok = 4;
                            summa = (countList[countList.Count - 1] - countList[countList.Count - 2]) * mnogitel;
                            table_text += summa.ToString();
                        }
                        //

                        // занос в LABEL
                        Table.Add(table_text);
                        //

                        for (int i = 0; i <= Number_names; i++)
                        {
                            Type.Add("ARRAY OF " + text);
                            if (begunok == 10) 
                            {
                                Dimension.Add("2");
                            }
                            else
                            {
                                Dimension.Add("1");
                            }    
                            Volume.Add(summa.ToString());
                        }

                        // простой тип
                        text += _terminalSymbol;

                        Simpletypeanalyzer simpletypeanalyzer = new Simpletypeanalyzer(text);

                        simpletypeanalyzer.Analyze(ref t_errorMessage_P);

                        if (simpletypeanalyzer.CurrentState != StateSimple.E)
                        {
                            CurrentState = StateMain.Q;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = t_errorMessage_P;
                        }
                        //



                        Number_names = 0;
                        table_text = "";
                        text = "";
                        break;

                    // Q
                    case StateMain.Q:
                        if (symbol == ';')
                        {
                            CurrentState = StateMain.R;
                            if (begunok == 1)
                            {
                                // занос в LABEL
                                Table.Add(table_text);
                                //
                            }
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.Q;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалcя пробел или ;";
                        }



                        table_text = "";
                        break;

                    // R
                    case StateMain.R:
                        string t_errorMessage_R = "";

                        if (symbol == _terminalSymbol)
                        {
                            CurrentState = StateMain.F;
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.R;
                        }
                        // индетификатор
                        else
                        {
                            bool key_B = true;

                            // вырез индентификатора
                            while (key_B == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ':' || symbol == ',')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];
                            }
                            //


                            // проверка
                            bool key_E_R = true;

                            if (wortList.Contains(text))
                            {
                                errorMessage = "Семантическая ошибка\n" + "Дублирование имен\n";
                                CurrentState = StateMain.E;
                                key_E_R = false;
                                Index--;
                            }
                            //

                            // VAR s,q:ARRAY [12..23] OF word; e:ARRAY [12..23] OF word;
                            wortList.Add(text);

                            // заносим в LIST
                            table_text += text + "   ";
                            //

                            Name.Add(text);

                            text = text.ToLower();

                            for (int i = 0; i < list.Count; i++)
                            {
                                if (string.Compare(text, list[i]) == 0)
                                {
                                    errorMessage += "Семантическая ошибка\n" + "Совпадение с ключевами словами\n";
                                    CurrentState = StateMain.E;
                                    key_E_R = false;
                                    Index--;
                                    break;
                                }
                            }

                            if (text.Length > 8)
                            {
                                errorMessage += "Семантическая ошибка\n" + "Идентификатор большей длины\n";
                                CurrentState = StateMain.E;
                                key_E_R = false;
                                Index--;
                            }

                            if (key_E_R == true) 
                            {
                                Index--;

                                text += _terminalSymbol;

                                Identifieranalyzer identifieranalyzer = new Identifieranalyzer(text);

                                identifieranalyzer.Analyze(ref t_errorMessage_R);

                                if (identifieranalyzer.CurrentState != State1.E)
                                {
                                    CurrentState = StateMain.C;
                                }
                                else
                                {
                                    errorMessage = t_errorMessage_R;
                                    CurrentState = StateMain.E;
                                }
                            }
                            //



                            text = "";
                        }
                        break;
                    case StateMain.T:
                        // петля
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.T;
                        }
                        //
                        else
                        {
                            string t_errorMessage_T = "";

                            bool key_T = true;
                            // вырез константы
                            while (key_T == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ']' || symbol == '.')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];

                            }
                            //

                            Index--;

                            string text_copy_I = text;

                            // константа 3
                            text += _terminalSymbol;

                            Constantanalyzer constantanalyzer = new Constantanalyzer(text);

                            constantanalyzer.Analyze(ref t_errorMessage_T);

                            if (constantanalyzer.CurrentState != State.E)
                            {
                                CurrentState = StateMain.U;

                                int r1 = text_copy_I.IndexOf('+');
                                // заносим константу в список для дальнейших проверок и операций
                                if (r1 == -1)
                                {

                                    countList.Add(int.Parse(text_copy_I));
                                }
                                else
                                {
                                    text_copy_I.Remove(0);
                                    countList.Add(int.Parse(text_copy_I));
                                }

                                // проверка на диапазоном
                                if (countList[countList.Count - 1] < -32768 || countList[countList.Count - 1] > 32767)
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Целая константа находится за допустимым диапазоном";
                                    t_errorMessage_T = "0";
                                }
                                //

                            }
                            else
                            {
                                CurrentState = StateMain.E;
                                errorMessage = t_errorMessage_T;
                            }
                            //



                            text = "";
                        }
                        break;

                    case StateMain.U:
                        if (symbol == '.' && str[Index] == '.')
                        {
                            Index = Index + 1;
                            CurrentState = StateMain.V;
                        }
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.U;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалось ..";
                        }



                        text = "";
                        break;

                    case StateMain.V:
                        // петля
                        if (Char.IsWhiteSpace(symbol) == true)
                        {
                            CurrentState = StateMain.V;
                        }
                        else
                        {
                            // константа 2
                            string t_errorMessage_V = "";

                            bool key_V = true;
                            // вырез константы
                            while (key_V == true)
                            {
                                if (symbol == ' ' || symbol == _terminalSymbol || symbol == ']' || symbol == '.')
                                {
                                    break;
                                }

                                text += symbol;
                                Index++;
                                symbol = str[Index - 1];

                            }
                            //

                            Index--;

                            string text_copy_K = text;

                            text += _terminalSymbol;

                            Constantanalyzer constantanalyzer = new Constantanalyzer(text);

                            constantanalyzer.Analyze(ref t_errorMessage_V);

                            if (constantanalyzer.CurrentState != State.E)
                            {
                                CurrentState = StateMain.W;

                                // проверки констант
                                int r1 = text_copy_K.IndexOf('+');

                                if (r1 == -1)
                                {

                                    countList.Add(int.Parse(text_copy_K));
                                }
                                else
                                {
                                    text_copy_K.Remove(0);
                                    countList.Add(int.Parse(text_copy_K));
                                }

                                if (countList[countList.Count - 1] < -32768 || countList[countList.Count - 1] > 32767)
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Целые константы находятся за допустимым диапазоном";
                                }

                                if (countList[countList.Count - 2] > countList[countList.Count - 1])
                                {
                                    CurrentState = StateMain.E;
                                    errorMessage = "Семантическая ошибка\n" + "Начальная константа больше конечной";
                                }
                                //
                            }
                            else
                            {
                                CurrentState = StateMain.E;
                                errorMessage = t_errorMessage_V;
                            }



                            text = "";
                        }

                        break;
                    case StateMain.W:
                        if (symbol == ']')
                        {
                            CurrentState = StateMain.M;
                            begunok = 10;
                        }
                        // петля
                        else if (Char.IsWhiteSpace(symbol) == true)
                        {                            
                            CurrentState = StateMain.W;
                        }
                        else
                        {
                            CurrentState = StateMain.E;
                            errorMessage = "Ожидалcя пробел или ]";
                        }
                        break;
                    case StateMain.F:
                        break;
                    case StateMain.E:
                        break;
                    default:
                        break;
                }

            }

            

        }     
    }
}
