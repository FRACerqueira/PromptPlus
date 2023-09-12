// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PPlus
{
    internal static class UtilExtension
    {
        public const string UnicodeEllipsis = "…";
        public const string AcsiiEllipsis = "...";

        public static void Merge(this IList<Func<object, ValidationResult>> source, IEnumerable<Func<object, ValidationResult>> validators)
        {
            foreach (var validator in validators ?? Enumerable.Empty<Func<object, ValidationResult>>())
            {
                source.Add(validator);
            }
        }

        // Cache whether or not internally normalized line endings
        // already are normalized. No reason to do yet another replace if it is.
        private static readonly bool _alreadyNormalized
            = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

        public static string NormalizeNewLines(this string? text)
        {
            text = text?.Replace("\r\n", "\n", StringComparison.Ordinal);
            text ??= string.Empty;
            if (!_alreadyNormalized)
            {
                text = text.Replace("\n", Environment.NewLine, StringComparison.Ordinal);
            }
            return text;
        }

        public static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new PromptPlusException("UtilExtension.IEnumerable with source null");
            }

            return source.GetEnumerator().Enumerate();
        }

        private static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerator<T> source)
        {
            if (source is null)
            {
                throw new PromptPlusException("UtilExtension.IEnumerable with source null");
            }

            var first = true;
            var last = !source.MoveNext();
            T current;

            for (var index = 0; !last; index++)
            {
                current = source.Current;
                last = !source.MoveNext();
                yield return (index, first, last, current);
                first = false;
            }
        }

    }
}
