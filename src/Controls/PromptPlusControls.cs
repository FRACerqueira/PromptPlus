// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Controls.AutoComplete;
using PromptPlusLibrary.Controls.Calendar;
using PromptPlusLibrary.Controls.ChartBar;
using PromptPlusLibrary.Controls.FileMultiSelect;
using PromptPlusLibrary.Controls.FileSelect;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Controls.KeyPress;
using PromptPlusLibrary.Controls.MaskEdit;
using PromptPlusLibrary.Controls.MultiSelect;
using PromptPlusLibrary.Controls.NodeTreeMultiSelect;
using PromptPlusLibrary.Controls.NodeTreeRemoteSelect;
using PromptPlusLibrary.Controls.NodeTreeSelect;
using PromptPlusLibrary.Controls.ProgressBar;
using PromptPlusLibrary.Controls.ReadLine;
using PromptPlusLibrary.Controls.RemoteMultiSelect;
using PromptPlusLibrary.Controls.RemoteSelect;
using PromptPlusLibrary.Controls.Select;
using PromptPlusLibrary.Controls.Slider;
using PromptPlusLibrary.Controls.Switch;
using PromptPlusLibrary.Controls.TableMultiSelect;
using PromptPlusLibrary.Controls.TableSelect;
using PromptPlusLibrary.Controls.WaitProcess;
using PromptPlusLibrary.Controls.WaitTimer;
using PromptPlusLibrary.Resources;
using System;
using System.Linq;

namespace PromptPlusLibrary.Controls
{
    internal sealed class PromptPlusControls(IConsole console, PromptConfig promptConfig) : IControls
    {
        public IEmacs InputEmacs(string initialvalue = "")
        {
            return new ReadLineEmacs(console, promptConfig, initialvalue);
        }

        public IInputControl Input(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new InputControl(console, promptConfig, opt);
        }

        public IHistory History(string filename)
        {
            return new History(filename);
        }

        public ICalendarControl Calendar(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new CalendarControl(false, console, promptConfig, opt);
        }

        public IKeyPressControl KeyPress(string prompt = "", string? description = null, bool showresult = false)
        {
            BaseControlOptions opt = new(promptConfig);
            if (!showresult)
            {
                opt.HideAfterFinish(true);
                opt.HideOnAbort(true);
            }
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new KeyPressControl(console, promptConfig, opt);
        }

        public IKeyPressControl Confirm(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            KeyPressControl aux = new(console, promptConfig, opt);
            aux.AddKeyValid(GetCultureYes(promptConfig), null, Messages.YesText);
            aux.AddKeyValid(GetCultureNo(promptConfig), null, Messages.NoText);
            return aux;
        }

        public IWaitTimerControl WaitTimer(int mileseconds, string prompt = "", string? description = null, bool showresult = false)
        {
            return WaitTimer(TimeSpan.FromMilliseconds(mileseconds), prompt, description, showresult);
        }

        public IWaitTimerControl WaitTimer(TimeSpan time, string prompt = "", string? description = null, bool showresult = false)
        {
            BaseControlOptions opt = new(promptConfig);
            if (!showresult)
            {
                opt.HideAfterFinish(true);
                opt.HideOnAbort(true);
            }
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new WaitTimerControl(time, console, promptConfig, opt);
        }

        private static ConsoleKey GetCultureYes(PromptConfig promptConfig)
        {
            foreach (ConsoleKey item in Enum.GetValues<ConsoleKey>().Where(x => x.ToString().Length == 1))
            {
                if (item.ToString()[0].Equals(promptConfig.YesChar))
                {
                    return item;
                }
            }
            return ConsoleKey.Y;
        }

        private static ConsoleKey GetCultureNo(PromptConfig promptConfig)
        {
            foreach (ConsoleKey item in Enum.GetValues<ConsoleKey>().Where(x => x.ToString().Length == 1))
            {
                if (item.ToString()[0].Equals(promptConfig.NoChar))
                {
                    return item;
                }
            }
            return ConsoleKey.N;
        }

        public IWaitProcessControl WaitProcess(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new WaitProcessControl(console, promptConfig, opt);
        }

        public IProgressBarControl ProgressBar(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new ProgressBarControl(console, promptConfig, opt);
        }

