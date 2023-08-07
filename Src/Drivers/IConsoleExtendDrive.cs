// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PPlus
{
    /// <summary>
    /// Represents the interface for extend console.
    /// </summary>
    public interface IConsoleExtendDrive
    {
        /// <summary>
        /// Get Current Screen Buffer
        /// </summary>
        TargetBuffer CurrentBuffer { get; }

        /// <summary>
        /// Swap Screen Buffer
        /// </summary>
        /// <param name="value">The target buffer</param>
        /// <returns>True when console has capacity to swap to target buffer, otherwhise false</returns>
        bool SwapBuffer(TargetBuffer value);

        /// <summary>
        /// Run a action on target screen buffer and return to original screen buffer
        /// </summary>
        /// <param name="target">The target buffer</param>
        /// <param name="value">The action</param>        
        /// <param name="defaultforecolor">The default fore color</param>        
        /// <param name="defaultbackcolor">The default back color</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param> 
        /// <returns>True when console has capacity to run on target buffer, otherwhise false</returns>
        bool OnBuffer(TargetBuffer target, Action<CancellationToken> value, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// The extend capacity is enabled
        /// </summary>
        bool EnabledExtend { get; }

    }
}
