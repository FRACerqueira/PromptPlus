// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;


namespace PromptPlusControls.Controls
{
    internal class SelectControl<T> : ControlBase<T>, IDisposable
    {
        private readonly SelectOptions<T> _options;
        private readonly InputBuffer _filterBuffer = new();
        private readonly Paginator<T> _localpaginator;

        public SelectControl(SelectOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _localpaginator = new Paginator<T>(options.Items, options.PageSize, Optional<T>.Create(options.DefaultValue), options.TextSelector);
            _localpaginator.FirstItem();
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

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out T result)
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
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _localpaginator.TryGetSelectedItem(out result):
                        return true;
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        if (!typeof(T).Name.StartsWith("EnumValue`"))
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
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            if (_localpaginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToBackwardString());
            }
            if (_localpaginator.TryGetSelectedItem(out var result) && !_localpaginator.IsUnSelected)
            {
                var answ = _options.TextSelector(result);
                var aux = _filterBuffer.ToBackwardString();
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
                    screenBuffer.WriteLineSelector(value);
                }
                else
                {
                    screenBuffer.WriteLineNotSelector(value);
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
    }
}
