// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Drivers
{
    /// <summary>
    /// Represents the interface with all Methods of the Joint control
    /// </summary>
    public interface IJointConsole
    {

        /// <summary>
        /// Write result function to output console and next.
        /// </summary>
        /// <param name="func">The function</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole Write(Func<string> func);

        /// <summary>
        /// Write a Exception to output console and next
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole Write(Exception value, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Write a text to output console and next
        /// </summary>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole Write(string value, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Write result function with line terminator to output console next.
        /// </summary>
        /// <param name="func">The function</param>
        /// <returns>Number of lines write on console</returns>
        IJointConsole WriteLine(Func<string> func);

        /// <summary>
        /// Write a Exception with line terminator to output console and next
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole WriteLine(Exception value, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Write lines with line terminator and next
        /// </summary>
        /// <param name="steps">Numbers de lines.</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole WriteLines(int steps = 1);

        /// <summary>
        /// Write a text to output console with line terminator and next.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole WriteLine(string? value = null, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes text line representation whie colors and Write single dash after and next.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions"><see cref="DashOptions"/> character</param>
        /// <param name="extralines">Number lines to write after write value</param>
        /// <param name="style">The <see cref="Style"/> to write.</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null);


        /// <summary>
        /// Writes text line representation whie colors in a pair of lines of dashes and next.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions"><see cref="DashOptions"/> character</param>
        /// <param name="extralines">Number lines to write after write value</param>
        /// <param name="style">The <see cref="Style"/> to write.</param>
        /// <returns><see cref="IJointConsole"/></returns>
        IJointConsole DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null);

        /// <summary>
        /// Get number of lines write on Join.
        /// </summary>
        int CountLines();   
    }
}
