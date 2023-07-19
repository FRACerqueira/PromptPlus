// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PPlus.Controls
{
    internal class WaitOptions : BaseOptions
    {
        private WaitOptions()
        {
            throw new PromptPlusException("WaitOptions CTOR NotImplemented");
        }

        internal WaitOptions(bool showcursor) : base(showcursor)
        {
            Steps = new();
            States = new();
        }
        public TimeSpan TimeDelay { get; set; }
        public string OverWriteTitlekName { get; set; }
        public bool WaitTime { get; set; }
        public bool ShowCountdown { get; set; }
        public List<Action<CancellationToken>> Steps { get; set; }
        public List<StateProcess> States { get; set; }
        public Style SpinnerStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public string? Finish { get; set; }
        public Spinners Spinner { get; set; } = new Spinners(SpinnersType.Ascii, false);
        public int MaxDegreeProcess { get; set; } = Environment.ProcessorCount;
        public bool ShowElapsedTime { get; set; }
    }
}
