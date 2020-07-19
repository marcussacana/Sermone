namespace Sermone.Types
{
    public struct Config
    {
        public int SettingsVersion { get; set; }

        public string DenyList { get; set; }
        public string IgnoreList { get; set; }
        public string QuoteList { get; set; }
        public string Breakline { get; set; }
        public string AcceptableRanges { get; set; }

        public string CustomCSS { get; set; }
        public int SelectionMode { get; set; }
        public bool AllowNumbers { get; set; }
        public bool FromAsian { get; set; }
        public int Sensitivity { get; set; }

        public int Language { get; set; }

        
        //Paste
        public int BackupOn { get; set; }
        public string PasteUsername { get; set; }
        public string PastePassword { get; set; }
        public int PasteClient { get; set; }
    }
}
