// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Threading;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the interface with all Methods/Properties of the control
    /// </summary>
    public interface IPromptControls<T>
    {
        /// <summary>
        /// Execute this control and return ResultPrompt with type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value"><see cref="CancellationToken"/> for control</param>
        /// <returns><see cref="ResultPrompt{T}"/></returns>
        ResultPrompt<T> Run(CancellationToken? value = null);
    }
}
