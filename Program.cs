using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sermone.Languages;
using Blazor.FileReader;
using Blazored.LocalStorage;
using BlazorFileSaver;
using Blazor.ModalDialog;

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
            builder.Services.AddFileReaderService(opt => opt.UseWasmSharedBuffer = false);
            builder.RootComponents.Add<App>("app");

            SermoneInitialize();

            await builder.Build().RunAsync();
        }
        private static void SermoneInitialize()
        {
            Engine.Language = new Portuguese();
            Engine.CanSave = false;
            Engine.NotSaved = false;
        }
    }
}
