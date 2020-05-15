using Blazor.FileReader;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using BlazorFileSaver;
using BlazorWorker.Core;
using BlazorWorker.WorkerBackgroundService;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SacanaWrapper;
using Sermone.Components;
using Sermone.Languages;
using Sermone.Pages;
using Sermone.Tools;
using Sermone.Types;
using System;
using System.Threading.Tasks;

namespace Sermone
{
    static partial class Engine
    {
        public static bool CanSave;
        public static ILang Language;

        public static IFileReaderService FReader => MainNavMenu.ReaderService;
        public static ILocalStorageService LocalStorage => MainNavMenu.LocalStorage;
        public static IBlazorFileSaver FSaver => MainNavMenu.SaverService;
        public static IJSRuntime JSRuntime => MainNavMenu.JSRuntime;
        public static IWorkerFactory Worker => MainNavMenu.Worker;
        public static IToastService Toast => MainNavMenu.Toast;

        public static IWorkerBackgroundService<CompressorService> Compressor;

#pragma warning disable 649
        public static ElementReference InputRef;
        public static RemoteWrapper Wrapper = new RemoteWrapper();
        public static DialogueEditor EditorBox;
        public static LoadingViewer Loading;
        public static MainLayout MainLayout;
        public static Pages.Index IndexPage;
        public static NavMenu MainNavMenu;
        public static ListBox DialogueBox;
#pragma warning restore 649

        public static string CurrentName;
        public static byte[] CurrentScript;

        public static IPluginCreator[] Plugins;
        public static IPlugin CurrentPlugin;

        public static bool NotSaved;

        public static string CurrentDialogue = string.Empty;

        public static string CurrentSearch = null;

        public static Config Settings;
    }
}
