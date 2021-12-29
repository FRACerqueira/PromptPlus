using System.Globalization;

using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditNumber : MaskedOptions
    {
        public OptionsForMaskeditNumber(CultureInfo culture, int ammountInteger, int ammountDecimal, MaskedSignal acceptSignal, string defaultvalue) : base()
        {
            Type = MaskedType.Number;
            DefaultValueWitdMask = defaultvalue;
            CurrentCulture = culture;
            AmmountInteger = ammountInteger;
            AmmountDecimal = ammountDecimal;
            AcceptSignal = acceptSignal;
        }
    }
}
