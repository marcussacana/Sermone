namespace Sermone.Languages
{
    public class English : ILang
    {
        public string EnglishLanguageName => "English (US)";
        public string NativeLanguageName => "English (US)";

        //Dropdown
        public string File => "File";
        public string Open => "Open";
        public string Save => "Save";

        //Nav Bar
        public string Search => "Find";

        //Loading Page
        public string Loading => "Loading...";
        public string LoadingDesc => "Initializing {0}...";
        public string RefreshingDesc => "Updating {0}...";
        public string PluginList => "Plugin List";

        //Buttons
        public string Next => "Next >";
        public string Back => "< Back";

        //About
        public string AboutVersion => "Prototype Version";
        public string AboutDesc => "This is a prototype version of the software focused on experimental purposes and, because it lacks of more advanced resources for any practical means, errors can occur during use.";

        //Notifications
        public string BackgroundTaskWarning => "Sermone is working in background, please don't close the application without finishing the current task.";
        public string NoResultsFound => "No results found.";
        public string TryOthersWords => "Try again with other words.";

        //Settings
        public string Settings => "Settings";
        public string Language => "Language";

        public string DennyPattern => "Denied Patterns";
        public string IgnorePattern => "Skippable Patterns";
        public string QuotesPattern => "Quote Patterns";
        public string Breakline => "Line Break";
        public string AcceptableRange => "Acceptable Range";
        public string AsianMode => "Asian Mode";
        public string Sensitivity => "Sensitivity";

        //Settings Tooltips
        public string DenyPatternTooltip => "Patterns list which never appear in a dialogue. Separeted by semicolon.";
        public string IgnorePatternTooltip => "Special patterns list which appear in a dialogue and must be tolerated by the filter. Separeted by semicolon.";
        public string QuotesPatternTooltip => "String list of dialogues citations. It is composed by two characters, which the first is the oppening and the second is the closing. Separeted by semicolon.";
        public string BreaklineTooltip => "Special character used in the game to break the dialogue line. Escaped content.";
        public string AcceptableRangeTooltip => "Range list of the characters used in the dialogues. Use a hyphen to declare a range between the first and the last character. Non-separated content.";
        public string AsianModeTooltip => "Filter optimization of dialogues for texts in japanese/chinese.";
        public string SensitivityTooltip => "Set the sensitivity of the dialogue filter algorithm, which a higher number makes it more tolerant and a lesser number less tolerant.";

    }
}
