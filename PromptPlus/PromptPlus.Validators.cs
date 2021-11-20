// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus
{
    public static class PromptPlusValidators
    {
        /// <summary>
        /// Create Function to validate Uri Scheme
        /// </summary>
        /// <param name="uriKind">Kind of uri</param>
        /// <param name="allowedUriSchemes">list of allowed uri scheme. Schemes must be separated by a semicolon.</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns></returns>
        public static Func<object, ValidationResult> IsUriScheme(UriKind uriKind = UriKind.Absolute, string allowedUriSchemes = null, string errorMessage = null)
        {
            return input =>
            {
                var allowedScheme = (allowedUriSchemes ?? string.Empty).Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = Uri.TryCreate(input.ToString(), uriKind, out var uri);
                if (result && allowedScheme.Length > 0)
                {
                    if (!allowedScheme.Any(x => x.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        result = false;
                    }
                }
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsNumber(CultureInfo? cultureinfo = null, string errorMessage = null)
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
                var numOk = double.TryParse(localinput, NumberStyles.Number, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _);
                if (!numOk)
                {
                    return new ValidationResult(Messages.Invalid);
                }
                return ValidationResult.Success;
            };
        }

        public static Func<object, ValidationResult> IsCurrency(CultureInfo? cultureinfo = null, string errorMessage = null)
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
                var numOk = double.TryParse(localinput, NumberStyles.Currency, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _);
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
                var currentcult = cultureinfo ?? Thread.CurrentThread.CurrentUICulture;
                var dateOk = DateTime.TryParseExact(localinput, currentcult.DateTimeFormat.GetAllDateTimePatterns(), currentcult, DateTimeStyles.None, out _);
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
                if (input is not string)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
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
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }

                if (strValue.Length >= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? string.Format(Messages.MinLength, length));
            };
        }

        public static Func<object, ValidationResult> MaxLength(int length, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }

                if (strValue.Length <= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? string.Format(Messages.MaxLength, length));
            };
        }

        public static Func<object, ValidationResult> RegularExpression(string pattern, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }

                if (Regex.IsMatch(strValue, pattern))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.NoMatchRegex);
            };
        }

        public static Func<object, ValidationResult> IsTypeBoolean(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = bool.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeByte(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = byte.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }
        public static Func<object, ValidationResult> IsTypeChar(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = char.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeDecimal(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = decimal.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeDouble(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = double.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeShort(string errorMessage = null)
        {
            return IsTypeInt16(errorMessage);
        }

        public static Func<object, ValidationResult> IsTypeInt16(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = short.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeInt(string errorMessage = null)
        {
            return IsTypeInt32(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeInt32(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = int.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeLong(string errorMessage = null)
        {
            return IsTypeInt64(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeInt64(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = long.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeSByte(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = sbyte.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeFloat(string errorMessage = null)
        {
            return IsTypeSingle(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeSingle(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = float.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeUshort(string errorMessage = null)
        {
            return IsTypeUInt16(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeUInt16(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = ushort.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeUInt(string errorMessage = null)
        {
            return IsTypeUInt32(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeUInt32(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = uint.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeULong(string errorMessage = null)
        {
            return IsTypeUInt64(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeUInt64(string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = ulong.TryParse(input.ToString(), out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        public static Func<object, ValidationResult> IsTypeDateTime(string errorMessage = null)
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
                var currentcult = Thread.CurrentThread.CurrentUICulture;
                var dateOk = DateTime.TryParse(localinput, currentcult, DateTimeStyles.None, out _);
                if (!dateOk)
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                return ValidationResult.Success;
            };
        }
    }
}
