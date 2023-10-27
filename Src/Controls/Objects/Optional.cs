// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Scenario usage:
// https://stackoverflow.com/questions/65787544/nullable-enum-type-cannot-be-assigned-to-null-when-used-as-a-generic
// ***************************************************************************************

namespace PPlus.Controls.Objects
{
    internal readonly struct Optional<T>
    {
        private Optional(T value, bool hasValue = true)
        {
            HasValue = hasValue;
            Value = value;
        }

        public bool HasValue { get; }

        public T Value { get; }

        public static Optional<T> Set(T value)
        {
            return new Optional<T>(value, true);
        }

        public static Optional<T> Empty()
        {
            return new(default, false);
        }

        public static implicit operator T(Optional<T> optional) => optional.Value;

    }
}
