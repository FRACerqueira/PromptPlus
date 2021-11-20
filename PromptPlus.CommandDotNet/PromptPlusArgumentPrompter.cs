using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;

using CommandDotNet;
using CommandDotNet.Prompts;

using PPlus;
using PPlus.Drivers;

using PPlus.Objects;

namespace PromptPlusCommandDotNet
{
    /// <summary>
    /// Contains the logic to prompt for the various types of arguments.
    /// </summary>
    public class PromptPlusArgumentPrompter : IArgumentPrompter
    {
        private readonly int _pageSize;
        private readonly Func<CommandContext, IArgument, string>? _getPromptTextCallback;

        /// <summary>
        /// Contains the logic to prompt for the various types of arguments.
        /// </summary>
        /// <param name="pageSize">the page size for selection lists.</param>
        /// <param name="getPromptTextCallback">Used to customize the generation of the prompt text.</param>
        public PromptPlusArgumentPrompter(
            int pageSize = 10,
            Func<CommandContext, IArgument, string>? getPromptTextCallback = null)
        {
            _pageSize = pageSize;
            _getPromptTextCallback = getPromptTextCallback;
        }

        public virtual ICollection<string> PromptForArgumentValues(
            CommandContext ctx, IArgument argument, out bool isCancellationRequested)
        {
            PromptPlus.DriveConsole(ctx.Services.GetOrThrow<IConsoleDriver>());

            var kindprompt = PromptPlusUtil.EnsureValidPromptPlusType(argument);

            if (kindprompt == PromptPlusTypeKind.None)
            {
                return PromptForTypeArgumentValues(ctx, argument, out isCancellationRequested);
            }
            else
            {
                return PromptForPromptPlusTypeArgumentValues(ctx, argument, kindprompt, out isCancellationRequested);
            }
        }

        private ICollection<string> PromptForTypeArgumentValues(CommandContext ctx, IArgument argument, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var description = _getPromptTextCallback?.Invoke(ctx, argument) ?? string.Empty;
            if (string.IsNullOrEmpty(description))
            {
                var aux = PromptPlusUtil.FindAttribute<DescriptionAttribute>(argument);
                if (aux is not null)
                {
                    description = aux.Description;
                }
            }
            if (string.IsNullOrEmpty(description))
            {
                if (argument.TypeInfo.Type == typeof(DateTime))
                {
                    description = $"Type({argument.TypeInfo.DisplayName} - YYYY/MM/DD)";
                }
                else
                {
                    description = $"Type({argument.TypeInfo.DisplayName})";
                }
            }
            else
            {
                if (argument.TypeInfo.Type == typeof(DateTime))
                {
                    description = $". Type({argument.TypeInfo.DisplayName} - YYYY/MM/DD)";
                }
                else
                {
                    description += $". Type({argument.TypeInfo.DisplayName})";
                }
            }

            var promptText = argument.Name;
            var isPassword = argument.TypeInfo.UnderlyingType == typeof(Password);
            var defaultValue = argument.Default?.Value;

            Func<object, ValidationResult> validatetypeRecognized;
            var mask = string.Empty;
            TypeCode typeCodearg;
            if (argument.TypeInfo.Type.IsGenericType && argument.TypeInfo.Type.GenericTypeArguments.Count() == 1)
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

                    return PromptAllowedManyValues(ctx, _pageSize, promptText, description, argument, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        var c = PromptPlus
                                .ListMasked(promptText, description)
                                .MaskType(MaskedType.Generic, mask)
                                .PageSize(_pageSize)
                                .ShowInputType(false)
                                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                .AddValidators(argument.ImportUserValidationAttribute())
                                .AddValidator(validatetypeRecognized);

                        var p = c.HideAfterFinish(true)
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
                        var uriAtt = PromptPlusUtil.FindAttribute<PromptValidatorUriAttribute>(argument);
                        var c = PromptPlus
                                .List<string>(promptText, description)
                                .PageSize(_pageSize)
                                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                .AddValidators(argument.ImportUserValidationAttribute());

                        if (uriAtt is not null)
                        {
                            c.AddValidator(PromptPlusValidators.IsUriScheme(uriAtt.Kind, uriAtt.AllowedUriScheme));
                        }
                        var p = c.HideAfterFinish(true)
                                .Run(ctx.CancellationToken);
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
                    return PromptAllowedOnlyValue(ctx, _pageSize, promptText, description, argument, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        if (argument.TypeInfo.Type == typeof(bool))
                        {
                            return PromptBooleanValue(ctx, defaultValue != null && (bool)defaultValue, promptText, description, argument.AllowedValues.ToArray(), out isCancellationRequested);
                        }
                        var p = PromptPlus
                             .MaskEdit(MaskedType.Generic, promptText, description)
                             .Mask(mask)
                             .AddValidators(argument.ImportUserValidationAttribute())
                             .AddValidator(validatetypeRecognized)
                             .HideAfterFinish(true)
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
                        return PromptDefaultValue(ctx, isPassword, promptText, description, argument, out isCancellationRequested);
                    }
                }
            }
        }

