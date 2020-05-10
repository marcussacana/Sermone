using Sermone.Tools;
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
            string Query = Value.Minify();

            if (CurrentSearch != Query)
            {
                CurrentSearch = Query;
                if (string.IsNullOrEmpty(Query))
                {
                    for (int i = 0; i < DialogueBox.Items.Length; i++)
                    {
                        if (DialogueBox.Items[i].VirtualChecked != null)
                        {
                            DialogueBox.Items[i].VirtualChecked = null;
                            DialogueBox.Refresh(i);
                        }
                    }
                    return;
                }

                await SelectChanged(0);

                for (int i = 0; i < DialogueBox.Items.Length; i++)
                {
                    if (i % 50 == 0)
                        await DoEvents();

                    bool Match = DialogueBox.Items[i].Checked && DialogueBox.Items[i].Value.Minify().Contains(Query);
                    DialogueBox.Items[i].VirtualChecked = Match;
                    DialogueBox.Refresh(i);
                }
            }
            else if (!string.IsNullOrEmpty(Query))
                await SelectChanged(DialogueBox.SelectedIndex+1);

            if (string.IsNullOrEmpty(Query))
                return;

            if (!DialogueBox.Items[DialogueBox.SelectedIndex].VirtualChecked ?? true)
                await NextDialogue();
        }

        public static void CheckedChanged(int ID, bool Checked)
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
            await DialogueBox.EnsureItemVisible(NewID);
            EditorBox.Refresh();
        }

        public async static Task UpdateDialogue()
        {
            DialogueBox.Items[DialogueBox.SelectedIndex].Value = CurrentDialogue;
            DialogueBox.Refresh(DialogueBox.SelectedIndex);

            await NextDialogue();
        }
        
        public async static Task BackDialogue() {
            int Back = DialogueBox.SelectedIndex - 1;
            if (Back < 0 || Back > DialogueBox.Items.Length)
                Back = 0;

            while (Back > 0 && !(DialogueBox.Items[Back].VirtualChecked ?? DialogueBox.Items[Back].Checked))
                Back--;

            if (Back < DialogueBox.Items.Length)
            {
                await SelectChanged(Back);
                return;
            }
        }

        public async static Task NextDialogue() {
            int Next = DialogueBox.SelectedIndex + 1;
            if (Next < 0 || Next > DialogueBox.Items.Length)
                Next = 0;

            while (Next < DialogueBox.Items.Length && !(DialogueBox.Items[Next].VirtualChecked ?? DialogueBox.Items[Next].Checked))
                Next++;

            if (Next < DialogueBox.Items.Length)
            {
                await SelectChanged(Next);
                return;
            }
        }
    }
}
