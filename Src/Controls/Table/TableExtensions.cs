using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPlus.Controls;
using PPlus.Controls.Table;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Table Control to Write to console. 
        /// </summary>
        ///<param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlChartBar"/></returns>
        public static IControlTable<T> Table<T>(Action<IPromptConfig> config = null) where T : class 
        {
            var opt = new TableOptions<T>(_styleschema, _configcontrols, _consoledrive, false);
            config?.Invoke(opt);
            return new TableControl<T>(_consoledrive, opt);
        }
    }
}
