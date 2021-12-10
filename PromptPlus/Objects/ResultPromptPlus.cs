// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using Microsoft.Extensions.Logging;

namespace PPlus.Objects
{
    public struct ResultPromptPlus<T>
    {

        internal ResultPromptPlus(T value, bool aborted, bool? abortall = null, ControlLog? log = null)
        {
            Value = value;
            IsAborted = aborted;
            IsAllAborted = abortall ?? false;
            LogControl = log;
        }

        internal bool IsAllAborted { get; }

        internal static ResultPromptPlus<T> AbortAll()
        {
            return new ResultPromptPlus<T>(default, true, true);
        }

        public T Value { get; }
        public bool IsAborted { get; }

        public ControlLog? LogControl { get; set; }
    }
}
