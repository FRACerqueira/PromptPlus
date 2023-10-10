// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace PPlus.Controls
{
    internal class MaskEditControl : BaseControl<ResultMasked>, IControlMaskEdit
    {
        private readonly MaskEditOptions _options;
        private MaskedBuffer _inputBuffer;
        private Paginator<ItemHistory> _localpaginator;
        private IList<ItemHistory> _itemsHistory = new List<ItemHistory>();
        private string _originalText = string.Empty;
        private bool _isInAutoCompleteMode;
        private int _completionsIndex = -1;
        private SuggestionOutput? _completions = null;
        private string _defaultHistoric = null;


        public MaskEditControl(IConsoleControl console, MaskEditOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.FilterType == FilterMode.Disabled && _options.HistoryMinimumPrefixLength > 0)
            {
                throw new PromptPlusException("HistoryMinimumPrefixLength mustbe zero when FilterType is Disabled");
            }

            if (_options.CurrentCulture == null)
            {
                _options.CurrentCulture = _options.Config.AppCulture;
            }
            if (_options.HistoryEnabled)
            {
                LoadHistory();
            }
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
            }
            if (_options.Type == ControlMaskedType.Generic)
            {
                _options.FillNumber = null;
                _options.AcceptEmptyValue = false;
            }

            _inputBuffer = new(_options);

            if (!string.IsNullOrEmpty(_defaultHistoric))
            {
                _inputBuffer.Load(_inputBuffer.RemoveMask(_defaultHistoric, false));
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.DefaultValue))
                {
                    _inputBuffer.Load(_inputBuffer.RemoveMask(_options.DefaultValue, true));
                }
            }
            return _inputBuffer.ToMasked();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlMaskEdit

        public IControlMaskEdit FilterType(FilterMode value)
        {
            if (value == FilterMode.Disabled)
            {
                _options.HistoryMinimumPrefixLength = 0;
            }
            _options.FilterType = value;
            return this;
        }

        public IControlMaskEdit OverwriteDefaultFrom(string value, TimeSpan? timeout)
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

        public IControlMaskEdit AcceptEmptyValue(bool value = true)
        {
            _options.AcceptEmptyValue = value;
            return this;
        }

        public IControlMaskEdit TypeTipStyle(Style value)
        {
            _options.TypeTipStyle = value;
            return this;
        }

        public IControlMaskEdit NegativeStyle(Style value)
        {
            _options.NegativeStyle = value;
            return this;
        }

        public IControlMaskEdit PositiveStyle(Style value)
        {
            _options.PositiveStyle = value;
            return this;
        }

        public IControlMaskEdit InputToCase(CaseOptions value)
        {
            _options.InputToCase = value;
            return this;
        }

        public IControlMaskEdit AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlMaskEdit Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlMaskEdit DefaultIfEmpty(string value, bool zeroIsEmpty = true)
        {
            _options.DefaultEmptyValue = value;
            _options.ZeroIsEmpty = zeroIsEmpty;
            return this;
        }

        public IControlMaskEdit ChangeDescription(Func<string, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlMaskEdit HistoryEnabled(string value)
        {
            _options.HistoryFileName = value;
            return this;
        }

        public IControlMaskEdit HistoryMaxItems(byte value)
        {
            _options.HistoryMaxItems = value;
            return this;
        }

        public IControlMaskEdit HistoryMinimumPrefixLength(int value)
        {
            if (value < 0)
            {
                throw new PromptPlusException("HistoryMinimumPrefixLength must be greater than or equal to zero");
            }
            if (_options.FilterType == FilterMode.Disabled && value > 0)
            {
                throw new PromptPlusException("HistoryMinimumPrefixLength mustbe zero when FilterType is Disabled");
            }
            _options.HistoryMinimumPrefixLength = value;
            return this;
        }

        public IControlMaskEdit HistoryPageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("HistoryPageSize must be greater than or equal to 1");
            }
            _options.HistoryPageSize = value;
            return this;
        }

        public IControlMaskEdit HistoryTimeout(TimeSpan value)
        {
            if (value.TotalMilliseconds == 0)
            {
                throw new PromptPlusException("HistoryTimeout must be greater than 0");
            }
            _options.HistoryTimeout = value;
            return this;
        }

        public IControlMaskEdit Default(string value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlMaskEdit SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value)
        {
            _options.SuggestionHandler = value;
            return this;
        }

        public IControlMaskEdit ValidateOnDemand(bool value = true)
        {
            _options.ValidateOnDemand = value;
            return this;
        }

        public IControlMaskEdit Mask(string value, char? promptmask = null)
        {
            _options.Type = ControlMaskedType.Generic;
            if (string.IsNullOrEmpty(value))
            {
                throw new PromptPlusException("Mask is Null Or Empty");
            }
            _options.MaskValue = value;
            if (promptmask != null)
            {
                _options.Symbols(SymbolType.MaskEmpty,promptmask.Value.ToString());
            }
            return this;
        }

        public IControlMaskEdit Mask(MaskedType maskedType, char? promptmask = null)
        {
            switch (maskedType)
            {
                case MaskedType.DateOnly:
                    _options.Type = ControlMaskedType.DateOnly;
                    break;
                case MaskedType.TimeOnly:
                    _options.Type = ControlMaskedType.TimeOnly;
                    break;
                case MaskedType.DateTime:
                    _options.Type = ControlMaskedType.DateTime;
                    break;
                case MaskedType.Number:
                    promptmask = null;
                    _options.Type = ControlMaskedType.Number;
                    break;
                case MaskedType.Currency:
                    promptmask = null;
                    _options.Type = ControlMaskedType.Currency;
                    break;
            }
            _options.MaskValue = null;
            if (promptmask != null)
            {
                _options.Symbols(SymbolType.MaskEmpty, promptmask.Value.ToString());
            }
            return this;
        }

        public IControlMaskEdit Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlMaskEdit Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlMaskEdit FillZeros(bool value = true)
        {
            if (value)
            {
                _options.FillNumber = MaskedBuffer.Defaultfill;
            }
            else
            {
                _options.FillNumber = null;
            }
            return this;
        }

        public IControlMaskEdit FormatYear(FormatYear value)
        {
            _options.FmtYear = value;
            return this;
        }

        public IControlMaskEdit FormatTime(FormatTime value)
        {
            _options.FmtTime = value;
            return this;
        }

        public IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal)
        {
            if (intvalue < 0)
            {
                throw new PromptPlusException("intvalue must be greater than 0");
            }
            if (decimalvalue < 0)
            {
                throw new PromptPlusException("intvalue must be greater than 0");
            }
            if (intvalue + decimalvalue == 0)
            {
                throw new PromptPlusException("intvalue + decimalvalue must be greater than 0");
            }
            _options.AmmountInteger = intvalue;
            _options.AmmountDecimal = decimalvalue;
            _options.AcceptSignal = acceptSignal;
            return this;
        }

        public IControlMaskEdit DescriptionWithInputType(FormatWeek week = FormatWeek.None)
        {
            _options.DescriptionWithInputType = true;
            _options.ShowDayWeek = week;
            return this;
        }

        #endregion

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultMasked result, bool aborted)
        {
            var answer = result.Masked;
            if (_options.AcceptEmptyValue)
            {
                if (_options.FillNumber != null && IsMaskTypeDateTime())
                {
                    if (long.TryParse(result.Input, out var numbervalue))
                    {
                        if (numbervalue == 0)
                        {
                            answer = string.Empty;
                        }
                    }
                }
                else if (IsMaskTypeNumberOrCurrency() && _options.ZeroIsEmpty)
                {
                    if (double.TryParse(result.Input, out var numbervalue))
                    {
                        if (numbervalue == 0)
                        {
                            answer = string.Empty;
                        }
                    }
                }
            }
            if (!aborted)
            {
                SaveHistory(answer);
                if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                {
                    SaveDefaultHistory(answer);
                }
            }
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            if (_options.OptHideAnswer)
            {
                return;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            if (_isInAutoCompleteMode || _options.ShowingHistory)
            {
                screenBuffer.WriteSuggestion(_options, _inputBuffer.ToMasked());
                screenBuffer.SaveCursor();
            }
            else
            {
                if (_inputBuffer.NegativeNumberInput)
                {
                    screenBuffer.WriteNegativeAnswer(_options, _inputBuffer.ToBackwardString());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteNegativeAnswer(_options, _inputBuffer.ToForwardString());
                }
                else
                {
                    screenBuffer.WritePositiveAnswer(_options, _inputBuffer.ToBackwardString());
                    screenBuffer.SaveCursor();
                    screenBuffer.WritePositiveAnswer(_options, _inputBuffer.ToForwardString());
                }
            }
            screenBuffer.WriteLineDescriptionMaskEdit(_options, _inputBuffer.ToMasked(), _inputBuffer.Tooltip);
            if (_options.ShowingHistory)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    var value = item.History;
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(_options, value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, value);
                    }
                }
                if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            screenBuffer.WriteLineTooltipsMaskEdit(_options, _isInAutoCompleteMode);
        }

        public override ResultPrompt<ResultMasked> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            bool tryagain;
            do
            {
                ClearError();
                tryagain = false;
                var keyInfo = WaitKeypress(cancellationToken);

                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    continue;
                }
                if (CheckAbortKey(keyInfo.Value) && !(_options.ShowingHistory || _isInAutoCompleteMode))
                {
                    abort = true;
                    endinput = true;
                    break;
                }

                _originalText = _inputBuffer.ToString();
                if (_options.InputToCase != CaseOptions.Any)
                {
                    keyInfo = keyInfo.Value.ToCase(_options.InputToCase);
                }
                //apply suggestion
                if (_options.SuggestionHandler != null && (keyInfo.Value.IsPressTabKey() || keyInfo.Value.IsPressShiftTabKey()))
                {
                    if (!_isInAutoCompleteMode)
                    {
                        _completions = _options.SuggestionHandler.Invoke(new SuggestionInput(_inputBuffer.ToString(), _options.OptContext));
                        if (_completions.HasValue && _completions.Value.Suggestions.Count > 0)
                        {
                            _completionsIndex = -1;
                            _options.ShowingHistory = false;
                            _localpaginator = null;
                            _isInAutoCompleteMode = true;
                        }
                        else
                        {
                            tryagain = true;
                        }
                    }
                    ExecuteAutoComplete(keyInfo.Value.IsPressShiftTabKey());
                }
                //cancel suggestion
                else if (_options.SuggestionHandler != null && _isInAutoCompleteMode && keyInfo.Value.IsPressEscKey())
                {
                    _inputBuffer.Clear().Load(_originalText);
                    ClearMode();
                }
                //show history
                else if (_options.HistoryEnabled && !_options.ShowingHistory && (keyInfo.Value.IsPressDownArrowKey() || keyInfo.Value.IsPressPageDownKey()) && _itemsHistory.Count > 0 && _inputBuffer.Length >= _options.HistoryMinimumPrefixLength)
                {
                    _localpaginator = new Paginator<ItemHistory>(
                        _options.FilterType,
                        GetItemHistory(_options.FilterType),
                        _options.HistoryPageSize, Optional<ItemHistory>.s_empty, 
                        (item1,item2) => item1.History == item2.History,
                        (item) => item.History);

                    if (_localpaginator.Count > 0)
                    {
                        if (_isInAutoCompleteMode)
                        {
                            _completionsIndex = -1;
                            _completions = null;
                            _isInAutoCompleteMode = false;
                        }
                        _inputBuffer.Clear().Load(_inputBuffer.RemoveMask(_localpaginator.SelectedItem.History, true));
                        _options.ShowingHistory = true;
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
                //clear history
                else if (_options.HistoryEnabled && _options.ShowingHistory && keyInfo.Value.IsPressCtrlDeleteKey() && _itemsHistory.Count > 0)
                {
                    ClearHistory();
                    ClearMode();
                }
                //Navegator history
                else if (_options.HistoryEnabled && _options.ShowingHistory && IskeyPageNavegator(keyInfo.Value, _localpaginator))
                {
                    _inputBuffer.Clear().Load(_inputBuffer.RemoveMask(_localpaginator.SelectedItem.History, true));
                }
                //cancel history
                else if (_options.HistoryEnabled && _options.ShowingHistory && keyInfo.Value.IsPressEscKey())
                {
                    ClearMode();
                    _inputBuffer.Clear().Load(_originalText);
                }
                else if (keyInfo.Value.IsPressHomeKey())
                {
                    ClearMode();
                    _inputBuffer.ToHome();
                    tryagain = false;
                }
                else if (keyInfo.Value.IsPressEndKey())
                {
                    ClearMode();
                    _inputBuffer.ToEnd();
                    tryagain = false;
                }
                else if (keyInfo.Value.IsPressLeftArrowKey())
                {
                    var cls = ClearMode();
                    var act = _inputBuffer.Backward();
                    tryagain = !(cls || act);
                }
                else if (keyInfo.Value.IsPressRightArrowKey())
                {
                    var cls = ClearMode();
                    var act = _inputBuffer.Forward();
                    tryagain = !(cls || act);
                }
                else if (keyInfo.Value.IsPressBackspaceKey())
                {
                    var cls = ClearMode();
                    var act = _inputBuffer.Backspace();
                    tryagain = !(cls || act);
                }
                else if (keyInfo.Value.IsPressDeleteKey())
                {
                    var cls = ClearMode();
                    var act = _inputBuffer.Delete();
                    tryagain = !(cls || act);
                }
                else if (keyInfo.Value.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    ClearMode();
                    _inputBuffer.Clear();
                }
                else if (keyInfo.Value.IsPressSpecialKey(ConsoleKey.L, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    ClearMode();
                    _inputBuffer.Clear();
                }
                //completed input
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    ClearMode();
                    if (!string.IsNullOrEmpty(_options.DefaultEmptyValue))
                    {
                        if (_options.FillNumber != null && IsMaskTypeDateTime())
                        {
                            if (long.TryParse(_inputBuffer.ToString(), out var numbervalue))
                            {
                                if (numbervalue == 0)
                                {
                                    _inputBuffer.Clear();
                                    _inputBuffer.Load(_inputBuffer.RemoveMask(_options.DefaultEmptyValue, true));
                                }
                            }
                        }
                        else if (IsMaskTypeNumberOrCurrency() && _options.ZeroIsEmpty)
                        {
                            if (double.TryParse(_inputBuffer.ToString(), out var numbervalue))
                            {
                                if (numbervalue == 0)
                                {
                                    _inputBuffer.Clear();
                                    _inputBuffer.Load(_inputBuffer.RemoveMask(_options.DefaultEmptyValue, true));
                                }
                            }
                        }
                        else if (_inputBuffer.Length == 0)
                        {
                            _inputBuffer.Clear();
                            _inputBuffer.Load(_inputBuffer.RemoveMask(_options.DefaultEmptyValue, true));
                        }
                    }
                    endinput = true;
                    break;
                }
                else if (_inputBuffer.IsPrintable(keyInfo.Value))
                {
                    _inputBuffer.Insert(keyInfo.Value.KeyChar, out var isvalid);
                    if (isvalid && (_options.ShowingHistory || _isInAutoCompleteMode))
                    {
                        ClearMode();
                    }
                    else
                    {
                        tryagain = !isvalid;
                    }
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
                        tryagain = true;
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                _inputBuffer.Clear();
                endinput = true;
                abort = true;
            }
            FinishResult = _inputBuffer.ToMasked();
            _originalText = FinishResult;
            var executevalidate = true;
            if (_options.AcceptEmptyValue)
            {
                if (_options.FillNumber != null && IsMaskTypeDateTime())
                {
                    if (long.TryParse(_inputBuffer.ToString(), out var numbervalue))
                    {
                        if (numbervalue == 0)
                        {
                            _inputBuffer.Clear();
                            executevalidate = false;
                        }
                    }
                } 
                else if (IsMaskTypeNotGeneric() && string.IsNullOrEmpty(FinishResult))
                {
                    executevalidate = false;
                }
            }
            if (executevalidate)
            {
                if (_options.ValidateOnDemand || endinput)
                {
                    ClearError();
                    if (!TryValidate(FinishResult, _options.Validators))
                    {
                        if (!abort)
                        {
                            endinput = false;
                        }
                    }
                }
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
            return new ResultPrompt<ResultMasked>(new ResultMasked(_inputBuffer.RemoveMask(_inputBuffer.ToMasked(),true), _inputBuffer.ToMasked()), abort, !endinput, notrender);
        }

        private bool IsMaskTypeNotGeneric()
        {
            return _options.Type != ControlMaskedType.Generic;
        }

        private bool IsMaskTypeNumberOrCurrency()
        {
            return _options.Type == ControlMaskedType.Number ||
                _options.Type == ControlMaskedType.Currency;
        }

        private bool IsMaskTypeDateTime()
        {
            return _options.Type == ControlMaskedType.DateOnly ||
                _options.Type == ControlMaskedType.TimeOnly ||
                _options.Type == ControlMaskedType.DateTime;
        }
        private bool ClearMode()
        {
            if (_options.ShowingHistory || _isInAutoCompleteMode)
            {
                _completionsIndex = -1;
                _options.ShowingHistory = false;
                _localpaginator = null;
                _completions = null;
                _isInAutoCompleteMode = false;
                return true;
            }
            return false;
        }

        private bool ExecuteAutoComplete(bool Previus)
        {
            if (!_completions.HasValue)
            {
                return false;
            }
            if (Previus)
            {
                PreviusCompletions();
            }
            else
            {
                NextCompletions();
            }
            _inputBuffer
                .Clear()
                .Load(_inputBuffer.RemoveMask(_completions.Value.Suggestions[_completionsIndex],true));
            return true;
        }

        private void NextCompletions()
        {
            _completionsIndex++;
            if (_completionsIndex > _completions.Value.Suggestions.Count - 1)
            {
                _completionsIndex = 0;
            }
        }

        private void PreviusCompletions()
        {
            _completionsIndex--;
            if (_completionsIndex < 0)
            {
                _completionsIndex = _completions.Value.Suggestions.Count - 1;
            }
        }


        private IEnumerable<ItemHistory> GetItemHistory(FilterMode filterMode)
        {
            if (filterMode == FilterMode.Contains)
            {
                return _itemsHistory.Where(x => _inputBuffer.RemoveMask(x.History, true)
                            .Contains(_inputBuffer.RemoveMask(_inputBuffer.ToMasked(), true), StringComparison.InvariantCultureIgnoreCase)
                                && DateTime.Now < new DateTime(x.TimeOutTicks));
            }
            else if (filterMode == FilterMode.StartsWith)
            {
                return _itemsHistory.Where(x => _inputBuffer.RemoveMask(x.History, true)
                        .StartsWith(_inputBuffer.RemoveMask(_inputBuffer.ToMasked(), true), StringComparison.InvariantCultureIgnoreCase)
                            && DateTime.Now < new DateTime(x.TimeOutTicks));
            }
            return _itemsHistory.Where(x => DateTime.Now < new DateTime(x.TimeOutTicks));
        }

        private void LoadHistory()
        {
            if (_options.HistoryFileName != null)
            {
                _itemsHistory = FileHistory.LoadHistory(_options.HistoryFileName,_options.HistoryMaxItems);
            }
        }

        private void SaveHistory(string? value)
        {
            if (_options.HistoryFileName != null && value != null && value.Length > 0)
            {
                FileHistory.AddHistory(value, _options.HistoryTimeout, _itemsHistory);
                FileHistory.SaveHistory(_options.HistoryFileName, _itemsHistory, _options.HistoryMaxItems);
            }
        }

        private void LoadDefaultHistory()
        {
            _defaultHistoric = null;
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom,1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = aux[0].History;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveDefaultHistory(string value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(value, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }

        private void ClearHistory()
        {
            if (_options.HistoryFileName != null)
            {
                FileHistory.ClearHistory(_options.HistoryFileName);
                _itemsHistory.Clear();
            }
        }
    }
}
