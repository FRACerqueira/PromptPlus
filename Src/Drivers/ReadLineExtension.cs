using System;
using PPlus.Controls;
using PPlus.Controls.Objects;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// <br>Read the line from stream using Emacs keyboard shortcuts. A line is defined as a sequence of characters followed by</br>
        /// <br>a car return ('\r'), a line feed ('\n'), or a carriage return</br>
        /// <br>immedy followed by a line feed. The resulting string does not</br>
        /// <br>contain the terminating carriage return and/or line feed.</br>
        /// </summary>
        /// <param name="maxlength">The input Max-length</param>
        /// <param name="caseOptions">The input <see cref="CaseOptions"/></param>
        /// <param name="afteraccept">The user action after each accepted keystroke. Firt param is input text, Second param is relative cursor position of text</param>
        /// <returns>
        /// The string input value.
        /// </returns>
        public static string ReadLineWithEmacs(uint? maxlength = uint.MaxValue, CaseOptions caseOptions = CaseOptions.Any, Action<string,int> afteraccept = null)
        {
            var _inputBuffer = new EmacsBuffer(caseOptions, null,maxlength.Value);
            var endinput = false;
            var (Left, Top) = GetCursorPosition();
            do
            {
                var keyInfo = ReadKey(true);
                if (!_inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo))
                {
                    if (keyInfo.IsPressEnterKey())
                    {
                        endinput = true;
                    }
                    continue;
                }
                CursorVisible = false;
                SetCursorPosition(Left, Top);
                Write(_inputBuffer.ToBackward(),clearrestofline:true);
                var pos = GetCursorPosition();
                Write(_inputBuffer.ToForward());
                SetCursorPosition(pos.Left, pos.Top);
                afteraccept?.Invoke(_inputBuffer.ToString(), pos.Left-Left);
                CursorVisible = true;
            } while (!endinput);
            SetCursorPosition(Left, Top);
            WriteLine(_inputBuffer.ToString());
            return _inputBuffer.ToString();
        }
    }
}
