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
    internal class MaskEditListControl : BaseControl<IEnumerable<ResultMasked>>, IControlMaskEditList
    {
        private readonly MaskEditListOptions _options;
        private MaskedBuffer _inputBuffer;
        private Paginator<ItemListControl> _localpaginator;
        private bool _isInAutoCompleteMode;
        private int _completionsIndex = -1;
        private SuggestionOutput? _completions = null;
        private int _editingItem = -1;
        public MaskEditListControl(IConsoleControl console, MaskEditListOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            _options.CurrentCulture ??= _options.Config.AppCulture;

            _options.Validators.Add(PromptValidators.Required());
            if (_options.Type == ControlMaskedType.Generic)
            {
                _options.FillNumber = null;
                _options.AcceptEmptyValue = false;
            }

            var remove = new List<ItemListControl>();
            var qtd = 0;
            foreach (var item in _options.Items)
            {
                qtd++;
                if (qtd > _options.Maximum)
                {
                    remove.Add(item);
                }
            }
            foreach (var item in remove)
            {
                _options.Items.Remove(item);
            }
            remove.Clear();
            if (!_options.AllowDuplicate)
            {
                var aux = _options.Items
                    .Where(x => _options.Items.Count(y => y.Text.Equals(x.Text)) > 1)
                    .Select(x => x.Text)
                    .Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();

                foreach (var item in aux)
                {
                    var dup = _options.Items.Where(x => x.Text.Equals(item)).ToArray();
                    if (dup.Length > 1)
                    {
                        for (int i = 1; i < dup.Length; i++)
                        {
                            _options.Items.Remove(dup[i]);
                        }
                    }
                }
            }

            _localpaginator = new Paginator<ItemListControl>(
                FilterMode.StartsWith,
                _options.Items, 
                _options.PageSize, 
                Optional<ItemListControl>.s_empty, 
                (item1,item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.Text, 
                (item) => !item.Immutable);
            _localpaginator.UnSelected();

            _inputBuffer = new(_options);

            if (!string.IsNullOrEmpty(_options.DefaultValue))
            {
                _inputBuffer.Load(_inputBuffer.RemoveMask(_options.DefaultValue, true));
            }
            return _inputBuffer.ToMasked();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlMaskEditList

        public IControlMaskEditList Interaction<T>(IEnumerable<T> values, Action<IControlMaskEditList, T> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlMaskEditList Default(string value)
        {
            _options.DefaultValue = value;      
            return this;
        }

        public IControlMaskEditList AddItem(string value, bool immutable = false)
        {
            _options.Items.Add(new ItemListControl(value, immutable));
            return this;
        }

        public IControlMaskEditList AddItems(IEnumerable<string> value, bool immutable = false)
        {
            foreach (var item in value)
            {
                _options.Items.Add(new ItemListControl(item, immutable));
            }
            return this;
        }

        public IControlMaskEditList AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlMaskEditList AllowDuplicate()
        {
            _options.AllowDuplicate = true;
            return this;
        }

        public IControlMaskEditList AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal)
        {
            _options.AmmountInteger = intvalue;
            _options.AmmountDecimal = decimalvalue;
            _options.AcceptSignal = acceptSignal;
            return this;
        }

        public IControlMaskEditList ChangeDescription(Func<string, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlMaskEditList Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlMaskEditList Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlMaskEditList Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlMaskEditList DescriptionWithInputType(FormatWeek week = FormatWeek.None)
        {
            _options.DescriptionWithInputType = true;
            _options.ShowDayWeek = week;
            return this;
        }

        public IControlMaskEditList AcceptEmptyValue()
        {
            _options.AcceptEmptyValue = true;
            return this;
        }

        public IControlMaskEditList FillZeros()
        {
            _options.FillNumber = MaskedBuffer.Defaultfill;
            return this;
        }

        public IControlMaskEditList FormatTime(FormatTime value)
        {
            _options.FmtTime = value;
            return this;
        }

        public IControlMaskEditList FormatYear(FormatYear value)
        {
            _options.FmtYear = value;
            return this;
        }

        public IControlMaskEditList HotKeyEditItem(HotKey value)
        {
            _options.EditItemPress = value;
            return this;
        }

        public IControlMaskEditList HotKeyRemoveItem(HotKey value)
        {
            _options.RemoveItemPress = value;
            return this;
        }
        public IControlMaskEditList InputToCase(CaseOptions value)
        {
            _options.InputToCase = value;
            return this;
        }

        public IControlMaskEditList Mask(MaskedType maskedType, char? promptmask = null)
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
                    _options.Type = ControlMaskedType.Number;
                    break;
                case MaskedType.Currency:
                    _options.Type = ControlMaskedType.Currency;
                    break;
            }
            _options.MaskValue = null;
            if (promptmask != null)
            {
                _options.Symbols(SymbolType.MaskEmpty,promptmask.Value.ToString());
            }
            return this;
        }

        public IControlMaskEditList Mask(string value, char? promptmask = null)
        {
            _options.Type = ControlMaskedType.Generic;
            if (string.IsNullOrEmpty(value))
            {
                throw new PromptPlusException("Mask is Null Or Empty");
            }
            _options.MaskValue = value;
            if (promptmask != null)
            {
                _options.Symbols(SymbolType.MaskEmpty, promptmask.Value.ToString());
            }
            return this;
        }

        public IControlMaskEditList NegativeStyle(Style value)
        {
            _options.NegativeStyle = value;
            return this;
        }

        public IControlMaskEditList PositiveStyle(Style value)
        {
            _options.PositiveStyle = value;
            return this;
        }

        public IControlMaskEditList PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlMaskEditList Range(int minvalue, int? maxvalue = null)
        {
            if (!maxvalue.HasValue)
            {
                maxvalue = _options.Maximum;
            }
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
                throw new PromptPlusException($"RangerSelect invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue.Value;
            return this;
        }

        public IControlMaskEditList SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value)
        {
            _options.SuggestionHandler = value;
            return this;
        }

        public IControlMaskEditList TypeTipStyle(Style value)
        {
            _options.TypeTipStyle = value;
            return this;
        }

        #endregion

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            if (_isInAutoCompleteMode)
            {
                var answer = FinishResult;
                screenBuffer.WriteSuggestion(_options, answer);
                screenBuffer.SaveCursor();
            }
            else
            {
                if (_editingItem >= 0)
                {
                    screenBuffer.WriteTaggedInfo(_options, $"({Messages.EditMode}) ");
                }
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
            screenBuffer.WriteLineDescriptionMaskEditList(_options, _inputBuffer.ToMasked(), _inputBuffer.Tooltip);
            var subset = _localpaginator.ToSubset();
            var pos = -1;
            foreach (var item in subset)
            {
                pos++;
                if (!item.Immutable && _localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<string>.Default.Equals(item.Text, selectedItem.Text) && pos == _localpaginator.SelectedIndex)
                {
                    screenBuffer.WriteLineSelector(_options, item.Text);
                }
                else
                {
                    if (item.Immutable)
                    {
                        screenBuffer.WriteLineNotSelectorDisabled(_options, item.Text);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(_options, item.Text);
                    }
                }
            }
            if (_localpaginator.Count > 0)
            {
                if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
                }
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            screenBuffer.WriteLineTooltipsMaskEditList(_options, _isInAutoCompleteMode, _editingItem >= 0, _localpaginator.SelectedIndex >= 0);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<ResultMasked> result, bool aborted)
        {
            string answer = string.Join(", ", result.Select(x => x.Masked));
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

        public override ResultPrompt<IEnumerable<ResultMasked>> TryResult(CancellationToken cancellationToken)
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
                //abort edit
                if (CheckAbortKey(keyInfo.Value) && _editingItem >= 0)
                {
                    _inputBuffer.Clear();
                    _editingItem = -1;
                    _localpaginator.UnSelected();
                    break;
                }
                //abort control
                if (CheckAbortKey(keyInfo.Value) && !_isInAutoCompleteMode)
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    continue;
                }
                if (_options.InputToCase != CaseOptions.Any)
                {
                    keyInfo = keyInfo.Value.ToCase(_options.InputToCase);
                }
                if (keyInfo.Value.IsPressHomeKey())
                {
                    ResetAutoComplete();
                    _inputBuffer.ToHome();
                    tryagain = false;
                }
                else if (keyInfo.Value.IsPressEndKey())
                {
                    ResetAutoComplete();
                    _inputBuffer.ToEnd();
                    tryagain = false;
                }
                else if (keyInfo.Value.IsPressLeftArrowKey())
                {
                    ResetAutoComplete();
                    var act = _inputBuffer.Backward();
                    tryagain = !act;
                }
                else if (keyInfo.Value.IsPressRightArrowKey())
                {
                    ResetAutoComplete();
                    var act = _inputBuffer.Forward();
                    tryagain = !act;
                }
                else if (keyInfo.Value.IsPressBackspaceKey())
                {
                    ResetAutoComplete();
                    var act = _inputBuffer.Backspace();
                    tryagain = !act;
                }
                else if (keyInfo.Value.IsPressDeleteKey())
                {
                    ResetAutoComplete();
                    var act = _inputBuffer.Delete();
                    tryagain = !act;
                }
                else if (keyInfo.Value.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    ResetAutoComplete();
                    _inputBuffer.Clear();
                }
                else if (keyInfo.Value.IsPressSpecialKey(ConsoleKey.L, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    ResetAutoComplete();
                    _inputBuffer.Clear();
                }
                else if (keyInfo.Value.IskeyPageNavagator(_localpaginator))
                {
                    ResetAutoComplete();
                    if (_editingItem >= 0)
                    {
                        _inputBuffer.Clear();
                    }
                    _editingItem = -1;
                }
                //edit Item selected
                else if (_options.EditItemPress.Equals(keyInfo.Value) && _editingItem < 0 && _localpaginator.SelectedIndex >= 0)
                {
                    _inputBuffer.Clear().Load(_inputBuffer.RemoveMask(_localpaginator.SelectedItem.Text,true));
                    _editingItem = _localpaginator.SelectedIndex;
                }
                //remove Item selected
                else if (_options.RemoveItemPress.Equals(keyInfo.Value) && _localpaginator.SelectedIndex >= 0)
                {
                    if (_localpaginator.SelectedItem.Immutable)
                    {
                        tryagain = true;
                        continue;
                    }
                    var pos = _localpaginator.SelectedIndex + (_localpaginator.SelectedPage * _localpaginator.Count);
                    _options.Items.RemoveAt(pos);
                    if (_options.Items.Count == 0)
                    {
                        _localpaginator = new Paginator<ItemListControl>(
                            FilterMode.StartsWith,
                            _options.Items, 
                            _options.PageSize, 
                            Optional<ItemListControl>.s_empty,
                            (item1, item2) => item1.UniqueId == item2.UniqueId,
                            (item) => item.Text,
                            (item) => !item.Immutable);
                        _inputBuffer.Clear();
                        break;
                    }
                    if (pos > _options.Items.Count - 1)
                    {
                        pos = _options.Items.Count - 1;
                    }
                    var item = _options.Items[pos];
                    _localpaginator = new Paginator<ItemListControl>(
                        FilterMode.StartsWith,
                        _options.Items, 
                        _options.PageSize, 
                        Optional<ItemListControl>.Create(item),
                        (item1, item2) => item1.UniqueId == item2.UniqueId,
                        (item) => item.Text,
                        (item) => !item.Immutable);
                    _inputBuffer.Clear();
                    _editingItem = -1;
                }
                //completed list input
                else if (keyInfo.Value.IsPressSpecialKey(ConsoleKey.Enter, ConsoleModifiers.Control))
                {
                    _editingItem = -1;
                    ResetAutoComplete();
                    endinput = true;
                    break;
                }
                //update to list input
                else if (keyInfo.Value.IsPressEnterKey() && _editingItem >= 0)
                {
                    if (!_options.AllowDuplicate)
                    {
                        var aux = _inputBuffer.ToMasked();
                        if (aux.Equals(_localpaginator.SelectedItem.Text, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _inputBuffer.Clear();
                            _localpaginator.UnSelected();
                            _editingItem = -1;
                            break;
                        }
                        else
                        {
                            if (_options.Items.Any(x => x.Text.Equals(aux, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                SetError(Messages.ListItemAlreadyexists);
                                break;
                            }
                        }
                    }
                    if (!TryValidate(_inputBuffer.ToMasked(), _options.Validators))
                    {
                        break;
                    }
                    _options.Items[_editingItem] = new ItemListControl(_inputBuffer.ToMasked());
                    _localpaginator = new Paginator<ItemListControl>(
                        FilterMode.StartsWith,
                        _options.Items, 
                        _options.PageSize, 
                        Optional<ItemListControl>.Create(new ItemListControl(_inputBuffer.ToMasked())),
                        (item1, item2) => item1.UniqueId == item2.UniqueId,
                        (item) => item.Text,
                        (item) => !item.Immutable);
                    _inputBuffer.Clear();
                    _localpaginator.UnSelected();
                    _editingItem = -1;
                    break;
                }
                //add to list input
                else if (keyInfo.Value.IsPressEnterKey() && _editingItem < 0)
                {
                    if (!_options.AllowDuplicate)
                    {
                        var aux = _inputBuffer.ToMasked();
                        if (_options.Items.Any(x => x.Text.Equals(aux, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            SetError(Messages.ListItemAlreadyexists);
                            break;
                        }
                    }
                    if (_options.Maximum <= _options.Items.Count)
                    {
                        SetError(string.Format(Messages.ListMaxSelection, _options.Maximum));
                        break;
                    }
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
                    ResetAutoComplete();
                    if (executevalidate)
                    {
                        if (!TryValidate(_inputBuffer.ToMasked(), _options.Validators))
                        {
                            break;
                        }
                    }
                    _options.Items.Add(new ItemListControl(_inputBuffer.ToMasked()));
                    _localpaginator = new Paginator<ItemListControl>(
                        FilterMode.StartsWith,
                        _options.Items, 
                        _options.PageSize, 
                        Optional<ItemListControl>.Create(new ItemListControl(_inputBuffer.ToMasked())),
                        (item1, item2) => item1.UniqueId == item2.UniqueId,
                        (item) => item.Text,
                        (item) => !item.Immutable);
                    _inputBuffer.Clear();
                    _editingItem = -1;
                    break;
                }
                //apply suggestion and not edit
                else if (_editingItem < 0 && _options.SuggestionHandler != null && (keyInfo.Value.IsPressTabKey() || keyInfo.Value.IsPressShiftTabKey()))
                {
                    if (!_isInAutoCompleteMode)
                    {
                        _completions = _options.SuggestionHandler.Invoke(new SuggestionInput(_inputBuffer.ToMasked(), _options.OptContext));
                        if (_completions.HasValue && _completions.Value.Suggestions.Count > 0)
                        {
                            _completionsIndex = -1;
                            _isInAutoCompleteMode = true;
                        }
                        else
                        {
                            tryagain = true;
                            continue;
                        }
                    }
                    ExecuteAutoComplete(keyInfo.Value.IsPressShiftTabKey());
                }
                //cancel suggestion
                else if (_options.SuggestionHandler != null && _isInAutoCompleteMode && keyInfo.Value.IsPressEscKey())
                {
                    _isInAutoCompleteMode = false;
                    _completionsIndex = -1;
                    _completions = null;
                }
                else if (!char.IsControl(keyInfo.Value.KeyChar))
                {
                    _inputBuffer.Insert(keyInfo.Value.KeyChar, out var isvalid);
                    if (isvalid && (_options.ShowingHistory || _isInAutoCompleteMode))
                    {
                        ResetAutoComplete();
                        break;
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
            if (endinput)
            {
                ClearError();
                if (_options.Items.Count < _options.Minimum)
                {
                    SetError(string.Format(Messages.ListMinSelection, _options.Minimum));
                    if (!abort)
                    {
                        endinput = false;
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
            return new ResultPrompt<IEnumerable<ResultMasked>>(
                _options.Items.Select(x => new ResultMasked(_inputBuffer.RemoveMask(x.Text, true), x.Text)), abort, !endinput,notrender);
        }

        private bool ResetAutoComplete()
        {
            if (_isInAutoCompleteMode)
            {
                _completionsIndex = -1;
                _completions = null;
                _isInAutoCompleteMode = false;
                return true;
            }
            return false;
        }

        private bool IsMaskTypeNotGeneric()
        {
            return _options.Type != ControlMaskedType.Generic;
        }

        private bool IsMaskTypeDateTime()
        {
            return _options.Type == ControlMaskedType.DateOnly ||
                _options.Type == ControlMaskedType.TimeOnly ||
                _options.Type == ControlMaskedType.DateTime;
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
            _inputBuffer.Clear().Load(_inputBuffer.RemoveMask(_completions.Value.Suggestions[_completionsIndex],true));
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

    }
}
