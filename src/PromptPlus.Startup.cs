// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides the global entry point for all Prompt services.
    /// </summary>
    public static partial class PromptPlus
    {
        private static PromptConfig _promptConfig;
        private static readonly IConsole _console;

        /// <summary>
        /// Static constructor. Detects environment capabilities and initializes the internal console driver.
        /// </summary>
        static PromptPlus()
        {
            _console = ConsolePlus.Driver;
            _promptConfig = new PromptConfig();
            ConsolePlus.RunAtomic(() =>
            {
                if (File.Exists(IPromptPlusConfig.NameResourcePromptPlusConfigFile))
                {
                    try
                    {
                        _promptConfig = JsonSerializer.Deserialize<PromptConfig>(
                            File.ReadAllText(IPromptPlusConfig.NameResourcePromptPlusConfigFile), PromptConfig.JsonOptions) ?? _promptConfig;
                    }
                    catch (Exception ex)
                    {
                        throw new AbortException(PromptPlusResources.MsgErrorPromptPlusConfig, ex);
                    }
                }
            });
            ConsolePlus.ActionBeforeExit((drive, ex, ctrlC) =>
            {
                if (ex != null && !ctrlC)
                {
                    var env = ex is AbortException abortEx
                        ? abortEx.Environment
                        : new AbortException("Debug", ex).Environment;

                    WriteErrorLog(env, ex);
                }
            });
        }

        /// <summary>
        /// Persists the environment and exception details to a human-readable log file
        /// located in the user's local application data folder. The path resolves
        /// correctly on Windows, Linux and macOS. Any failure while writing the log is
        /// silently ignored so it never interferes with application shutdown.
        /// </summary>
        /// <param name="env">The environment description associated with the failure.</param>
        /// <param name="exception">The exception (including inner exceptions) to record.</param>
        private static void WriteErrorLog(string env, Exception exception)
        {
            try
            {
                var logFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "PromptPlus");
                Directory.CreateDirectory(logFolder);

                var logPath = Path.Combine(logFolder, "PromptPlus.error.log");

                var nl = Environment.NewLine;
                var sb = new System.Text.StringBuilder();
                sb.Append("========================================").Append(nl);
                sb.AppendFormat(CultureInfo.InvariantCulture, "[{0:yyyy-MM-dd HH:mm:ss}]", DateTime.Now).Append(nl);
                sb.Append("----- Environment -----").Append(nl);
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", env).Append(nl);
                sb.Append("----- Exception -----").Append(nl);

                var current = exception;
                var level = 0;
                while (current != null)
                {
                    var indent = level == 0 ? string.Empty : string.Format(CultureInfo.InvariantCulture, "[Inner {0}] ", level);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}Type   : {1}", indent, current.GetType().FullName).Append(nl);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}Message: {1}", indent, current.Message).Append(nl);
                    if (!string.IsNullOrWhiteSpace(current.StackTrace))
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0}Stack  :", indent).Append(nl);
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", current.StackTrace).Append(nl);
                    }
                    current = current.InnerException;
                    level++;
                }
                sb.Append(nl);

                File.WriteAllText(logPath, sb.ToString());
            }
            catch
            {
                // Ignore any failure while writing the diagnostic log.
            }
        }

        /// <summary>
        /// Gets the global configuration for PromptPlus, allowing for customization of behavior and appearance across all PromptPlus components.
        /// </summary>
        public static IPromptPlusConfig Config => _promptConfig;

        /// <summary>
        /// Gets a factory for interactive controls (input, select, file select, progress, masking, etc.).
        /// Each method returns a fluent configuration object.
        /// </summary>
        public static IControls Controls => new PromptPlusControls(_console, _promptConfig);

        /// <summary>
        /// Gets a factory for creating and emitting visual widgets (banner, dash lines, chart bar, slider, etc.).
        /// </summary>
        public static IWidgets Widgets => new PromptPlusWidgets(_console, _promptConfig);


        /// <summary>
        /// Gets the console interface used by PromptPlus, providing access to console input/output operations and properties.
        /// </summary>
        public static IConsole Console => _console;
    }
}
