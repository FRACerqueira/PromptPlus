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
    /// <summary>
    /// Represents the interface with all Methods of the AddtoList control
    /// </summary>
    public interface IControlList : IPromptControls<IEnumerable<string>>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T">Type external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList Interaction<T>(IEnumerable<T> values, Action<IControlList, T> action);

        /// <summary>
        /// Default initial value when when stated.
        /// </summary>
        /// <param name="value">initial value</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList Default(string value);

        /// <summary>
        /// Transform char input using <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">Transform option</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList InputToCase(CaseOptions value);

        /// <summary>
        /// Execute a function to accept input.
        /// <br>If result true accept input; otherwise, ignore input.</br>
        /// </summary>
        /// <param name="value">function to accept input</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList AcceptInput(Func<char, bool> value);

        /// <summary>
        /// MaxLength of input text.The value must be greater than or equal to 1
        /// <br>Default value is 0 (no limit)</br>
        /// </summary>
        /// <param name="value">Length</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList MaxLength(ushort value);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embedding</br>
        /// </summary>
        /// <param name="validators">the function validator. <see cref="ValidationResult"/></param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList AddValidators(params Func<object, ValidationResult>[] validators);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add Suggestion Handler feature
        /// </summary>
        /// <param name="value">function to apply suggestions. <see cref="SuggestionInput"/> and <seealso cref="SuggestionOutput"/></param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value);

        /// <summary>
        /// Add item to list
        /// </summary>
        /// <param name="value">Text item to add</param>
        /// <param name="immutable"> true the item cannot be removed; otherwise yes.</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList AddItem(string value, bool immutable = false);

        /// <summary>
        /// Add items colletion to list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="immutable"> true the item cannot be removed; otherwise yes.</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList AddItems(IEnumerable<string> values, bool immutable = false);

        /// <summary>
        /// Set max.item view per page.
        /// <br>Default value : 10.The value must be greater than or equal to 1</br>
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList PageSize(int value);

        /// <summary>
        /// Allow duplicate items.Default value for this control is false.
        /// </summary>
        /// <param name="value">Allow duplicate items</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList AllowDuplicate(bool value = true);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Overwrite a HotKey to edit item. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to edit item</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList HotKeyEditItem(HotKey value);

        /// <summary>
        /// Overwrite a HotKey to remove item. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to remove item</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList HotKeyRemoveItem(HotKey value);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlList"/></returns>
        IControlList ChangeDescription(Func<string, string> value);

    }
}
