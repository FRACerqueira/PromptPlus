using System.Globalization;

using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditCurrency : MaskedOptions
    {
        public OptionsForMaskeditCurrency(CultureInfo culture, int ammountInteger, int ammountDecimal, MaskedSignal acceptSignal, string defaultvalue) : base()
        {
            Type = MaskedType.Currency;
            DefaultValueWitdMask = defaultvalue;
            CurrentCulture = culture;
            AmmountInteger = ammountInteger;
            AmmountDecimal = ammountDecimal;
            AcceptSignal = acceptSignal;
        }
    }
}
