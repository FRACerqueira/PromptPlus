// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Represents the interface for write text console.
    /// </summary>
    public interface IBackendTextWrite
    {
        /// <summary>
        /// Write a text to output console.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        int Write(string value, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Write a text to output console with line terminator.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        int WriteLine(string? value = null, Style? style = null, bool clearrestofline = true);

    }
}
