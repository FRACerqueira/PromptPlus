// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Environment;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Provides the global/default configuration applied to all PromptPlus controls (culture, hotkeys, symbols, pagination and general behavior).
    /// </summary>
    /// <remarks>
    /// Use <see cref="IPromptPlusConfig"/> for consuming configuration settings. This sealed implementation handles
    /// culture resource switching and Unicode symbol capability detection.
    /// </remarks>
    internal sealed class PromptConfig : IPromptPlusConfig
    {
        private char? _yesChar;
        private char? _noChar;
        private CultureInfo _defaultCulture;
        private double _defaultContrastRatio = 2.7;
        private byte _defaultMaxLenghtFilterText = 25;
        private byte _defaultPageSize;
        private byte _defaultChartWidth = 80;
        private char _defaultSecretChar = '#';
        private char _defaultPromptMaskEdit = '_';
        private byte _defaultProgressBarWidth = 40;
        private byte _defaultSliderWidth = 30;
        private byte _defaultSwitchWidth = 4;
        private string _defaultSufixAfterPrompt = ": ";
        private string _defaultPrefixExtraInfo = " (";
        private string _defaultSuffixExtraInfo = ")";
        private bool _defaultRemoveHandlerCtrlC ;
        private static readonly HashSet<string> _supportedCultures = new(StringComparer.OrdinalIgnoreCase)
        {
            "en-us", "pt-br", "de-de", "es-es", "fr-fr", "it-it", "ja-jp", "ko-kr", "nl-be", "ru-ru", "zh-cn"
        };
        internal static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new CultureInfoJsonConverter(),
                new ColorJsonConverter(),
                new HotKeyJsonConverter()
            }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptConfig"/> class using the current thread culture and Unicode symbols enabled.
        /// </summary>
        public PromptConfig()
        {
            _defaultCulture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc/>
        public void ToFile(string foldername)
        {
            ArgumentNullException.ThrowIfNull(foldername);
            File.WriteAllText(Path.Combine(foldername, IPromptPlusConfig.NameResourcePromptPlusConfigFile),
                JsonSerializer.Serialize(this, JsonOptions));
        }

        /// <inheritdoc/>
        public string SufixAfterPrompt
        {
            get => _defaultSufixAfterPrompt;
            set => _defaultSufixAfterPrompt = value ?? "";
        }

        /// <inheritdoc/>
        public string PrefixExtraInfo
        {
            get => _defaultPrefixExtraInfo;
            set => _defaultPrefixExtraInfo = value ?? "";
        }
        /// <inheritdoc/>
        public string SuffixExtraInfo
        {
            get => _defaultSuffixExtraInfo;
            set => _defaultSuffixExtraInfo = value ?? "";
        }

        /// <inheritdoc/>
        public double ContrastRatio
        {
            get => _defaultContrastRatio;
            set => _defaultContrastRatio = value;
        }

        /// <inheritdoc/>
        public char YesChar
        {
            get => _yesChar ?? PromptPlusResources.YesChar[0];
            set => _yesChar = value;
        }

        /// <inheritdoc/>
        public byte PageSize
        {
            get => _defaultPageSize;
            set => _defaultPageSize = value;
        }

        /// <inheritdoc/>
        public byte ChartWidth
        {
            get => _defaultChartWidth   ;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                else if (value > 200)
                {
                    value = 200;
                }
                _defaultChartWidth = value;
            }
        }

        /// <inheritdoc/>
        public char SecretChar
        {
            get => _defaultSecretChar;
            set => _defaultSecretChar = value;
        }


        /// <inheritdoc/>
        public char PromptMaskEdit
        {
            get => _defaultPromptMaskEdit;
            set => _defaultPromptMaskEdit = value;
        }

        /// <inheritdoc/>
        public byte ProgressBarWidth
        {
            get => _defaultProgressBarWidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultProgressBarWidth = value;
            }
        }

        /// <inheritdoc/>
        public byte SliderWidth
        {
            get => _defaultSliderWidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                _defaultSliderWidth = value;
            }
        }

        /// <inheritdoc/>
        public byte SwitchWidth
        {
            get => _defaultSwitchWidth;
            set
            {
                if (value < 4)
                {
                    value = 4;
                }
                else if (value > 10)
                {
                    value = 10;
                }
                _defaultSwitchWidth = value;
            }
        }

        /// <inheritdoc/>
        public char NoChar
        {
            get => _noChar ?? PromptPlusResources.NoChar[0];
            set => _noChar = value;
        }

        /// <inheritdoc/>
        public byte MaxLenghtFilterText
        {
            get => _defaultMaxLenghtFilterText;
            set
            {
                if (value > 50)
                {
                    value = 50;
                }

                if (value < 5)
                {
                    value = 5;
                }

                _defaultMaxLenghtFilterText = value;
            }
        }

        /// <inheritdoc/>
        public bool RemoveHandlerCtrlC
        {
            get => _defaultRemoveHandlerCtrlC;
            set => _defaultRemoveHandlerCtrlC = value;
        }

        /// <inheritdoc/>
        public bool EnabledAbortKey { get; set; } = true;

        /// <inheritdoc/>
        public bool ShowMessageAbortKey { get; set; } = true;

        /// <inheritdoc/>
        public bool ShowTooltip { get; set; } = true;

        /// <inheritdoc/>
        public bool HideAfterFinish { get; set; }

        /// <inheritdoc/>
        public bool HideOnAbort { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(CultureInfoJsonConverter))]
        public CultureInfo DefaultCulture
        {
            get => _defaultCulture ?? CultureInfo.InvariantCulture;
            set
            {
                value ??= CultureInfo.InvariantCulture;
                if (!ImplementedResource(value) && File.Exists($"PromptPlus.{value.Name}.resources"))
                {
                    ResourceManager rm = ResourceManager.CreateFileBasedResourceManager(
                        "PromptPlus",
                        Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
                        null
                    );
                    FieldInfo innerField = typeof(PromptPlusResources).GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static)!;
                    innerField.SetValue(null, rm);
                }
                PromptPlusResources.Culture = value;
                _defaultCulture = value;
            }
        }

        /// <inheritdoc/>
        public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Sunday;

        /// <inheritdoc/>
        public HotKey HotKeyFilterAllSelected { get; set; } = HotKey.DefaultFilterAllSelected;

        /// <inheritdoc/>
        public HotKey HotKeyChartBarSwitchLayout { get; set; } = HotKey.DefaultChartBarSwitchLayout;

        /// <inheritdoc/>
        public HotKey HotKeyChartBarSwitchLegend { get; set; } = HotKey.DefaultChartBarSwitchLegend;

        /// <inheritdoc/>
        public HotKey HotKeyChartBarSwitchOrder { get; set; } = HotKey.DefaultChartBarSwitchOrder;

        /// <inheritdoc/>
        public HotKey HotKeyToggleAll { get; set; } = HotKey.DefaultToggleAll;

        /// <inheritdoc/>
        public HotKey HotKeyToggleFullPath { get; set; } = HotKey.DefaultToggleFullPath;

        /// <inheritdoc/>
        public HotKey HotKeySelectWildcard { get; set; } = HotKey.DefaultToggleWildcard;

        /// <inheritdoc/>
        public HotKey HotKeyInputPasswordView { get; set; } = HotKey.DefaultInputPasswordView;

        /// <inheritdoc/>
        public HotKey HotKeyCalendarSwitchNotes { get; set; } = HotKey.DefaultCalendarSwitchNotes;

        /// <inheritdoc/>
        public HotKey HotKeyInputHistoryView { get; set; } = HotKey.DefaultInputHistoryView;

        /// <inheritdoc/>
        public HotKey HotKeyTooltip { get; set; } = HotKey.DefaultTooltip;

        /// <inheritdoc/>
        public HotKey HotKeyTooltipShowHide { get; set; } = HotKey.DefaultTooltipShowHide;

        /// <inheritdoc/>
        public HotKey HotKeyAbortKeyPress { get; } = HotKey.DefaultAbortKeyPress;

        public string PaginationTemplateValue( int totalCount, int selectedpage, int pagecount)
        {
            var template = PromptPlusResources.PaginationTemplate;
            var paginationFormat = CompositeFormat.Parse(template);
            return string.Format(DefaultCulture, paginationFormat, totalCount, selectedpage, pagecount);
        }

        private static bool ImplementedResource(CultureInfo cultureInfo)
        {
            return _supportedCultures.Contains(cultureInfo.Name.ToLowerInvariant());
        }
    }
}
