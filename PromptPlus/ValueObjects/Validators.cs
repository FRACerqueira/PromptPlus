// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

using PromptPlusControls.Internal;

namespace PromptPlusControls.ValueObjects
{
    public static class Validators
    {
        public static Func<object, ValidationResult> IsNumber(MaskedOptionNumberType optionmask, CultureInfo? cultureinfo = null, string errorMessage = null)
        {
            return input =>
            {
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var numOk = optionmask switch
                {
                    MaskedOptionNumberType.Number => double.TryParse(localinput, NumberStyles.Number, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _),
                    MaskedOptionNumberType.Currency => double.TryParse(localinput, NumberStyles.Currency, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _),
                    _ => false,
                };
                if (!numOk)
                {
                    return new ValidationResult(Messages.Invalid);
                }
                return ValidationResult.Success;
            };
        }


        public static Func<object, ValidationResult> IsDateTime(CultureInfo? cultureinfo = null, string errorMessage = null)
        {
            return input =>
            {
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var dateOk = DateTime.TryParseExact(localinput, cultureinfo.DateTimeFormat.GetAllDateTimePatterns(), cultureinfo ?? Thread.CurrentThread.CurrentUICulture, DateTimeStyles.None, out _);
                if (!dateOk)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                return ValidationResult.Success;
            };
        }

        public static Func<object, ValidationResult> Required(string errorMessage = null)
        {
            return input =>
            {
                if (input == null)
                {
                    return new ValidationResult(errorMessage ?? Messages.Required);
                }

                if (input is string strValue && string.IsNullOrEmpty(strValue))
                {
                    return new ValidationResult(errorMessage ?? Messages.Required);
                }

                return ValidationResult.Success;
            };
        }

        public static Func<object, ValidationResult> MinLength(int length, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return ValidationResult.Success;
                }

                if (strValue.Length >= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.MinLength);
            };
        }

        public static Func<object, ValidationResult> MaxLength(int length, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return ValidationResult.Success;
                }

                if (strValue.Length <= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.MaxLength);
            };
        }

        public static Func<object, ValidationResult> RegularExpression(string pattern, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return ValidationResult.Success;
                }

                if (Regex.IsMatch(strValue, pattern))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.NoMatchRegex);
            };
        }
    }
}
