using Blazor.FileReader;
using Blazored.LocalStorage;
using BlazorFileSaver;
using Microsoft.AspNetCore.Components;
using SacanaWrapper;
using Sermone.Components;
using Sermone.Languages;

namespace Sermone
{
    static partial class Engine
    {
        public static bool CanSave;
        public static ILang Language;

        public static IFileReaderService FReader => MainNavMenu.ReaderService;
        public static ILocalStorageService LocalStorage => MainNavMenu.LocalStorage;
        public static IBlazorFileSaver FSaver => MainNavMenu.SaverService;


        public static ElementReference InputRef;
        public static RemoteWrapper Wrapper = new RemoteWrapper();
        public static PluginLoading Loading;
        public static DialogueEditor EditorBox;
        public static NavMenu MainNavMenu;
        public static ListBox DialogueBox;

        public static string CurrentName;
        public static byte[] CurrentScript;

        public static IPluginCreator[] Plugins;
        public static IPlugin CurrentPlugin;

        public static bool NotSaved;

        public static string CurrentDialogue = string.Empty;
    }
}
