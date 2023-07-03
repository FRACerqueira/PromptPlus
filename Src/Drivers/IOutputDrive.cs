// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.IO;
using System.Text;

namespace PPlus
{
    /// <summary>
    /// Represents the interface for output console.
    /// </summary>
    public interface IOutputDrive: IBackendTextWrite
    {
        /// <summary>
        /// Get output CodePage.
        /// </summary>
        int CodePage { get; }

        /// <summary>
        ///  Gets a value that indicates whether output has been redirected from the standard output stream.
        /// </summary>     
        bool IsOutputRedirected { get; }

        /// <summary>
        ///  Gets a value that indicates whether error has been redirected from the standard error stream.
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// Get/set an encoding for standard output stream.
        /// </summary>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// Get standard output stream.
        /// </summary>
        TextWriter Out { get; }

        /// <summary>
        /// Get standard error stream.
        /// </summary>
        TextWriter Error { get; }

        /// <summary>
        /// set standard output stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard output.</param>
        void SetOut(TextWriter value);

        /// <summary>
        /// set standard error stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard error.</param>
        void SetError(TextWriter value);

        /// <summary>
        /// <para>Clears the console buffer and corresponding console window of display information.</para>
        /// <br>Move cursor fom top console.</br>
        /// </summary>
        void Clear();

        /// <summary>
        /// Plays the sound of a beep through the console speaker.
        /// </summary>
        void Beep();
    }
}
