// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Interface for config controls to overwrite default values
    /// </summary>
    public interface IPromptConfig
    {
        /// <summary>
        /// Overwrite default Enabled/Disabled AbortKey press of control
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig EnabledAbortKey(bool value);

        /// <summary>
        /// Overwrite default Show/Hide Tooltip of control
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig ShowTooltip(bool value);


        /// <summary>
        /// Overwrite default Show pagination only if exists
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig ShowOnlyExistingPagination(bool value);

        /// <summary>
        /// Overwrite default DisableToggleTooltip of control
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig DisableToggleTooltip(bool value);

        /// <summary>
        /// Overwrite default Clear render area of control after finished
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig HideAfterFinish(bool value);


        /// <summary>
        /// Overwrite default Clear render area of control after AbortKey press
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig HideOnAbort(bool value);

        /// <summary>
        /// Add generic action for the control when change <see cref="StageControl"/> of control
        /// </summary>
        /// <param name="stage">Stage control</param>
        /// <param name="useraction">Action to execute.
        /// First param is generic conext(<see cref="SetContext"/>).
        /// Second param is curent input value of control.
        /// </param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig AddExtraAction(StageControl stage, Action<object, object?> useraction);

        /// <summary>
        /// Set generic context for then control to pass in stage ExtraAction parameter
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig SetContext(object value);

        /// <summary>
        /// Overwrite default style for <see cref="StyleControls"/> of control
        /// </summary>
        /// <param name="styleControl">Style overwriter</param>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig ApplyStyle(StyleControls styleControl, Style value);

        /// <summary>
        /// Set description for the control
        /// </summary>
        /// <param name="value">Text description. Accept markup color</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Description(string value);

        /// <summary>
        /// Set description for the control
        /// </summary>
        /// <param name="value">Value description with style</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Description(StringStyle value);


        /// <summary>
        /// Set prompt for the control
        /// </summary>
        /// <param name="value">Text prompt. Accept markup color</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Prompt(string value);

        /// <summary>
        /// Set prompt for the control
        /// </summary>
        /// <param name="value">Value prompt with style</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Prompt(StringStyle value);

        /// <summary>
        /// Set prompt for the control
        /// </summary>
        /// <param name="value">
        /// Text Tooltips. Accept markup color.
        /// This text overwrite default tooltips control.
        /// </param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Tooltips(string value);

        /// <summary>
        /// Set Tooltips for the control. This value overwrite default tooltips control.
        /// </summary>
        /// <param name="value">Value tooltip with style</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Tooltips(StringStyle value);

        /// <summary>
        /// Overwrite default Symbols for <see cref="SymbolType"/> of control
        /// </summary>
        /// <param name="schema">Symbol overwriter</param>
        /// <param name="value">Text when **not** is-unicode supported</param>
        /// <param name="unicode">Text when has is-unicode supported</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig Symbols(SymbolType schema, string value, string? unicode = null);
    }
}
