using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sermone.Pastes;

namespace Sermone
{
    public static partial class Engine
    {
        public static Dictionary<long, string> Pastes = new Dictionary<long, string>();
        public static IPaste Paste = null;
        public static int Changed;
        public static long CurrentPaste = -1;
        public static async Task DialogueUpdated()
        {
            return;
            if (Changed++ < Settings.BackupOn)
                return;

            Changed = 0;

            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();

            if (CurrentPaste == -1)
                CurrentPaste = await Paste.CreatePaste(CurrentName, Lines);
            else
                await Paste.SetPaste(CurrentName, Lines, CurrentPaste);

            Toast.ShowSuccess(Language.BackupUpdated, Language.Success);
        }

        public static async Task ShowBackups() { 
            Toast.ShowError("This Feature isn't available in the current Sermone version.", "Not Available");
        }
    }
}