        public ISliderControl Slider(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new SliderControl(false, console, promptConfig, opt);
        }

        public ISwitchControl Switch(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new SwitchControl(false, console, promptConfig, opt);
        }

        public IChartBarControl ChartBar(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new ChartBarControl(false, console, promptConfig, opt);
        }

        public ISelectControl<T> Select<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new SelectControl<T>(console, promptConfig, opt);
        }

        public IMultiSelectControl<T> MultiSelect<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MultiSelectControl<T>(console, promptConfig, opt);
        }

        public IAutoCompleteControl AutoComplete(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new AutoCompleteControl(console, promptConfig, opt);
        }

        public IMaskEditStringControl<string> MaskEdit(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MaskEditControl<string>(console, promptConfig, opt);
        }

        public IMaskEditDateTimeControl<DateTime> MaskDateTime(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<DateTime> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetMask("d/M/y h:m:s", false);
            return ctrl;
        }

        public IMaskEditDateTimeControl<DateTime> MaskDate(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<DateTime> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetMask("d/M/y", false);
            return ctrl;
        }

        public IMaskEditDateTimeControl<DateOnly> MaskDateOnly(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<DateOnly> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetMask("d/M/y", false);
            return ctrl;
        }

        public IMaskEditDateTimeControl<DateTime> MaskTime(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<DateTime> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetMask("h:m:s", false);
            return ctrl;
        }

        public IMaskEditDateTimeControl<TimeOnly> MaskTimeOnly(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<TimeOnly> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetMask("h:m:s", false);
            return ctrl;
        }

        public IMaskEditNumberControl<int> MaskInteger(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<int> ctrl = new(console, promptConfig, opt);
            return ctrl;
        }

        public IMaskEditNumberControl<long> MaskLong(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<long> ctrl = new(console, promptConfig, opt);
            return ctrl;
        }

        public IMaskEditCurrencyControl<decimal> MaskDecimalCurrency(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<decimal> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetCurrencyMask();
            return ctrl;
        }

        public IMaskEditCurrencyControl<decimal> MaskDecimal(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<decimal> ctrl = new(console, promptConfig, opt);
            return ctrl;
        }

        public IMaskEditCurrencyControl<double> MaskDoubleCurrency(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<double> ctrl = new(console, promptConfig, opt);
            ctrl.InternalSetCurrencyMask();
            return ctrl;
        }

        public IMaskEditCurrencyControl<double> MaskDouble(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            MaskEditControl<double> ctrl = new(console, promptConfig, opt);
            return ctrl;
        }

        public ITableSelectControl<T> TableSelect<T>(string prompt = "", string? description = null) where T : class
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TableSelectControl<T>(false, console, promptConfig, opt);
        }

        public ITableMultiSelectControl<T> TableMultiSelect<T>(string prompt = "", string? description = null) where T : class
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TableMultiSelectControl<T>(console, promptConfig, opt);
        }

        public IFileSelectControl FileSelect(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new FileSelectControl(console, promptConfig, opt);
        }

        public IFileMultiSelectControl FileMultiSelect(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new FileMultiSelectControl(console, promptConfig, opt);
        }

        public INodeTreeSelectControl<T> NodeTreeSelect<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new NodeTreeSelectControl<T>(console, promptConfig, opt);
        }

        public INodeTreeMultiSelectControl<T> NodeTreeMultiSelect<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new NodeTreeMultiSelectControl<T>(console, promptConfig, opt);
        }

        public IRemoteSelectControl<T1, T2> RemoteSelect<T1,T2>(string prompt = "", string? description = null) where T1 : class where T2 : class
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new RemoteSelectControl<T1,T2>(console, promptConfig, opt);
        }

        public IRemoteMultiSelectControl<T1, T2> RemoteMultiSelect<T1, T2>(string prompt = "", string? description = null) where T1 : class where T2 : class
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new RemoteMultiSelectControl<T1, T2>(console, promptConfig, opt);
        }

        public INodeTreeRemoteSelectControl<T1,T2> NodeTreeRemoteSelect<T1,T2>(string prompt = "", string? description = null) where T1 : class where T2 : class
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new NodeTreeRemoteSelectControl<T1,T2>(console, promptConfig, opt);
        }

    }
}
