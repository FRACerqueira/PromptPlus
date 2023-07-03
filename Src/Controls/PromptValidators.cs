// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;

namespace PPlus.Controls
{
    public static class PromptValidators
    {
        /// <summary>
        /// Import Validators from object to control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// Typeof object
        /// <param name="instance"></param>
        /// Instance of object
        /// <param name="expression"></param>
        /// The expression to object property 
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult>[] ImportValidators<T>(T instance, Expression<Func<T, object>> expression)
        {
            return ImportValidators(instance, expression.Body).ToArray();
        }

         /// <summary>
        /// Function to validate Uri Scheme
        /// </summary>
        /// <param name="uriKind">Kind of uri</param>
        /// <param name="allowedUriSchemes">list of allowed uri scheme. Schemes must be separated by a semicolon.</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
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

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Number
        /// </summary>
        /// <param name="cultureinfo">Culture to validate</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var numOk = double.TryParse(localinput, NumberStyles.Number, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _);
                if (!numOk)
                {
                    return new ValidationResult(Messages.ValidateInvalid);
                }
                return ValidationResult.Success;
            };
        }

        /// <summary>
        /// Function to validate Currency
        /// </summary>
        /// <param name="cultureinfo">Culture to validate</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var numOk = double.TryParse(localinput, NumberStyles.Currency, cultureinfo ?? Thread.CurrentThread.CurrentUICulture, out _);
                if (!numOk)
                {
                    return new ValidationResult(Messages.ValidateInvalid);
                }
                return ValidationResult.Success;
            };
        }


        /// <summary>
        /// Function to validate DateTime
        /// </summary>
        /// <param name="cultureinfo">Culture to validate</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var currentcult = cultureinfo ?? Thread.CurrentThread.CurrentUICulture;
                var dateOk = DateTime.TryParseExact(localinput, currentcult.DateTimeFormat.GetAllDateTimePatterns(), currentcult, DateTimeStyles.None, out _);
                if (!dateOk)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                return ValidationResult.Success;
            };
        }

        /// <summary>
        /// Function to validate Required
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> Required(string errorMessage = null)
        {
            return input =>
            {
                if (input == null)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateRequired);
                }
                if (input is not string)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                if (input is string strValue && string.IsNullOrEmpty(strValue))
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateRequired);
                }
                return ValidationResult.Success;
            };
        }

        /// <summary>
        /// Function to validate MinLength
        /// </summary>
        /// <param name="length">MinLength value</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> MinLength(int length, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }

                if (strValue.Length >= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? string.Format(Messages.ValidateMinLength, length));
            };
        }

        /// <summary>
        /// Function to validate MaxLength
        /// </summary>
        /// <param name="length">MaxLength value</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> MaxLength(int length, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }

                if (strValue.Length <= length)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? string.Format(Messages.ValidateMaxLength, length));
            };
        }

        /// <summary>
        /// Function to validate RegularExpression
        /// </summary>
        /// <param name="pattern">RegularExpression value</param>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> RegularExpression(string pattern, string errorMessage = null)
        {
            return input =>
            {
                if (input is not string strValue)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }

                if (Regex.IsMatch(strValue, pattern))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateNoMatchRegex);
            };
        }

        /// <summary>
        /// Function to validate Is Type Boolean
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var result = bool.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Byte
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>

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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var result = byte.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Char
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var result = char.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Decimal
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = decimal.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Double
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = double.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Short
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeShort(string errorMessage = null)
        {
            return IsTypeInt16(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type Int16
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = short.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Int
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeInt(string errorMessage = null)
        {
            return IsTypeInt32(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type Int32
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = int.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Long
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeLong(string errorMessage = null)
        {
            return IsTypeInt64(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type Int64
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = long.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type SByte
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = sbyte.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Float
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeFloat(string errorMessage = null)
        {
            return IsTypeSingle(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type Single
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = float.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type Ushort
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeUshort(string errorMessage = null)
        {
            return IsTypeUInt16(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type UInt16
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = ushort.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type UInt
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeUInt(string errorMessage = null)
        {
            return IsTypeUInt32(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type UInt32
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = uint.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type ULong
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
        public static Func<object, ValidationResult> IsTypeULong(string errorMessage = null)
        {
            return IsTypeUInt64(errorMessage);
        }

        /// <summary>
        /// Function to validate Is Type UInt64
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                localinput = NormalizeFormatNumber(localinput);
                var result = ulong.TryParse(localinput, out _);
                if (result)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
            };
        }

        /// <summary>
        /// Function to validate Is Type DateTime
        /// </summary>
        /// <param name="errorMessage">Custom error message to show</param>
        /// <returns>the function to validation</returns>
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
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
                }
                var currentcult = Thread.CurrentThread.CurrentUICulture;
                var dateOk = DateTime.TryParse(localinput, currentcult, DateTimeStyles.None, out _);
                if (!dateOk)
                {
                    return new ValidationResult(errorMessage ?? Messages.ValidateInvalid);
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

        private static IList<Func<object, ValidationResult>> ImportValidators(object instance, Expression expression)
        {
            if (expression == null)
            {
                throw new PromptPlusException("ImportValidators. The expression cannot be null");
            }

            // Reference type property or field
            if (expression is MemberExpression memberExpression)
            {
                var displayAttribute = memberExpression.Member.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return memberExpression.Member.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                    (x =>
                    {
                        var validationContext = new ValidationContext(instance)
                        {
                            DisplayName = displayAttribute == null ? memberExpression.Member.Name : displayAttribute.GetPrompt(),
                            MemberName = memberExpression.Member.Name
                        };
                        ValidationResult func(object input) => x.GetValidationResult(input, validationContext);
                        return (Func<object, ValidationResult>)func;
                    })
                    .ToList();
            }
            // Reference type method
            if (expression is MethodCallExpression methodCallExpression)
            {
                var displayAttribute = methodCallExpression.Method.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return methodCallExpression.Method.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                    (x =>
                    {
                        var validationContext = new ValidationContext(instance)
                        {
                            DisplayName = displayAttribute == null ? methodCallExpression.Method.Name : displayAttribute.GetPrompt(),
                            MemberName = methodCallExpression.Method.Name
                        };
                        ValidationResult func(object input) => x.GetValidationResult(input, validationContext);
                        return (Func<object, ValidationResult>)func;
                    })
                    .ToList();
            }
            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                return ImportValidators(instance, unaryExpression);
            }
            throw new PromptPlusException("ImportValidators. Invalid expression.");
        }

        private static IList<Func<object, ValidationResult>> ImportValidators(object instance, UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                var displayAttribute = methodExpression.Method.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return methodExpression.Method.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                (x =>
                {
                    var validationContext = new ValidationContext(instance)
                    {
                        DisplayName = displayAttribute == null ? methodExpression.Method.Name : displayAttribute.GetPrompt(),
                        MemberName = methodExpression.Method.Name
                    };
                    ValidationResult func(object input) => x.GetValidationResult(input, validationContext);
                    return (Func<object, ValidationResult>)func;
                })
                .ToList();
            }
            var memberexpress = (MemberExpression)unaryExpression.Operand;
            var displayAttr = memberexpress.Member.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            return memberexpress.Member.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
            (x =>
            {
                var validationContext = new ValidationContext(instance)
                {
                    DisplayName = displayAttr == null ? memberexpress.Member.Name : displayAttr.GetPrompt(),
                    MemberName = memberexpress.Member.Name
                };
                ValidationResult func(object input) => x.GetValidationResult(input, validationContext);
                return (Func<object, ValidationResult>)func;
            })
            .ToList();
        }

    }
}
