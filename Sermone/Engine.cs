using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Sermone.Types;
using SacanaWrapper;
using Sermone.Dialogs;
using Sermone.Tools;

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
            if (!CanSave && AsSecondary)
            {
                Toast.ShowError(Language.OpenAScriptBefore, Language.Error);
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

            LastWorkingPlugin = (IPluginCreator)Result.ReturnParameters["Plugin"];
            await Open(true, AsSecondary);
        }

        public static async Task OpenFile()
        {
            var OriPlugin = CurrentPlugin;
            var OriLastWork = LastWorkingPlugin;

            if (!OpenAsSecondary)
            {
                DialogueBox.SelectedIndex = 0;
                DialogueBox.SetItems(new ListBoxItemInfo[0]);
                DialogueBox.Refresh();
            }

            string[] Strings = null;

            await JSWrapper.SetTile($"{Language.Loading} - Sermone");

            if (LastWorkingPlugin != null)
                Strings = TryUsePlugin(LastWorkingPlugin);

            if (!IsValidStrings(Strings))
            {
                if (ForceLastPlugin)
                {
                    Toast.ShowError(Language.PluginDontSupport, Language.NotSupported);
                    return;
                }

                var CurrentExt = Path.GetExtension(CurrentName).ToLower();

                var SupportedPlugins = (from x in Plugins where x.Filter.ToLower().Contains(CurrentExt) select x);
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
                Toast.ShowError(Language.NotSupportedPluginFound, Language.Error);
                return;
            }

            if (OpenAsSecondary)
            {
                OpenAsSecondary = false;
                CurrentPlugin = OriPlugin;
                LastWorkingPlugin = OriLastWork;
                if (Strings.Length != DialogueBox.Items.Length)
                {
                    Toast.ShowError(Language.IncompatibleReferenceScript, Language.Error);
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

        private static string[] TryUsePlugin(IPluginCreator Plugin)
        {
            try
            {
                if (Plugin.InitializeWithScript)
                    CurrentPlugin = Plugin.Create(CurrentScript);
                else
                    CurrentPlugin = Plugin.Create();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin \"{Plugin.Name}\" Creation Error:\n{ex}");
                return null;
            }

            try
            {
                return Plugin.InitializeWithScript ? CurrentPlugin.Import() : CurrentPlugin.Import(CurrentScript);
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

            if (Name == null)
                Name = CurrentName;

            NotSaved = false;
            var Lines = (from x in DialogueBox.Items select x.Value).ToArray();
            var Data = CurrentPlugin.Export(Lines);
            await FSaver.SaveAsBase64(Name, Convert.ToBase64String(Data), "application/octet-stream");
        }

        public static async Task DoEvents() => await Task.Delay(10);
    }
}
