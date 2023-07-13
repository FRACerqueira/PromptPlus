// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create ChartBar Control to Write to console. 
        /// </summary>
        /// <returns><see cref="IControlChart"/></returns>
        public static IControlChart ChartBar()
        {
            return new ChartControl(_consoledrive, ChartType.StandBar);
        }

        /// <summary>
        /// Create Chart Statk Control to Write to console. 
        /// </summary>
        /// <returns><see cref="IControlChart"/></returns>
        public static IControlChart ChartStackBar()
        {
            return new ChartControl(_consoledrive, ChartType.StackBar);
        }

    }
}
