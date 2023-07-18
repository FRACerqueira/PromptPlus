// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace PPlus.Controls
{
    internal class ListControl : BaseControl<IEnumerable<string>>, IControlList
    {
        private readonly ListOptions _options;
        private EmacsBuffer _inputBuffer;
        private Paginator<ItemListControl> _localpaginator;
        private bool _isInAutoCompleteMode;
        private int _completionsIndex = -1;
        private SugestionOutput? _completions = null;
        private int _editingItem = -1;
        public ListControl(IConsoleControl console, ListOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            _options.Validators.Add(PromptValidators.Required());

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

            _inputBuffer = new(_options.InputToCase, _options.AcceptInput, _options.MaxLenght);
            if (!string.IsNullOrEmpty(_options.DefaultValue))
            {
                _inputBuffer.LoadPrintable(_options.DefaultValue);
            }
            FinishResult = _inputBuffer.ToString();
            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlList

        public IControlList Interaction<T>(IEnumerable<T> values, Action<IControlList, T> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlList Default(string value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlList InputToCase(CaseOptions value)
        {
            _options.InputToCase = value;
            return this;
        }

        public IControlList AddValidators(params Func<object, ValidationResult>[] validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlList Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

          
        public IControlList SuggestionHandler(Func<SugestionInput, SugestionOutput> value)
        {
            _options.SuggestionHandler = value;
            return this;
        }

        public IControlList MaxLenght(ushort value)
        {
            _options.MaxLenght = value;
            return this;
        }

        public IControlList AcceptInput(Func<char, bool> value)
        {
            _options.AcceptInput = value;
            return this;
        }

        public IControlList AddItem(string value, bool immutable = false)
        {
            _options.Items.Add(new ItemListControl(value, immutable));
            return this;
        }

        public IControlList AddItems(IEnumerable<string> value, bool immutable = false)
        {
            foreach (var item in value)
            {
                _options.Items.Add(new ItemListControl(item, immutable));
            }
            return this;
        }

        public IControlList PageSize(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.PageSize = value;
            return this;
        }

        public IControlList AllowDuplicate()
        {
            _options.AllowDuplicate = true;
            return this;
        }

        public IControlList Range(int minvalue, int? maxvalue = null)
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

        public IControlList HotKeyEditItem(HotKey value)
        {
            _options.EditItemPress = value;
            return this;
        }

        public IControlList HotKeyRemoveItem(HotKey value)
        {
            _options.RemoveItemPress = value;
            return this;
        }


        #endregion

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<string> result, bool aborted)
        {
            string answer = string.Join(", ", result);
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options, "");
            if (_isInAutoCompleteMode)
            {
                var answer = FinishResult;
                screenBuffer.WriteSugestion(_options, answer);
                screenBuffer.SaveCursor();
            }
            else
            {
                if (_editingItem >= 0)
                {
                    screenBuffer.WriteTaggedInfo(_options, $"({Messages.EditMode}) ");
                }
                screenBuffer.WriteAnswer(_options, _inputBuffer.ToBackward());
                screenBuffer.SaveCursor();
                screenBuffer.WriteAnswer(_options, _inputBuffer.ToForward());
            }
            screenBuffer.WriteLineDescriptionList(_options, FinishResult);
            screenBuffer.WriteLineValidate(ValidateError, _options);
            screenBuffer.WriteLineTooltipsList(_options, _isInAutoCompleteMode, _editingItem >=0, _localpaginator.SelectedIndex >=0);
            var subset = _localpaginator.ToSubset();
            if (subset.Count > 0)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(Messages.AddedItems, _options.OptStyleSchema.TaggedInfo());
            }
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
                screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
            }
        }

        public override ResultPrompt<IEnumerable<string>> TryResult(CancellationToken cancellationToken)
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
                if (CheckAbortKey(keyInfo.Value) && _editingItem >=0)
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

                var acceptedkey = _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value);
                if (acceptedkey)
                {
                    if (_isInAutoCompleteMode)
                    {
                        ResetAutoComplete();
                    }
                }
                else if (keyInfo.Value.IskeyPageNavagator(_localpaginator))
                {
                    if (_editingItem >= 0)
                    {
                        _inputBuffer.Clear();
                    }
                    _editingItem = -1;
                }
                //edit Item seleted
                else if (_options.EditItemPress.Equals(keyInfo.Value) && _editingItem < 0 && _localpaginator.SelectedIndex >= 0)
                {
                    _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.Text);
                    _editingItem = _localpaginator.SelectedIndex;
                }
                //remove Item seleted
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
                        var aux = _inputBuffer.ToString();
                        if (aux.Equals(_localpaginator.SelectedItem.Text, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _inputBuffer.Clear();
                            _localpaginator.UnSelected();
                            _editingItem = -1;
                            break;
                        }
                        else
                        {
                            if (_options.Items.Count(x => x.Text.Equals(aux, StringComparison.InvariantCultureIgnoreCase)) > 0)
                            {
                                SetError(Messages.ListItemAlreadyexists);
                                break;
                            }
                        }
                    }
                    if (!TryValidate(_inputBuffer.ToString(), _options.Validators))
                    {
                        break;
                    }
                    _options.Items[_editingItem] = new ItemListControl(_inputBuffer.ToString());
                    _localpaginator = new Paginator<ItemListControl>(
                        FilterMode.StartsWith,
                        _options.Items, 
                        _options.PageSize, 
                        Optional<ItemListControl>.Create(new ItemListControl(_inputBuffer.ToString())),
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
                        var aux = _inputBuffer.ToString();
                        if (_options.Items.Count(x => x.Text.Equals(aux, StringComparison.InvariantCultureIgnoreCase)) > 0)
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
                    if (!TryValidate(_inputBuffer.ToString(), _options.Validators))
                    {
                        break;
                    }
                    ResetAutoComplete();
                    _options.Items.Add(new ItemListControl(_inputBuffer.ToString()));
                    _localpaginator = new Paginator<ItemListControl>(
                        FilterMode.StartsWith,
                        _options.Items, 
                        _options.PageSize, 
                        Optional<ItemListControl>.Create(new ItemListControl(_inputBuffer.ToString())),
                        (item1, item2) => item1.UniqueId == item2.UniqueId,
                        (item) => item.Text,
                        (item) => !item.Immutable);
                    _inputBuffer.Clear();
                    _editingItem = -1;
                    break;
                }
                //apply sugestion and not edit
                else if (_editingItem < 0 && _options.SuggestionHandler != null && (keyInfo.Value.IsPressTabKey() || keyInfo.Value.IsPressShiftTabKey()))
                {
                    if (!_isInAutoCompleteMode)
                    {
                        _completions = _options.SuggestionHandler.Invoke(new SugestionInput(_inputBuffer.ToString(), _options.OptContext));
                        if (_completions.HasValue && _completions.Value.Sugestions.Count > 0)
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
                //cancel sugestion
                else if (_options.SuggestionHandler != null && _isInAutoCompleteMode && keyInfo.Value.IsPressEscKey())
                {
                    _isInAutoCompleteMode = false;
                    _completionsIndex = -1;
                    _completions = null;
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
            FinishResult = _inputBuffer.ToString();
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
            return new ResultPrompt<IEnumerable<string>>(_options.Items.Select(x => x.Text), abort, !endinput);
        }

        private void ResetAutoComplete()
        {
            _completionsIndex = -1;
            _completions = null;
            _isInAutoCompleteMode = false;
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
            _inputBuffer.Clear().LoadPrintable(_completions.Value.Sugestions[_completionsIndex]);
            return true;
        }

        private void NextCompletions()
        {
            _completionsIndex++;
            if (_completionsIndex > _completions.Value.Sugestions.Count - 1)
            {
                _completionsIndex = 0;
            }
        }

        private void PreviusCompletions()
        {
            _completionsIndex--;
            if (_completionsIndex < 0)
            {
                _completionsIndex = _completions.Value.Sugestions.Count - 1;
            }
        }

    }
}
