using System.Globalization;

using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditDateOnly : MaskedOptions
    {
        public OptionsForMaskeditDateOnly(CultureInfo culture, string defaultvalue, bool fillzeros = false) : base()
        {
            Type = MaskedType.DateOnly;
            DefaultValueWitdMask = defaultvalue;
            ShowDayWeek = FormatWeek.Short;
            FillNumber = fillzeros ? MaskedBuffer.Defaultfill : null;
            CurrentCulture = culture;
        }
    }
}
