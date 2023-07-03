// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PPlus.Controls
{
    internal class MaskEditOptions : BaseOptions
    {

        internal MaskEditOptions(bool showcursor) : base(showcursor)
        {
        }

        public FilterMode FilterType { get; set; } = FilterMode.StartsWith;
        public bool ZeroIsEmpty { get; set; } = true;
        public bool AcceptEmptyValue { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;
        public string MaskValue { get; set; }
        public string DateFmt { get; set; }
        public Style TypeTipStyle { get; set; } = PromptPlus.StyleSchema.Sugestion();
        public Style PositiveStyle { get; set; } = PromptPlus.StyleSchema.Answer();
        public Style NegativeStyle { get; set; } = PromptPlus.StyleSchema.Answer();
        public ControlMaskedType Type { get; set; } = ControlMaskedType.Generic;
        public FormatYear FmtYear { get; set; } = FormatYear.Long;
        public FormatTime FmtTime { get; set; } = FormatTime.HMS;
        public CultureInfo CurrentCulture { get; set; } = null;
        public bool AcceptSignal { get; set; }
        public int AmmountDecimal { get; set; }
        public int AmmountInteger { get; set; }
        public FormatWeek ShowDayWeek { get; set; } = FormatWeek.None;
        public char? FillNumber { get; set; } = null;
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public string DefaultEmptyValue { get; set; }
        public string DefaultValue { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public bool ValidateOnDemand { get; set; }
        public Func<string, string> ChangeDescription { get; set; }
        public int HistoryPageSize { get; set; } = PromptPlus.Config.PageSize;
        public int HistoryMinimumPrefixLength { get; set; } = 0;
        public byte HistoryMaxItems { get; set; } = byte.MaxValue;
        public TimeSpan HistoryTimeout { get; set; } = PromptPlus.Config.HistoryTimeout;
        public string? HistoryFileName { get; set; }
        public Func<SugestionInput, SugestionOutput>? SuggestionHandler { get; set; }
        public bool ShowingHistory { get; set; } = false;
        public bool HistoryEnabled => !string.IsNullOrEmpty(HistoryFileName);
        public bool DescriptionWithInputType { get; set; }

    }
}
