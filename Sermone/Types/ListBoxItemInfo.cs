namespace Sermone.Types
{
    public struct ListBoxItemInfo
    {
        public bool Visible;
        public int? VisibleIndex;
        public bool? VirtualChecked;
        public bool Checked;
        public string Value;
        public string SubValue;

        public ListBoxItemInfo(bool Checked, bool Visible, string Value, string SubValue) {
            this.Checked = Checked;
            this.Visible = Visible;
            this.Value = Value;
            this.SubValue = SubValue;
            VisibleIndex = null;
            VirtualChecked = null;
        }
    }
}
