using SacanaWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sermone.Tools
{
    public static class Initializer
    {
        static List<IPluginCreator> Plugins;
        static Dictionary<string, byte[]> CacheDic;
        static IAsyncEnumerable<IPluginCreator> Creator;
        static IAsyncEnumerator<IPluginCreator> CreatorEnumerator;

        static IAsyncEnumerable<ValueTuple<string, byte[]>> Cache;
        static IAsyncEnumerator<ValueTuple<string, byte[]>> CacheEnumerator;

        public static async Task LoadPlugins()
        {
            if (Cache == null)
            {
                Engine.Loading.LoadStatus = "0%";
                Engine.Loading.Refresh();

                CacheDic = new Dictionary<string, byte[]>(); ;
                Cache = LoadCache((x) =>
                {
                    Engine.Loading.LoadStatus = x;
                    Engine.Loading.Refresh();
                });
                CacheEnumerator = Cache.GetAsyncEnumerator();
            }
            else
            {
                var Pair = CacheEnumerator.Current;
                if (Pair.Item1 != null && !CacheDic.ContainsKey(Pair.Item1))
                {
                    CacheDic[Pair.Item1] = Pair.Item2;
                }
                if (!await CacheEnumerator.MoveNextAsync())
                {
                    //Cache Initialization Finished
                    if (CreatorEnumerator == null)
                    {
                        Engine.Loading.LoadStatus = Engine.Language.PluginList;
                        Engine.Loading.Refresh();

                        RemoteWrapper.Cache = CacheDic;
                        RemoteWrapper.ForceBinary = true;
                        RemoteWrapper.HttpClient = new HttpClient();

                        Plugins = new List<IPluginCreator>();
                        Engine.Wrapper = new RemoteWrapper();
                        Creator = Engine.Wrapper.GetAllPlugins((x) =>
                        {
                            Engine.Loading.LoadStatus = x;
                            Engine.Loading.Refresh();
                        });
                        CreatorEnumerator = Creator.GetAsyncEnumerator();
                    }
                    else
                    {
                        var Plugin = CreatorEnumerator.Current;
                        if (Plugin != null && !Plugins.Contains(Plugin))
                            Plugins.Add(Plugin);
                        if (!await CreatorEnumerator.MoveNextAsync())
                        {
                            //Cache Storage Refreshed
                            Engine.Plugins = Plugins.ToArray();
                            Engine.MainNavMenu.Refresh();
                            Program.Navigate("/");
                            return;
                        }
                    }
                }
            }
        }
        private static async IAsyncEnumerable<ValueTuple<string, byte[]>> LoadCache(Action<string> ProgressChanged = null)
        {
            int Length = await Engine.LocalStorage.LengthAsync();
            for (int i = 0; i < Length; i++) {
                ProgressChanged?.Invoke(i.ToPercentage(Length));
                var Name = await Engine.LocalStorage.KeyAsync(i);

                if (Name.StartsWith("CB64")) {
                    var Compressed = await Engine.LocalStorage.GetItemAsync<byte[]>(Name);
                    var Decompressed = await Engine.Compressor.Decompress(Compressed);
                    if (Decompressed != null)
                        yield return new ValueTuple<string, byte[]>(Name.Substring(4), Decompressed);
                }
            }

            ProgressChanged?.Invoke("100%");
        }
        public static async IAsyncEnumerable<bool> SaveCache(Action<string> ProgressChanged = null)
        {
            var Length = RemoteWrapper.Cache.Count;
            for (int i = 0; i < Length; i++)
            {
                ProgressChanged?.Invoke(i.ToPercentage(Length));
                var Item = RemoteWrapper.Cache.ElementAt(i);
                var Compressed = await Engine.Compressor.Compress(Item.Value);
                await Engine.LocalStorage.SetItemAsync("CB64" + Item.Key, Compressed);
                yield return true;
            }
            ProgressChanged?.Invoke("100%");
        }
    }
}
