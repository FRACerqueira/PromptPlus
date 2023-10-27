// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Scenario usage:
// https://stackoverflow.com/questions/65787544/nullable-enum-type-cannot-be-assigned-to-null-when-used-as-a-generic
// ***************************************************************************************

using System;

namespace PPlus.Controls.Objects
{
    /// <summary>
    /// Represents a optional value
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    internal readonly struct Optional<T>
    {
        private Optional(T value, bool hasValue = true)
        {
            HasValue = hasValue;
            Value = value;
        }

        /// <summary>
        /// Get if the existing value
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Get Value
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Sets the value to non-optional
        /// </summary>
        /// <param name="value"></param>
        /// <returns><see cref="Optional{T}"/></returns>
        public static Optional<T> Set(T value)
        {
            return new Optional<T>(value, true);
        }

        /// <summary>
        /// Sets the empty value to optional
        /// </summary>
        /// <returns><see cref="Optional{T}"/></returns>
        public static Optional<T> Empty()
        {
            return new(default, false);
        }

        /// <summary>
        /// Implicit cast to <see cref="Optional{T}"></see>
        /// </summary>
        /// <param name="optional">The value to cast</param>

        public static implicit operator T(Optional<T> optional) => optional.Value;

        /// <summary>
        /// Compare if it is equal to <see cref="Optional{T}"/> 
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Rith operand</param>
        /// <returns><see cref="bool"/></returns>
        public static bool operator ==(T left, Optional<T> right)
        {
            return left!.Equals(right.Value);
        }

        /// <summary>
        /// Compare if it is not equal to <see cref="Optional{T}"/> 
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Rith operand</param>
        /// <returns><see cref="bool"/></returns>
        public static bool operator !=(T left, Optional<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compare with a object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns><see cref="bool"/></returns>
        public override bool Equals(object? obj)
        {
            if (obj is Optional<T> item)
            {
                if (Value == null || !item.HasValue)
                {
                    return obj == null;
                }
                return Value.Equals(item.Value);
            }
            else if (obj is T itemT)
            {
                if (Value == null)
                {
                    return obj == null;
                }
                return Value.Equals(itemT);
            }
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
