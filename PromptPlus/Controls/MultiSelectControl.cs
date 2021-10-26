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
    internal class MultiSelectControl<T> : ControlBase<IEnumerable<T>>, IControlMultiSelect<T>
    {

        private readonly MultiSelectOptions<T> _options;
        private readonly List<ItemMultSelect<T>> _selectedItems = new();
        private readonly InputBuffer _filterBuffer = new();
        private Paginator<ItemMultSelect<T>> _localpaginator;

        public MultiSelectControl(MultiSelectOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
        }

        public override void InitControl()
        {
            if (_options.Items is null)
            {
                throw new ArgumentNullException(nameof(_options.Items));
            }
            if (typeof(T).IsEnum && _options.Items.Count == 0)
            {
                AddEnum();
            }
            foreach (var item in _options.Items.Where(x => !x.IsGroup))
            {
                item.Disabled = _options.DisableItems.Contains(item.Value);
                item.Text = _options.TextSelector.Invoke(item.Value);
            }
            _localpaginator = new Paginator<ItemMultSelect<T>>(_options.Items, _options.PageSize, Optional<ItemMultSelect<T>>.s_empty, (item) =>
            {
                if (!item.IsGroup)
                {
                    return _options.TextSelector.Invoke(item.Value);
                }
                return item.Group;
            }, (item) => !item.Disabled);
            _localpaginator.FirstItem();
            if (_options.DefaultValues != null)
            {
                foreach (var item in _options.DefaultValues)
                {
                    var aux = _options.Items.FirstOrDefault(x => !x.IsGroup && x.Value.Equals(item));
                    var grp = string.Empty;
                    if (!string.IsNullOrEmpty(aux.Group))
                    {
                        grp = aux.Group;
                    }
                    _selectedItems.Add(new ItemMultSelect<T> { Value = item, Text = _options.TextSelector.Invoke(item), Group = grp, Disabled = aux.Disabled });
                }
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
                else if (PromptPlus.SelectAll.Equals(keyInfo))
                {
                    var disabledselect = _selectedItems.Where(x => x.Disabled).Select(x => x.Value).ToArray();

                    if (_selectedItems.Count < _options.Items.Count(x => !x.IsGroup) - _options.DisableItems.Count + disabledselect.Length)
                    {
                        _selectedItems.Clear();
                        _selectedItems.AddRange(_options.Items.Where(x => (!x.IsGroup && !x.Disabled) || disabledselect.Contains(x.Value)));
                    }
                    else
                    {
                        _selectedItems.Clear();
                        _selectedItems.AddRange(_options.Items.Where(x => !x.IsGroup && disabledselect.Contains(x.Value)));
                    }
                    continue;
                }
                else if (PromptPlus.InvertSelect.Equals(keyInfo))
                {
                    var disabledselect = _selectedItems.Where(x => x.Disabled).Select(x => x.Value).ToArray();
                    var aux = _options.Items.Where(x => !x.IsGroup && (!_selectedItems.Contains(x) && !x.Disabled) || disabledselect.Contains(x.Value)).ToArray();
                    _selectedItems.Clear();
                    _selectedItems.AddRange(aux);
                    continue;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _selectedItems.Count >= _options.Minimum:
                        result = _selectedItems.Select(x => x.Value);
                        return true;
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                        SetError(string.Format(Messages.MultiSelectMinSelection, _options.Minimum));
                        break;
                    case ConsoleKey.Spacebar when keyInfo.Modifiers == 0 && _localpaginator.TryGetSelectedItem(out var currentItem):
                    {
                        if (currentItem.IsGroup)
                        {
                            if (currentItem.IsSelected)
                            {
                                var aux = _selectedItems.Where(x => x.Group == currentItem.Group && !x.IsGroup && !x.Disabled).ToArray();
                                foreach (var item in aux)
                                {
                                    var index = _selectedItems.FindIndex(x => x.Value.Equals(item.Value));
                                    _selectedItems.RemoveAt(index);
                                }
                                currentItem.IsSelected = false;
                            }
                            else
                            {
                                var aux = _options.Items.Where(x => x.Group == currentItem.Group && !x.IsGroup && !x.Disabled);
                                foreach (var item in aux)
                                {
                                    var index = _selectedItems.FindIndex(x => x.Value.Equals(item.Value));
                                    if (index < 0)
                                    {
                                        _selectedItems.Add(item);
                                    }
                                }
                                var auxsel = _options.Items.Where(x => _selectedItems.Contains(x)).ToArray();
                                _selectedItems.Clear();
                                _selectedItems.AddRange(auxsel);
                                currentItem.IsSelected = true;
                            }
                        }
                        else
                        {
                            var index = _selectedItems.FindIndex(x => x.Value.Equals(currentItem.Value));
                            if (index >= 0)
                            {
                                _selectedItems.RemoveAt(index);
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
                                    var auxsel = _options.Items.Where(x => _selectedItems.Contains(x)).ToArray();
                                    _selectedItems.Clear();
                                    _selectedItems.AddRange(auxsel);
                                }
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
                screenBuffer.Write(_filterBuffer.ToBackward());
            }
            if (showSelected && !_localpaginator.IsUnSelected)
            {
                screenBuffer.WriteAnswer(string.Join(", ", _selectedItems.Select(x => x.Text)));
            }
            screenBuffer.PushCursor();
            if (_localpaginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToForward());
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
                string value;
                var indentgroup = string.Empty;
                if (item.IsGroup)
                {
                    value = item.Group;
                }
                else
                {
                    value = _options.TextSelector(item.Value);
                    if (!string.IsNullOrEmpty(item.Group))
                    {
                        if (item.IsLastItem)
                        {
                            indentgroup = $" {PromptPlus.Symbols.IndentEndGroup}";
                        }
                        else
                        {
                            indentgroup = $" {PromptPlus.Symbols.IndentGroup}";
                        }
                    }
                }

                if (_selectedItems.Count(x => !x.IsGroup && x.Value.Equals(item.Value)) > 0)
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && item.Value.Equals(selectedItem.Value))
                    {
                        screenBuffer.WriteLineMarkSelectIndent(indentgroup);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotMarkSelectIndent(indentgroup);
                    }
                    screenBuffer.WriteMarkSelect(value);
                }
                else
                {
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && ((!item.IsGroup && item.Value.Equals(selectedItem.Value)) || (item.IsGroup && item.Group == selectedItem.Group)))
                    {
                        if (item.IsGroup)
                        {
                            screenBuffer.WriteLineGroupIndent(indentgroup);
                        }
                        else
                        {
                            screenBuffer.WriteLineMarkNotSelectIndent(indentgroup);
                        }
                    }
                    else
                    {
                        if (item.IsGroup)
                        {
                            screenBuffer.WriteLineGroupUnselectIndent(indentgroup);
                        }
                        else
                        {
                            screenBuffer.WriteLineNotMarkUnSelectIndent(indentgroup);
                        }
                    }
                    if (item.Disabled)
                    {
                        screenBuffer.WriteNotMarkSelectDisabled(value);
                    }
                    else
                    {
                        screenBuffer.WriteNotMarkSelect(value);
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
            _options.Items.Add(new ItemMultSelect<T> { Value = value, Disabled = false, Group = string.Empty });
            return this;
        }

        public IControlMultiSelect<T> AddItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.Items.Add(new ItemMultSelect<T> { Value = item, Disabled = false, Group = string.Empty });
            }
            return this;
        }

        public IControlMultiSelect<T> AddGroup(IEnumerable<T> value, string group)
        {
            if (!string.IsNullOrEmpty(group) && !typeof(T).IsEnum)
            {
                _options.Items.Add(new ItemMultSelect<T> { Value = default, Disabled = false, Group = group, IsGroup = true });
            }
            else
            {
                group = string.Empty;
            }
            var pos = 0;
            foreach (var item in value)
            {
                if (pos == value.Count() - 1)
                {
                    _options.Items.Add(new ItemMultSelect<T> { Value = item, Disabled = false, Group = group, IsLastItem = true });
                }
                else
                {
                    _options.Items.Add(new ItemMultSelect<T> { Value = item, Disabled = false, Group = group });
                }
                pos++;
            }
            return this;
        }

        public IControlMultiSelect<T> HideItem(T value)
        {
            _options.HideItems.Add(value);
            return this;
        }

        public IControlMultiSelect<T> HideItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.HideItems.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> DisableItem(T value)
        {
            _options.DisableItems.Add(value);
            return this;
        }

        public IControlMultiSelect<T> DisableItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.DisableItems.Add(item);
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
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
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
                _options.Items.Add(new ItemMultSelect<T> { Value = item.Item2, Text = _options.TextSelector(item.Item2), Disabled = false, Group = string.Empty });
            }
        }

        private string EnumDisplay(T value)
        {
            var name = value.ToString();
            var displayAttribute = value.GetType().GetField(name)?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? name;
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion
    }
}
