using aio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sermone.Tools
{
    public static class Initializer
    {
        public static async Task LoadPlugins()
        {
            Engine.Plugins = AIO.GetPluginCreators();
            Engine.MainNavMenu.Refresh();
            Program.Navigate("/");
        }
    }
}
