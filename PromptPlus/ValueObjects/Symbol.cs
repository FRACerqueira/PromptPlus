// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
// Inspired by the work https://github.com/shibayan/Sharprompt
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace PromptPlusControls.ValueObjects
{
    public class Symbol
    {
        private string _value;
        private string _fallbackValue;

        public Symbol()
        {
            _value = " ";
            _fallbackValue = "  ";
        }

        public Symbol(string value, string fallbackValue)
        {
            _value = value ?? " ";
            _fallbackValue = fallbackValue ?? " ";
        }

        public string Value
        {
            get { return _value; }
            set { _value = value ?? " "; }
        }

        public string FallbackValue
        {
            get { return _fallbackValue; }
            set { _fallbackValue = value ?? " "; }
        }

        public override string ToString()
        {
            return IsUnicodeSupported ? Value : FallbackValue;
        }

        public static implicit operator string(Symbol symbol) => symbol.ToString();

        private static bool IsUnicodeSupported => !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Console.OutputEncoding.CodePage == 1200 || Console.OutputEncoding.CodePage == 65001;
    }
}
