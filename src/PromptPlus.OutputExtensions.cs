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

        /// <summary>Replaces all line endings in the specified string with the standard environment new line sequence.</summary>
        /// <remarks>This method is useful for ensuring consistent line endings across different platforms or input sources.</remarks>
        /// <param name="input">The input string whose line endings will be normalized. Can be null.</param>
        /// <returns>A string with all line endings replaced by the environment's new line sequence, or empty string if the input is null.</returns>
        public static string NormalizeNewLine(this string input)
        {
            return UtilExtension.NormalizeNewLines(input);
        }

        /// <summary>
        /// Write lines with line terminator
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> drive</param>
        /// <param name="steps">Numbers de lines.</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        public static void WriteLines(this IConsole console, int steps = 1, bool clearrestofline = true)
        {
            for (int i = 0; i < steps; i++)
            {
                console.WriteLine("", null, clearrestofline);
            }
        }

        /// <summary>
        /// Clears the console buffer with <see cref="Color"/> and set BackgroundColor with <see cref="Color"/>
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> drive</param>
        /// <param name="backcolor">The <see cref="Color"/> Background</param>
        public static void Clear(this IConsole console, Color? backcolor = null)
        {
            if (backcolor.HasValue)
            {
                console.BackgroundColor = Color.ToConsoleColor(backcolor.Value);
            }
            console.Clear();
        }


        /// <summary>
        ///  Clear line
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> drive</param>
        /// <param name="row">The row to clear</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        public static void ClearLine(this IConsole console, int? row = null, Style? style = null)
        {
            row ??= _consoledrive.CursorTop;
            console.SetCursorPosition(0, row.Value);
            console.Write(' ', style, true);
            console.SetCursorPosition(0, row.Value);
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
