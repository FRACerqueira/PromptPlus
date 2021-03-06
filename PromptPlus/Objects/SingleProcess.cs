// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Objects
{
    public struct SingleProcess
    {
        public SingleProcess()
        {
            ProcessToRun = null;
            ProcessId = Guid.NewGuid().ToString();
            ProcessTextResult = x => x == null ? "" : x.ToString();
        }

        public SingleProcess(Func<CancellationToken, Task<object>> processToRun, string idProcess = null, Func<object, string> processTextResult = null)
        {
            ProcessToRun = processToRun;
            ProcessId = idProcess ?? Guid.NewGuid().ToString();
            ProcessTextResult = processTextResult ?? (x => x == null ? "" : x.ToString());
        }

        public string ProcessId { get; }
        public Func<CancellationToken, Task<object>> ProcessToRun { get; }
        public Func<object, string> ProcessTextResult { get; }

    }
}
