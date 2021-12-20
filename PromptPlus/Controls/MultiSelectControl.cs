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

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

using static PPlus.PromptPlus;

namespace PPlus.Controls
{
    internal class MultiSelectControl<T> : ControlBase<IEnumerable<T>>, IControlMultiSelect<T>
    {

        private readonly MultiSelectOptions<T> _options;
        private readonly List<ItemMultSelect<T>> _selectedItems = new();
        private readonly ReadLineBuffer _filterBuffer = new();
        private Paginator<ItemMultSelect<T>> _localpaginator;
        private const string Namecontrol = "PromptPlus.MultiSelect";

        public MultiSelectControl(MultiSelectOptions<T> options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override IEnumerable<T> InitControl()
        {
            Thread.CurrentThread.CurrentCulture = DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = DefaultCulture;

            if (_options.Items is null)
            {
                throw new ArgumentNullException(nameof(_options.Items));
            }
            if (typeof(T).IsEnum && _options.Items.Count == 0)
            {
                AddEnum();
            }
            var qtdgrp = _options.Items.Count(x => x.IsGroup);

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
            if (_options.DescriptionSelector != null)
            {
                if (_localpaginator.SelectedIndex < 0)
                {
                    _options.Description = null;
                }
                else
                {
                    if (_localpaginator.SelectedItem.IsGroup)
                    {
                        _options.Description = _localpaginator.SelectedItem.Group;
                    }
                    else
                    {
                        if (_options.ShowGroupOnDescription)
                        {
                            if (string.IsNullOrEmpty(_localpaginator.SelectedItem.Group))
                            {
                                _options.Description = _options.NoGroupDescription ?? " ";
                            }
                            else
                            {
                                _options.Description = _localpaginator.SelectedItem.Group;
                            }
                        }
                        else
                        {
                            _options.Description = _options.DescriptionSelector.Invoke(_localpaginator.SelectedItem.Value);
                        }
                    }
                }
            }

            if (EnabledLogControl)
            {
                AddLog("Groups", qtdgrp.ToString(), LogKind.Property);
                AddLog("DefaultValues", _options.DefaultValues.Count.ToString(), LogKind.Property);
                AddLog("DisableItems", _options.DisableItems.Count.ToString(), LogKind.Property);
                AddLog("HideItems", _options.HideItems.Count.ToString(), LogKind.Property);
                AddLog("Items", _options.Items.Count.ToString(), LogKind.Property);
                AddLog("Maximum", _options.Maximum.ToString(), LogKind.Property);
                AddLog("Minimum", _options.Minimum.ToString(), LogKind.Property);
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

            return _selectedItems.Select(x => x.Value);
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
                _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo, out var acceptedkey);
                if (acceptedkey)
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                }
                else if (CheckDefaultKey(keyInfo))
                {
                    ///none
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    ///none
                }
                else if (UnSelectFilter.Equals(keyInfo))
                {
                    _localpaginator.UnSelected();
                }
                else if (SelectAll.Equals(keyInfo))
                {

                    var maxqtd = _options.Items.Count(x => !x.IsGroup && !x.Disabled);
                    if (_selectedItems.Count == maxqtd)
                    {
                        continue;
                    }
                    if (maxqtd > _options.Maximum)
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                        continue;
                    }
                    _selectedItems.Clear();
                    _selectedItems.AddRange(_options.Items.Where(x => !x.IsGroup && !x.Disabled));
                }
                else if (InvertSelect.Equals(keyInfo))
                {
                    var maxqtd = _options.Items.Count(x => !x.IsGroup && !x.Disabled && !_selectedItems.Contains(x));
                    if (maxqtd > _options.Maximum)
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                        continue;
                    }
                    var aux = _selectedItems.ToArray();
                    _selectedItems.Clear();
                    _selectedItems.AddRange(_options.Items.Where(x => !x.IsGroup && !x.Disabled && !aux.Contains(x)));
                }
                else if (MarkSelect.Equals(keyInfo))
                {
                    _localpaginator.TryGetSelectedItem(out var currentItem);
                    if (currentItem.IsGroup)
                    {
                        var maxgrp = _options.Items.Count(x => x.Group == currentItem.Group && !x.IsGroup && !x.Disabled);
                        var selgrp = _selectedItems.Count(y => _options.Items.Where(x => x.Group == currentItem.Group && !x.IsGroup && !x.Disabled).Contains(y));
                        if (maxgrp == selgrp)
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
                            var qtd = aux.Sum(x => _selectedItems.FindIndex(x => x.Value.Equals(x.Value)) >= 0 ? 1 : 0);
                            if (_selectedItems.Count + qtd > _options.Maximum)
                            {
                                SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                            }
                            else
                            {
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
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    if (_selectedItems.Count < _options.Minimum)
                    {
                        SetError(string.Format(Messages.MultiSelectMinSelection, _options.Minimum));
                    }
                    else
                    {
                        result = _selectedItems.Select(x => x.Value);
                        return true;
                    }
                }
                else
                {
                    isvalidhit = null;
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            if (_options.DescriptionSelector != null)
            {
                if (_localpaginator.SelectedIndex < 0)
                {
                    _options.Description = null;
                }
                else
                {
                    if (_localpaginator.SelectedItem.IsGroup)
                    {
                        _options.Description = _localpaginator.SelectedItem.Group;
                    }
                    else
                    {
                        if (_options.ShowGroupOnDescription)
                        {
                            if (string.IsNullOrEmpty(_localpaginator.SelectedItem.Group))
                            {
                                _options.Description = _options.NoGroupDescription ?? " ";
                            }
                            else
                            {
                                _options.Description = _localpaginator.SelectedItem.Group;
                            }
                        }
                        else
                        {
                            _options.Description = _options.DescriptionSelector.Invoke(_localpaginator.SelectedItem.Value);
                        }
                    }
                }
            }
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

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_localpaginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(string.Format(Messages.MultiSelectKeyNavigation));
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
                            indentgroup = $" {Symbols.IndentEndGroup}";
                        }
                        else
                        {
                            indentgroup = $" {Symbols.IndentGroup}";
                        }
                    }
                }

