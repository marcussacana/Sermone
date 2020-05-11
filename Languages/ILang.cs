using Microsoft.AspNetCore.Components.Rendering;
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
        public string Loading { get; }
        public string LoadingDesc { get; }
        public string RefreshingDesc { get; }
        public string PluginList { get; }

        //Buttons
        public string Next { get; }
        public string Back { get; }

        //About
        public string AboutVersion { get; }
        public string AboutDesc { get; }

        //Notifications
        public string BackgroundTaskWarning { get; }
        public string NoResultsFound { get; }
        public string TryOthersWords { get; }

        //Settings
        public string Settings { get; }
        public string Language { get; }
        public string DennyPattern { get; }
        public string IgnorePattern { get; }
        public string QuotesPattern { get; }
        public string Breakline { get; }
        public string AcceptableRange { get; }
        public string AsianMode { get; }
        public string Sensitivity { get; }
    }
}
