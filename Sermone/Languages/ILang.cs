namespace Sermone.Languages
{
    public interface ILang
    {
        public string EnglishLanguageName { get; }
        public string NativeLanguageName { get; }

        //Dropdown
        public string File { get; }
        public string Open { get; }
        public string OpenAs { get; }
        public string OpenReference { get; }
        public string OpenAsReference { get; }
        public string Save { get; }
        
        public string Filter { get; }
        public string AutoSelect { get; }
        public string SelectAll { get; }
        public string DeselectAll { get; }

        //Nav Bar
        public string Search { get; }

        //Loading Page
        public string Loading { get; }
        public string LoadingDesc { get; }
        public string RefreshingDesc { get; }
        public string PluginList { get; }

        //Buttons
        public string Next { get; }
        public string Back { get; }
        public string Refresh { get; }

        //About
        public string AboutVersion { get; }
        public string AboutDesc { get; }

        //Plugin Picker
        public string SelectAPlugin { get; }
        public string Name { get; }
        public string Extension { get; }
        public string ReadWrite { get; }
        public string ReadOnly { get; }
        public string WriteOnly { get; }

        //Notifications
        public string BackgroundTaskWarning { get; }
        public string NoResultsFound { get; }
        public string TryOthersWords { get; }
        public string NotSupported { get; }
        public string NotSupportedPluginFound { get; }
        public string PluginDontSupport { get; }
        public string Success { get; }
        public string BackupUpdated { get; }
        public string Error { get; }
        public string BackupFailed { get; }
        public string BackupIncompatible { get; }
        public string OpenAScriptBefore { get; }
        public string IncompatibleReferenceScript { get; }

        //Settings
        public string Settings { get; }
        public string Language { get; }
        public string Username { get; }
        public string Password { get; }
        public string BackupOn { get; }
        public string DennyPattern { get; }
        public string IgnorePattern { get; }
        public string QuotesPattern { get; }
        public string Breakline { get; }
        public string AcceptableRange { get; }
        public string AsianMode { get; }
        public string Sensitivity { get; }
        public string Export { get; }
        public string Import { get; }

        //Settings Tooltips
        public string DenyPatternTooltip { get; }
        public string IgnorePatternTooltip { get; }
        public string QuotesPatternTooltip { get; }
        public string BreaklineTooltip { get; }
        public string AcceptableRangeTooltip { get; }
        public string AsianModeTooltip { get; }
        public string SensitivityTooltip { get; }

        //Dialogs
        public string AreYouSure { get; }
        public string LoadBackupWarn { get; }
    }
}
