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

namespace PromptPlusLibrary.Controls.NodeTreeMultiSelect
{
    internal sealed class NodeTreeMultiSelectControl<T> : BaseControlPrompt<T[]>, INodeTreeMultiSelectControl<T>
    {
        private readonly Dictionary<NodeTreeStyles, Style> _optStyles = BaseControlOptions.LoadStyle<NodeTreeStyles>();
        private readonly List<ItemNodeControl<T>> _items = [];
        private readonly Func<ItemNodeControl<T>, bool> IsRoot;
        private readonly ConcurrentQueue<(string, bool, bool, List<ItemNodeControl<T>>)> _resultTask = [];
        private readonly Func<T, T, bool> _equalItems = (x, y) => x?.Equals(y) ?? false;
        private readonly List<ItemNodeControl<T>> _checkeditems = [];
        private Func<T, string>? _changeDescription;
        private Func<T, string>? _textSelector;
        private NodeTree<T>? _nodestree;
        private bool _hideSize;
        private int _indexTooptip;
        private string _tooltipModeSelect = string.Empty;
        private bool _showInfoFullPath;
        private byte _pageSize;
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, bool>? _predicatevaliddisabled;
        private Paginator<ItemNodeControl<T>>? _localpaginator;
        private string _nodeseparator = "|";
        private string[] _toggerTooptips;
        private byte _maxWidth;
        private bool _hideCountSelected;
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private EmacsBuffer? _resultbuffer;


        public NodeTreeMultiSelectControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            IsRoot = (item) => item.UniqueId == (_items.Count == 0 ? "" : _items[0].UniqueId);
            _toggerTooptips = [];
            _maxWidth = ConfigPlus.MaxWidth;
            _pageSize = ConfigPlus.PageSize;
        }

        #region INodeTreeMultiSelectControl

        public INodeTreeMultiSelectControl<T> HideCountSelected(bool value = true)
        {
            _hideCountSelected = value;
            return this;
        }

        public INodeTreeMultiSelectControl<T> MaxWidth(byte maxWidth)
        {
            if (maxWidth < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 10.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        public INodeTreeMultiSelectControl<T> Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue > (maxvalue ?? int.MaxValue))
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minSelect = minvalue;
            _maxSelect = maxvalue ?? int.MaxValue;
            return this;
        }

