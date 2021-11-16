// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusInternal
{
    internal readonly struct Optional<T>
    {
        private Optional(T value)
        {
            HasValue = true;
            Value = value;
        }

        public bool HasValue { get; }

        public T Value { get; }

        public static implicit operator T(Optional<T> optional) => optional.Value;

        internal static readonly Optional<T> s_empty = new();

        public static Optional<T> Create(T value)
        {
            return value == null ? s_empty : new Optional<T>(value);
        }

        public static Optional<T> Create(object value)
        {
            return value == null ? s_empty : new Optional<T>((T)value);
        }
    }
}
