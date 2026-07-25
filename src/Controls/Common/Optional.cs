// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Scenario usage:
// https://stackoverflow.com/questions/65787544/nullable-enum-type-cannot-be-assigned-to-null-when-used-as-a-generic
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls.Common
{
    /// <summary>
    /// Represents an optional value.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    internal readonly struct Optional<T> : IEquatable<Optional<T>>
    {
        private Optional(T value, bool hasValue = true)
        {
            HasValue = hasValue;
            Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether this instance contains a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the contained value. Only meaningful when <see cref="HasValue"/> is <c>true</c>.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Returns an <see cref="Optional{T}"/> wrapping the given value.
        /// </summary>
        public static Optional<T> Set(T value) => new(value, true);

        /// <summary>
        /// Returns an empty <see cref="Optional{T}"/> with no value.
        /// </summary>
        public static Optional<T> Empty() => new(default!, false);

        /// <summary>
        /// Implicit cast to <see cref="Optional{T}"></see>
        /// </summary>
        /// <param name="optional">The value to cast</param>

        public static implicit operator T(Optional<T> optional) => optional.Value;

        /// <summary>
        /// Compares a raw value with an <see cref="Optional{T}"/>.
        /// </summary>
        public static bool operator ==(T left, Optional<T> right)
            => right.HasValue && EqualityComparer<T>.Default.Equals(left, right.Value);

        /// <summary>
        /// Compares a raw value with an <see cref="Optional{T}"/> for inequality.
        /// </summary>
        public static bool operator !=(T left, Optional<T> right)
            => !(left == right);

        /// <summary>
        /// Compares two <see cref="Optional{T}"/> instances for equality.
        /// </summary>
        public static bool operator ==(Optional<T> left, Optional<T> right)
            => left.Equals(right);

        /// <summary>
        /// Compares two <see cref="Optional{T}"/> instances for inequality.
        /// </summary>
        public static bool operator !=(Optional<T> left, Optional<T> right)
            => !left.Equals(right);

        /// <summary>
        /// Determines whether this instance equals another <see cref="Optional{T}"/>.
        /// </summary>
        public bool Equals(Optional<T> other)
        {
            if (!HasValue && !other.HasValue) return true;
            if (HasValue != other.HasValue) return false;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether this instance equals an object.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Optional<T> optional) return Equals(optional);
            if (obj is T raw) return HasValue && EqualityComparer<T>.Default.Equals(Value, raw);
            return false;
        }

        /// <summary>
        /// Get the HashCode
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Value, HasValue);
        }
    }
}
