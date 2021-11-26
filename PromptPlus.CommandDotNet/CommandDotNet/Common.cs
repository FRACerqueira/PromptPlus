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
    internal static class Common
    {
        public static ICollection<string> PromptForTypeArgumentValues(CommandContext ctx, IArgument argument, string description, int pageSize,string[] lastvalue,  out bool isCancellationRequested)
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
                if (argument.TypeInfo.Type != typeof(DateTime))
                {
                    description = $"{typeinf}";
                }
            }
            else
            {
                if (argument.TypeInfo.Type != typeof(DateTime))
                {
                    description += $". {typeinf}";
                }
            }

            var promptText = argument.Name;
            var isPassword = argument.TypeInfo.UnderlyingType == typeof(Password);
            var defaultValue = argument.Default?.Value;

            Func<object, ValidationResult> validatetypeRecognized;

            var mask = string.Empty;
            (int intpos, int decpos) range = new(-1, -1);
            var accepSignal = false;
            var ctrlmasktype = MaskedType.Generic;

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
                    range.intpos = 3;
                    range.decpos = 0;
                    break;
                case TypeCode.Char:
                    validatetypeRecognized = PromptPlusValidators.IsTypeChar();
                    mask = "A{1}";
                    break;
                case TypeCode.DateTime:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDateTime();
                    ctrlmasktype = MaskedType.DateOnly;
                    break;
                case TypeCode.Decimal:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDecimal();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 15;
                    range.decpos = 5;
                    accepSignal = true;
                    break;
                case TypeCode.Double:
                    validatetypeRecognized = PromptPlusValidators.IsTypeDouble();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 15;
                    range.decpos = 5;
                    accepSignal = true;
                    break;
                case TypeCode.Int16:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt16();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 5;
                    range.decpos = 0;
                    accepSignal = true;
                    break;
                case TypeCode.Int32:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt32();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 10;
                    range.decpos = 0;
                    accepSignal = true;
                    break;
                case TypeCode.Int64:
                    validatetypeRecognized = PromptPlusValidators.IsTypeInt64();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 20;
                    range.decpos = 0;
                    accepSignal = true;
                    break;
                case TypeCode.SByte:
                    validatetypeRecognized = PromptPlusValidators.IsTypeSByte();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 3;
                    range.decpos = 0;
                    accepSignal = true;
                    break;
                case TypeCode.Single:
                    validatetypeRecognized = PromptPlusValidators.IsTypeSingle();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 10;
                    range.decpos = 0;
                    accepSignal = true;
                    break;
                case TypeCode.UInt16:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt16();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 5;
                    range.decpos = 0;
                    break;
                case TypeCode.UInt32:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt32();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 10;
                    range.decpos = 0;
                    break;
                case TypeCode.UInt64:
                    validatetypeRecognized = PromptPlusValidators.IsTypeUInt64();
                    ctrlmasktype = MaskedType.Number;
                    range.intpos = 20;
                    range.decpos = 0;
                    break;
                default:
                    validatetypeRecognized = null;
                    break;
            }

            if (argument.Arity.AllowsMany())
            {
                if (argument.AllowedValues.Any())
                {
                    return PromptAllowedManyValues(ctx, pageSize, promptText, description, argument,lastvalue, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        var p = new ResultPromptPlus<IEnumerable<ResultMasked>>(null,true);
                        switch (ctrlmasktype)
                        {
                            case MaskedType.DateOnly:
                                p = PromptPlus
                                    .ListMasked(promptText,description) 
                                    .MaskType(MaskedType.DateOnly)
                                    .FillZeros(true)
                                    .AddItems(lastvalue)
                                    .PageSize(pageSize)
                                    .ShowInputType(true)
                                    .ShowDayWeek(FormatWeek.Short)
                                    .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                    .AddValidators(argument.ImportDataAnnotationsValidations())
                                    .AddValidator(validatetypeRecognized)
                                    .Config(ctx => ctx.HideAfterFinish(true))
                                    .Run(ctx.CancellationToken);
                                break;
                            case MaskedType.Number:
                                p = PromptPlus
                                    .ListMasked(promptText, description)
                                    .MaskType(MaskedType.Number)
                                    .AmmoutPositions(range.intpos,range.decpos)
                                    .AddItems(lastvalue)
                                    .AcceptSignal(accepSignal)
                                    .PageSize(pageSize)
                                    .ShowInputType(false)
                                    .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                    .AddValidators(argument.ImportDataAnnotationsValidations())
                                    .AddValidator(validatetypeRecognized)
                                    .Config(ctx => ctx.HideAfterFinish(true))
                                    .Run(ctx.CancellationToken);
                                break;
                            case MaskedType.Generic:
                                p = PromptPlus
                                    .ListMasked(promptText, description)
                                    .MaskType(MaskedType.Generic, mask)
                                    .PageSize(pageSize)
                                    .AddItems(lastvalue)
                                    .ShowInputType(true)
                                    .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                                    .AddValidators(argument.ImportDataAnnotationsValidations())
                                    .AddValidator(validatetypeRecognized)
                                    .Config(ctx => ctx.HideAfterFinish(true))
                                    .Run(ctx.CancellationToken);
                                break;
                        }
                        if (p.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        if (ctrlmasktype == MaskedType.Number)
                        {
                            var aux = p.Value.Select(x => x.Masked).ToArray();
                            var grpsep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;
                            for (var i = 0; i < aux.Length; i++)
                            {
                                aux[i] = aux[i].Replace(grpsep, "").TrimStart('0');
                                if (string.IsNullOrEmpty(aux[i]))
                                {
                                    aux[i] = "0";
                                }
                            }
                            return aux;
                        }
                        return p.Value.Select(x => x.Masked).ToArray();
                    }
                    else
                    {
                        var uriAtt = argument.FindArgumentAttribute<PromptValidatorUriAttribute>();
                        var c = PromptPlus
                                .List<string>(promptText, description)
                                .PageSize(pageSize)
                                .AddItems(lastvalue)
                                .Config(ctx => ctx.HideAfterFinish(true))
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
                    return PromptAllowedOnlyValue(ctx, pageSize, promptText, description, argument,lastvalue, out isCancellationRequested);
                }
                else
                {
                    if (validatetypeRecognized != null)
                    {
                        if (argument.TypeInfo.Type == typeof(bool))
                        {
                            var defvalue = defaultValue != null && (bool)defaultValue;
                            if (lastvalue is not null)
                            {
                                if (lastvalue.Length == 1)
                                {
                                    bool.TryParse(lastvalue[0], out defvalue);
                                }
                            }
                            return PromptBooleanValue(ctx, defvalue, promptText, description, argument.AllowedValues.ToArray(), out isCancellationRequested);
                        }
                        object defmaskvalue = null;
                        var p = new ResultPromptPlus<ResultMasked>(new ResultMasked(), true);
                          switch (ctrlmasktype)
                        {
                            case MaskedType.DateOnly:
                                if (lastvalue is not null)
                                {
                                    if (lastvalue.Length == 1)
                                    {
                                        defmaskvalue = DateTime.Parse(lastvalue[0]);
                                    }
                                }
                                p = PromptPlus
                                    .MaskEdit(MaskedType.DateOnly, promptText, description)
                                    .Default(defmaskvalue)
                                    .FillZeros(true)
                                    .ShowInputType(true)
                                    .ShowDayWeek(FormatWeek.Short)
                                    .AddValidators(argument.ImportDataAnnotationsValidations())
                                    .AddValidator(validatetypeRecognized)
                                    .Config(ctx => ctx.HideAfterFinish(true))
                                    .Run(ctx.CancellationToken); break;
                            case MaskedType.Number:
                                if (lastvalue is not null)
                                {
                                    if (lastvalue.Length == 1)
                                    {
                                        defmaskvalue = double.Parse(lastvalue[0]);
                                    }
                                }
                                p = PromptPlus
                                    .MaskEdit(MaskedType.Number,promptText, description)
                                    .AmmoutPositions(range.intpos, range.decpos)
                                    .Default(defmaskvalue)
                                    .AcceptSignal(accepSignal)
                                    .ShowInputType(false)
                                    .AddValidators(argument.ImportDataAnnotationsValidations())
                                    .AddValidator(validatetypeRecognized)
                                    .Config(ctx => ctx.HideAfterFinish(true))
                                    .Run(ctx.CancellationToken);
                                break;
                            case MaskedType.Generic:
                                if (lastvalue is not null)
                                {
                                    if (lastvalue.Length == 1)
                                    {
                                        defmaskvalue = lastvalue[0];
                                    }
                                }
                                p = PromptPlus
                                     .MaskEdit(MaskedType.Generic, promptText, description)
                                     .Mask(mask)
                                     .Default(defmaskvalue)
                                     .ShowInputType(true)
                                     .AddValidators(argument.ImportDataAnnotationsValidations())
                                     .AddValidator(validatetypeRecognized)
                                     .Config(ctx => ctx.HideAfterFinish(true))
                                     .Run(ctx.CancellationToken);
                                break;
                        }
                        if (p.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        if (ctrlmasktype == MaskedType.Number)
                        {
                            var aux = p.Value.Masked;
                            var grpsep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;
                            aux = aux.Replace(grpsep, "").TrimStart('0');
                            if (string.IsNullOrEmpty(aux))
                            {
                                aux = "0";
                            }
                            return new[] { aux };
                        }
                        return new[] { p.Value.Masked };
                    }
                    else
                    {
                        return PromptSingleValue(ctx, isPassword, promptText, description, argument, DefaultValueForType(defaultValue), lastvalue, out isCancellationRequested);
                    }
                }
            }
        }

        public static ICollection<string> PromptForPromptPlusTypeArgumentValues(CommandContext ctx, IArgument argument, string description, int pageSize, PromptPlusTypeKind kind,string[] lastvalue, out bool isCancellationRequested)
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
                            .AddItems(lastvalue)
                            .PageSize(pageSize)
                            .Culture(numberAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
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
                            .AddItems(lastvalue)
                            .PageSize(pageSize)
                            .Culture(currencyAtt.Culture)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p1.Value.Select(x => x.Masked).ToArray();
                    }
                    case PromptPlusTypeKind.DateOnly:
                    {
                        var DateTimeAtt = argument.FindArgumentAttribute<PromptPlusTypeDateAttribute>();
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.DateOnly)
                            .PageSize(pageSize)
                            .FillZeros(true)
                            .FormatTime(DateTimeAtt.TimeKind)
                            .FormatYear(DateTimeAtt.YearKind)
                            .ShowDayWeek(FormatWeek.Short)
                            .Culture(DateTimeAtt.Culture)
                            .AddItems(lastvalue)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return p1.Value.Select(x => x.Masked).ToArray();
                    }
                    case PromptPlusTypeKind.TimeOnly:
                    {
                        var DateTimeAtt = argument.FindArgumentAttribute<PromptPlusTypeTimeAttribute>();
                        var p1 = PromptPlus.ListMasked(promptText, description)
                            .MaskType(MaskedType.TimeOnly)
                            .PageSize(pageSize)
                            .FillZeros(true)
                            .FormatTime(DateTimeAtt.TimeKind)
                            .FormatYear(DateTimeAtt.YearKind)
                            .ShowDayWeek(FormatWeek.Short)
                            .Culture(DateTimeAtt.Culture)
                            .AddItems(lastvalue)
                            .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
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
                            string.Format(Exceptions.Ex_PromptTypeKind,kind,argument.Name));
                }
            }
            else
            {
                if (lastvalue is not null && lastvalue.Length == 1)
                {
                    defaultValue = lastvalue[0];
                }
                switch (kind)
                {
                    case PromptPlusTypeKind.Generic:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeMaskedAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.Generic, promptText, description)
                            .Mask(att.Mask)
                            .Default(defaultValue)
                            .UpperCase(att.Uppercase)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.Number:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeNumberAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.Number, promptText, description)
                            .AmmoutPositions(att.IntegerPart, att.DecimalPart)
                            .AcceptSignal(att.AccepSignal)
                            .Default(defaultValue)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
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
                            .Default(defaultValue)
                            .Culture(att.Culture)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.DateOnly:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeDateAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.DateOnly, promptText, description)
                            .FillZeros(true)
                            .FormatTime(att.TimeKind)
                            .FormatYear(att.YearKind)
                            .ShowDayWeek(FormatWeek.Short)
                            .Culture(att.Culture)
                            .Default(defaultValue)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
                            .Run(ctx.CancellationToken);
                        if (p1.IsAborted)
                        {
                            isCancellationRequested = true;
                            return Array.Empty<string>();
                        }
                        return new string[] { p1.Value.Masked };
                    }
                    case PromptPlusTypeKind.TimeOnly:
                    {
                        var att = argument.FindArgumentAttribute<PromptPlusTypeDateAttribute>();
                        var p1 = PromptPlus.MaskEdit(MaskedType.TimeOnly, promptText, description)
                            .FillZeros(true)
                            .FormatTime(att.TimeKind)
                            .FormatYear(att.YearKind)
                            .ShowDayWeek(FormatWeek.Short)
                            .Culture(att.Culture)
                            .Default(defaultValue)
                            .AddValidators(argument.ImportDataAnnotationsValidations())
                            .Config(ctx => ctx.HideAfterFinish(true))
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
                            .Default(defaultValue?.ToString())
                            .PrefixExtension(att.PrefixExtension)
                            .PromptCurrentPath(true)
                            .SupressHidden(true)
                            .promptSearchPattern(true)
                            .SearchPattern(att.SearchPattern)
                            .Config(ctx => ctx.HideAfterFinish(true))
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

        public static ICollection<string> PromptBooleanValue(CommandContext ctx, bool defaultValue, string promptText, string description, string[] allowervalues, out bool isCancellationRequested)
        {

            isCancellationRequested = false;
            var c = PromptPlus
                .SliderSwitch(promptText, description)
                .Config(ctx => ctx.HideAfterFinish(true))
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

        public static ICollection<string> PromptSingleValue(CommandContext ctx, bool isPassword, string promptText, string description, IArgument argument, string defaultvalue,string[] lastvalue, out bool isCancellationRequested)
        {
            string initvalue = null;
            if (lastvalue is not null)
            {
                if (lastvalue.Length == 1)
                {
                    initvalue = lastvalue[0];
                }
            }

            isCancellationRequested = false;
            var uriAtt = argument.FindArgumentAttribute<PromptValidatorUriAttribute>();

            var c = PromptPlus
                .Input(promptText, description)
                .Default(defaultvalue)
                .InitialValue(initvalue)
                .AddValidators(argument.ImportDataAnnotationsValidations())
                .Config(ctx => ctx.HideAfterFinish(true))
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

        public static ICollection<string> PromptAllowedManyValues(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument,string[] lastvalue, out bool isCancellationRequested)
        {
            isCancellationRequested = false;

            if (argument.TypeInfo.UnderlyingType == typeof(bool))
            {
                var p = PromptPlus
                    .ListMasked(promptText, $"{description}. [0]False [1]True")
                    .MaskType(MaskedType.Generic, "C[01]")
                    .PageSize(pageSize)
                    .UpperCase(true)
                    .AddItems(lastvalue)
                    .TransformItems((item) =>
                    {
                        if (item.ToLowerInvariant() == "true")
                        {
                            return "1";
                        }
                        if (item.ToLowerInvariant() == "false")
                        {
                            return "0";
                        }
                        return item;
                    })
                    .ShowInputType(false)
                    .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                    .AddValidators(argument.ImportDataAnnotationsValidations())
                    .AddValidator(PromptPlusValidators.IsTypeBoolean())
                    .Config(ctx => ctx.HideAfterFinish(true))
                    .Run(ctx.CancellationToken);
                if (p.IsAborted)
                {
                    isCancellationRequested = true;
                    return Array.Empty<string>();
                }
                var result = new List<string>();
                foreach (var item in p.Value)
                {
                    if (item.Masked == "1" || item.Masked == "T")
                    {
                        result.Add("true");
                    }
                    else
                    {
                        result.Add("false");
                    }
                }
                return result.ToArray();
            }
            else
            {
                var p = PromptPlus
                    .MultiSelect<string>(promptText, description)
                    .AddItems(argument.AllowedValues)
                    .AddDefaults(lastvalue)
                    .Range(argument.Arity.Minimum, argument.Arity.Maximum)
                    .PageSize(pageSize)
                    .Config(ctx => ctx.HideAfterFinish(true))
                    .Run(ctx.CancellationToken);
                if (p.IsAborted)
                {
                    isCancellationRequested = true;
                    return Array.Empty<string>();
                }
                return p.Value.ToArray();
            }
        }

        public static ICollection<string> PromptAllowedOnlyValue(CommandContext ctx, int pageSize, string promptText, string description, IArgument argument,string[] lastvalue, out bool isCancellationRequested)
        {
            string defvalue = null;
            if (lastvalue is not null)
            {
                if (lastvalue.Length == 1)
                {
                    defvalue = lastvalue[0];
                }
            }
            isCancellationRequested = false;
            var p = PromptPlus
                .Select<string>(promptText, description)
                .AddItems(argument.AllowedValues)
                .Default(defvalue)
                .PageSize(pageSize)
                .Config(ctx => ctx.HideAfterFinish(true))
                .Run(ctx.CancellationToken);
            if (p.IsAborted)
            {
                isCancellationRequested = true;
                return Array.Empty<string>();
            }
            return new[] { p.Value };
        }

        public static string DefaultValueForType(object value)
        {
            if (value is null)
            {
                return null;
            }
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.Boolean:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return value.ToString();
                case TypeCode.DateTime:
                    return ((DateTime)value).ToString("D");
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
