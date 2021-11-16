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

using PromptPlusInternal;

using PromptPlusObjects;

namespace PromptPlusControls
{
    internal class SelectControl<T> : ControlBase<T>, IControlSelect<T>
    {
        private readonly SelectOptions<T> _options;
        private readonly InputBuffer _filterBuffer = new();
        private Paginator<T> _localpaginator;
        private bool _autoselect = false;
        public SelectControl(SelectOptions<T> options) : base(options, true)
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
            if (_options.HideItems.Count > 0)
            {
                foreach (var item in _options.HideItems)
                {
                    _options.Items.Remove(item);
                }
            }
            _options.PageSize ??= _options.Items.Count;
            _options.TextSelector ??= (x => x?.ToString());
            if (IsDisabled(_options.DefaultValue))
            {
                _localpaginator = new Paginator<T>(_options.Items, _options.PageSize, Optional<T>.s_empty, _options.TextSelector, IsNotDisabled);
            }
            else
            {
                _localpaginator = new Paginator<T>(_options.Items, _options.PageSize, Optional<T>.Create(_options.DefaultValue), _options.TextSelector, IsNotDisabled);
            }
            _localpaginator.FirstItem();
            if (_options.DescriptionSelector != null)
            {
                _options.Description = _options.DescriptionSelector.Invoke(_localpaginator.SelectedItem);
            }
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
                }

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
                    continue;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _localpaginator.TryGetSelectedItem(out result):
                        return true;
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        if (!typeof(T).IsEnum)
                        {
                            result = default;
                            return true;
                        }
                        SetError(Messages.Required);
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
                if (_localpaginator.Count == 1 && _options.AutoSelectIfOne)
                {
                    _autoselect = true;
                }
                else
                {
                    _autoselect = false;
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
            _options.HideItems.Add(value);
            return this;
        }

        public IControlSelect<T> HideItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.HideItems.Add(item);
            }
            return this;
        }

        public IControlSelect<T> DisableItem(T value)
        {
            _options.DisableItems.Add(value);
            return this;
        }

        public IControlSelect<T> DisableItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.DisableItems.Add(item);
            }
            return this;
        }

        public IControlSelect<T> AddItem(T value)
        {
            _options.Items.Add(value);
            return this;
        }

        public IControlSelect<T> AddItems(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                _options.Items.Add(item);
            }
            return this;
        }

        private bool IsDisabled(T item)
        {
            return _options.DisableItems.Contains(item);
        }

        private bool IsNotDisabled(T item)
        {
            return !_options.DisableItems.Contains(item);
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

        public IPromptControls<T> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<T> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<T> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<T> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<T> Run(CancellationToken? value = null)
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
