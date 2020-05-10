using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sermone.Types
{
    public struct Config
    {
        public string DenyList;
        public string IgnoreList;
        public string QuoteList;
        public string Breakline;
        public string AcceptableRanges;
        public bool AllowNumbers;
        public bool FromAsian;
        public int Sensitivity;

        public int Language;

    }
}
