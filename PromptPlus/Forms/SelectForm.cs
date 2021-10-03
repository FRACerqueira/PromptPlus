// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

using PromptPlus.Internal;
using PromptPlus.Options;

namespace PromptPlus.Forms
{
    internal class SelectForm<T> : FormBase<T>
    {
        private readonly SelectOptions<T> _options;
        private readonly InputBuffer _filterBuffer = new();

        public SelectForm(SelectOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            Paginator = new Paginator<T>(options.Items, options.PageSize, Optional<T>.Create(options.DefaultValue), options.TextSelector);
            Paginator.FirstItem();
            _options = options;
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
                else if (IskeyPageNavagator(keyInfo, Paginator))
                {
                    continue;
                }
                else if (PPlus.UnSelectFilter.Equals(keyInfo))
                {
                    Paginator.UnSelected();
                    result = default;
                    return isvalidhit;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && Paginator.TryGetSelectedItem(out result):
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
                        Paginator.UpdateFilter(_filterBuffer.Backward().ToString());
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        Paginator.UpdateFilter(_filterBuffer.Forward().ToString());
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        Paginator.UpdateFilter(_filterBuffer.Backspace().ToString());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        Paginator.UpdateFilter(_filterBuffer.Delete().ToString());
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Paginator.UpdateFilter(_filterBuffer.Insert(keyInfo.KeyChar).ToString());
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
            if (Paginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToBackwardString());
            }
            if (Paginator.TryGetSelectedItem(out var result) && !Paginator.IsUnSelected)
            {
                screenBuffer.WriteAnswer(_options.TextSelector(result));
            }
            screenBuffer.PushCursor();
            if (Paginator.IsUnSelected)
            {
                screenBuffer.Write(_filterBuffer.ToForwardString());
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (Paginator.PageCount > 1)
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
            var subset = Paginator.ToSubset();
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (Paginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<T>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.WriteLineSelector(value);
                }
                else
                {
                    screenBuffer.WriteLineNotSelector(value);
                }
            }
            if (Paginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(Paginator.PaginationMessage());
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
