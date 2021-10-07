// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusControls.ValueObjects
{
    public class SingleProcess
    {
        public string ProcessId { get; set; } = Guid.NewGuid().ToString();
        public Func<CancellationToken, Task<object>> ProcessToRun { get; set; }
        public Func<object, string> ProcessTextResult { get; set; } = x => x == null ? "" : x.ToString();

    }
}
