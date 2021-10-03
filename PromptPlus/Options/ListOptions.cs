// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using PromptPlus.Internal;

namespace PromptPlus.Options
{
    public class ListOptions<T> : BaseOptions
    {
        public bool AllowDuplicate { get; set; } = true;

        public int? PageSize { get; set; }

        public int Minimum { get; set; } = 0;

        public bool UpperCase { get; set; } = false;

        public int Maximum { get; set; } = int.MaxValue;

        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();

        public Func<T, string> TextSelector { get; set; } = x => x.ToString();

        internal MaskedOptions MaskedOption { get; set; }

        private string _maskValue;

        internal string MaskValue
        {
            get { return (_maskValue ?? string.Empty).Trim(); }
            set
            {
                _maskValue = value;
            }
        }

        internal bool ShowInputType { get; set; } = true;

        internal bool FillZeros { get; set; } = false;

        internal CultureInfo CurrentCulture { get; set; } = PPlus.DefaultCulture;
    }
}
