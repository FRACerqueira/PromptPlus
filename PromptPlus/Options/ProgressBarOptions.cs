// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

using PromptPlus.ValueObjects;

namespace PromptPlus.Options
{
    public class ProgressBarOptions : BaseOptions
    {
        public object InterationId { get; set; }

        public Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> UpdateHandler { get; set; }

        public int Witdth { get; set; } = PPlus.ProgressgBarWitdth;

        internal int DoneDelay { get; private set; } = PPlus.ProgressgBarDoneDelay;

        internal int ProcessCheckInterval { get; private set; } = PPlus.ProgressgBarCheckDelay;

    }
}
