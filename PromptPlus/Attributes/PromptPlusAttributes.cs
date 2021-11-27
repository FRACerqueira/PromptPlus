using System;
using System.Globalization;
using System.Threading;

using PPlus.Objects;

namespace PPlus.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptInitialValueAttribute : Attribute
    {
        public PromptInitialValueAttribute(string initialValue = null, bool ever = false)
        {
            InitialValue = initialValue;
            EverInitialValue = ever;
        }

        public virtual string InitialValue { get; private set; }
        public virtual bool EverInitialValue { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptValidatorUriAttribute : Attribute
    {
        public PromptValidatorUriAttribute(UriKind uriKind = UriKind.Absolute, string allowedUriScheme = null)
        {
            Kind = uriKind;
            AllowedUriScheme = allowedUriScheme;
        }

        public virtual UriKind Kind { get; private set; }
        public virtual string AllowedUriScheme { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeNumberAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeNumberAttribute(int integerpart, int decimalpart)
        {
            if (integerpart + decimalpart == 0)
            {
                throw new ArgumentException(
                 $"{nameof(integerpart)} or {nameof(decimalpart)} must be greater than zero.");
            }
            IntegerPart = integerpart;
            DecimalPart = decimalpart;
            AccepSignal = false;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeNumberAttribute(int integerpart, int decimalpart, bool accepSignal = true, CultureInfo? cultureInfo = null)
        {
            if (integerpart + decimalpart == 0)
            {
                throw new ArgumentException(
                 $"{nameof(integerpart)} or {nameof(decimalpart)} must be greater than zero.");
            }
            IntegerPart = integerpart;
            DecimalPart = decimalpart;
            AccepSignal = accepSignal;
            Culture = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.Number;
        public virtual int IntegerPart { get; private set; }
        public virtual int DecimalPart { get; private set; }
        public virtual bool AccepSignal { get; private set; }
        public virtual CultureInfo Culture { get; private set; }

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeCurrencyAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeCurrencyAttribute(int integerpart, int decimalpart, bool accepSignal = true, CultureInfo? cultureInfo = null)
        {
            if (integerpart + decimalpart == 0)
            {
                throw new ArgumentException(
                 $"{nameof(integerpart)} or {nameof(decimalpart)} must be greater than zero.");
            }
            IntegerPart = integerpart;
            DecimalPart = decimalpart;
            AccepSignal = accepSignal;
            Culture = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.Currency;

        public virtual int IntegerPart { get; private set; }
        public virtual int DecimalPart { get; private set; }
        public virtual bool AccepSignal { get; private set; }
        public virtual CultureInfo Culture { get; private set; }

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeDateAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeDateAttribute()
        {
            DateTimeKind = PromptDateTimeKind.DateOnly;
            YearKind = FormatYear.Y4;
            TimeKind = FormatTime.HMS;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeDateAttribute(FormatYear formatYear = FormatYear.Y4,CultureInfo? cultureInfo = null)
        {
            DateTimeKind = PromptDateTimeKind.DateOnly;
            YearKind = formatYear;
            TimeKind = FormatTime.HMS;
            Culture = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.DateOnly;
        public PromptDateTimeKind DateTimeKind { get; private set; }
        public virtual FormatYear YearKind { get; private set; }
        public virtual FormatTime TimeKind { get; private set; }
        public virtual CultureInfo Culture { get; private set; }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeTimeAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeTimeAttribute()
        {
            DateTimeKind = PromptDateTimeKind.TimeOnly;
            YearKind = FormatYear.Y4;
            TimeKind = FormatTime.HMS;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeTimeAttribute(FormatTime formatTime = FormatTime.HMS, CultureInfo? cultureInfo = null)
        {
            DateTimeKind = PromptDateTimeKind.TimeOnly;
            YearKind = FormatYear.Y4;
            TimeKind = formatTime;
            Culture = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.TimeOnly;
        public PromptDateTimeKind DateTimeKind { get; private set; }
        public virtual FormatYear YearKind { get; private set; }
        public virtual FormatTime TimeKind { get; private set; }
        public virtual CultureInfo Culture { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeBrowserAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeBrowserAttribute(
            BrowserFilter kind = BrowserFilter.None,
            string prefixExtension = null,
            string searchPattern = null,
            bool allowNotSelected = false)
        {
            Kind = kind;
            PrefixExtension = prefixExtension;
            SearchPattern = searchPattern;
            AllowNotSelected = allowNotSelected;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.Browser;

        public bool AllowNotSelected { get; private set; }
        public BrowserFilter Kind { get; private set; }
        public string PrefixExtension { get; private set; }
        public string SearchPattern { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeMaskedAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeMaskedAttribute(
            string mask,
            bool uppercase)
        {
            Mask = mask;
            Uppercase = uppercase;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.Generic;
        public string Mask { get; private set; }
        public bool Uppercase { get; private set; }
    }

    internal interface IPromptType
    {
        PromptPlusTypeKind TypeKind { get; }
    }

    public enum PromptPlusTypeKind
    {
        None,
        Number,
        Currency,
        DateOnly,
        TimeOnly,
        Browser,
        Generic
    }

}
