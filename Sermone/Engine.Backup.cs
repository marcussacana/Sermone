using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sermone.Dialogs;
using Sermone.Pastes;

namespace Sermone
{
    public static partial class Engine
    {
        public static IPaste Paste = null;
        public static int Changed;
        public static long CurrentPaste = -1;
        public static async Task DialogueUpdated()
        {
            if (Changed++ < Settings.BackupOn)
                return;

            if (Paste == null)
            {
                var Name = Path.GetFileNameWithoutExtension(CurrentName);

                if (!Name.EndsWith("_Backup"))
                    Name += "_Backup";

                Name = Path.GetExtension(CurrentName);

                await SaveFile(Name);
            }

            try
            {

                Changed = 0;

                var Lines = (from x in DialogueBox.Items select x.Value).ToArray();

                if (CurrentPaste == -1)
                    CurrentPaste = await Paste.CreatePaste(CurrentName, Lines);
                else
                    await Paste.SetPaste(CurrentName, Lines, CurrentPaste);

                Toast.ShowSuccess(Language.BackupUpdated);
            }
            catch
            {
                Toast.ShowError(Language.BackupFailed);
            }
        }

        public static async Task ShowBackups()
        {
            if (CurrentScript == null || Paste == null)
                return;

            var Rst = await Modal.ShowDialogAsync<BackupPicker>("Backups");
            if (!Rst.Success)
                return;

            CurrentPaste = (long)Rst.ReturnParameters.Get<object>("Backup");
            await LoadBackup();
        }

        public static async Task LoadBackup()
        {
            try
            {

                await JSWrapper.SetTile("Sermone - " + Language.Loading);
                var Backup = await Paste.GetPaste(CurrentPaste);
                if (Math.Abs(Backup.Content.Length - DialogueBox.Items.Length) > 1)
                {
                    Toast.ShowError(Language.BackupIncompatible);
                }
                else
                {
                    for (int i = 0; i < DialogueBox.Items?.Length; i++)
                    {
                        DialogueBox.Items[i].Value = Backup.Content[i];
                        DialogueBox.Refresh(i);

                        if ((i % 50) == 0)
                            await DoEvents();
                    }
                }
            }
            catch
            {
                Toast.ShowError(Language.BackupFailed);
            }
            await JSWrapper.SetTile("Sermone");
        }
    }
}
