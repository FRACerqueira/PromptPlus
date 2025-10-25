// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.WaitProcess
{
    internal sealed class ItemWaitTask(StateProcess stateProcess, object? parameter, Action<object?, CancellationToken> processAction)
    {
        public object? Parameter { get; } = parameter;
        public Action<object?, CancellationToken> ProcessAction { get; } = processAction;
        public StateProcess State { get; set; } = stateProcess;
        public Task? TaskRunning { get; set; }
    }
}
