// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        internal const int MaxShowTasks = 1;
        internal const int RollupFactor = 13;
        internal const int SpeedAnimation = 100;
        internal const int SliderWitdth = 30;
        internal const int ProgressgBarWitdth = 30;
        internal const int ProgressgBarDoneDelay = 1000;
        internal const int ProgressgBarCheckDelay = 50;

        static PromptPlus()
        {
            AppCulture = Thread.CurrentThread.CurrentCulture;
            AppCultureUI = Thread.CurrentThread.CurrentUICulture;
            s_defaultCulture = AppCulture;
            LoadConfigFromFile();
        }

        internal static CultureInfo AppCulture { get; private set; }

        internal static CultureInfo AppCultureUI { get; private set; }

        public static HotKey AbortAllPipesKeyPress { get; set; } = new(ConsoleKey.X, true, false, false);

        public static HotKey AbortKeyPress { get; set; } = new(ConsoleKey.Escape, false, false, false);

        public static HotKey TooltipKeyPress { get; set; } = new(ConsoleKey.F1, false, false, false);

        public static HotKey ResumePipesKeyPress { get; set; } = new(ConsoleKey.F2, false, false, false);

        public static HotKey UnSelectFilter { get; set; } = new(ConsoleKey.F, true, false, false);

        public static HotKey SwitchViewPassword { get; set; } = new(ConsoleKey.V, true, false, false);

        public static HotKey SelectAll { get; set; } = new(ConsoleKey.A, true, false, false);

        public static HotKey InvertSelect { get; set; } = new(ConsoleKey.I, true, false, false);

        public static HotKey RemoveAll { get; set; } = new(ConsoleKey.R, true, false, false);


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

        internal static string LocalizateFormatException(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return Messages.InvalidTypeBoolean;
                case TypeCode.Byte:
                    return Messages.InvalidTypeByte;
                case TypeCode.Char:
                    return Messages.InvalidTypeChar;
                case TypeCode.DateTime:
                    return Messages.InvalidTypeDateTime;
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Messages.InvalidTypeNumber;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.String:
                    break;
            }
            return Messages.Invalid;
        }
    }
}
