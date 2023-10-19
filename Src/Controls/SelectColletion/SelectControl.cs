// ***************************************************************************************
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
    internal class SelectControl<T> : BaseControl<T>, IControlSelect<T>
    {
        private readonly SelectOptions<T> _options;
        private Paginator<ItemSelect<T>> _localpaginator;
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase,modefilter:true);
        private Optional<T> _defaultHistoric = Optional<T>.Create(null);
        private bool ShowingFilter => _filterBuffer.Length > 0;
        private int _lengthSeparationline;

        public SelectControl(IConsoleControl console, SelectOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        #region IControlSelect

        public IControlSelect<T> Separator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
        {
            if (_options.OrderBy != null)
            {
                throw new PromptPlusException("Separator cannot be used OrderBy/OrderByDescending");
            }
            switch (separatorLine)
            {
                case SeparatorLine.DoubleLine:
                case SeparatorLine.SingleLine:
                    _options.Items.Add(new ItemSelect<T> 
                    { 
                        Disabled = true, 
                        IsGroupHeader = true, 
                        IsSeparator = true,
                        SeparatorType = separatorLine,
                    });
                    break;
                case SeparatorLine.Char:
                    if (!value.HasValue)
                    {
                        throw new PromptPlusException($"char value is empty");
                    }
                    _options.Items.Add(new ItemSelect<T>
                    {
                        Disabled = true,
                        IsGroupHeader = true,
                        IsSeparator = true,
                        SeparatorType = separatorLine,
                        CharSeparation = value.Value
                    });
                    break;
                default:
                    throw new PromptPlusException($"Not implemented SeparatorLine: {separatorLine}");
            }
            return this;
        }

        public IControlSelect<T> ShowTipGroup(bool value = true)
        {
            _options.ShowGroupTip = value;
            return this;
        }

        public IControlSelect<T> AddItemGrouped(string group, T value, bool disable = false)
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
                    _options.Items.Add(new ItemSelect<T> { Value = value, Group = group, IsLastItemGroup = true, Disabled = disable});
                    break;
                }
            }
            if (!found || !added)
            {
                if (!found)
                {
                    _options.Items.Add(new ItemSelect<T> { IsGroupHeader = true, Text = null, Value = default, Group = group, Disabled = disable});
                }
                _options.Items.Add(new ItemSelect<T> { Value = value, Group = group, IsLastItemGroup = true, Disabled = disable});
            }
            return this;
        }

        public IControlSelect<T> AddItemsGrouped(string group, IEnumerable<T> value, bool disable = false)
        {
            if (_options.OrderBy != null)
            {
                throw new PromptPlusException("AddItemsGrouped cannot be used OrderBy/OrderByDescending");
            }
            foreach (var item in value)
            {
                AddItemGrouped(group, item, disable);
            }
            return this;
        }

        public IControlSelect<T> OrderBy(Expression<Func<T, object>> value)
        {
            if (_options.Items.Any(x => x.IsGroupHeader))
            {
                throw new PromptPlusException("OrderBy cannot be used Separator or Grouped item");
            }
            _options.IsOrderDescending = false;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlSelect<T> OrderByDescending(Expression<Func<T, object>> value)
        {
            if (_options.Items.Any(x => x.IsGroupHeader))
            {
                throw new PromptPlusException("OrderByDescending cannot be used Separator or Grouped item");
            }
            _options.IsOrderDescending = true;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlSelect<T>, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }
        public IControlSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout)
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

        public IControlSelect<T> AddItem(T value, bool disable = false)
        {
            _options.Items.Add(new ItemSelect<T> { Value = value, Disabled = disable });
            return this;
        }

        public IControlSelect<T> EqualItems(Func<T, T, bool> comparer)
        {
            _options.EqualItems = comparer;
            return this;
        }


        public IControlSelect<T> AddItems(IEnumerable<T> value, bool disable = false)
        {
            foreach (var item in value)
            {
                AddItem(item, disable);
            }
            return this;
        }

        public IControlSelect<T> AddItemsTo(AdderScope scope, IEnumerable<T> values)
        {
            InternalAdd(scope, values);
            return this;
        }


        public IControlSelect<T> AddItemsTo(AdderScope scope,params T[] values)
        {
            InternalAdd(scope,values);
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
                        throw new PromptPlusException($"AddItemsTo : {scope} Not Implemented");
                }
            }
        }

        public IControlSelect<T> AutoSelect(bool value = true)
        {
            _options.AutoSelect = value;
            return this;
        }

        public IControlSelect<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlSelect<T> Default(T value)
        {
            _options.DefaultValue = Optional<T>.Create(value);
            return this;
        }

        public IControlSelect<T> ChangeDescription(Func<T, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlSelect<T> FilterType(FilterMode value)
        {
            _options.FilterType = value;
            return this;
        }

        public IControlSelect<T> PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlSelect<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value;
            return this;
        }

        #endregion

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
                List<ItemSelect<T>> founds;
                founds = _options.Items.FindAll(x => !x.IsGroupHeader && _options.EqualItems(x.Value, item));
                if (founds.Any())
                {
                    foreach (var itemfound in founds)
                    {
                        itemfound.Disabled = true;
                    }
                }
            }

            var maxlensep = 0;
            foreach (var item in _options.Items)
            {
                if (item.IsGroupHeader && !item.IsSeparator)
                {
                    if (maxlensep < item.Group.Length)
                    { 
                        maxlensep = item.Group.Length;
                    }
                }
                else if (!item.IsGroupHeader)
                {
                    if (maxlensep < item.Text.Length)
                    {
                        maxlensep = item.Text.Length;
                    }
                }
            }
            _lengthSeparationline = maxlensep;

            Optional<T> defvalue = Optional<T>.s_empty;

            Optional<ItemSelect<T>> defvaluepage = Optional<ItemSelect<T>>.s_empty;

            if (_options.DefaultValue.HasValue)
            { 
                defvalue = Optional<T>.Create(_options.DefaultValue.Value);
            }
            if (_defaultHistoric.HasValue)
            {
                defvalue = Optional<T>.Create(_defaultHistoric.Value);
            }

            if (defvalue.HasValue)
            {
                var found = _options.Items.FirstOrDefault(x => !x.IsGroupHeader && _options.EqualItems(x.Value, defvalue.Value));
                if (found != null && !found.Disabled)
                {
                    defvaluepage = Optional<ItemSelect<T>>.Create(found);
                }
            }

            if (_options.OrderBy != null)
            {
                if (_options.IsOrderDescending)
                {
                    _options.Items = _options.Items.Where(x => !x.IsSeparator).OrderByDescending(x => x.Group ?? string.Empty + _options.OrderBy.Invoke(x.Value) ?? x.Text).ToList();
                }
                else
                {
                    _options.Items = _options.Items.Where(x => !x.IsSeparator).OrderBy(x => x.Group ?? string.Empty +  _options.OrderBy.Invoke(x.Value) ?? x.Text).ToList();
                }
            }

            _localpaginator = new Paginator<ItemSelect<T>>(
                _options.FilterType,
                _options.Items, 
                _options.PageSize,
                defvaluepage,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.Text??string.Empty, 
                (item) => !item.IsGroupHeader && IsEnnabled(item),
                (item) => !item.IsGroupHeader);

            if (_localpaginator.TotalCountValid > 0 &&  _localpaginator.SelectedItem != null && (_localpaginator.SelectedItem.Disabled || _localpaginator.SelectedItem.IsGroupHeader)) 
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

            FinishResult = string.Empty;
            if (!_localpaginator.IsUnSelected)
            {
                FinishResult = _localpaginator.SelectedItem.Text;
            }
            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            var first = !string.IsNullOrEmpty(_options.OptPrompt);
            if (ShowingFilter)
            {
                first = false;
                screenBuffer.WriteFilterSelect(_options, FinishResult, _filterBuffer);
                screenBuffer.SaveCursor();
                if (!_options.OptMinimalRender)
                {
                    screenBuffer.WriteTaggedInfo(_options, $" ({Messages.Filter})");
                }
                _options.OptShowCursor = true;
            }
            else
            {
                if (!_options.OptMinimalRender)
                {
                    first = false;
                    screenBuffer.WriteAnswer(_options, FinishResult);
                    screenBuffer.SaveCursor();
                }
                else
                {
                    _options.OptShowCursor = false;
                }
            }
            var hasdesc  = screenBuffer.WriteLineDescriptionSelect(_options,_localpaginator.SelectedItem);
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
                    if (item.IsSeparator)
                    {
                        value = new string('-', _lengthSeparationline);
                        switch (item.SeparatorType.Value)
                        {
                            case SeparatorLine.SingleLine:
                                value = new string(_options.Symbol(SymbolType.SingleBorder)[0], _lengthSeparationline);
                                break;
                            case SeparatorLine.DoubleLine:
                                value = new string(_options.Symbol(SymbolType.DoubleBorder)[0], _lengthSeparationline);
                                break;
                            case SeparatorLine.Char:
                                value = new string(item.CharSeparation.Value, _lengthSeparationline);
                                break;
                        }
                    }
                    else
                    {
                        value = item.Group;
                    }
                }
                else
                {
                    value = item.Text;
                    if (!string.IsNullOrEmpty(item.Group))
                    {
                        if (item.IsLastItemGroup)
                        {
                            indentgroup = $" {_options.Symbol(SymbolType.IndentEndGroup)}";
                        }
                        else
                        {
                            indentgroup = $" {_options.Symbol(SymbolType.IndentGroup)}";
                        }
                    }
                }
                if (_options.OptMinimalRender)
                {
                    screenBuffer.SaveCursor();
                }
                if (_localpaginator.TryGetSelected(out var selectedItem) && EqualityComparer<ItemSelect<T>>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.WriteLineSelector(_options, value, indentgroup,!first);
                }
                else
                {
                    if (IsDisabled(item))
                    {
                        screenBuffer.WriteLineNotSelectorDisabled(_options, value,indentgroup,!first);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, value, indentgroup, !first);
                    }
                }
                first =false;
            }
            if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage(_options.OptPaginationTemplate));
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            if (_options.ShowGroupTip && !string.IsNullOrEmpty(_localpaginator.SelectedItem?.Group ?? string.Empty))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(_localpaginator.SelectedItem.Group, _options.OptStyleSchema.Tooltips());
            }
            screenBuffer.WriteLineTooltipsSelect(_options);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T result, bool aborted)
        {
            string answer = _options.TextSelector(result);
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            if (!aborted)
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

        public override ResultPrompt<T> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            bool tryagain;
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
                else if (_options.FilterType != FilterMode.Disabled && _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    if (_localpaginator.Count == 1 && !_localpaginator.IsUnSelected && _options.AutoSelect)
                    {
                        endinput = true;
                    }
                }
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    if (_localpaginator.SelectedIndex >= 0)
                    {
                        if (!_localpaginator.SelectedItem.Disabled)
                        {
                            endinput = true;
                            break;
                        }
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMinSelection,1));
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
            if (_localpaginator.SelectedIndex >= 0)
            {
                FinishResult = _localpaginator.SelectedItem.Text;
                return new ResultPrompt<T>(_localpaginator.SelectedItem.Value, abort, !endinput);
            }
            else
            {
                endinput = false;
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
            return new ResultPrompt<T>(default, abort, !endinput, notrender);
        }

        private void SaveHistory(T value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = JsonSerializer.Serialize<T>(value);
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist =  FileHistory.AddHistory(aux, _options.TimeoutOverwriteDefault,null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }

        private void LoadHistory()
        {
            _defaultHistoric = Optional<T>.Create(null);
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = Optional<T>.Create(JsonSerializer.Deserialize<T>(aux[0].History));
                    }
                    catch
                    {
                        //invalid Deserialize history 
                    }
                }
            }
        }

        private bool IsDisabled(ItemSelect<T> item)
        {
            return _options.Items.Any(x => x.UniqueId == item.UniqueId && x.Disabled);
        }

        private bool IsEnnabled(ItemSelect<T> item)
        {
            return !IsDisabled(item);
        }

        private void AddEnum()
        {

            var aux = Enum.GetValues(typeof(T));
            var result = new List<Tuple<int, ItemSelect<T>>>();
            foreach (var item in aux)
            {
                var name = item.ToString();
                var displayAttribute = typeof(T).GetField(name)?.GetCustomAttribute<DisplayAttribute>();
                var order = displayAttribute?.GetOrder() ?? int.MaxValue;
                result.Add(new Tuple<int, ItemSelect<T>>(order, new ItemSelect<T> { Value = (T)item, Text = _options.TextSelector((T)item) }));
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
    }
}
