// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusControls.Drivers
{
    public interface IConsoleDriver
    {
        ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken);
        void Beep();
        void ClearLine(int top);
        void ClearRestOfLine(ConsoleColor? color);
        ConsoleKeyInfo ReadKey();
        void Write(string value, ConsoleColor color, ConsoleColor? colorbg = null);
        void WriteLine();
        void SetCursorPosition(int left, int top);
        bool KeyAvailable { get; }
        bool CursorVisible { get; set; }
        int CursorLeft { get; }
        int CursorTop { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        bool IsRunningTerminal { get; }
    }
}
