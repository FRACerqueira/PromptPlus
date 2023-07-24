// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Controls.Pipeline;
using System;

namespace PPlus
{
    public static partial class PromptPlus
    {
        /// <summary>
        /// Create Pipeline Control
        /// </summary>
        /// <typeparam name="T">type of return</typeparam>
        /// <param name="startvalue">Initial value</param>
        /// <returns><see cref="PipelineControl{T}"/></returns>
        public static IControlPipeline<T> Pipeline<T>(T startvalue)
        {
            return Pipeline<T>(startvalue, null);
        }

        /// <summary>
        /// Create Pipeline Control
        /// </summary>
        /// <typeparam name="T">type of return</typeparam>
        /// <param name="startvalue">Initial value</param>
        /// <param name="config">The config action <see cref="IPromptConfig"/></param>
        /// <returns><see cref="PipelineControl{T}"/></returns>
        public static IControlPipeline<T> Pipeline<T>(T startvalue, Action<IPromptConfig> config)
        {
            var opt = new PipelineOptions<T>(false)
            {
                CurrentValue = startvalue
            };
            config?.Invoke(opt);
            return new PipelineControl<T>(_consoledrive, opt);
        }
    }
}