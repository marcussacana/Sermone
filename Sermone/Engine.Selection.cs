using Sermone.Tools;
using System.Linq;
using System.Threading.Tasks;

namespace Sermone
{
    public static partial class Engine
    {
        public static async Task AutoSelect() {
#if !DEBUG
            var Result = await(from x in DialogueBox.Items select x.Value).ToArray().BulkIsDialogue();
            for (int i = 0, x = 0; i < Result.Length; i++)
            {
                if (!Result[i])
                {
                    DialogueBox.Items[i].Checked = Result[i];
                    DialogueBox.Refresh(i);
                    x++;
                }

                if (x % 50 == 0)
                    await DoEvents();
            }
#endif
        }

        public static async Task ForceSelection(bool Checked) {
            for (int i = 0; i < DialogueBox.Items.Length; i++) {
                DialogueBox.Items[i].Checked = Checked;
                DialogueBox.Refresh(i);

                if (i % 50 == 0)
                    await DoEvents();
            }
        }
    }
}
