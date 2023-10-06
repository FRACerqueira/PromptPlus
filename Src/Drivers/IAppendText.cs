// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Drivers
{
    /// <summary>
    /// Represents the interface with all Methods of the Conteole Append control
    /// </summary>
    public interface IAppendText
    {
        /// <summary>
        /// Add text to the recording buffer
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <returns><see cref="IAppendText"/></returns>
        IAppendText And(string text);

        /// <summary>
        /// Add text to the recording buffer  with <see cref="Style"/>
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="style">The <see cref="Style"/></param>
        /// <returns><see cref="IAppendText"/></returns>
        IAppendText And(string text, Style style);

        /// <summary>
        /// Add text to the recording buffer  with forecolor <see cref="Color"/>
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="color">The forecolor. <see cref="Color"/></param>
        /// <returns><see cref="IAppendText"/></returns>
        IAppendText And(string text, Color color);

        /// <summary>
        /// Write a text to output console.
        /// </summary>
        void Write();

        /// <summary>
        /// Write line terminator a text to output console with line terminator.
        /// </summary>
        void WriteLine();
    }
}
