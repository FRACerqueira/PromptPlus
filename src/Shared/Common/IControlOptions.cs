// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring control behavior and presentation.
    /// </summary>
    public interface IControlOptions
    {

        /// <summary>
        /// Sets a suffix string to be displayed after the prompt text.
        /// </summary>
        /// <param name="sufix">The suffix string to display after the prompt text.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions SufixAfterPrompt(string sufix = ": ");

        /// <summary>
        /// Sets the prefix string displayed before extra info text.
        /// </summary>
        /// <param name="prefix">The prefix string. Defaults to <c>"("</c>.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions PrefixExtraInfo(string prefix = "(");

        /// <summary>
        /// Sets the suffix string displayed after extra info text.
        /// </summary>
        /// <param name="suffix">The suffix string. Defaults to <c>")"</c>.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions SuffixExtraInfo(string suffix = ")");

        /// <summary>
        /// Enables or disables the abort (Esc) hotkey for the control.
        /// </summary>
        /// <param name="isEnabled">
        /// If <c>true</c>, the abort key is enabled; otherwise, it is disabled. Default is <c>true</c>.
        /// </param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions EnabledAbortKey(bool isEnabled = true);

        /// <summary>
        /// Shows or hides the abort key help message (localized when available).
        /// </summary>
        /// <param name="isshow">
        /// If <c>true</c>, the abort key message is displayed; otherwise, it is hidden. Default is <c>true</c>.
        /// </param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions ShowMessageAbortKey(bool isshow = true);

        /// <summary>
        /// Shows or hides the tooltip associated with the control.
        /// </summary>
        /// <param name="isVisible">
        /// If <c>true</c>, the tooltip is shown; otherwise, it is hidden. Default is <c>true</c>.
        /// </param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions ShowTooltip(bool isVisible = true);

        /// <summary>
        /// Clears the control's render area after successful completion.
        /// </summary>
        /// <param name="shouldHide">
        /// If <c>true</c>, the render area is cleared; otherwise, it remains. Default is <c>true</c>.
        /// </param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions HideAfterFinish(bool shouldHide = true);

        /// <summary>
        /// Clears the control's render area after an abort (escape) action.
        /// </summary>
        /// <param name="shouldHide">
        /// If <c>true</c>, the render area is cleared on abort; otherwise, it remains. Default is <c>true</c>.
        /// </param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions HideOnAbort(bool shouldHide = true);

        /// <summary>
        /// Sets a descriptive text displayed with the control.
        /// </summary>
        /// <param name="description">The description text. If <c>null</c> or empty, any existing description may be cleared.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions Description(string description);

        /// <summary>
        /// Sets the prompt text displayed to the user.
        /// </summary>
        /// <param name="prompt">The prompt text. Should be concise and user‑facing.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions Prompt(string prompt);
    }
}
