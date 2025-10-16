// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;

namespace PromptPlusLibrary
{
    public static partial class PromptPlus
    {
        /// <summary>
        ///  Create context to write on standard error output stream for any output included until the 'dispose' is done.
        /// </summary>
        /// <returns><see cref="IDisposable"/></returns>
        public static IDisposable OutputError(this IConsole console)
        {
            return new RedirectToErrorOutput((IConsoleExtend)console);
        }

        /// <summary>
        /// Write lines with line terminator
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> drive</param>
        /// <param name="steps">Numbers de lines.</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        public static (int Left, int Top) WriteLines(this IConsole console, int steps = 1, bool clearrestofline = true)
        {
            using (InternalExclusiveContext(console))
            {
                for (int i = 0; i < steps; i++)
                {
                    console.WriteLine("", null, clearrestofline);
                }
                return console.GetCursorPosition();
            }
        }

        /// <summary>
        /// Wait all output using exclusive buffer to console
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> drive</param>
        /// <returns>Buffer <see cref="IJointOutput"/></returns>
        public static IJointOutput Join(this IConsole console)
        {
            return new JointOutput(console);
        }
    }
}
