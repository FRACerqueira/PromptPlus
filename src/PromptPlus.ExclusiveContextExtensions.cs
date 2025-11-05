// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;

namespace PromptPlusLibrary
{
    public static partial class PromptPlus
    {

        /// <summary>
        /// Create Exclusive context to write on standard output stream for any output included until the 'dispose' is done.
        /// </summary>
        /// <param name="console">The drive console <see cref="IConsole"/> drive</param>
        /// <returns><see cref="IDisposable"/></returns>
        public static IDisposable ExclusiveContext(this IConsole console)
        {
            return new ExclusiveContextOutput((IConsoleExtend)console, false);
        }

        internal static IDisposable InternalExclusiveContext(this IConsole console)
        {
            return new ExclusiveContextOutput((IConsoleExtend)console, true);
        }

        internal static IDisposable InternalExclusiveContext(this IConsoleExtend console)
        {
            return new ExclusiveContextOutput(console, true);
        }
    }
}
