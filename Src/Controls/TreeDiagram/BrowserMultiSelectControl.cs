// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class BrowserMultiSelectControl : BaseControl<ItemBrowser[]>, IControlBrowserMultiSelect, IDisposable
    {
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase, modefilter: true);
        private readonly List<ItemTreeViewFlatNode<ItemBrowser>> _flatnodes;
        private readonly BrowserOptions _options;
        private readonly object _root = new();
        private TreeView<ItemBrowser> _browserTreeView;
        private Paginator<ItemTreeViewFlatNode<ItemBrowser>> _localpaginator;
        private bool _firstLoad = true;
        private (int CursorLeft, int CursorTop) _cusorSpinner;
        private CancellationTokenSource _lnkcts;
        private CancellationTokenSource _ctsesc;
        private Task _taskspinner;
        private bool _disposed;
        private bool _loadFolderFinish = true;
        private List<(string UniqueId, ItemBrowser value)> _selectedItems;
        private bool _rootExpand = true;

        public BrowserMultiSelectControl(IConsoleControl console, BrowserOptions options) : base(console, options)
        {
            _options = options;
            _flatnodes = new();
            _selectedItems = new();
        }


        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctsesc?.Cancel();
                    if (_taskspinner != null)
                    {
                        if (!_taskspinner.IsCanceled)
                        {
                            _taskspinner?.Wait(CancellationToken.None);
                        }
                    }
                    _taskspinner?.Dispose();
                    _lnkcts?.Dispose();
                    _ctsesc?.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion

        #region IControlSelectBrowser

        public IControlBrowserMultiSelect AddFixedSelect(params string[] values)
        {
            foreach (var item in values)
            {
                _options.FixedSelected.Add(item);
            }
            return this;
        }

        public IControlBrowserMultiSelect FilterType(FilterMode value)
        {
            _options.FilterType = value;
            return this;
        }

        public IControlBrowserMultiSelect DisabledRecursiveExpand(bool value = true)
        {
            if (_options.ExpandAll)
            {
                throw new PromptPlusException("DisabledRecursiveExpand cannot be used when Root setted with expandall = true");
            }
            _options.DisabledRecursiveExpand = value;
            _options.ExpandAll = false;
            return this;
        }

        public IControlBrowserMultiSelect NoSpinner(bool value = true)
        {
            if (value)
            {
                _options.Spinner = null;
            }
            else
            {
                _options.Spinner = new Spinners(SpinnersType.Ascii, ConsolePlus.IsUnicodeSupported);
            }
            return this;
        }

        public IControlBrowserMultiSelect SelectAll(Func<ItemBrowser, bool> selectAllExpression = null)
        {
            _options.SelectAll = true;
            _options.SelectAllExpression = selectAllExpression;
            return this;
        }

        public IControlBrowserMultiSelect Range(int minvalue, int? maxvalue = null)
        {
            if (!maxvalue.HasValue)
            {
                maxvalue = _options.Maximum;
            }
            if (minvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue})");
            }
            if (maxvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. maxvalue({maxvalue})");
            }
            if (minvalue > maxvalue)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue}) > maxvalue({maxvalue})");
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue.Value;
            return this;
        }

        public IControlBrowserMultiSelect AcceptHiddenAttributes(bool value = true)
        {
            _options.AcceptHiddenAttributes = value;
            return this;
        }

        public IControlBrowserMultiSelect AcceptSystemAttributes(bool value = true)
        {
            _options.AcceptSystemAttributes = value;
            return this;
        }

        public IControlBrowserMultiSelect AfterCollapsed(Action<ItemBrowser> value)
        {
            _options.AfterCollapsed = value;
            return this;
        }

        public IControlBrowserMultiSelect AfterExpanded(Action<ItemBrowser> value)
        {
            _options.AfterExpanded = value;
            return this;
        }

        public IControlBrowserMultiSelect BeforeCollapsed(Action<ItemBrowser> value)
        {
            _options.BeforeCollapsed = value;
            return this;
        }

        public IControlBrowserMultiSelect BeforeExpanded(Action<ItemBrowser> value)
        {
            _options.BeforeExpanded = value;
            return this;
        }

        public IControlBrowserMultiSelect Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlBrowserMultiSelect Default(string value)
        {
            if (!string.IsNullOrEmpty(_options.DefautPath))
            {
                if (_options.DefautPath.StartsWith(_options.RootFolder))
                {
                    throw new PromptPlusException($"Defaut Path({value}) Not child of Root Folder({_options.RootFolder}). Set root first!");
                }
            }
            _options.DefautPath = value;
            return this;
        }

        public IControlBrowserMultiSelect HotKeyToggleExpand(HotKey value)
        {
            _options.HotKeyToggleExpandPress = value;
            return this;
        }

        public IControlBrowserMultiSelect HotKeyToggleExpandAll(HotKey value)
        {
            _options.HotKeyToggleExpandAllPress = value;
            return this;
        }

        public IControlBrowserMultiSelect HotKeyFullPath(HotKey value)
        {
            _options.HotKeyTooltipFullPath = value;
            return this;
        }

        public IControlBrowserMultiSelect OnlyFolders(bool value = true)
        {
            _options.OnlyFolders = value;
            return this;
        }

        public IControlBrowserMultiSelect PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlBrowserMultiSelect Root(string value,bool expandall, Func<ItemBrowser, bool>? validselect = null, Func<ItemBrowser, bool>? setdisabled = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (expandall && _options.DisabledRecursiveExpand)
            {
                throw new PromptPlusException("expandall = true cannot be used with DisabledRecursiveExpand");
            }
            if (!string.IsNullOrEmpty(_options.DefautPath))
            {
                if (_options.DefautPath.StartsWith(_options.RootFolder))
                {
                    throw new PromptPlusException($"Defaut Path({_options.DefautPath}) Not child of Root Folder({value})");
                }
            }
            _options.RootFolder = value;
            _options.ExpandAll = expandall;
            _options.ExpressionSelected = validselect;
            _options.ExpressionDisabled = setdisabled;
            return this;
        }

        public IControlBrowserMultiSelect SearchFilePattern(string value)
        {
            _options.SearchFilePattern = value;
            return this;
        }

        public IControlBrowserMultiSelect SearchFolderPattern(string value)
        {
            _options.SearchFolderPattern = value;
            return this;
        }

        public IControlBrowserMultiSelect ShowCurrentFolder(bool value = true)
        {
            _options.ShowCurrentFolder = value;
            return this;
        }

        public IControlBrowserMultiSelect ShowExpand(bool value = true)
        {
            _options.ShowExpand = value;
            return this;
        }

        public IControlBrowserMultiSelect ShowLines(bool value = true)
        {
            _options.ShowLines = value;
            return this;
        }

        public IControlBrowserMultiSelect ShowSize(bool value = true)
        {
            _options.ShowSize = value;
            return this;
        }

        public IControlBrowserMultiSelect Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
        {
            if (spinnersType == SpinnersType.Custom && customspinner.Any())
            {
                throw new PromptPlusException("Custom spinner not have data");
            }
            if (spinnersType == SpinnersType.Custom)
            {
                _options.Spinner = new Spinners(SpinnersType.Custom, ConsolePlus.IsUnicodeSupported, speedAnimation ?? 80, customspinner);
            }
            else
            {
                _options.Spinner = new Spinners(spinnersType, ConsolePlus.IsUnicodeSupported);
            }
            return this;
        }

        public IControlBrowserMultiSelect Styles(BrowserStyles content, Style value)
        {
            _options.StyleControl(content, value);
            return this;
        }

        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_options.RootFolder))
            {
                throw new PromptPlusException($"RootFolder({_options.RootFolder}) not found");
            }
            if (!string.IsNullOrEmpty(_options.DefautPath) && !Directory.Exists(_options.DefautPath))
            {
                throw new PromptPlusException($"DefautPath({_options.DefautPath}) not found");
            }
            _ctsesc = new CancellationTokenSource();
            _lnkcts = CancellationTokenSource.CreateLinkedTokenSource(_ctsesc.Token, cancellationToken);

            _browserTreeView = new TreeView<ItemBrowser>(_options.ExpressionSelected)
            {
                TextTree = (item) => item.Name
            };

            FinishResult = string.Empty;
            return FinishResult;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_firstLoad)
            {
                var oldcur = ConsolePlus.CursorVisible;
                ConsolePlus.SetCursorPosition(0, ConsolePlus.CursorTop);
                var (CursorLeft, CursorTop) = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);

                _firstLoad = false;
                ConsolePlus.CursorVisible = false;
                var top = ConsolePlus.CursorTop;
                var qtd = 0;
                if (!string.IsNullOrEmpty(_options.OptPrompt) && !_options.OptMinimalRender)
                {
                    qtd = ConsolePlus.Write($"{_options.OptPrompt}: ", _options.StyleContent(StyleControls.Prompt), true);
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        (CursorLeft, CursorTop) = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                }
                if (!_options.OptMinimalRender)
                {
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write($"... ", _options.StyleContent(StyleControls.Answer), false);
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        (CursorLeft, CursorTop) = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                }

                _cusorSpinner = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);

                if (_options.Spinner != null)
                {
                    ConsolePlus.WriteLine();
                }

                if (!string.IsNullOrEmpty(_options.OptDescription) && !_options.OptMinimalRender)
                {
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.WriteLine();
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(_options.OptDescription, _options.StyleContent(StyleControls.Description));
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }

                }
                if (_options.Spinner != null)
                {
                    _loadFolderFinish = false;
                    _taskspinner = Task.Run(() =>
                    {
                        WriteSpinner(_cusorSpinner, _lnkcts.Token);
                    }, CancellationToken.None);
                }
                FistLoadRoot(_lnkcts.Token);
                _loadFolderFinish = true;
                _taskspinner.Wait(CancellationToken.None);
                ConsolePlus.SetCursorPosition(CursorLeft, CursorTop);
                ConsolePlus.Write("", _options.StyleContent(StyleControls.Prompt), true);
                ConsolePlus.CursorVisible = oldcur;
                if (_localpaginator.Count > 0)
                {
                    FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
                }
            }
            screenBuffer.WritePrompt(_options, "");
            var hasprompt = (_options.OptPrompt ?? string.Empty).Length > 0 && !_options.OptMinimalRender;

            if (_filterBuffer.Length > 0)
            {
                hasprompt = true;
                if (_localpaginator.TryGetSelected(out var showItem))
                {
                    var item = showItem.Value.Name;
                    screenBuffer.WriteFilterBrowserMultiSelect(_options, item,_filterBuffer);
                    if (!_options.OptMinimalRender)
                    {
                        screenBuffer.WriteTaggedInfo(_options, $" ({Messages.Filter})");
                    }
                }
                else
                {
                    screenBuffer.WriteEmptyFilter(_options, _filterBuffer.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteEmptyFilter(_options, _filterBuffer.ToForward());
                }
            }
            else
            {
                if (!_options.OptMinimalRender)
                {
                    if (hasprompt)
                    {
                        screenBuffer.SaveCursor();
                    }
                    screenBuffer.WriteAnswer(_options, FinishResult);
                    //try save cursor
                    screenBuffer.SaveCursor();
                }
            }
            if (!string.IsNullOrEmpty(_options.OptDescription) && !_options.OptMinimalRender)
            {
                if (hasprompt)
                {
                    screenBuffer.NewLine();
                }
                hasprompt = true;
                screenBuffer.AddBuffer(_options.OptDescription, _options.StyleContent(StyleControls.Description));
            }
            var subset = _localpaginator.GetPageData();
            if (_options.ShowCurrentFolder)
            {
                if (_localpaginator.TryGetSelected(out var showItem))
                {
                    if (hasprompt)
                    {
                        screenBuffer.NewLine();
                    }
                    if (_options.ShowCurrentFullPath)
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentSelected}: {showItem.Value.FullPath}", _options.StyleContent(StyleControls.TaggedInfo).Overflow(Overflow.Crop), true);
                    }
                    else
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentFolder}: {showItem.Value.CurrentFolder}", _options.StyleContent(StyleControls.TaggedInfo).Overflow(Overflow.Crop), true);
                    }
                    //try save cursor
                    screenBuffer.SaveCursor();
                }
            }
            _localpaginator.TryGetSelected(out var selectedItem);
            foreach (var item in subset)
            {
                if (EqualityComparer<ItemTreeViewFlatNode<ItemBrowser>>.Default.Equals(item, selectedItem))
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineNotDisabledMultiSelectorBrowser(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineMultiSelectorBrowser(_options, item);
                    }
                }
                else
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineDisabledNotMultiSelectorBrowser(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotMultiSelectorBrowser(_options, item);
                    }
                }
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePaginationMultiSelect(_options, _localpaginator.PaginationMessage(_options.OptPaginationTemplate), _selectedItems.Count);
            }
            else
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{_options.Symbol(SymbolType.Selected)}: {_selectedItems.Count}", _options.StyleContent(StyleControls.TaggedInfo), true);
            }
            if (_localpaginator.TryGetSelected(out var selitem))
            {
                screenBuffer.WriteLineTooltipsMultiSelectBrowser(_options, selitem);
            }
        }

        public override ResultPrompt<ItemBrowser[]> TryResult(CancellationToken cancellationToken)
        {
            if (_lnkcts.IsCancellationRequested && _options.OptEnabledAbortKey)
            {
                return new ResultPrompt<ItemBrowser[]>(Array.Empty<ItemBrowser>(), true,false);
            }
            if (ConsolePlus.CursorLeft + FinishResult.Length + 1 > ConsolePlus.BufferWidth)
            {
                _cusorSpinner = (ConsolePlus.BufferWidth - 2, ConsolePlus.CursorTop);
            }
            else
            {
                _cusorSpinner = (ConsolePlus.CursorLeft + FinishResult.Length + 1, ConsolePlus.CursorTop);
            }
            var endinput = false;
            var abort = false;
            bool tryagain;
            if (!_loadFolderFinish)
            {
                while (KeyAvailable)
                {
                    _ = WaitKeypress(_lnkcts.Token);
                }
                if (_options.Spinner == null)
                {
                    cancellationToken.WaitHandle.WaitOne(10);
                }
                return new ResultPrompt<ItemBrowser[]>(_selectedItems.Select(x => x.value).ToArray(), _lnkcts.Token.IsCancellationRequested, true, true, false);
            }
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
                    break;
                }
                if (CheckAbortKey(keyInfo.Value))
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (IskeyPageNavegator(keyInfo.Value, _localpaginator))
                {
                    break;
                }
                else if (keyInfo.Value.IsPressHomeKey())
                {
                    _localpaginator.Home();
                    break;
                }
                else if (keyInfo.Value.IsPressSpaceKey())
                {
                    _filterBuffer.Clear();
                    ItemTreeViewFlatNode<ItemBrowser> currentItem = null;
                    if (_localpaginator.SelectedIndex >= 0)
                    {
                        currentItem = _localpaginator.SelectedItem;
                    }
                    if (currentItem != null)
                    {
                        var fnode = _browserTreeView.FindNode(currentItem.UniqueId);
                        if (fnode.IsMarked)
                        {
                            _browserTreeView.UnSelectectAll(fnode);
                            RemoveSelectAll(fnode);
                        }
                        else
                        {
                            var aux = _selectedItems.ToArray();
                            _browserTreeView.SelectAll(fnode);
                            AddSelectAll(fnode);
                            if (_selectedItems.Count > _options.Maximum)
                            {
                                _browserTreeView.UnSelectectAll(fnode);
                                _selectedItems = aux.ToList();
                                if (_filterBuffer.Length > 0)
                                {
                                    _filterBuffer.Clear();
                                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                                }
                                SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                            }
                        }
                        LoadFlatNodes(fnode, true);
                        break;
                    }
                    if (!tryagain)
                    {
                        break;
                    }
                }
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    if (_selectedItems.Count < _options.Minimum)
                    {
                        _filterBuffer.Clear();
                        endinput = false;
                        SetError(string.Format(Messages.MultiSelectMinSelection, _options.Minimum));
                    }
                    else
                    {
                        endinput = true;
                    }
                    break;
                }
                else if (_options.FilterType != FilterMode.Disabled && _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    break;
                }
                else if (_options.HotKeyTooltipFullPath.Equals(keyInfo.Value))
                {
                    _options.ShowCurrentFullPath = !_options.ShowCurrentFullPath;
                    break;
                }
                else if (_options.HotKeyToggleExpandPress.Equals(keyInfo.Value) && _localpaginator.SelectedItem.Value.IsFolder)
                {
                    _filterBuffer.Clear();
                    var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                    if (fnode.IsExpanded && !fnode.IsRoot)
                    {
                        var oldcur = ConsolePlus.CursorVisible;
                        if (_options.Spinner != null)
                        {
                            ConsolePlus.CursorVisible = false;
                            _loadFolderFinish = false;
                            _taskspinner = Task.Run(() =>
                            {
                                WriteSpinner(_cusorSpinner, _lnkcts.Token);
                            }, CancellationToken.None);
                        }
                        _options.BeforeCollapsed?.Invoke(fnode.Value);
                        _browserTreeView.CollapseAll(fnode);
                        _options.AfterCollapsed?.Invoke(fnode.Value);
                        _loadFolderFinish = true;
                        _taskspinner.Wait(CancellationToken.None);
                        LoadFlatNodes(fnode, true);
                        ConsolePlus.CursorVisible = oldcur;
                    }
                    else
                    {
                        var oldcur = ConsolePlus.CursorVisible;
                        if (_options.Spinner != null)
                        {
                            ConsolePlus.CursorVisible = false;
                            _loadFolderFinish = false;
                            _taskspinner = Task.Run(() =>
                            {
                                WriteSpinner(_cusorSpinner, _lnkcts.Token);
                            }, CancellationToken.None);
                        }
                        _options.BeforeExpanded?.Invoke(fnode.Value);
                        TryLoadFolder(false, fnode, null, false, cancellationToken);
                        _browserTreeView.Expand(fnode);
                        _options.AfterExpanded?.Invoke(fnode.Value);
                        _loadFolderFinish = true;
                        _taskspinner.Wait(CancellationToken.None);
                        LoadFlatNodes(fnode, true);
                        ConsolePlus.CursorVisible = oldcur;
                    }
                    break;
                }
                else if (_options.HotKeyToggleExpandAllPress.Equals(keyInfo.Value) && _localpaginator.SelectedItem.Value.IsFolder)
                {
                    _filterBuffer.Clear();
                    var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                    var IsExpand = fnode.IsExpanded;
                    if (fnode.IsRoot)
                    {
                        _rootExpand = !_rootExpand;
                        IsExpand = _rootExpand;
                    }
                    if (fnode.HasAnyNotExpand())
                    {
                        IsExpand = false;
                    }
                    if (IsExpand)
                    {
                        var oldcur = ConsolePlus.CursorVisible;
                        if (_options.Spinner != null)
                        {
                            ConsolePlus.CursorVisible = false;
                            _loadFolderFinish = false;
                            _taskspinner = Task.Run(() =>
                            {
                                WriteSpinner(_cusorSpinner, _lnkcts.Token);
                            }, CancellationToken.None);
                        }
                        _options.BeforeCollapsed?.Invoke(fnode.Value);
                        _browserTreeView.CollapseAll(fnode);
                        _options.AfterCollapsed?.Invoke(fnode.Value);
                        _loadFolderFinish = true;
                        _taskspinner.Wait(CancellationToken.None);
                        LoadFlatNodes(fnode, true);
                        ConsolePlus.CursorVisible = oldcur;
                    }
                    else
                    {
                        var oldcur = ConsolePlus.CursorVisible;
                        if (_options.Spinner != null)
                        {
                            ConsolePlus.CursorVisible = false;
                            _loadFolderFinish = false;
                            _taskspinner = Task.Run(() =>
                            {
                                WriteSpinner(_cusorSpinner, _lnkcts.Token);
                            }, CancellationToken.None);
                        }
                        _options.BeforeExpanded?.Invoke(fnode.Value);
                        if (_options.DisabledRecursiveExpand)
                        {
                            TryLoadFolder(false, fnode, null, false, cancellationToken);
                            _browserTreeView.Expand(fnode);
                        }
                        else
                        {
                            TryLoadFolder(false, fnode, null, true, cancellationToken);
                            _browserTreeView.ExpandAll(fnode);
                        }
                        _options.AfterExpanded?.Invoke(fnode.Value);
                        _loadFolderFinish = true;
                        _taskspinner.Wait(CancellationToken.None);
                        LoadFlatNodes(fnode, true);
                        ConsolePlus.CursorVisible = oldcur;
                    }
                    break;
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
                        if (KeyAvailable)
                        {
                            tryagain = true;
                        }
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                _filterBuffer.Clear();
                _localpaginator.UnSelected();
                endinput = true;
                abort = true;
            }
            else if (_selectedItems.Count > _options.Maximum)
            {
                _filterBuffer.Clear();
                endinput = false;
                SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
            }
            FinishResult = string.Empty;
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            if (_selectedItems.Any())
            {
                FinishResult = string.Join(", ", _selectedItems.Select(x => x.value.Name));
                return new ResultPrompt<ItemBrowser[]>(_selectedItems.Select(x => x.value).ToArray(), abort, !endinput, notrender);
            }
            return new ResultPrompt<ItemBrowser[]>(Array.Empty<ItemBrowser>(), abort, !endinput, notrender);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ItemBrowser[] result, bool aborted)
        {
            if (_options.OptMinimalRender)
            {
                return;
            }
            string answer = string.Join(", ", result.Select(x => x.FullPath));
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            Dispose();
        }

        private void RemoveSelectAll(TreeNode<ItemBrowser> node)
        {
            var index = _selectedItems.FindIndex(x => x.UniqueId == node.UniqueId);
            if (index >= 0 && !IsFixedSelect(node))
            {
                _selectedItems.RemoveAt(index);
            }
            if (node.Childrens != null)
            {
                foreach (var child in node.Childrens)
                {
                    RemoveSelectAll(child);
                }
            }
        }

        private void AddSelectAll(TreeNode<ItemBrowser> node)
        {
            if (node == null)
            {
                return;
            }
            var index = _selectedItems.FindIndex(x => x.UniqueId == node.UniqueId);
            if (index < 0 && node.IsSelected)
            {
                _selectedItems.Add(new(node.UniqueId, node.Value));
            }
            if (node.Childrens != null)
            {
                foreach (var child in node.Childrens)
                {
                    AddSelectAll(child);
                }
            }
        }

        private void LoadFlatNodes(TreeNode<ItemBrowser> defaultnodeselected, bool updatePaginator)
        {
            _flatnodes.Clear();
            if (_browserTreeView.Root == null)
            {
                return;
            }
            var nodeselect = _browserTreeView.Root;
            _flatnodes.Add(new ItemTreeViewFlatNode<ItemBrowser>
            {
                UniqueId = nodeselect.UniqueId,
                IsDisabled = nodeselect.IsDisabled,
                Value = nodeselect.Value,
                MessagesNodes = ShowItem(nodeselect),
                IsRoot = true
            });
            nodeselect = nodeselect.NextNode;

            while (nodeselect?.NextNode != null)
            {
                if ((!nodeselect.IsHasChild && nodeselect.Parent.IsExpanded) || nodeselect.Parent.IsExpanded || nodeselect.IsRoot)
                {
                    _flatnodes.Add(new ItemTreeViewFlatNode<ItemBrowser>
                    {
                        UniqueId = nodeselect.UniqueId,
                        IsDisabled = nodeselect.IsDisabled,
                        Value = nodeselect.Value,
                        MessagesNodes = ShowItem(nodeselect),
                        IsRoot = false
                    });
                }
                nodeselect = nodeselect.NextNode;
            }
            if (nodeselect != null && (nodeselect.Parent.IsExpanded || nodeselect.IsRoot))
            {
                _flatnodes.Add(new ItemTreeViewFlatNode<ItemBrowser>
                {
                    UniqueId = nodeselect.UniqueId,
                    Value = nodeselect.Value,
                    MessagesNodes = ShowItem(nodeselect),
                    IsRoot = false,
                    IsDisabled = nodeselect.IsDisabled
                });
            }

            if (updatePaginator)
            {
                _localpaginator = new Paginator<ItemTreeViewFlatNode<ItemBrowser>>(
                    _options.FilterType,
                    _flatnodes,
                    _options.PageSize,
                    Optional<ItemTreeViewFlatNode<ItemBrowser>>.Set(new ItemTreeViewFlatNode<ItemBrowser> { UniqueId = defaultnodeselected.UniqueId, IsDisabled = defaultnodeselected.IsDisabled,  IsRoot = defaultnodeselected.IsRoot, Value = defaultnodeselected.Value, MessagesNodes = ShowItem(defaultnodeselected) }),
                    (item1,item2) => item1.UniqueId == item2.UniqueId,
                    (item) => item.Value.Name);
            }
            _browserTreeView.SetCurrentNode(nodeselect);
        }

        private void FistLoadRoot(CancellationToken cancellationToken)
        {
            TryLoadFolder(true, null, _options.RootFolder, _options.ExpandAll, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                _browserTreeView = new TreeView<ItemBrowser>(_options.ExpressionSelected)
                {
                    TextTree = (item) => item.Name
                };
            }
            else
            {
                _browserTreeView.SetCurrentNode(_browserTreeView.Root);
                if (_options.SelectAll)
                {
                    _browserTreeView.SelectAll(_browserTreeView.Root, _options.SelectAllExpression);
                }
                if (_options.ExpandAll)
                {
                    _browserTreeView.ExpandAll(_browserTreeView.Root);
                }
                else
                {
                    _browserTreeView.Expand(_browserTreeView.Root);
                }
            }
            InitSelectedNodes();
        }

        private ItemShowTreeView ShowItem(TreeNode<ItemBrowser> item)
        {
            var result = new ItemShowTreeView
            {
                TextItem = item.Text
            };
            if (item.Level == 0)
            {
                result.TextSelected = item.IsSelected ? $" {_options.Symbol(SymbolType.Selected)} " : $" {_options.Symbol(SymbolType.NotSelect)} ";
                return result;
            }

            var level = item.Level;
            var auxline = new string[level];
            if (item.NextNode == null)
            {
                if (_options.ShowLines)
                {
                    auxline[level - 1] = _options.Symbol(SymbolType.TreeLinecorner);
                }
            }
            else
            {
                if (_options.ShowLines)
                {
                    if (item.NextNode.Level < item.Level)
                    {
                        auxline[level - 1] = _options.Symbol(SymbolType.TreeLinecorner);
                    }
                    else
                    {
                        auxline[level - 1] = _options.Symbol(SymbolType.TreeLinecross);
                    }
                }
            }
            level--;
            var node = item.Parent;
            if (node.Level == 0 && node.Childrens.Last().UniqueId == item.UniqueId)
            {
                auxline[level] = _options.Symbol(SymbolType.TreeLinecorner);
            }
            while (level > 0)
            {
                var islast = node.NextNode == null || node.IsParentLast();
                if (_options.ShowLines)
                {
                    if (islast)
                    {
                        auxline[level - 1] = _options.Symbol(SymbolType.TreeLinespace); 
                    }
                    else
                    {
                        auxline[level - 1] = _options.Symbol(SymbolType.TreeLinevertical); 
                    }
                }
                else
                {
                    auxline[level - 1] = _options.Symbol(SymbolType.TreeLinespace);
                }
                node = node.Parent;
                level--;
            }
            if (item.IsParentLast())
            {
                auxline[^1] = _options.Symbol(SymbolType.TreeLinecorner);
            }
            foreach (var itemaux in auxline)
            {
                result.TextLines += itemaux;
            }
            result.TextSelected = item.IsSelected ? $" {_options.Symbol(SymbolType.Selected)} " : $" {_options.Symbol(SymbolType.NotSelect)} ";
            if (item.Value.IsFolder)
            {
                if (_options.ShowSize)
                {
                    if (item.Childrens != null)
                    {
                        result.TextSize = $" ({item.Value.Length} items)";
                    }
                }
                if (_options.ShowExpand)
                {
                    result.TextExpand = item.IsExpanded ? _options.Symbol(SymbolType.Expanded) : _options.Symbol(SymbolType.Collapsed);
                }
            }
            else
            {
                if (_options.ShowSize)
                {
                    result.TextSize = $" ({item.BytesToString()})";
                }
            }

            return result;
        }

        private void WriteSpinner((int CursorLeft, int CursorTop) cursor, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                lock (_root)
                {
                    if (_loadFolderFinish)
                    {
                        break;
                    }
                    while (KeyAvailable)
                    {
                        var keypress = WaitKeypress(_lnkcts.Token);
                        if (_options.OptEnabledAbortKey && keypress != null)
                        {
                            if (CheckAbortKey(keypress.Value))
                            {
                                while (KeyAvailable)
                                {
                                    _ = WaitKeypress(_lnkcts.Token);
                                }
                                _ctsesc.Cancel();
                                break;
                            }
                        }
                    }
                    if (_loadFolderFinish)
                    {
                        break;
                    }
                    if (_options.Spinner != null)
                    {
                        ConsolePlus.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                        var spn = _options.Spinner.NextFrame(cancellationToken);
                        ConsolePlus.Write($"{spn}", _options.StyleContent(StyleControls.Spinner).Overflow(Overflow.Crop), true);
                    }
                }
            }
        }

        private TreeNode<ItemBrowser> TryLoadFolder(bool refresh, TreeNode<ItemBrowser> node, string pathroot,bool allnodes, CancellationToken cancellationToken)
        {
            if (pathroot == null)
            {
                if (node == null)
                {
                    return null;
                }
                if (!node.Value.IsFolder)
                {
                    return node;
                }
            }
            else
            {
                var rootdi = new DirectoryInfo(pathroot);
 
                node = _browserTreeView.AddRootNode(new ItemBrowser
                {
                    CurrentFolder = rootdi.Name,
                    FullPath = rootdi.FullName,
                    IsFolder = true,
                    Name = rootdi.Name
                });
                node.IsDisabled = _options.ExpressionDisabled?.Invoke(node.Value) ?? false;
                if (!node.IsDisabled)
                {
                    node.IsSelected = _options.ExpressionSelected?.Invoke(node.Value) ?? false;
                }
                if (!node.IsSelected && IsFixedSelect(node))
                {
                    node.IsSelected = true;
                    node.IsDisabled = true;
                }
            }
            var loadfiles = false;
            if (node.Childrens == null || refresh)
            {
                node.Childrens = new();
                loadfiles = true;
            }
            IEnumerable<FileSystemInfo> infobrowser = Array.Empty<FileSystemInfo>();
            if (loadfiles)
            {
                var di = new DirectoryInfo(node.Value.FullPath);

                try
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        infobrowser = di.EnumerateDirectories(_options.SearchFolderPattern, SearchOption.TopDirectoryOnly);
                    }
                    if (!cancellationToken.IsCancellationRequested && !_options.OnlyFolders)
                    {
                        if (!infobrowser.Any())
                        {
                            infobrowser = Array.Empty<FileSystemInfo>();
                        }
                        infobrowser = infobrowser.Concat(di.EnumerateFiles(_options.SearchFilePattern, SearchOption.TopDirectoryOnly));
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    //Access error
                }
            }
            var lastnextnode = node.NextNode;
            var index = -1;
            foreach (FileSystemInfo info in infobrowser.OrderBy(x => x.Name))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return node;
                }

                var isFolder = info.Attributes.HasFlag(FileAttributes.Directory);

                ItemBrowser item = null;
                bool normalattrb = !(info.Attributes.HasFlag(FileAttributes.System) || info.Attributes.HasFlag(FileAttributes.Hidden));
                if ((info.Attributes.HasFlag(FileAttributes.System) && _options.AcceptSystemAttributes) || (info.Attributes.HasFlag(FileAttributes.Hidden) && _options.AcceptHiddenAttributes) || normalattrb)
                {
                    item = new ItemBrowser
                    {
                        CurrentFolder = isFolder ? info.Name : new DirectoryInfo(((FileInfo)info).DirectoryName).Name,
                        FullPath = info.FullName,
                        IsFolder = isFolder,
                        Name = info.Name,
                        Length = isFolder ? 0 : ((FileInfo)info).Length,
                    };
                }
                if (item != null)
                {
                    index++;
                    var newnode = _browserTreeView.AddNode(node, item);
                    newnode.IsDisabled = _options.ExpressionDisabled?.Invoke(newnode.Value) ?? false;
                    if (!newnode.IsDisabled)
                    {
                        newnode.IsSelected = _options.ExpressionSelected?.Invoke(newnode.Value) ?? false;
                    }
                    if (!newnode.IsSelected && IsFixedSelect(newnode))
                    {
                        newnode.IsSelected = true;
                        newnode.IsDisabled = true;
                    }
                }
            }
            if (index >= 0)
            {
                node.Childrens.Last().NextNode = lastnextnode;
            }
            if (allnodes && node.IsHasChild)
            {
                foreach (var item in node.Childrens.Where(x => x.Value.IsFolder))
                {
                    TryLoadFolder(false, item, null, allnodes, cancellationToken);
                }
            }
            node.UpdateTreeLength<ItemBrowser>();
            return node;
        }

        private bool IsFixedSelect(TreeNode<ItemBrowser> item)
        {
            if (_options.ExpressionSelected?.Invoke(item.Value) ?? true)
            {
                return _options.FixedSelected.Any(x => x.Equals(item.Value.FullPath, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }

        private void InitSelectedNodes()
        {
            Optional<ItemTreeViewFlatNode<ItemBrowser>> defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.Empty();
            var nodeselect = _browserTreeView.Root;
            _selectedItems.Clear();
            AddSelectAll(nodeselect);
            LoadFlatNodes(_browserTreeView.CurrentNode, false);

            var defopt = _options.DefautPath;

            if (!string.IsNullOrEmpty(defopt))
            {
                foreach (var item in _flatnodes.Where(x => x.Value.FullPath.Equals(defopt)))
                {
                    defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.Set(item);
                }
            }
            else
            {
                var node = _browserTreeView.Root;
                if (node != null)
                {
                    defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.Set(new ItemTreeViewFlatNode<ItemBrowser> { UniqueId = node.UniqueId, IsDisabled = node.IsDisabled, IsRoot = node.IsRoot, Value = node.Value, MessagesNodes = ShowItem(node) });
                }
            }


            _localpaginator = new Paginator<ItemTreeViewFlatNode<ItemBrowser>>(
                _options.FilterType,
                _flatnodes,
                _options.PageSize,
                defvalue,
                (item1,item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.Value.Name);

            FinishResult = string.Empty;
            if (_localpaginator.Count > 0 &&  _localpaginator.SelectedIndex >= 0)
            {
                FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
            }
        }

    }
}
