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
        IPromptConfig EnabledAbortKey(bool value = true);

        /// <summary>
        /// Overwrite default Show/Hide Tooltip of control
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig ShowTooltip(bool value = true);


        /// <summary>
        /// Overwrite default Show pagination only if exists
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig ShowOnlyExistingPagination(bool value = true);

        /// <summary>
        /// Overwrite default DisableToggleTooltip of control
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig DisableToggleTooltip(bool value = true);

        /// <summary>
        /// Overwrite default Clear render area of control after finished
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig HideAfterFinish(bool value = true);


        /// <summary>
        /// Overwrite default Clear render area of control after AbortKey press
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig HideOnAbort(bool value = true);

        /// <summary>
        /// Overwrite default Hide Answer
        /// <br>When true, the prompt and control description are not rendered, showing only the minimum necessary without using resources (except the default tooltips when used)</br>
        /// </summary>
        /// <param name="value">value</param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig MinimalRender(bool value = true);

        /// <summary>
        /// Overwrite PaginationTemplate
        /// </summary>
        /// <param name="value">
        /// The function
        /// <br>string to show = Func(Total items,Current Page,Total pages)</br>
        /// </param>
        /// <returns><see cref="IPromptConfig"/></returns>
        IPromptConfig PaginationTemplate(Func<int, int, int, string>? value);

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

    }
}
