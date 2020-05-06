using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using Sermone.Types;
using SacanaWrapper;
using System.Collections;
using Blazored.LocalStorage;

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
                    Console.WriteLine($"Plugin Creation Error:\n{ex.ToString()}");
                    continue;
                }

                DialogueBox.SetItems((from x in CurrentPlugin.Import()
                                      select new ListBoxItemInfo() {
                                          Checked = false,
                                          Value = x
                                      }).ToArray());

                DialogueBox.Refresh();

                CanSave = true;
                MainNavMenu.Refresh();
            }
        }

        public static async Task SaveFile() {
            if (!CanSave)
                return;

            NotSaved = false;
            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();
            var Data = CurrentPlugin.Export(Lines);
            await FSaver.SaveAsBase64(CurrentName, Convert.ToBase64String(Data), "application/octet-stream");
        }
    }
}
