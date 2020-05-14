using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sermone.Languages;
using Blazor.FileReader;
using Blazored.LocalStorage;
using BlazorFileSaver;
using Blazor.ModalDialog;
using BlazorWorker.Core;
using Blazored.Toast;
using Sermone.Types;
using Sermone.Tools;
using Newtonsoft.Json;

namespace Sermone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WorkerProxy.Dependencies = new string[] {
                "System.IO.Compression.dll",
                "BrotliSharpLib.dll"
            };
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazorContextMenu();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazorFileSaver();
            builder.Services.AddModalDialog();
            builder.Services.AddWorkerFactory();
            builder.Services.AddBlazoredToast();
            builder.Services.AddFileReaderService(opt => opt.UseWasmSharedBuffer = false);
            builder.RootComponents.Add<App>("app");
            Engine.Language = new Portuguese();
            await builder.Build().RunAsync();
        }
        public static async Task EntryPoint()
        {
            if (await Engine.LocalStorage.ContainKeyAsync("Settings"))
            {
                Engine.Settings = await Engine.LocalStorage.GetItemAsync<Config>("Settings");
            }
            else
            {
                Engine.Settings = new Config()
                {
                    AcceptableRanges = "0-9A-Za-zÀ-ÃÇ-ÎÓ-ÕÚ-Ûà-ãç-îó-õú-û｡-ﾟ!?~.,''\"",
                    AllowNumbers = true,
                    Breakline = "\\n",
                    DenyList = "@;§;_;<;>;/;[;];=",
                    FromAsian = false,
                    IgnoreList = ";､;｡",
                    QuoteList = "<>;();[];“”;［］;《》;«»;「」;『』;【】;（）;'';\"\"",
                    Sensitivity = 4,
                    Language = 0
                };
            }

            if (!GetLanguageByID(Engine.Settings.Language, out Engine.Language)) {
                Engine.Language = new Portuguese();
            }

            Strings.Initialize();
        }

        public static async Task UpdateSettings() { 
            await Engine.LocalStorage.SetItemAsync("Settings", Engine.Settings);
        }

        public static bool GetLanguageByID(int ID, out ILang Language) {
            Language = AllLanguages[0];
            if (ID < 0 || ID >= AllLanguages.Length)
                return false;

            Language = AllLanguages[ID];
            return true;
        }

        public static ILang[] AllLanguages = new ILang[] {
            new Portuguese()
        };
    }
}
