// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class ListControl<T> : ControlBase<IEnumerable<T>>, IDisposable, IControlList<T>
    {
        private readonly ListOptions<T> _options;
        private readonly Type _targetType = typeof(T);
        private readonly Type _underlyingType = Nullable.GetUnderlyingType(typeof(T));
        private readonly InputBuffer _inputBuffer = new();
        private readonly List<T> _inputItems = new();
        private Paginator<T> _localpaginator;

        public ListControl(ListOptions<T> options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
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

        public override void InitControl()
        {
            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
            _localpaginator.FirstItem();
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
                            inputValue = TypeHelper<T>.ConvertTo(input);
                            if (!TryValidate(inputValue, _options.Validators))
                            {
                                result = _inputItems;
                                return false;
                            }
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
            FinishResult = string.Format(Messages.FinishResultList, result.Count());
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlList

        public IControlList<T> Prompt(string value)
        {
            _options.Message = value;
            return this;
        }

        public IControlList<T> PageSize(int value)
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

        public IControlList<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value ?? (x => x.ToString());
            return this;
        }

        public IControlList<T> Range(int minvalue, int maxvalue)
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

        public IControlList<T> UpperCase(bool value)
        {
            _options.UpperCase = value;
            return this;
        }

        public IControlList<T> AllowDuplicate(bool value)
        {
            _options.AllowDuplicate = value;
            return this;
        }

        public IControlList<T> Addvalidator(Func<object, ValidationResult> validator)
        {
            return Addvalidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlList<T> Addvalidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            _options.Validators.Merge(validators);
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
            return Start(value ?? CancellationToken.None);
        }


        public IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
        {
            PipeCondition = condition;
            return this;
        }

        public IFormPlusBase AddPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }
        #endregion
    }
}