        public INodeTreeMultiSelectControl<T> ChangeDescription(Func<T, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public INodeTreeMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, INodeTreeMultiSelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T? item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        public INodeTreeMultiSelectControl<T> HideSize(bool value = true)
        {
            _hideSize = value;
            return this;
        }

        public INodeTreeMultiSelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public INodeTreeMultiSelectControl<T> Styles(NodeTreeStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public INodeTreeMultiSelectControl<T> TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value), "TextSelector is null");
            return this;
        }

        public INodeTreeMultiSelectControl<T> AddRootNode(T value, bool valuechecked = false, string nodeseparator = "|")
        {
            if (_nodestree != null)
            {
                throw new InvalidOperationException("There is already a root node!");
            }
            ArgumentException.ThrowIfNullOrEmpty(nodeseparator);
            _nodestree = new NodeTree<T>
            {
                Node = value,
                Checked = valuechecked,
            };
            _nodeseparator = nodeseparator;
            return this;
        }

        public INodeTreeMultiSelectControl<T> AddChildNode(T parent, T value, bool valuechecked = false)
        {
            if (_nodestree == null)
            {
                throw new InvalidOperationException("Not have Root node. Execute AddRootNode first!");
            }
            NodeTree<T> nodeparent = FindNode(_nodestree, parent) ?? throw new ArgumentException("Not found parent node!. Add parent node first!");
            nodeparent.Childrens.Add(new NodeTree<T>
            {
                Node = value,
                ParentiId = nodeparent.UniqueId,
                Checked = valuechecked
            });
            return this;
        }

        public INodeTreeMultiSelectControl<T> PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public INodeTreeMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public INodeTreeMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect)
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

        public INodeTreeMultiSelectControl<T> PredicateDisabled(Func<T, bool> validdisabled)
        {
            ArgumentNullException.ThrowIfNull(validdisabled);
            _predicatevaliddisabled = validdisabled;
            return this;
        }

        #endregion  

        public override void InitControl(CancellationToken cancellationToken)
        {
            _resultbuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);

            _textSelector ??= (x) => x?.ToString() ?? string.Empty;

            InitViewNodes();

            _tooltipModeSelect = GetTooltipModeSelect();
            LoadTooltipToggle();
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteNodeFullpath(screenBuffer);

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
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
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
                        ResultCtrl = new ResultPrompt<T[]>([.. _checkeditems.Select(x => x.Value)], false);
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
                        //has result backgroud result
                        if (_resultTask.TryDequeue(out (string key, bool isload, bool isrecursive, List<ItemNodeControl<T>> values) resultitems))
                        {
                            if (resultitems.isload)
                            {
                                LoadTaskResult(resultitems.key, resultitems.values, resultitems.isrecursive, cancellationToken);
                            }
                            else
                            {
                                UnloadTaskResult(resultitems.key, cancellationToken);
                            }
                            break;
                        }
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
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.IsDisabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
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
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.IsDisabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.IsDisabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.IsDisabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
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
                    else if (_localpaginator!.SelectedItem != null && !IsRoot(_localpaginator.SelectedItem) && _localpaginator.SelectedItem.CountChildren > 0 && keyinfo.KeyChar == '+' && keyinfo.Modifiers == ConsoleModifiers.Alt)
                    {
                        if (_localpaginator!.SelectedItem!.IsExpanded)
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        _localpaginator.SelectedItem.IsExpanded = true;
                        _localpaginator.SelectedItem.Status = NodeStatus.Loading;
                        (string, bool, bool, List<ItemNodeControl<T>>) newitems = CreateLoadNode(_localpaginator.SelectedItem, true);
                        _resultTask.Enqueue(newitems);
                        break;
                    }
                    else if (_localpaginator!.SelectedItem != null && !IsRoot(_localpaginator.SelectedItem) && _localpaginator.SelectedItem.CountChildren > 0 && "+-".Contains(keyinfo.KeyChar) && keyinfo.Modifiers == ConsoleModifiers.None)
                    {
                        if (keyinfo.KeyChar == '+')
                        {
                            if (_localpaginator!.SelectedItem!.IsExpanded)
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            _localpaginator.SelectedItem.IsExpanded = true;
                            _localpaginator.SelectedItem.Status = NodeStatus.Loading;
                            (string, bool, bool, List<ItemNodeControl<T>>) newitems = CreateLoadNode(_localpaginator.SelectedItem, false);
                            _resultTask.Enqueue(newitems);
                            break;
                        }
                        if (keyinfo.KeyChar == '-')
                        {
                            if (!_localpaginator!.SelectedItem!.IsExpanded)
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            if (HasSeletedItems())
                            {
                                SetError(Messages.NotCollapse);
                                break;
                            }
                            _localpaginator.SelectedItem.IsExpanded = false;
                            _localpaginator.SelectedItem.Status = NodeStatus.Unloading;
                            _resultTask.Enqueue((_localpaginator.SelectedItem.UniqueId, false, false, []));
                            break;
                        }
                        continue;
                    }
                    else if (keyinfo.IsPressCtrlSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.IsDisabled)
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
                    else if (keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.IsDisabled)
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

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            else
            {
                answer = _resultbuffer!.ToString();
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[NodeTreeStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[NodeTreeStyles.Answer].Overflow(Overflow.Ellipsis));
            _showInfoFullPath = true;
            WriteNodeFullpath(screenBuffer);
            return true;
        }

        public override void FinalizeControl()
        {
            //none
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
                    if (chkindex < 0)
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
            _localpaginator = new Paginator<ItemNodeControl<T>>(
                FilterMode.Disabled,
                _items,
                _pageSize,
                Optional<ItemNodeControl<T>>.Empty(),
                (item1, item2) => item1.UniqueId == item2.UniqueId);

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (_localpaginator.SelectedItem!.IsDisabled)
            {
                SetError(Messages.SelectionDisabled);
            }
        }

        private void LoadRoot()
        {
            List<string> entries = [];

            T? rootnode = _nodestree!.Node;
            string textroot = _textSelector!(rootnode);
            (bool okmark, _) = _predicatevalidselect?.Invoke(rootnode) ?? (true, null);
            _items.Add(new ItemNodeControl<T>(_nodestree.UniqueId)
            {
                IsExpanded = true,
                Status = NodeStatus.Done,
                IsMarked = okmark && _nodestree.Checked,
                IsDisabled = _predicatevaliddisabled?.Invoke(rootnode) ?? false,
                Level = 0,
                ParentUniqueId = null,
                FullPath = textroot,
                Text = textroot,
                CountChildren = _nodestree.Childrens.Count,
                Value = rootnode
            });
            int pos = -1;
            foreach (NodeTree<T> item in _nodestree.Childrens)
            {
                pos++;
                bool first = false;
                bool last = false;
                if (pos == 0)
                {
                    first = true;
                }
                if (pos == _nodestree.Childrens.Count - 1)
                {
                    last = true;
                }
                string text = _textSelector!(item.Node);
                (bool okmarkitem, _) = _predicatevalidselect?.Invoke(item.Node) ?? (true, null);
                _items.Add(new ItemNodeControl<T>(item.UniqueId)
                {
                    IsExpanded = false,
                    Status = item.Childrens.Count > 0 ? NodeStatus.NotLoad : NodeStatus.Done,
                    FirstItem = first,
                    LastItem = last,
                    IsMarked = okmarkitem && item.Checked,
                    IsDisabled = _predicatevaliddisabled?.Invoke(item.Node) ?? false,
                    Level = 1,
                    ParentUniqueId = item.ParentiId,
                    FullPath = CreateFullPath(item.ParentiId, text),
                    Text = text,
                    CountChildren = item.Childrens.Count,
                    Value = item.Node
                });
            }
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

        private NodeTree<T>? FindNode(NodeTree<T> currentnode, T value)
        {
            if (_equalItems(currentnode.Node, value))
            {
                return currentnode;
            }
            if (currentnode.Childrens.Count > 0)
            {
                foreach (NodeTree<T> child in currentnode.Childrens)
                {
                    NodeTree<T>? aux = FindNode(child, value);
                    if (aux != null)
                    {
                        return aux;
                    }
                }
            }
            return null;
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
            screenBuffer.WriteLine(str, styleAnswer);
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

        private void WriteListSelect(BufferScreen screenBuffer)
        {
            ArraySegment<ItemNodeControl<T>> subset = _localpaginator!.GetPageData();
            foreach (ItemNodeControl<T> item in subset)
            {
                bool checkroot = IsRoot(item);
                Style stl = _optStyles[NodeTreeStyles.UnSelected];
                Style stlexp = _optStyles[NodeTreeStyles.ExpandSymbol];
                Style stlsiz = _optStyles[NodeTreeStyles.ChildsCount];
                if (item.IsDisabled && !checkroot)
                {
                    stl = _optStyles[NodeTreeStyles.Disabled];
                }
                if (_localpaginator.TryGetSelected(out ItemNodeControl<T>? selectedItem) && EqualityComparer<ItemNodeControl<T>>.Default.Equals(item, selectedItem))
                {
                    stl = _optStyles[NodeTreeStyles.Selected];
                    stlexp = stl;
                    stlsiz = stl;
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", stl);
                    if (item.IsMarked)
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), stl);
                    }
                    else
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), stl);
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
                    if (item.IsMarked)
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Selected), stl);
                    }
                    else
                    {
                        screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.NotSelect), stl);
                    }
                }
                screenBuffer.Write(CreateIndentation(item), _optStyles[NodeTreeStyles.Lines]);
                if (item.CountChildren > 0)
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
                screenBuffer.Write(item.Text!, stl);
                if (!_hideSize && !checkroot && item.CountChildren > 0)
                {
                    screenBuffer.Write($"({item.CountChildren})", stlsiz);
                }
                screenBuffer.WriteLine("", Style.Default());
            }
            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[NodeTreeStyles.Pagination]);
                if (!_hideCountSelected)
                {
                    screenBuffer.Write(string.Format(Messages.TooltipCountCheck, _checkeditems.Count), _optStyles[NodeTreeStyles.TaggedInfo]);
                }
                screenBuffer.WriteLine(template, _optStyles[NodeTreeStyles.Pagination]);
            }
            else
            {
                if (!_hideCountSelected)
                {
                    screenBuffer.WriteLine(string.Format(Messages.TooltipCountCheck, _checkeditems.Count), _optStyles[NodeTreeStyles.TaggedInfo]);
                }
            }
        }

        private string CreateIndentation(ItemNodeControl<T> item)
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

        private void LoadTaskResult(string key, List<ItemNodeControl<T>> Items, bool isrecursive, CancellationToken cancellationToken)
        {
            int index = _items.FindIndex(x => x.UniqueId == key);
            if (index == -1)
            {
                return;
            }
            if (_items[index].CountChildren == 0)
            {
                throw new InvalidOperationException("Internal error");
            }
            if (_items[index].Status != NodeStatus.Loading)
            {
                return;
            }
            int posindex = index;
            foreach (ItemNodeControl<T> item in Items)
            {
                if (cancellationToken.IsCancellationRequested || _items[index].Status != NodeStatus.Loading)
                {
                    break;
                }
                posindex++;
                if (item.Status == NodeStatus.NotLoad && isrecursive)
                {
                    item.IsExpanded = true;
                    item.Status = NodeStatus.Loading;
                    _items.Insert(posindex, item);
                    (string, bool, bool, List<ItemNodeControl<T>>) newitems = CreateLoadNode(item, true);
                    _resultTask.Enqueue(newitems);
                }
                else
                {
                    _items.Insert(posindex, item);
                }
                if (item.IsMarked)
                {
                    _checkeditems.Add(item);
                }
            }
            if (cancellationToken.IsCancellationRequested || _items[index].Status != NodeStatus.Loading)
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
            _indexTooptip = 0;
            Optional<ItemNodeControl<T>> defaultvalue;
            if (isrecursive)
            {
                defaultvalue = Optional<ItemNodeControl<T>>.Empty();
            }
            else
            {
                defaultvalue = Optional<ItemNodeControl<T>>.Set(_items[index]);
            }
            _localpaginator!.UpdatColletion(_items, defaultvalue);
            _items[index].Status = NodeStatus.Done;
        }

        private void UnloadTaskResult(string key, CancellationToken cancellationToken)
        {
            int index = _items.FindIndex(x => x.UniqueId == key);
            if (index == -1)
            {
                return;
            }
            if (_items[index].CountChildren == 0)
            {
                throw new InvalidOperationException("Internal error");
            }
            int posindex = index + 1;
            while (posindex < _items.Count)
            {
                if (_items[posindex].ParentUniqueId != key || cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (_items[posindex].CountChildren > 0 && _items[posindex].Status == NodeStatus.Done)
                {
                    UnloadTaskResult(_items[posindex].UniqueId, cancellationToken);
                }
                _items.RemoveAt(posindex);
                continue;
            }
            _indexTooptip = 0;
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            _localpaginator!.UpdatColletion(_items, Optional<ItemNodeControl<T>>.Set(_items[index]));
            _items[index].Status = NodeStatus.NotLoad;
        }

        private (string, bool, bool, List<ItemNodeControl<T>>) CreateLoadNode(ItemNodeControl<T> currentnode, bool isrecursive)
        {
            int pos = 0;
            NodeTree<T>? nodetree = FindNode(_nodestree!, currentnode.Value);
            List<ItemNodeControl<T>> newitems = [];
            int level = currentnode.Level + 1;
            foreach (NodeTree<T> item in nodetree!.Childrens)
            {
                bool first = false;
                bool last = false;
                if (pos == 0)
                {
                    first = true;
                }
                if (pos == nodetree.Childrens.Count - 1)
                {
                    last = true;
                }
                string text = _textSelector!(item.Node);
                (bool okmarkitem, _) = _predicatevalidselect?.Invoke(item.Node) ?? (true, null);
                var newitem = new ItemNodeControl<T>(item.UniqueId)
                {
                    IsExpanded = false,
                    Status = item.Childrens.Count > 0 ? NodeStatus.NotLoad : NodeStatus.Done,
                    FirstItem = first,
                    LastItem = last,
                    IsMarked = okmarkitem && item.Checked,
                    IsDisabled = _predicatevaliddisabled?.Invoke(item.Node) ?? false,
                    Level = level,
                    ParentUniqueId = nodetree.UniqueId,
                    FullPath = CreateFullPath(nodetree.UniqueId, text),
                    Text = text,
                    CountChildren = item.Childrens.Count,
                    Value = item.Node
                };
                newitems.Add(newitem);
                pos++;
            }
            return (currentnode.UniqueId, true, isrecursive, newitems);
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

        private ConsoleKeyInfo WaitKeypressDiscovery(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (!_resultTask.IsEmpty)
                {
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
    }
}
