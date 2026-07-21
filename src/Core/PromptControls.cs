// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Calendar;
using PromptPlusLibrary.Controls.ChartBar;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.FileExec;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Controls.KeyPress;
using PromptPlusLibrary.Controls.MaskEdit;
using PromptPlusLibrary.Controls.MultiFile;
using PromptPlusLibrary.Controls.MultiSelect;
using PromptPlusLibrary.Controls.MultiTable;
using PromptPlusLibrary.Controls.MultiTasks;
using PromptPlusLibrary.Controls.MultiTree;
using PromptPlusLibrary.Controls.ProgressBar;
using PromptPlusLibrary.Controls.Select;
using PromptPlusLibrary.Controls.Slider;
using PromptPlusLibrary.Controls.Switch;
using PromptPlusLibrary.Controls.Table;
using PromptPlusLibrary.Controls.TaskExec;
using PromptPlusLibrary.Controls.Time;
using PromptPlusLibrary.Controls.Tree;
using PromptPlusLibrary.Resources;
using System;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Internal implementation of the IControls interface, providing factory methods for creating interactive controls with fluent configuration options.
    /// </summary>
    /// <param name="console">The console interface used for input/output operations.</param>
    /// <param name="promptConfig">The global configuration for PromptPlus.</param>
    internal sealed class PromptPlusControls(IConsole console, PromptConfig promptConfig) : IControls
    {
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

        public IKeyPressControl KeyPress(string prompt = "", string? description = null, bool showresult = true)
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
            return new KeyPressControl(false, console, promptConfig, opt);
        }

        public IKeyPressControl Confirm(string prompt = "", string? description = null, bool showresult = true)
        {
            static (ConsoleKey YesKey, ConsoleKey NoKey) GetCultureKeys(PromptConfig promptConfig)
            {
                ConsoleKey yesKey = ConsoleKey.Y;
                ConsoleKey noKey = ConsoleKey.N;
                bool foundYes = false;
                bool foundNo = false;

                foreach (ConsoleKey item in Enum.GetValues<ConsoleKey>())
                {
                    string keyName = item.ToString();
                    if (keyName.Length != 1)
                    {
                        continue;
                    }

                    char keyChar = keyName[0];
                    if (!foundYes && keyChar.Equals(promptConfig.YesChar))
                    {
                        yesKey = item;
                        foundYes = true;
                    }

                    if (!foundNo && keyChar.Equals(promptConfig.NoChar))
                    {
                        noKey = item;
                        foundNo = true;
                    }

                    if (foundYes && foundNo)
                    {
                        break;
                    }
                }

                return (yesKey, noKey);
            }

            (ConsoleKey yesKey, ConsoleKey noKey) = GetCultureKeys(promptConfig);

            BaseControlOptions opt = new(promptConfig);
            if (!showresult)
            {
                opt.HideAfterFinish(true);
                opt.HideOnAbort(true);
            }
            if (!string.IsNullOrEmpty(prompt))
            {
                opt.Prompt($"{prompt} ({yesKey}/{noKey})");
            }
            else
            {
                opt.Prompt($"{PromptPlusResources.PressKey} ({yesKey}/{noKey})");
            }

            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            KeyPressControl aux = new(false, console, promptConfig, opt);
            aux.AddValidKey(yesKey, null);
            aux.AddValidKey(noKey, null);
            return aux;
        }

        public IProgressBarControl ProgressBar(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new ProgressBarControl(false, console, promptConfig, opt);
        }

        public IInputSecretControl Secret(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new InputControl(true, console, promptConfig, opt);
        }

        public IInputControl Input(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new InputControl(false, console, promptConfig, opt);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ITableControl<T> Table<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TableControl<T>(console, promptConfig, opt);
        }


        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IMultiTableControl<T> MultiTable<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MultiTableControl<T>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public ISwitchControl Switch(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new SwitchContrrol(false, console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public ITimeControl Time(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TimeControl(console, promptConfig, opt);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IMaskEditNumberControl<int> MaskInteger(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MaskEditControl<int>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IMaskEditNumberControl<long> MaskLong(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MaskEditControl<long>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IMaskEditCurrencyControl<decimal> MaskDecimal(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MaskEditControl<decimal>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IMaskEditCurrencyControl<double> MaskDouble(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MaskEditControl<double>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public ITaskControl Task(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TaskControl(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IMultiTasksControl MultiTasks(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MultiTasksControl(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IFileControl File(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new FileControl(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IMultiFileControl MultiFile(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MultiFileControl(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public ITreeControl<T> Tree<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new TreeControl<T>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IMultiTreeControl<T> MultiTree<T>(string prompt = "", string? description = null)
        {
            BaseControlOptions opt = new(promptConfig);
            opt.Prompt(prompt);
            if (!string.IsNullOrEmpty(description))
            {
                opt.Description(description);
            }
            return new MultiTreeControl<T>(console, promptConfig, opt);
        }

        /// <inheritdoc/>
        public IHistory History(string filename)
        {
            return new HistoryControl(filename);
        }

        /// <inheritdoc/>
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


    }
}
