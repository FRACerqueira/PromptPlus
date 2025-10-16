// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Interface for Emacs ReadLine functionality.
    /// </summary>
    public interface IEmacs
    {
        /// <summary>
        /// Sets a validation function for key input.
        /// </summary>
        /// <param name="validateKeyFunc">A function to validate the input character.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>
        IEmacs ValidateKey(Func<char, bool> validateKeyFunc);

        /// <summary>
        /// Sets the maximum length for the input.
        /// </summary>
        /// <param name="maxLength">The maximum length of the input.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>
        IEmacs MaxLength(int maxLength);

        /// <summary>
        /// Sets the text to read only.
        /// </summary>
        /// <param name="value">If set to <c>true</c>, the read only initial text.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>

        IEmacs ReadOnly(bool value = true);

        /// <summary>
        /// Sets the maximum width for the input.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MaxWidth(int)"/> must be less than <see cref="MaxLength(int)"/> - 4, otherwise it will be ignored.
        /// </remarks>
        /// <param name="maxWidth">The maximum width of the input.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>
        IEmacs MaxWidth(int maxWidth);

        /// <summary>
        /// Sets the case transformation options for the input.
        /// </summary>
        /// <param name="caseOptions">The case transformation options.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>
        IEmacs CaseOptions(CaseOptions caseOptions);

        /// <summary>
        /// Sets whether the escape key should abort the input.
        /// </summary>
        /// <param name="escAbort">If set to <c>true</c>, the escape key will abort and clear the input.</param>
        /// <returns>The current <see cref="IEmacs"/> instance.</returns>
        IEmacs EscAbort(bool escAbort = true);

        /// <summary>
        /// Reads a line of input with the specified initial value.
        /// </summary>
        /// <returns>The input string, or <c>null</c> if the input was aborted.</returns>
        string? ReadLine();
    }
}
