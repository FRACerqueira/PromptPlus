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

using PromptPlus.Internal;
using PromptPlus.Options;
using PromptPlus.ValueObjects;

namespace PromptPlus
{
    public static partial class PPlus
    {
        public static class Pipe
        {
            public static IFormPPlusBase AnyKey()
            {
                return KeyPressForm(new KeyPressOptions { HideAfterFinish = true });
            }

            public static IFormPPlusBase KeyPress(Action<KeyPressOptions> configure)
            {
                var options = new KeyPressOptions();
                configure(options);
                return KeyPressForm(options);
            }

            public static IFormPPlusBase KeyPress(KeyPressOptions options)
            {
                return KeyPressForm(options);
            }

            public static IFormPPlusBase KeyPress(string message, char? Keypress, ConsoleModifiers? keymodifiers = null)
            {
                return KeyPressForm(message, Keypress, keymodifiers);
            }

            public static IFormPPlusBase MaskEdit(MaskedGenericType masktype, string message, string maskedvalue, string defaultValue = null, bool upperCase = true, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, maskedvalue, defaultValue, upperCase, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPPlusBase MaskEdit(MaskedDateType masktype, string message, DateTime? defaultValue = null, CultureInfo cultureinfo = null, bool fillzeros = false, bool showtypeinput = true, FormatYear fyear = FormatYear.Y4, FormatTime ftime = FormatTime.HMS, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, defaultValue, cultureinfo, fillzeros, showtypeinput, fyear, ftime, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPPlusBase MaskEdit(MaskedNumberType masktype, string message, int ammoutInteger, int ammoutDecimal, double? defaultValue = null, CultureInfo cultureinfo = null, MaskedSignal signal = MaskedSignal.None, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return MaskEditForm(masktype, message, ammoutInteger, ammoutDecimal, defaultValue, cultureinfo, signal, showtypeinput, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPPlusBase Input<T>(InputOptions options)
            {
                return InputForm<T>(options);
            }

            public static IFormPPlusBase Input<T>(Action<InputOptions> configure)
            {
                var options = new InputOptions();
                configure(options);
                return InputForm<T>(options);
            }

            public static IFormPPlusBase Input<T>(string message, object defaultValue = null, IList<Func<object, ValidationResult>> validators = null)
            {
                return InputForm<T>(message, defaultValue, validators, false, false);
            }

            public static IFormPPlusBase Password(string message, bool swithVisible = true, IList<Func<object, ValidationResult>> validators = null)
            {
                return InputForm<string>(message, null, validators, true, swithVisible);
            }

            public static IFormPPlusBase SliderNumber<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return SliderNumberForm(options);
            }

            public static IFormPPlusBase NumberUpDown<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                options.Type = SliderNumberType.UpDown;
                return SliderNumberForm(options);
            }

            public static IFormPPlusBase SliderNumber<T>(Action<SliderNumberOptions<T>> configure) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                var options = new SliderNumberOptions<T>();
                configure(options);
                return SliderNumber(options);
            }

            public static IFormPPlusBase NumberUpDown<T>(Action<SliderNumberOptions<T>> configure) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                var options = new SliderNumberOptions<T>();
                configure(options);
                options.Type = SliderNumberType.UpDown;
                return SliderNumber(options);
            }

            public static IFormPPlusBase SliderNumber<T>(string message, T value, T min, T max, T shortstep, T? largestep = null, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return SliderNumberForm(message, value, min, max, shortstep, largestep, fracionalDig);
            }

            public static IFormPPlusBase NumberUpDown<T>(string message, T value, T min, T max, T step, int fracionalDig = 0) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
            {
                return NumberUpDownForm(message, value, min, max, step, fracionalDig);
            }

            public static IFormPPlusBase SliderSwitche(SliderSwitcheOptions options)
            {
                return SliderSwitcheForm(options);
            }

            public static IFormPPlusBase SliderSwitche(Action<SliderSwitcheOptions> configure)
            {
                var options = new SliderSwitcheOptions();
                configure(options);
                return SliderSwitche(options);
            }


            public static IFormPPlusBase SliderSwitche(string message, bool value, string offvalue = null, string onvalue = null)
            {
                return SliderSwitcheForm(message, value, offvalue, onvalue);
            }

            public static IFormPPlusBase Progressbar(ProgressBarOptions options)
            {
                return ProgressbarForm(options);
            }

            public static IFormPPlusBase Progressbar(Action<ProgressBarOptions> configure)
            {
                var options = new ProgressBarOptions();
                configure(options);
                return Progressbar(options);
            }

            public static IFormPPlusBase Progressbar(string title, Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> updateHandler, int width = ProgressgBarWitdth, object interationId = null)
            {
                return ProgressbarForm(title, interationId, updateHandler, width);
            }

            public static IFormPPlusBase WaitProcess<T>(WaitProcessOptions<T> options)
            {
                return WaitProcessForm(options);
            }

