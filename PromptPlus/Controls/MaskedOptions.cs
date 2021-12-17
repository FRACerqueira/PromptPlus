// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal class MaskedOptions : BaseOptions
    {
        public string DateFmt { get; set; }
        public MaskedType Type { get; set; } = MaskedType.Generic;
        public FormatYear FmtYear { get; set; } = FormatYear.Y4;
        public FormatTime FmtTime { get; set; } = FormatTime.HMS;
        public CultureInfo CurrentCulture { get; set; } = PromptPlus.DefaultCulture;
        public MaskedSignal AcceptSignal { get; set; } = MaskedSignal.None;
        public bool OnlyDecimal => AmmountInteger == 0;
        public char? FillNumber { get; set; }
        public int AmmountDecimal { get; set; }
        public int AmmountInteger { get; set; }
        public object DefaultObject { get; set; }
        public bool UpperCase { get; set; } = true;

        private string _maskValue;
        public string MaskValue
        {
            get { return _maskValue ?? string.Empty; }
            set
            {
                _maskValue = value;
            }
        }

        public bool ValidateOnDemand { get; set; }
        public bool ShowInputType { get; set; } = true;
        public FormatWeek ShowDayWeek { get; set; } = FormatWeek.None;
        public string DefaultValueWitdMask { get; set; }
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<ResultMasked, string> DescriptionSelector { get; set; }
        public Func<string, string> TransformItems { get; set; }
    }
}
