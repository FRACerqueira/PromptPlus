// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Exception thrown when a PromptPlus operation is aborted.
    /// </summary>
    /// <remarks>
    /// In addition to the error message, every instance transparently captures a snapshot of the
    /// runtime environment at the moment the exception is created: the console/terminal size, the
    /// cursor position, the PromptPlus version, the operating system version and the individual
    /// properties of the active console profile (culture, encodings, colors, terminal capabilities,
    /// ANSI/Unicode support and color depth). The snapshot is exposed as a single human-readable
    /// <see cref="Environment"/> string, populated automatically by the constructor so consumers never
    /// need to provide it. The capture is fully defensive: if the console driver is not yet available
    /// (for example when the exception is raised during static initialization), unavailable values are
    /// rendered as <c>n/a</c> instead of throwing.
    /// </remarks>
    internal sealed class AbortException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the AbortException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AbortException(string message) : base(message)
        {
            Environment = CaptureEnvironment();
        }

        /// <summary>
        /// Initializes a new instance of the AbortException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AbortException(string message, Exception innerException) : base(message, innerException)
        {
            Environment = CaptureEnvironment();
        }

        /// <summary>
        /// Gets a human-readable, multi-line description of the runtime environment captured at the time
        /// the exception was created (screen size, cursor position, PromptPlus and OS versions and the
        /// active console profile details). Unavailable values are rendered as <c>n/a</c>.
        /// </summary>
        public string Environment { get; }

        /// <summary>
        /// Builds a defensive, human-readable snapshot of the runtime environment. Any failure while
        /// gathering an individual value is swallowed so that constructing the exception never throws.
        /// </summary>
        private static string CaptureEnvironment()
        {
            const string na = "n/a";

            var promptPlusVersion = SafeGet(() =>
                Assembly.GetExecutingAssembly()
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString());

            var osName = SafeGet(GetOsName);

            var osVersion = SafeGet(() => RuntimeInformation.OSDescription?.Trim())
                ?? SafeGet(() => System.Environment.OSVersion.ToString());

            var sb = new StringBuilder();
            sb.Append(CultureInfo.InvariantCulture, $"PromptPlus Version : {Text(promptPlusVersion, na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"OS Name            : {Text(osName, na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"OS Version         : {Text(osVersion, na)}").AppendLine();

            var console = SafeGet(() => ConsolePlus.Driver);
            if (console is null)
            {
                sb.Append("Console            : ").Append(na);
                return sb.ToString();
            }

            var width = SafeGet(() => console.Width);
            var height = SafeGet(() => console.Height);
            var (Left, Top) = SafeGet(() => console.GetCursorPosition());

            sb.Append(CultureInfo.InvariantCulture, $"Screen Size        : {width} x {height}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"Cursor Position    : ({Left}, {Top})").AppendLine();

            var profile = SafeGet(() => console.Profile);
            if (profile is null)
            {
                sb.Append("Profile            : ").Append(na);
                return sb.ToString();
            }

            sb.AppendLine("Profile:");
            sb.Append(CultureInfo.InvariantCulture, $"  Name             : {Text(SafeGet(() => profile.ProfileName), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Original Culture : {Text(SafeGet(() => profile.OriginalCulture), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Input Encoding   : {Text(SafeGet(() => profile.DefaultInputEncoding?.WebName), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Output Encoding  : {Text(SafeGet(() => profile.DefaultOutputEncoding?.WebName), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Foreground Color : {Text(SafeGet(() => profile.DefaultForegroundColor.ToString()), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Background Color : {Text(SafeGet(() => profile.DefaultBackgroundColor.ToString()), na)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Is Terminal      : {SafeGet(() => profile.IsTerminal)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Interactive      : {SafeGet(() => profile.Interactive)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Unicode Support  : {SafeGet(() => profile.SupportUnicode)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  ANSI Support     : {SafeGet(() => profile.SupportsAnsi)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Color Depth      : {SafeGet(() => profile.ColorDepth)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Detected ANSI    : {SafeGet(() => profile.DetectedAnsiSupport)}").AppendLine();
            sb.Append(CultureInfo.InvariantCulture, $"  Detected Unicode : {SafeGet(() => profile.DetectedUnicodeSupport)}");

            return sb.ToString();
        }

        private static string Text(string? value, string fallback)
            => string.IsNullOrWhiteSpace(value) ? fallback : value;

        private static string GetOsName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "macOS";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return "FreeBSD";
            }
            return System.Environment.OSVersion.Platform.ToString();
        }

        private static T? SafeGet<T>(Func<T> getter)
        {
            try
            {
                return getter();
            }
            catch
            {
                return default;
            }
        }
    }
}
