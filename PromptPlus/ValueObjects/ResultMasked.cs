// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlusControls.ValueObjects
{
    public struct ResultMasked
    {
        internal ResultMasked(string value, string valueMask)
        {
            Input = value;
            Masked = valueMask;
            ObjectValue = null;
        }
        public object ObjectValue { get; internal set; }
        public string Input { get; private set; }
        public string Masked { get; private set; }
    }
}
