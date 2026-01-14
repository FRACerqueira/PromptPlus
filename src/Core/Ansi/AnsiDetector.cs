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
            return !string.IsNullOrWhiteSpace(term) && _regexes.Any(regex => regex.IsMatch(term));
        }

        /// <summary>
        /// Detect whether current environment supports ANSI sequences.
        /// Returns tuple (SupportsAnsi, LegacyConsole).
        /// - SupportsAnsi: true when we can emit ANSI sequences and they will be interpreted.
        /// - LegacyConsole: true when environment is a legacy Windows console that doesn't support ANSI.
        /// </summary>
        public static (bool SupportsAnsi, bool LegacyConsole) Detect()
        {
            // Running on Windows?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // ConEmu explicit opt-in
                string? conEmu = Environment.GetEnvironmentVariable("ConEmuANSI");
                if (!string.IsNullOrEmpty(conEmu) && conEmu.Equals("On", StringComparison.OrdinalIgnoreCase))
                {
                    return (true, false);
                }

                // ANSICON (older ANSI emulator), Windows Terminal (WT_SESSION), and similar env hints
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ANSICON")) ||
                    !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WT_SESSION")) ||
                    IsKnownTerminalProgram())
                {
                    // Even if these env vars are present, we should still try to enable console mode if possible.
                    bool supports = Windows.SupportsAnsi(out bool legacyConsole);
                    return (supports, legacyConsole);
                }

                // Otherwise query Windows console directly (stdout preferred).
                bool supportsAnsi = Windows.SupportsAnsi(out bool legacy);
                return (supportsAnsi, legacy);
            }
            // Non-Windows platforms: decide from TERM/COLORTERM
            return DetectFromTerm();
        }

        private static (bool SupportsAnsi, bool LegacyConsole) DetectFromTerm()
        {
            // If output is redirected, ANSI sequences probably won't be useful.
            if (Console.IsOutputRedirected && Console.IsErrorRedirected)
            {
                return (false, true);
            }

            // Common variable that indicates color support (truecolor/24bit or general color)
            string? colorterm = Environment.GetEnvironmentVariable("COLORTERM");
            if (!string.IsNullOrWhiteSpace(colorterm))
            {
                // If COLORTERM is present, assume ANSI/VT sequences are supported on non-Windows
                return (true, false);
            }

            // TERM-based detection (xterm, screen, linux, cygwin, etc).
            string? term = Environment.GetEnvironmentVariable("TERM");
            if (!string.IsNullOrWhiteSpace(term) && _regexes.Any(regex => regex.IsMatch(term)))
            {
                return (true, false);
            }

            // Fallback: if TERM_PROGRAM suggests a modern terminal (vscode, iTerm, Apple_Terminal)
            if (IsKnownTerminalProgram())
            {
                return (true, false);
            }

            return (false, true);
        }

        private static bool IsKnownTerminalProgram()
        {
            string? termProgram = Environment.GetEnvironmentVariable("TERM_PROGRAM");
            if (string.IsNullOrWhiteSpace(termProgram))
            {
                return false;
            }

            // Some known modern terminal identifiers
            string[] known =
            [
                "vscode",      // VS Code integrated terminal
                "vscode-insiders",
                "iTerm.app",   // iTerm2 on macOS
                "Apple_Terminal",
                "WindowsTerminal",
            ];

            return known.Any(k => termProgram.Contains(k, StringComparison.OrdinalIgnoreCase));
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

        private static partial class Windows
        {
            private const int STD_OUTPUT_HANDLE = -11;
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
                    // If output is redirected to a file/pipe, the console APIs will fail. Respect redirection.
                    if (Console.IsOutputRedirected && Console.IsErrorRedirected)
                    {
                        isLegacy = false;
                        return false;
                    }

                    // Prefer stdout handle, then stderr.
                    nint handle = GetStdHandle(STD_OUTPUT_HANDLE);
                    if (handle == nint.Zero || !GetConsoleMode(handle, out uint mode))
                    {
                        // try stderr as fallback
                        handle = GetStdHandle(STD_ERROR_HANDLE);
                        if (handle == nint.Zero || !GetConsoleMode(handle, out mode))
                        {
                            // Could not get console mode. Fall back to TERM detection (WSL, MSYS, Cygwin)
                            (bool ansiFromTerm, bool legacyFromTerm) = DetectFromTerm();
                            isLegacy = ansiFromTerm ? legacyFromTerm : isLegacy;
                            return ansiFromTerm;
                        }
                    }

                    // If virtual terminal processing already enabled, we support ANSI
                    if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)
                    {
                        return true;
                    }

                    // Attempt to enable virtual terminal processing.
                    uint newMode = mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                    if (!SetConsoleMode(handle, newMode))
                    {
                        // Enabling failed: treat as legacy Windows console.
                        isLegacy = true;
                        return false;
                    }

                    // Successfully enabled.
                    isLegacy = false;
                    return true;
                }
                catch
                {
                    // Unknown failure; assume no ANSI support.
                    isLegacy = true;
                    return false;
                }
            }
        }
    }
}