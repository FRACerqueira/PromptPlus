// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlus.Internal;
using PromptPlus.Options;
using PromptPlus.Resources;

namespace PromptPlus.Forms
{
    internal class MultiSelectForm<T> : FormBase<IEnumerable<T>>
    {

        private readonly MultiSelectOptions<T> _options;
        private readonly Paginator<T> _localpaginator;
        private readonly List<T> _selectedItems = new();
        private readonly InputBuffer _filterBuffer = new();

        public MultiSelectForm(MultiSelectOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            if (options.Minimum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Minimum), string.Format(Exceptions.Ex_MinArgumentOutOfRange, options.Minimum));
            }

            if (options.Maximum < options.Minimum)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_MaxArgumentOutOfRange, options.Maximum, options.Minimum), nameof(options.Maximum));
            }
            _localpaginator = new Paginator<T>(options.Items, options.PageSize, Optional<T>.s_empty, options.TextSelector);
            _localpaginator.FirstItem();
            if (options.DefaultValues != null)
            {
                _selectedItems.AddRange(options.DefaultValues);
            }
            _options = options;
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out IEnumerable<T> result)
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

                else if (PPlus.UnSelectFilter.Equals(keyInfo))
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
    }
}
