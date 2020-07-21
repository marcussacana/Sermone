namespace Sermone.Languages
{
    public class English : ILang
    {
        public string EnglishLanguageName => "English (US)";
        public string NativeLanguageName => "English (US)";

        //Dropdown
        public string Open => "Open";
        public string OpenAs => "Open As";
        public string OpenReference => "Open Reference";
        public string OpenAsReference => "Open Reference As";
        public string Save => "Save";
        public string Filter => "Filter";
        public string AutoSelect => "Auto Select";
        public string SelectAll => "Select All";
        public string DeselectAll => "Deselect All";

        //Nav Bar
        public string Menu => "Menu";
        public string File => "File";
        public string Search => "Find";
        public string Others => "Others";

        //Loading Page
        public string Loading => "Loading...";
        public string LoadingDesc => "Initializing {0}...";
        public string RefreshingDesc => "Updating {0}...";
        public string PluginList => "Plugin List";

        //Buttons
        public string Next => "Next >";
        public string Back => "< Back";
        public string BackNoArrow => "Back";
        public string Refresh => "Refresh";

        //About
        public string AboutVersion => "Alpha Version";
        public string AboutDesc => "This is a Alpha release, bugs may be occur and not at all resources are done for use.";

        //Plugin Picker
        public string SelectAPlugin => "Select a Plugin";
        public string Name => "Name";
        public string Extension => "Extension";
        public string ReadWrite => "Read/Write";
        public string ReadOnly => "Read Only";
        public string WriteOnly => "Write Only";


        //Notifications
        public string BackgroundTaskWarning => "Sermone is working in background, please don't close the application without finishing the current task.";
        public string NoResultsFound => "No results found.";
        public string TryOthersWords => "Try again with other words.";
        public string NotSupported => "Not Supported";
        public string NotSupportedPluginFound => "There is no plugin that supports the selected file";
        public string PluginDontSupport => "The selected plugin does not support this file.";
        public string Success => "Success";
        public string BackupUpdated => "Backup Updated";
        public string Error => "Error";
        public string BackupFailed => "Backup Failed, check your internet connection.";
        public string BackupIncompatible => "Selected backup isn’t compatible with current script.";
        public string OpenAScriptBefore => "You must open a script before.";
        public string IncompatibleReferenceScript => "The Script selected as a reference is incompatible with the currently open script.";

        //Settings
        public string Settings => "Settings";
        public string Language => "Language";
        public string Username => "Username";
        public string Password => "Password";
        public string BackupOn => "Backup On";
        public string Theme => "Theme";
        public string CustomCss => "Custom CSS";

        public string DefaultSelection => "Default Selection";
        public string DennyPattern => "Denied Patterns";
        public string IgnorePattern => "Skippable Patterns";
        public string QuotesPattern => "Quote Patterns";
        public string Breakline => "Line Break";
        public string AcceptableRange => "Acceptable Range";
        public string AsianMode => "Asian Mode";
        public string Sensitivity => "Sensitivity";
        public string Export => "Export";
        public string Import => "Import";

        //Settings Tooltips
        public string DenyPatternTooltip => "Patterns list which never appear in a dialogue. Separeted by semicolon.";
        public string IgnorePatternTooltip => "Special patterns list which appear in a dialogue and must be tolerated by the filter. Separeted by semicolon.";
        public string QuotesPatternTooltip => "String list of dialogues citations. It is composed by two characters, which the first is the oppening and the second is the closing. Separeted by semicolon.";
        public string BreaklineTooltip => "Special character used in the game to break the dialogue line. Escaped content.";
        public string AcceptableRangeTooltip => "Range list of the characters used in the dialogues. Use a hyphen to declare a range between the first and the last character. Non-separated content.";
        public string AsianModeTooltip => "Filter optimization of dialogues for texts in japanese/chinese.";
        public string SensitivityTooltip => "Set the sensitivity of the dialogue filter algorithm, which a higher number makes it more tolerant and a lesser number less tolerant.";

        //Dialogs
        public string AreYouSure => "Are you Sure?";
        public string LoadBackupWarn => "If you load this backup, all of your current script changes will be lost";
    }
}
