using Microsoft.JSInterop;
using System.Linq;
using System.Reflection;

namespace BlazorWorker.WorkerCore
{
    // Serves as a wrapper around a JSObject.
    class DOMObject
    {

        public JSInProcessRuntime DefaultWebAssemblyJSRuntime;

        public DOMObject(object jsobject) {
            var AspNetCoreWebAssembly = Assembly.Load("Microsoft.AspNetCore.Components.WebAssembly");
            var tDefWebAsm = (from x in AspNetCoreWebAssembly.GetTypes() where x.Name == "DefaultWebAssemblyJSRuntime" select x).Single();
            var tInstance = tDefWebAsm.GetField("Instance", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            DefaultWebAssemblyJSRuntime = (JSInProcessRuntime)tInstance.GetValue(null);
        }

        public object Invoke(string Method, params object[] args) => Invoke<object>(Method, args);
        public T Invoke<T>(string method, params object[] args)
        {
            return DefaultWebAssemblyJSRuntime.Invoke<T>(method, args);
        }

    }
}
