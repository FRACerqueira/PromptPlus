// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class MultiSelectControl<T> : ControlBase<IEnumerable<T>>, IDisposable, IControlMultiSelect<T>
    {

        private readonly MultiSelectOptions<T> _options;
        private readonly List<T> _selectedItems = new();
        private readonly InputBuffer _filterBuffer = new();
        private Paginator<T> _localpaginator;

        public MultiSelectControl(MultiSelectOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
        }

        public new void Dispose()
        {
            if (_localpaginator != null)
            {
                _localpaginator.Dispose();
            }
            base.Dispose();
        }

        public override void InitControl()
        {
            if (typeof(T).IsEnum && _options.Items.Count == 0)
            {
                AddEnum();
            }
            _localpaginator = new Paginator<T>(_options.Items, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
            _localpaginator.FirstItem();
            if (_options.DefaultValues != null)
            {
                _selectedItems.AddRange(_options.DefaultValues);
            }
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out IEnumerable<T> result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);

                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    continue;
                }

                else if (PromptPlus.UnSelectFilter.Equals(keyInfo))
                {
                    _localpaginator.UnSelected();
                    result = default;
                    return isvalidhit;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.A when keyInfo.Modifiers == ConsoleModifiers.Alt:
                    {
                        if (_selectedItems.Count != _options.Items.Count())
                        {
                            _selectedItems.Clear();
                            _selectedItems.AddRange(_options.Items);
                        }
                        else
                        {
                            _selectedItems.Clear();
                        }
                        break;
                    }
                    case ConsoleKey.I when keyInfo.Modifiers == ConsoleModifiers.Alt:
                    {
                        var aux = _options.Items.Where(x => !_selectedItems.Contains(x)).ToArray();
                        _selectedItems.Clear();
                        _selectedItems.AddRange(aux);
                        break;
                    }
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _selectedItems.Count >= _options.Minimum:
                        result = _selectedItems;
                        return true;
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                        SetError(string.Format(Messages.MultiSelectMinSelection, _options.Minimum));
                        break;
                    case ConsoleKey.Spacebar when keyInfo.Modifiers == 0 && _localpaginator.TryGetSelectedItem(out var currentItem):
                    {
                        if (_selectedItems.Contains(currentItem))
                        {
                            _selectedItems.Remove(currentItem);
                        }
                        else
                        {
                            if (_selectedItems.Count >= _options.Maximum)
                            {
                                SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                            }
                            else
                            {
                                _selectedItems.Add(currentItem);
                            }
                        }

                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _localpaginator.UpdateFilter(_filterBuffer.Backward().ToString());
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_filterBuffer.Forward().ToString());
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _localpaginator.UpdateFilter(_filterBuffer.Backspace().ToString());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_filterBuffer.Delete().ToString());
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _localpaginator.UpdateFilter(_filterBuffer.Insert(keyInfo.KeyChar).ToString());
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            result = null;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            var showSelected = (_selectedItems.Count > 0 && _filterBuffer.Length == 0) || !_localpaginator.IsUnSelected;
            if (_localpaginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToBackwardString());
            }
            if (showSelected && !_localpaginator.IsUnSelected)
            {
                screenBuffer.WriteAnswer(string.Join(", ", _selectedItems.Select(_options.TextSelector)));
            }
            screenBuffer.PushCursor();
            if (_localpaginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToForwardString());
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_localpaginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(Messages.MultiSelectKeyNavigation);
                }
            }

            if (_filterBuffer.Length > 0)
            {
                screenBuffer.WriteLineFilter(Messages.ItemsFiltered);
                screenBuffer.WriteFilter($" ({_filterBuffer})");
            }
            var subset = _localpaginator.ToSubset();
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<T>.Default.Equals(item, selectedItem))
                {
                    if (_selectedItems.Contains(item))
                    {
                        screenBuffer.WriteLineMarkSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotMarkSelect(value);
                    }
                }
                else
                {
                    if (_selectedItems.Contains(item))
                    {
                        screenBuffer.WriteLineSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelect(value);
                    }
                }
            }
            if (_localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<T> result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = string.Join(", ", result.Select(_options.TextSelector));
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlMultiSelect

        public IControlMultiSelect<T> Prompt(string value)
        {
            _options.Message = value;
            return this;
        }

        public IControlMultiSelect<T> AddDefault(T value)
        {
            _options.DefaultValues.Add(value);
            return this;
        }
        public IControlMultiSelect<T> AddDefaults(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.DefaultValues.Add(item);
            }
            return this;
        }
        public IControlMultiSelect<T> AddItem(T value)
        {
            _options.Items.Add(value);
            return this;
        }

        public IControlMultiSelect<T> AddItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.Items.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> PageSize(int value)
        {
            if (value < 0)
            {
                _options.PageSize = null;
            }
            else
            {
                _options.PageSize = value;
            }
            return this;
        }
        public IControlMultiSelect<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value ?? (x => x.ToString());
            return this;
        }

        public IControlMultiSelect<T> Ranger(int minvalue, int maxvalue)
        {
            if (minvalue < 0)
            {
                minvalue = 0;
            }
            if (maxvalue < 0)
            {
                maxvalue = minvalue;
            }
            if (minvalue > maxvalue)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{minvalue},{maxvalue}"));
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue;
            return this;
        }

        public IPromptControls<IEnumerable<T>> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<IEnumerable<T>> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<IEnumerable<T>> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<IEnumerable<T>> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<IEnumerable<T>> Run(CancellationToken? value = null)
        {
            InitControl();
            return Start(value ?? CancellationToken.None);
        }

        private void AddEnum()
        {
            _options.TextSelector = EnumDisplay;
            var aux = Enum.GetValues(typeof(T));
            var result = new List<Tuple<int, T>>();
            foreach (var item in aux)
            {
                var name = item.ToString();
                var displayAttribute = typeof(T).GetField(name)?.GetCustomAttribute<DisplayAttribute>();
                var order = displayAttribute?.GetOrder() ?? int.MaxValue;
                result.Add(new Tuple<int, T>(order, (T)item));
            }
            foreach (var item in result.OrderBy(x => x.Item1))
            {
                _options.Items.Add(item.Item2);
            }
        }

        private string EnumDisplay(T value)
        {
            var name = value.ToString();
            var displayAttribute = value.GetType().GetField(name)?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? name;
        }


        public IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
        {
            PipeCondition = condition;
            return this;
        }

        public IFormPlusBase AddPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }
        #endregion
    }
}
