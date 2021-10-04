// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlusControls.ValueObjects
{
    public class ResultPromptPlus<T>
    {

        public ResultPromptPlus(T value, bool aborted)
        {
            Value = value;
            IsAborted = aborted;
        }

        internal bool IsAllAborted { get; private set; }

        internal static ResultPromptPlus<T> AbortAll()
        {
            return new ResultPromptPlus<T>(default, true) { IsAllAborted = true };
        }

        public T Value { get; }
        public bool IsAborted { get; internal set; }
    }
}
