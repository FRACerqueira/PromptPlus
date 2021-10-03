// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using PromptPlus.Options;
using PromptPlus.ValueObjects;

namespace PromptPlus.Internal
{
    internal class MaskedOptions : BaseOptions
    {
        #region internals

        internal string DateFmt { get; set; }

        internal MaskedType Type { get; set; } = MaskedType.Generic;

        internal FormatYear FmtYear { get; set; } = FormatYear.Y4;

        internal FormatTime FmtTime { get; set; } = FormatTime.HMS;

        internal CultureInfo CurrentCulture { get; set; } = PPlus.DefaultCulture;

        internal MaskedSignal AcceptSignal { get; set; } = MaskedSignal.None;

        internal bool OnlyDecimal { get; set; }

        internal char? FillNumber { get; set; }

        #endregion

        public bool UpperCase { get; set; } = true;

        private string _maskValue;
        public string MaskValue
        {
            get { return (_maskValue ?? string.Empty).Trim(); }
            set
            {
                _maskValue = value;
            }
        }

        public bool ShowInputType { get; set; } = true;

        public string DefaultValueWitdhoutMask { get; set; }

        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
    }
}
