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
        bool EnabledExclusiveContext { get; set; }

        bool AbortedByCtrlC { get; }

        bool IsExitDefaultCancel { get; }

        bool WriteToErroOutput { get; set; }

        SemaphoreSlim ExclusiveContext { get; }

        (int Left, int Top, int scrolled) PreviewCursorPosition(int left, int top);

        CancellationToken TokenCancelPress { get; }

        void SetUserPressKeyAborted();

        void ResetTokenCancelPress();

        Color ForegroundColor { get; set; }

        Color BackgroundColor { get; set; }

        bool CursorVisible { get; set; }

        (int Left, int Top) GetCursorPosition();

        void SetCursorPosition(int left, int top);

        bool KeyAvailable { get; }

        ConsoleKeyInfo ReadKey(bool intercept = false);

        byte PadLeft { get; }

        byte PadRight { get; }

        bool IsUnicodeSupported { get; }

        int BufferWidth { get; }

        bool UserPressKeyAborted { get; }

        AfterCancelKeyPress BehaviorAfterCancelKeyPress { get; }

        (int Left, int Top) RawWrite(string value, Style? style = null, bool clearrestofline = false);

        (int Left, int Top) RawWriteLine(string value, Style? style = null, bool clearrestofline = false);

    }
}
