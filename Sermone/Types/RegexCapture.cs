namespace Sermone.Types
{
    public struct RegexCapture
    {
        public int Index { get; init; }
        public int Length { get; init; }

        public string Value { get; init; }
        public int EndIndex => Index + Length;

        public RegexCapture(int Index, int Length, string Value)
        {
            this.Index = Index;
            this.Length = Length;
            this.Value = Value;
        }
    }
}
