﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sermone.Languages;
using Blazor.FileReader;
using Blazored.LocalStorage;
using BlazorFileSaver;
using Blazor.ModalDialog;
using BlazorWorker.Core;
using Sermone.Types;

namespace Sermone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazorContextMenu();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazorFileSaver();
            builder.Services.AddModalDialog();
            builder.Services.AddWorkerFactory();
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
        }

        public static bool GetLanguageByID(int ID, out ILang Language) { 
            Language = Engine.Settings.Language switch
            {
                0 => new Portuguese(),
                _ => null
            };
            return Language != null;
        }
    }
}
