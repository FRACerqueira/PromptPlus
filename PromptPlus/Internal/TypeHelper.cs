// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.ComponentModel;

namespace PromptPlusControls.Internal
{
    internal static class TypeHelper<T>
    {
        private static readonly Type s_targetType = typeof(T);
        private static readonly Type s_underlyingType = Nullable.GetUnderlyingType(typeof(T));
        public static T ConvertTo(string value) => (T)TypeDescriptor.GetConverter(s_underlyingType ?? s_targetType).ConvertFromString(value);
    }
}
