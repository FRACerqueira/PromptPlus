// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary.Core
{
    internal interface IConsoleExtend : IDisposable
    {
        bool WriteToErroOutput { get; set; }

        SemaphoreSlim ExclusiveContext { get; }

        (int Left, int Top, int scrolled) PreviewCursorPosition(int left, int top);
    }
}
