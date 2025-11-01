// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.NodeTreeRemoteMultiSelect
{
    internal sealed class NodeTreeRemoteMultiSelectControl<T1, T2> : BaseControlPrompt<T1[]>, INodeTreeRemoteMultiSelectControl<T1, T2> where T1 : class, new() where T2 : class
    {
        private readonly Dictionary<NodeTreeStyles, Style> _optStyles = BaseControlOptions.LoadStyle<NodeTreeStyles>();
        private readonly List<ItemNodeControl<T1>> _items = [];
        private readonly Func<ItemNodeControl<T1>, bool> IsRoot;
        private Func<T1, string>? _uniqueexpression;
        private Func<T1, T2, (bool, T2, IEnumerable<(bool, T1)>)>? _predicateSearchItems;
        private Func<Exception, string>? _searchItemsErrorMessage;
        private Func<T1, bool>? _predicateChildAllowed;
        private Func<T1, string?>? _extraInfoNode;
        private T2 _searchItemsControl;
        private Task? _loadingItemTask;
        private readonly ConcurrentQueue<(string Key, bool IsLoadMore, bool IsLoad, bool IsFinihed, Exception? Error, List<ItemNodeControl<T1>> Items)> _resultTask = [];
        private Func<T1, string>? _changeDescription;
        private Func<T1, string>? _textSelector;
        private NodeTree<T1>? _nodestree;
        private bool _hideSize;
        private int _indexTooptip;
        private string _tooltipModeSelect = string.Empty;
        private bool _showInfoFullPath;
        private byte _pageSize;
        private Func<T1, (bool, string?)>? _predicatevalidselect;
        private Func<T1, bool>? _predicatevaliddisabled;
        private Paginator<ItemNodeControl<T1>>? _localpaginator;
        private string _nodeseparator = "|";
        private string[] _toggerTooptips;
        private bool _disableRecursiveCount;
        private readonly string _loadMoreId = Guid.NewGuid().ToString();
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private bool _hideCountSelected;
        private byte _maxWidth;
        private readonly List<ItemNodeControl<T1>> _checkeditems = [];
        private EmacsBuffer _resultbuffer;
        private string _lastinput;



#pragma warning disable IDE0079
#pragma warning disable IDE0290 // Use primary constructor
        public NodeTreeRemoteMultiSelectControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _searchItemsControl = default!;
            IsRoot = (item) => item.UniqueId == (_items.Count == 0 ? "" : _items[0].UniqueId);
            _toggerTooptips = [];
            _resultbuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _lastinput = string.Empty;
            _maxWidth = ConfigPlus.MaxWidth;
            _pageSize = ConfigPlus.PageSize;
        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079

        #region INodeTreeRemoteMultiSelectControl

        public INodeTreeRemoteMultiSelectControl<T1, T2> MaxWidth(byte maxWidth)
        {
            if (maxWidth < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 10.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> HideCountSelected(bool value = true)
        {
            _hideCountSelected = value;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue > (maxvalue ?? int.MaxValue))
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minSelect = minvalue;
            _maxSelect = maxvalue ?? int.MaxValue;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> DisableRecursiveCount(bool value = true)
        {
            _disableRecursiveCount = value;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> ExtraInfo(Func<T1, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoNode = extraInfoNode;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> PredicateChildAllowed(Func<T1, bool> childallowed)
        {
            ArgumentNullException.ThrowIfNull(childallowed);
            _predicateChildAllowed = childallowed;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> SearchMoreItems(Func<T1, T2, (bool, T2, IEnumerable<(bool, T1)>)> values, Func<Exception, string>? erroMessage = null)
        {
            ArgumentNullException.ThrowIfNull(values);
            _predicateSearchItems = values;
            _searchItemsErrorMessage = erroMessage;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> ChangeDescription(Func<T1, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> HideCount(bool value = true)
        {
            _hideSize = value;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> Styles(NodeTreeStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> TextSelector(Func<T1, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> UniqueId(Func<T1, string> uniquevalue)
        {
            ArgumentNullException.ThrowIfNull(uniquevalue);
            _uniqueexpression = uniquevalue;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> AddRootNode(T1 value, T2 initialvalue, bool valuechecked, string nodeseparator = "|")
        {
            if (_nodestree != null)
            {
                throw new InvalidOperationException("There is already a root node!");
            }
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(initialvalue);
            ArgumentException.ThrowIfNullOrEmpty(nodeseparator);
            _searchItemsControl = initialvalue;
            _nodestree = new NodeTree<T1>
            {
                Node = value,
                Checked = valuechecked
            };
            _nodeseparator = nodeseparator;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        public INodeTreeRemoteMultiSelectControl<T1, T2> PredicateDisabled(Func<T1, bool> validdisabled)
        {
            ArgumentNullException.ThrowIfNull(validdisabled);
            _predicatevaliddisabled = validdisabled;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            ValidateConstraints();

            _resultbuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);

            InitViewNodes();

            _loadingItemTask = Task.Run(() => LoadMoreItems(_items[0].UniqueId, true), cancellationToken);

            _tooltipModeSelect = GetTooltipModeSelect();
            LoadTooltipToggle();
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteFolderInfo(screenBuffer);

            WriteListSelect(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsoleKeyInfo keyinfo = WaitKeypressDiscovery(cancellationToken);

                    #region default Press to Finish and tooltip

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T1[]>([], true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T1[]>([], true);
                        break;
                    }
                    else if (_loadingItemTask == null && keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.IsLoadMoreNode)
                    {
                        _indexTooptip = 0;
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                            break;
                        }
                        if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                            break;
                        }
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T1[]>([.. _checkeditems.Select(x => x.Value)], false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
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

                    else if (keyinfo.Key == ConsoleKey.None && keyinfo.Modifiers == ConsoleModifiers.Alt)
                    {
                        //has result backgroud task
                        if (_resultTask.TryDequeue(out var resultitems))
                        {
                            if (resultitems.IsLoad)
                            {
                                LoadTaskResult(resultitems, cancellationToken);
                            }
                            else
                            {
                                UnloadTaskResult(resultitems.Key, cancellationToken);
                            }
                            break;
                        }
                    }
                    else if (_loadingItemTask == null && keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null && _localpaginator.SelectedItem.IsLoadMoreNode)
                    {
                        var index = _items.FindIndex(x => x.UniqueId == _localpaginator!.SelectedItem!.UniqueId);
                        _items[index].Status = NodeStatus.Loading;
                        _loadingItemTask = Task.Run(() => LoadMoreItems(_items[index].UniqueId, true), cancellationToken);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        _indexTooptip = 0;
                        if (string.IsNullOrEmpty(_localpaginator!.SelectedItem!.ParentUniqueId))
                        {
                            if (!_localpaginator!.Home())
                            {
                                continue;
                            }
                            break;
                        }
                        int index = _items.FindIndex(x => x.UniqueId == _localpaginator!.SelectedItem!.ParentUniqueId);
                        _localpaginator.EnsureVisibleIndex(index);
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        _indexTooptip = 0;
                        string? parent = _localpaginator!.SelectedItem!.ParentUniqueId;
                        int index = _localpaginator!.SelectedIndex;
                        if (_localpaginator!.SelectedItem.CountChildren > 0 && _localpaginator!.SelectedItem.IsExpanded && _localpaginator!.SelectedItem!.Status == NodeStatus.Done)
                        {
                            parent = _localpaginator!.SelectedItem!.UniqueId;
                            index = _items.FindIndex(x => x.UniqueId == parent) + 1;
                        }
                        while (index < _items.Count)
                        {
                            if (_items[index].LastItem && _items[index].ParentUniqueId == parent)
                            {
                                break;
                            }
                            index++;
                        }
                        _localpaginator!.EnsureVisibleIndex(index);
                        break;
                    }
                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                        {
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localpaginator.NextItem();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator!.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator!.PreviousItem();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (ConfigPlus.HotKeyToggleFullPath.Equals(keyinfo))
                    {
                        _indexTooptip = 0;
                        _showInfoFullPath = !_showInfoFullPath;
                        break;
                    }
                    else if (_loadingItemTask == null && _localpaginator!.SelectedItem != null && !IsRoot(_localpaginator.SelectedItem!) && _localpaginator.SelectedItem.AllowsChildren && "+-".Contains(keyinfo.KeyChar) && keyinfo.Modifiers == ConsoleModifiers.None)
                    {
                        if (keyinfo.KeyChar == '+')
                        {
                            if (_localpaginator.SelectedItem.IsExpanded)
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            _localpaginator.SelectedItem.IsExpanded = true;
                            _localpaginator.SelectedItem.Status = NodeStatus.Loading;
                            _loadingItemTask = Task.Run(() => LoadMoreItems(_localpaginator.SelectedItem.UniqueId, false), cancellationToken);
                            break;
                        }
                        if (keyinfo.KeyChar == '-')
                        {
                            if (!_localpaginator.SelectedItem.IsExpanded)
                            {
                                continue;
                            }
                            if (HasSeletedItems())
                            {
                                SetError(Messages.NotCollapse);
                                break;
                            }
                            _indexTooptip = 0;
                            _localpaginator.SelectedItem.IsExpanded = false;
                            _localpaginator.SelectedItem.Status = NodeStatus.Unloading;
                            _loadingItemTask = Task.Run(() => _resultTask.Enqueue((_localpaginator.SelectedItem.UniqueId, false, false, false, null, [])), cancellationToken);
                            break;
                        }
                        continue;
                    }
                    else if (keyinfo.IsPressCtrlSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.IsDisabled && !_localpaginator.SelectedItem.IsLoadMoreNode)
                    {
                        int index = _items.FindIndex(x => x.UniqueId == _localpaginator.SelectedItem.UniqueId);
                        bool mark = !_items[index].IsMarked;
                        MarkAllNodes(index);
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                        }
                        else if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.IsDisabled && !_localpaginator.SelectedItem.IsLoadMoreNode)
                    {
                        int index = _items.FindIndex(x => x.UniqueId == _localpaginator.SelectedItem.UniqueId);
                        bool mark = !_items[index].IsMarked;
                        _items[index].IsMarked = mark;
                        if (!mark)
                        {
                            int chkindex = _checkeditems.FindIndex(x => x.UniqueId == _items[index].UniqueId);
                            _checkeditems.RemoveAt(chkindex);
                        }
                        else
                        {
                            (bool ok, string? message) = _predicatevalidselect?.Invoke(_items[index].Value) ?? (true, null);
                            if (!ok)
                            {
                                if (string.IsNullOrEmpty(message))
                                {
                                    SetError(Messages.PredicateSelectInvalid);
                                }
                                else
                                {
                                    SetError(message);
                                }
                            }
                            else
                            {
                                _checkeditems.Add(_items[index]);
                            }
                        }
                        if (_checkeditems.Count == 0)
                        {
                            _resultbuffer!.Clear();
                        }
                        else
                        {
                            _resultbuffer!.LoadPrintable(_checkeditems.Select(x => _textSelector!.Invoke(x.Value) ?? string.Empty).Aggregate((x, y) => $"{x},{y}"));
                        }
                        int countselect = _checkeditems.Count;
                        if (countselect < _minSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
                        }
                        else if (countselect > _maxSelect)
                        {
                            SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_resultbuffer!.IsPrintable(keyinfo.KeyChar) && _resultbuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        private void UnloadTaskResult(string key, CancellationToken cancellationToken)
        {
            int index = _items.FindIndex(x => x.UniqueId == key);
            if (index == -1)
            {
                return;
            }
            int posindex = index + 1;
            while (posindex < _items.Count)
            {
                if (_items[posindex].ParentUniqueId != key || cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (_predicateChildAllowed!(_items[posindex].Value) && _items[posindex].Status == NodeStatus.Done)
                {
                    UnloadTaskResult(_items[posindex].UniqueId, cancellationToken);
                }
                int parentindex = -1;
                string? parentid = _items[posindex].ParentUniqueId;
                do
                {
                    parentindex = _items.FindIndex(x => x.UniqueId == parentid);
                    if (parentindex >= 0)
                    {
                        _items[parentindex].CountChildren--;
                        parentid = _items[parentindex].ParentUniqueId;
                    }
                }
                while (parentindex > 0);
                _items.RemoveAt(posindex);
                continue;
            }
            _indexTooptip = 0;
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            _localpaginator!.UpdatColletion(_items, Optional<ItemNodeControl<T1>>.Set(_items[index]));
            _items[index].Status = NodeStatus.NotLoad;
        }

        private void LoadTaskResult((string Key, bool IsLoadMore, bool IsLoad, bool IsFinihed, Exception? Error, List<ItemNodeControl<T1>> Items) taskresult, CancellationToken cancellationToken)
        {
            //find index loaded node
            int index = _items.FindIndex(x => x.UniqueId == taskresult.Key);
            if (index == -1)
            {
                SetError("Internal Error at LoadTaskResult");
                return;
            }
            if (taskresult.Error != null)
            {
                if (_searchItemsErrorMessage != null)
                {
                    SetError(_searchItemsErrorMessage.Invoke(taskresult.Error));
                }
                else
                {
                    SetError(taskresult.Error.Message);
                }
                if (!_items[index].IsLoadMoreNode)
                {
                    _items[index].Status = NodeStatus.NotLoad;
                }
            }
            else
            {
                int posindex = index;
                if (_items[index].IsLoadMoreNode)
                {
                    _items.RemoveAt(index);
                    posindex--;
                }
                foreach (ItemNodeControl<T1> item in taskresult.Items)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    posindex++;
                    _items.Insert(posindex, item);
                    if (item.IsMarked)
                    {
                        _checkeditems.Add(item);
                    }
                }
                if (!taskresult.IsLoadMore)
                {
                    _items[index].Status = NodeStatus.Done;
                }
                //update size in parent nodes
                var curnode = _items[index];
                int parentindex = -1;
                var qtd = taskresult.Items.Count(x => !x.IsLoadMoreNode);
                do
                {
                    curnode.CountChildren += qtd;
                    if (_disableRecursiveCount)
                    {
                        break;
                    }
                    parentindex = _items.FindIndex(x => x.UniqueId == curnode.ParentUniqueId);
                    if (parentindex >= 0)
                    {
                        curnode = _items[parentindex];
                    }
                }
                while (parentindex > 0);
            }
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (_checkeditems.Count == 0)
            {
                _resultbuffer!.Clear();
            }
            else
            {
                _resultbuffer!.LoadPrintable(_checkeditems.Select(x => _textSelector!.Invoke(x.Value) ?? string.Empty).Aggregate((x, y) => $"{x},{y}"));
            }
            int countselect = _checkeditems.Count;
            if (countselect < _minSelect)
            {
                SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
            }
            else if (countselect > _maxSelect)
            {
                SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
            }
            Optional<ItemNodeControl<T1>> defaultvalue = Optional<ItemNodeControl<T1>>.Set(_items[index]);
            _localpaginator!.UpdatColletion(_items, defaultvalue);
            _indexTooptip = 0;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = _localpaginator!.SelectedItem.Text!;
            }
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[NodeTreeStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[NodeTreeStyles.Answer]);

            _showInfoFullPath = true;
            WriteNodeFullpath(screenBuffer);

            return true;
        }

        public override void FinalizeControl()
        {
            if (_loadingItemTask != null)
            {
                _loadingItemTask.Wait();
                _loadingItemTask.Dispose();
            }
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[NodeTreeStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {

            Style styleAnswer = _optStyles[NodeTreeStyles.Answer];
            string str = _resultbuffer!.IsHideLeftBuffer
                ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
            screenBuffer.Write(str, styleAnswer);

            screenBuffer.Write(_resultbuffer!.ToBackward(), styleAnswer);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(_resultbuffer!.ToForward(true), styleAnswer);
            str = _resultbuffer.IsHideRightBuffer
                ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
            screenBuffer.Write(str, styleAnswer);
            if (_loadingItemTask != null)
            {
                screenBuffer.Write($"({Messages.Loading})", styleAnswer);
            }
            screenBuffer.WriteLine("", Style.Default());
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_localpaginator!.SelectedItem.Value) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[NodeTreeStyles.Description]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[NodeTreeStyles.Error]);
                ClearError();
                return;
            }
        }

        private void WriteFolderInfo(BufferScreen screenBuffer)
        {
            if (_showInfoFullPath)
            {
                int pos = _localpaginator!.SelectedItem!.FullPath!.LastIndexOf($"{_nodeseparator}{_localpaginator!.SelectedItem!.Text!}", StringComparison.Ordinal);
                string info;
                if (pos < 0)
                {
                    info = _localpaginator!.SelectedItem!.FullPath;
                }
                else
                {
                    info = _localpaginator!.SelectedItem!.FullPath![..pos];
                }
                if (!string.IsNullOrEmpty(info))
                {
                    screenBuffer.Write(Messages.NodePath, _optStyles[NodeTreeStyles.Answer]);
                    screenBuffer.WriteLine(info, _optStyles[NodeTreeStyles.Answer]);
                }
            }
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
            ];
            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            lsttooltips.Add(Messages.TooltipPages);
            lsttooltips.Add($"{Messages.TooltipExpandAllPress}, {Messages.TooltipLevelHomeEnd}, {Messages.InputFinishEnter}");
            _toggerTooptips = [.. lsttooltips];
        }

        private void WriteListSelect(BufferScreen screenBuffer)
        {
            ArraySegment<ItemNodeControl<T1>> subset = _localpaginator!.GetPageData();
            foreach (ItemNodeControl<T1> item in subset)
            {
                var isSelected = false;
                bool checkroot = IsRoot(item);
                Style stl = _optStyles[NodeTreeStyles.UnSelected];
                Style stlexp = _optStyles[NodeTreeStyles.ExpandSymbol];
                Style stlsiz = _optStyles[NodeTreeStyles.ChildsCount];
                if (item.IsDisabled && !checkroot)
                {
                    stl = _optStyles[NodeTreeStyles.Disabled];
                }
                if (_localpaginator.TryGetSelected(out ItemNodeControl<T1>? selectedItem) && EqualityComparer<ItemNodeControl<T1>>.Default.Equals(item, selectedItem))
                {
                    isSelected = true;
                    stl = _optStyles[NodeTreeStyles.Selected];
                    stlexp = stl;
                    stlsiz = stl;
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", stl);
                    if (item.IsLoadMoreNode)
                    {
                        screenBuffer.Write(new string(' ',ConfigPlus.GetSymbol(SymbolType.Selected).Length), stl);
                    }
                    else
                    {
                        if (item.IsMarked)
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), stl);
                        }
                        else
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), stl);
                        }
                    }
                }
                else
                {
                    if (!item.IsDisabled)
                    {
                        if (checkroot)
                        {
                            stl = _optStyles[NodeTreeStyles.Root];
                        }
                        else
                        {
                            stl = _optStyles[NodeTreeStyles.Node];
                        }
                    }
                    screenBuffer.Write(" ", stl);
                    if (item.IsLoadMoreNode)
                    {
                        screenBuffer.Write(new string(' ', ConfigPlus.GetSymbol(SymbolType.Selected).Length), stl);
                    }
                    else
                    {
                        if (item.IsMarked)
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), stl);
                        }
                        else
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), stl);
                        }
                    }
                }
                screenBuffer.Write(CreateIndentation(item), _optStyles[NodeTreeStyles.Lines]);
                if (item.AllowsChildren && !item.IsLoadMoreNode)
                {
                    if (!checkroot)
                    {
                        if (item.IsExpanded)
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Expanded), stlexp);
                        }
                        else
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Collapsed), stlexp);
                        }
                    }
                }
                if (item.IsLoadMoreNode && !isSelected)
                {
                    screenBuffer.Write(item.Text!, _optStyles[NodeTreeStyles.Answer]);
                }
                else
                {
                    screenBuffer.Write(item.Text!, stl);
                }

                var msgsize = string.Empty;
                if (!item.IsLoadMoreNode)
                {
                    if (!_hideSize && !checkroot)
                    {
                        if (item.Status == NodeStatus.NotLoad)
                        {
                            msgsize = $"(?";
                        }
                        else
                        {
                            if (item.AllowsChildren)
                            {
                                msgsize = $"({item.CountChildren}";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ExtraText))
                    {
                        if (msgsize.Length != 0)
                        {
                            msgsize += ", ";
                            msgsize += item.ExtraText;
                        }
                        else
                        {
                            msgsize = $"({item.ExtraText}";
                        }
                    }
                    if (msgsize.Length != 0)
                    {
                        msgsize += ")";
                    }
                }
                screenBuffer.WriteLine(msgsize, stlsiz);
            }

            string template = ConfigPlus.PaginationTemplate.Invoke(
                            _localpaginator.TotalCountValid,
                            _localpaginator.SelectedPage + 1,
                            _localpaginator.PageCount)!;
            screenBuffer.Write(template, _optStyles[NodeTreeStyles.Pagination]);
            if (!_hideCountSelected)
            {
                screenBuffer.Write(string.Format(Messages.TooltipCountCheck, _checkeditems.Count), _optStyles[NodeTreeStyles.TaggedInfo]);
            }
            screenBuffer.WriteLine("", Style.Default());
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            else
            {
                tooltip = _tooltipModeSelect;
            }
            if (!string.IsNullOrEmpty(tooltip))
            {
                screenBuffer.Write(tooltip, _optStyles[NodeTreeStyles.Tooltips]);
            }
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private string CreateIndentation(ItemNodeControl<T1> item)
        {
            StringBuilder result = new();
            if (item.Level == 0)
            {
                return string.Empty;
            }
            string? parent = item.ParentUniqueId;
            var aux = new Stack<string>();
            for (int i = 0; i < item.Level - 1; i++)
            {
                string syb = ConfigPlus.GetSymbol(SymbolType.TreeLinevertical);
                if (!string.IsNullOrEmpty(parent))
                {
                    int index = _items.FindIndex(x => x.UniqueId == parent);
                    if (_items[index].LastItem)
                    {
                        syb = new string(' ', ConfigPlus.GetSymbol(SymbolType.TreeLinevertical).Length);
                    }
                    parent = _items[index].ParentUniqueId;
                }
                aux.Push(syb);
            }
            while (aux.TryPop(out string? indentation))
            {
                result.Append(indentation);
            }
            if (item.FirstItem && !item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecross));
            }
            else if (item.FirstItem && item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecorner));
            }
            else if (!item.FirstItem && !item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecross));
            }
            else if (!item.FirstItem && item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecorner));
            }
            return result.ToString();
        }

        private void WriteNodeFullpath(BufferScreen screenBuffer)
        {
            if (_showInfoFullPath)
            {
                int pos = _localpaginator!.SelectedItem!.FullPath!.LastIndexOf($"{_nodeseparator}{_localpaginator!.SelectedItem!.Text!}", StringComparison.Ordinal);
                string info;
                if (pos < 0)
                {
                    info = _localpaginator!.SelectedItem!.FullPath;
                }
                else
                {
                    info = _localpaginator!.SelectedItem!.FullPath![..pos];
                }
                if (!string.IsNullOrEmpty(info))
                {
                    screenBuffer.Write(Messages.NodePath, _optStyles[NodeTreeStyles.Answer]);
                    screenBuffer.WriteLine(info, _optStyles[NodeTreeStyles.Answer]);
                }
            }
        }

        private string GetTooltipModeSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append($"{string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip)}, {Messages.MultiSelectCheck}");
            tooltip.Append(", ");
            tooltip.Append(string.Format(Messages.TooltipSelectAll, "Ctrl+Space"));
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipToggleExpandPress);
            return tooltip.ToString();
        }

        private ConsoleKeyInfo WaitKeypressDiscovery(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_loadingItemTask != null && _loadingItemTask.IsCompleted)
                {
                    _loadingItemTask.Dispose();
                    _loadingItemTask = null;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, true, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            if (ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                return ConsolePlus.ReadKey(true);
            }
            return new ConsoleKeyInfo();
        }

        private void ValidateConstraints()
        {
            if (_uniqueexpression == null)
            {
                throw new InvalidOperationException("UniqueId cannot be Null.");
            }
            if (_textSelector == null)
            {
                throw new InvalidOperationException("TextSelector cannot be Null.");
            }
            if (_predicateChildAllowed == null)
            {
                throw new InvalidOperationException("PredicateChildAllowed cannot be Null.");
            }
            if (_predicateSearchItems == null)
            {
                throw new InvalidOperationException("PredicateSearchItems cannot be Null.");
            }
        }

        private void LoadRoot()
        {
            T1? rootnode = _nodestree!.Node;
            string textroot = _textSelector!(rootnode);
            _items.Add(new ItemNodeControl<T1>(_nodestree.UniqueId)
            {
                IsExpanded = true,
                Status = NodeStatus.Loading,
                IsMarked = false,
                IsDisabled = _predicatevaliddisabled?.Invoke(rootnode) ?? false,
                Level = 0,
                ParentUniqueId = null,
                FullPath = textroot,
                Text = textroot,
                Value = rootnode,
                FirstItem = true,
                LastItem = true,
                AllowsChildren = true,
                ExtraText = _extraInfoNode?.Invoke(rootnode) ?? null,
            });
        }

        private void LoadMoreItems(string parentId, bool isLoadMore)
        {
            var index = _items.FindIndex(x => x.UniqueId == parentId);
            (bool Finished, T2 SearchItemsControl, IEnumerable<(bool Checked, T1 NewItem)> Newitems) result = (false, _searchItemsControl, []);
            Exception? err = null;
            try
            {
                result = _predicateSearchItems!.Invoke(_items[index].Value, _searchItemsControl);
                _searchItemsControl = result.SearchItemsControl;
            }
            catch (Exception ex)
            {
                err = ex;
            }
            var newitems = CreateEnqueueNewitems(_items[index], isLoadMore, result.Finished, result.Newitems, err);
            _resultTask.Enqueue(newitems);
        }

        private (string Key, bool IsLoadMore, bool IsLoad, bool IsFinished, Exception? Error, List<ItemNodeControl<T1>> Items) CreateEnqueueNewitems(ItemNodeControl<T1> parentnode, bool isLoadMore, bool isfinished, IEnumerable<(bool Checked, T1 NewItem)> result, Exception? err)
        {
            if (err != null)
            {
                return (parentnode.UniqueId, isLoadMore, true, isfinished, err, []);
            }
            int pos = -1;
            List<ItemNodeControl<T1>> newitems = [];
            var poslastitem = result.Count() - 1;
            var newlevel = parentnode.Level + 1;
            var parentid = parentnode.UniqueId;
            var countnodes = parentnode.CountChildren;
            if (isLoadMore)
            {
                if (parentnode.ParentUniqueId != null)
                {
                    newlevel = parentnode.Level;
                    var index = _items.FindIndex(x => x.UniqueId == parentnode.ParentUniqueId);
                    parentid = _items[index].UniqueId;
                    countnodes = _items[index].CountChildren;
                }
            }
            foreach (var item in result)
            {
                pos++;
                bool first = false;
                bool last = false;
                if (pos == 0 && countnodes == 0)
                {
                    first = true;
                }
                if (pos == poslastitem && isfinished)
                {
                    last = true;
                }
                var text = _textSelector!(item.NewItem);
                var fullpath = CreateFullPath(parentnode.UniqueId, text);
                var extratxt = _extraInfoNode?.Invoke(item.NewItem) ?? null;
                var allowsChildren = _predicateChildAllowed!(item.NewItem);
                var sta = allowsChildren ? NodeStatus.NotLoad : NodeStatus.Done;

                newitems.Add(new ItemNodeControl<T1>(_uniqueexpression!(item.NewItem))
                {
                    IsExpanded = false,
                    Status = sta,
                    FirstItem = first,
                    LastItem = last,
                    IsMarked = item.Checked,
                    IsDisabled = _predicatevaliddisabled?.Invoke(item.NewItem) ?? false,
                    Level = newlevel,
                    ParentUniqueId = parentid,
                    Value = item.NewItem,
                    Text = text,
                    FullPath = fullpath,
                    ExtraText = extratxt,
                    AllowsChildren = allowsChildren,
                });
            }
            if (!isfinished)
            {
                newitems.Add(new ItemNodeControl<T1>($"{newlevel}.{_loadMoreId}")
                {
                    IsExpanded = false,
                    Status = NodeStatus.Done,
                    FirstItem = false,
                    LastItem = true,
                    IsMarked = false,
                    IsDisabled = true,
                    Level = newlevel,
                    ParentUniqueId = parentid,
                    Value = new T1(),
                    Text = Messages.LoadMore,
                    AllowsChildren = false,
                    IsLoadMoreNode = true
                });
            }
            return (parentnode.UniqueId, isLoadMore, true, isfinished, null, newitems);
        }

        private string CreateFullPath(string? parentid, string textnode)
        {
            var parents = new Stack<string>();
            if (string.IsNullOrEmpty(parentid))
            {
                return textnode;
            }
            while (!string.IsNullOrEmpty(parentid))
            {
                int index = _items.FindIndex(x => x.UniqueId == parentid);
                parents.Push(_items[index].Text!);
                parentid = _items[index].ParentUniqueId;
            }
            var result = new StringBuilder();
            while (parents.TryPop(out string? item))
            {
                result.Append(item);
                result.Append(_nodeseparator);
                if (!parents.TryPeek(out _))
                {
                    result.Append(textnode);
                }
            }
            return result.ToString();
        }

        private void MarkAllNodes(int index)
        {
            bool mark = !_items[index].IsMarked;
            _items[index].IsMarked = mark;
            int level = _items[index].Level;
            string? parent;
            int chkindex;
            if (!mark)
            {
                chkindex = _checkeditems.FindIndex(x => x.UniqueId == _items[index].UniqueId);
                _checkeditems.RemoveAt(chkindex);
                parent = _items[index].ParentUniqueId;
                if (parent != null)
                {
                    chkindex = _items.FindIndex(x => x.UniqueId == parent);
                    _items[chkindex].IsMarked = false;
                    string uid = _items[chkindex].UniqueId;
                    chkindex = _checkeditems.FindIndex(x => x.UniqueId == uid);
                    if (chkindex >= 0)
                    {
                        _checkeditems.RemoveAt(chkindex);
                    }
                }
            }
            else
            {
                if (!_items[index].IsLoadMoreNode)
                {
                    (bool ok, string? message) = _predicatevalidselect?.Invoke(_items[index].Value) ?? (true, null);
                    if (!ok)
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            SetError(Messages.PredicateSelectInvalid);
                        }
                        else
                        {
                            SetError(message);
                        }
                    }
                    else
                    {
                        _checkeditems.Add(_items[index]);
                    }
                }
            }
            bool isvalid = true;
            index++;
            while (index < _items.Count)
            {
                if (_items[index].Level <= level)
                {
                    break;
                }
                chkindex = _checkeditems.FindIndex(x => x.UniqueId == _items[index].UniqueId);
                if (!mark)
                {
                    if (chkindex >= 0)
                    {
                        _checkeditems.RemoveAt(chkindex);
                    }
                }
                else
                {
                    if (chkindex < 0 && !_items[index].IsLoadMoreNode)
                    {
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_items[index].Value) ?? (true, null);
                        if (!ok)
                        {
                            isvalid = false;
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(Messages.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                        }
                        else
                        {
                            _checkeditems.Add(_items[index]);
                        }
                    }
                    else
                    {
                        isvalid = false;
                    }
                }
                if (isvalid)
                {
                    _items[index].IsMarked = mark;
                }
                index++;
            }
        }

        private bool HasSeletedItems()
        {
            int index = _items.FindIndex(x => x.UniqueId == _localpaginator!.SelectedItem.UniqueId);
            string parent = _items[index].UniqueId;
            index++;
            while (index < _items.Count - 1)
            {
                if (_items[index].IsMarked)
                {
                    return true;
                }
                if (_items[index].ParentUniqueId == parent && _items[index].LastItem)
                {
                    break;
                }
                index++;
            }
            return false;
        }

        private void InitViewNodes()
        {
            _items.Clear();
            LoadRoot();
            _checkeditems.AddRange([.. _items.Where(x => x.IsMarked)]);
            if (_checkeditems.Count == 0)
            {
                _resultbuffer!.Clear();
            }
            else
            {
                _resultbuffer!.LoadPrintable(_checkeditems.Select(x => _textSelector!.Invoke(x.Value) ?? string.Empty).Aggregate((x, y) => $"{x},{y}"));
            }
            int countselect = _checkeditems.Count;
            if (countselect < _minSelect)
            {
                SetError(string.Format(Messages.MultiSelectMinSelection, _minSelect));
            }
            else if (countselect > _maxSelect)
            {
                SetError(string.Format(Messages.MultiSelectMaxSelection, _maxSelect));
            }
            _localpaginator = new Paginator<ItemNodeControl<T1>>(
                FilterMode.Disabled,
                _items,
                _pageSize,
                Optional<ItemNodeControl<T1>>.Empty(),
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                null,
                null,
                (item) => !item.IsLoadMoreNode);

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (_localpaginator.SelectedItem!.IsDisabled)
            {
                SetError(Messages.SelectionDisabled);
            }
        }
    }
}
