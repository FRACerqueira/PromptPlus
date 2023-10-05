// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Input control
    /// </summary>
    public interface IControlInput : IPromptControls<string>
    {
        /// <summary>
        /// Default value when finished value is empty.
        /// </summary>
        /// <param name="value">Finished value default</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput DefaultIfEmpty(string value);


        /// <summary>
        /// Default value when stated.
        /// </summary>
        /// <param name="value">Value default</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput Default(string value);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Transform char input using <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">Transform option</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput InputToCase(CaseOptions value);

        /// <summary>
        /// Execute a function to accept char input.
        /// <br>If result true accept char input; otherwise, ignore char input.</br>
        /// </summary>
        /// <param name="value">function to accept</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput AcceptInput(Func<char, bool> value);

        /// <summary>
        /// MaxLength of input text.The value must be greater than or equal to 1
        /// <br>Default value is 0 (no limit)</br>
        /// </summary>
        /// <param name="value">Length</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput MaxLength(ushort value);

        /// <summary>
        /// The input is a secret. the input text is masked to '#' (default value)
        /// </summary>
        /// <param name="value">char secret</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput IsSecret(char? value = '#');

        /// <summary>
        /// Enable user to view the input without mask.
        /// </summary>
        /// <param name="hotkeypress">Overwrite a <see cref="HotKey"/> to toggle view. Default value is 'F2'</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput EnabledViewSecret(HotKey? hotkeypress = null);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embedding</br>
        /// </summary>
        /// <param name="validators">the function validator. <see cref="ValidationResult"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput AddValidators(params Func<object, ValidationResult>[] validators);

        /// <summary>
        /// Execute validators foreach input
        /// </summary>
        /// <param name="value">true execute validators foreach input; otherwise, only at finish.</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput ValidateOnDemand(bool value = true);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add Suggestion Handler feature
        /// </summary>
        /// <param name="value">function to apply suggestions. <see cref="SuggestionInput"/> and <seealso cref="SuggestionOutput"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value);

        /// <summary>
        /// Minimum chars to enabled history feature. Default value is 0.
        /// <br>History items are filtered by the starts with entry.</br>
        /// <br>When command FilterType set to <see cref="FilterMode"/> Disabled History items the value must be zero</br>
        /// </summary>
        /// <param name="value">Minimum chars number</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput HistoryMinimumPrefixLength(int value);

        /// <summary>
        /// Enabled saved history inputs.
        /// <br>The history file is saved in <see cref="Environment.SpecialFolder.UserProfile"/> in the 'PromptPlus.History' folder.</br> 
        /// </summary>
        /// <param name="value">name of file to saved history</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput HistoryEnabled(string value);

        /// <summary>
        /// Set timeout to valid items saved on history. Default value is 365 days.
        /// </summary>
        /// <param name="value">timeout value</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput HistoryTimeout(TimeSpan value);

        /// <summary>
        /// Set maximum items saved on history.After maximum the items are rotates.
        /// </summary>
        /// <param name="value"> maximum items saved</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput HistoryMaxItems(byte value);


        /// <summary>
        /// Set max.item view per page on history.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput HistoryPageSize(int value);

        /// <summary>
        /// Filter strategy for filter items in History colletion
        /// <br>Default value is FilterMode.StartsWith</br>
        /// <br>When <see cref="FilterMode"/> is set to Disabled, the HistoryMinimumPrefixLength value is automatically set to zero</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput FilterType(FilterMode value);
    }
}
