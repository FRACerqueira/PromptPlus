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

namespace PPlus.Controls
{
    internal class SelectControl<T> : ControlBase<T>, IControlSelect<T>
    {
        private readonly SelectOptions<T> _options;
        private readonly ReadLineBuffer _filterBuffer = new();
        private Paginator<T> _localpaginator;
        private bool _autoselect = false;
        private const string Namecontrol = "PromptPlus.Select";

        public SelectControl(SelectOptions<T> options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override T InitControl()
        {
            if (_options.Items is null)
            {
                throw new ArgumentNullException(nameof(_options.Items));
            }
            if (typeof(T).IsEnum && _options.Items.Count == 0)
            {
                AddEnum();
            }
            if (_options.HideItems.Count > 0)
            {
                foreach (var item in _options.HideItems)
                {
                    _options.Items.Remove(item);
                }
            }
            _options.PageSize ??= _options.Items.Count;
            _options.TextSelector ??= (x => x?.ToString());
            T result = default;
            if (IsDisabled(_options.DefaultValue))
            {
                _localpaginator = new Paginator<T>(_options.Items, _options.PageSize, Optional<T>.s_empty, _options.TextSelector, IsNotDisabled);
            }
            else
            {
                result = _options.DefaultValue;
                _localpaginator = new Paginator<T>(_options.Items, _options.PageSize, Optional<T>.Create(_options.DefaultValue), _options.TextSelector, IsNotDisabled);
            }
            _localpaginator.FirstItem();
            if (_options.DescriptionSelector != null)
            {
                _options.Description = _options.DescriptionSelector.Invoke(_localpaginator.SelectedItem);
            }

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("DisableItems", _options.DisableItems.Count.ToString(), LogKind.Property);
                AddLog("HideItems", _options.HideItems.Count.ToString(), LogKind.Property);
                AddLog("Items", _options.Items.Count.ToString(), LogKind.Property);
                AddLog("AutoSelectIfOne", _options.AutoSelectIfOne.ToString(), LogKind.Property);
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
            }
            return result;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out T result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                ConsoleKeyInfo keyInfo;
                if (_autoselect)
                {
                    keyInfo = new ConsoleKeyInfo((char)13, ConsoleKey.Enter, false, false, false);
                }
                else
                {
                    keyInfo = WaitKeypress(cancellationToken);
                    _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo,_options.Items, out var acceptedkey);
                    if (acceptedkey)
                    {
                        _localpaginator.UpdateFilter(_filterBuffer.ToString());
                        if (_localpaginator.Count == 1 && _options.AutoSelectIfOne)
                        {
                            _autoselect = true;
                        }
                        else
                        {
                            _autoselect = false;
                        }
                        continue;
                    }
                }
                if (CheckDefaultKey(keyInfo))
                {
                    ///none
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    ///none
                }
                else if (PromptPlus.UnSelectFilter.Equals(keyInfo))
                {
                    _localpaginator.UnSelected();
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    if (_localpaginator.TryGetSelectedItem(out result))
                    {
                        return true;
                    }
                    if (!typeof(T).IsEnum)
                    {
                        result = default;
                        return true;
                    }
                    SetError(Messages.Required);
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
                    _options.Description = _options.DescriptionSelector.Invoke(_localpaginator.SelectedItem);
                }
            }
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            if (_localpaginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToBackward());
            }
            if (_localpaginator.TryGetSelectedItem(out var result) && !_localpaginator.IsUnSelected)
            {
                var answ = _options.TextSelector(result);
                var aux = _filterBuffer.ToBackward();
                if (answ != aux && _localpaginator.Count == 1)
                {
                    screenBuffer.WriteFilter(aux);
                    screenBuffer.WriteAnswer(answ.Substring(aux.Length));
                }
                else
                {
                    screenBuffer.WriteAnswer(_options.TextSelector(result));
                }
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
                    screenBuffer.WriteHint(Messages.SelectKeyNavigation);

                }

            }

            if (_filterBuffer.Length > 0)
            {
                screenBuffer.WriteLineFilter(Messages.ItemsFiltered);
                screenBuffer.WriteFilter(_filterBuffer.ToString());
            }
            var subset = _localpaginator.ToSubset();
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<T>.Default.Equals(item, selectedItem))
                {
                    if (IsDisabled(item))
                    {
                        screenBuffer.WriteLineSelectorDisabled(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineSelector(value);
                    }
                }
                else
                {
                    if (IsDisabled(item))
                    {
                        screenBuffer.WriteLineNotSelectorDisabled(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(value);
                    }
                }
            }
            if (_localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = _options.TextSelector(result);
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlSelect

        public IControlSelect<T> AutoSelectIfOne()
        {
            _options.AutoSelectIfOne = true;
            return this;
        }

        public IControlSelect<T> Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlSelect<T> Default(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DefaultValue = value;
            return this;
        }

        public IControlSelect<T> PageSize(int value)
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

        public IControlSelect<T> DescriptionSelector(Func<T, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlSelect<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value ?? (x => x.ToString());
            return this;
        }

        public IControlSelect<T> HideItem(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.HideItems.Add(value);
            return this;
        }

        public IControlSelect<T> HideItems(IEnumerable<T> value)
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

        public IControlSelect<T> DisableItem(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DisableItems.Add(value);
            return this;
        }

        public IControlSelect<T> DisableItems(IEnumerable<T> value)
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

        public IControlSelect<T> AddItem(T value)
        {
            if (value == null)
            {
                return this;
            }
            _options.Items.Add(value);
            return this;
        }

        public IControlSelect<T> AddItems(IEnumerable<T> value)
        {
            if (value == null)
            {
                return this;
            }
            foreach (var item in value)
            {
                _options.Items.Add(item);
            }
            return this;
        }

        private bool IsDisabled(T item)
        {
            return _options.DisableItems.Any(x => x.Equals(item));
        }

        private bool IsNotDisabled(T item)
        {
            return !_options.DisableItems.Any(x => x.Equals(item));
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

        public IControlSelect<T> Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion
    }
}
