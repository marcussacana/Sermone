using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Sermone.Types;
using Sermone.Tools;
using Microsoft.JSInterop;

namespace Sermone
{
    static partial class Engine
    {
        public async static void FileChanged() {
            foreach (var file in await FReader.CreateReference(InputRef).EnumerateFilesAsync())
            {
                var FileInfo = await file.ReadFileInfoAsync();
                CurrentName = FileInfo.Name;

                using (var Stream = await file.CreateMemoryStreamAsync(4096))
                {
                    CurrentScript = Stream.ToArray();
                    await OpenFile();
                }
            }
        }

        public static async Task OpenFile() {
            DialogueBox.SetItems(new ListBoxItemInfo[0]);
            DialogueBox.Refresh();

            var CurrentExt = Path.GetExtension(CurrentName).ToLower();
            foreach (var Plugin in Plugins) {
                if (!Plugin.Filter.ToLower().Contains(CurrentExt))
                    continue;

                try
                {
                    CurrentPlugin = Plugin.Create(CurrentScript);
                }
                catch (Exception ex) {
                    Console.Error.WriteLine($"Plugin Creation Error:\n{ex}");
                    continue;
                }


                await SetTile($"Sermone - {Language.Loading}");
                DialogueBox.SetItems((from x in CurrentPlugin.Import()
                                      select new ListBoxItemInfo() {
                                        Checked = true,
                                        Visible = true,
                                        Value = x
                                    }).ToArray());
                DialogueBox.Refresh();

                CanSave = true;
                MainNavMenu.Refresh();
                break;
            }

            var Result = await (from x in DialogueBox.Items select x.Value).ToArray().BulkIsDialogue();
            for (int i = 0, x = 0; i < Result.Length; i++) {
                if (!Result[i]) {
                    DialogueBox.Items[i].Checked = Result[i];
                    DialogueBox.Refresh(i);
                    x++;
                }

                if (x % 50 == 0)
                    await DoEvents();
            }
            await SetTile($"Sermone");
        }

        public static async Task SaveFile() {
            if (!CanSave)
                return;

            NotSaved = false;
            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();
            var Data = CurrentPlugin.Export(Lines);
            await FSaver.SaveAsBase64(CurrentName, Convert.ToBase64String(Data), "application/octet-stream");
        }

        public static async Task SetTile(string Title) => await JSRuntime.InvokeVoidAsync("SetTitle", Title);
        public static async Task DoEvents() => await Task.Delay(10);
    }
}
