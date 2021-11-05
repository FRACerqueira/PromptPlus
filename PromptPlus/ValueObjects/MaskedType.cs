// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusControls.ValueObjects
{
    public enum MaskedType
    {
        Generic,
        DateOnly,
        TimeOnly,
        DateTime,
        Number,
        Currency
    }

    public enum FormatYear
    {
        Y4,
        Y2,
    }

    public enum FormatWeek
    {
        None,
        Short,
        Long
    }

    public enum FormatTime
    {
        HMS,
        OnlyHM,
        OnlyH,
    }
}
