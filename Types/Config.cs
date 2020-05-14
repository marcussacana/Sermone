namespace Sermone.Types
{
    public struct Config
    {
        public string DenyList { get; set; }
        public string IgnoreList { get; set; }
        public string QuoteList { get; set; }
        public string Breakline { get; set; }
        public string AcceptableRanges { get; set; }
        public bool AllowNumbers { get; set; }
        public bool FromAsian { get; set; }
        public int Sensitivity { get; set; }

        public int Language { get; set; }

    }
}
