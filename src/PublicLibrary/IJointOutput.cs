// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines a fluent API for writing styled (and token‑colored) text to an exclusive console output buffer.
    /// </summary>
    /// <remarks>
    /// Implementations typically stage writes in an off‑screen buffer and flush them atomically via <see cref="Done"/>.
    /// Methods returning <see cref="IJointOutput"/> support chaining.
    /// </remarks>
    public interface IJointOutput
    {
        /// <summary>
        /// Clears the buffered content (and console surface) and moves the cursor to the top (0,0).
        /// </summary>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput Clear();

        /// <summary>
        /// Resets the current (active) foreground and background colors to their defaults.
        /// </summary>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput ResetColor();

        /// <summary>
        /// Sets the default foreground and background colors used for subsequent write operations when no explicit <see cref="Style"/> is supplied.
        /// </summary>
        /// <param name="foreground">The default foreground <see cref="Color"/>.</param>
        /// <param name="background">The default background <see cref="Color"/>.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput DefaultColors(Color foreground, Color background);

        /// <summary>
        /// Writes a character array using an optional override <see cref="Style"/>.
        /// </summary>
        /// <param name="buffer">The characters to write. Ignored if empty.</param>
        /// <param name="style">Optional style override. If <c>null</c>, the current output style/default colors are used.</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears from the end of the written content to the end of the physical line.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput Write(char[] buffer, Style? style = null, bool clearRestOfLine = false);

        /// <summary>
        /// Writes a string using an optional override <see cref="Style"/>.
        /// </summary>
        /// <param name="value">The text to write. If <c>null</c> or empty, nothing is written.</param>
        /// <param name="style">Optional style override. If <c>null</c>, the current output style/default colors are used.</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears from the end of the written content to the end of the physical line.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput Write(string value, Style? style = null, bool clearRestOfLine = false);

        /// <summary>
        /// Writes a string whose content contains color tokens, applying the specified overflow strategy.
        /// </summary>
        /// <param name="value">The tokenized text to write (implementation‑defined token syntax).</param>
        /// <param name="overflow">How to handle content wider than the console buffer (see <see cref="Overflow"/>).</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears from the end of the written content to the end of the physical line.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput WriteColor(string value, Overflow overflow = Overflow.None, bool clearRestOfLine = false);

        /// <summary>
        /// Writes a character array followed by a line terminator.
        /// </summary>
        /// <param name="buffer">The characters to write. Ignored if empty.</param>
        /// <param name="style">Optional style override. If <c>null</c>, the current output style/default colors are used.</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears the remainder of the current line before emitting the newline.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput WriteLine(char[] buffer, Style? style = null, bool clearRestOfLine = true);

        /// <summary>
        /// Writes a string followed by a line terminator.
        /// </summary>
        /// <param name="value">The text to write. If <c>null</c> or empty, only the line terminator is emitted.</param>
        /// <param name="style">Optional style override. If <c>null</c>, the current output style/default colors are used.</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears the remainder of the current line before emitting the newline.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput WriteLine(string value, Style? style = null, bool clearRestOfLine = true);

        /// <summary>
        /// Writes a tokenized string followed by a line terminator, applying the specified overflow strategy.
        /// </summary>
        /// <param name="value">The tokenized text to write.</param>
        /// <param name="overflow">How to handle content wider than the console buffer (see <see cref="Overflow"/>).</param>
        /// <param name="clearRestOfLine">If <c>true</c>, clears the remainder of the current line before emitting the newline.</param>
        /// <returns>The same <see cref="IJointOutput"/> instance for chaining.</returns>
        IJointOutput WriteLineColor(string value, Overflow overflow = Overflow.None, bool clearRestOfLine = true);

        /// <summary>
        /// Flushes the staged buffer to the console, releases exclusive access, and returns the final cursor position.
        /// </summary>
        /// <returns>
        /// A tuple with the final cursor coordinates:
        /// <c>Left</c> = column position (0‑based),
        /// <c>Top</c> = row position (0‑based).
        /// </returns>
        (int Left, int Top) Done();
    }
}
