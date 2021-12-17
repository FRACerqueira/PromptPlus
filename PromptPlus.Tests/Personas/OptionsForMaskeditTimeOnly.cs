using System.Globalization;

using PPlus.Controls;
using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Tests.Personas
{
    internal class OptionsForMaskeditTimeOnly : MaskedOptions
    {
        public OptionsForMaskeditTimeOnly(CultureInfo culture, string defaultvalue, bool fillzeros = false) : base()
        {
            Type = MaskedType.TimeOnly;
            DefaultValueWitdMask = defaultvalue;
            ShowDayWeek = FormatWeek.Short;
            FillNumber = fillzeros ? MaskedBuffer.Defaultfill : null;
            CurrentCulture = culture;
        }
    }
}
