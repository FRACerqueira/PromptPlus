// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the buffer of the Joint to output console
    /// </summary>
    public interface IJointOutput
    {
        /// <summary>
        /// Clears the console buffer and corresponding console window of display information.
        /// Moves cursor to the top of the console.
        /// </summary>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput Clear();

        /// <summary>
        /// Resets colors to default values.
        /// </summary>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput ResetColor();

        /// <summary>
        /// Sets the default foreground and background console colors.
        /// </summary>
        /// <param name="foreground">The default console foreground color.</param>
        /// <param name="background">The default console background color.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput DefaultColors(Color foreground, Color background);

        /// <summary>
        /// Writes the text representation of a character array to the standard output stream.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> to overwrite the current output style.</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput Write(char[] buffer, Style? style = null, bool clearRestOfLine = false);

        /// <summary>
        /// Writes the text representation of a string value to the standard output stream.
        /// </summary>
        /// <param name="value">A string value to write.</param>
        /// <param name="style">The <see cref="Style"/> to overwrite the current output style.</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput Write(string value, Style? style = null, bool clearRestOfLine = false);

        /// <summary>
        /// Writes the text representation of a string value with token colors to the standard output stream.
        /// </summary>
        /// <param name="value">A string value with token colors to write.</param>
        /// <param name="overflow">The <see cref="Overflow"/> Strategy</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput WriteColor(string value, Overflow overflow = Overflow.None, bool clearRestOfLine = false);

        /// <summary>
        /// Writes the text representation of a character array to the standard output stream with a line terminator.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> to overwrite the current output style.</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput WriteLine(char[] buffer, Style? style = null, bool clearRestOfLine = true);

        /// <summary>
        /// Writes the text representation of a string value to the standard output stream with a line terminator.
        /// </summary>
        /// <param name="value">A string value to write.</param>
        /// <param name="style">The <see cref="Style"/> to overwrite the current output style.</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput WriteLine(string value, Style? style = null, bool clearRestOfLine = true);

        /// <summary>
        /// Writes the text representation of a string value with token colors to the standard output stream with a line terminator.
        /// </summary>
        /// <param name="value">A string value with token colors to write.</param>
        /// <param name="overflow">The <see cref="Overflow"/> Strategy</param>
        /// <param name="clearRestOfLine">Indicates whether to clear the rest of the line.</param>
        /// <returns>The current <see cref="IJointOutput"/> instance.</returns>
        IJointOutput WriteLineColor(string value, Overflow overflow = Overflow.None, bool clearRestOfLine = true);

        /// <summary>
        /// Releases the console exclusive buffer.
        /// </summary>
        /// <returns>The number of lines written on the console.</returns>
        (int Left, int Top) Done();
    }
}
