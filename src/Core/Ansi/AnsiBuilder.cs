// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Text;

namespace PromptPlusLibrary.Core.Ansi
{
    internal static class AnsiBuilder
    {
        public static string Build(IConsole console, string text, Style style, bool clearrestofline)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text), "AnsiBuild.Build is null");
            }
            if (!console.SupportsAnsi)
            {
                return text;
            }

            byte[] startcodes;

            StringBuilder result = new();

            if (clearrestofline)
            {
                startcodes = [.. AnsiColorBuilder.GetAnsiCodes(console.ColorDepth,
                    style.Background,
                    false)];
                result.Append(AnsiSequences.SGR(startcodes));
                result.Append(AnsiSequences.EL(0));
            }
            startcodes = [];
            bool fg = false;
            bool bg = false;
            if (style.Foreground != console.ForegroundColor)
            {
                fg = true;
                startcodes = [.. AnsiColorBuilder.GetAnsiCodes(console.ColorDepth,
                        style.Foreground,
                        true)];
            }

            if (style.Background != console.BackgroundColor)
            {
                bg = true;
                startcodes =
                [
                    .. startcodes,
                    .. AnsiColorBuilder.GetAnsiCodes(
                    console.ColorDepth,
                    style.Background,
                    false),
                ];
            }
            if (startcodes.Length != 0)
            {
                result.Append(AnsiSequences.SGR(startcodes));
            }
            result.Append(text);
            if (startcodes.Length != 0)
            {
                startcodes = [];
                if (fg)
                {
                    startcodes =
                    [
                        .. startcodes,
                        .. AnsiColorBuilder.GetAnsiCodes(console.ColorDepth,
                            console.ForegroundColor,
                            true),
                    ];
                }
                if (bg)
                {
                    startcodes =
                    [
                        .. startcodes,
                        .. AnsiColorBuilder.GetAnsiCodes(
                            console.ColorDepth,
                            console.BackgroundColor,
                            false),
                    ];
                }
                result.Append(AnsiSequences.SGR(startcodes));
            }
            return result.ToString();
        }
    }
}
