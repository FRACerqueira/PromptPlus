// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines options for configuring control behavior.
    /// </summary>
    public interface IControlOptions
    {
        /// <summary>
        /// Enables or disables the abort key for the control.
        /// </summary>
        /// <param name="isEnabled">If <c>true</c>, the abort key is enabled; otherwise, it is disabled.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions EnabledAbortKey(bool isEnabled = true);

        /// <summary>
        /// Show/hide message of abort key.
        /// </summary>
        /// <param name="isshow">If <c>true</c>, show text message from resources; otherwise no.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions ShowMesssageAbortKey(bool isshow = true);

        /// <summary>
        /// Shows or hides the tooltip for the control.
        /// </summary>
        /// <param name="isVisible">If <c>true</c>, the tooltip is shown; otherwise, it is hidden.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions ShowTooltip(bool isVisible = true);

        /// <summary>
        /// Hides the control's render area after it finishes execution.
        /// </summary>
        /// <param name="shouldHide">If <c>true</c>, the render area is cleared; otherwise, it remains visible.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions HideAfterFinish(bool shouldHide = true);

        /// <summary>
        /// Hides the control's render area after an abort key press.
        /// </summary>
        /// <param name="shouldHide">If <c>true</c>, the render area is cleared after abort; otherwise, it remains visible.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions HideOnAbort(bool shouldHide = true);

        /// <summary>
        /// Sets a description for the control with an optional style.
        /// </summary>
        /// <param name="description">The text description.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions Description(string description);

        /// <summary>
        /// Sets a Prompt for the control with an optional style.
        /// </summary>
        /// <param name="prompt">The text of prompt.</param>
        /// <returns>The current <see cref="IControlOptions"/> instance for chaining.</returns>
        IControlOptions Prompt(string prompt);
    }
}
