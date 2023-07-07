// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlMaskEditList : IPromptControls<IEnumerable<ResultMasked>>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T">Typeof item</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Interaction<T>(IEnumerable<T> values, Action<IControlMaskEditList, T> action);


        /// <summary>
        /// Default initial value when when stated.
        /// </summary>
        /// <param name="value">initial value</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Default(string value);

        /// <summary>
        /// Overwrite <see cref="Style"/> to region tip type input.
        /// <br>Default Foreground : 'ConsoleColor.Yellow'</br>
        /// <br>Default Background : same Console Background when setted</br>
        /// </summary>
        /// <param name="value">Style</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList TypeTipStyle(Style value);

        /// <summary>
        /// Overwrite <see cref="Style"/> to region neggative input.
        /// <br>Default Foreground : 'StyleControls.Answer'</br>
        /// <br>Default Background : same Console Background when setted</br>
        /// </summary>
        /// <param name="value">Style</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList NegativeStyle(Style value);

        /// <summary>
        /// Overwrite <see cref="Style"/> to region positive input.
        /// <br>Default Foreground : 'StyleControls.Answer'</br>
        /// <br>Default Background : Same Console Background when setted</br>
        /// </summary>
        /// <param name="value">Style</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList PositiveStyle(Style value);

        /// <summary>
        /// Defines mask input. Rules for Generic type:
        /// <br>9 - Only a numeric character</br> 
        /// <br>L - Only a letter</br> 
        /// <br>C - OnlyCustom character</br> 
        /// <br>A - Any character</br> 
        /// <br>N - OnlyCustom character +  Only a numeric character</br> 
        /// <br>X - OnlyCustom character +  Only a letter</br> 
        /// <br>\ - Escape character</br> 
        /// <br>{ - Initial delimiter for repetition of masks</br> 
        /// <br>} - Final delimiter for repetition of masks</br> 
        /// <br>[-Initial delimiter for list of Custom character</br> 
        /// <br>] - Final delimiter for list of Custom character</br> 
        /// </summary>
        /// <param name="value">text of masked when type is Generic. otherwise must be null.</param>
        /// <param name="promptmask">Prompt mask overwriter. Default value is '■'/'_'</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Mask(string value = null, char? promptmask = null);

        /// <summary>
        /// Defines type of mask control.
        /// </summary>
        /// <param name="maskedType">Type masked</param>
        /// <param name="promptmask">Prompt mask overwriter. Default value is '■'/'_'</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Mask(MaskedType maskedType, char? promptmask = null);


        /// <summary>
        /// <see cref="CultureInfo"/> to validate input when the type is not generic.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">CultureInfo to use on validate</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to validate input when the type is not generic.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use on validate</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Culture(string value);

        /// <summary>
        /// Fill zeros mask.
        /// <br>Not valid for type MaskedType.Generic (this set will be ignored).</br>
        /// <br>When used this feature the AcceptEmptyValue feature will be ignored.</br>
        /// <br>When MaskedType.Number or MaskedType.Currency this feature is always on.</br>
        /// </summary>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList FillZeros();

        /// <summary>
        /// Defines if year is long or short.
        /// <br>Valid only for type MaskedType.DateOnly or DateTime, otherwise this set will be ignored.</br>
        /// </summary>
        /// <param name="value"><see cref="Controls.FormatYear"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList FormatYear(FormatYear value);

        /// <summary>
        /// Defines time parts input.
        /// <br>Valid only for type MaskedType.TimeOnly or DateTime, otherwise this set will be ignored.</br>
        /// </summary>
        /// <param name="value"><see cref="Controls.FormatTime"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList FormatTime(FormatTime value);

        /// <summary>
        /// Defines integer lenght, decimal lenght and accept signl.
        /// <br>Valid only for type MaskedType.Number or Currency, otherwise this set will be ignored.</br>
        /// <br>This set is Requeried for these types.</br>
        /// </summary>
        /// <param name="intvalue">integer lenght</param>
        /// <param name="decimalvalue">decimal lenght</param>
        /// <param name="acceptSignal">True accept signal; otherwise, no.</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal);

        /// <summary>
        /// Append to desription the tip of type input. 
        /// </summary>
        /// <param name="week">show name of week for type date. <see cref="FormatWeek"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList DescriptionWithInputType(FormatWeek week = FormatWeek.None);

        /// <summary>
        /// Transform char input using <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">Transform option</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList InputToCase(CaseOptions value);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embeding</br>
        /// </summary>
        /// <param name="validators">the function validator. <see cref="ValidationResult"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AddValidators(params Func<object, ValidationResult>[] validators);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add Suggestion Handler feature
        /// </summary>
        /// <param name="value">function to apply suggestions. <see cref="SugestionInput"/> and <seealso cref="SugestionOutput"/></param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList SuggestionHandler(Func<SugestionInput, SugestionOutput> value);

        /// <summary>
        /// Accept empty value
        /// <br>Valid only for type not equal MaskedType.Generic, otherwise this set will be ignored.</br>
        /// </summary>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AcceptEmptyValue();

        /// <summary>
        /// Add item to initial list
        /// </summary>
        /// <param name="value">Text item to add</param>
        /// <param name="immutable"> true the item cannot be removed; otherwise yes.</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AddItem(string value, bool immutable = false);

        /// <summary>
        /// Add items colletion to initial list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="immutable"> true the item cannot be removed; otherwise yes.</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AddItems(IEnumerable<string> values, bool immutable = false);

        /// <summary>
        /// Set max.item view per page.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList PageSize(int value);

        /// <summary>
        /// Allow duplicate items.Default value for this control is false.
        /// </summary>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList AllowDuplicate();

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Overwrite a HotKey to edit item. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to edit item</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList HotKeyEditItem(HotKey value);

        /// <summary>
        /// Overwrite a HotKey to remove item. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to remove item</param>
        /// <returns><see cref="IControlMaskEditList"/></returns>
        IControlMaskEditList HotKeyRemoveItem(HotKey value);

    }
}