                if (_selectedItems.Any(x => !x.IsGroup && x.Value.Equals(item.Value)))
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


        public IControlMultiSelect<T> Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlMultiSelect<T> ShowGroupOnDescription(string noGroupMessage)
        {
            _options.ShowGroupOnDescription = true;
            _options.NoGroupDescription = noGroupMessage;
            return this;
        }

        public IControlMultiSelect<T> AddDefault(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DefaultValues.Add(value);
            return this;
        }

        public IControlMultiSelect<T> AddDefaults(IEnumerable<T> value)
        {
            if (value == null)
            {
                return this;
            }
            foreach (var item in value)
            {
                _options.DefaultValues.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> AddItem(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.Items.Add(new ItemMultSelect<T> { Value = value, Disabled = false, Group = string.Empty });
            return this;
        }

        public IControlMultiSelect<T> AddItems(IEnumerable<T> value)
        {
            if (value == null)
            {
                return this;
            }
            foreach (var item in value)
            {
                _options.Items.Add(new ItemMultSelect<T> { Value = item, Disabled = false, Group = string.Empty });
            }
            return this;
        }

        public IControlMultiSelect<T> AddGroup(IEnumerable<T> value, string group)
        {
            if (value == null || string.IsNullOrEmpty(group))
            {
                return this;
            }
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
            if (value == null)
            {
                return this;
            }
            _options.HideItems.Add(value);
            return this;
        }

        public IControlMultiSelect<T> HideItems(IEnumerable<T> value)
        {
            if (value == null)
            {
                return this;
            }
            foreach (var item in value)
            {
                _options.HideItems.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> DisableItem(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DisableItems.Add(value);
            return this;
        }

        public IControlMultiSelect<T> DisableItems(IEnumerable<T> value)
        {
            if (value == null)
            {
                return this;
            }
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

        public IControlMultiSelect<T> DescriptionSelector(Func<T, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlMultiSelect<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value ?? (x => x.ToString());
            return this;
        }

        public IControlMultiSelect<T> Range(int minvalue, int maxvalue)
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

        public IControlMultiSelect<T> Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
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

        #endregion
    }
}
