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
                string localinput = null;
                if (input is string)
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (input.GetType().Equals(typeof(Uri)))
                {
                    localinput = input.ToString();
                }
                if (localinput is null)
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
                else if (IsNumber(input.GetType()))
                {
                    return ValidationResult.Success;
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
                else if (IsNumber(input.GetType()))
                {
                    return ValidationResult.Success;
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
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.DateTime)
                {
                    return ValidationResult.Success;
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                    if (localinput == "0")
                    {
                        localinput = false.ToString();
                    }
                    else if (localinput == "1")
                    {
                        localinput = true.ToString();
                    }
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                    if (localinput == "0")
                    {
                        localinput = false.ToString();
                    }
                    else if (localinput == "1")
                    {
                        localinput = true.ToString();
                    }
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Boolean)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = bool.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Byte)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = byte.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Char)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                var result = char.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Decimal)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = decimal.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Decimal)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = double.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Int16)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = short.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Int32)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = int.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.Invalid);
            };
        }

        private static string NormalizeFormatNumber(string value)
        {
            var grpsep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;
            value = value
                .Replace(grpsep, "")
                .Replace(Thread.CurrentThread.CurrentCulture.NumberFormat.PositiveSign, "");
            if (value.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NegativeSign))
            {
                value = value
                .Replace(Thread.CurrentThread.CurrentCulture.NumberFormat.NegativeSign, "");
                value = $"{Thread.CurrentThread.CurrentCulture.NumberFormat.NegativeSign}{value}";
            }
            return value;
        }

        public static Func<object, ValidationResult> IsTypeLong(string errorMessage = null)
        {
            return IsTypeInt64(errorMessage);
        }
        public static Func<object, ValidationResult> IsTypeInt64(string errorMessage = null)
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
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Int64)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = long.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.SByte)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = sbyte.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.Single)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = float.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.UInt16)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = ushort.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.UInt32)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = uint.TryParse(localinput, out _);
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
                var localinput = string.Empty;
                if (input.GetType().Equals(typeof(string)))
                {
                    localinput = input.ToString();
                }
                else if (input.GetType().Equals(typeof(ResultMasked)))
                {
                    localinput = ((ResultMasked)input).Masked;
                }
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.UInt64)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage ?? Messages.Invalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = ulong.TryParse(localinput, out _);
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
                else if (Type.GetTypeCode(input.GetType()) == TypeCode.DateTime)
                {
                    return ValidationResult.Success;
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

        private static bool IsNumber(Type value)
        {
            return Type.GetTypeCode(value) switch
            {
                TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
                _ => false,
            };
        }
    }
}
