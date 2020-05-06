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

        //Nav Bar
        public string Search { get; }

        //Loading Page
        public string PluginList { get; }
        public string LoadingPlugin { get; }
        public string LoadingDesc { get; }

        //Buttons
        public string Next { get; }
        public string Back { get; }
    }
}
