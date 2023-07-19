// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the ChartBar control
    /// </summary>
    public interface IControlChartBar: IPromptControls<bool>
    {

        /// <summary>
        /// Define type to ChartBar.
        /// </summary>
        /// <param name="value">The <see cref="ChartType"/>. Default value 'ChartType.StandBar'</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Type(ChartType value);

        /// <summary>
        /// Define type Bar to ChartBar.
        /// </summary>
        /// <param name="value">The <see cref="ChartBarType"/>. Default value 'ChartType.Fill'</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar BarType(ChartBarType value);



        /// <summary>
        /// <see cref="CultureInfo"/> to on show value format.
        /// </summary>
        /// <param name="value">CultureInfo to use</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to show value format.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Culture(string value);

        /// <summary>
        /// Define Width to ChartBar. Default value is 80.
        /// </summary>
        /// <param name="value">Width</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Width(int value);

        /// <summary>
        /// Define Tille Alignment. 
        /// </summary>
        /// <param name="value">The <see cref="Alignment"/> title</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar TitleAlignment(Alignment value = Alignment.Left);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Type external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Interaction<T1>(IEnumerable<T1> values, Action<IControlChartBar, T1> action);

        /// <summary>
        /// Styles for ChartBar content
        /// </summary>
        /// <param name="styletype"><see cref="StyleChart"/> of content</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Styles(StyleChart styletype, Style value);

        /// <summary>
        /// Add item to ChartBar
        /// </summary>
        /// <param name="label">Label Item to add</param>
        /// <param name="value">Value to Item</param>
        /// <param name="colorbar">
        /// The <see cref="Color"/> bar. 
        /// <br>If not informed, the color bar will be chosen in descending sequence from 15 to 0 and then back to 15.</br>
        /// </param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar AddItem(string label, double value, Color? colorbar = null);

        /// <summary>
        /// Define the Fracional Digits of value to show. Default is 0.
        /// </summary>
        /// <param name="value">Fracional Digits</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar FracionalDig(int value);

        /// <summary>
        /// Sort bars and labels
        /// </summary>
        /// <param name="chartOrder">The sort type</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar OrderBy(ChartOrder chartOrder);

        /// <summary>
        /// Hide Percent in bar
        /// </summary>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HidePercent();

        /// <summary>
        /// Hide value in bar
        /// </summary>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HideValue();

        /// <summary>
        /// Pad-Left to write ChartBar
        /// </summary>
        /// <param name="value">Number of spaces. Default value is 0.</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar PadLeft(byte value);

        /// <summary>
        /// Show Legends after ChartBar
        /// </summary>
        /// <param name="withvalue">Show value in legend</param>
        /// <param name="withPercent">Show Percent in legend</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar ShowLegends(bool withvalue = true, bool withPercent = true);

        /// <summary>
        /// Hide info of ordination labels
        /// </summary>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HideOrdination();

        /// <summary>
        /// Enabled Interaction  to switch Type , Legend and order when browse the charts / Legends.
        /// </summary>
        /// <param name="switchType">Enabled switch Type </param>
        /// <param name="switchLegend">Enabled switch legend</param>
        /// <param name="switchorder">Enabled switch Ordination</param>
        /// <param name="pagesize">Set max.item view per page.Default value for this control is 10.</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar EnabledInteractionUser(bool switchType = true, bool switchLegend = true, bool switchorder = true, int? pagesize = null);

        /// <summary>
        /// Overwrite a HotKey to Switch Type Chart. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to Switch Type Chart</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HotKeySwitchType(HotKey value);

        /// <summary>
        /// Overwrite a HotKey to Switch Legend Chart. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to Switch Legend Chart</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HotKeySwitchLegend(HotKey value);

        /// <summary>
        /// Overwrite a HotKey to Switch ordination bar and label. Default value is 'F4' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to Switch ordination bar and label</param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar HotKeySwitchOrder(HotKey value);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">Action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlChartBar"/></returns>
        IControlChartBar Config(Action<IPromptConfig> context);
    }
}
