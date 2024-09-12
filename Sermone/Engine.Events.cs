using Sermone.Tools;
using System.Text.RegularExpressions;

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

                bool Found = false;
                await SelectChanged(0);

                for (int i = 0; i < DialogueBox.Items.Length; i++)
                {
                    if (i % 50 == 0)
                        await DoEvents();

                    bool Match = DialogueBox.Items[i].Checked && DialogueBox.Items[i].Value.Minify().Contains(Query);
                    if (Match)
                        Found = true;

                    DialogueBox.Items[i].VirtualChecked = Match;
                    DialogueBox.Refresh(i);
                }

                if (!Found) {
                    Toast.ShowInfo(Language.TryOthersWords);
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(Query))
                await SelectChanged(DialogueBox.SelectedIndex+1);

            if (string.IsNullOrEmpty(Query))
                return;

            if (!DialogueBox.Items[DialogueBox.SelectedIndex].VirtualChecked ?? true)
                if (!await NextDialogue()) {
                    await SelectChanged(0);
                    await NextDialogue();
                }
        }

        public static void CheckedChanged(int ID, bool Checked)
        {
            DialogueBox.Items[ID].Checked = Checked;
            DialogueBox.Refresh(ID);
        }

        public async static Task SelectChanged(int NewID)
        {
            int OldID = DialogueBox.SelectedIndex;
            var NewValue = DialogueBox.Items[NewID].Value;

            CurrentDialogueUnfiltered = null;

            if (!string.IsNullOrWhiteSpace(Settings.RegexFilter))
            {
                var Result = NewValue.Match(Settings.RegexFilter);

                if (Result.Any())
                {
                    var Capture = Result.First();

                    CurrentDialogueUnfiltered = NewValue;
                    NewValue = Capture.Value;
                }
            }

            CurrentDialogue = NewValue;
            DialogueBox.SelectedIndex = NewID;
            DialogueBox.Refresh(NewID);
            DialogueBox.Refresh(OldID);
            EditorBox.Refresh();

            await DoEvents();
            await DialogueBox.EnsureItemVisible(NewID);
        }

        public async static Task UpdateDialogue()
        {
            await JSWrapper.SetUnsaved(true);

            var NewDialogue = CurrentDialogue;

            if (CurrentDialogueUnfiltered != null)
            {
                var Result = CurrentDialogueUnfiltered.Match(Settings.RegexFilter);

                var Capture = Result.First();

                NewDialogue = CurrentDialogueUnfiltered.Substring(0, Capture.Index);
                NewDialogue += CurrentDialogue;
                NewDialogue += CurrentDialogueUnfiltered.Substring(Capture.Index + Capture.Length);
            }

            DialogueBox.Items[DialogueBox.SelectedIndex].Value = NewDialogue;
            DialogueBox.Refresh(DialogueBox.SelectedIndex);

            await NextDialogue();
            await DialogueUpdated();
        }
        
        public async static Task<bool> BackDialogue() {
            int Back = DialogueBox.SelectedIndex - 1;
            int Begin = Back;
            if (Back < 0 || Back > DialogueBox.Items.Length)
                Back = 0;

            while (Back > 0 && !(DialogueBox.Items[Back].VirtualChecked ?? DialogueBox.Items[Back].Checked))
                Back--;

            if (Back == Begin && !(DialogueBox.Items[Back].VirtualChecked ?? DialogueBox.Items[Back].Checked))
                return false;

            if (Back < DialogueBox.Items.Length)
            {
                await SelectChanged(Back);
                return true;
            }

            return false;
        }

        public async static Task<bool> NextDialogue() {
            int Next = DialogueBox.SelectedIndex + 1;
            int Begin = Next;
            if (Next < 0 || Next > DialogueBox.Items.Length)
                Next = 0;

            while (Next < DialogueBox.Items.Length && !(DialogueBox.Items[Next].VirtualChecked ?? DialogueBox.Items[Next].Checked))
                Next++;

            if (Next >= DialogueBox.Items.Length)
            {
                Toast.ShowSuccess(Language.Congratulations);
                return false;
            }

            if (Next == Begin && !(DialogueBox.Items[Next].VirtualChecked ?? DialogueBox.Items[Next].Checked))
                return false;

            if (Next < DialogueBox.Items.Length) {
                await SelectChanged(Next);
                return true;
            }

            return false;
        }
    }
}
