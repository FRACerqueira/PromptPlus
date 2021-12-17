using PPlus.Controls;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditGeneric : MaskedOptions
    {
        public OptionsForMaskeditGeneric(string mask, string defaultvalue): base()
        {
            Type = MaskedType.Generic;
            MaskValue = mask;
            DefaultValueWitdMask = defaultvalue;
        }
    }
}
