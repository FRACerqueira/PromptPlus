﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading;

namespace PPlus.Controls
{
    internal class MultiSelectControl<T> : BaseControl<IEnumerable<T>>, IControlMultiSelect<T>
    {
        private readonly MultiSelectOptions<T> _options;
        private readonly List<ItemMultSelect<T>> _selectedItems = new();
        private Paginator<ItemMultSelect<T>> _localpaginator;
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase, modefilter: true);
        private Optional<IList<T>> _defaultHistoric = Optional<IList<T>>.Empty();
        private bool ShowingFilter => _filterBuffer.Length > 0;


        public MultiSelectControl(IConsoleControl console, MultiSelectOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadHistory();
            }

            if (typeof(T).IsEnum)
            {
                _options.TextSelector ??= EnumDisplay;
                AddEnum();
            }
            else
            {
                _options.TextSelector ??= (item) => item.ToString();
            }

            _options.EqualItems ??= (item1, item2) => item1.Equals(item2);

            foreach (var item in _options.Items.Where(x => !x.IsGroupHeader))
            {
                item.Text = _options.TextSelector.Invoke(item.Value);
            }

            foreach (var item in _options.RemoveItems)
            {
                int index;
                do
                {
                    index = _options.Items.FindIndex(x => !x.IsGroupHeader && _options.EqualItems(x.Value, item));
                    if (index >= 0)
                    {
                        _options.Items.RemoveAt(index);
                    }
                }
                while (index >= 0);
            }

            foreach (var item in _options.DisableItems)
            {
                List<ItemMultSelect<T>> founds;
                founds = _options.Items.FindAll(x => !x.IsGroupHeader && _options.EqualItems(x.Value, item));
                if (founds.Any())
                {
                    foreach (var itemfound in founds)
                    {
                        itemfound.Disabled = true;
                    }
                }
            }

            Optional<IList<T>> defvalue = _options.DefaultValues;
            if (_defaultHistoric.HasValue)
            {
                defvalue = _defaultHistoric;
            }
            else
            {
                if (!defvalue.HasValue)
                {
                    defvalue = Optional<IList<T>>.Empty();
                }
            }

            if (defvalue.HasValue)
            {
                foreach (var item in defvalue.Value.Where(x => !IsDisabled(x)))
                {
                    IEnumerable<ItemMultSelect<T>> foundmark;
                    foundmark = _options.Items.Where(x => !x.IsGroupHeader && _options.EqualItems(x.Value, item));
                    foreach (var itemmark in foundmark)
                    {
                        if (_selectedItems.Count <= _options.Maximum)
                        {
                            itemmark.IsCheck = true;
                        }
                    }
                }
            }
            var qtdsel = 0;
            for (int i = 0; i < _options.Items.Count; i++)
            {
                if (_options.Items[i].IsCheck && !_options.Items[i].IsGroupHeader)
                {
                    qtdsel++;
                    if (qtdsel > _options.Maximum)
                    {
                        _options.Items[i].IsCheck = false;
                    }
                }
            }

            foreach (var item in _options.Items.Where(x => x.IsCheck && !x.IsGroupHeader))
            {
                var found = false;
                found = _selectedItems.Any(x => !x.IsGroupHeader && _options.EqualItems(x.Value, item.Value));
                if (!found)
                {
                    _selectedItems.Add(item);
                }
            }

            if (_options.Maximum > _options.Items.Count)
            {
                _options.Maximum = _options.Items.Count;
            }

            if (_options.OrderBy != null)
            {
                if (_options.IsOrderDescending)
                {
                    _options.Items = _options.Items.OrderByDescending(x => x.Group ?? string.Empty + _options.OrderBy.Invoke(x.Value)).ToList();
                    var auxsel = _selectedItems.OrderByDescending(x => x.Group ?? string.Empty + x.Text).ToArray();
                    _selectedItems.Clear();
                    _selectedItems.AddRange(auxsel);
                }
                else
                {
                    _options.Items = _options.Items.OrderBy(x => x.Group ?? string.Empty + _options.OrderBy.Invoke(x.Value)).ToList();
                    var auxsel = _selectedItems.OrderBy(x => x.Group ?? string.Empty + x.Text).ToArray();
                    _selectedItems.Clear();
                    _selectedItems.AddRange(auxsel);
                }
            }

