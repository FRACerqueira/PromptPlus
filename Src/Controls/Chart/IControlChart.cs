// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the Breakdown ChartBar Control interface
    /// </summary>
    public interface IControlChart

    {
        /// <summary>
        /// <see cref="CultureInfo"/> to on show value format.
        /// </summary>
        /// <param name="value">CultureInfo to use</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to show value format.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Culture(string value);

        /// <summary>
        /// Define Width to Widgets. Default value is 80.
        /// </summary>
        /// <param name="value">Width</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Width(int value);

        /// <summary>
        /// Define Tille to Widgets. 
        /// </summary>
        /// <param name="value">Title</param>
        /// <param name="titlealigment">Title Aligment.If the title is greater than the size of the chart Align will be left.</param>
        /// <param name="style"><see cref="Style"/> of Title</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Title(string value, TitleAligment titlealigment = TitleAligment.Left, Style? style = null);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Type external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Interaction<T1>(IEnumerable<T1> values, Action<IControlChart, T1> action);

        /// <summary>
        /// Styles for content chart/>
        /// </summary>
        /// <param name="styletype"><see cref="StyleChart"/> of content chart</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart Styles(StyleChart styletype, Style value);

        /// <summary>
        /// Add item to ChartBar
        /// </summary>
        /// <param name="label">Label Item to add</param>
        /// <param name="value">Value to Item</param>
        /// <param name="colorbar">The <see cref="Color"/> bar. If not informed, the colorbar will be chosen in sequence starting at zero.</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlChart AddItem(string label, double value, Color? colorbar = null);

        /// <summary>
        /// Set <see cref="char"/> to show progress.Default value '#'
        /// <br>Valid on ProgressBarType.Char, otherwise is ignored </br>
        /// </summary>
        /// <param name="value">Char to show</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart CharBar(char value);

        /// <summary>
        /// Define the Fracional Digits of value to show. Default is 0.
        /// </summary>
        /// <param name="value">Fracional Digits</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart FracionalDig(int value);

        /// <summary>
        /// Sort Item by Highest Value
        /// </summary>
        /// <param name="chartOrder">The sort value chart items</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart OrderBy(ChartOrder chartOrder);

        /// <summary>
        /// Show Percent in ChartBar bar
        /// </summary>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart HidePercent();

        /// <summary>
        /// Hide value in ChartBar bar
        /// </summary>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart HideValue();

        /// <summary>
        /// PadLeft bar chart with spaces 
        /// </summary>
        /// <param name="value">Number of spaces</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart ChartPadLeft(int value = 1);

        /// <summary>
        /// Show Legends after ChartBar
        /// </summary>
        /// <param name="withvalue">Show value in legend</param>
        /// <param name="withPercent">Show Percent in legend</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart ShowLegends(bool withvalue = true, bool withPercent = true);


        /// <summary>
        /// Set max.item view per page.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlChart"/></returns>
        IControlChart PageSize(int value);

        /// <summary>
        /// Execute this control and show ChartBar.
        /// </summary>
        /// <param name="barType">The type chart Bar. <see cref="ChartBarType"/></param>
        /// <param name="dashOptions">The type of <see cref="BannerDashOptions"/></param>
        /// <param name="colorDash">The <see cref="Color"/> Dash</param>
        void Run(ChartBarType? barType = ChartBarType.Fill, BannerDashOptions dashOptions = BannerDashOptions.None , Color? colorDash = null);


    }
}
