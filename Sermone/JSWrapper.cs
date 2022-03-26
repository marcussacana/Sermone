using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Sermone
{
    internal static class JSWrapper
    {
        public static async Task EnableTooltips()
        {
            await Engine.JSRuntime.InvokeVoidAsync("EnableTooltip");
        }
        public static async Task DestroyTooltips()
        {
            await Engine.JSRuntime.InvokeVoidAsync("DestroyTooltip");
        }
        public static async Task SetTile(string Title)
        {
            await Engine.JSRuntime.InvokeVoidAsync("SetTitle", Title);
        }
        public static async Task SetUnsaved(bool Unsaved)
        {
            Engine.Modified = Unsaved;
            await Engine.JSRuntime.InvokeVoidAsync("SetUnsaved", Unsaved);
        }
        public static async Task OpenFile(string InputId = "FOpen")
        {
            await Engine.JSRuntime.InvokeVoidAsync("OpenFile", InputId);
        }
        public static async Task Focus(this ElementReference Element)
        {
            await Engine.JSRuntime.InvokeVoidAsync("FocusElement", Element);
        }
        public static async Task EnsureItemVisible(int ID)
        {
            await Engine.JSRuntime.InvokeVoidAsync("EnsureItemVisible", ID);
        }
        public static async Task SetCustomCSS(string CSS)
        {
            await Engine.JSRuntime.InvokeVoidAsync("SetCustomCSS", CSS);
        }
        public static async Task<string> GetBaseDirectory()
        {
            return await Engine.JSRuntime.InvokeAsync<string>("GetBaseDirectory");
        }
        public static async Task<string> Request(string Method, string URL, string Data = null)
        {
            return await Engine.JSRuntime.InvokeAsync<string>("Request", new string[] { Method, URL, Data });
        }
    }
}
