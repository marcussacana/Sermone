using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sermone
{
    static partial class Engine
    {
        public async static Task DoSearch(string Value)
        {

        }

        public async static Task CheckedChanged(int ID, bool Checked)
        {
            DialogueBox.Items[ID].Checked = Checked;
        }

        public async static Task SelectChanged(int NewID)
        {
            CurrentDialogue = DialogueBox.Items[NewID].Value;
            DialogueBox.SelectedIndex = NewID;
            DialogueBox.Refresh();
            EditorBox.Refresh();
        }

        public async static Task UpdateDialogue()
        {
            DialogueBox.Items[DialogueBox.SelectedIndex].Value = CurrentDialogue;
            int Next = DialogueBox.SelectedIndex + 1;
            while (Next < DialogueBox.Items.Length && !DialogueBox.Items[Next].Checked)
                Next++;
            if (Next < DialogueBox.Items.Length)
            {
                await SelectChanged(Next);
                return;
            }
        }
    }
}
