using System.Net;
using System.Net.Http;
using Blazor.FileReader;
using Blazor.ModalDialog;
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
using Sermone.Tools;
using Sermone.Types;

namespace Sermone
{
    static partial class Engine
    {
        public static bool CanSave;
        public static ILang Language;

        public static ILocalStorageService LocalStorage => Header.LocalStorage;
        public static IFileReaderService FReader => Header.ReaderService;
        public static IBlazorFileSaver FSaver => Header.SaverService;
        public static IModalDialogService Modal => Header.Modal;
        public static IJSRuntime JSRuntime => Header.JSRuntime;
        public static IWorkerFactory Worker => Header.Worker;
        public static IToastService Toast => Header.Toast;

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
        public static IPluginCreator LastWorkingPlugin;
        public static IPlugin CurrentPlugin;

        public static bool OpenAsSecondary;

        public static bool NotSaved;
        public static bool ForceLastPlugin;

        public static string CurrentDialogue = string.Empty;

        public static string CurrentSearch = null;

        public static Config Settings;
    }
}
