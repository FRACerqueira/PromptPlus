// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Core
{
    internal sealed class JointOutput(IConsole console) : IJointOutput
    {
        private readonly IConsole _console = console;

        public IJointOutput Clear()
        {
            _console.Clear();
            return this;
        }

        public IJointOutput DefaultColors(Color foreground, Color background)
        {
            _console.DefaultColors(foreground, background);
            return this;
        }

        public (int Left, int Top) Done()
        {
            (int Left, int Top) result = _console.GetCursorPosition();
            return result;
        }

        public IJointOutput ResetColor()
        {
            _console.ResetColor();
            return this;
        }

        public IJointOutput Write(char[] buffer, Style? style = null, bool clearrestofline = false)
        {
            _console.Write(buffer, style, clearrestofline);
            return this;
        }

        public IJointOutput Write(string value, Style? style = null, bool clearrestofline = false)
        {
            _console.Write(value, style, clearrestofline);
            return this;
        }

        public IJointOutput WriteLine(char[] buffer, Style? style = null, bool clearrestofline = true)
        {
            _console.WriteLine(buffer, style, clearrestofline);
            return this;
        }

        public IJointOutput WriteLine(string value, Style? style = null, bool clearrestofline = true)
        {
            _console.WriteLine(value, style, clearrestofline);
            return this;
        }
    }
}
