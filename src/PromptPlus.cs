// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Controls;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Core.Ansi;
using PromptPlusLibrary.Drivers;
using PromptPlusLibrary.Widgets;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents all controls, methods, properties and extensions for <see cref="PromptPlus"/>.
    /// </summary>
    public static partial class PromptPlus
    {
        private static readonly CultureInfo _appConsoleCulture;
        private static readonly Encoding _originalCodePageEncode;
        private static readonly PromptConfig _promptConfig;
        private static IConsole _consoledrive;
        private static readonly ConsoleColor _originalForecolor;
        private static readonly ConsoleColor _originalBackcolor;
#if NET9_0_OR_GREATER
        private static readonly Lock _lockprofile = new();
#else
        private static readonly object _lockprofile = new();
#endif

        /// <summary>
        /// The console drive init.
        /// </summary>
        static PromptPlus()
        {
            _appConsoleCulture = CultureInfo.CurrentCulture;
            _originalCodePageEncode = System.Console.OutputEncoding;
            (bool localSupportsAnsi, bool localIsLegacy) = AnsiDetector.Detect();
            bool termdetect = UtilExtension.HasTerminalSupport();
            ColorSystem colordetect = ColorSystemDetector(localSupportsAnsi);
            bool unicodesupported = UtilExtension.HasUnicodeSupport();

            ConsoleColor forecolor = System.Console.ForegroundColor;
            ConsoleColor backcolor = System.Console.BackgroundColor;
            if ((int)forecolor < 0 || (int)forecolor > 14 || (int)backcolor < 0 || (int)backcolor > 14) //wsl Linux terminal
            {
                forecolor = ConsoleColor.White;
                backcolor = ConsoleColor.Black;
            }
            _originalForecolor = forecolor;
            _originalBackcolor = backcolor;

            ProfileDrive profileDrive = new(
                "PromptPlus",
                termdetect,
                unicodesupported,
                localSupportsAnsi,
                localIsLegacy,
                colordetect,
                forecolor,
                backcolor,
                Overflow.None,
                0,
                0);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _consoledrive = new ConsoleDriveWindows(profileDrive);
            }
            else
            {
                _consoledrive = new ConsoleDriveLinux(profileDrive);
            }
            _promptConfig = new(unicodesupported, _appConsoleCulture);

            _consoledrive.CursorVisible = true;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            _consoledrive.ResetColor();
            _consoledrive.Clear();
        }

        /// <summary>
        /// Get global properties config for PromptPlus
        /// </summary>
        public static PromptConfig Config => _promptConfig;

        /// <summary>
        /// Represents all Widgets for PromptPlus
        /// </summary>
        public static IWidgets Widgets => new PromptPlusWidgets(_consoledrive, _promptConfig);


        /// <summary>
        /// Represents all controls for PromptPlus
        /// </summary>
        public static IControls Controls => new PromptPlusControls(_consoledrive, _promptConfig);

        /// <summary>
        /// 
        /// </summary>
        public static void ProfileConfig(string name, Action<IProfileSetup> config)
        {
            lock (_lockprofile)
            {
                ProfileDrive profileDrive;
                using (_consoledrive.ExclusiveContext())
                {
                    ProfileSetup newprofile = new()
                    {
                        DefaultConsoleForegroundColor = _consoledrive.ForegroundColor,
                        DefaultConsoleBackgroundColor = _consoledrive.BackgroundColor,
                        PadLeft = _consoledrive.PadLeft,
                        PadRight = _consoledrive.PadRight,
                        OverflowStrategy = _consoledrive.OverflowStrategy
                    };
                    config(newprofile);
                    profileDrive = new ProfileDrive(
                         name,
                         _consoledrive.IsTerminal,
                         _consoledrive.IsUnicodeSupported,
                         _consoledrive.SupportsAnsi,
                         _consoledrive.IsLegacy,
                         _consoledrive.ColorDepth,
                         newprofile.DefaultConsoleForegroundColor,
                         newprofile.DefaultConsoleBackgroundColor,
                         newprofile.OverflowStrategy,
                         newprofile.PadLeft,
                         newprofile.PadRight);
                    _consoledrive.ResetColor();
                    _consoledrive.Clear();
                    _consoledrive.Out.Flush();
                }
                ((IConsoleExtend)_consoledrive).Dispose();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _consoledrive = new ConsoleDriveWindows(profileDrive);
                }
                else
                {
                    _consoledrive = new ConsoleDriveLinux(profileDrive);
                }
                _consoledrive.ResetColor();
                _consoledrive.Clear();
            }
        }

        /// <summary>
        /// Gets the Console drive.
        /// </summary>
        public static IConsole Console => _consoledrive;

        #region private methods

        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            ((IConsoleExtend)_consoledrive).Dispose();
            Thread.CurrentThread.CurrentCulture = _appConsoleCulture;
            System.Console.OutputEncoding = _originalCodePageEncode;
            System.Console.ForegroundColor = _originalForecolor;
            System.Console.BackgroundColor = _originalBackcolor;
            System.Console.ResetColor();
        }


        // Adapted from https://github.com/willmcgugan/rich/blob/f0c29052c22d1e49579956a9207324d9072beed7/rich/console.py#L391
        private static ColorSystem ColorSystemDetector(bool supportsAnsi)
        {
            // No colors?
            if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
            {
                return ColorSystem.NoColors;
            }

            // Windows?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!supportsAnsi)
                {
                    // Figure out what we should do here.
                    // Does really all Windows terminals support
                    // eight-bit colors? Probably not...
                    return ColorSystem.EightBit;
                }

                // Windows 10.0.15063 and above support true color,
                // and we can probably assume that the next major
                // version of Windows will support true color as well.
                if (GetWindowsVersionInformation(out int major, out int build))
                {
                    if (major == 10 && build >= 15063)
                    {
                        return ColorSystem.TrueColor;
                    }
                    else if (major > 10)
                    {
                        return ColorSystem.TrueColor;
                    }
                }
            }
            else
            {
                string? colorTerm = Environment.GetEnvironmentVariable("COLORTERM");
                if (!string.IsNullOrWhiteSpace(colorTerm))
                {
                    if (colorTerm.Equals("truecolor", StringComparison.OrdinalIgnoreCase) ||
                       colorTerm.Equals("24bit", StringComparison.OrdinalIgnoreCase))
                    {
                        return ColorSystem.TrueColor;
                    }
                }
            }

            // Should we default to eight-bit colors?
            return ColorSystem.EightBit;
        }

        private static bool GetWindowsVersionInformation(out int major, out int build)
        {
            major = 0;
            build = 0;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            Regex regex = RegexCheckWindows();
            Match match = regex.Match(RuntimeInformation.OSDescription);
            if (match.Success && int.TryParse(match.Groups["major"].Value, out major))
            {
                if (int.TryParse(match.Groups["build"].Value, out build))
                {
                    return true;
                }
            }

            return false;
        }

        [GeneratedRegex("Microsoft Windows (?'major'[0-9]*).(?'minor'[0-9]*).(?'build'[0-9]*)\\s*$")]
        private static partial Regex RegexCheckWindows();

        #endregion
    }
}
