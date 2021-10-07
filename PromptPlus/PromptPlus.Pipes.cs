// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static class Pipe
        {
            public static IFormPlusBase AnyKey()
            {
                return KeyPressForm(new KeyPressOptions { HideAfterFinish = true });
            }

            public static IFormPlusBase KeyPress(Action<KeyPressOptions> configure)
            {
                var options = new KeyPressOptions();
                configure(options);
                return KeyPressForm(options);
            }

            public static IFormPlusBase KeyPress(KeyPressOptions options)
            {
                return KeyPressForm(options);
            }

            public static IFormPlusBase KeyPress(string message, char? Keypress, ConsoleModifiers? keymodifiers = null)
            {
                return KeyPressForm(message, Keypress, keymodifiers);
            }

            public static IFormPlusBase MaskEdit(MaskedGenericType masktype, string message, string maskedvalue, string defaultValue = null, bool upperCase = true, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, maskedvalue, defaultValue, upperCase, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPlusBase MaskEdit(MaskedDateType masktype, string message, DateTime? defaultValue = null, CultureInfo cultureinfo = null, bool fillzeros = false, bool showtypeinput = true, FormatYear fyear = FormatYear.Y4, FormatTime ftime = FormatTime.HMS, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, defaultValue, cultureinfo, fillzeros, showtypeinput, fyear, ftime, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPlusBase MaskEdit(MaskedNumberType masktype, string message, int ammoutInteger, int ammoutDecimal, double? defaultValue = null, CultureInfo cultureinfo = null, MaskedSignal signal = MaskedSignal.None, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, ammoutInteger, ammoutDecimal, defaultValue, cultureinfo, signal, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPlusBase Input(InputOptions options)
            {
                return InputForm(options);
            }

            public static IFormPlusBase Input(Action<InputOptions> configure)
            {
                var options = new InputOptions();
                configure(options);
                return InputForm(options);
            }

            public static IFormPlusBase Input<T>(string message, string defaultValue = null, IList<Func<object, ValidationResult>> validators = null)
            {
                return InputForm(message, defaultValue, validators, false, false);
            }

            public static IFormPlusBase Password(string message, bool swithVisible = true, IList<Func<object, ValidationResult>> validators = null)
            {
                return InputForm(message, null, validators, true, swithVisible);
            }

            public static IFormPlusBase SliderNumber<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return SliderNumberForm(options);
            }

            public static IFormPlusBase NumberUpDown<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                options.Type = SliderNumberType.UpDown;
                return SliderNumberForm(options);
            }

            public static IFormPlusBase SliderNumber<T>(Action<SliderNumberOptions<T>> configure) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                var options = new SliderNumberOptions<T>();
                configure(options);
                return SliderNumber(options);
            }

            public static IFormPlusBase NumberUpDown<T>(Action<SliderNumberOptions<T>> configure) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                var options = new SliderNumberOptions<T>();
                configure(options);
                options.Type = SliderNumberType.UpDown;
                return SliderNumber(options);
            }

            public static IFormPlusBase SliderNumber<T>(string message, T value, T min, T max, T shortstep, T? largestep = null, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return SliderNumberForm(message, value, min, max, shortstep, largestep, fracionalDig);
            }

            public static IFormPlusBase NumberUpDown<T>(string message, T value, T min, T max, T step, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return NumberUpDownForm(message, value, min, max, step, fracionalDig);
            }

            public static IFormPlusBase SliderSwitche(SliderSwitcheOptions options)
            {
                return SliderSwitcheForm(options);
            }

            public static IFormPlusBase SliderSwitche(Action<SliderSwitcheOptions> configure)
            {
                var options = new SliderSwitcheOptions();
                configure(options);
                return SliderSwitche(options);
            }


            public static IFormPlusBase SliderSwitche(string message, bool value, string offvalue = null, string onvalue = null)
            {
                return SliderSwitcheForm(message, value, offvalue, onvalue);
            }

            public static IFormPlusBase Progressbar(ProgressBarOptions options)
            {
                return ProgressbarForm(options);
            }

            public static IFormPlusBase Progressbar(Action<ProgressBarOptions> configure)
            {
                var options = new ProgressBarOptions();
                configure(options);
                return Progressbar(options);
            }

            public static IFormPlusBase Progressbar(string title, Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> updateHandler, int width = ProgressgBarWitdth, object interationId = null)
            {
                return ProgressbarForm(title, interationId, updateHandler, width);
            }

            public static IFormPlusBase WaitProcess(string v, WaitProcessOptions options)
            {
                return WaitProcessForm(options);
            }

            public static IFormPlusBase WaitProcess(Action<WaitProcessOptions> configure)
            {
                var options = new WaitProcessOptions();
                configure(options);
                return WaitProcessForm(options);
            }

            public static IFormPlusBase WaitProcess(string title, SingleProcess process)
            {
                return WaitProcessForm(
                    title,
                    new List<SingleProcess>()
                    {
                        process
                    });
            }

            public static IFormPlusBase WaitProcess(string title, IEnumerable<SingleProcess> process)
            {
                return WaitProcessForm(title, process);
            }

            public static IFormPlusBase Confirm(ConfirmOptions options)
            {
                return ConfirmForm(options);
            }

            public static IFormPlusBase Confirm(Action<ConfirmOptions> configure)
            {
                var options = new ConfirmOptions();
                configure(options);
                return Confirm(options);
            }

            public static IFormPlusBase Confirm(string message, bool? defaultValue = null)
            {
                return ConfirmForm(message, defaultValue);
            }

            public static IFormPlusBase Select<T>(SelectOptions<T> options, CancellationToken? cancellationToken = null)
            {
                return SelectForm(options);
            }

            public static IFormPlusBase Select<T>(Action<SelectOptions<T>> configure)
            {
                var options = new SelectOptions<T>();
                configure(options);
                return Select(options);
            }

            public static IFormPlusBase Select<T>(string message, T? defaultValue = null, int? pageSize = null) where T : struct, Enum
            {
                return SelectForm(message, defaultValue, pageSize);
            }

            public static IFormPlusBase Select<T>(string message, IEnumerable<T> items, object defaultValue = null, int? pageSize = null, Func<T, string> textSelector = null)
            {
                return SelectForm(message, items, defaultValue, pageSize, textSelector);
            }

            public static IFormPlusBase MultiSelect<T>(MultiSelectOptions<T> options)
            {
                return MultiSelectForm(options);
            }

            public static IFormPlusBase MultiSelect<T>(Action<MultiSelectOptions<T>> configure)
            {
                var options = new MultiSelectOptions<T>();
                configure(options);
                return MultiSelect(options);
            }

            public static IFormPlusBase MultiSelect<T>(string message, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null) where T : struct, Enum
            {
                return MultiSelectForm(message, minimum, maximum, defaultValues, pageSize);
            }

            public static IFormPlusBase MultiSelect<T>(string message, IEnumerable<T> items, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, Func<T, string> textSelector = null)
            {
                return MultiSelectForm(message, items, minimum, maximum, defaultValues, pageSize, textSelector);
            }

            public static IFormPlusBase List<T>(ListOptions<T> options, CancellationToken? cancellationToken = null)
            {
                return ListForm(options);
            }

            public static IFormPlusBase List<T>(Action<ListOptions<T>> configure)
            {
                var options = new ListOptions<T>();
                configure(options);
                return List(options);
            }

            public static IFormPlusBase List<T>(string message, int minimum = 1, int maximum = -1, int? pageSize = null, bool uppercase = false, bool allowduplicate = true, IList<Func<object, ValidationResult>> validators = null)
            {
                return ListForm<T>(message, minimum, maximum, pageSize, uppercase, allowduplicate, validators);
            }

            public static IFormPlusBase ListMasked<T>(string message, string maskValue, int minimum = 1, int maximum = -1, bool uppercase = false, bool showInputType = true, bool fillzeros = false, CultureInfo? cultureInfo = null, int? pageSize = null, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return ListMaskedForm<T>(message, maskValue, minimum, maximum, uppercase, showInputType, fillzeros, cultureInfo, pageSize, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPlusBase Browser(BrowserOptions options)
            {
                return BrowserForm(options);
            }

            public static IFormPlusBase Browser(Action<BrowserOptions> configure)
            {
                var options = new BrowserOptions();
                configure(options);
                return Browser(options);
            }

            public static IFormPlusBase Browser(BrowserFilter fileBrowserChoose, string message, string defaultValue = null, string prefixExtension = null, bool allowNotSelected = false, string rootFolder = null, string searchPattern = null, int? pageSize = null, bool supressHidden = true, bool promptCurrentPath = true, bool promptSearchPattern = true)
            {
                return BrowserForm(fileBrowserChoose, message, defaultValue, prefixExtension, allowNotSelected, rootFolder, searchPattern, pageSize, supressHidden, promptCurrentPath, promptSearchPattern);
            }
        }
    }
}
