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
    internal class SelectBrowserControl : BaseControl<ItemBrowser>, IControlSelectBrowser, IDisposable
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
        private bool _rootExpand = true;

        public SelectBrowserControl(IConsoleControl console, BrowserOptions options) : base(console, options)
        {
            _options = options;
            _flatnodes = new();
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
                    _taskspinner.Dispose();
                    _lnkcts?.Dispose();
                    _ctsesc?.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion

        #region IControlSelectBrowser

        public IControlSelectBrowser NoSpinner()
        {
            _options.Spinner = null;
            return this;
        }

        public IControlSelectBrowser DisabledRecursiveExpand()
        {
            _options.DisabledRecursiveExpand = true;
            _options.ExpandAll = false;
            return this;
        }

        public IControlSelectBrowser AcceptHiddenAttributes(bool value)
        {
            _options.AcceptHiddenAttributes = value;
            return this;
        }

        public IControlSelectBrowser FilterType(FilterMode value)
        {
            _options.FilterType = value;
            return this;
        }
        public IControlSelectBrowser AcceptSystemAttributes(bool value)
        {
            _options.AcceptSystemAttributes = value;
            return this;
        }

        public IControlSelectBrowser AfterCollapsed(Action<ItemBrowser> value)
        {
            _options.AfterCollapsed = value;
            return this;
        }

        public IControlSelectBrowser AfterExpanded(Action<ItemBrowser> value)
        {
            _options.AfterExpanded = value;
            return this;
        }

        public IControlSelectBrowser BeforeCollapsed(Action<ItemBrowser> value)
        {
            _options.BeforeCollapsed = value;
            return this;
        }

        public IControlSelectBrowser BeforeExpanded(Action<ItemBrowser> value)
        {
            _options.BeforeExpanded = value;
            return this;
        }

        public IControlSelectBrowser Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlSelectBrowser Default(string value)
        {
            _options.DefautPath = value;
            return this;
        }

        public IControlSelectBrowser HotKeyToggleExpand(HotKey value)
        {
            _options.HotKeyToggleExpandPress = value;
            return this;
        }
        public IControlSelectBrowser HotKeyToggleExpandAll(HotKey value)
        {
            _options.HotKeyToggleExpandAllPress = value;
            return this;
        }

        public IControlSelectBrowser HotKeyFullPath(HotKey value)
        {
            _options.HotKeyTooltipFullPath = value;
            return this;
        }

        public IControlSelectBrowser OnlyFolders(bool value)
        {
            _options.OnlyFolders = value;
            return this;
        }

        public IControlSelectBrowser PageSize(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.PageSize = value;
            return this;
        }

        public IControlSelectBrowser Root(string value, bool expandall = true, Func<ItemBrowser, bool>? validselect = null, Func<ItemBrowser, bool>? setdisabled = null)
        {
            _options.RootFolder = value;
            _options.ExpressionSeleted = validselect;
            _options.ExpressionDisabled = setdisabled;
            _options.ExpandAll = expandall;
            return this;
        }

        public IControlSelectBrowser SearchFilePattern(string value)
        {
            _options.SearchFilePattern = value;
            return this;
        }

        public IControlSelectBrowser SearchFolderPattern(string value)
        {
            _options.SearchFolderPattern = value;
            return this;
        }

        public IControlSelectBrowser ShowCurrentFolder(bool value)
        {
            _options.ShowCurrentFolder = value;
            return this;
        }

        public IControlSelectBrowser ShowExpand(bool value)
        {
            _options.ShowExpand = value;
            return this;
        }

        public IControlSelectBrowser ShowLines(bool value)
        {
            _options.ShowLines = value;
            return this;
        }

        public IControlSelectBrowser ShowSize(bool value)
        {
            _options.ShowSize = value;
            return this;
        }

        public IControlSelectBrowser Spinner(SpinnersType spinnersType, Style? spinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
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
            if (spinnerStyle.HasValue)
            {
                _options.SpinnerStyle = spinnerStyle.Value;
            }
            return this;
        }

        public IControlSelectBrowser Styles(StyleBrowser styletype, Style value)
        {
            value = value.Overflow(Overflow.Crop);
            switch (styletype)
            {
                case StyleBrowser.CurrentFolder:
                    _options.CurrentFolderStyle = value;
                    break;
                case StyleBrowser.UnselectedRoot:
                    _options.RootStyle = value;
                    break;
                case StyleBrowser.SelectedRoot:
                    _options.SelectedRootStyle = value;
                    break;
                case StyleBrowser.Lines:
                    _options.LineStyle = value;
                    break;
                case StyleBrowser.UnselectedSize:
                    _options.SizeStyle = value;
                    break;
                case StyleBrowser.UnselectedExpand:
                    _options.ExpandStyle = value;
                    break;
                case StyleBrowser.UnselectedFolder:
                    _options.FolderStyle = value;
                    break;
                case StyleBrowser.UnselectedFile:
                    _options.FileStyle = value;
                    break;
                case StyleBrowser.SelectedFolder:
                    _options.SelectedFolderStyle = value;
                    break;
                case StyleBrowser.SelectedFile:
                    _options.SelectedFileStyle = value;
                    break;
                case StyleBrowser.SelectedSize:
                    _options.SelectedSizeStyle = value;
                    break;
                case StyleBrowser.SelectedExpand:
                    _options.SelectedExpandStyle = value;
                    break;
                default:
                    throw new PromptPlusException($"StyleBrowser: {styletype} Not Implemented");
            }
            return this;

        }


        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_options.RootFolder))
            {
                throw new PromptPlusException("Not have Root Folder to run");
            }

            if (!string.IsNullOrEmpty(_options.DefautPath))
            {
                if (_options.DefautPath.StartsWith(_options.RootFolder))
                {
                    throw new PromptPlusException("Defaut Path Not child of Root Folder");
                }
            }

            if (_options.DisabledRecursiveExpand)
            {
                _options.ExpandAll = false;
            }

            _ctsesc = new CancellationTokenSource();
            _lnkcts = CancellationTokenSource.CreateLinkedTokenSource(_ctsesc.Token, cancellationToken);

            _browserTreeView = new TreeView<ItemBrowser>(_options.ExpressionSeleted)
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
                var scrool = 0;

                _firstLoad = false;
                ConsolePlus.CursorVisible = false;
                var top = ConsolePlus.CursorTop;
                var qtd = ConsolePlus.Write($"{_options.OptPrompt}: ", _options.OptStyleSchema.Prompt(), true);
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    scrool += dif;
                }

                top = ConsolePlus.CursorTop;
                qtd = ConsolePlus.Write($"{Messages.Loading} ", _options.OptStyleSchema.Answer(), false);
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    scrool += dif;
                }

                _cusorSpinner = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);

                if (!string.IsNullOrEmpty(_options.OptDescription))
                {
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.WriteLine();
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        scrool += dif;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(_options.OptDescription, _options.OptStyleSchema.Description());
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        scrool += dif;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }

                }
                if (_options.OptShowTooltip)
                {
                    var tp = _options.OptToolTip;
                    if (string.IsNullOrEmpty(tp))
                    {
                        tp = ScreenBufferSelectBrowser.DefaultToolTipSelectLoadRoot(_options);
                    }
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.WriteLine();
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        scrool += dif;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(tp, _options.OptStyleSchema.Tooltips());
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        scrool += dif;
                        _cusorSpinner = (_cusorSpinner.CursorLeft, _cusorSpinner.CursorTop - dif);
                    }
                }
                CursorTop -= scrool;
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
                ConsolePlus.Write("", _options.OptStyleSchema.Prompt(), true);
                ConsolePlus.CursorVisible = oldcur;
                FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
            }
            screenBuffer.WritePrompt(_options, "");
            if (_filterBuffer.Length > 0)
            {
                if (_localpaginator.TryGetSelectedItem(out var showItem))
                {
                    screenBuffer.WriteFilterBrowserSelect(_options, FinishResult, _filterBuffer);
                    screenBuffer.WriteTaggedInfo(_options, $" ({Messages.Filter})");
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
                screenBuffer.SaveCursor();
                screenBuffer.WriteAnswer(_options, FinishResult);
            }
            if (!string.IsNullOrEmpty(_options.OptDescription))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(_options.OptDescription, _options.OptStyleSchema.Description());
            }
            var subset = _localpaginator.ToSubset();
            if (_options.ShowCurrentFolder)
            {
                if (_localpaginator.TryGetSelectedItem(out var showItem))
                {
                    screenBuffer.NewLine();
                    if (_options.ShowCurrentFullPath)
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentSeleted}: {showItem.Value.FullPath}", _options.CurrentFolderStyle, true);
                    }
                    else
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentFolder}: {showItem.Value.CurrentFolder}", _options.CurrentFolderStyle, true);
                    }
                }
            }
            screenBuffer.WriteLineValidate(ValidateError, _options);
            if (_localpaginator.TryGetSelectedItem(out var selectedItem))
            {
                screenBuffer.WriteLineTooltipsBrowser(_options, selectedItem);
            }
            foreach (var item in subset)
            {
                if (EqualityComparer<ItemTreeViewFlatNode<ItemBrowser>>.Default.Equals(item, selectedItem))
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineDisabledSelectorBrowser(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineSelectorBrowser(_options, item);
                    }
                }
                else
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineDisabledNotSelectorBrowser(_options, item);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelectorBrowser(_options, item);
                    }
                }
            }
            screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
        }

        public override ResultPrompt<ItemBrowser> TryResult(CancellationToken cancellationToken)
        {
            if (_lnkcts.IsCancellationRequested && _options.OptEnabledAbortKey)
            {
                return new ResultPrompt<ItemBrowser>(null, true, false);
            }
            if (ConsolePlus.CursorLeft + FinishResult.Length + 1 > ConsolePlus.BufferWidth)
            {
                _cusorSpinner = (ConsolePlus.BufferWidth - 1, ConsolePlus.CursorTop);
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
                return new ResultPrompt<ItemBrowser>(_browserTreeView.CurrentNode?.Value, _lnkcts.Token.IsCancellationRequested, true, true, false);
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
                else if (_filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    break;
                }
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    if (_localpaginator.SelectedIndex >= 0)
                    {
                        var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                        if (fnode.IsDisabled)
                        {
                            SetError(Messages.SelectionDisabled);
                        }
                        else
                        {
                            if (!_options.ExpressionSeleted?.Invoke(_localpaginator.SelectedItem.Value) ?? false)
                            {
                                SetError(Messages.SelectionInvalid);
                            }
                            else
                            {
                                endinput = true;
                            }
                        }
                        break;
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMinSelection, 1));
                        break;
                    }
                }
                else if (_options.HotKeyTooltipFullPath.Equals(keyInfo.Value))
                {
                    _options.ShowCurrentFullPath = !_options.ShowCurrentFullPath;
                    break;
                }
                else if (_options.HotKeyToggleExpandPress.Equals(keyInfo.Value) && _localpaginator.SelectedItem.Value.IsFolder)
                {
                    var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                    if (fnode.IsExpanded && !_localpaginator.SelectedItem.IsRoot)
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
                    tryagain = true;
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                _filterBuffer.Clear();
                _localpaginator.UnSelected();
                endinput = true;
                abort = true;
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
            if (_localpaginator.SelectedIndex >= 0)
            {
                FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
                return new ResultPrompt<ItemBrowser>(_localpaginator.SelectedItem.Value, abort, !endinput, notrender);
            }
            return new ResultPrompt<ItemBrowser>(null, abort, !endinput,notrender);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ItemBrowser result, bool aborted)
        {
            string answer;
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            else
            {
                answer = result.FullPath;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            Dispose();
        }

        private void LoadFlatNodes(TreeNode<ItemBrowser> defaultnodeselected, bool updatePaginator)
        {
            _flatnodes.Clear();
            var nodeselect = _browserTreeView.Root;
            _flatnodes.Add(new ItemTreeViewFlatNode<ItemBrowser>
            {
                UniqueId = nodeselect.UniqueId,
                Value = nodeselect.Value,
                MessagesNodes = ShowItem(nodeselect),
                IsRoot = true,
                IsDisabled = nodeselect.IsDisabled
            });
            nodeselect = nodeselect.NextNode;

            while (nodeselect?.NextNode != null)
            {
                if ((!nodeselect.IsHasChild && nodeselect.Parent.IsExpanded) || nodeselect.Parent.IsExpanded || nodeselect.IsRoot)
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
                    Optional<ItemTreeViewFlatNode<ItemBrowser>>.Create(new ItemTreeViewFlatNode<ItemBrowser> { UniqueId = defaultnodeselected.UniqueId, IsDisabled = defaultnodeselected.IsDisabled, IsRoot = defaultnodeselected.IsRoot, Value = defaultnodeselected.Value, MessagesNodes = ShowItem(defaultnodeselected) }),
                    (item1, item2) => item1.UniqueId == item2.UniqueId,
                    (item) => item.Value.Name);
            }
            _browserTreeView.SetCurrentNode(nodeselect);
        }

        private void FistLoadRoot(CancellationToken cancellationToken)
        {
            Optional<ItemTreeViewFlatNode<ItemBrowser>> defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.s_empty;
            TryLoadFolder(true, null, _options.RootFolder, _options.ExpandAll, cancellationToken);
            _browserTreeView.SetCurrentNode(_browserTreeView.Root);
            if (_options.ExpandAll)
            {
                _browserTreeView.ExpandAll(_browserTreeView.Root);
            }
            else
            {
                _browserTreeView.Expand(_browserTreeView.Root);
            }

            LoadFlatNodes(_browserTreeView.CurrentNode, false);

            var defopt = _options.DefautPath;

            if (!string.IsNullOrEmpty(defopt))
            {
                foreach (var item in _flatnodes)
                {
                    if (item.Value.FullPath.Equals(defopt))
                    {
                        defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.Create(item);
                        break;
                    }
                }
            }
            else
            {
                var node = _browserTreeView.Root;
                defvalue = Optional<ItemTreeViewFlatNode<ItemBrowser>>.Create(new ItemTreeViewFlatNode<ItemBrowser> { UniqueId = node.UniqueId, IsDisabled = node.IsDisabled, IsRoot = node.IsRoot, Value = node.Value, MessagesNodes = ShowItem(node) });
            }

            _localpaginator = new Paginator<ItemTreeViewFlatNode<ItemBrowser>>(
                _options.FilterType,
                _flatnodes,
                _options.PageSize,
                defvalue,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => item.Value.Name);

            FinishResult = string.Empty;
            if (_localpaginator.SelectedIndex >= 0)
            {
                FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
            }
        }

        private ItemShowTreeView ShowItem(TreeNode<ItemBrowser> item)
        {
            var result = new ItemShowTreeView
            {
                TextItem = item.Text
            };
            if (item.Level == 0)
            {
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
                auxline[auxline.Length - 1] = _options.Symbol(SymbolType.TreeLinecorner);
            }
            foreach (var itemaux in auxline)
            {
                result.TextLines += itemaux;
            }

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
                        if (_options.OptEnabledAbortKey)
                        {
                            if (keypress != null)
                            {
                                if (CheckAbortKey(keypress.Value))
                                {
                                    while (KeyAvailable)
                                    {
                                        _ = WaitKeypress(_lnkcts.Token);
                                    }
                                    _lnkcts.Cancel();
                                    break;
                                }
                            }
                        }
                    }
                    if (_loadFolderFinish)
                    {
                        break;
                    }
                    var (CursorLeft, CursorTop) = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                    if (CursorLeft != cursor.CursorLeft || CursorTop != cursor.CursorTop)
                    {
                        ConsolePlus.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                    }
                    if (_options.Spinner != null)
                    {
                        var spn = _options.Spinner.NextFrame(cancellationToken);
                        ConsolePlus.Write($"{spn}", _options.SpinnerStyle.Overflow(Overflow.Ellipsis), true);
                    }
                    ConsolePlus.SetCursorPosition(CursorLeft, CursorTop);
                }
            }
        }

        private TreeNode<ItemBrowser> TryLoadFolder(bool refresh, TreeNode<ItemBrowser> node, string pathroot, bool allnodes, CancellationToken cancellationToken)
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
                if (!rootdi.Exists)
                {
                    throw new PromptPlusException("Root not exists");
                }
                node = _browserTreeView.AddRootNode(new ItemBrowser
                {
                    CurrentFolder = rootdi.Name,
                    FullPath = rootdi.FullName,
                    IsFolder = true,
                    Name = rootdi.Name
                });
                node.IsDisabled = _options.ExpressionDisabled?.Invoke(node.Value) ?? false;
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
                if (!di.Exists)
                {
                    throw new PromptPlusException("Node Folder not exists");
                }
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
                catch  (UnauthorizedAccessException)
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
                    _browserTreeView.AddNode(node, item, _options.ExpressionDisabled?.Invoke(item) ?? false);
                }
            }
            if (index >= 0)
            {
                node.Childrens.Last().NextNode = lastnextnode;
            }
            if (allnodes)
            {
                if (node.IsHasChild)
                {
                    foreach (var item in node.Childrens.Where(x => x.Value.IsFolder))
                    {
                        TryLoadFolder(refresh, item, null, allnodes, cancellationToken);
                    }
                }
            }
            node.UpdateTreeLenght<ItemBrowser>();
            return node;
        }
    }
}
