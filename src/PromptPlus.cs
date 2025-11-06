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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
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

            _consoledrive = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new ConsoleDriveWindows(profileDrive)
                : (IConsole)new ConsoleDriveLinux(profileDrive);

            _promptConfig = new(unicodesupported, _appConsoleCulture);

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
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                }
                catch (Exception ex)
                {
                    WriteCrashLog(typeof(PromptPlus), ex);
                    throw;
                }
            }

            _consoledrive.CursorVisible = true;

            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                if (((IConsoleExtend)Console).AbortedByCtrlC)
                {
                    AppDomainUnloadedException error = new("Press Ctrl+C or Ctrl+Break");
                    try
                    {
                        WriteCrashLog(typeof(PromptPlus), error);
                        _promptConfig.AfterError?.Invoke(error);
                        System.Console.WriteLine($"{error}");
                    }
                    catch
                    {
                        //none
                    }
                }
                if (_promptConfig.ResetBasicStateAfterExist)
                {
                    ResetState();
                }
            };

            AppDomain.CurrentDomain.FirstChanceException += ((object? o, FirstChanceExceptionEventArgs e) =>
            {
                if (e.Exception.GetType() == typeof(AppDomainUnloadedException))
                {
                    if (_consoledrive.UserPressKeyAborted && ((IConsoleExtend)_consoledrive).IsExitDefaultCancel)
                    {
                        ((IConsoleExtend)_consoledrive).ResetTokenCancelPress();
                        Environment.Exit(1);
                    }
                }
            });

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try
                {
                    if (((IConsoleExtend)Console).AbortedByCtrlC)
                    {
                        AppDomainUnloadedException error = new("Press Ctrl+C or Ctrl+Break");
                        WriteCrashLog(typeof(PromptPlus), error);
                        _promptConfig.AfterError?.Invoke(error);
                        System.Console.WriteLine($"{error}");
                    }
                    else
                    {
                        WriteCrashLog(typeof(PromptPlus), (Exception)e.ExceptionObject);
                        _promptConfig.AfterError?.Invoke((Exception)e.ExceptionObject);
                    }
                }
                catch
                {
                    //none
                }
                if (_promptConfig.ResetBasicStateAfterExist)
                {
                    ResetState();
                }
            };
            _consoledrive.ResetColor();
            _consoledrive.Clear();
            _consoledrive.RemoveCancelKeyPress();
        }

        /// <summary>
        /// Gets the global configuration instance applied to newly created controls and widgets.
        /// </summary>
        public static IPromptPlusConfig Config => _promptConfig;

        /// <summary>
        /// Gets a factory for creating and emitting visual widgets (banner, dash lines, chart bar, slider, etc.).
        /// </summary>
        public static IWidgets Widgets => new PromptPlusWidgets((IConsoleExtend)_consoledrive, _promptConfig);

        /// <summary>
        /// Gets a factory for interactive controls (input, select, file select, progress, masking, etc.).
        /// Each method returns a fluent configuration object.
        /// </summary>
        public static IControls Controls => new PromptPlusControls((IConsoleExtend)_consoledrive, _promptConfig);

        /// <summary>
        /// Creates a new configuration file for PromptPlus using the name <see cref="NameResourceConfigFile"/>
        /// </summary>
        /// <param name="foldername">The name of the foder to create file <see cref="NameResourceConfigFile"/> . Must be a valid folder path and cannot be null or empty.</param>
        public static void CreatePromptPlusConfigFile(string foldername)
        {

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
            File.WriteAllText(Path.Combine(foldername, NameResourceConfigFile),
                JsonSerializer.Serialize(_promptConfig, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                }
            ));
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        }

        /// <summary>
        /// Gets the default file name for the PromptPlus resource configuration file.
        /// </summary>
        public static string NameResourceConfigFile => "PromptPlus.config";

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

                _consoledrive = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? new ConsoleDriveWindows(profileDrive)
                    : (IConsole)new ConsoleDriveLinux(profileDrive);
                _consoledrive.ResetColor();
                _consoledrive.Clear();
            }
        }

        /// <summary>
        /// Gets the current console driver abstraction providing low-level I/O, color and buffer operations.
        /// </summary>
        public static IConsole Console => _consoledrive;

        #region private methods

        internal static void ResetState()
        {
            if (Console.SupportsAnsi)
            {
                System.Console.Write(AnsiSequences.SM());
            }
            else
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    System.Console.CursorVisible = true;
                }
            }
            if (_consoledrive.IsEnabledSwapScreen)
            {
                if (_consoledrive.CurrentBuffer == TargetScreen.Secondary)
                {
                    System.Console.Write("\u001b[?1049l");
                }
            }
            Thread.CurrentThread.CurrentCulture = _appConsoleCulture;
            System.Console.ForegroundColor = _originalForecolor;
            System.Console.BackgroundColor = _originalBackcolor;
            System.Console.ResetColor();
        }

        private static (string key, string value)[] GetAllProperties(object obj)
        {
            List<(string key, string value)> result = [];
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                try
                {
                    string value = (prop.GetValue(obj)?.ToString()) ?? "null";
                    if (prop.PropertyType == typeof(SemaphoreSlim))
                    {
                        if (value != null)
                        {
                            value = $"SemaphoreSlim:Count:{((SemaphoreSlim)prop.GetValue(obj)!).CurrentCount}";
                        }
                    }
                    else if (prop.PropertyType == typeof(Encoding))
                    {
                        if (value != null)
                        {
                            value = $"{((Encoding)prop.GetValue(obj)!).EncodingName}";
                        }
                    }
                    result.Add((prop.Name, value!));
                }
                catch (TargetInvocationException)
                {
                    //skip
                }
            }

            return [.. result];
        }

        internal static void WriteCrashLog(Type source, Exception? ex)
        {
            var platform = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var version = source.Assembly.GetName()?.Version?.ToString() ?? string.Empty;
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            (string key, string value)[] consoleproperties = GetAllProperties(_consoledrive);
            (string key, string value)[] configproperties = GetAllProperties(_promptConfig);
            (string key, string value)[] optionproperties = _promptConfig.TraceBaseControlOptions == null ? [] : GetAllProperties(_promptConfig.TraceBaseControlOptions);

            string folderPath = Path.Combine(_promptConfig.FolderLog, "PromptPlus.Log");
            var logFileName = $"PromptPlusLog{DateTime.Now:yyyyMMdd}.log";
            string filePath = Path.Combine(folderPath, logFileName);
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, "PromptPlusLog*.log");
                foreach (var file in files)
                {
                    FileInfo fi = new(file);
                    if (DateOnly.FromDateTime(fi.CreationTime) < DateOnly.FromDateTime(DateTime.Now).AddDays(-7))
                    {
                        File.Delete(file);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filePath, true);
                writer.WriteLine($"PromptPlus({version}) Crash Log : {DateTime.Now}");
                writer.WriteLine($"Platform : {platform} UserInteractive : {Environment.UserInteractive}");
                writer.WriteLine($"Framework : {framework}");
                if (_promptConfig.TraceCurrentFileNameControl != null)
                {
                    writer.WriteLine($"File Source : {_promptConfig.TraceCurrentFileNameControl}");
                }
                writer.WriteLine($"CurrentThread Culture : {culture}");
                writer.WriteLine($"UserPressKeyAborted : {_consoledrive.UserPressKeyAborted}");
                writer.WriteLine($"CancellationRequested : {((IConsoleExtend)_consoledrive).TokenCancelPress.IsCancellationRequested}");
                if (ex != null)
                {
                    writer.WriteLine("Exception Details");
                    writer.WriteLine("-----------------");
                    writer.WriteLine($"  {ex}");
                }
                if (optionproperties != null)
                {
                    writer.WriteLine($"Options properties");
                    writer.WriteLine($"------------------");
                    foreach ((string? key, string? value) in optionproperties)
                    {
                        writer.WriteLine($"  {key}: {value}");
                    }
                }
                writer.WriteLine($"Console properties");
                writer.WriteLine($"------------------");
                foreach ((string? key, string? value) in consoleproperties)
                {
                    writer.WriteLine($"  {key}: {value}");
                }
                writer.WriteLine($"Config properties");
                writer.WriteLine($"-----------------");
                foreach ((string? key, string? value) in configproperties)
                {
                    writer.WriteLine($"  {key}: {value}");
                }
                writer.WriteLine(new string('=', 80));
            }
            finally
            {
                try
                {
                    writer?.Flush();
                    writer?.Close();
                }
                catch
                {
                    //none
                }
                writer?.Dispose();
            }
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
