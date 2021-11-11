// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace PromptPlusControls.ValueObjects
{
    public struct Symbol
    {

        public Symbol(string value = null, string fallbackValue = null)
        {
            Value = value ?? " ";
            FallbackValue = fallbackValue ?? " ";
        }

        public string Value { get; set; }

        public string FallbackValue { get; set; }

        public override string ToString()
        {
            return IsUnicodeSupported ? Value : FallbackValue;
        }

        public static implicit operator string(Symbol symbol) => symbol.ToString();

        private static bool IsUnicodeSupported => !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Console.OutputEncoding.CodePage == 1200 || Console.OutputEncoding.CodePage == 65001;
    }
}
