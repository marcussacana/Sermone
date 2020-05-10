﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Sermone.Tools
{
    public static class CharacterRanges
    {
        public static IEnumerable<CharacterRange> GetRanges(string List)
        {
            if (List == null)
                yield break;

            for (int i = 0; i < List.Length; i++)
            {
                char Char = List[i];
                char? Next = null;

                if (i + 1 < List.Length)
                    Next = List[i + 1];

                if (Next == '-' && i + 2 < List.Length)
                    Next = List[i + 2];
                else
                    Next = null;

                if (Next != null)
                    yield return new CharacterRange() { Begin = Char, End = Next.Value };
                else
                    yield return new CharacterRange() { Begin = Char, End = Char };
            }
        }

        public static int TotalMissmatch(string String, IEnumerable<CharacterRange> Ranges)
        {
            if (Ranges.Count() == 0)
                return 0;

            int Missmatch = 0;
            foreach (var Char in String)
                if (!CharInRange(Char, Ranges) && !char.IsWhiteSpace(Char))                
                    Missmatch++;
                
            return Missmatch;
        }

        public static bool CharInRange(char Char, IEnumerable<CharacterRange> Ranges)
        {
            foreach (var Range in Ranges)
            {
                if (Char >= Range.Begin && Char <= Range.End)
                    return true;
            }
            return false;
        }
    }

    public struct CharacterRange
    {
        public ushort Begin;
        public ushort End;
    }
}
