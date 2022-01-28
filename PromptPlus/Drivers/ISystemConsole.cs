// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Text;

namespace PPlus.Drivers
{
    public interface ISystemConsole
    {
        bool IsInputRedirected { get; }
        bool IsOutputRedirected { get; }
        bool IsErrorRedirected { get; }

        Encoding OutputEncoding { get; set; }
        Encoding InputEncoding { get; set; }
        TextWriter Out { get; }
        TextReader In { get; }
        TextWriter Error { get; }
        void SetIn(TextReader value);
        void SetOut(TextWriter value);
        void SetError(TextWriter value);
        void Clear();
        ConsoleKeyInfo ReadKey(bool intercept);
        void Beep();
        void Write(string value);
        void WriteLine(string value);
        bool KeyAvailable { get; }
        bool CursorVisible { get; set; }
        int CursorLeft { get; }
        int CursorTop { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        void ResetColor();
        void SetCursorPosition(int left, int top);
    }
}
