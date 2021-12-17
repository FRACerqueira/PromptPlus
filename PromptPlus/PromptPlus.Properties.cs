// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

using PPlus.Resources;
using PPlus.Drivers;
using PPlus.Internal;
using PPlus.Objects;
using Microsoft.Extensions.Logging;

namespace PPlus
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "by design")]
    public static partial class PromptPlus
    {
        internal const int DefaultMinBufferHeight = 10;
        internal const int MaxShowTasks = 1;
        internal const int RollupFactor = 13;
        internal const int SpeedAnimation = 50;
        internal const int SliderWitdth = 30;
        internal const int ProgressgBarWitdth = 30;
        internal const int ProgressgBarDoneDelay = 1000;
        internal const int ProgressgBarCheckDelay = 50;

        internal static object LockObj = new();

        #region internal properties

        internal static int MinBufferHeight { get; set; } = DefaultMinBufferHeight;

        internal static ConsoleColor DefaultForeColor { get; private set; }

        internal static ConsoleColor DefaultBackColor { get; private set; }

        internal static ILogger PPlusLog { get; private set; }

        [ThreadStatic]
        internal static bool ExclusiveMode = false;
        [ThreadStatic]
        private static IConsoleDriver _PPlusConsole;
        internal static IConsoleDriver PPlusConsole
        {
            get
            {
                return _PPlusConsole;
            }
            private set
            {
                _PPlusConsole = value;
            }
        }

        internal static bool NoInterative => PPlusConsole.IsInputRedirected || PPlusConsole.IsOutputRedirected;

        internal static CultureInfo AppCulture { get; set; }

        internal static CultureInfo AppCultureUI { get; set; }

        internal static bool IsRunningWithCommandDotNet { get; set; }

        #endregion

        public static HotKey AbortAllPipesKeyPress { get; set; } = new(UserHotKey.F7, true, false, false);

        internal static HotKey AbortKeyPress { get; set; } = new(ConsoleKey.Escape, false, false, false);

        public static HotKey TooltipKeyPress { get; set; } = new(UserHotKey.F1, false, false, false);

        public static HotKey ResumePipesKeyPress { get; set; } = new(UserHotKey.F2, false, false, false);

        public static HotKey ToggleVisibleDescription { get; set; } = new(UserHotKey.F3, false, false, false);

        public static HotKey UnSelectFilter { get; set; } = new(UserHotKey.F4, false, false, false);

        public static HotKey SwitchViewPassword { get; set; } = new(UserHotKey.F5, true, false, false);

        public static HotKey SelectAll { get; set; } = new(UserHotKey.F5, false, false, false);

        public static HotKey InvertSelect { get; set; } = new(UserHotKey.F6, false, false, false);

        public static HotKey RemoveAll { get; set; } = new(UserHotKey.F4, false, false, false);

        public static HotKey MarkSelect { get; set; } = new(UserHotKey.F8, false, false, false);

        private static CultureInfo s_defaultCulture;

        public static CultureInfo DefaultCulture
        {
            get
            {
                return s_defaultCulture;
            }
            set
            {
                if (!IsImplementedResource(value))
                {
                    if (File.Exists($"PromptPlus.{value.Name}.resources"))
                    {

                        var rm = ResourceManager.CreateFileBasedResourceManager($"PromptPlus", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), null);
                        var innerField = typeof(PromptPlusResources).GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static);
                        innerField.SetValue(null, rm);
                        s_defaultCulture = value;
                    }
                }
                else
                {
                    s_defaultCulture = value;
                }
                PromptPlusResources.Culture = s_defaultCulture;
                Messages.UpdateCulture();
            }
        }

        public static bool EnabledBeep { get; set; } = false;

        public static bool EnabledStandardTooltip { get; set; } = true;

        public static bool EnabledPromptTooltip { get; set; } = true;

        public static bool EnabledAbortKey { get; set; } = true;

        public static bool EnabledAbortAllPipes { get; set; } = true;

        public static char PasswordChar { get; set; } = '#';

        public static bool EnabledLogControl { get; set; } = false;

        public static bool ForwardingLogToLoggerProvider { get; set; } = false;

        private static ILoggerFactory _loggerFactory = null;
        public static ILoggerFactory LoggerFactory
        {
            get { return _loggerFactory; }
            set
            {
                if (value is null)
                {
                    _loggerFactory = null;
                    PPlusLog = null;
                    return;
                }
                _loggerFactory = value;
                PPlusLog = _loggerFactory.CreateLogger(typeof(PromptPlus));
            }
        }

        private static bool IsImplementedResource(CultureInfo cultureInfo)
        {
            if (cultureInfo.IsNeutralCulture || new CultureInfo(cultureInfo.TwoLetterISOLanguageName).IsNeutralCulture)
            {
                return true;
            }
            var code = cultureInfo.Name;
            if (code == "pt-BR")
            {
                return true;
            }
            return false;
        }

    }
}
