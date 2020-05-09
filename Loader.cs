using SacanaWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sermone
{
    public static class Loader
    {
        static List<IPluginCreator> Plugins;
        static Dictionary<string, byte[]> CacheDic;
        static IAsyncEnumerable<IPluginCreator> Creator;
        static IAsyncEnumerator<IPluginCreator> CreatorEnumerator;

        static IAsyncEnumerable<ValueTuple<string, byte[]>> Cache;
        static IAsyncEnumerator<ValueTuple<string, byte[]>> CacheEnumerator;

        static IAsyncEnumerable<bool> Saver;
        static IAsyncEnumerator<bool> SaverEnumerator;

        public static async Task LoadPlugins()
        {
            if (Cache == null)
            {
                Engine.Loading.LoadStatus = "0%";
                Engine.Loading.Refresh();

                CacheDic = new Dictionary<string, byte[]>(); ;
                Cache = LoadCache(async (x) =>
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
                        RemoteWrapper.HttpClient = new HttpClient();

                        Plugins = new List<IPluginCreator>();
                        Engine.Wrapper = new RemoteWrapper();
                        Creator = Engine.Wrapper.GetAllPlugins(async (x) =>
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
                            //Plugin Initialization Finished
                            if (SaverEnumerator == null)
                            {
                                Engine.Loading.LoadStatus = "0%";
                                Engine.Loading.Description = Engine.Language.RefreshingDesc;
                                Engine.Loading.Refresh();

                                Saver = SaveCache(async (x) =>
                                {
                                    Engine.Loading.LoadStatus = x;
                                    Engine.Loading.Refresh();
                                });
                                SaverEnumerator = Saver.GetAsyncEnumerator();
                            }
                            else
                            {
                                _ = SaverEnumerator;
                                if (!await SaverEnumerator.MoveNextAsync())
                                {
                                    //Cache Storage Refreshed
                                    Engine.Plugins = Plugins.ToArray();
                                    Engine.MainNavMenu.Refresh();
                                    Engine.MainNavMenu.Navigator.NavigateTo("/");
                                }
                            }
                        }
                    }
                }
            }
        }
        private static async IAsyncEnumerable<ValueTuple<string, byte[]>> LoadCache(Action<string> ProgressChanged = null)
        {
            int Length = await Engine.LocalStorage.LengthAsync();
            for (int i = 0; i < Length; i++) {
                ProgressChanged?.Invoke($"{Math.Round(((double)i/Length * 100))}%");
                var Name = await Engine.LocalStorage.KeyAsync(i);
                if (Name.StartsWith("B64"))
                    yield return new ValueTuple<string, byte[]>(Name, await Engine.LocalStorage.GetItemAsync<byte[]>(Name));
            }

            ProgressChanged?.Invoke("100%");
        }
        private static async IAsyncEnumerable<bool> SaveCache(Action<string> ProgressChanged = null)
        {
            var Length = RemoteWrapper.Cache.Count;
            for (int i = 0; i < Length; i++)
            {
                ProgressChanged?.Invoke($"{Math.Round(((double)i / Length * 100))}%");
                var Item = RemoteWrapper.Cache.ElementAt(i);
                await Engine.LocalStorage.SetItemAsync("B64" + Item.Key, Item.Value);
                yield return true;
            }
            ProgressChanged?.Invoke("100%");
        }
    }
}
