// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

using PromptPlusControls.Controls;
using PromptPlusControls.FIGlet;
using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
      
        public static IFIGlet Banner(string value)
        {
            return new BannerControl(value);
        }

        public static IControlKeyPress KeyPress(char? Keypress = null, ConsoleModifiers? keymodifiers = null)
        {
            return new keyPressControl(new KeyPressOptions { KeyPress = Keypress, KeyModifiers = keymodifiers });
        }

        public static IControlMaskEdit MaskEdit(MaskedType type, string prompt = null)
        {
            return type switch
            {
                MaskedType.Generic => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Generic, Message = prompt }),
                MaskedType.DateOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateOnly, Message = prompt }),
                MaskedType.TimeOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.TimeOnly, Message = prompt }),
                MaskedType.DateTime => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateTime, Message = prompt }),
                MaskedType.Number => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Number, Message = prompt }),
                MaskedType.Currency => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Currency, Message = prompt }),
                _ => throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, type))
            };
        }

        public static IControlInput Input(string prompt = null)
        {
            return new InputControl(new InputOptions() { Message = prompt });
        }

        public static IControlConfirm Confirm(string prompt = null)
        {
            return new ConfirmControl(new ConfirmOptions() { Message = prompt });
        }

        public static IControlSliderNumber SliderNumber(string prompt = null)
        {
            return new SliderNumberControl(new SliderNumberOptions() { Message = prompt, Type = SliderNumberType.LeftRight });
        }

        public static IControlSliderNumber NumberUpDown(string prompt = null)
        {
            return new SliderNumberControl(new SliderNumberOptions() { Message = prompt, Type = SliderNumberType.UpDown });
        }

        public static IControlSliderSwitche SliderSwitche(string prompt = null)
        {
            return new SliderSwitcheControl(new SliderSwitcheOptions() { Message = prompt });
        }

        public static IControlProgressbar Progressbar(string prompt = null)
        {
            return new ProgressBarControl(new ProgressBarOptions() { Message = prompt });
        }

        public static IControlWaitProcess WaitProcess(string prompt = null)
        {
            return new WaitProcessControl(new WaitProcessOptions() { Message = prompt });
        }

        public static IControlSelect<T> Select<T>(string prompt = null)
        {
            return new SelectControl<T>(new SelectOptions<T>() { Message = prompt });
        }

        public static IControlMultiSelect<T> MultiSelect<T>(string prompt = null)
        {
            return new MultiSelectControl<T>(new MultiSelectOptions<T>() { Message = prompt });
        }

        public static IControlBrowser Browser(string prompt = null)
        {
            return new BrowserControl(new BrowserOptions() { Message = prompt });
        }

        public static IControlList<T> List<T>(string prompt = null)
        {
            return new ListControl<T>(new ListOptions<T>() { Message = prompt });
        }

        public static IControlListMasked ListMasked(string prompt = null)
        {
            return new MaskedListControl(new ListOptions<string>() { Message = prompt });
        }

        public static IControlPipeLine Pipeline()
        {
            return new PipeLineControl();
        }
    }
}

