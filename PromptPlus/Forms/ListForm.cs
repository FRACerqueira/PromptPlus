// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;
using PromptPlusControls.Resources;

namespace PromptPlusControls.Forms
{
    internal class ListForm<T> : FormBase<IEnumerable<T>>
    {
        private Paginator<T> _localpaginator;
        private readonly ListOptions<T> _options;
        private readonly Type _targetType = typeof(T);
        private readonly Type _underlyingType = Nullable.GetUnderlyingType(typeof(T));
        private readonly InputBuffer _inputBuffer = new();
        private readonly List<T> _inputItems = new();

        public ListForm(ListOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            if (options.Minimum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Minimum), string.Format(Exceptions.Ex_MinArgumentOutOfRange, options.Minimum));
            }

            if (options.Maximum < options.Minimum)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_MaxArgumentOutOfRange, options.Maximum, options.Minimum), nameof(options.Maximum));
            }
            _options = options;
            _localpaginator = new Paginator<T>(_inputItems, options.PageSize, Optional<T>.s_empty, options.TextSelector);
            _localpaginator.FirstItem();
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

                else if (PromptPlus.RemoveAll.Equals(keyInfo))
                {
                    var aux = _inputItems.Where(x => _options.TextSelector(x).IndexOf(_inputBuffer.ToString(), StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                    _inputItems.RemoveAll(x => aux.Contains(x));
                    _inputBuffer.Clear();
                    _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                    _localpaginator.FirstItem();
                    result = _inputItems;
                    return false;
                }


                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        var input = _inputBuffer.ToString();
                        try
                        {
                            result = _inputItems;

                            if (string.IsNullOrEmpty(input))
                            {
                                if (_inputItems.Count >= _options.Minimum)
                                {
                                    return true;
                                }
                                SetError(string.Format(Messages.ListMinSelection, _options.Minimum));
                                return false;
                            }

                            if (_inputItems.Count >= _options.Maximum)
                            {
                                SetError(string.Format(Messages.ListMaxSelection, _options.Maximum));
                                return false;
                            }

                            T inputValue;
                            Thread.CurrentThread.CurrentUICulture = _options.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = _options.CurrentCulture;
                            inputValue = TypeHelper<T>.ConvertTo(input);
                            if (!TryValidate(inputValue, _options.Validators))
                            {
                                Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
                                Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
                                result = _inputItems;
                                return false;
                            }
                            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
                            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;

                            if (!_options.AllowDuplicate)
                            {
                                if (_inputItems.Contains(inputValue))
                                {
                                    SetError(Messages.ListItemAlreadyexists);
                                    return false;
                                }
                            }
                            _inputBuffer.Clear();
                            _inputItems.Add(inputValue);
                            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                            result = _inputItems;
                            return false;
                        }
                        catch (FormatException)
                        {
                            SetError(PromptPlus.LocalizateFormatException(typeof(T)));
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _localpaginator.UpdateFilter(_inputBuffer.Backward().ToString());
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_inputBuffer.Forward().ToString());
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _localpaginator.UpdateFilter(_inputBuffer.Backspace().ToString());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_inputBuffer.Delete().ToString());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        if (_localpaginator.TryGetSelectedItem(out var selected))
                        {
                            var inputValue = (T)Convert.ChangeType(selected, _underlyingType ?? _targetType);

                            if (!TryValidate(inputValue, _options.Validators))
                            {
                                result = _inputItems;
                                return false;
                            }

                            if (_inputItems.Contains(inputValue))
                            {
                                _inputItems.Remove(inputValue);
                            }

                            _inputBuffer.Clear();
                            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                            _localpaginator.FirstItem();
                            result = _inputItems;
                            return false;
                        }
                        break;
                    }
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _localpaginator.UpdateFilter(_inputBuffer.Insert(_options.UpperCase ? char.ToUpper(keyInfo.KeyChar) : keyInfo.KeyChar).ToString());
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

            screenBuffer.PushCursor(_inputBuffer);

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
                    screenBuffer.WriteHint(Messages.ListKeyNavigation);
                }
            }

            if (_inputBuffer.Length > 0)
            {
                screenBuffer.WriteLineFilter(Messages.ItemsFiltered);
                screenBuffer.WriteFilter($" ({_inputBuffer})");
            }
            var subset = _localpaginator.ToSubset();
            var index = 0;
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (_inputBuffer.Length == 0 || EqualityComparer<string>.Default.Equals(_inputBuffer.ToString(), value))
                {
                    if (_localpaginator.SelectedIndex == index)
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
                    if (_localpaginator.SelectedIndex == index)
                    {
                        screenBuffer.WriteLineSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelect(value);
                    }
                }
                index++;
            }
            if (_localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<T> result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = string.Join(", ", result);
            screenBuffer.WriteAnswer(FinishResult);
        }
    }
}
