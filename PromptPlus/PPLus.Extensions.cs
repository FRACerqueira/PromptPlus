// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;

using PromptPlus.Internal;
using PromptPlus.ValueObjects;

namespace PromptPlus
{
    public static partial class PPlus
    {
        public static IFormPPlusBase Step(this IFormPPlusBase form, string title, Func<ResultPipe[], object, bool> condition = null, object contextstate = null, string id = null)
        {
            form.PipeId = id ?? Guid.NewGuid().ToString();
            form.PipeTitle = !string.IsNullOrEmpty(title) ? title : Messages.EmptyTitle;
            form.PipeCondition = condition;
            form.ContextState = contextstate;
            return form;
        }

        public static TypeCode GetTypeCode<T>(this ResultPPlus<T> result) => LocalGetTypeCode(result);
        public static bool ToBoolean<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (bool)ChangeType(result);
        public static byte ToByte<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (byte)ChangeType(result);
        public static char ToChar<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (char)ChangeType(result);
        public static DateTime ToDateTime<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (DateTime)ChangeType(result);
        public static decimal ToDecimal<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (decimal)ChangeType(result);
        public static double ToDouble<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (double)ChangeType(result);
        public static short ToInt16<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (short)ChangeType(result);
        public static int ToInt32<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (int)ChangeType(result);
        public static long ToInt64<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (long)ChangeType(result);
        public static sbyte ToSByte<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (sbyte)ChangeType(result);
        public static float ToSingle<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (float)ChangeType(result);
        public static string ToString<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (string)ChangeType(result);
        public static object ToType<T>(this ResultPPlus<T> result, Type conversionType, IFormatProvider provider = null) => Convert.ChangeType(result, conversionType);
        public static ushort ToUInt16<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (ushort)ChangeType(result);
        public static uint ToUInt32<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (uint)ChangeType(result);
        public static ulong ToUInt64<T>(this ResultPPlus<T> result, IFormatProvider provider = null) => (ulong)ChangeType(result);
        private static object ChangeType<T>(ResultPPlus<T> promptres)
        {
            switch (LocalGetTypeCode(promptres))
            {
                case TypeCode.Boolean:
                    if (promptres.IsAborted)
                    {
                        return false;
                    }
                    return Convert.ToDateTime(promptres.Value);
                case TypeCode.Byte:
                    if (promptres.IsAborted)
                    {
                        return new byte();
                    }
                    return Convert.ToByte(promptres.Value);
                case TypeCode.Char:
                    if (promptres.IsAborted)
                    {
                        return new char();
                    }
                    return Convert.ToChar(promptres.Value);
                case TypeCode.DateTime:
                    if (promptres.IsAborted)
                    {
                        return new DateTime();
                    }
                    return Convert.ToDateTime(promptres.Value);
                case TypeCode.Decimal:
                    if (promptres.IsAborted)
                    {
                        return new decimal();
                    }
                    return Convert.ToDecimal(promptres.Value);
                case TypeCode.Double:
                    if (promptres.IsAborted)
                    {
                        return new double();
                    }
                    return Convert.ToDouble(promptres.Value);
                case TypeCode.Int16:
                    if (promptres.IsAborted)
                    {
                        return new short();
                    }
                    return Convert.ToInt16(promptres.Value);
                case TypeCode.Int32:
                    if (promptres.IsAborted)
                    {
                        return new int();
                    }
                    return Convert.ToInt32(promptres.Value);
                case TypeCode.Int64:
                    if (promptres.IsAborted)
                    {
                        return new long();
                    }
                    return Convert.ToInt64(promptres.Value);
                case TypeCode.SByte:
                    if (promptres.IsAborted)
                    {
                        return new sbyte();
                    }
                    return Convert.ToSByte(promptres.Value);
                case TypeCode.Single:
                    if (promptres.IsAborted)
                    {
                        return new float();
                    }
                    return Convert.ToSingle(promptres.Value);
                case TypeCode.String:
                    if (promptres.IsAborted)
                    {
                        return string.Empty;
                    }
                    return Convert.ToString(promptres.Value);
                case TypeCode.UInt16:
                    if (promptres.IsAborted)
                    {
                        return new ushort();
                    }
                    return Convert.ToUInt16(promptres.Value);
                case TypeCode.UInt32:
                    if (promptres.IsAborted)
                    {
                        return new uint();
                    }
                    return Convert.ToUInt32(promptres.Value);
                case TypeCode.UInt64:
                    if (promptres.IsAborted)
                    {
                        return new ulong();
                    }
                    return Convert.ToUInt64(promptres.Value);
            }
            return null;
        }
        private static TypeCode LocalGetTypeCode<T>(ResultPPlus<T> result)
        {
            var type = result.GetType().Name;
            if (type == "Boolean")
            {
                return TypeCode.Boolean;
            }
            if (type == "Byte")
            {
                return TypeCode.Byte;
            }
            if (type == "Char")
            {
                return TypeCode.Char;
            }
            if (type == "DateTime")
            {
                return TypeCode.DateTime;
            }
            if (type == "DBNull")
            {
                return TypeCode.DBNull;
            }
            if (type == "Decimal")
            {
                return TypeCode.Decimal;
            }
            if (type == "Double")
            {
                return TypeCode.Double;
            }
            if (type == "Int16")
            {
                return TypeCode.Int16;
            }
            if (type == "Int32")
            {
                return TypeCode.Int32;
            }
            if (type == "Int64")
            {
                return TypeCode.Int64;
            }
            if (type == "Int64")
            {
                return TypeCode.Int64;
            }
            if (type == "SByte")
            {
                return TypeCode.SByte;
            }
            if (type == "Single")
            {
                return TypeCode.Single;
            }
            if (type == "String")
            {
                return TypeCode.String;
            }
            if (type == "String")
            {
                return TypeCode.String;
            }
            if (type == "UInt16")
            {
                return TypeCode.UInt16;
            }
            if (type == "UInt32")
            {
                return TypeCode.UInt32;
            }
            if (type == "UInt64")
            {
                return TypeCode.UInt64;
            }
            return TypeCode.Object;
        }
    }
}
