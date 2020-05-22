using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Sermone.Tools
{
    public static class XMLHTTPRequest
    {
        public static async Task<string> Request(string Method, string URL, string Data = null)
        {
            return await Engine.JSRuntime.InvokeAsync<string>("Request", new string[] { Method, URL, Data });
        }
    }
}