            _localpaginator = new Paginator<ItemMultSelect<T>>(
                _options.FilterType,
                _options.Items, 
                _options.PageSize, 
                Optional<ItemMultSelect<T>>.Empty(),
                (item1,item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.Text??string.Empty,
                (item) => !item.Disabled,
                (item) => !item.IsGroupHeader);


            if (_localpaginator.TotalCountValid > 0 && _localpaginator.SelectedItem != null && _localpaginator.SelectedItem.Disabled)
            {
                _localpaginator.UnSelected();
                if (!defvalue.HasValue)
                {
                    _localpaginator.FirstItem();
                }
            }

            if (_options.Items.Count == 0)
            {
                _localpaginator.UnSelected();
            }
            else
            {
                _localpaginator.FirstItem();
            }
            FinishResult = string.Join(", ", _selectedItems.Select(x => x.Text));
            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlMultiSelect

        public IControlMultiSelect<T> Styles(MultiSelectStyles content, Style value)
        {
            _options.StyleControl(content, value);
            return this;
        }

        public IControlMultiSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlMultiSelect<T>, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }


        public IControlMultiSelect<T> OrderBy(Expression<Func<T, object>> value)
        {
            if (_options.Items.Any(x => x.IsGroupHeader))
            {
                throw new PromptPlusException("OrderBy cannot be used with Grouped item");
            }
            _options.IsOrderDescending = false;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlMultiSelect<T> OrderByDescending(Expression<Func<T, object>> value)
        {
            if (_options.Items.Any(x => x.IsGroupHeader))
            {
                throw new PromptPlusException("OrderByDescending cannot be used with Grouped item");
            }
            _options.IsOrderDescending = true;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlMultiSelect<T> FilterType(FilterMode value)
        {
            _options.FilterType = value;
            return this;
        }

        public IControlMultiSelect<T> ShowTipGroup(bool value = true)
        {
            _options.ShowGroupTip = value;
            return this;
        }
        public IControlMultiSelect<T> HotKeySelectAll(HotKey value)
        {
            _options.SelectAllPress = value;
            return this;
        }

        public IControlMultiSelect<T> HotKeyInvertSelected(HotKey value)
        {
            _options.InvertSelectedPress = value;
            return this;
        }

        public IControlMultiSelect<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlMultiSelect<T> AddDefault(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                if (_options.DefaultValues.Value == null)
                {
                    _options.DefaultValues = Optional<IList<T>>.Set(new List<T>());
                }
                _options.DefaultValues.Value.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> AddDefault(params T[] value)
        {
            foreach (var item in value)
            {
                if (_options.DefaultValues.Value == null)
                {
                    _options.DefaultValues = Optional<IList<T>>.Set(new List<T>());
                }
                _options.DefaultValues.Value.Add(item);
            }
            return this;
        }

        public IControlMultiSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                if (timeout.Value.TotalMilliseconds == 0)
                {
                    throw new PromptPlusException("timeout must be greater than 0");
                }
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlMultiSelect<T> PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlMultiSelect<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value;
            return this;
        }

        public IControlMultiSelect<T> EqualItems(Func<T, T, bool> comparer)
        {
            _options.EqualItems = comparer;
            return this;
        }

        public IControlMultiSelect<T> ChangeDescription(Func<T, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlMultiSelect<T> AddItem(T value, bool disable = false, bool selected = false)
        {
            var newitem = new ItemMultSelect<T>
            {
                Value = value,
                Disabled = disable,
                IsCheck = selected
            };
            _options.Items.Add(newitem);
            return this;
        }

        public IControlMultiSelect<T> AddItems(IEnumerable<T> value, bool disable = false, bool selected = false)
        {
            foreach (var item in value)
            {
                AddItem(item, disable,selected);
            }
            return this;
        }

        public IControlMultiSelect<T> AddItemGrouped(string group, T value, bool disable = false, bool selected = false)
        {
            if (_options.OrderBy != null)
            {
                throw new PromptPlusException("AddItemGrouped cannot be used OrderBy/OrderByDescending");
            }
            var found = false;
            var added = false;
            foreach (var item in _options.Items.Where(x => !x.IsGroupHeader && !string.IsNullOrEmpty(x.Group)))
            {
                if (item.Group == group)
                {
                    item.IsLastItemGroup = false;
                    found = true;
                }
                else if (item.Group != group && found)
                {
                    added = true;
                    _options.Items.Add(new ItemMultSelect<T> { Value = value, Group = group, IsLastItemGroup = true, Disabled = disable, IsCheck = selected });
                    break;
                }
            }
            if (!found || !added)
            {
                if (!found)
                {
                    _options.Items.Add(new ItemMultSelect<T> { IsGroupHeader = true,  Text = null, Value = default, Group = group, Disabled = disable, IsCheck = selected });
                }
                _options.Items.Add(new ItemMultSelect<T> { Value = value, Group = group, IsLastItemGroup = true, Disabled = disable, IsCheck = selected });
            }
            return this;
        }

        public IControlMultiSelect<T> AddItemsGrouped(string group, IEnumerable<T> value,  bool disable = false, bool selected = false)
        {
            if (_options.OrderBy != null)
            {
                throw new PromptPlusException("AddItemsGrouped cannot be used OrderBy/OrderByDescending");
            }
            foreach (var item in value)
            {
                AddItemGrouped(group, item, disable,selected);
            }
            return this;
        }

        public IControlMultiSelect<T> AddItemsTo(AdderScope scope,params T[] values)
        {
            InternalAdd(scope, values);
            return this;
        }

        public IControlMultiSelect<T> AddItemsTo(AdderScope scope, IEnumerable<T> values)
        {
            InternalAdd(scope, values);
            return this;
        }

        private void InternalAdd(AdderScope scope, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                switch (scope)
                {
                    case AdderScope.Disable:
                        {
                            _options.DisableItems.Add(item);
                        }
                        break;
                    case AdderScope.Remove:
                        {
                            _options.RemoveItems.Add(item);
                        }
                        break;
                    default:
                        throw new PromptPlusException($"AdderScope : {scope} Not Implemented");
                }
            }
        }

        public IControlMultiSelect<T> OverflowAnswer(Overflow value)
        {
            _options.StyleControl(StyleControls.Answer, _options.StyleContent(StyleControls.Answer).Overflow(value));
            return this;
        }

        public IControlMultiSelect<T> Range(int minvalue, int? maxvalue = null)
        {
            if (!maxvalue.HasValue)
            {
               maxvalue = _options.Maximum;
            }
            if (minvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue})");
            }
            if (maxvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. maxvalue({maxvalue})");
            }
            if (minvalue > maxvalue)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue}) > maxvalue({maxvalue})");
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue.Value;
            return this;
        }

        #endregion

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            var first = !string.IsNullOrEmpty(_options.OptPrompt);
            if (ShowingFilter)
            {
                _options.OptShowCursor = true;
                if (_localpaginator.TryGetSelected(out var showItem))
                {
                    var item = showItem.Text;
                    screenBuffer.WriteFilterMultiSelect(_options, item, _filterBuffer);
                    screenBuffer.SaveCursor();
                }
                else
                {
                    screenBuffer.WriteEmptyFilter(_options, _filterBuffer.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteEmptyFilter(_options, _filterBuffer.ToForward());
                }
                if (!_options.OptMinimalRender)
                {
                    screenBuffer.WriteTaggedInfo(_options, $" ({Messages.Filter})");
                }
                first = false;
            }
            else
            {
                if (!_options.OptMinimalRender)
                {
                    screenBuffer.WriteAnswer(_options, FinishResult);
                    first = false;
                }
                else
                {
                    _options.OptShowCursor = false;
                }
                screenBuffer.SaveCursor();
            }
            var hasdesc = screenBuffer.WriteLineDescriptionMultiSelect(_options, _localpaginator.SelectedItem);
            if (first && hasdesc)
            {
                first = false;
            }
            var subset = _localpaginator.GetPageData();
            foreach (var item in subset)
            {
                if (first)
                {
                    screenBuffer.SaveCursor();
                }
                string value;
                var indentgroup = string.Empty;
                if (item.IsGroupHeader)
                {
                    value = item.Group;
                }
                else
                {
                    value = item.Text;
                    if (!string.IsNullOrEmpty(item.Group))
                    {
                        if (item.IsLastItemGroup)
                        {
                            indentgroup = $" {_options.Symbol( SymbolType.IndentEndGroup)}";
                        }
                        else
                        {
                            indentgroup = $" {_options.Symbol(SymbolType.IndentGroup)}";
                        }
                    }
                }
 
                if (item.IsCheck)
                {
                    if (_localpaginator.TryGetSelected(out var selectedItem) && EqualityComparer<ItemMultSelect<T>>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineIndentCheckSelect(_options, indentgroup,!first);
                        screenBuffer.WriteCheckValueSelect(_options, value);
                    }
                    else
                    {
                        if (item.Disabled)
                        {
                            screenBuffer.WriteLineIndentCheckUnSelect(_options, indentgroup, !first);
                            screenBuffer.WriteCheckedValueDisabled(_options, value);
                        }
                        else
                        {
                            screenBuffer.WriteLineIndentCheckNotSelect(_options, indentgroup,!first);
                            screenBuffer.WriteCheckValueNotSelect(_options, value);
                        }
                    }
                }
                else
                {
                    if (_localpaginator.TryGetSelected(out var selectedItem) && EqualityComparer<ItemMultSelect<T>>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineIndentUncheckedSelect(_options, indentgroup,!first);
                        screenBuffer.WriteUncheckedValueSelect(_options, value);
                    }
                    else
                    {
                        if (item.Disabled)
                        {
                            screenBuffer.WriteLineIndentUncheckedDisabled(_options, indentgroup,!first);
                            screenBuffer.WriteUncheckedValueDisabled(_options, value);
                        }
                        else
                        {
                            screenBuffer.WriteLineIndentUncheckedNotSelect(_options, indentgroup,!first);
                            screenBuffer.WriteUncheckedValueNotSelect(_options, value);
                        }
                    }
                }
                first = false;
            }
            if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePaginationMultiSelect(_options, _localpaginator.PaginationMessage(_options.OptPaginationTemplate), _selectedItems.Count);
            }
            else
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{_options.Symbol(SymbolType.Selected)}: {_selectedItems.Count}", _options.StyleContent(StyleControls.TaggedInfo), true);
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            if (_options.ShowGroupTip && !string.IsNullOrEmpty(_localpaginator.SelectedItem?.Group ?? string.Empty))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(_localpaginator.SelectedItem.Group, _options.StyleContent(StyleControls.GroupTip));
            }
            screenBuffer.WriteLineTooltipsMultiSelect(_options);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<T> result, bool aborted)
        {
            string answer = string.Join(", ", result.Select(x => _options.TextSelector(x)));
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            else
            {
                SaveHistory(result);
            }
            if (_options.OptMinimalRender)
            {
                return;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<IEnumerable<T>> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            var tryagain = false;
            do
            {
                tryagain = false;
                ClearError();
                var keyInfo = WaitKeypress(cancellationToken);
                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                    break;
                }
                if (CheckAbortKey(keyInfo.Value))
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    continue;
                }
                if (IskeyPageNavegator(keyInfo.Value, _localpaginator))
                {
                    continue;
                }
                else if (_options.SelectAllPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    var qtd = _options.Items.Count(x => !x.IsGroupHeader && !x.Disabled);
                    if (qtd <= _options.Maximum)
                    {
                        foreach (var item in _options.Items)
                        {
                            if (!item.Disabled)
                            {
                                item.IsCheck = true;
                            }
                            if (!item.IsGroupHeader && !_selectedItems.Contains(item))
                            {
                                if (!item.Disabled)
                                {
                                    _selectedItems.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                    }
                }
                else if (_options.InvertSelectedPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    var qtd = _options.Items.Count(x => !x.IsGroupHeader && !x.Disabled);
                    var qtdsel = _selectedItems.Count;
                    var diff = qtd - qtdsel;
                    if (diff <= _options.Maximum)
                    {
                        foreach (var item in _options.Items.Where(x => !x.IsGroupHeader))
                        {
                            if (!item.Disabled)
                            {
                                item.IsCheck = !item.IsCheck;
                            }
                            if (!item.IsCheck)
                            {
                                _selectedItems.Remove(item);
                            }
                            else
                            {
                                if (!_selectedItems.Select(x => x.Value).Any(x => x.Equals(item.Value)))
                                {
                                    _selectedItems.Add(item);
                                }
                            }
                        }
                        var grps = _options.Items.Where(x => x.IsGroupHeader).Select(x => x.Group);
                        foreach (var item in grps)
                        {
                            var cntgrp = _options.Items.Count(x => !x.IsGroupHeader && x.Group == item && !x.Disabled);
                            var cntSel = _options.Items.Count(x => !x.IsGroupHeader && x.Group == item && x.IsCheck);
                            var hg = _options.Items.First(x => x.IsGroupHeader && x.Group == item);
                            hg.IsCheck = (cntgrp == cntSel);
                        }
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                    }
                }
                else if (keyInfo.Value.IsPressSpaceKey())
                {
                    _localpaginator.TryGetSelected(out var currentItem);
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString(), Optional<ItemMultSelect<T>>.Set(currentItem));
                    if (currentItem != null)
                    {
                        if (currentItem.IsGroupHeader)
                        {
                            var grp = currentItem.Group;
                            var maxgrp = _options.Items.Count(x => x.Group == currentItem.Group && !x.IsGroupHeader && !x.Disabled);
                            var selgrp = _selectedItems.Count(y => _options.Items.Where(x => x.Group == currentItem.Group && !x.IsGroupHeader && !x.Disabled).Contains(y));
                            if (maxgrp == selgrp)
                            {
                                var aux = _selectedItems.Where(x => x.Group == currentItem.Group && !x.IsGroupHeader && !x.Disabled).ToArray();
                                foreach (var item in aux)
                                {
                                    var index = _selectedItems.FindIndex(x => x.Value.Equals(item.Value));
                                    _selectedItems.RemoveAt(index);
                                }
                                foreach (var item in _options.Items.Where(x => x.Group == grp && !x.Disabled))
                                {
                                    item.IsCheck = false;
                                }
                            }
                            else
                            {
                                var aux = _options.Items.Where(x => x.Group == currentItem.Group && !x.IsGroupHeader && !x.Disabled);
                                var qtd = _selectedItems.Count;
                                qtd += aux.Sum(x => _selectedItems.FindIndex(s => s.Value.Equals(x.Value)) < 0 ? 1 : 0);

                                if (qtd > _options.Maximum)
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
                                    var auxsel = _options.Items.Where(x => _selectedItems.Select(x => x.Value).Contains(x.Value)).ToArray();
                                    _selectedItems.Clear();
                                    _selectedItems.AddRange(auxsel);
                                    foreach (var item in _options.Items.Where(x => x.Group == grp && !x.Disabled))
                                    {
                                        item.IsCheck = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var index = _selectedItems.FindIndex(x => x.Value.Equals(currentItem.Value));
                            if (index >= 0)
                            {
                                _selectedItems.RemoveAt(index);
                                currentItem.IsCheck = false;
                                if (!string.IsNullOrEmpty(currentItem.Group))
                                {
                                    var hg = _options.Items.First(x => x.IsGroupHeader && x.Group == currentItem.Group);
                                    hg.IsCheck = false;
                                }
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
                                    var auxsel = _options.Items.Where(x => _selectedItems.Select(x => x.Value).Contains(x.Value)).ToArray();
                                    _selectedItems.Clear();
                                    _selectedItems.AddRange(auxsel);
                                    currentItem.IsCheck = true;
                                    if (!string.IsNullOrEmpty(currentItem.Group))
                                    {
                                        var cntgrp = _options.Items.Count(x => !x.IsGroupHeader && x.Group == currentItem.Group && !x.Disabled);
                                        var cntSel = _options.Items.Count(x => !x.IsGroupHeader && x.Group == currentItem.Group && x.IsCheck);
                                        if (cntgrp == cntSel)
                                        {
                                            var hg = _options.Items.First(x => x.IsGroupHeader && x.Group == currentItem.Group);
                                            hg.IsCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (_options.FilterType != FilterMode.Disabled && _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                }
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    if (_selectedItems.Count >= _options.Minimum)
                    {
                        _filterBuffer.Clear();
                        _localpaginator.UpdateFilter(_filterBuffer.ToString());
                        endinput = true;
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMinSelection, _options.Minimum));
                    }
                    break;
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (KeyAvailable)
                        {
                            tryagain = true;
                        }
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                _filterBuffer.Clear();
                _localpaginator.UpdateFilter(_filterBuffer.ToString());
                _localpaginator.UnSelected();
                endinput = true;
                abort = true;
            }
            FinishResult = string.Empty;
            if (_selectedItems.Count > 0)
            {
                FinishResult = string.Join(", ", _selectedItems.Select(x => x.Text));
            }
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }

            return new ResultPrompt<IEnumerable<T>>(_selectedItems.Select(x => x.Value), abort, !endinput, notrender);
        }

        private void AddEnum()
        {

            var aux = Enum.GetValues(typeof(T));
            var result = new List<Tuple<int, ItemMultSelect<T>>>();
            foreach (var item in aux)
            {
                var name = item.ToString();
                var displayAttribute = typeof(T).GetField(name)?.GetCustomAttribute<DisplayAttribute>();
                var order = displayAttribute?.GetOrder() ?? int.MaxValue;
                result.Add(new Tuple<int, ItemMultSelect<T>>(order, new ItemMultSelect<T> { Value = (T)item, Text = _options.TextSelector((T)item) }));
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
            return displayAttribute?.GetName() ?? name;
        }

        private void LoadHistory()
        {
            _defaultHistoric = Optional<IList<T>>.Empty();
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = Optional<IList<T>>.Set(JsonSerializer.Deserialize<IList<T>>(aux[0].History));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveHistory(IEnumerable<T> value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = JsonSerializer.Serialize<IEnumerable<T>>(value);
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(aux, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }

        private bool IsDisabled(T item)
        {
            return _options.DisableItems.Any(x => x.Equals(item));
        }
    }
}
