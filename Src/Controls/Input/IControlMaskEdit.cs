﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the MaskEdit control
    /// </summary>
    public interface IControlMaskEdit : IPromptControls<ResultMasked>
    {
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
        /// <param name="value">text of masked.</param>
        /// <param name="promptmask">Prompt mask overwriter. Default value is '■'/'_'</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Mask(string value, char? promptmask = null);

        /// <summary>
        /// Defines type of mask control.
        /// </summary>
        /// <param name="maskedType"><see cref="MaskedType"/></param>
        /// <param name="promptmask">Prompt mask overwriter. Default value is '■'/'_'</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Mask(MaskedType maskedType,char? promptmask = null);

        /// <summary>
        /// Accept empty value
        /// <br>Valid only for type not equal MaskedType.Generic, otherwise this set will be ignored.</br>
        /// </summary>
        /// <param name="value">Accept empty value</param> 
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit AcceptEmptyValue(bool value = true);

        /// <summary>
        /// Default value (with mask!) when finished value is empty.
        /// </summary>
        /// <param name="value">Finished value default</param>
        /// <param name="zeroIsEmpty">Valid only for type MaskedType.Number or MaskedType.Currency, otherwise this set will be ignored.</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit DefaultIfEmpty(string value,bool zeroIsEmpty = true);

        /// <summary>
        /// Fill zeros mask.Default false.
        /// <br>Not valid for type MaskedType.Generic (this set will be ignored).</br>
        /// <br>When used this feature the AcceptEmptyValue feature will be ignored.</br>
        /// <br>When MaskedType.Number or MaskedType.Currency this feature is always on.</br>
        /// </summary>
        /// <param name="value">Fill zeros mask</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit FillZeros(bool value = true);

        /// <summary>
        /// <see cref="CultureInfo"/> to validate input when the type is not generic.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">CultureInfo to use on validate</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to validate input when the type is not generic.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use on validate</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Culture(string value);

        /// <summary>
        /// Defines if year is long or short.
        /// <br>Valid only for type MaskedType.DateOnly or DateTime, otherwise this set will be ignored.</br>
        /// </summary>
        /// <param name="value"><see cref="Controls.FormatYear"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit FormatYear(FormatYear value);

        /// <summary>
        /// Defines time parts input.
        /// <br>Valid only for type MaskedType.TimeOnly or DateTime, otherwise this set will be ignored.</br>
        /// </summary>
        /// <param name="value"><see cref="Controls.FormatTime"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit FormatTime(FormatTime value);

        /// <summary>
        /// Defines integer length, decimal length and accept signl.
        /// <br>Valid only for type MaskedType.Number or Currency, otherwise this set will be ignored.</br>
        /// <br>This set is Requeried for these types.</br>
        /// </summary>
        /// <param name="intvalue">integer length</param>
        /// <param name="decimalvalue">decimal length</param>
        /// <param name="acceptSignal">True accept signal; otherwise, no.</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal);

        /// <summary>
        /// Show the tip of type input. 
        /// </summary>
        /// <param name="week">show name of week for type date. <see cref="FormatWeek"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit ShowTipInputType(FormatWeek week = FormatWeek.None);

        /// <summary>
        /// Default value (with mask!) when stated.
        /// </summary>
        /// <param name="value">Value default</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Default(string value);

        /// <summary>
        /// Overwrite default start value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Transform char input using <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">Transform option</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit InputToCase(CaseOptions value);

        /// <summary>
        /// Add a validator to accept sucessfull finish of control.
        /// <br>Tip: see <see cref="PromptValidators"/> to validators embedding</br>
        /// </summary>
        /// <param name="validators">the function validator. <see cref="ValidationResult"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit AddValidators(params Func<object, ValidationResult>[] validators);

        /// <summary>
        /// Execute validators foreach input
        /// </summary>
        /// <param name="value">true execute validators foreach input; otherwise, only at finish.</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit ValidateOnDemand(bool value = true);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add Suggestion (with mask!) Handler feature
        /// </summary>
        /// <param name="value">function to apply suggestions. <see cref="SuggestionInput"/> and <seealso cref="SuggestionOutput"/></param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value);

        /// <summary>
        /// Minimum chars to enabled history feature. Default value is 0.
        /// <br>History items are filtered by the starts with entry.</br>
        /// <br>When command FilterType set to <see cref="FilterMode"/> Disabled History items the value must be zero</br>
        /// </summary>
        /// <param name="value">Minimum chars number</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit HistoryMinimumPrefixLength(int value);

        /// <summary>
        /// Enabled saved history inputs.
        /// <br>The history file is saved in <see cref="Environment.SpecialFolder.UserProfile"/> in the 'PromptPlus.History' folder.</br> 
        /// </summary>
        /// <param name="value">name of file to saved history</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit HistoryEnabled(string value);

        /// <summary>
        /// Set timeout to valid items saved on history. Default value is 365 days.
        /// </summary>
        /// <param name="value">timeout value</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit HistoryTimeout(TimeSpan value);

        /// <summary>
        /// Set maximum items saved on history.After maximum the items are rotates.
        /// </summary>
        /// <param name="value"> maximum items saved</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit HistoryMaxItems(byte value);


        /// <summary>
        /// Set max.item view per page on history.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit HistoryPageSize(int value);

        /// <summary>
        /// Overwrite Styles
        /// </summary>
        /// <param name="content">The <see cref="MaskEditStyles"/> content</param>
        /// <param name="value">The <see cref="Style"/> to apply</param>
        /// <returns><see cref="IControlMaskEdit"/></returns>
        IControlMaskEdit Styles(MaskEditStyles content, Style value);
    }
}
