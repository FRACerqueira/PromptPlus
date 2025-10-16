// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace PromptPlusLibrary.Core.Ansi
{
    internal static partial class AnsiDetector
    {
        private static readonly Regex[] _regexes =
        [
            XtermRegex(), // xterm, PuTTY, Mintty
                RxvtRegex(), // RXVT
                EtermRegex(), // Eterm
                ScreenRegex(), // GNU screen, tmux
                TmuxRegex(), // tmux
                Vt100Regex(), // DEC VT series
                Vt102Regex(), // DEC VT series
                Vt220Regex(), // DEC VT series
                Vt320Regex(), // DEC VT series
                AnsiRegex(), // ANSI
                ScoAnsiRegex(), // SCO ANSI
                CygwinRegex(), // Cygwin, MinGW
                LinuxRegex(), // Linux console
                KonsoleRegex(), // Konsole
                BvtermRegex(), // Bitvise SSH Client
                St256colorRegex(), // Suckless Simple Terminal, st
                AlacrittyRegex(), // Alacritty
            ];

        public static bool IsValidTerminal(string term)
        {
            if (!string.IsNullOrWhiteSpace(term) && _regexes.Any(regex => regex.IsMatch(term)))
            {
                return true;
            }
            return false;
        }

        public static (bool SupportsAnsi, bool LegacyConsole) Detect()
        {
            // Running on Windows?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Running under ConEmu?
                string? conEmu = Environment.GetEnvironmentVariable("ConEmuANSI");
                if (!string.IsNullOrEmpty(conEmu) && conEmu.Equals("On", StringComparison.OrdinalIgnoreCase))
                {
                    return (true, false);
                }

                bool supportsAnsi = Windows.SupportsAnsi(out bool legacyConsole);
                return (supportsAnsi, legacyConsole);
            }
            return DetectFromTerm();
        }

        private static (bool SupportsAnsi, bool LegacyConsole) DetectFromTerm()
        {
            // Check if the terminal is of type ANSI/VT100/xterm compatible.
            string? term = Environment.GetEnvironmentVariable("TERM");
            if (!string.IsNullOrWhiteSpace(term) && _regexes.Any(regex => regex.IsMatch(term)))
            {
                return (true, false);
            }

            return (false, true);
        }

        [GeneratedRegex("^xterm")]
        private static partial Regex XtermRegex();

        [GeneratedRegex("^rxvt")]
        private static partial Regex RxvtRegex();

        [GeneratedRegex("^eterm")]
        private static partial Regex EtermRegex();

        [GeneratedRegex("^screen")]
        private static partial Regex ScreenRegex();

        [GeneratedRegex("tmux")]
        private static partial Regex TmuxRegex();

        [GeneratedRegex("^vt100")]
        private static partial Regex Vt100Regex();

        [GeneratedRegex("^vt102")]
        private static partial Regex Vt102Regex();

        [GeneratedRegex("^vt220")]
        private static partial Regex Vt220Regex();

        [GeneratedRegex("^vt320")]
        private static partial Regex Vt320Regex();

        [GeneratedRegex("ansi")]
        private static partial Regex AnsiRegex();

        [GeneratedRegex("scoansi")]
        private static partial Regex ScoAnsiRegex();

        [GeneratedRegex("cygwin")]
        private static partial Regex CygwinRegex();

        [GeneratedRegex("linux")]
        private static partial Regex LinuxRegex();

        [GeneratedRegex("konsole")]
        private static partial Regex KonsoleRegex();

        [GeneratedRegex("bvterm")]
        private static partial Regex BvtermRegex();

        [GeneratedRegex("^st-256color")]
        private static partial Regex St256colorRegex();

        [GeneratedRegex("alacritty")]
        private static partial Regex AlacrittyRegex();

        unsafe private static partial class Windows
        {
            private const int STD_ERROR_HANDLE = -12;

            private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

            private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

            [LibraryImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static partial bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

            [LibraryImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static partial bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

            [LibraryImport("kernel32.dll", SetLastError = true)]
            private static partial nint GetStdHandle(int nStdHandle);

            [LibraryImport("kernel32.dll")]
            public static partial uint GetLastError();

            public static bool SupportsAnsi(out bool isLegacy)
            {
                isLegacy = false;

                try
                {
                    nint @out = GetStdHandle(STD_ERROR_HANDLE);
                    if (!GetConsoleMode(@out, out uint mode))
                    {
                        // Could not get console mode, try TERM (set in cygwin, WSL-Shell).
                        (bool ansiFromTerm, bool legacyFromTerm) = DetectFromTerm();

                        isLegacy = ansiFromTerm ? legacyFromTerm : isLegacy;
                        return ansiFromTerm;
                    }

                    if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
                    {
                        isLegacy = true;

                        // Try enable ANSI support.
                        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                        if (!SetConsoleMode(@out, mode))
                        {
                            // Enabling failed.
                            return false;
                        }

                        isLegacy = false;
                    }

                    return true;
                }
                catch
                {
                    // All we know here is that we don't support ANSI.
                    return false;
                }
            }
        }
    }
}
