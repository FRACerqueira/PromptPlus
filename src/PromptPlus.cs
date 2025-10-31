// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Controls;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Core.Ansi;
using PromptPlusLibrary.Drivers;
using PromptPlusLibrary.PublicLibrary;
using PromptPlusLibrary.Widgets;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides the global entry point for all PromptPlus controls, widgets, configuration access and console services.
    /// </summary>
    /// <remarks>
    /// The static initialization sequence detects terminal capabilities (ANSI, Unicode, color depth, legacy mode),
    /// captures the original console state (culture, encoding, colors) and prepares an internal profile.
    /// Resources are restored automatically on process exit.
    /// </remarks>
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
        /// Static constructor. Detects environment capabilities and initializes the internal console driver.
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
            if ((int)forecolor < 0 || (int)forecolor > 14 || (int)backcolor < 0 || (int)backcolor > 14) // WSL / Linux terminal fallback
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

            _promptConfig = new(unicodesupported, _appConsoleCulture)
            {
                OriginalOutputEncoding = _originalCodePageEncode,
                OriginalForecolor = _originalForecolor,
                OriginalBackcolor = _originalBackcolor
            };

            if (File.Exists(NameResourceConfigFile))
            {
                try
                {
#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
#pragma warning disable CS8601 // Possible null reference assignment.
                    _promptConfig = JsonSerializer.Deserialize<PromptConfig>(File.ReadAllText(NameResourceConfigFile),
                        new JsonSerializerOptions
                        {
                            Converters = { new JsonStringEnumConverter() }
                        });
                    _promptConfig!.Init(unicodesupported, _appConsoleCulture);
                    _promptConfig!.OriginalOutputEncoding = _originalCodePageEncode;
                    _promptConfig!.OriginalForecolor = _originalForecolor;
                    _promptConfig!.OriginalBackcolor = _originalBackcolor;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                }
                catch (Exception)
                {
                    throw;
                }
            }

            _consoledrive.CursorVisible = true;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            _consoledrive.ResetColor();
            _consoledrive.Clear();
        }

        /// <summary>
        /// Gets the global configuration instance applied to newly created controls and widgets.
        /// </summary>
        public static IPromptPlusConfig Config => _promptConfig;

        /// <summary>
        /// Gets a factory for creating and emitting visual widgets (banner, dash lines, chart bar, slider, etc.).
        /// </summary>
        public static IWidgets Widgets => new PromptPlusWidgets(_consoledrive, _promptConfig);

        /// <summary>
        /// Gets a factory for interactive controls (input, select, file select, progress, masking, etc.).
        /// Each method returns a fluent configuration object.
        /// </summary>
        public static IControls Controls => new PromptPlusControls(_consoledrive, _promptConfig);

        /// <summary>
        /// Creates a new configuration file for PromptPlus using the name <see cref="NameResourceConfigFile"/>
        /// </summary>
        /// <param name="foldername">The name of the foder to create file <see cref="NameResourceConfigFile"/> . Must be a valid folder path and cannot be null or empty.</param>
        public static void CreatePromptPlusConfigFile(string foldername)
        { 
            var text = ReadEmbeddedTextResource($"PromptPlusLibrary.Resources.{NameResourceConfigFile}");
            if (string.IsNullOrEmpty(text))
            {
                throw new InvalidOperationException($"Embedded resource PromptPlusLibrary.Resources.{NameResourceConfigFile} not found.");
            }
            File.WriteAllText(Path.Combine(foldername, NameResourceConfigFile), text);
        }

        /// <summary>
        /// Gets the default file name for the PromptPlus resource configuration file.
        /// </summary>
        public static string NameResourceConfigFile => "PromptPlus.config";

        private static string ReadEmbeddedTextResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string result = string.Empty;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return string.Empty;
                }

                using (StreamReader reader = new(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return result;
        }

        /// <summary>
        /// Reconfigures the active console profile (colors, padding, overflow). Thread-safe.
        /// </summary>
        /// <param name="name">Logical profile name.</param>
        /// <param name="config">Delegate used to mutate an <see cref="IProfileSetup"/> prior to applying.</param>
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
        /// Gets the current console driver abstraction providing low-level I/O, color and buffer operations.
        /// </summary>
        public static IConsole Console => _consoledrive;

        #region private methods

        /// <summary>
        /// Restores original console state (culture, encoding, colors) on process exit.
        /// </summary>
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
        /// <summary>
        /// Detects the best color system supported by the current terminal/console environment.
        /// </summary>
        /// <param name="supportsAnsi">Indicates if ANSI sequences are supported.</param>
        /// <returns>The detected <see cref="ColorSystem"/>.</returns>
        private static ColorSystem ColorSystemDetector(bool supportsAnsi)
        {
            if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
            {
                return ColorSystem.NoColors;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!supportsAnsi)
                {
                    return ColorSystem.EightBit;
                }

                if (GetWindowsVersionInformation(out int major, out int build))
                {
                    if ((major == 10 && build >= 15063) || major > 10)
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

            return ColorSystem.EightBit;
        }

        /// <summary>
        /// Attempts to extract Windows version information (major, build) from OS description.
        /// </summary>
        /// <param name="major">Outputs Windows major version.</param>
        /// <param name="build">Outputs Windows build number.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise <c>false</c>.</returns>
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
