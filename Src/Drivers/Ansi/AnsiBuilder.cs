// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/spectreconsole/spectre.console
// ***************************************************************************************

using System;
using System.Linq;
using System.Text;

namespace PPlus.Drivers.Ansi
{
    internal static class AnsiBuilder
    {
        public static string Build(out int spaces, IProfileDrive profileDrive, int offsetCursorLeft, Segment[] segments, bool clearrestofline = false)
        {
            var builder = new StringBuilder();
            var offset = offsetCursorLeft;
            spaces = 0;
            foreach (var segment in segments)
            {
                if (segment.IsAnsiControl)
                {
                    if (profileDrive.SupportsAnsi)
                    {
                        builder.Append(segment.Text);
                    }
                    continue;
                }
                var parts = segment.Text.Split(Environment.NewLine);
                foreach (var (_, _, last, part) in parts.Enumerate())
                {
                    var cls = clearrestofline;
                    if (!last)
                    {
                        cls = true;
                    }
                    if (part != null)
                    {
                        builder.Append(Build(profileDrive, Style.ApplyOverflowStrategy(offset, profileDrive.BufferWidth, segment.Style.OverflowStrategy, part, profileDrive.IsUnicodeSupported), segment.Style, cls));
                        offset += part.Length;
                    }

                    if (!last)
                    {
                        if (cls)
                        {
                            if (profileDrive.SupportsAnsi)
                            {
                                builder.Append(AnsiSequences.EL(0));
                            }
                            else
                            {
                                var spc = new string(' ', profileDrive.BufferWidth - offset - profileDrive.PadLeft - profileDrive.PadRight);
                                spaces = spc.Length;
                                builder.Append(spc);
                            }
                        }
                        builder.Append(Environment.NewLine);
                        if (profileDrive.PadLeft > 0)
                        {
                            builder.Append(new string(' ', profileDrive.PadLeft));
                        }
                        offset = profileDrive.PadLeft;
                    }
                }
            }
            return builder.ToString();
        }

        private static string Build(IProfileDrive profileDrive, string text, Style style, bool clearrestofline)
        {
            if (text is null)
            {
                throw new PromptPlusException($"AnsiBuild.Build {nameof(text)} is null");
            }
            if (!profileDrive.SupportsAnsi)
            {
                return text;
            }

            byte[] startcodes;

            var result = new StringBuilder();

            if (clearrestofline)
            {
                startcodes = AnsiColorBuilder.GetAnsiCodes(profileDrive.ColorDepth,
                    style.Background,
                    false).ToArray();
                result.Append(AnsiSequences.SGR(startcodes));
                result.Append(AnsiSequences.EL(0));
            }
            startcodes = Array.Empty<byte>();
            var fg = false;
            var bg = false;
            if (style.Foreground != profileDrive.ForegroundColor)
            {
                fg = true;
                startcodes = AnsiColorBuilder.GetAnsiCodes(profileDrive.ColorDepth,
                        style.Foreground,
                        true).ToArray();
            }

            if (style.Background != profileDrive.BackgroundColor)
            {
                bg = true;
                startcodes = startcodes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                    profileDrive.ColorDepth,
                    style.Background,
                    false)).ToArray();
            }
            if (startcodes.Length != 0)
            {
                result.Append(AnsiSequences.SGR(startcodes));
            }
            result.Append(text);
            if (startcodes.Length != 0)
            {
                startcodes = Array.Empty<byte>();
                if (fg)
                {
                    startcodes = startcodes.Concat(AnsiColorBuilder.GetAnsiCodes(profileDrive.ColorDepth,
                        profileDrive.ForegroundColor,
                        //Color.DefaultForecolor,
                        true)).ToArray();
                }
                if (bg)
                {
                    startcodes = startcodes.Concat(AnsiColorBuilder.GetAnsiCodes(
                        profileDrive.ColorDepth,
                        profileDrive.BackgroundColor,
                        //Color.DefaultBackcolor,
                        false)).ToArray();
                }
                result.Append(AnsiSequences.SGR(startcodes));
            }
            return result.ToString();
        }
   }
}
