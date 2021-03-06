using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    internal class ReadlineOptions : BaseOptions
    {
        public int? PageSize { get; set; }
        public string InitialValue { get; set; }
        public string InitialError { get; set; }
        public int MinimumPrefixLength { get; set; } = 3;
        public bool FinishWhenHistoryEnter { get; set; }
        public bool SaveHistoryAtFinish { get; set; } = true;
        public byte MaxHistory { get; set; } = byte.MaxValue;
        public TimeSpan TimeoutHistory { get; set; } = new TimeSpan(365, 0, 0, 0, 0);
        public string FileNameHistory { get; set; }
        public bool EnabledHistory { get; set; } = false;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
    }
}