            public static IFormPPlusBase WaitProcess<T>(Action<WaitProcessOptions<T>> configure)
            {
                var options = new WaitProcessOptions<T>();
                configure(options);
                return WaitProcessForm(options);
            }

            public static IFormPPlusBase WaitProcess<T>(string title, Func<Task<T>> process, Func<T, string> processTextResult = null)
            {
                return WaitProcessForm(
                    title,
                    new List<SingleProcess<T>>()
                    {
                     new SingleProcess<T>{ ProcessToRun = process }
                    },
                    processTextResult);
            }

            public static IFormPPlusBase WaitProcess<T>(string title, IEnumerable<SingleProcess<T>> process, Func<T, string> processTextResult = null)
            {
                return WaitProcessForm(title, process, processTextResult);
            }

            public static IFormPPlusBase Confirm(ConfirmOptions options)
            {
                return ConfirmForm(options);
            }

            public static IFormPPlusBase Confirm(Action<ConfirmOptions> configure)
            {
                var options = new ConfirmOptions();
                configure(options);
                return Confirm(options);
            }

            public static IFormPPlusBase Confirm(string message, bool? defaultValue = null)
            {
                return ConfirmForm(message, defaultValue);
            }

            public static IFormPPlusBase Select<T>(SelectOptions<T> options, CancellationToken? cancellationToken = null)
            {
                return SelectForm(options);
            }

            public static IFormPPlusBase Select<T>(Action<SelectOptions<T>> configure)
            {
                var options = new SelectOptions<T>();
                configure(options);
                return Select(options);
            }

            public static IFormPPlusBase Select<T>(string message, T? defaultValue = null, int? pageSize = null) where T : struct, Enum
            {
                return SelectForm(message, defaultValue, pageSize);
            }

            public static IFormPPlusBase Select<T>(string message, IEnumerable<T> items, object defaultValue = null, int? pageSize = null, Func<T, string> textSelector = null)
            {
                return SelectForm(message, items, defaultValue, pageSize, textSelector);
            }

            public static IFormPPlusBase MultiSelect<T>(MultiSelectOptions<T> options)
            {
                return MultiSelectForm(options);
            }

            public static IFormPPlusBase MultiSelect<T>(Action<MultiSelectOptions<T>> configure)
            {
                var options = new MultiSelectOptions<T>();
                configure(options);
                return MultiSelect(options);
            }

            public static IFormPPlusBase MultiSelect<T>(string message, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null) where T : struct, Enum
            {
                return MultiSelectForm(message, minimum, maximum, defaultValues, pageSize);
            }

            public static IFormPPlusBase MultiSelect<T>(string message, IEnumerable<T> items, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, Func<T, string> textSelector = null)
            {
                return MultiSelectForm(message, items, minimum, maximum, defaultValues, pageSize, textSelector);
            }

            public static IFormPPlusBase List<T>(ListOptions<T> options, CancellationToken? cancellationToken = null)
            {
                return ListForm(options);
            }

            public static IFormPPlusBase List<T>(Action<ListOptions<T>> configure)
            {
                var options = new ListOptions<T>();
                configure(options);
                return List(options);
            }

            public static IFormPPlusBase List<T>(string message, int minimum = 1, int maximum = -1, int? pageSize = null, bool uppercase = false, bool allowduplicate = true, IList<Func<object, ValidationResult>> validators = null)
            {
                return ListForm<T>(message, minimum, maximum, pageSize, uppercase, allowduplicate, validators);
            }

            public static IFormPPlusBase ListMasked<T>(string message, string maskValue, int minimum = 1, int maximum = -1, bool uppercase = false, bool showInputType = true, bool fillzeros = false, CultureInfo? cultureInfo = null, int? pageSize = null, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
            {
                return ListMaskedForm<T>(message, maskValue, minimum, maximum, uppercase, showInputType, fillzeros, cultureInfo, pageSize, validators, enabledPromptTooltip, enabledAbortKey, enabledAbortAllPipes, hideAfterFinish);
            }

            public static IFormPPlusBase Browser(BrowserOptions options)
            {
                return BrowserForm(options);
            }

            public static IFormPPlusBase Browser(Action<BrowserOptions> configure)
            {
                var options = new BrowserOptions();
                configure(options);
                return Browser(options);
            }

            public static IFormPPlusBase Browser(BrowserFilter fileBrowserChoose, string message, string defaultValue = null, string prefixExtension = null, bool allowNotSelected = false, string rootFolder = null, string searchPattern = null, int? pageSize = null, bool supressHidden = true, bool promptCurrentPath = true, bool promptSearchPattern = true)
            {
                return BrowserForm(fileBrowserChoose, message, defaultValue, prefixExtension, allowNotSelected, rootFolder, searchPattern, pageSize, supressHidden, promptCurrentPath, promptSearchPattern);
            }
        }
    }
}
