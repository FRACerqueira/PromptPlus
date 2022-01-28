using System.Globalization;

using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditDateTime : MaskedOptions
    {
        public OptionsForMaskeditDateTime(CultureInfo culture, string defaultvalue, bool fillzeros = false) : base()
        {
            Type = MaskedType.DateTime;
            DefaultValueWitdMask = defaultvalue;
            ShowDayWeek = FormatWeek.Short;
            FillNumber = fillzeros ? MaskedBuffer.Defaultfill : null;
            CurrentCulture = culture;
        }
    }
}
