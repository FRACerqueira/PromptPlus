﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PPlus.Controls
{
    internal class TreeViewMultiSelectControl<T> : BaseControl<T[]>, IControlTreeViewMultiSelect<T>
    {
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase, modefilter: true);
        private readonly List<ItemTreeViewFlatNode<T>> _flatnodes;
        private readonly TreeViewOptions<T> _options;
        private Paginator<ItemTreeViewFlatNode<T>> _localpaginator;
        private TreeView<T> _browserTreeView = null;
        private bool _rootExpand = true;
        private List<(string UniqueId, string Fullpath, T value)> _selectedItems;

        public TreeViewMultiSelectControl(IConsoleControl console, TreeViewOptions<T> options) : base(console, options)
        {
            _options = options;
            _flatnodes = new();
            _selectedItems = new();
        }

        #region IControlTreeViewMultiSelect

        public IControlTreeViewMultiSelect<T> AddFixedSelect(params T[] values)
        {
            foreach (var item in values)
            {
                _options.FixedSelected.Add(item);
            }
            return this;
        }
        public IControlTreeViewMultiSelect<T> SelectAll(Func<T, bool> selectAllExpression = null)
        {
            _options.SelectAll = true;
            _options.SelectAllExpression = selectAllExpression;
            return this;
        }
        public IControlTreeViewMultiSelect<T> Range(int minvalue, int? maxvalue = null)
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


        public IControlTreeViewMultiSelect<T> Interaction(IEnumerable<T> values, Action<IControlTreeViewMultiSelect<T>, T> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }
        public IControlTreeViewMultiSelect<T> FilterType(FilterMode value)
        {
            _options.FilterType = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlTreeViewMultiSelect<T> Styles(TreeViewStyles content, Style value)
        {
            _options.StyleControl(content, value);
            return this;
        }

        public IControlTreeViewMultiSelect<T> ShowLines(bool value = true)
        {
            _options.ShowLines = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> ShowExpand(bool value = true)
        {
            _options.ShowLines = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> RootNode(T value, Func<T, string> textnode, Func<T, bool>? validselect = null, Func<T, bool>? setdisabled = null, char? separatePath = null, Func<T, string> uniquenode = null)
        {
            _options.TextNode = textnode ?? throw new PromptPlusException("Not have Text-Node to run");
            _options.ExpressionSelected = validselect;
            _options.ExpressionDisabled = setdisabled;
            var diabled = _options.ExpressionDisabled?.Invoke(value) ?? false;
            _options.Nodes = new TreeOption<T>
            {
                Disabled = diabled,
                Selected = !diabled && (_options.ExpressionSelected?.Invoke(value) ?? false),
                Node = value
            };
            if (separatePath.HasValue)
            {
                _options.SeparatePath = separatePath.Value;
            }
            if (uniquenode != null)
            {
                _options.UniqueNode = uniquenode;
            }
            return this;
        }

        public IControlTreeViewMultiSelect<T> ExpandAll(bool value = true)
        {
            _options.ExpandAll = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> AddNode(T value)
        {
            if (_options.Nodes == null)
            {
                throw new PromptPlusException("Not have Root node!. Execute RootNode first!");
            }
            if (_options.Nodes.Childrens == null)
            {
                _options.Nodes.Childrens = new();
            }
            var diabled = _options.ExpressionDisabled?.Invoke(value) ?? false;
            var selected = !diabled && (_options.ExpressionSelected?.Invoke(value) ?? false);
            _options.Nodes.Childrens.Add(new TreeOption<T> { Node = value, Disabled = diabled, Selected = selected});
            return this;
        }

        public IControlTreeViewMultiSelect<T> AddNode(T parent, T value)
        {
            if (_options.Nodes == null)
            {
                throw new PromptPlusException("Not have Root node!. Execute RootNode first!");
            }
            var nodeparent = _options.FindNode(null, parent) ?? throw new PromptPlusException("Not found parent node!. Add parent node first!");
            nodeparent.Childrens ??= new();
            var diabled = _options.ExpressionDisabled?.Invoke(value) ?? false;
            var selected = !diabled && (_options.ExpressionSelected?.Invoke(value) ?? false);
            nodeparent.Childrens.Add(new TreeOption<T> { Node = value, Disabled = diabled, Selected = selected });
            return this;
        }

        public IControlTreeViewMultiSelect<T> Default(T value)
        {
            _options.DefautNode = Optional<T>.Set(value);
            return this;
        }

        public IControlTreeViewMultiSelect<T> ShowCurrentNode(bool value = true)
        {
            _options.ShowCurrentNode = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> HotKeyFullPath(HotKey value)
        {
            _options.HotKeyFullPathNodePress = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> HotKeyToggleExpand(HotKey value)
        {
            _options.HotKeyToggleExpandPress = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> HotKeyToggleExpandAll(HotKey value)
        {
            _options.HotKeyToggleExpandAllPress = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> AfterExpanded(Action<T> value)
        {
            _options.AfterExpanded = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> AfterCollapsed(Action<T> value)
        {
            _options.AfterCollapsed = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> BeforeExpanded(Action<T> value)
        {
            _options.BeforeExpanded = value;
            return this;
        }

        public IControlTreeViewMultiSelect<T> BeforeCollapsed(Action<T> value)
        {
            _options.BeforeCollapsed = value;
            return this;
        }

        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.Nodes == null)
            {
                throw new PromptPlusException("Not have Root node to run");
            }
            if (_options.Nodes.Node == null)
            {
                throw new PromptPlusException("Not have Root node to run");
            }
            if (_options.Nodes.Childrens == null)
            {
                throw new PromptPlusException("Not have Childrens nodes to run");
            }
            if (!_options.Nodes.Childrens.Any())
            {
                throw new PromptPlusException("Not have Childrens nodes to run");
            }
            if (_options.FixedSelected.Count > _options.Maximum)
            {
                throw new PromptPlusException("FixedSelected Count > Maximum Selected");
            }

            _browserTreeView = new TreeView<T>(_options.ExpressionSelected)
            {
                TextTree = _options.TextNode
            };
            var root = _browserTreeView.AddRootNode(_options.Nodes.Node,_options.Nodes.Disabled, _options.Nodes.Selected);

            CreateTreeview(root);
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

            _options.Nodes = null;

            var defnode = _browserTreeView.Root;
            if (_options.DefautNode.HasValue)
            {
                var aux = defnode.FindTreeByValue(_options.DefautNode.Value, _options.UniqueNode);
                if (aux != null)
                {
                    defnode = aux;
                }
            }

            InitSelectedNodes();

            LoadFlatNodes(defnode, true);

            FinishResult = string.Empty;
            if (_localpaginator.SelectedIndex >= 0)
            {
                FinishResult = _localpaginator.SelectedItem.MessagesNodes.TextItem;
            }
            return FinishResult;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var hasprompt = string.IsNullOrEmpty(_options.OptPrompt) && !_options.OptMinimalRender;
            screenBuffer.WritePrompt(_options, "");
            if (_filterBuffer.Length > 0)
            {
                hasprompt = true;
                if (_localpaginator.TryGetSelected(out var showItem))
                {
                    var item = showItem.MessagesNodes.TextItem;
                    screenBuffer.WriteFilterTreeViewMultiSelect(_options, item, _filterBuffer);
                    screenBuffer.SaveCursor();
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
                _options.OptShowCursor = true;
            }
            else
            {
                if (!_options.OptMinimalRender)
                {
                    hasprompt = true;
                    screenBuffer.SaveCursor();
                    string answer = FinishResult;
                    if (_selectedItems.Any())
                    {
                        answer = string.Join(", ", _selectedItems.Select(x => _options.TextNode(x.value)));
                    }
                    screenBuffer.WriteAnswer(_options, answer);
                }
                else
                {
                    _options.OptShowCursor = false;
                }

            }
            if (!string.IsNullOrEmpty(_options.OptDescription) && !_options.OptMinimalRender)
            {
                hasprompt = true;
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(_options.OptDescription, _options.StyleContent(StyleControls.Description));
            }
            var subset = _localpaginator.GetPageData();
            var showitem = _localpaginator.TryGetSelected(out var selectedItem);
            if (_options.ShowCurrentNode && !_options.OptMinimalRender)
            {
                if (showitem)
                {
                    screenBuffer.NewLine();
                    if (_options.ShowCurrentFulPathNode)
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentSelected}: {selectedItem.MessagesNodes.TextFullpath}", _options.StyleContent(StyleControls.TaggedInfo));
                    }
                    else
                    {
                        screenBuffer.AddBuffer($"{Messages.CurrentFolder}: {selectedItem.MessagesNodes.TextItem}", _options.StyleContent(StyleControls.TaggedInfo));
                    }
                }
            }
            foreach (var item in subset)
            {
                var fnode = _browserTreeView.Root.FindTreeByValue(selectedItem.Value, _options.UniqueNode);
                if (EqualityComparer<ItemTreeViewFlatNode<T>>.Default.Equals(item, selectedItem))
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineDisabledSelectorTreeViewMultiSelect(_options, item, hasprompt);
                    }
                    else
                    {
                        screenBuffer.WriteLineSelectorTreeViewMultiSelect(_options, item, hasprompt);
                    }
                }
                else
                {
                    if (item.IsDisabled)
                    {
                        screenBuffer.WriteLineDisabledNotSelectorTreeViewMultiSelect(_options, item, hasprompt);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelectorTreeViewMultiSelect(_options, item, fnode.Childrens != null, hasprompt);
                    }
                }
                hasprompt = true;
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
            if (showitem)
            {
                var fnode = _browserTreeView.Root.FindTreeByValue(selectedItem.Value, _options.UniqueNode);
                screenBuffer.WriteLineTooltipsTreeViewMultiSelect(_options, fnode.Childrens != null);
            }
        }

        public override ResultPrompt<T[]> TryResult(CancellationToken cancellationToken)
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
                else if (keyInfo.Value.IsPressSpaceKey())
                {
                    _filterBuffer.Clear();
                    ItemTreeViewFlatNode<T> currentItem = null;
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
                else if (_options.FilterType != FilterMode.Disabled &&  _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    break;
                }
                else if (_options.HotKeyFullPathNodePress.Equals(keyInfo.Value))
                {
                    _options.ShowCurrentFulPathNode = !_options.ShowCurrentFulPathNode;
                    break;
                }
                else if (_options.HotKeyToggleExpandPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                    if (fnode.Childrens == null)
                    {
                        tryagain = true;
                    }
                    else
                    {
                        if (fnode.IsExpanded && !_localpaginator.SelectedItem.IsRoot)
                        {
                            var oldcur = ConsolePlus.CursorVisible;
                            _options.BeforeCollapsed?.Invoke(fnode.Value);
                            _browserTreeView.CollapseAll(fnode);
                            _options.AfterCollapsed?.Invoke(fnode.Value);
                            LoadFlatNodes(fnode, true);
                            ConsolePlus.CursorVisible = oldcur;
                        }
                        else
                        {
                            var oldcur = ConsolePlus.CursorVisible;
                            _options.BeforeExpanded?.Invoke(fnode.Value);
                            _browserTreeView.Expand(fnode);
                            _options.AfterExpanded?.Invoke(fnode.Value);
                            LoadFlatNodes(fnode, true);
                            ConsolePlus.CursorVisible = oldcur;
                        }
                        break;
                    }
                }
                else if (_options.HotKeyToggleExpandAllPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    var fnode = _browserTreeView.FindNode(_localpaginator.SelectedItem.UniqueId);
                    if (fnode.Childrens == null)
                    {
                        tryagain = true;
                    }
                    else
                    {
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
                            _options.BeforeCollapsed?.Invoke(fnode.Value);
                            _browserTreeView.CollapseAll(fnode);
                            _options.AfterCollapsed?.Invoke(fnode.Value);
                            LoadFlatNodes(fnode, true);
                            ConsolePlus.CursorVisible = oldcur;
                        }
                        else
                        {
                            var oldcur = ConsolePlus.CursorVisible;
                            _options.BeforeExpanded?.Invoke(fnode.Value);
                            _browserTreeView.ExpandAll(fnode);
                            _options.AfterExpanded?.Invoke(fnode.Value);
                            LoadFlatNodes(fnode, true);
                            ConsolePlus.CursorVisible = oldcur;
                        }
                        break;
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
                FinishResult = string.Join(", ", _selectedItems.Select(x => _options.TextNode(x.value)));
                return new ResultPrompt<T[]>(_selectedItems.Select(x => x.value).ToArray(), abort, !endinput, notrender);
            }
            return new ResultPrompt<T[]>(Array.Empty<T>(), abort, !endinput,notrender);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T[] result, bool aborted)
        {
            if (_options.OptMinimalRender)
            {
                return;
            }
            string answer;
            if (aborted)
            {
                answer = Messages.CanceledKey;
            }
            else
            {
                var aux = new List<string>();
                foreach (var item in result)
                {
                    aux.Add(DefaultFullPath(item));
                }
                answer = string.Join(", ", aux);
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        private bool IsFixedSelect(T item, Func<T, string> uniquenode)
        {
            if (_options.ExpressionSelected?.Invoke(item) ?? true)
            {
                if (uniquenode == null)
                {
                    return _options.FixedSelected.Any(x => x.Equals(item));
                }
                return _options.FixedSelected.Any(x => uniquenode(x) == uniquenode(item));
            }
            return false;
        }

        private void CreateTreeview(TreeNode<T> node)
        {
            var auxnode = _options.FindNode(null, node.Value);
            if (auxnode.Childrens == null)
            {
                var lastnextnode = node.NextNode;
                var newnode = _browserTreeView.AddNode(node, auxnode.Node, auxnode.Disabled, auxnode.Selected);
                if (!newnode.IsSelected)
                {
                    if (IsFixedSelect(newnode.Value, _options.UniqueNode))
                    {
                        newnode.IsSelected = true;
                        newnode.IsDisabled = true;
                    }
                }
                newnode.IsMarked = newnode.IsSelected;
                node.Childrens.Last().NextNode = lastnextnode;
            }
            else
            {
                var childnodes = auxnode.Childrens;
                var lastnextnode = node.NextNode;
                foreach (var item in childnodes)
                {

                    var newnode = _browserTreeView.AddNode(node, item.Node, item.Disabled, item.Selected);
                    newnode.IsMarked = newnode.IsSelected;
                    if (!newnode.IsSelected)
                    {
                        if (IsFixedSelect(newnode.Value, _options.UniqueNode))
                        {
                            newnode.IsSelected = true;
                            newnode.IsDisabled = true;
                        }
                    }
                    if (item.Childrens != null)
                    {
                        newnode.Childrens = new();
                    }
                }
                node.Childrens.Last().NextNode = lastnextnode;
                foreach (var item in node.Childrens.Where(x => x.Childrens != null))
                {
                    CreateTreeview(item);
                }
            }
        }

        private void InitSelectedNodes()
        {
            var nodeselect = _browserTreeView.Root;
            while (nodeselect.NextNode != null)
            {
                if (nodeselect.IsSelected)
                {
                    AddSelectAll(nodeselect);
                }
                nodeselect = nodeselect.NextNode;
            }
            if (nodeselect != null)
            {
                if (nodeselect.IsSelected)
                {
                    AddSelectAll(nodeselect);
                }
            }
        }

        private void LoadFlatNodes(TreeNode<T> defaultnodeselected, bool updatePaginator)
        {
            _flatnodes.Clear();
            var nodeselect = _browserTreeView.Root;
            _flatnodes.Add(new ItemTreeViewFlatNode<T>
            {
                UniqueId = nodeselect.UniqueId,
                Value = nodeselect.Value,
                MessagesNodes = ShowItem(nodeselect),
                IsRoot = true,
                IsDisabled = nodeselect.IsDisabled
            });
            nodeselect = nodeselect.NextNode;

            while (nodeselect.NextNode != null)
            {
                if ((!nodeselect.IsHasChild && nodeselect.Parent.IsExpanded) || nodeselect.Parent.IsExpanded || nodeselect.IsRoot)
                {
                    _flatnodes.Add(new ItemTreeViewFlatNode<T>
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
                _flatnodes.Add(new ItemTreeViewFlatNode<T>
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
                _localpaginator = new Paginator<ItemTreeViewFlatNode<T>>(
                    _options.FilterType,
                    _flatnodes,
                    _options.PageSize,
                    Optional<ItemTreeViewFlatNode<T>>.Set(new ItemTreeViewFlatNode<T> { UniqueId = defaultnodeselected.UniqueId, IsDisabled = defaultnodeselected.IsDisabled, IsRoot = defaultnodeselected.IsRoot, Value = defaultnodeselected.Value, MessagesNodes = ShowItem(defaultnodeselected) }),
                    (item1,item2) => item1.UniqueId == item2.UniqueId,
                    (item) => item.MessagesNodes.TextItem);
            }
            _browserTreeView.SetCurrentNode(nodeselect);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0056:Use index operator", Justification = "<Pending>")]
        private ItemShowTreeView ShowItem(TreeNode<T> item)
        {
            ItemShowTreeView result;
            if (item.Level == 0)
            {
                result = new ItemShowTreeView
                {
                    TextItem = item.Text,
                    TextFullpath = item.Text,
                };
            }
            else
            {
                result = new ItemShowTreeView
                {
                    TextItem = item.Text,
                    TextFullpath = DefaultFullPath(item.Value),
                };
            }
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
                auxline[auxline.Length - 1] = _options.Symbol(SymbolType.TreeLinecorner);
            }
            result.TextSelected = item.IsSelected ? $" {_options.Symbol(SymbolType.Selected)} " : $" {_options.Symbol(SymbolType.NotSelect)} ";
            foreach (var itemaux in auxline)
            {
                result.TextLines += itemaux;
            }
            if (item.Childrens != null)
            {
                if (_options.ShowExpand)
                {
                    result.TextExpand = item.IsExpanded ? _options.Symbol(SymbolType.Expanded) : _options.Symbol(SymbolType.Collapsed);
                }
            }
            return result;
        }

        private string DefaultFullPath(T value)
        {
            var fnode = _browserTreeView.Root.FindTreeByValue(value, _options.UniqueNode);
            if (fnode == null)
            {
                return string.Empty;
            }
            var paths = new List<string>();
            while (fnode.Parent != null)
            {
                paths.Add(fnode.Text);
                fnode = fnode.Parent;
            }
            paths.Add(fnode.Text);
            paths.Reverse();
            return string.Join(_options.SeparatePath, paths);
        }

        private void RemoveSelectAll(TreeNode<T> node)
        {
            var index = _selectedItems.FindIndex(x => x.UniqueId == node.UniqueId);
            if (index >= 0)
            {
                if (!IsFixedSelect(node.Value, _options.UniqueNode))
                {
                    _selectedItems.RemoveAt(index);
                }
            }
            if (node.Childrens != null)
            {
                foreach (var child in node.Childrens)
                {
                    RemoveSelectAll(child);
                }
            }
        }

        private void AddSelectAll(TreeNode<T> node)
        {
            var index = _selectedItems.FindIndex(x => x.UniqueId == node.UniqueId);
            if (index < 0)
            {
                if (node.IsSelected)
                {
                    _selectedItems.Add(new(node.UniqueId, DefaultFullPath(node.Value), node.Value));
                }
            }
            if (node.Childrens != null)
            {
                foreach (var child in node.Childrens)
                {
                    AddSelectAll(child);
                }
            }
        }
    }
}
