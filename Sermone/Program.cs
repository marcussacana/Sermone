using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sermone.Languages;
using Blazored.LocalStorage;
using BlazorFileSaver;
using Blazor.ModalDialog;
using BlazorWorker.Core;
using Blazored.Toast;
using Sermone.Types;
using Sermone.Tools;
using System;
using System.Text;
using Sermone.Pastes;
using Tewr.Blazor.FileReader;

namespace Sermone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //builder.Services.AddBlazorContextMenu();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazorFileSaver();
            builder.Services.AddModalDialog();
            builder.Services.AddWorkerFactory();
            builder.Services.AddBlazoredToast();
            builder.Services.AddFileReaderService(opt => opt.UseWasmSharedBuffer = false);
            builder.RootComponents.Add<App>("app");
            Engine.Language = new English();
            await builder.Build().RunAsync();
        }
        public static async Task<bool> EntryPoint()
        {
            if (BasePath != null)
                return false;

            if (await Engine.LocalStorage.ContainKeyAsync("Settings")) {
                Console.WriteLine("Loading Settings...");
                Engine.Settings = await Engine.LocalStorage.GetItemAsync<Config>("Settings");
            }
            else
            {
                Console.WriteLine("Using Default Settings...");
                Engine.Settings = new Config()
                {
                    AcceptableRanges = "0-9A-Za-zÀ-ÃÇ-ÎÓ-ÕÚ-Ûà-ãç-îó-õú-û｡-ﾟ!?~.,[](){}''\"",
                    AllowNumbers = true,
                    Breakline = "\\n",
                    DenyList = "@;§;_;<;>;/;=",
                    FromAsian = false,
                    IgnoreList = ";､;｡;%;{;}",
                    QuoteList = "<>;();[];“”;［］;《》;«»;「」;『』;【】;（）;'';\"\"",
                    Sensitivity = 3,
                    BackupOn = 15,
                    Language = 0
                };
            }

            await UpdateSettings(false);

            Strings.Initialize();

            BasePath = await JSWrapper.GetBaseDirectory();
            if (!BasePath.EndsWith("/"))
                BasePath += "/";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return true;
        }

        public static async Task UpdateSettings(bool Save = true)
        {
            GetLanguageByID(Engine.Settings.Language, out Engine.Language);
            await JSWrapper.SetCustomCSS(Engine.Settings.CustomCSS);

            if (Save)
                await Engine.LocalStorage.SetItemAsync("Settings", Engine.Settings);

            Engine.Paste = null;
            GetPasteCreatorByID(Engine.Settings.PasteClient, out IPasteCreator PasteCreator);
            if (PasteCreator != null && !string.IsNullOrEmpty(Engine.Settings.PasteUsername) && !string.IsNullOrEmpty(Engine.Settings.PastePassword)) { 
                Engine.Paste = await PasteCreator.Create(Engine.Settings.PasteUsername, Engine.Settings.PastePassword);
            }
        }

        public static bool GetLanguageByID(int ID, out ILang Language) {
            Language = AllLanguages[0];
            if (ID < 0 || ID >= AllLanguages.Length)
                return false;

            Language = AllLanguages[ID];
            return true;
        }
        public static bool GetPasteCreatorByID(int ID, out IPasteCreator Paste) {
            Paste = null;
            if (ID < 0 || ID >= AllPastes.Length)
                return false;

            Paste = AllPastes[ID];
            return true;
        }

        static string BasePath = null;
        public static void Navigate(string Path) {
            Header.Navigator.NavigateTo(BasePath + Path.TrimStart('/'));
        }

        public static ILang[] AllLanguages = new ILang[] {
            new English(), new Portuguese()
        };

        public static IPasteCreator[] AllPastes = new IPasteCreator[] {
        };
    }
}
