using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using CommandDotNet;

using PPlus.Attributes;
using PPlus.CommandDotNet.Resources;
using PPlus.Objects;

namespace PPlus.CommandDotNet
{
    public static class UtilExtension
    {
        internal static ICollection<string> PromptForTypeArgumentValues(CommandContext ctx, IArgument argument, string description, int pageSize,bool disableEscAbort,  out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            if (string.IsNullOrEmpty(description))
            {
                var aux = argument.FindArgumentAttribute<DescriptionAttribute>();
                if (aux is not null)
                {
                    description = aux.Description;
                }
            }
            var typeinf = string.Format(Messages.WizardTypeInfo, argument.TypeInfo.DisplayName);
            if (string.IsNullOrEmpty(description))
            {
                if (argument.TypeInfo.Type == typeof(DateTime))
                {
                    description = $"{typeinf} - YYYY/MM/DD";
                }
                else
                {
                    description = $"{typeinf}";
                }
            }
            else
            {
                if (argument.TypeInfo.Type == typeof(DateTime))
                {
                    description = $". {typeinf} - YYYY/MM/DD";
                }
                else
                {
                    description += $". {typeinf}";
                }
            }

            var promptText = argument.Name;
            var isPassword = argument.TypeInfo.UnderlyingType == typeof(Password);
            var defaultValue = argument.Default?.Value;

            Func<object, ValidationResult> validatetypeRecognized;
            var mask = string.Empty;
            TypeCode typeCodearg;
            if (argument.TypeInfo.Type.IsGenericType && argument.TypeInfo.Type.GenericTypeArguments.Length == 1)
            {
                typeCodearg = Type.GetTypeCode(argument.TypeInfo.Type.GenericTypeArguments[0]);
            }
            else
            {
                typeCodearg = Type.GetTypeCode(argument.TypeInfo.Type);
            }
            switch (typeCodearg)
            {
                case TypeCode.Boolean:
                    validatetypeRecognized = PromptPlusValidators.IsTypeBoolean();
                    break;
                case TypeCode.Byte:
                    validatetypeRecognized = PromptPlusValidators.IsTypeByte();
                    mask = "9{3}";
                    break;
                case TypeCode.Char:
                    validatetypeRecognized = PromptPlusValidators.IsTypeChar();
                    mask = "A{1}";
                    break;
                case TypeCode.DateTime:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDateTime();
                    mask = "9{4}" + Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator + "C{1}[01]9{1}" + Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator + "C{1}[0123]9{1}";
                    break;
                case TypeCode.Decimal:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDecimal();
                    mask = "C{1}[+-]9{15}" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator + "9{5}";
                    break;
                case TypeCode.Double:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDouble();
                    mask = "C{1}[+-]9{15}" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator + "9{5}";
                    break;
                case TypeCode.Int16:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt16();
                    mask = "9{5}";
                    break;
                case TypeCode.Int32:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt32();
                    mask = "9{10}";
                    break;
                case TypeCode.Int64:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt64();
                    mask = "9{20}";
                    break;
                case TypeCode.SByte:
                    validatetypeRecognized = PromptPlusValidators.IsTypeSByte();
                    mask = "C{1}[+-]9{3}";
                    break;
                case TypeCode.Single:
                    validatetypeRecognized = PromptPlusValidators.IsTypeSingle();
                    mask = "9{10}";
                    break;
                case TypeCode.UInt16:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt16();
                    mask = "C{1}[+-]9{5}";
                    break;
                case TypeCode.UInt32:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt32();
                    mask = "C{1}[+-]9{10}";
                    break;
                case TypeCode.UInt64:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt64();
                    mask = "C{1}[+-]9{20}";
                    break;
                default:
                    validatetypeRecognized = null;
                    break;
            }

            if (argument.Arity.AllowsMany())
            {
                if (argument.AllowedValues.Any())
                {

                    return PromptAllowedManyValues(ctx, pageSize, promptText, description, argument, disableEscAbort, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        var p = PromptPlus
                                .ListMasked(promptText, description)
                                .MaskType(MaskedType.Generic, mask)
                                .PageSize(pageSize)
                                .ShowInputType(false)
                                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                .AddValidators(argument.ImportDataAnnotationsValidations())
                                .AddValidator(validatetypeRecognized)
                                .Config(ctx => ctx.HideAfterFinish(true))
                                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                                .Run(ctx.CancellationToken);

                        if (p.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p.Value.Select(x => x.Masked).ToArray();
                    }
                    else
                    {
                        var uriAtt = argument.FindArgumentAttribute<PromptValidatorUriAttribute>();
                        var c = PromptPlus
                                .List<string>(promptText, description)
                                .PageSize(pageSize)
                                .Config(ctx => ctx.HideAfterFinish(true))
                                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                .AddValidators(argument.ImportDataAnnotationsValidations());

                        if (uriAtt is not null)
                        {
                            c.AddValidator(PromptPlusValidators.IsUriScheme(uriAtt.Kind, uriAtt.AllowedUriScheme));
                        }
                        var p = c.Run(ctx.CancellationToken);

                        if (p.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p.Value.ToArray();
                    }
                }
            }
            else
            {
                if (argument.AllowedValues.Any() && argument.TypeInfo.Type != typeof(bool))
                {
                    return PromptAllowedOnlyValue(ctx, pageSize, promptText, description, argument, disableEscAbort, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        if (argument.TypeInfo.Type == typeof(bool))
                        {
                            return PromptBooleanValue(ctx, defaultValue != null && (bool)defaultValue, promptText, description, argument.AllowedValues.ToArray(),disableEscAbort, out isCancellationRequested);
                        }
                        var p = PromptPlus
                             .MaskEdit(MaskedType.Generic, promptText, description)
                             .Mask(mask)
                             .AddValidators(argument.ImportDataAnnotationsValidations())
                             .AddValidator(validatetypeRecognized)
                             .Config(ctx=> ctx.HideAfterFinish(true))
                             .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                             .Run(ctx.CancellationToken);

                        if (p.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new[] { p.Value.Masked };
                    }
                    else
                    {
                        return PromptSingleValue(ctx, isPassword, promptText, description, argument, disableEscAbort, out isCancellationRequested);
                    }
                }
            }
        }

        internal static ICollection<string> PromptForPromptPlusTypeArgumentValues(CommandContext ctx, IArgument argument, string description, int pageSize, PromptPlusTypeKind kind, bool disableEscAbort, out bool isCancellationRequested)
        {

            isCancellationRequested = false;
            if (string.IsNullOrEmpty(description))
            {
                var aux = argument.FindArgumentAttribute<DescriptionAttribute>();
                if (aux is not null)
                {
                    description = aux.Description;
                }
            }
            var promptText = argument.Name;
            var defaultValue = argument.Default?.Value;

            if (argument.Arity.AllowsMany())
            {
                switch (kind)
                {
                    case PromptPlusTypeKind.Number:
                    {
                        var numberAtt = argument.FindArgumentAttribute<PromptPlusTypeNumberAttribute>();
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.Number)
                            .AmmoutPositions(numberAtt.IntegerPart, numberAtt.DecimalPart)
                            .AcceptSignal(numberAtt.AccepSignal)
                            .PageSize(pageSize)
                            .Culture(numberAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p1.Value.Select(x => x.Masked).ToArray();
                    }
                    case PromptPlusTypeKind.Currency:
                    {
                        var currencyAtt = argument.FindArgumentAttribute<PromptPlusTypeCurrencyAttribute>();
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.Currency)
                            .AmmoutPositions(currencyAtt.IntegerPart, currencyAtt.DecimalPart)
                            .AcceptSignal(currencyAtt.AccepSignal)
                            .PageSize(pageSize)
                            .Culture(currencyAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p1.Value.Select(x => x.Masked).ToArray();
                    }
                    case PromptPlusTypeKind.DateTime:
                    {
                        var DateTimeAtt = argument.FindArgumentAttribute<PromptPlusTypeDateTimeAttribute>();
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType((MaskedType)Enum.Parse(typeof(MaskedType), DateTimeAtt.DateTimeKind.ToString(), true))
                            .PageSize(pageSize)
                            .FillZeros(true)
                            .FormatTime(DateTimeAtt.TimeKind)
                            .FormatYear(DateTimeAtt.YearKind)
                            .Culture(DateTimeAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p1.Value.Select(x => x.Masked).ToArray();
                    }
                    default:
                        throw new InvalidConfigurationException(
                         $"Invalid PromptTypeKind {kind} for {argument.Name}. Remove PromptPlus Type!");
                }
            }
            else
            {
                switch (kind)
                {
                    case PromptPlusTypeKind.Number:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeNumberAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.Number, promptText, description)
                            .AmmoutPositions(att.IntegerPart, att.DecimalPart)
                            .AcceptSignal(att.AccepSignal)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.Currency:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeNumberAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.Currency, promptText, description)
                            .AmmoutPositions(att.IntegerPart, att.DecimalPart)
                            .AcceptSignal(att.AccepSignal)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.DateTime:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeDateTimeAttribute>();
                        var p1 = PromptPlus.MaskEdit((MaskedType)Enum.Parse(typeof(MaskedType), att.DateTimeKind.ToString()), promptText, description)
                            .FillZeros(true)
                            .FormatTime(att.TimeKind)
                            .FormatYear(att.YearKind)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.Browser:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeBrowserAttribute>();
                        var p1 = PromptPlus.Browser(promptText, description)
                            .AllowNotSelected(att.AllowNotSelected)
                            .Filter(att.Kind)
                            .PageSize(pageSize)
                            .PrefixExtension(att.PrefixExtension)
                            .PromptCurrentPath(true)
                            .SupressHidden(true)
                            .promptSearchPattern(true)
                            .SearchPattern(att.SearchPattern)
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new[] { Path.Combine(p1.Value.PathValue, p1.Value.SelectedValue) };
                    }
                    default:
                        throw new InvalidConfigurationException(
                            string.Format(Exceptions.Ex_PromptTypeKind,kind,argument.Name));
                }
            }
        }

        private static ICollection<string> PromptBooleanValue(CommandContext ctx, bool defaultValue, string promptText, string description, string[] allowervalues, bool disableEscAbort, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var c = PromptPlus
                .SliderSwitch(promptText, description)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .Default(defaultValue);

            if (allowervalues.Length == 2)
            {
                c.OnValue(allowervalues[0]);
                c.OffValue(allowervalues[1]);
            }
            var p = c.Run(ctx.CancellationToken);

            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value.ToString() };
        }

