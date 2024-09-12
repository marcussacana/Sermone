﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sermone.Tools
{
    public class StringsService
    {
        public void Initialize(string[] IgnoreList, string[] DenyList, ushort[] BeginAcceptableRanges, ushort[] EndAcceptableRanges,
            string PontuationList, string SpecialList, string OpenQuotes, string CloseQuotes, string PontuationJapList, int Sensitivity,
            bool FromAsian, bool AllowNumbers, string Breakline, string RegexFilter) {

            var AcceptableRanges = new CharacterRange[BeginAcceptableRanges.Length];
            for (int i = 0; i < AcceptableRanges.Length; i++)
            {
                AcceptableRanges[i] = new CharacterRange()
                {
                    Begin = BeginAcceptableRanges[i],
                    End = EndAcceptableRanges[i]
                };
            }

            var Quotes = new Quote[OpenQuotes.Length];
            for (int i = 0; i < Quotes.Length; i++)
            {
                Quotes[i] = new Quote()
                {
                    Start = OpenQuotes[i],
                    End = CloseQuotes[i]
                };
            }

            Initialize(IgnoreList, DenyList, AcceptableRanges, PontuationList.ToCharArray(), SpecialList.ToCharArray(), Quotes, PontuationJapList.ToCharArray(), Sensitivity, FromAsian, AllowNumbers, Breakline, RegexFilter);
        }
        public void Initialize(string[] IgnoreList, string[] DenyList, CharacterRange[] AcceptableRanges,
            char[] PontuationList, char[] SpecialList, Quote[] Quotes, char[] PontuationJapList, int Sensitivity,
            bool FromAsian, bool AllowNumbers, string Breakline, string RegexFilter)
        {

            this.IgnoreList = IgnoreList;
            this.DenyList = DenyList;
            this.AcceptableRanges = AcceptableRanges;
            this.PontuationList = PontuationList;
            this.SpecialList = SpecialList;
            this.Quotes = Quotes;
            this.PontuationJapList = PontuationJapList;
            this.Sensitivity = Sensitivity;
            this.FromAsian = FromAsian;
            this.AllowNumbers = AllowNumbers;
            this.Breakline = Breakline;
            this.RegexFilter = RegexFilter;
        }

        public struct Quote
        {
            public char Start;
            public char End;
        }

        string[] IgnoreList;
        string[] DenyList;
        CharacterRange[] AcceptableRanges;
        char[] PontuationList;
        char[] SpecialList;
        Quote[] Quotes;
        char[] PontuationJapList;

        int Sensitivity;
        bool FromAsian;
        bool AllowNumbers;
        string Breakline;
		string RegexFilter;

		public bool[] IsDialogue(string[] Strings, int? Caution = null, bool UseAcceptableRange = true) {
            var Results = new bool[Strings.Length];
            for (int i = 0; i < Strings.Length; i++)
                Results[i] = IsDialogue(Strings[i], Caution, UseAcceptableRange);
            return Results;
        }

        public bool IsDialogue(string String, int? Caution = null, bool UseAcceptableRange = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(String))
                    return false;

                if (!string.IsNullOrWhiteSpace(RegexFilter) && String.Match(RegexFilter).Any())
                {
                    return true;
                }

                string Str = String.Trim();
                Str = Str.Replace(Breakline, "\n");

                foreach (string Ignore in IgnoreList)
                    Str = Str.Replace(Ignore, "");

                foreach (string Deny in DenyList)
                {
                    if (Str.ToLower().Contains(Deny.ToLower()))
                        return false;
                }

                if (string.IsNullOrWhiteSpace(Str))
                    return false;

                if (UseAcceptableRange && CharacterRanges.TotalMissmatch(Str, AcceptableRanges) > 0)
                    return false;

                string[] Words = Str.Split(' ');

                int Spaces = Str.Where(x => x == ' ' || x == '\t').Count();
                int Pontuations = Str.Where(x => PontuationList.Contains(x)).Count();
                int WordCount = Words.Where(x => x.Length >= 2 && !string.IsNullOrWhiteSpace(x)).Count();
                int Specials = Str.Where(x => char.IsSymbol(x)).Count();
                Specials += Str.Where(x => char.IsPunctuation(x)).Count() - Pontuations;
                int SpecialsStranges = Str.Where(x => SpecialList.Contains(x)).Count();

                int Uppers = Str.Where(x => char.IsUpper(x)).Count();
                int Latim = Str.Where(x => x >= 'A' && x <= 'z').Count();
                int Numbers = Str.Where(x => x >= '0' && x <= '9').Count();
                int NumbersJap = Str.Where(x => x >= '０' && x <= '９').Count();
                int JapChars = Str.Where(x => (x >= '、' && x <= 'ヿ') || (x >= '｡' && x <= 'ﾝ')).Count();
                int Kanjis = Str.Where(x => x >= '一' && x <= '龯').Count();


                bool IsCaps = Str.ToUpper() == Str;
                bool IsJap = JapChars + Kanjis > Latim / 2;


                //More Points = Don't Looks a Dialogue
                //Less Points = Looks a Dialogue
                int Points = 0;

                if (Str.Length > 4)
                {
                    string ext = Str.Substring(Str.Length - 4, 4);
                    try
                    {
                        if (System.IO.Path.GetExtension(ext).Trim('.').Length == 3)
                            Points += 2;
                    }
                    catch { }
                }

                bool BeginQuote = false;
                Quote? LineQuotes = null;
                foreach (Quote Quote in Quotes)
                {
                    BeginQuote |= Str.StartsWith(Quote.Start.ToString());

                    if (Str.StartsWith(Quote.Start.ToString()) && Str.EndsWith(Quote.End.ToString()))
                    {
                        Points -= 3;
                        LineQuotes = Quote;
                        break;
                    }
                    else if (Str.StartsWith(Quote.Start.ToString()) || Str.EndsWith(Quote.End.ToString()))
                    {
                        Points--;
                        LineQuotes = Quote;
                        break;
                    }
                }
                try
                {
                    char Last = (LineQuotes == null ? Str.Last() : Str.TrimEnd(LineQuotes.Value.End).Last());
                    if (IsJap && PontuationJapList.Contains(Last))
                        Points -= 3;

                    if (!IsJap && (PontuationList).Contains(Last))
                        Points -= 3;

                }
                catch { }
                try
                {
                    char First = (LineQuotes == null ? Str.First() : Str.TrimEnd(LineQuotes.Value.Start).First());
                    if (IsJap && PontuationJapList.Contains(First))
                        Points -= 3;

                    if (!IsJap && (PontuationList).Contains(First))
                        Points -= 3;

                }
                catch { }

                if (!IsJap)
                {
                    foreach (string Word in Words)
                    {
                        int WNumbers = Word.Where(c => char.IsNumber(c)).Count();
                        int WLetters = Word.Where(c => char.IsLetter(c)).Count();
                        if (WLetters > 0 && WNumbers > 0)
                        {
                            Points += 2;
                        }
                        if (Word.Trim(PontuationList).Where(c => PontuationList.Contains(c)).Count() != 0)
                        {
                            Points += 2;
                        }
                    }
                }

                if (!BeginQuote && !char.IsLetter(Str.First()))
                    Points += 2;

                if (Specials > WordCount)
                    Points++;

                if (Specials > Latim + JapChars)
                    Points += 2;

                if (SpecialsStranges > 0)
                    Points += 2;

                if (SpecialsStranges > 3)
                    Points++;

                if ((Pontuations == 0) && (WordCount <= 2) && !IsJap)
                    Points++;

                if (Uppers > Pontuations + 2 && !IsCaps)
                    Points++;

                if (Spaces > WordCount * 2)
                    Points++;

                if (Uppers > Spaces + 1 && !IsCaps)
                    Points++;

                if (IsJap && Spaces == 0)
                    Points--;

                if (!IsJap && Spaces == 0)
                    Points += 2;

                if (WordCount <= 2 && Numbers != 0 && !AllowNumbers)
                    Points += (int)(PercentOf(Str, Numbers) / 10);

                if (Str.Length <= 3 && !IsJap)
                    Points++;

                if (Numbers >= (IsJap ? Kanjis + JapChars : Latim))
                    Points += 3;

                if (IsJap && Kanjis / 2 > JapChars)
                    Points--;

                if (IsJap && JapChars > Kanjis)
                    Points--;

                if (IsJap && Latim != 0)
                    Points += (int)(PercentOf(Str, Latim) / 10) + 2;

                if (IsJap && NumbersJap != 0)
                    Points += (int)(PercentOf(Str, NumbersJap) / 10) + 2;

                if (IsJap && Numbers != 0)
                    Points += (int)(PercentOf(Str, Numbers) / 10) + 3;

                if (IsJap && Pontuations != 0)
                    Points += (int)(PercentOf(Str, Pontuations) / 10) + 2;

                if (Str.Trim() == string.Empty)
                    return false;

                if (Str.Trim().Trim(Str.Trim().First()) == string.Empty)
                    Points += 2;

                if (IsJap != FromAsian)
                    return false;

                bool Result = Points < (Caution ?? Sensitivity);
                return Result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return false;
            }
        }
        internal double PercentOf(string Str, int Value)
        {
            var Result = Value / (double)Str.Length;
            return Result * 100;
        }
    }
}
