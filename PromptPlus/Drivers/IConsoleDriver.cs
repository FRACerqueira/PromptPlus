// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PPlus.Objects;

namespace PPlus.Drivers
{
    public interface IConsoleDriver : ISystemConsole
    {
        bool NoColor { get; }
        bool NoInterative { get; }
        void ClearLine(int top);
        void ClearRestOfLine(ConsoleColor? color);
        ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken cancellationToken);
        void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null);
        void Write(params ColorToken[] tokens);
        void WriteLine(string value = null, ConsoleColor? color = null, ConsoleColor? colorbg = null);
        void WriteLine(params ColorToken[] tokens);
        bool IsRunningTerminal { get; }
    }
}
