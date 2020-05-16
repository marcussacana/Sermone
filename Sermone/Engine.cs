using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Sermone.Types;
using Sermone.Tools;
using Microsoft.JSInterop;
using System.Security.Cryptography;
using SacanaWrapper;

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
            DialogueBox.SelectedIndex = 0;
            DialogueBox.SetItems(new ListBoxItemInfo[0]);
            DialogueBox.Refresh();

            string[] Strings = null;

            await SetTile($"Sermone - {Language.Loading}");

            if (LastWorkingPlugin != null)
                Strings = TryUsePlugin(LastWorkingPlugin);

            if (Strings == null)
            {
                var CurrentExt = Path.GetExtension(CurrentName).ToLower();

                var SupportedPlugins = (from x in Plugins where x.Filter.ToLower().Contains(CurrentExt) select x);
                var NotSupportedPlugins = (from x in Plugins where !SupportedPlugins.Contains(x) select x);

                foreach (var Plugin in SupportedPlugins)
                {
                    Strings = TryUsePlugin(Plugin);
                    if (Strings != null)
                    {
                        LastWorkingPlugin = Plugin;
                        break;
                    }
                }

                if (Strings == null)
                {
                    foreach (var Plugin in NotSupportedPlugins)
                    {
                        Strings = TryUsePlugin(Plugin);
                        if (Strings != null)
                        {
                            LastWorkingPlugin = Plugin;
                            break;
                        }
                    }
                }
            }

            DialogueBox.SetItems((from x in Strings
                                  select new ListBoxItemInfo() {
                                      Checked = true,
                                      Visible = true,
                                      Value = x
                                  }).ToArray());

            DialogueBox.Refresh();

            CanSave = true;
            MainNavMenu.Refresh();

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

        private static string[] TryUsePlugin(IPluginCreator Plugin) {
            IPlugin CurrentPlugin;
            try {
                if (Plugin.InitializeWithScript)
                    CurrentPlugin = Plugin.Create(CurrentScript);
                else
                    CurrentPlugin = Plugin.Create();
            }
            catch (Exception ex) {
                Console.Error.WriteLine($"Plugin \"{Plugin.Name}\" Creation Error:\n{ex}");
                return null;
            }

            try {
               return Plugin.InitializeWithScript ? CurrentPlugin.Import() : CurrentPlugin.Import(CurrentScript);
            }
            catch (Exception ex){
                Console.Error.WriteLine($"Plugin \"{Plugin.Name}\" Load Error:\n{ex}"); 
                return null; 
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

        public static async Task SetTile(string Title) => await JSRuntime.InvokeVoidAsync("SetTitle", Title);
        public static async Task DoEvents() => await Task.Delay(10);
    }
}
