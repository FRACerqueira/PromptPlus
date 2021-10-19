// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class ProgressBarOptions : BaseOptions
    {
        public object InterationId { get; set; }

        public Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> UpdateHandler { get; set; }

        public int Witdth { get; set; } = PromptPlus.ProgressgBarWitdth;

        public int DoneDelay { get; private set; } = PromptPlus.ProgressgBarDoneDelay;

        public int ProcessCheckInterval { get; private set; } = PromptPlus.ProgressgBarCheckDelay;

    }
}
