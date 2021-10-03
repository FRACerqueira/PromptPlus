// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlus.ValueObjects
{
    internal enum MaskedType
    {
        Generic,
        DateOnly,
        TimeOnly,
        DateTime,
        Number,
        Currency
    }

    public enum MaskedOptionDateType
    {
        DateOnly,
        TimeOnly,
        DateTime,
    }

    public enum MaskedOptionNumberType
    {
        Number,
        Currency
    }

    public enum MaskedOptionGenericType
    {
        Generic,
    }

    public class MaskedDateType
    {
        internal MaskedDateType()
        {
        }

        private MaskedOptionDateType _localoption = MaskedOptionDateType.DateOnly;

        internal MaskedType SeletedOption { get; private set; } = MaskedType.DateOnly;

        internal MaskedOptionDateType Option
        {
            get { return _localoption; }
            set
            {
                _localoption = value;
                switch (value)
                {
                    case MaskedOptionDateType.DateOnly:
                        SeletedOption = MaskedType.DateOnly;
                        break;
                    case MaskedOptionDateType.TimeOnly:
                        SeletedOption = MaskedType.TimeOnly;
                        break;
                    case MaskedOptionDateType.DateTime:
                        SeletedOption = MaskedType.DateTime;
                        break;
                }
            }
        }
    }

    public class MaskedNumberType
    {
        internal MaskedNumberType()
        {
        }

        private MaskedOptionNumberType _localoption = MaskedOptionNumberType.Number;

        internal MaskedType SeletedOption { get; private set; } = MaskedType.DateOnly;

        internal MaskedOptionNumberType Option
        {
            get { return _localoption; }
            set
            {
                _localoption = value;
                switch (value)
                {
                    case MaskedOptionNumberType.Number:
                        SeletedOption = MaskedType.Number;
                        break;
                    case MaskedOptionNumberType.Currency:
                        SeletedOption = MaskedType.Currency;
                        break;
                }
            }
        }
    }

    public class MaskedGenericType
    {
        internal MaskedGenericType()
        {
        }

        internal MaskedType SeletedOption => MaskedType.Generic;

        internal MaskedOptionGenericType Option => MaskedOptionGenericType.Generic;
    }
}