        private static ICollection<string> PromptSingleValue(CommandContext ctx, bool isPassword, string promptText, string description, IArgument argument, bool disableEscAbort, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var uriAtt = argument.FindArgumentAttribute<PromptValidatorUriAttribute>();

            var c = PromptPlus
                .Input(promptText, description)
                .Default(argument.Default?.Value?.ToString())
                .AddValidators(argument.ImportDataAnnotationsValidations())
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .AddValidator(argument.Arity.Minimum > 0 ? PromptPlusValidators.Required() : null);
            if (isPassword)
            {
                c.IsPassword(isPassword);
            }
            if (uriAtt is not null)
            {
                c.AddValidator(PromptPlusValidators.IsUriScheme(uriAtt.Kind, uriAtt.AllowedUriScheme));
            }
            var p = c.Run(ctx.CancellationToken);

            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value };
        }

        private static ICollection<string> PromptAllowedManyValues(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument, bool disableEscAbort, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var p = PromptPlus
                .MultiSelect<string>(promptText, description)
                .AddItems(argument.AllowedValues)
                .AddDefault(argument.Default?.Value?.ToString())
                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return p.Value.ToArray();
        }

        private static ICollection<string> PromptAllowedOnlyValue(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument, bool disableEscAbort, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var p = PromptPlus
                .Select<string>(promptText, description)
                .AddItems(argument.AllowedValues)
                .Default(argument.Default?.Value?.ToString())
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Config(ctx => ctx.EnabledAbortKey(!disableEscAbort))
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value };
        }

        public static string CopyCallerDescription(this Type source, [CallerMemberName] string? caller = null)
        {
            var m = source.GetMethod(caller);
            var att = m.GetCustomAttributes().FirstOrDefault(a => a as INameAndDescription != null);
            if (att != null)
            {
                return ((INameAndDescription)att).Description;
            }
            att = m.GetCustomAttributes().FirstOrDefault(a => a as DescriptionAttribute != null);
            if (att != null)
            {
                return ((DescriptionAttribute)att).Description;
            }
            return null;
        }

        public static PromptPlusTypeKind EnsureValidPromptPlusType(this IArgument instance)
        {
            var isPassword = instance.TypeInfo.UnderlyingType == typeof(Password);
            var aux = instance.CustomAttributes.GetCustomAttributes(false).Where(a => a as IPromptType != null);
            var qtd = aux.Count();
            if (qtd == 0)
            {
                return PromptPlusTypeKind.None;
            }
            if (qtd > 1)
            {
                throw new InvalidConfigurationException(
                    string.Format(Exceptions.Ex_PromptTypeMany,instance.Name));
            }
            if (instance.AllowedValues.Any() || instance.TypeInfo.Type == typeof(bool) || isPassword)
            {
                throw new InvalidConfigurationException(
                    string.Format(Exceptions.Ex_PromptTypeError,instance.Name));
            }
            var result = PromptPlusTypeKind.None;
            if (aux is not null)
            {
                result = ((IPromptType)aux.First()).TypeKind;
            }
            if (instance.Arity.AllowsMany() && result == PromptPlusTypeKind.Browser)
            {
                throw new InvalidConfigurationException(
                    string.Format(Exceptions.Ex_PromptTypeBrowser,instance.Name));
            }
            return result;
        }

        public static T? FindArgumentAttribute<T>(this IArgument instance) where T : Attribute
        {
            var att = instance.CustomAttributes.GetCustomAttributes(false).FirstOrDefault(a => a as T != null);
            if (att != null)
            {
                return (T)att;
            }
            return null;
        }

        public static IList<Func<object, ValidationResult>>? ImportDataAnnotationsValidations(this IArgument argument)
        {
            List<Func<object, ValidationResult>>? result = new();
            var att = argument.CustomAttributes.GetCustomAttributes(true)
                .Where(x => x as ValidationAttribute != null);
            foreach (ValidationAttribute item in att)
            {
                var validationContext = new ValidationContext(argument)
                {
                    DisplayName = argument.TypeInfo.DisplayName ?? argument.Name,
                    MemberName = argument.Name
                };
                ValidationResult func(object input)
                {
                    return item.GetValidationResult(input, validationContext);
                }
                result.Add(func);
            }
            return result;
        }
    }
}
