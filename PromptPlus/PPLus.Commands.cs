// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PromptPlus.Forms;
using PromptPlus.Internal;
using PromptPlus.Options;
using PromptPlus.Resources;
using PromptPlus.ValueObjects;

namespace PromptPlus
{
    public static partial class PPlus
    {
        private static readonly char? s_defaultfill = '0';

        public static ResultPPlus<IEnumerable<ResultPipe>> Pipeline(IList<IFormPPlusBase> steps, CancellationToken? cancellationToken = null)
        {
            foreach (var item in steps)
            {
                if (string.IsNullOrEmpty(item.PipeId))
                {
                    throw new ArgumentException(Exceptions.EX_PipeLineEmptyId);
                }
            }
            using var pipeline = new PipeLineForms(steps);
            return pipeline.Start(cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<bool> AnyKey(CancellationToken? cancellationToken = null)
        {
            using var form = KeyPressForm(new KeyPressOptions { });
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<bool> KeyPress(Action<KeyPressOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new KeyPressOptions();
            configure(options);
            return KeyPress(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<bool> KeyPress(KeyPressOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = KeyPressForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static keyPressForm KeyPressForm(KeyPressOptions options)
        {
            return new keyPressForm(options);
        }

        public static ResultPPlus<bool> KeyPress(string message, char? Keypress, ConsoleModifiers? keymodifiers = null, CancellationToken? cancellationToken = null)
        {
            using var form = KeyPressForm(message, Keypress, keymodifiers);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static keyPressForm KeyPressForm(string message, char? Keypress, ConsoleModifiers? keymodifiers = null)
        {
            var options = new KeyPressOptions
            {
                KeyPress = Keypress,
                Message = message,
                KeyModifiers = keymodifiers
            };
            return KeyPressForm(options);
        }

        internal static ResultPPlus<ResultMasked> MaskEdit(MaskedOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = MaskEditForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MaskedInputForm MaskEditForm(MaskedOptions options)
        {
            return new MaskedInputForm(options);
        }

        public static ResultPPlus<ResultMasked> MaskEdit(MaskedGenericType masktype, string message, string maskedvalue, string defaultValue = null, bool upperCase = true, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
        {
            using var form = MaskEditForm(masktype, message, maskedvalue, defaultValue, upperCase, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MaskedInputForm MaskEditForm(MaskedGenericType masktype, string message, string maskedvalue, string defaultValue, bool upperCase, bool showtypeinput, IList<Func<object, ValidationResult>> validators, bool enabledPromptTooltip, bool enabledAbortKey, bool enabledAbortAllPipes, bool hideAfterFinish)
        {
            var options = new MaskedOptions
            {
                Type = masktype.SeletedOption,
                FmtYear = FormatYear.Y4,
                FmtTime = FormatTime.HMS,
                AcceptSignal = MaskedSignal.None,
                CurrentCulture = DefaultCulture,
                ShowInputType = showtypeinput,
                Message = message,
                MaskValue = maskedvalue,
                DefaultValueWitdhoutMask = defaultValue,
                UpperCase = upperCase,
                EnabledAbortAllPipes = enabledAbortAllPipes,
                EnabledPromptTooltip = enabledPromptTooltip,
                EnabledAbortKey = enabledAbortKey,
                HideAfterFinish = hideAfterFinish
            };

            if (validators != null)
            {
                options.Validators.Merge(validators);
            }

            return MaskEditForm(options);
        }

        public static ResultPPlus<ResultMasked> MaskEdit(MaskedDateType masktype, string message, DateTime? defaultValue = null, CultureInfo cultureinfo = null, bool fillzeros = false, bool showtypeinput = true, FormatYear fyear = FormatYear.Y4, FormatTime ftime = FormatTime.HMS, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
        {
            using var form = MaskEditForm(masktype, message, defaultValue, cultureinfo, fillzeros, showtypeinput, fyear, ftime, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MaskedInputForm MaskEditForm(MaskedDateType masktype, string message, DateTime? defaultValue, CultureInfo? cultureinfo, bool fillzeros, bool showtypeinput, FormatYear fyear, FormatTime ftime, IList<Func<object, ValidationResult>> validators, bool enabledPromptTooltip, bool enabledAbortKey, bool enabledAbortAllPipes, bool hideAfterFinish)
        {
            var paramAM = DefaultCulture.DateTimeFormat.AMDesignator;
            if (cultureinfo == null)
            {
                cultureinfo = DefaultCulture;
            }

            string defaultdateValue = null;
            var stddtfmt = cultureinfo.DateTimeFormat.ShortDatePattern.ToUpper().Split(cultureinfo.DateTimeFormat.DateSeparator[0]);
            var yearlen = "4";
            if (fyear == FormatYear.Y2)
            {
                yearlen = "2";
            }
            var fmtdate = $"{yearlen}:{stddtfmt[0][0]}{stddtfmt[1][0]}{stddtfmt[2][0]}";
            if (defaultValue.HasValue)
            {
                defaultdateValue = defaultValue.Value.ToString(cultureinfo.DateTimeFormat.UniversalSortableDateTimePattern);
                var dtstring = defaultdateValue.Substring(0, defaultdateValue.IndexOf(' '));
                switch (fyear)
                {
                    case FormatYear.Y4:
                        break;
                    case FormatYear.Y2:
                        dtstring = dtstring.Substring(2);
                        break;
                    default:
                        break;
                }
                var dtelem = dtstring.Split('-');
                for (var i = 0; i < stddtfmt.Length; i++)
                {
                    if (stddtfmt[i][0] == 'D')
                    {
                        stddtfmt[i] = dtelem[2];
                    }
                    else if (stddtfmt[i][0] == 'M')
                    {
                        stddtfmt[i] = dtelem[1];
                    }
                    else if (stddtfmt[i][0] == 'Y')
                    {
                        stddtfmt[i] = dtelem[0];
                    }
                }
                dtstring = $"{stddtfmt[0]}{cultureinfo.DateTimeFormat.DateSeparator}{stddtfmt[1]}{cultureinfo.DateTimeFormat.DateSeparator}{stddtfmt[2]}";
                var tmstring = defaultdateValue.Substring(defaultdateValue.IndexOf(' ') + 1);
                tmstring = tmstring.Replace("Z", "");
                var tmelem = tmstring.Split(':');
                var hr = int.Parse(tmstring.Substring(0, 2));
                string tmsignal;
                if (hr > 12)
                {
                    tmsignal = cultureinfo.DateTimeFormat.PMDesignator.ToUpper()[0].ToString();
                }
                else
                {
                    tmsignal = cultureinfo.DateTimeFormat.AMDesignator.ToUpper()[0].ToString();
                }
                if (string.IsNullOrEmpty(paramAM) && !string.IsNullOrEmpty(cultureinfo.DateTimeFormat.AMDesignator))
                {
                    hr -= 12;
                    tmstring = $"{hr.ToString().PadLeft(2, '0')}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}";
                }
                else if (!string.IsNullOrEmpty(paramAM) && string.IsNullOrEmpty(cultureinfo.DateTimeFormat.AMDesignator))
                {
                    hr += 12;
                    tmstring = $"{hr.ToString().PadLeft(2, '0')}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}";
                }
                else
                {
                    tmstring = $"{tmelem[0]}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}{cultureinfo.DateTimeFormat.TimeSeparator}{tmelem[1]}";
                }
                switch (masktype.Option)
                {
                    case MaskedOptionDateType.DateOnly:
                        defaultdateValue = dtstring;
                        break;
                    case MaskedOptionDateType.TimeOnly:
                        defaultdateValue = $"{tmstring}{tmsignal}";
                        break;
                    case MaskedOptionDateType.DateTime:
                        defaultdateValue = $"{dtstring}{tmstring}{tmsignal}";
                        break;
                }
                defaultdateValue = defaultdateValue.Replace(cultureinfo.DateTimeFormat.DateSeparator, "");
                defaultdateValue = defaultdateValue.Replace(cultureinfo.DateTimeFormat.TimeSeparator, "");
            }

            var maskvalue = masktype.SeletedOption switch
            {
                MaskedType.Generic or MaskedType.Number or MaskedType.Currency => throw new ArgumentException(masktype.SeletedOption.ToString()),
                MaskedType.DateOnly => CreateMaskedOnlyDate(fyear, cultureinfo),
                MaskedType.TimeOnly => CreateMaskedOnlyTime(ftime, cultureinfo),
                MaskedType.DateTime => CreateMaskedOnlyDateTime(fyear, ftime, cultureinfo),
                _ => throw new ArgumentException(masktype.SeletedOption.ToString()),
            };

            var options = new MaskedOptions
            {
                DateFmt = fmtdate,
                Type = masktype.SeletedOption,
                FmtYear = fyear,
                FmtTime = ftime,
                AcceptSignal = MaskedSignal.None,
                CurrentCulture = cultureinfo,
                ShowInputType = showtypeinput,
                Message = message,
                MaskValue = maskvalue,
                FillNumber = fillzeros ? s_defaultfill : null,
                DefaultValueWitdhoutMask = defaultdateValue,
                EnabledAbortAllPipes = enabledAbortAllPipes,
                EnabledPromptTooltip = enabledPromptTooltip,
                EnabledAbortKey = enabledAbortKey,
                HideAfterFinish = hideAfterFinish
            };
            options.Validators.Add(Validators.IsDateTime(cultureinfo, Messages.Invalid));

            if (validators != null)
            {
                options.Validators.Merge(validators);
            }

            return MaskEditForm(options);
        }

        public static ResultPPlus<ResultMasked> MaskEdit(MaskedNumberType masktype, string message, int ammoutInteger, int ammoutDecimal, double? defaultValue = null, CultureInfo cultureinfo = null, MaskedSignal signal = MaskedSignal.None, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
        {
            using var form = MaskEditForm(masktype, message, ammoutInteger, ammoutDecimal, defaultValue, cultureinfo, signal, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MaskedInputForm MaskEditForm(MaskedNumberType masktype, string message, int ammoutInteger, int ammoutDecimal, double? defaultValue, CultureInfo cultureinfo, MaskedSignal signal, bool showtypeinput, IList<Func<object, ValidationResult>> validators, bool enabledPromptTooltip, bool enabledAbortKey, bool enabledAbortAllPipes, bool hideAfterFinish)
        {
            string defaultnumValue = null;
            if (defaultValue.HasValue)
            {
                var sep = DefaultCulture.NumberFormat.NumberDecimalSeparator[0];
                if (masktype.Option == MaskedOptionNumberType.Currency)
                {
                    sep = DefaultCulture.NumberFormat.CurrencyDecimalSeparator[0];
                }
                var aux = defaultValue.ToString();
                var pos = aux.IndexOf(sep);
                string intvalue;
                var decvalue = new string('0', ammoutDecimal);
                if (pos >= 0)
                {
                    intvalue = aux.Substring(0, pos).PadLeft(ammoutInteger, '0');
                    decvalue = aux.Substring(pos + 1).PadRight(ammoutDecimal, '0');
                }
                else
                {
                    intvalue = aux.PadLeft(ammoutInteger, '0');
                }
                var defsignal = "";
                if (signal == MaskedSignal.Enabled && defaultValue < 0)
                {
                    defsignal = "-";
                }
                if (signal == MaskedSignal.Enabled && defaultValue > 0)
                {
                    defsignal = "+";
                }
                if (ammoutInteger == 0)
                {
                    defaultnumValue = $"{decvalue}{defsignal}";
                }
                else
                {
                    defaultnumValue = $"{intvalue}{decvalue}{defsignal}";
                }
            }
            if (cultureinfo == null)
            {
                cultureinfo = DefaultCulture;
            }

            var maskvalue = masktype.SeletedOption switch
            {
                MaskedType.Generic or MaskedType.DateOnly or MaskedType.TimeOnly or MaskedType.DateTime => throw new ArgumentException(masktype.SeletedOption.ToString()),
                MaskedType.Number => CreateMaskedNumber(cultureinfo, ammoutInteger, ammoutDecimal),
                MaskedType.Currency => CreateMaskedCurrency(cultureinfo, ammoutInteger, ammoutDecimal),
                _ => throw new ArgumentException(masktype.SeletedOption.ToString()),
            };
            var options = new MaskedOptions
            {
                Type = masktype.SeletedOption,
                FmtYear = FormatYear.Y4,
                FmtTime = FormatTime.HMS,
                AcceptSignal = signal,
                CurrentCulture = cultureinfo,
                ShowInputType = showtypeinput,
                Message = message,
                MaskValue = maskvalue,
                FillNumber = s_defaultfill,
                OnlyDecimal = ammoutInteger == 0,
                DefaultValueWitdhoutMask = defaultnumValue,
                EnabledAbortAllPipes = enabledAbortAllPipes,
                EnabledPromptTooltip = enabledPromptTooltip,
                EnabledAbortKey = enabledAbortKey,
                HideAfterFinish = hideAfterFinish
            };
            options.Validators.Add(Validators.IsNumber(masktype.Option, cultureinfo, Messages.Invalid));
            if (validators != null)
            {
                options.Validators.Merge(validators);
            }

            return MaskEditForm(options);
        }

        public static MaskedDateType MaskTypeDateOnly => new() { Option = MaskedOptionDateType.DateOnly };

        public static MaskedDateType MaskTypeDateTime => new() { Option = MaskedOptionDateType.DateTime };

        public static MaskedDateType MaskTypeTimeOnly => new() { Option = MaskedOptionDateType.TimeOnly };

        public static MaskedNumberType MaskTypeNumber => new() { Option = MaskedOptionNumberType.Number };

        public static MaskedNumberType MaskTypeCurrency => new() { Option = MaskedOptionNumberType.Currency };

        public static MaskedGenericType MaskTypeGeneric => new();

        public static ResultPPlus<T> Input<T>(InputOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = InputForm<T>(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static InputForm<T> InputForm<T>(InputOptions options)
        {
            return new InputForm<T>(options);
        }

        public static ResultPPlus<T> Input<T>(Action<InputOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new InputOptions();
            configure(options);
            return Input<T>(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<T> Input<T>(string message, object defaultValue = null, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
        {
            using var form = InputForm<T>(message, defaultValue, validators, false, false);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<string> Password(string message, bool swithVisible = true, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
        {
            var options = new InputOptions
            {
                Message = message,
                IsPassword = true,
                SwithVisiblePassword = swithVisible
            };

            if (validators != null)
            {
                options.Validators.Merge(validators);
            }

            return Input<string>(options, cancellationToken ?? CancellationToken.None);
        }

        internal static InputForm<T> InputForm<T>(string message, object defaultValue = null, IList<Func<object, ValidationResult>> validators = null, bool ispassword = false, bool swithVisible = true)
        {
            var options = new InputOptions
            {
                Message = message,
                DefaultValue = defaultValue,
                IsPassword = ispassword,
                SwithVisiblePassword = swithVisible
            };

            if (validators != null)
            {
                options.Validators.Merge(validators);
            }
            return InputForm<T>(options);
        }

        public static ResultPPlus<T> SliderNumber<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            using var form = SliderNumberForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SliderNumberForm<T> SliderNumberForm<T>(SliderNumberOptions<T> options) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            return new SliderNumberForm<T>(options);
        }

        public static ResultPPlus<T> SliderNumber<T>(Action<SliderNumberOptions<T>> configure, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var options = new SliderNumberOptions<T>();

            configure(options);

            return SliderNumber(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<T> SliderNumber<T>(string message, T value, T min, T max, T shortstep, T? largestep = null, int fracionalDig = 0, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            T large;
            if (largestep == null)
            {
                large = (T)Convert.ChangeType(int.Parse(max.ToString()) / 10, typeof(T));
            }
            else
            {
                large = largestep.Value;
            }
            using var form = SliderNumberForm(message, value, min, max, shortstep, large, fracionalDig);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SliderNumberForm<T> SliderNumberForm<T>(string message, T value, T min, T max, T shortstep, T? largestep = null, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var options = new SliderNumberOptions<T>
            {
                FracionalDig = fracionalDig,
                LargeStep = largestep,
                Max = max,
                Message = message,
                Min = min,
                ShortStep = shortstep,
                Value = value
            };
            return SliderNumberForm(options);
        }

        public static ResultPPlus<T> NumberUpDown<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            options.Type = SliderNumberType.UpDown;
            using var form = SliderNumberForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<T> NumberUpDown<T>(Action<SliderNumberOptions<T>> configure, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var options = new SliderNumberOptions<T>();
            configure(options);
            options.Type = SliderNumberType.UpDown;
            return SliderNumber(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<T> NumberUpDown<T>(string message, T value, T min, T max, T step, int fracionalDig = 0, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            using var form = NumberUpDownForm(message, value, min, max, step, fracionalDig);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SliderNumberForm<T> NumberUpDownForm<T>(string message, T value, T min, T max, T step, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var options = new SliderNumberOptions<T>
            {
                Type = SliderNumberType.UpDown,
                FracionalDig = fracionalDig,
                LargeStep = step,
                Max = max,
                Message = message,
                Min = min,
                ShortStep = step,
                Value = value
            };
            return SliderNumberForm(options);
        }

        public static ResultPPlus<bool> SliderSwitche(SliderSwitcheOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = SliderSwitcheForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SliderSwitcheForm SliderSwitcheForm(SliderSwitcheOptions options)
        {
            return new SliderSwitcheForm(options);
        }

        public static ResultPPlus<bool> SliderSwitche(Action<SliderSwitcheOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new SliderSwitcheOptions();

            configure(options);

            return SliderSwitche(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<bool> SliderSwitche(string message, bool value, string offvalue = null, string onvalue = null, CancellationToken? cancellationToken = null)
        {
            using var form = SliderSwitcheForm(message, value, offvalue, onvalue);
            return form.Start(cancellationToken ?? CancellationToken.None);

        }

        internal static SliderSwitcheForm SliderSwitcheForm(string message, bool value, string offvalue, string onvalue)
        {
            var options = new SliderSwitcheOptions
            {
                Message = message,
                Value = value,
                OffValue = offvalue ?? Messages.OffValue,
                OnValue = onvalue ?? Messages.OnValue
            };
            return SliderSwitcheForm(options);
        }

        public static ResultPPlus<ProgressBarInfo> Progressbar(ProgressBarOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = ProgressbarForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static ProgressBarForm ProgressbarForm(ProgressBarOptions options)
        {
            return new ProgressBarForm(options);
        }

        public static ResultPPlus<ProgressBarInfo> Progressbar(Action<ProgressBarOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new ProgressBarOptions();

            configure(options);

            return Progressbar(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<ProgressBarInfo> Progressbar(string title, Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> updateHandler, int width = ProgressgBarWitdth, object interationId = null, CancellationToken? cancellationToken = null)
        {
            using var form = ProgressbarForm(title, interationId, updateHandler, width);
            return form.Start(cancellationToken ?? CancellationToken.None);

        }

        internal static ProgressBarForm ProgressbarForm(string title, object interationId, Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> updateHandler, int width)
        {
            var options = new ProgressBarOptions
            {
                Message = title,
                InterationId = interationId ?? 0,
                UpdateHandler = updateHandler,
                Witdth = width < ProgressgBarWitdth ? ProgressgBarWitdth : (width > 100 ? 100 : width)
            };
            return ProgressbarForm(options);
        }

        public static ResultPPlus<IEnumerable<T>> WaitProcess<T>(WaitProcessOptions<T> options, CancellationToken? cancellationToken = null)
        {
            using var form = WaitProcessForm(options);
            var res = form.Start(cancellationToken ?? CancellationToken.None);
            if (res.IsAborted)
            {
                return new ResultPPlus<IEnumerable<T>>(default, true);
            }
            return new ResultPPlus<IEnumerable<T>>(res.Value, res.IsAborted);
        }

        internal static WaitProcessForm<T> WaitProcessForm<T>(WaitProcessOptions<T> options)
        {
            return new WaitProcessForm<T>(options);
        }

        public static ResultPPlus<IEnumerable<T>> WaitProcess<T>(Action<WaitProcessOptions<T>> configure, CancellationToken? cancellationToken = null)
        {
            var options = new WaitProcessOptions<T>();

            configure(options);

            return WaitProcess(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<IEnumerable<T>> WaitProcess<T>(string title, Func<Task<T>> process, Func<T, string> processTextResult = null, CancellationToken? cancellationToken = null)
        {
            return WaitProcess(
                title,
                new List<SingleProcess<T>>()
                {
                     new SingleProcess<T>{ ProcessToRun = process }
                },
                processTextResult,
                cancellationToken);
        }

        public static ResultPPlus<IEnumerable<T>> WaitProcess<T>(string title, IEnumerable<SingleProcess<T>> process, Func<T, string> processTextResult = null, CancellationToken? cancellationToken = null)
        {
            using var form = WaitProcessForm(title, process, processTextResult);
            var res = form.Start(cancellationToken ?? CancellationToken.None);
            if (res.IsAborted)
            {
                return new ResultPPlus<IEnumerable<T>>(default, true);
            }
            return new ResultPPlus<IEnumerable<T>>(res.Value, res.IsAborted);
        }

        internal static WaitProcessForm<T> WaitProcessForm<T>(string title, IEnumerable<SingleProcess<T>> process, Func<T, string> processTextResult = null)
        {
            Func<T, string> aux = x => x?.ToString();
            var options = new WaitProcessOptions<T>
            {
                Message = title,
                Process = process,
                ProcessTextResult = processTextResult ?? aux
            };
            return WaitProcessForm(options);
        }

        public static ResultPPlus<bool> Confirm(ConfirmOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = ConfirmForm(options);

            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static ConfirmForm ConfirmForm(ConfirmOptions options)
        {
            return new ConfirmForm(options);
        }

        public static ResultPPlus<bool> Confirm(Action<ConfirmOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new ConfirmOptions();
            configure(options);
            return Confirm(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<bool> Confirm(string message, bool? defaultValue = null, CancellationToken? cancellationToken = null)
        {
            using var form = ConfirmForm(message, defaultValue);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static ConfirmForm ConfirmForm(string message, bool? defaultValue = null)
        {
            var options = new ConfirmOptions
            {
                Message = message,
                DefaultValue = defaultValue
            };

            return ConfirmForm(options);
        }

        public static ResultPPlus<T> Select<T>(SelectOptions<T> options, CancellationToken? cancellationToken = null)
        {
            using var form = SelectForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SelectForm<T> SelectForm<T>(SelectOptions<T> options)
        {
            return new SelectForm<T>(options);
        }

        public static ResultPPlus<T> Select<T>(Action<SelectOptions<T>> configure, CancellationToken? cancellationToken = null)
        {
            var options = new SelectOptions<T>();
            configure(options);
            return Select(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<EnumValue<T>> Select<T>(string message, T? defaultValue = null, int? pageSize = null, CancellationToken? cancellationToken = null) where T : struct, Enum
        {
            using var form = SelectForm(message, defaultValue, pageSize);
            var aux = form.Start(cancellationToken ?? CancellationToken.None);
            if (aux.IsAborted)
            {
                return new ResultPPlus<EnumValue<T>>(default, true);
            }
            return new ResultPPlus<EnumValue<T>>(aux.Value, aux.IsAborted);

        }

        internal static SelectForm<EnumValue<T>> SelectForm<T>(string message, T? defaultValue = null, int? pageSize = null) where T : struct, Enum
        {
            var items = EnumValue<T>.GetValues();
            var options = new SelectOptions<EnumValue<T>>
            {
                Message = message,
                Items = items,
                DefaultValue = (EnumValue<T>)defaultValue,
                PageSize = pageSize ?? items.Count(),
                TextSelector = x => x?.DisplayName
            };
            return SelectForm(options);
        }

        public static ResultPPlus<T> Select<T>(string message, IEnumerable<T> items, object defaultValue = null, int? pageSize = null, Func<T, string> textSelector = null, CancellationToken? cancellationToken = null)
        {
            using var form = SelectForm(message, items, defaultValue, pageSize, textSelector);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static SelectForm<T> SelectForm<T>(string message, IEnumerable<T> items, object defaultValue = null, int? pageSize = null, Func<T, string> textSelector = null)
        {
            var options = new SelectOptions<T>
            {
                Message = message,
                Items = items,
                DefaultValue = defaultValue,
                PageSize = pageSize ?? items.Count(),
                TextSelector = textSelector ?? (x => x?.ToString())
            };

            return SelectForm(options);
        }

        public static ResultPPlus<IEnumerable<T>> MultiSelect<T>(MultiSelectOptions<T> options, CancellationToken? cancellationToken = null)
        {
            using var form = MultiSelectForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MultiSelectForm<T> MultiSelectForm<T>(MultiSelectOptions<T> options)
        {
            return new MultiSelectForm<T>(options);
        }

        public static ResultPPlus<IEnumerable<T>> MultiSelect<T>(Action<MultiSelectOptions<T>> configure, CancellationToken? cancellationToken = null)
        {
            var options = new MultiSelectOptions<T>();
            configure(options);
            return MultiSelect(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<IEnumerable<EnumValue<T>>> MultiSelect<T>(string message, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, CancellationToken? cancellationToken = null) where T : struct, Enum
        {
            using var form = MultiSelectForm(message, minimum, maximum, defaultValues, pageSize);
            var aux = form.Start(cancellationToken ?? CancellationToken.None);
            if (aux.IsAborted)
            {
                return new ResultPPlus<IEnumerable<EnumValue<T>>>(default, true);
            }
            return new ResultPPlus<IEnumerable<EnumValue<T>>>(aux.Value, aux.IsAborted);

        }

        internal static MultiSelectForm<EnumValue<T>> MultiSelectForm<T>(string message, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null) where T : struct, Enum
        {
            var items = EnumValue<T>.GetValues();

            var options = new MultiSelectOptions<EnumValue<T>>
            {
                Message = message,
                Items = items,
                DefaultValues = defaultValues?.Select(x => (EnumValue<T>)x),
                PageSize = pageSize ?? items.Count(),
                Minimum = minimum < 0 ? 0 : minimum,
                Maximum = maximum < 0 ? int.MaxValue : maximum,
                TextSelector = x => x.DisplayName
            };
            return MultiSelectForm(options);
        }

        public static ResultPPlus<IEnumerable<T>> MultiSelect<T>(string message, IEnumerable<T> items, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, Func<T, string> textSelector = null, CancellationToken? cancellationToken = null)
        {
            using var form = MultiSelectForm(message, items, minimum, maximum, defaultValues, pageSize, textSelector);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MultiSelectForm<T> MultiSelectForm<T>(string message, IEnumerable<T> items, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, Func<T, string> textSelector = null)
        {
            var options = new MultiSelectOptions<T>
            {
                Message = message,
                Items = items,
                DefaultValues = defaultValues,
                PageSize = pageSize ?? items.Count(),
                Minimum = minimum < 0 ? 0 : minimum,
                Maximum = maximum < 0 ? int.MaxValue : maximum,
                TextSelector = textSelector ?? (x => x?.ToString())
            };

            return MultiSelectForm(options);
        }

        public static ResultPPlus<IEnumerable<T>> List<T>(ListOptions<T> options, CancellationToken? cancellationToken = null)
        {
            using var form = ListForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static ListForm<T> ListForm<T>(ListOptions<T> options)
        {
            return new ListForm<T>(options);
        }

        public static ResultPPlus<IEnumerable<T>> List<T>(Action<ListOptions<T>> configure, CancellationToken? cancellationToken = null)
        {
            var options = new ListOptions<T>();
            configure(options);
            return List(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<IEnumerable<T>> List<T>(string message, int minimum = 0, int maximum = int.MaxValue, int? pageSize = null, bool uppercase = false, bool allowduplicate = true, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
        {
            using var form = ListForm<T>(message, minimum, maximum, pageSize, uppercase, allowduplicate, validators);
            return form.Start(cancellationToken ?? CancellationToken.None);

        }

        internal static ListForm<T> ListForm<T>(string message, int minimum, int maximum, int? pageSize, bool uppercase, bool allowduplicate, IList<Func<object, ValidationResult>> validators)
        {
            var options = new ListOptions<T>
            {
                Message = message,
                Minimum = minimum < 0 ? 0 : minimum,
                Maximum = maximum < 0 ? int.MaxValue : maximum,
                PageSize = pageSize ?? int.MaxValue,
                UpperCase = uppercase,
                AllowDuplicate = allowduplicate
            };

            if (validators != null)
            {
                options.Validators.Merge(validators);

            }
            return ListForm(options);
        }

        public static ResultPPlus<IEnumerable<T>> ListMasked<T>(string message, string maskValue, int minimum = 0, int maximum = int.MaxValue, bool uppercase = false, bool showInputType = true, bool fillzeros = false, CultureInfo? cultureInfo = null, int? pageSize = null, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
        {
            using var form = ListMaskedForm<T>(message, maskValue, minimum, maximum, uppercase, showInputType, fillzeros, cultureInfo, pageSize, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static MaskedListForm<T> ListMaskedForm<T>(string message, string maskValue, int minimum, int maximum, bool uppercase, bool showInputType, bool fillzeros, CultureInfo? cultureInfo, int? pageSize, IList<Func<object, ValidationResult>> validators, bool enabledPromptTooltip, bool enabledAbortKey, bool enabledAbortAllPipes, bool hideAfterFinish)
        {
            var localopc = new MaskedOptions
            {
                Type = MaskedType.Generic,
                FmtYear = FormatYear.Y4,
                FmtTime = FormatTime.HMS,
                AcceptSignal = MaskedSignal.None,
                CurrentCulture = cultureInfo ?? DefaultCulture,
                ShowInputType = showInputType,
                Message = message,
                MaskValue = maskValue,
                DefaultValueWitdhoutMask = null,
                FillNumber = fillzeros ? s_defaultfill : null,
                UpperCase = uppercase,
                EnabledAbortAllPipes = enabledAbortAllPipes,
                EnabledPromptTooltip = enabledPromptTooltip,
                EnabledAbortKey = enabledAbortKey,
                HideAfterFinish = hideAfterFinish
            };
            var options = new ListOptions<T>
            {
                Message = message,
                Minimum = minimum < 0 ? 0 : minimum,
                Maximum = maximum < 0 ? int.MaxValue : maximum,
                PageSize = pageSize ?? int.MaxValue,
                MaskedOption = localopc,
                UpperCase = uppercase
            };

            if (validators != null)
            {
                options.Validators.Merge(validators);

            }
            return new MaskedListForm<T>(options);
        }

        public static ResultPPlus<ResultBrowser> Browser(BrowserOptions options, CancellationToken? cancellationToken = null)
        {
            using var form = BrowserForm(options);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static BrowserForm BrowserForm(BrowserOptions options)
        {
            return new BrowserForm(options);
        }

        public static ResultPPlus<ResultBrowser> Browser(Action<BrowserOptions> configure, CancellationToken? cancellationToken = null)
        {
            var options = new BrowserOptions();
            configure(options);
            return Browser(options, cancellationToken ?? CancellationToken.None);
        }

        public static ResultPPlus<ResultBrowser> Browser(BrowserFilter fileBrowserChoose, string message, string defaultValue = null, string prefixExtension = null, bool allowNotSelected = false, string rootFolder = null, string searchPattern = null, int? pageSize = null, bool supressHidden = true, bool promptCurrentPath = true, bool promptSearchPattern = true, CancellationToken? cancellationToken = null)
        {
            using var form = BrowserForm(fileBrowserChoose, message, defaultValue, prefixExtension, allowNotSelected, rootFolder, searchPattern, pageSize, supressHidden, promptCurrentPath, promptSearchPattern);
            return form.Start(cancellationToken ?? CancellationToken.None);
        }

        internal static BrowserForm BrowserForm(BrowserFilter fileBrowserChoose, string message, string defaultValue = null, string prefixExtension = null, bool allowNotSelected = false, string rootFolder = null, string searchPattern = null, int? pageSize = null, bool supressHidden = true, bool promptCurrentPath = true, bool promptSearchPattern = true)
        {
            var options = new BrowserOptions
            {
                AllowNotSelected = allowNotSelected,
                Filter = fileBrowserChoose,
                DefaultValue = defaultValue,
                PageSize = pageSize ?? int.MaxValue,
                PrefixExtension = prefixExtension,
                ShowNavigationCurrentPath = promptCurrentPath,
                ShowSearchPattern = promptSearchPattern,
                RootFolder = rootFolder,
                SearchPattern = searchPattern,
                SupressHidden = supressHidden,
                Message = message
            };
            return BrowserForm(options);
        }

        private static string CreateMaskedOnlyDate(FormatYear dateplaceholder, CultureInfo cultureInfo)
        {
            return dateplaceholder switch
            {
                FormatYear.Y2 => $"99\\{cultureInfo.DateTimeFormat.DateSeparator}99\\{cultureInfo.DateTimeFormat.DateSeparator}99",
                FormatYear.Y4 => $"99\\{cultureInfo.DateTimeFormat.DateSeparator}99\\{cultureInfo.DateTimeFormat.DateSeparator}9999",
                _ => throw new ArgumentException(dateplaceholder.ToString()),
            };
        }

        private static string CreateMaskedOnlyTime(FormatTime timePlaceholder, CultureInfo cultureInfo)
        {
            return timePlaceholder switch
            {
                FormatTime.HMS => $"99\\{cultureInfo.DateTimeFormat.TimeSeparator}99\\{cultureInfo.DateTimeFormat.TimeSeparator}99",
                FormatTime.OnlyHM => $"99\\{cultureInfo.DateTimeFormat.TimeSeparator}99\\{cultureInfo.DateTimeFormat.TimeSeparator}00",
                FormatTime.OnlyH => $"99\\{cultureInfo.DateTimeFormat.TimeSeparator}00\\{cultureInfo.DateTimeFormat.TimeSeparator}00",
                _ => throw new ArgumentException(timePlaceholder.ToString()),
            };
        }

        private static string CreateMaskedOnlyDateTime(FormatYear dateplaceholder, FormatTime timePlaceholder, CultureInfo cultureInfo)
        {
            return CreateMaskedOnlyDate(dateplaceholder, cultureInfo) + " " + CreateMaskedOnlyTime(timePlaceholder, cultureInfo);
        }

        private static string CreateMaskedNumber(CultureInfo cultureInfo, int integerplacehold, int decimalplacehold)
        {
            var topmask = string.Empty;
            if (integerplacehold % cultureInfo.NumberFormat.NumberGroupSizes[0] > 0)
            {
                topmask = new string('9', integerplacehold % cultureInfo.NumberFormat.NumberGroupSizes[0]);
            }
            else
            {
                if (integerplacehold == 0)
                {
                    topmask = "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < integerplacehold / cultureInfo.NumberFormat.NumberGroupSizes[0]; i++)
            {
                result += cultureInfo.NumberFormat.NumberGroupSeparator + new string('9', cultureInfo.NumberFormat.NumberGroupSizes[0]);
            }
            if (decimalplacehold > 0)
            {
                result += cultureInfo.NumberFormat.NumberDecimalSeparator + new string('9', decimalplacehold);
            }
            return result;
        }

        private static string CreateMaskedCurrency(CultureInfo cultureInfo, int integerplacehold, int decimalplacehold)
        {
            var csymb = cultureInfo.NumberFormat.CurrencySymbol.ToCharArray();
            var topmask = string.Empty;
            foreach (var item in csymb)
            {
                topmask += "\\" + item;
            }
            topmask += " ";

            if (integerplacehold % cultureInfo.NumberFormat.CurrencyGroupSizes[0] > 0)
            {
                topmask += new string('9', integerplacehold % cultureInfo.NumberFormat.CurrencyGroupSizes[0]);
            }
            else
            {
                if (integerplacehold == 0)
                {
                    topmask += "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < integerplacehold / cultureInfo.NumberFormat.CurrencyGroupSizes[0]; i++)
            {
                result += cultureInfo.NumberFormat.CurrencyGroupSeparator + new string('9', cultureInfo.NumberFormat.CurrencyGroupSizes[0]);
            }
            if (decimalplacehold > 0)
            {
                result += cultureInfo.NumberFormat.CurrencyDecimalSeparator + new string('9', decimalplacehold);
            }
            else
            {
                result += cultureInfo.NumberFormat.CurrencyDecimalSeparator + new string('0', cultureInfo.NumberFormat.CurrencyDecimalDigits);
            }
            return result;
        }
    }
}
