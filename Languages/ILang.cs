using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sermone.Languages
{
    public interface ILang
    {
        public string EnglishLanguageName { get; }
        public string NativeLanguageName { get; }

        //Dropdown
        public string File { get; }
        public string Open { get; }
        public string Save { get; }


    }
}
