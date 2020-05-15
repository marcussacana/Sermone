namespace Sermone.Types
{
    public struct ListBoxItemInfo
    {
        public bool Visible;
        public int? VisibleIndex;
        public bool? VirtualChecked;
        public bool Checked;
        public string Value;

        public ListBoxItemInfo(bool Checked, bool Visible, string Value) {
            this.Checked = Checked;
            this.Visible = Visible;
            this.Value = Value;
            VisibleIndex = null;
            VirtualChecked = null;
        }
    }
}
