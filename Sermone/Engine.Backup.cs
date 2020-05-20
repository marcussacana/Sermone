using System.Linq;
using System.Threading.Tasks;
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

            Changed = 0;

            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();

            if (CurrentPaste == -1)
                CurrentPaste = await Paste.CreatePaste(CurrentName, Lines);
            else
                await Paste.SetPaste(CurrentName, Lines, CurrentPaste);

            Toast.ShowSuccess("Backup Finished");
        }
    }
}
