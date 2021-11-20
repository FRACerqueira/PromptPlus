using System;
using System.Globalization;
using System.Threading;

using CommandDotNet;

using PPlus.Objects;

namespace PromptPlusCommandDotNet
{

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
                throw new InvalidConfigurationException(
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
                throw new InvalidConfigurationException(
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
                throw new InvalidConfigurationException(
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
    public class PromptPlusTypeDateTimeAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeDateTimeAttribute()
        {
            DateTimeKind = PromptDateTimeKind.DateOnly;
            YearKind = FormatYear.Y4;
            TimeKind = FormatTime.HMS;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeDateTimeAttribute(PromptDateTimeKind dateTimeKind = PromptDateTimeKind.DateOnly, FormatYear formatYear = FormatYear.Y4, FormatTime formatTime = FormatTime.HMS, CultureInfo? cultureInfo = null)
        {
            DateTimeKind = dateTimeKind;
            YearKind = formatYear;
            TimeKind = formatTime;
            Culture = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.DateTime;
        public PromptDateTimeKind DateTimeKind { get; private set; }
        public virtual FormatYear YearKind { get; private set; }
        public virtual FormatTime TimeKind { get; private set; }
        public virtual CultureInfo Culture { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PromptPlusTypeBrowserAttribute : Attribute, IPromptType
    {
        public PromptPlusTypeBrowserAttribute()
        {
        }

        public PromptPlusTypeBrowserAttribute(
            BrowserFilter kind = BrowserFilter.None,
            string defaultvalue = null,
            string prefixExtension = null,
            string root = null,
            string searchPattern = null,
            bool allowNotSelected = false)
        {
            Kind = kind;
            DefaultValue = defaultvalue;
            PrefixExtension = prefixExtension;
            Root = root;
            SearchPattern = searchPattern;
            AllowNotSelected = allowNotSelected;
        }

        public PromptPlusTypeKind TypeKind => PromptPlusTypeKind.Browser;

        public bool AllowNotSelected { get; private set; }
        public BrowserFilter Kind { get; private set; }
        public string DefaultValue { get; private set; }
        public string PrefixExtension { get; private set; }
        public string Root { get; private set; }
        public string SearchPattern { get; private set; }
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
        DateTime,
        Browser
    }

}
