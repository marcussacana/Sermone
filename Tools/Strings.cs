using BlazorWorker.BackgroundServiceFactory;
using BlazorWorker.WorkerBackgroundService;
using Dynamitey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sermone.Tools.StringsService;

namespace Sermone.Tools
{

    public static class Strings
    {
        static Strings()
        {
            PontuationJapList = new char[] { '。', '？', '！', '…', '、', '―' };
            SpecialList = new char[] { '_', '=', '+', '#', ':', '$', '@' };
            PontuationList = new char[] { '.', '?', '!', '…', ',' };

            AcceptableRanges = CharacterRanges.GetRanges(Engine.Settings.AcceptableRanges).ToArray();
            DenyList = Engine.Settings.DenyList.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            IgnoreList = Engine.Settings.IgnoreList.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Quotes = Engine.Settings.QuoteList.Split(';')
                    .Where(x => x.Length == 2)
                    .Select(x => new Quote() { Start = x[0], End = x[1] }).ToArray();

            Service = new StringsService();
            Service.Initialize(IgnoreList, DenyList, AcceptableRanges, PontuationList, SpecialList, Quotes, PontuationJapList, Engine.Settings.Sensitivity, Engine.Settings.FromAsian, Engine.Settings.AllowNumbers, Engine.Settings.Breakline);
            BackgroundService = null;
        }

        public async static Task<bool[]> BulkIsDialogue(this string[] Strings, int? Caution = null, bool UseAcceptableRanges = true)
        {
            var lIgnoreList = IgnoreList;
            var lDenyList = DenyList;
            var lBeginAcceptableRanges = (from x in AcceptableRanges select x.Begin).ToArray();
            var lEndAcceptableRanges   = (from x in AcceptableRanges select x.End  ).ToArray();
            var lPontuationList = new string(PontuationList);
            var lSpecialList = new string(SpecialList);
            var lOpenQuotes =  new string((from x in Quotes select x.Start).ToArray());
            var lCloseQuotes = new string((from x in Quotes select x.End).ToArray());
            var lPontuationJapList = new string(PontuationJapList);
            var lSensitivity = Engine.Settings.Sensitivity;
            var lFromAsian = Engine.Settings.FromAsian;
            var lAllowNumbers = Engine.Settings.AllowNumbers;
            var lBreakline = Engine.Settings.Breakline;

            if (BackgroundService == null)
            {
                var Worker = await Engine.Worker.CreateAsync();
                BackgroundService = await Worker.CreateBackgroundServiceAsync<StringsService>();

                await BackgroundService.RunAsync((s) => s.Initialize(lIgnoreList, lDenyList, lBeginAcceptableRanges, lEndAcceptableRanges, lPontuationList, lSpecialList, lOpenQuotes, lCloseQuotes, lPontuationJapList, lSensitivity, lFromAsian, lAllowNumbers, lBreakline));
            }

            return await BackgroundService.RunAsync((s) => s.IsDialogue(Strings, Caution, UseAcceptableRanges));
        }

        public static IWorkerBackgroundService<StringsService> BackgroundService;

        public static string ToPercentage(this int Progress, int Maximum) => $"{Math.Round(((double)Progress / Maximum * 100))}%";

        public static char[] PontuationJapList;
        public static char[] SpecialList;
        public static char[] PontuationList;

        public static CharacterRange[] AcceptableRanges;
        public static string[] DenyList;
        public static string[] IgnoreList;
        public static Quote[] Quotes;

        public static StringsService Service;

        internal static bool IsDialogue(this string String, int? Caution = null, bool UseAcceptableRange = true) => 
            Service.IsDialogue(String, Caution, UseAcceptableRange);
        
        internal static string Escape(this string String)
        {
            StringBuilder SB = new StringBuilder();
            foreach (char c in String)
            {
                if (c == '\n')
                    SB.Append("\\n");
                else if (c == '\\')
                    SB.Append("\\\\");
                else if (c == '\t')
                    SB.Append("\\t");
                else if (c == '\r')
                    SB.Append("\\r");
                else
                    SB.Append(c);
            }
            return SB.ToString();
        }
        internal static string Unescape(this string String)
        {
            StringBuilder SB = new StringBuilder();
            bool Escape = false;
            foreach (char c in String)
            {
                if (c == '\\' & !Escape)
                {
                    Escape = true;
                    continue;
                }
                if (Escape)
                {
                    switch (c.ToString().ToLower()[0])
                    {
                        case '\\':
                            SB.Append('\\');
                            break;
                        case 'n':
                            SB.Append('\n');
                            break;
                        case 't':
                            SB.Append('\t');
                            break;
                        case '"':
                            SB.Append('"');
                            break;
                        case '\'':
                            SB.Append('\'');
                            break;
                        case 'r':
                            SB.Append('\r');
                            break;
                        default:
                            throw new Exception("\\" + c + " Isn't a valid string escape.");
                    }
                    Escape = false;
                }
                else
                    SB.Append(c);
            }

            return SB.ToString();
        }

        internal static string Minify(this string String) {
            StringBuilder SB = new StringBuilder();
            foreach (var Char in String) {
                if (char.IsWhiteSpace(Char))
                    continue;
                SB.Append(char.ToLowerInvariant(Char));
            }
            return SB.ToString();
        }
    }
}