        private ICollection<string> PromptForPromptPlusTypeArgumentValues(CommandContext ctx, IArgument argument, PromptPlusTypeKind kind, out bool isCancellationRequested)
        {

            isCancellationRequested = false;
            var description = _getPromptTextCallback?.Invoke(ctx, argument) ?? string.Empty;
            if (string.IsNullOrEmpty(description))
            {
                var aux = PromptPlusUtil.FindAttribute<DescriptionAttribute>(argument);
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
                        var numberAtt = PromptPlusUtil.FindAttribute<PromptPlusTypeNumberAttribute>(argument);
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.Number)
                            .AmmoutPositions(numberAtt.IntegerPart, numberAtt.DecimalPart)
                            .AcceptSignal(numberAtt.AccepSignal)
                            .PageSize(_pageSize)
                            .Culture(numberAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var currencyAtt = PromptPlusUtil.FindAttribute<PromptPlusTypeCurrencyAttribute>(argument);
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.Currency)
                            .AmmoutPositions(currencyAtt.IntegerPart, currencyAtt.DecimalPart)
                            .AcceptSignal(currencyAtt.AccepSignal)
                            .PageSize(_pageSize)
                            .Culture(currencyAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var DateTimeAtt = PromptPlusUtil.FindAttribute<PromptPlusTypeDateTimeAttribute>(argument);
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType((MaskedType)Enum.Parse(typeof(MaskedType), DateTimeAtt.DateTimeKind.ToString(), true))
                            .PageSize(_pageSize)
                            .FillZeros(true)
                            .FormatTime(DateTimeAtt.TimeKind)
                            .FormatYear(DateTimeAtt.YearKind)
                            .Culture(DateTimeAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var att = PromptPlusUtil.FindAttribute<PromptPlusTypeNumberAttribute>(argument);
                        var p1 = PromptPlus.MaskEdit(MaskedType.Number, promptText, description)
                            .AmmoutPositions(att.IntegerPart, att.DecimalPart)
                            .AcceptSignal(att.AccepSignal)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var att = PromptPlusUtil.FindAttribute<PromptPlusTypeNumberAttribute>(argument);
                        var p1 = PromptPlus.MaskEdit(MaskedType.Currency, promptText, description)
                            .AmmoutPositions(att.IntegerPart, att.DecimalPart)
                            .AcceptSignal(att.AccepSignal)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var att = PromptPlusUtil.FindAttribute<PromptPlusTypeDateTimeAttribute>(argument);
                        var p1 = PromptPlus.MaskEdit((MaskedType)Enum.Parse(typeof(MaskedType), att.DateTimeKind.ToString()), promptText, description)
                            .FillZeros(true)
                            .FormatTime(att.TimeKind)
                            .FormatYear(att.YearKind)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportUserValidationAttribute())
                            .HideAfterFinish(true)
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
                        var att = PromptPlusUtil.FindAttribute<PromptPlusTypeBrowserAttribute>(argument);
                        var p1 = PromptPlus.Browser(promptText, description)
                            .AllowNotSelected(att.AllowNotSelected)
                            .Default(att.DefaultValue)
                            .Filter(att.Kind)
                            .PageSize(_pageSize)
                            .PrefixExtension(att.PrefixExtension)
                            .PromptCurrentPath(true)
                            .SupressHidden(true)
                            .promptSearchPattern(true)
                            .Root(att.Root)
                            .SearchPattern(att.SearchPattern)
                            .HideAfterFinish(true)
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
                         $"Invalid PromptTypeKind {kind} for {argument.Name}. Remove PromptPlus Type!");
                }
            }
        }

        private static ICollection<string> PromptBooleanValue(CommandContext ctx, bool defaultValue, string promptText, string description, string[] allowervalues, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var c = PromptPlus
                .SliderSwitch(promptText, description)
                .Default(defaultValue);
            if (allowervalues.Length == 2)
            {
                c.OnValue(allowervalues[0]);
                c.OffValue(allowervalues[1]);
            }
            var p = c.HideAfterFinish(true)
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value.ToString() };
        }

        private static ICollection<string> PromptDefaultValue(CommandContext ctx, bool isPassword, string promptText, string description, IArgument argument, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var uriAtt = PromptPlusUtil.FindAttribute<PromptValidatorUriAttribute>(argument);

            var c = PromptPlus
                .Input(promptText, description)
                .Default(argument.Default?.Value?.ToString())
                .AddValidators(argument.ImportUserValidationAttribute())
                .AddValidator(argument.Arity.Minimum > 0 ? PromptPlusValidators.Required() : null);
            if (isPassword)
            {
                c.IsPassword(isPassword);
            }
            if (uriAtt is not null)
            {
                c.AddValidator(PromptPlusValidators.IsUriScheme(uriAtt.Kind, uriAtt.AllowedUriScheme));
            }
            var p = c.HideAfterFinish(true)
                .Run(ctx.CancellationToken);

            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value };
        }

        private static ICollection<string> PromptAllowedManyValues(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var p = PromptPlus
                .MultiSelect<string>(promptText, description)
                .AddItems(argument.AllowedValues)
                .AddDefault(argument.Default?.Value?.ToString())
                .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                .PageSize(pageSize)
                .HideAfterFinish(true)
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return p.Value.ToArray();
        }

        private static ICollection<string> PromptAllowedOnlyValue(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument, out bool isCancellationRequested)
        {
            isCancellationRequested = false;
            var p = PromptPlus
                .Select<string>(promptText, description)
                .AddItems(argument.AllowedValues)
                .Default(argument.Default?.Value?.ToString())
                .PageSize(pageSize)
                .HideAfterFinish(true)
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value };
        }

    }
}
