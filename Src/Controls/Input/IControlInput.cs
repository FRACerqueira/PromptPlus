// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlInput : IPromptControls<string>
    {
        /// <summary>
        /// <para>Default value when finished value is empty.</para>
        /// </summary>
        /// <param name="value">Finished value default</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput DefaultIfEmpty(string value);


        /// <summary>
        /// <para>Default value when stated.</para>
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
        /// <para>Execute a function to accept char input.</para>
        /// <br>If result true accept char input; otherwise, ignore char input.</br>
        /// </summary>
        /// <param name="value">function to accept</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput AcceptInput(Func<char, bool> value);

        /// <summary>
        /// MaxLenght of input text.
        /// </summary>
        /// <param name="value">Lenght</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput MaxLenght(ushort value);

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
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embeding</br>
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
        /// <param name="value">function to apply suggestions. <see cref="SugestionInput"/> and <seealso cref="SugestionOutput"/></param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput SuggestionHandler(Func<SugestionInput, SugestionOutput> value);

        /// <summary>
        /// Minimum chars to enabled history feature. Default value is 0.
        /// <br>History items are filtered by the starts with entry.</br>
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
        /// <para>Filter strategy for filter items in History colletion</para>
        /// <br>Default value is FilterMode.StartsWith</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlInput"/></returns>
        IControlInput FilterType(FilterMode value);
    }
}
