using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Sermone.Types;
using Sermone.Tools;
using Sermone.Dialogs;
using aio;
using BlazorWorker.WorkerBackgroundService;

namespace Sermone
{
    static partial class Engine
    {
        public async static void FileChanged()
        {
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

        public static async Task Open(bool OpenAs = false, bool AsSecondary = false)
        {
            if (Modified && !AsSecondary) {
                var Result = await Modal.ShowDialogAsync<Question>("", new Blazor.ModalDialog.ModalDialogOptions() {
                    BackgroundClickToClose = false,
                    ShowCloseButton = false
                }, new Blazor.ModalDialog.ModalDialogParameters() {
                    { "Text", Language.UnsavedChanges }
                });

                var YesNo = (bool)Result.ReturnParameters["Result"];
                if (!YesNo)
                    return;
            }

            if (!CanSave && AsSecondary)
            {
                Toast.ShowError(Language.OpenAScriptBefore);
                return;
            }
            
            ForceLastPlugin = OpenAs;
            OpenAsSecondary = AsSecondary;
            await JSWrapper.OpenFile();
        }

        public static async Task OpenAs(bool AsSecondary = false)
        {
            var Result = await Modal.ShowDialogAsync<PluginPicker>(Language.SelectAPlugin);
            await JSWrapper.DestroyTooltips();
            if (!Result.Success)
                return;

            LastWorkingPlugin = (PluginCreator)Result.ReturnParameters["Plugin"];
            await Open(true, AsSecondary);
        }

        public static async Task OpenFile()
        {
            var OriPlugin = CurrentPlugin;
            var OriLastWork = LastWorkingPlugin;

            if (!OpenAsSecondary)
            {
                await JSWrapper.SetUnsaved(false);

                DialogueBox.SelectedIndex = 0;
                DialogueBox.SetItems(new ListBoxItemInfo[0]);
                DialogueBox.Refresh();
            }

            string[] Strings = null;

            await JSWrapper.SetTile($"Sermone - {Language.Loading}");

            if (LastWorkingPlugin is not null)
                Strings = TryUsePlugin(LastWorkingPlugin);

            if (!IsValidStrings(Strings))
            {
                if (ForceLastPlugin)
                {
                    Toast.ShowError(Language.PluginDontSupport);
                    return;
                }

                var CurrentExt = Path.GetExtension(CurrentName).ToLower();

                var SupportedPlugins = (from x in Plugins where x.Extensions.ToLower().Contains(CurrentExt) select x);
                var NotSupportedPlugins = (from x in Plugins where !SupportedPlugins.Contains(x) select x);

                foreach (var Plugin in SupportedPlugins)
                {
                    Strings = TryUsePlugin(Plugin);
                    if (IsValidStrings(Strings))
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
                        if (IsValidStrings(Strings))
                        {
                            LastWorkingPlugin = Plugin;
                            break;
                        }
                    }
                }
            }

            if (Strings == null)
            {
                Toast.ShowError(Language.NotSupportedPluginFound);
                return;
            }

            if (Escaped = Settings.Escape)
            {
                for (int i = 0; i < Strings.Length; i++)
                {
                    Strings[i] = Strings[i].Escape();
                }
            }

            if (OpenAsSecondary)
            {
                OpenAsSecondary = false;
                CurrentPlugin = OriPlugin;
                LastWorkingPlugin = OriLastWork;
                if (Strings.Length != DialogueBox.Items.Length)
                {
                    Toast.ShowError(Language.IncompatibleReferenceScript);
                }
                else
                {
                    for (int i = 0; i < Strings.Length; i++)
                    {
                        DialogueBox.Items[i].SubValue = Strings[i];
                    }
                    DialogueBox.Refresh();
                }
            }
            else
            {
                DialogueBox.SetItems((from x in Strings select new ListBoxItemInfo(true, true, x, null)).ToArray());

                DialogueBox.Refresh();

                CanSave = true;
                MainNavMenu.Refresh();

                switch (Settings.SelectionMode)
                {
                    case 1:
                        await ForceSelection(true);
                        break;
                    default:
                        await AutoSelect();
                        break;
                }
            }

            await JSWrapper.SetTile($"{CurrentName} - Sermone");
        }

        private static bool IsValidStrings(string[] Strs)
        {
            if (Strs == null || Strs.Length == 0)
                return false;
            if (Strs.Length == 1)
                return Strs[0].IsDialogue();
            return true;
        }

        private static string[] TryUsePlugin(PluginCreator Plugin)
        {
            try
            {
                CurrentPlugin = Plugin.Create(CurrentScript);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin \"{Plugin.Name}\" Creation Error:\n{ex}");
                return null;
            }

            try
            {
                return CurrentPlugin.Import();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin \"{Plugin.Name}\" Load Error:\n{ex}");
                return null;
            }
        }
        public static async Task SaveFile(string Name = null)
        {
            if (!CanSave)
                return;

            await JSWrapper.SetUnsaved(false);

            if (Name == null)
                Name = CurrentName;

            NotSaved = false;
            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();

            if (Escaped)
            {
                for (var i = 0; i < Lines.Length; i++)
                {
                    Lines[i] = Lines[i].Unescape();
                }
            }

            var Data = CurrentPlugin.Export(Lines);
            await FSaver.SaveAsBase64(Name, Convert.ToBase64String(Data), "application/octet-stream");
        }

        public static async Task DoEvents() => await Task.Delay(10);
    }
}
