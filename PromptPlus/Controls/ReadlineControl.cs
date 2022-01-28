using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class ReadlineControl : ControlBase<string>, IControlReadline
    {
        private const string Namecontrol = "PromptPlus.Readline";
        private readonly ReadlineOptions _options;
        private Paginator<ItemHistory> _localpaginator;
        private ReadLineBuffer _inputBuffer;
        private IList<ItemHistory> _itemsHistory = new List<ItemHistory>();
        private bool _showingHistory;
        private string _originalText;

        public ReadlineControl(ReadlineOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override string InitControl()
        {
            if (string.IsNullOrEmpty(_options.FileNameHistory?.Trim()) && _options.EnabledHistory)
            {
                throw new NotImplementedException(string.Format(Exceptions.Ex_InvalidValue, nameof(_options.FileNameHistory)));
            }

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("MinimumPrefixLength", _options.MinimumPrefixLength.ToString(), LogKind.Property);
                AddLog("FinishWhenHistoryEnter", _options.FinishWhenHistoryEnter.ToString(), LogKind.Property);
                AddLog("AcceptInputTab", _options.AcceptInputTab.ToString(), LogKind.Property);
                AddLog("EnabledHistory", _options.EnabledHistory.ToString(), LogKind.Property);
                AddLog("PrefixFileNameHistory", _options.FileNameHistory, LogKind.Property);
                AddLog("SuggestionHandler", (_options.SuggestionHandler != null).ToString(), LogKind.Property);
                AddLog("MaxHistory", _options.MaxHistory.ToString(), LogKind.Property);
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
                AddLog("TimeoutHistory", _options.TimeoutHistory.ToString(), LogKind.Property);
                AddLog("Validators", _options.Validators.Count.ToString(), LogKind.Property);
            }

            _inputBuffer = new ReadLineBuffer(_options.AcceptInputTab, _options.SuggestionHandler);

            if (_options.EnabledHistory)
            {
                LoadHistory();
            }

            if (!string.IsNullOrEmpty(_options.InitialValue))
            {
                _inputBuffer.LoadPrintable(_options.InitialValue);
            }
            if (_options.InitialError is not null)
            {
                SetError(_options.InitialError);
            }
            if (_options.Context is null)
            {
                _options.Context = _inputBuffer.ToString();
            }
            return _inputBuffer.ToString();
        }

        private void AddAndSaveHistory(string value)
        {
            FileHistory.AddHistory(value, _options.TimeoutHistory, _itemsHistory);
            FileHistory.SaveHistory(_options.FileNameHistory, _itemsHistory);
        }

        private void LoadHistory()
        {
            _itemsHistory = FileHistory.LoadHistory(_options.FileNameHistory);
        }

        public override bool? TryResult(bool IsSummary, CancellationToken stoptoken, out string result)
        {
            bool? isvalidhit = false;
            if (IsSummary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(stoptoken);
                _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo, _options.Context, out var acceptedkey);
                if (acceptedkey)
                {
                    _showingHistory = false;
                    _originalText = null;
                    if (_inputBuffer.SugestionError is not null)
                    {
                        SetError(_inputBuffer.SugestionError);
                        _inputBuffer.ClearError();
                    }
                }
                else if (!_showingHistory && (keyInfo.IsPressDownArrowKey() || keyInfo.IsPressPageDownKey())
                    && _itemsHistory.Count > 0 && _inputBuffer.Length >= _options.MinimumPrefixLength)
                {
                    _originalText = _inputBuffer.ToString();
                    if (_inputBuffer.Length > 0)
                    {
                        _localpaginator = new Paginator<ItemHistory>(_itemsHistory
                            .Where(x => x.History.StartsWith(_inputBuffer.ToString(), StringComparison.InvariantCultureIgnoreCase)
                                && DateTime.Now < new DateTime(x.TimeOutTicks)),
                            _options.PageSize, Optional<ItemHistory>.s_empty, (item) => item.History);
                    }
                    else
                    {
                        _localpaginator = new Paginator<ItemHistory>(_itemsHistory
                            .Where(x => DateTime.Now < new DateTime(x.TimeOutTicks)),
                            _options.PageSize, Optional<ItemHistory>.s_empty, (item) => item.History);
                    }
                    if (_localpaginator.Count == 0)
                    {
                        _localpaginator = null;
                    }
                    else
                    {
                        _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                        _showingHistory = true;
                    }
                }
                else if (_showingHistory && keyInfo.IsPressEscKey())
                {
                    _inputBuffer.Clear().LoadPrintable(_originalText);
                    _showingHistory = false;
                    _localpaginator = null;
                }
                else if (_showingHistory && IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    if (_showingHistory)
                    {
                        if (_localpaginator.TryGetSelectedItem(out var resulthist))
                        {
                            var oldprompt = _inputBuffer.ToString();
                            var oldcursor = _inputBuffer.Position;

                            _inputBuffer.Clear().LoadPrintable(resulthist.History);
                            _localpaginator = null;
                        }
                        _originalText = null;
                    }
                    if (_options.FinishWhenHistoryEnter || !_showingHistory)
                    {
                        result = _inputBuffer.ToString();
                        try
                        {
                            if (!TryValidate(result, _options.Validators, false))
                            {
                                result = default;
                                return false;
                            }
                            if (_options.SaveHistoryAtFinish)
                            {
                                AddAndSaveHistory(result);
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                    }
                    _showingHistory = false;
                }
                else if (CheckDefaultKey(keyInfo))
                {
                    ///none
                }
                else
                {
                    isvalidhit = null;
                }
            } while (KeyAvailable && !stoptoken.IsCancellationRequested);
            result = default;
            return isvalidhit;
        }

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(_options.Message);
            if (_showingHistory)
            {
                var part1 = _inputBuffer.ToString().Substring(0, _originalText.Length);
                var part2 = _inputBuffer.ToString().Substring(_originalText.Length);
                screenBuffer.WriteAnswer(part1);
                screenBuffer.WriteFilter(part2);
                screenBuffer.PushCursor();
            }
            else
            {
                if (_inputBuffer.InputWithSugestion != null)
                {
                    screenBuffer.WriteAnswer(_inputBuffer.InputWithSugestion[0]);
                    screenBuffer.WriteFilter(_inputBuffer.InputWithSugestion[1]);
                    screenBuffer.PushCursor();
                    screenBuffer.WriteAnswer(_inputBuffer.InputWithSugestion[2]);
                }
                else
                {
                    screenBuffer.PushCursor(_inputBuffer);
                }
            }
            if (HasDescription)
            {
                if (!HideDescription)
                {
                    if (_inputBuffer.InputWithSugestion != null && !string.IsNullOrEmpty(_inputBuffer.InputWithSugestion[3]))
                    {
                        screenBuffer.WriteLineDescription(_inputBuffer.InputWithSugestion[3]);
                    }
                    else
                    {
                        screenBuffer.WriteLineDescription(_options.Description);
                    }
                }
            }

            if (EnabledStandardTooltip)
            {
                ShowStandardHotKeys(screenBuffer);
                CreateMessageHit(screenBuffer);
            }
            if (_showingHistory)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    var value = item.History;
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
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
            return _inputBuffer.ToString();
        }


        private void ShowStandardHotKeys(ScreenBuffer screenBuffer)
        {
            if (_showingHistory || _inputBuffer.IsInAutoCompleteMode())
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, false, _options.EnabledAbortAllPipes, !HasDescription);
            }
            else
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
            }

        }

        private void CreateMessageHit(ScreenBuffer screenBuffer)
        {
            var msg = new StringBuilder();
            if (_showingHistory)
            {
                if (_options.FinishWhenHistoryEnter)
                {
                    msg.Append(Messages.ReadlineFisnishHistoryhit);
                }
                else
                {
                    msg.Append(Messages.ReadlineNotFisnishHistoryhit);
                }
                msg.Append(", ");
                msg.Append(Messages.ReadlineHistoryEsc);
            }
            else
            {
                if (_options.EnabledHistory && !_inputBuffer.IsInAutoCompleteMode())
                {
                    msg.Append(string.Format(Messages.ReadlineHistoryhit, _options.MinimumPrefixLength));
                }
                if (_options.SuggestionHandler != null)
                {
                    if (msg.Length > 0)
                    {
                        msg.Append(", ");
                    }
                    msg.Append(Messages.ReadlineSugestionhit);
                    if (_inputBuffer.IsInAutoCompleteMode())
                    {
                        msg.Append(", ");
                        msg.Append(Messages.ReadlineSugestionMode);
                    }
                }
            }
            screenBuffer.WriteLineHint(msg.ToString());
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result)
        {
            if (result != null)
            {
                FinishResult = result;
                screenBuffer.WriteAnswer(FinishResult);
            }
        }

        #region IControlReadline

        public IControlReadline Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IControlReadline InitialValue(string value, string error = null)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            _options.InitialValue = value;
            _options.InitialError = error;
            return this;
        }

        public IControlReadline SuggestionHandler(Func<SugestionInput, SugestionOutput> value)
        {
            _options.SuggestionHandler = value;
            if (value is not null)
            {
                _options.AcceptInputTab = false;
            }
            return this;
        }

        public IControlReadline AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlReadline AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlReadline SaveHistoryAtFinish(bool value)
        {
            _options.SaveHistoryAtFinish = value;
            return this;
        }

        public IControlReadline EnabledHistory(bool value)
        {
            _options.EnabledHistory = value;
            return this;
        }

        public IControlReadline FinisWhenHistoryEnter(bool value)
        {
            _options.FinishWhenHistoryEnter = value;
            return this;
        }

        public IControlReadline MaxHistory(byte value)
        {
            _options.MaxHistory = value;
            return this;
        }

        public IControlReadline PageSize(int value)
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

        public IControlReadline MinimumPrefixLength(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            _options.MinimumPrefixLength = value;
            return this;
        }

        public IControlReadline FileNameHistory(string value)
        {
            _options.FileNameHistory = value;
            return this;
        }

        public IControlReadline Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlReadline TimeoutHistory(TimeSpan value)
        {
            _options.TimeoutHistory = value;
            return this;
        }

        #endregion
    }
}
