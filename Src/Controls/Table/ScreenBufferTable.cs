﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferTable
    {
        public static void WriteFilterTable<T>(this ScreenBuffer screenBuffer, TableOptions<T> options, string input, EmacsBuffer filter)
        {
            if (options.FilterType == FilterMode.StartsWith)
            {
                if (input.StartsWith(filter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    screenBuffer.WriteAnswer(options, input[..filter.Length]);
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteSuggestion(options, input[filter.Length..]);
                }
                else
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteEmptyFilter(options, filter.ToForward());
                }
            }
            else
            {
                var parts = input.ToUpperInvariant().Split(filter.ToString().ToUpperInvariant());
                if (parts.Length == 1 && string.IsNullOrEmpty(parts[0]))
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToString());
                    screenBuffer.SaveCursor();
                    return;
                }
                var first = true;
                var pos = 0;
                foreach (var itempart in parts)
                {
                    pos++;
                    screenBuffer.WriteSuggestion(options, itempart);
                    if (pos < parts.Length)
                    {
                        screenBuffer.WriteAnswer(options, filter.ToString());
                    }
                    if (first)
                    {
                        first = false;
                        screenBuffer.SaveCursor();
                    }
                }
            }
        }

        public static void WriteLineDescriptionTable<T>(this ScreenBuffer screenBuffer,T input,int rowpos, int colpos, TableOptions<T> options)
        {
            var result = options.OptDescription;
            if (input != null)
            {
                if (options.ChangeDescription != null)
                {
                    result = options.ChangeDescription.Invoke(input,rowpos,colpos);
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
            }
        }
    }
}