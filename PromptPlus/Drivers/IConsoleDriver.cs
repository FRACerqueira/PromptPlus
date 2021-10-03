// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;

namespace PromptPlus.Drivers
{
    public interface IConsoleDriver : IDisposable
    {
        ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken);
        void Beep();
        void Reset();
        void ClearLine(int top);
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
