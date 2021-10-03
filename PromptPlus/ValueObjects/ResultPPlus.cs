// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlus.ValueObjects
{
    public class ResultPPlus<T>
    {

        public ResultPPlus(T value, bool aborted)
        {
            Value = value;
            IsAborted = aborted;
        }

        internal bool IsAllAborted { get; private set; }

        internal static ResultPPlus<T> AbortAll()
        {
            return new ResultPPlus<T>(default, true) { IsAllAborted = true };
        }

        public T Value { get; }
        public bool IsAborted { get; internal set; }
    }
}
