// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Objects
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

    public enum PromptDateTimeKind
    {
        DateOnly,
        TimeOnly,
        DateTime
    }
}
