// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PPlus
{
    /// <summary>
    /// Represents the interface for input console.
    /// </summary>
    public interface IInputDrive
    {
        /// <summary>
        ///  Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br>
        /// </returns>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Wait Keypress from standard input stream
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <param name="cancellationToken"> The token to monitor for cancellation requests.</param> 
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br> 
        ///</returns>
        ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken);


        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        bool KeyAvailable { get; }


        /// <summary>
        /// <br>Read the line from stream. A line is defined as a sequence of characters followed by</br>
        /// <br>a car return ('\r'), a line feed ('\n'), or a carriage return</br>
        /// <br>immedy followed by a line feed. The resulting string does not</br>
        /// <br>contain the terminating carriage return and/or line feed.</br>
        /// </summary>
        /// <returns>
        /// The returned value is null if the end of the input stream has been reached.
        /// </returns>
        string? ReadLine();

        /// <summary>
        ///  Gets a value that indicates whether input has been redirected from the standard input stream.
        /// </summary>
        bool IsInputRedirected { get; }

        /// <summary>
        /// Get/set an encoding for standard input stream.
        /// </summary>
        Encoding InputEncoding { get; set; }

        /// <summary>
        /// Get standard input stream.
        /// </summary>
        TextReader In { get; }


        /// <summary>
        /// set standard input stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard input.</param>
        void SetIn(TextReader value);

    }
}
