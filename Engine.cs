using Sermone.Languages;
using Sermone.Shared;
using Sermone.Types;
using SacanaWrapper;
using Blazor.FileReader;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Linq;

namespace Sermone
{
    public static class Engine
    {
        public static bool CanSave;
        public static ILang Language;


        public static IFileReaderService FReader => MainNavMenu.ReaderService;
        public static ILocalStorageService LocalStorage => MainNavMenu.LocalStorage;

        public static ElementReference InputRef;

        public static RemoteWrapper Wrapper = new RemoteWrapper();

        public static NavMenu MainNavMenu = null;

        public static ListBox DialogueBox;

        public static string CurrentName;
        public static byte[] CurrentScript;

        public static IPluginCreator[] Plugins;
        public static IPlugin CurrentPlugin;

        public static void RefreshDialogueBox() {
            DialogueBox.Refresh();
        }

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
            if (Plugins == null) {
                await LoadCache();
                Plugins = await Wrapper.GetAllPlugins();
                await SaveCache();
            }

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
            }
        }

        public static async Task SaveFile() { 
        
        }

        public static void CheckedChanged(int ID, bool Checked) {

        }

        private static async Task LoadCache()
        {
            Dictionary<string, byte[]> Cache = new Dictionary<string, byte[]>();
            int Length = await LocalStorage.LengthAsync();
            for (int i = 0; i < Length; i++)
            {
                var Name = await LocalStorage.KeyAsync(i);
                if (Name.StartsWith("B64"))
                    Cache[Name.Substring(3)] = await LocalStorage.GetItemAsync<byte[]>(Name);
            }
            RemoteWrapper.HttpClient = new HttpClient();
            RemoteWrapper.Cache = Cache;
        }
        private static async Task SaveCache()
        {
            foreach (var Item in RemoteWrapper.Cache) {
                await LocalStorage.SetItemAsync("B64" + Item.Key, Item.Value);
            }
        }
    }
}
