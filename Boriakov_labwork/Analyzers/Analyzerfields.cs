using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boriakov_labwork.Analyzers
{
    class Analyzerfields
    {
        protected string str;
        protected char symbol;
        protected int index;
        protected const char _terminalSymbol = '?';

        public Analyzerfields(string _str) 
        {
            str = _str + _terminalSymbol;
        }
    }
}
