// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using Microsoft.Extensions.Logging;

namespace PPlus.Objects
{
    public struct ResultPromptPlus<T>
    {
        public ResultPromptPlus()
        {
            Value = default;
            IsAborted = false;
            IsAllAborted = false;
            LogControl = null;
        }

        internal ResultPromptPlus(T value, bool aborted, bool? abortall = null, ControlLog? log = null)
        {
            Value = value;
            IsAborted = aborted;
            IsAllAborted = abortall ?? false;
            LogControl = log;
        }

        internal bool IsAllAborted { get; }

        public T Value { get; }

        public bool IsAborted { get; }

        public ControlLog? LogControl { get; set; }
    }
}
