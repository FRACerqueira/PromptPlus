// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace PromptPlusLibrary.Controls.Input
{
    /// <inheritdoc/>   
    internal sealed class InputControl : BaseControlPrompt<string>, IInputControl, IInputSecretControl
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer line, optional error/group line, optional description line,
        /// tooltip line and an extra row for the pagination footer when active.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 7;
        private int _effectivePageSize;

        private static readonly CompositeFormat s_TooltipHistoryShowFormat = CompositeFormat.Parse(PromptPlusResources.TooltipHistoryShow);
        private static readonly CompositeFormat s_TooltipSuggestionToggleAutoFormat = CompositeFormat.Parse(PromptPlusResources.TooltipSuggestionToggleAuto);
        private static readonly CompositeFormat s_TooltipSuggestionTabFormat = CompositeFormat.Parse(PromptPlusResources.TooltipSuggestionTab);
        private readonly Dictionary<InputStyles, Style> _optStyles;
        private Func<char, bool>? _acceptvalue = (_) => true;
        private Func<string, string>? _changeDescription;
        private Func<string, Task<string>>? _changeDescriptionAsync;
        private string _defaultValue = string.Empty;
        private string _defaultIfEmpty = string.Empty;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private CaseOptions _inputToCase = CaseOptions.Any;
        private int _maxLength = int.MaxValue;
        private Func<string, string[]>? _suggestionHandler;
        private Func<string, Task<string[]>>? _suggestionHandlerAsync;
        private Func<string, (bool, string?)>? _predicatevalue = (input) => (true, input);
        private Func<string, Task<(bool, string?)>>? _predicatevalueAsync;
        private char _secretChar;
        private bool _isinputsecret;
        private bool _passwordvisible;
        private HotKey? _enabledViewSecret;
        private IList<ItemHistory>? _itemHistories;
        private EmacsConsoleBuffer? _inputdata;
        private ModeView _modeView = ModeView.Input;
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Input,[] },
            { ModeView.Sugestions,[] },
            { ModeView.History,[] }
        };
        private int _indexTooptip;
        private bool _updatePosAnswerBuffer;
        private string _lastinput = string.Empty;
        private Paginator<ItemHistory>? _localHistpaginator;
        private string[]? _suggestions;
        private int _curentSuggestion = -1;
        private bool _autocompleteSuggestions = true;
        private byte _minimumSuggestionLength;
        private Paginator<(string UniqueId, string SuggestionValue)>? _localSuggestionPaginator;

        #region IInputControl IInputSecretControl Implementation

        /// <inheritdoc/>
        IInputControl IInputControl.AcceptInput(Func<char, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _acceptvalue = value;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.AcceptInput(Func<char, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _acceptvalue = value;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.ChangeDescription(Func<string, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.ChangeDescription(Func<string, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.ChangeDescriptionAsync(Func<string, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = null;
            _changeDescriptionAsync = value;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.ChangeDescriptionAsync(Func<string, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = null;
            _changeDescriptionAsync = value;
            return this;
        }

        /// <inheritdoc/>
        public IInputControl Default(string value, bool useDefaultHistory)
        {
            _defaultValue = value;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public IInputControl DefaultIfEmpty(string value)
        {
            _defaultIfEmpty = value;
            return this;
        }

        /// <inheritdoc/>
        public IInputControl EnabledHistory(string filename, Action<IHistoryOptions>? options)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            }
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.InputToCase(CaseOptions value)
        {
            _inputToCase = value;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.InputToCase(CaseOptions value)
        {
            _inputToCase = value;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.MaxLength(int maxLength)
        {
            if (maxLength <= 0)
            {
                _maxLength = int.MaxValue;
            }
            else
            {
                _maxLength = maxLength;
            }
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.MaxLength(int maxLength)
        {
            if (maxLength <= 0)
            {
                _maxLength = int.MaxValue;
            }
            else
            {
                _maxLength = maxLength;
            }
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.Styles(InputStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.Styles(InputStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IInputControl SuggestionHandler(Func<string, string[]> value, bool autocomplete = true)
        {
            _suggestionHandler = value ?? throw new ArgumentNullException(nameof(value));
            _suggestionHandlerAsync = null;
            _autocompleteSuggestions = autocomplete;
            return this;
        }

        /// <inheritdoc/>
        public IInputControl SuggestionHandlerAsync(Func<string, Task<string[]>> value, bool autocomplete = true)
        {
            _suggestionHandlerAsync = value ?? throw new ArgumentNullException(nameof(value));
            _suggestionHandler = null;
            _autocompleteSuggestions = autocomplete;
            return this;
        }

        public IInputControl MinimumSuggestionLength(byte value)
        {
            _minimumSuggestionLength = value;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.PredicateSelected(Func<string, (bool, string?)> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalue = value;
            _predicatevalueAsync = null;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.PredicateSelected(Func<string, (bool, string?)> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalue = value;
            _predicatevalueAsync = null;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.PredicateSelected(Func<string, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalue = (input) => (value(input), (string?)null);
            _predicatevalueAsync = null;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.PredicateSelected(Func<string, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalue = (input) => (value(input), (string?)null);
            _predicatevalueAsync = null;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.PredicateSelectedAsync(Func<string, Task<(bool, string?)>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalueAsync = value;
            _predicatevalue = null;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.PredicateSelectedAsync(Func<string, Task<(bool, string?)>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalueAsync = value;
            _predicatevalue = null;
            return this;
        }

        /// <inheritdoc/>
        IInputControl IInputControl.PredicateSelectedAsync(Func<string, Task<bool>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalueAsync = async (input) => ((await value(input).ConfigureAwait(false)), (string?)null);
            _predicatevalue = null;
            return this;
        }

        /// <inheritdoc/>
        IInputSecretControl IInputSecretControl.PredicateSelectedAsync(Func<string, Task<bool>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _predicatevalueAsync = async (input) => ((await value(input).ConfigureAwait(false)), (string?)null);
            _predicatevalue = null;
            return this;
        }


        /// <inheritdoc/>
        public IInputSecretControl MaskSecret(char? value, bool enabledView)
        {
            _isinputsecret = true;
            _secretChar = value ?? ConfigPrompt.SecretChar;
            _enabledViewSecret = enabledView ? ConfigPrompt.HotKeyInputPasswordView : null;
            return this;
        }

        #endregion


        public InputControl(bool isSecret, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<InputStyles>(console.CurrentStyle);
            _secretChar = ConfigPrompt.SecretChar;
            _enabledViewSecret = ConfigPrompt.HotKeyInputPasswordView;
            _isinputsecret = isSecret;
        }

        /// <inheritdoc/>
        public override void InitControl(CancellationToken cancellationToken)
        {
            if (!_isinputsecret && _historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                NormalizeLoadedHistory();
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    _defaultValue = _itemHistories[0].History;
                }
            }

            _inputdata = new(false, _inputToCase,ConfigPrompt.EmacsKeyBindings,  _acceptvalue);

            if (!_isinputsecret && !string.IsNullOrEmpty(_defaultValue) && TryInputPredicate(_defaultValue))
            {
                _inputdata?.LoadPrintable(_defaultValue,_maxLength);
            }

            LoadTooltipToggle();
        }

        /// <summary>
        /// Evaluates the optional value predicate for <paramref name="value"/>, returning <c>true</c>
        /// when no predicate is configured or when it accepts the value. Used to decide whether a
        /// default/history value may pre-fill the input (rejected values are not honored).
        /// </summary>
        private bool TryInputPredicate(string value)
        {
            if (_predicatevalue == null && _predicatevalueAsync == null)
            {
                return true;
            }
            (bool ok, _) = _predicatevalueAsync != null
                ? _predicatevalueAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalue?.Invoke(value) ?? (true, (string?)null));
            return ok;
        }

        /// <inheritdoc/>
        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                if (_localHistpaginator != null && _modeView == ModeView.History)
                {
                    _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _historyOptions!.PageSizeValue);
                    if (_effectivePageSize != _localHistpaginator!.PageSize)
                    {
                        _localHistpaginator.UpdatePageSize(_effectivePageSize);
                    }
                }
                else if (_localSuggestionPaginator != null && _modeView == ModeView.Sugestions)
                {
                    _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _localSuggestionPaginator!.PageSize);
                    if (_effectivePageSize != _localSuggestionPaginator!.PageSize)
                    {
                        _localSuggestionPaginator.UpdatePageSize(_effectivePageSize);
                    }
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);

                    if (press.IsResize || press.IsCancelled)
                    {
                        if (!press.IsResize)
                        {
                            if (_modeView != ModeView.Input)
                            {
                                _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                                _localHistpaginator = null;
                                ResetSuggestions();
                                _modeView = ModeView.Input;
                            }
                        }
                        break;
                    }

                    _updatePosAnswerBuffer = false;

                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                            _localHistpaginator = null;
                            ResetSuggestions();
                            _modeView = ModeView.Input;
                        }
                        ResultCtrl = new ResultPrompt<string>(_inputdata!.ToString(), true);
                        break;
                    }
                    else if (keyinfo.IsPressCtrlAltTabKey() || keyinfo.IsPressFilterActivationKey())
                    {
                        _indexTooptip = 0;
                        continue;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.Sugestions && _localSuggestionPaginator != null)
                        {
                            _inputdata!.LoadPrintable(_localSuggestionPaginator.SelectedItem.SuggestionValue, _maxLength);
                        }
                        if (_modeView != ModeView.Input)
                        {
                            _localHistpaginator = null;
                            ResetSuggestions();
                        }
                        _lastinput = _inputdata!.ToString();
                        (bool ok, string? message) = _predicatevalueAsync is not null
                            ? _predicatevalueAsync.Invoke(_lastinput).ConfigureAwait(false).GetAwaiter().GetResult()
                            : _predicatevalue?.Invoke(_lastinput) ?? (true, null);
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(PromptPlusResources.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        _modeView = ModeView.Input;
                        if (!_isinputsecret && !string.IsNullOrEmpty(_defaultIfEmpty) && _inputdata!.Length == 0)
                        {
                            _lastinput = _defaultIfEmpty;
                        }
                        ResultCtrl = new ResultPrompt<string>(_lastinput, false);
                        SaveHistory(_lastinput);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip >= _toggerTooptips[_modeView].Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    #endregion

                    else if (_modeView == ModeView.Input && _enabledViewSecret != null && ConfigPrompt.HotKeyInputPasswordView.Equals(keyinfo))
                    {
                        _passwordvisible = !_passwordvisible;
                        _indexTooptip = 0;
                        break;
                    }

                    #region Histories

                    else if (_modeView == ModeView.Input && ConfigPrompt.HotKeyInputHistoryView.Equals(keyinfo) && (_itemHistories?.Count ?? 0) > 0 && _inputdata!.Length >= _historyOptions!.MinPrefixLengthValue)
                    {
                        _indexTooptip = 0;
                        FilterMode filter = _historyOptions!.FilterTypeValue;
                        List<ItemHistory> subhist = GetItemHistory(filter);
                        if (subhist.Count == 0)
                        {
                            SetError(PromptPlusResources.HistoryNotFound);
                            break;
                        }
                        _lastinput = _inputdata!.ToString();

                        _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _historyOptions!.PageSizeValue);

                        _localHistpaginator = new Paginator<ItemHistory>(
                            filter,
                            subhist,
                            _effectivePageSize,
                            Optional<ItemHistory>.Empty(),
                            (item1, item2) => item1.History.Equals(item2.History, StringComparison.OrdinalIgnoreCase),
                            (item) => item.History);
                        _modeView = ModeView.History;
                        _inputdata!.LoadPrintable(_localHistpaginator.SelectedItem.History, _maxLength);
                        _indexTooptip = 0;
                        ResetSuggestions();
                        break;

                    }
                    else if (_modeView == ModeView.History)
                    {
                        if (ConfigPrompt.HotKeyInputHistoryView.Equals(keyinfo))
                        {
                            _indexTooptip = 0;
                            _localHistpaginator = null;
                            ResetSuggestions();
                            _modeView = ModeView.Input;
                            _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                            break;
                        }
                        else if (keyinfo.IsPressCtrlDeleteKey())
                        {
                            _indexTooptip = 0;
                            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
                            _itemHistories!.Clear();
                            // History was cleared: rebuild the tooltip cache so the history
                            // hotkey hint is no longer advertised for the remainder of the control.
                            LoadTooltipToggle();
                            _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                            _localHistpaginator = null;
                            _modeView = ModeView.Input;
                            ResetSuggestions();
                            break;
                        }
                        else if (keyinfo.IsPressDownArrowKey())
                        {
                            if (_localHistpaginator!.IsLastPageItem)
                            {
                                _localHistpaginator.NextPage(IndexOption.FirstItem);
                            }
                            else
                            {
                                _localHistpaginator.NextItem();
                            }
                            _inputdata!.LoadPrintable(_localHistpaginator.SelectedItem.History, _maxLength);
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressUpArrowKey())
                        {
                            if (_localHistpaginator!.IsFirstPageItem)
                            {
                                _localHistpaginator!.PreviousPage(IndexOption.LastItem);
                            }
                            else
                            {
                                _localHistpaginator!.PreviousItem();
                            }
                            _inputdata!.LoadPrintable(_localHistpaginator.SelectedItem.History, _maxLength);
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localHistpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _inputdata!.LoadPrintable(_localHistpaginator.SelectedItem.History, _maxLength);
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localHistpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _inputdata!.LoadPrintable(_localHistpaginator.SelectedItem.History, _maxLength);
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressCtrlHomeKey())
                        {
                            if (!_localHistpaginator!.Home())
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressCtrlEndKey())
                        {
                            if (!_localHistpaginator!.End())
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.KeyChar != '\t' && _inputdata!.TryAcceptedReadlineConsoleKey(keyinfo, _maxLength))
                        {
                            _indexTooptip = 0;
                            if (_modeView != ModeView.Input)
                            {
                                _localHistpaginator = null;
                                ResetSuggestions();
                                _modeView = ModeView.Input;
                            }
                            break;
                        }
                    }

                    #endregion

                    #region Suggestions

                    else if (_modeView == ModeView.Input && (_suggestionHandlerAsync != null || _suggestionHandler != null) && (keyinfo.IsPressTabKey() ||( keyinfo.IsPressShiftTabKey() && !_autocompleteSuggestions)))
                    {
                        if (_inputdata!.ToString().Length < _minimumSuggestionLength)
                        {
                            continue;
                        }
                        if (_suggestions == null)
                        {
                            string currentInput = _inputdata!.ToString();
                            _suggestions = _suggestionHandlerAsync is not null
                                ? _suggestionHandlerAsync.Invoke(currentInput).ConfigureAwait(false).GetAwaiter().GetResult()
                                : _suggestionHandler!(currentInput);
                            _curentSuggestion = -1;
                        }
                        if (_suggestions.Length == 0)
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        if (_autocompleteSuggestions)
                        {
                            if (keyinfo.IsPressTabKey())
                            {
                                _curentSuggestion++;
                                if (_curentSuggestion > _suggestions.Length - 1)
                                {
                                    _curentSuggestion = 0;
                                }
                            }
                            else
                            {
                                _curentSuggestion--;
                                if (_curentSuggestion < 0)
                                {
                                    _curentSuggestion = _suggestions.Length - 1;
                                }
                            }
                            _inputdata!.LoadPrintable(_suggestions[_curentSuggestion], _maxLength);
                        }
                        else
                        {
                            _modeView = ModeView.Sugestions;
                            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, 5);
                            _localSuggestionPaginator = new Paginator<(string UniqueId, string SuggestionValue)>(
                                FilterMode.Disabled,
                                _suggestions.Select(x => (Guid.NewGuid().ToString(), x)),
                                _effectivePageSize,
                                Optional<(string UniqueId, string SuggestionValue)>.Empty(),
                                (item1, item2) => item1.UniqueId.Equals(item2.UniqueId, StringComparison.OrdinalIgnoreCase),
                                (item) => item.SuggestionValue);
                        }
                        break;

                    }
                    else if (_modeView == ModeView.Sugestions)
                    {
                         if (keyinfo.IsPressShiftTabKey())
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                            ResetSuggestions();
                            _modeView = ModeView.Input;
                            break;
                        }
                        else if (keyinfo.IsPressTabKey())
                        {
                            _indexTooptip = 0;
                            _inputdata!.LoadPrintable(_localSuggestionPaginator!.SelectedItem.SuggestionValue, _maxLength);
                            ResetSuggestions();
                            _modeView = ModeView.Input;
                            break;
                        }
                        else if (keyinfo.IsPressDownArrowKey())
                        {
                            if (_localSuggestionPaginator!.IsLastPageItem)
                            {
                                _localSuggestionPaginator.NextPage(IndexOption.FirstItem);
                            }
                            else
                            {
                                _localSuggestionPaginator.NextItem();
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressUpArrowKey())
                        {
                            if (_localSuggestionPaginator!.IsFirstPageItem)
                            {
                                _localSuggestionPaginator.PreviousPage(IndexOption.LastItem);
                            }
                            else
                            {
                                _localSuggestionPaginator.PreviousItem();
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localSuggestionPaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localSuggestionPaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressCtrlHomeKey())
                        {
                            if (!_localSuggestionPaginator!.Home())
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressCtrlEndKey())
                        {
                            if (!_localSuggestionPaginator!.End())
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        else if (_inputdata!.TryAcceptedReadlineConsoleKey(keyinfo, _maxLength))
                        {
                            _indexTooptip = 0;
                            if (_modeView != ModeView.Input)
                            {
                                ResetSuggestions();
                                _modeView = ModeView.Input;
                            }
                            break;
                        }
                    }

                    #endregion

                    else if ((_modeView == ModeView.Input || _modeView == ModeView.Sugestions) && keyinfo.KeyChar != '\t' && _inputdata!.TryAcceptedReadlineConsoleKey(keyinfo, _maxLength))
                    {
                        _indexTooptip = 0;
                        if (_suggestions != null)
                        {
                            ResetSuggestions();
                            _modeView = ModeView.Input;
                        }
                        break;
                    }
                }
                if (_modeView != ModeView.History)
                {
                    _lastinput = _inputdata!.ToString();
                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        /// <inheritdoc/>
        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            // Re-evaluate the effective page size every frame so the visible items count
            // stays in sync with the current console height (after any terminal resize).
            if (_historyOptions != null && _localHistpaginator != null)
            {
                int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, _historyOptions!.PageSizeValue);
                if (targetPageSize != _effectivePageSize)
                {
                    _effectivePageSize = targetPageSize;
                    _localHistpaginator?.UpdatePageSize(_effectivePageSize);
                }
            }

            WritePrompt(screenBuffer, _optStyles[InputStyles.Prompt]);

            WriteAnswer(screenBuffer);

            WriteDescription(screenBuffer);

            WriteSugestions(screenBuffer);

            WriteHistory(screenBuffer);

            WriteTooltip(screenBuffer);

            WriteError(screenBuffer, _optStyles[InputStyles.Error]);

        }

        /// <inheritdoc/>
        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _modeView = ModeView.Input;
            _localHistpaginator = null;
            ResetSuggestions();
            _updatePosAnswerBuffer = false;
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted)
            {
                answer = ResultCtrl!.Value.Content;
                if (_isinputsecret)
                {
                    answer = new string(_secretChar, answer.Length);
                }
            }
            else if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                answer = PromptPlusResources.CanceledKey;
            }
            WritePrompt(screenBuffer, _optStyles[InputStyles.Prompt]);
            screenBuffer.WriteLine(answer, _optStyles[InputStyles.Answer]);
            return true;
        }

        /// <inheritdoc/>
        public override void FinalizeControl()
        {
            //none
        }

        private string GetTooltipToggle()
        {
            switch (_modeView)
            {
                case ModeView.Input:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.Input].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.Input][_indexTooptip];
                    }
                case ModeView.Sugestions:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.Sugestions].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.Sugestions  ][_indexTooptip];
                    }
                case ModeView.History:
                    {
                        if (_indexTooptip >= _toggerTooptips[ModeView.History].Length)
                        {
                            _indexTooptip = 0;
                        }
                        return _toggerTooptips[ModeView.History][_indexTooptip];
                    }
                default:
                    throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[InputStyles.Tooltips]);
        }

        private void WriteSugestions(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.Sugestions)
            {
                return;
            }

            ArraySegment<(string UniqueID, string Value)> subset = _localSuggestionPaginator!.GetPageData(); // Cache the page data
            screenBuffer.WriteLine(PromptPlusResources.EntrySuggestion, _optStyles[InputStyles.Selected]);
            foreach (var (UniqueID, Value) in subset)
            {
                string value = Value;
                if (_localSuggestionPaginator.SelectedIndex >= 0 && _localSuggestionPaginator.SelectedItem.UniqueId == UniqueID)
                {
                    screenBuffer.Write($"{GetSymbol(SymbolType.Selector)}", _optStyles[InputStyles.Selected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", _optStyles[InputStyles.UnSelected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.UnSelected]);
                }
            }

            if (_localSuggestionPaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localSuggestionPaginator.TotalCountValid,
                    _localSuggestionPaginator.SelectedPage + 1,
                    _localSuggestionPaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[InputStyles.Pagination]);
            }
        }

        private void WriteHistory(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.History)
            {
                return;
            }

            ArraySegment<ItemHistory> subset = _localHistpaginator!.GetPageData(); // Cache the page data
            screenBuffer.WriteLine(PromptPlusResources.EntryHistory, _optStyles[InputStyles.Selected]);
            var pos = -1;
            foreach (ItemHistory item in subset)
            {
                pos++;
                string value = item.History;
                if (_localHistpaginator.SelectedIndex >= 0 && _localHistpaginator.SelectedIndex == pos)
                {
                    screenBuffer.Write($"{GetSymbol(SymbolType.Selector)}", _optStyles[InputStyles.Selected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", _optStyles[InputStyles.UnSelected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[InputStyles.UnSelected]);
                }
            }

            if (_localHistpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localHistpaginator.TotalCountValid,
                    _localHistpaginator.SelectedPage + 1,
                    _localHistpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[InputStyles.Pagination]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_updatePosAnswerBuffer)
            {
                _inputdata!.LoadPrintable(_lastinput!, _maxLength);
                _inputdata.ToHome();
            }
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_inputdata!, promptWidth);
            if (_isinputsecret && !_passwordvisible)
            {
                visibleLeft = new string(_secretChar, visibleLeft.Length);
                visibleRight = new string(_secretChar, visibleRight.Length);
            }
            screenBuffer.Write(visibleLeft, _optStyles[InputStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine(visibleRight, _optStyles[InputStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(_inputdata!.ToString())
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(_inputdata!.ToString()) ?? OptionsControl.DescriptionValue;
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[InputStyles.Description]);
            }
        }

       private List<ItemHistory> GetItemHistory(FilterMode filterMode)
        {
            long currentTime = DateTime.Now.Ticks;
            string inputData = _inputdata!.ToString();
            IList<ItemHistory> source = _itemHistories!;
            bool hasFilter = filterMode != FilterMode.Disabled && !string.IsNullOrEmpty(inputData);
            List<ItemHistory> result = new(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                ItemHistory item = source[i];
                if (currentTime > item.TimeOutTicks)
                {
                    continue;
                }
                if (hasFilter)
                {
                    bool match = filterMode switch
                    {
                        FilterMode.Contains => item.History.Contains(inputData, StringComparison.OrdinalIgnoreCase),
                        FilterMode.StartsWith => item.History.StartsWith(inputData, StringComparison.OrdinalIgnoreCase),
                        _ => throw new NotImplementedException($"FilterMode {filterMode} not implemented."),
                    };
                    if (!match)
                    {
                        continue;
                    }
                }
                result.Add(item);
            }
            return result;
        }

        private void SaveHistory(string value)
        {
            if (_historyOptions == null || string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist = FileHistory.AddHistory(value, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
            LoadTooltipToggle();

        }

        private void NormalizeLoadedHistory()
        {
            if (_itemHistories == null || _itemHistories.Count == 0)
            {
                return;
            }

            List<ItemHistory> normalized = new(_itemHistories.Count);
            bool changed = false;

            for (int i = 0; i < _itemHistories.Count; i++)
            {
                ItemHistory entry = _itemHistories[i];
                string normalizedText = NormalizeHistoryValue(entry.History);
                if (!string.Equals(entry.History, normalizedText, StringComparison.Ordinal))
                {
                    changed = true;
                }
                normalized.Add(new ItemHistory(normalizedText, entry.TimeOutTicks));
            }

            _itemHistories = normalized;

            if (!changed || _historyOptions == null)
            {
                return;
            }

            FileHistory.SaveHistory(_historyOptions.FileNameValue, normalized, _historyOptions.MaxItemsValue);
        }

        private static string NormalizeHistoryValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            try
            {
                if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
                {
                    string? decoded = JsonSerializer.Deserialize<string>(value);
                    if (decoded is not null)
                    {
                        return decoded;
                    }
                }
            }
            catch (JsonException)
            {
                // Keep original persisted value when it is not valid JSON content.
            }

            return value;
        }

        private void ResetSuggestions()
        {
            _curentSuggestion = -1;
            _suggestions = null;
            _localSuggestionPaginator = null;
        }

        private enum ModeView
        {
            Input,
            Sugestions,
            History
        }

        private string GetTooltipInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            if (_isinputsecret && _enabledViewSecret != null)
            {
                tooltip.Append(CultureInfo.CurrentCulture, $"{ConfigPrompt.HotKeyInputPasswordView}:{PromptPlusResources.TooltipViewPassword}.");
            }
            if ((_suggestionHandler != null || _suggestionHandlerAsync != null) && _autocompleteSuggestions)
            {
                tooltip.Append(string.Format(
                    ConfigPrompt.DefaultCulture,
                    s_TooltipSuggestionToggleAutoFormat,
                    _minimumSuggestionLength));
                tooltip.Append('.');
            }
            if ((_suggestionHandler != null || _suggestionHandlerAsync != null) && !_autocompleteSuggestions)
            {
                tooltip.Append(string.Format(
                    ConfigPrompt.DefaultCulture,
                    s_TooltipSuggestionTabFormat,
                    _minimumSuggestionLength));
                tooltip.Append('.');
                tooltip.Append(PromptPlusResources.TooltipSuggestionShiftTab);
                tooltip.Append('.');
            }
            if (_itemHistories != null && _itemHistories.Count > 0)
            {
                string historyTooltip = string.Format(
                    CultureInfo.CurrentCulture,
                    s_TooltipHistoryShowFormat,
                    _historyOptions!.MinPrefixLengthValue);
                tooltip.Append(CultureInfo.CurrentCulture, $"{ConfigPrompt.HotKeyInputHistoryView}:{historyTooltip}.");
            }
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    GetTooltipInput()
                ];
                if (mode == ModeView.Input)
                {
                    lsttooltips.Add(PromptPlusResources.TooltipNavegateTextPrompt);
                }
                if (mode == ModeView.History && _itemHistories != null && _itemHistories.Count > 0)
                {
                    lsttooltips.Add(PromptPlusResources.TooltipHistoryClear);
                }
                lsttooltips.AddRange(GetEmacsTooltips(false));
                if (OptionsControl.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }
    }
}
