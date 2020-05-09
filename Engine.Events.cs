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
            DialogueBox.Refresh(ID);
        }

        public async static Task SelectChanged(int NewID)
        {
            int OldID = DialogueBox.SelectedIndex;
            CurrentDialogue = DialogueBox.Items[NewID].Value;
            DialogueBox.SelectedIndex = NewID;
            DialogueBox.Refresh(NewID);
            DialogueBox.Refresh(OldID);
            EditorBox.Refresh();
        }

        public async static Task UpdateDialogue()
        {
            DialogueBox.Items[DialogueBox.SelectedIndex].Value = CurrentDialogue;
            DialogueBox.Refresh(DialogueBox.SelectedIndex);

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
