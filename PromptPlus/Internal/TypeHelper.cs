// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.ComponentModel;

namespace PromptPlus.Internal
{
    internal static class TypeHelper<T>
    {
        private static readonly Type s_targetType = typeof(T);
        private static readonly Type s_underlyingType = Nullable.GetUnderlyingType(typeof(T));

        public static bool IsValueType => s_targetType.IsValueType && s_underlyingType is null;

        public static T ConvertTo(string value) => (T)TypeDescriptor.GetConverter(s_underlyingType ?? s_targetType).ConvertFromString(value);
    }
}
