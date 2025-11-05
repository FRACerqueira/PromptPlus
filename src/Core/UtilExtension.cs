// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core.Ansi;
using PromptPlusLibrary.Core.Colors;
using PromptPlusLibrary.Core.Markup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Provides utility extension methods.
    /// </summary>
    internal static class UtilExtension
    {
        /// <summary>
        /// Tests if the terminal supports a specific Unicode glyph
        /// </summary>
        /// <param name="glyph">The Unicode character to test</param>
        /// <returns>true if the glyph is supported, false otherwise</returns>
        public static bool IsGlyphSupported(char glyph)
        {
            // First check basic Unicode support through encoding
            if (!HasUnicodeSupport())
            {
                return false;
            }

            // Then check specific terminal capabilities
            return IsGlyphInSupportedRange(glyph);
        }

        public static bool HasUnicodeSupport()
        {
            Encoding encoding = Console.OutputEncoding;

            // Check if encoding supports Unicode
            return encoding.Equals(Encoding.Unicode) ||
                   encoding.Equals(Encoding.UTF8) ||
                   encoding.Equals(Encoding.UTF32) ||
                   encoding.Equals(Encoding.BigEndianUnicode) ||
                   encoding.CodePage == 65001 ||
                   "850,437,65001".Contains(encoding.CodePage.ToString());
        }

        public static bool HasTerminalSupport()
        {
            // Check if running on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Check for Windows Terminal or ConEmu
                string? wtSession = Environment.GetEnvironmentVariable("WT_SESSION");
                string? conEmu = Environment.GetEnvironmentVariable("ConEmuANSI");

                if (!string.IsNullOrEmpty(wtSession) ||
                    (conEmu?.Equals("On", StringComparison.OrdinalIgnoreCase) ?? false))
                {
                    return true;
                }

                // Check Windows version for modern console
                if (Environment.OSVersion.Version.Build >= 22621)
                {
                    try
                    {
                        //https://support.microsoft.com/en-us/windows/command-prompt-and-windows-powershell-6453ce98-da91-476f-8651-5c14d5777c20
                        string keydefaultconsole = (string?)Microsoft.Win32.Registry.GetValue(
                            @"HKEY_CURRENT_USER\Console\%%Startup",
                            "DelegationConsole", null) ?? "";
                        if (keydefaultconsole == "{B23D10C0-E52E-411E-9D5B-C09FDF709C7D}")
                        {
                            return false;
                        }
                        else if (keydefaultconsole == "{00000000-0000-0000-0000-000000000000}") //Automatic select
                        {
                            string InstallPath = (string?)Microsoft.Win32.Registry.GetValue(
                                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\wt.exe",
                                "", null) ?? "";
                            if (InstallPath != "")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    catch
                    {
                        //none;
                    }
                    return true;
                }
            }
            else
            {
                // Check terminal type on Unix-like systems
                string term = Environment.GetEnvironmentVariable("TERM") ?? "";
                return AnsiDetector.IsValidTerminal(term);
            }

            return false;
        }

        private static bool IsGlyphInSupportedRange(char glyph)
        {
            // Check if the glyph is in any of our supported ranges
            int codePoint = glyph;
            if (codePoint >= 32 && codePoint <= 127)
            {
                return true;
            }
            return
                // Box Drawing Characters
                (codePoint >= 0x2500 && codePoint <= 0x257F) ||
                // Block Elements
                (codePoint >= 0x2580 && codePoint <= 0x259F) ||
                // Geometric Shapes
                (codePoint >= 0x25A0 && codePoint <= 0x25FF) ||
                // Mathematical Symbols
                (codePoint >= 0x2200 && codePoint <= 0x22FF);
        }

        /// <summary>
        /// ellipsis character.
        /// </summary>
        public static char Ellipsis = IsGlyphInSupportedRange('…') ? '…' : '.';


        // Cache whether or not internally normalized line endings
        // already are normalized. No reason to do yet another replace if it is.
        private static readonly bool _alreadyNormalized = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Determines whether the specified culture name corresponds to a valid culture recognized by the .NET
        /// framework.
        /// </summary>
        /// <remarks>This method does not throw an exception for invalid or unrecognized culture names.
        /// Use this method to safely check for culture existence before performing culture-specific
        /// operations.</remarks>
        /// <param name="name">The culture name to check.</param>
        /// <returns>true if the specified culture name is valid and recognized; otherwise, false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1309:Use ordinal string comparison", Justification = "By design")]
        public static bool ExistsCulture(this string name)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified culture name corresponds to a valid culture recognized by the .NET
        /// framework.
        /// </summary>
        /// <remarks>This method does not throw an exception for invalid or unrecognized culture names.
        /// Use this method to safely check for culture existence before performing culture-specific
        /// operations.</remarks>
        /// <param name="culture">The culture name to check.</param>
        /// <returns>true if the specified culture name is valid and recognized; otherwise, false.</returns>
        public static bool ExistsCulture(this CultureInfo culture)
        {
            return ExistsCulture(culture.Name);
        }

        /// <summary>
        /// Normalizes the new lines in the specified text.
        /// </summary>
        /// <param name="text">The text to normalize.</param>
        /// <returns>The text with normalized new lines.</returns>
        public static string NormalizeNewLines(this string? text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            if (text.Contains("\r\n"))
            {
                if (!_alreadyNormalized)
                {
                    return text;
                }
                return text.Replace("\r\n", Environment.NewLine, StringComparison.Ordinal);
            }
            if (text.Contains('\n'))
            {
                if (_alreadyNormalized)
                {
                    return text;
                }
                return text.Replace("\n", Environment.NewLine, StringComparison.Ordinal);
            }
            return text;
        }

        public static int LengthTokenColor(this string? text)
        {
            if (text is null)
            {
                return 0;
            }
            string localtext = text.NormalizeNewLines();
            int qtd = 0;
            if (localtext.Length == 0)
            {
                return 0;
            }
            using MarkupTokenizer tokenizer = new(localtext);
            while (tokenizer.MoveNext())
            {
                MarkupToken? token = tokenizer.Current;
                if (token == null)
                {
                    break;
                }
                if (token.Kind == MarkupTokenKind.Text)
                {
                    qtd += token.Value.Length;
                }
            }
            return qtd;
        }

        public static Segment[] ToSegment(this string? text,Style defaultstyletext, IConsole console)
        {
            if (string.IsNullOrEmpty(text))
            {
                return [new Segment("", defaultstyletext)];
            }
            string localtext = text.NormalizeNewLines();
            List<Segment> result = [];

            using MarkupTokenizer tokenizer = new(localtext);

            Stack<Style> stack = new();
            Color currentForeground = defaultstyletext.Foreground;
            Color currentBackground = defaultstyletext.Background;
            stack.Push(new Style(console.ForegroundColor, console.BackgroundColor));
            var onlytext = true;
            while (tokenizer.MoveNext())
            {
                MarkupToken? token = tokenizer.Current;
                if (token == null)
                {
                    break;
                }
                var notfound = false;
                if (token.Kind == MarkupTokenKind.Open)
                {
                    onlytext = false;
                    string[] parts = token.Value.Split([' ']);
                    if (parts.Length == 1 && parts[0].Contains(':'))
                    {
                        int index = parts[0].IndexOf(':');
                        parts = [parts[0][..index], "on", parts[0][(index + 1)..]];
                    }
                    bool foreground = true;
                    Color partForeground = currentForeground;
                    Color partBackground = currentBackground;
                    var first = true;
                    foreach (string part in parts)
                    {
                        if (part.Equals("on", StringComparison.OrdinalIgnoreCase))
                        {
                            foreground = false;
                            continue;
                        }

                        Color? color = ColorTable.GetColorRGB(part);
                        if (color == null)
                        {
                            if (part.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                            {
                                color = ParseHexColor(part);
                                if (color == null)
                                {
                                    return [new Segment(text??string.Empty, defaultstyletext)];
                                }
                            }
                            else if (part.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
                            {
                                color = ParseRgbColor(part);
                                if (color == null)
                                {
                                    return [new Segment(text ?? string.Empty, defaultstyletext)];
                                }
                            }
                            else
                            {
                                if (!first)
                                {
                                    return [new Segment(text ?? string.Empty, defaultstyletext)];
                                }
                                notfound = true;
                                token = new MarkupToken(MarkupTokenKind.Text, $"[{token.Value}]", token.Position);
                                break;
                            }
                        }
                        if (foreground)
                        {
                            partForeground = color.Value!;
                        }
                        else
                        {
                            partBackground = color.Value!;
                        }
                        first = false;
                    }
                    if (!notfound)
                    {
                        stack.Push(new Style(partForeground, partBackground));
                        currentForeground = partForeground;
                        currentBackground = partBackground;
                    }
                }
                if (token.Kind == MarkupTokenKind.Close)
                {
                    onlytext = false;
                    if (stack.Count == 0)
                    {
                        return [new Segment(text ?? string.Empty, defaultstyletext)];
                    }
                    Style oldstyle = stack.Pop();
                    if (stack.Count == 1)
                    {
                        stack.Pop();
                        oldstyle = defaultstyletext;
                    }
                    else
                    {
                        if (stack.Count == 0)
                        {
                            return [new Segment(text ?? string.Empty, defaultstyletext)];
                        }
                        oldstyle = stack.Peek();
                    }
                    currentForeground = oldstyle.Foreground;
                    currentBackground = oldstyle.Background;

                }
                if (token.Kind == MarkupTokenKind.Text)
                {
                    // Get the effective style.
                    result.Add(new Segment(token.Value, new Style(currentForeground, currentBackground)));
                }

            }
            if (stack.Count >=1)
            {
                if (onlytext)
                {
                    stack.Pop();
                }
                else
                {
                    return [new Segment(text ?? string.Empty, defaultstyletext)];
                }
            }
            return [.. result];
        }

        private static Color? ParseHexColor(string hex)
        {
            hex ??= string.Empty;
            hex = hex.Replace("#", string.Empty, StringComparison.Ordinal).Trim();

            try
            {
                if (!string.IsNullOrWhiteSpace(hex))
                {
                    if (hex.Length == 6)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(hex[..2], 16),
                            (byte)Convert.ToUInt32(hex.Substring(2, 2), 16),
                            (byte)Convert.ToUInt32(hex.Substring(4, 2), 16));
                    }
                    else if (hex.Length == 3)
                    {
                        return new Color(
                            (byte)Convert.ToUInt32(new string(hex[0], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[1], 2), 16),
                            (byte)Convert.ToUInt32(new string(hex[2], 2), 16));
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        private static Color? ParseRgbColor(string rgb)
        {
            try
            {
                string normalized = rgb ?? string.Empty;
                if (normalized.Length >= 3)
                {
                    // Trim parentheses
                    normalized = normalized[3..].Trim();

                    if (normalized.StartsWith("(", StringComparison.OrdinalIgnoreCase) &&
                       normalized.EndsWith(")", StringComparison.OrdinalIgnoreCase))
                    {
                        normalized = normalized.Trim('(').Trim(')');

                        string[] parts = normalized.Split([','], StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 3)
                        {
                            return new Color(
                                (byte)Convert.ToInt32(parts[0], CultureInfo.InvariantCulture),
                                (byte)Convert.ToInt32(parts[1], CultureInfo.InvariantCulture),
                                (byte)Convert.ToInt32(parts[2], CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}