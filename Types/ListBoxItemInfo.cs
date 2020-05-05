namespace Sermone.Types
{
    public struct ListBoxItemInfo
    {
        public bool Checked;
        public string Value;

        public ListBoxItemInfo(bool Checked, string Value) {
            this.Checked = Checked;
            this.Value = Value;
        }
    }
}
