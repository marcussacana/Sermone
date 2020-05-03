using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using Sermone.Languages;

namespace Sermone
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            SermoneInitialize();

            await builder.Build().RunAsync();
        }

        private static void SermoneInitialize()
        {
            Engine.Language = new Portuguese();
            Engine.CanSave = false;
        }
    }
}
