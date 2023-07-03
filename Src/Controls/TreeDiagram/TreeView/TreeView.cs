// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls
{
    internal class TreeView<T>
    {
        private TreeNode<T> _treeRoot;
        private Dictionary<string, TreeNode<T>> _treedic;
        private Func<T, string> _textfunc = (T) => T.ToString();
        private readonly Func<T, bool>? _validselect;
        public TreeView(Func<T, bool> validselect)
        {
            _treeRoot = null;
            _treedic = null;
            _validselect = validselect;
        }

        public Func<T, string> TextTree
        {
            get
            {
                return _textfunc;
            }
            set
            {
                value ??= (T) => T.ToString();
                _textfunc = value;
            }

        }

        public TreeNode<T>? CurrentNode { get; private set; } = null;

        public TreeNode<T> Root => _treeRoot;

        public TreeNode<T>[] Nodes => _treeRoot==null?Array.Empty<TreeNode<T>>(): _treeRoot.Childrens.ToArray();

        public virtual bool SetCurrentNode(TreeNode<T> node)
        {
            if (_treeRoot == null)
            {
                throw new PromptPlusException("Root not exists");
            }
            if (!ExistNode(node))
            {
                return false;
            }
            CurrentNode = node;
            return true;
        }

        public virtual TreeNode<T>? RootNode(bool currentNode = false)
        {
            if (_treeRoot == null)
            {
                throw new PromptPlusException("Root not exists");
            }
            var node = _treeRoot;
            if (currentNode)
            {
                CurrentNode = node;
            }
            return node;
        }

        public virtual TreeNode<T>? FirstNode(bool currentNode = false)
        {
            if (_treeRoot == null)
            {
                throw new PromptPlusException("Root not exists");
            }
            var node = _treeRoot.Childrens?.First();
            if (currentNode)
            {
                CurrentNode = node;
            }
            return node;
        }

        public virtual TreeNode<T>? LastNode(bool currentNode = false)
        {
            if (_treeRoot == null)
            {
                throw new PromptPlusException("Root not exists");
            }
            var node = _treeRoot.Childrens?.Last();
            if (currentNode)
            {
                CurrentNode = node;
            }
            return node;
        }

        public virtual TreeNode<T> AddRootNode(T value, bool disabled = false, bool tryselect = false)
        {
            if (_treeRoot != null)
            {
                throw new PromptPlusException("Root node already exists");
            }
            _treeRoot = new TreeNode<T>
            {
                Level = 0,
                Text = _textfunc.Invoke(value),
                Value = value,
                IsExpanded = true,     
                IsDisabled = disabled,
                IsSelected = false
            };
            if (tryselect && (_validselect?.Invoke(_treeRoot.Value) ?? true))
            {
                _treeRoot.IsSelected = true;
            }
            _treedic = new()
            {
                { _treeRoot.UniqueId, _treeRoot }
            };
            return _treeRoot;
        }

        public virtual TreeNode<T> AddNode(TreeNode<T> parent, T value, bool disabled = false, bool tryselect = false)
        {
            TreeNode<T>? prev;
            parent.Childrens ??= new List<TreeNode<T>>();
            if (!parent.Childrens.Any())
            {
                prev = parent;
            }
            else
            {
                prev = parent.Childrens.Last();
            }

            var node = new TreeNode<T>
            {
                Level = parent.Level + 1,
                Text = _textfunc.Invoke(value),
                Parent = parent,
                PrevNode = prev,
                NextNode = null,
                Value = value,
                IsExpanded = false,
                IsDisabled = disabled,
                IsSelected = false,

            };
            if (tryselect && (_validselect?.Invoke(_treeRoot.Value) ?? true))
            {
                _treeRoot.IsSelected = true;
            }
            parent.Childrens.Add(node);
            if (prev != null)
            {
                prev.NextNode = node;
            }
            _treedic.Add(node.UniqueId, node);
            return node;
        }

        public virtual void Expand(TreeNode<T> node)
        {
            RecursiveExpand(node, true, false);
        }

        public virtual void ExpandAll(TreeNode<T> node)
        {
            RecursiveExpand(node, true, true);
        }

        public virtual void Collapse(TreeNode<T> node)
        {
            RecursiveCollapse(node, true, false);
        }

        public virtual void CollapseAll(TreeNode<T> node)
        {
            RecursiveCollapse(node, true, true);
        }

        public virtual void Select(TreeNode<T> node)
        {
            RecursiveSelect(true, node, true, false, null);
        }

        public virtual void SelectAll(TreeNode<T> node, Func<T, bool>? selectedexpression = null)
        {
            RecursiveSelect(true, node, true, true, selectedexpression);
        }

        public virtual void Disable(TreeNode<T> node)
        {
            RecursiveDisabled(true, node, true, false);
        }

        public virtual void DisableAll(TreeNode<T> node)
        {
            RecursiveDisabled(true, node, true, true);
        }

        public virtual void UnDisable(TreeNode<T> node)
        {
            RecursiveDisabled(false, node, true, false);
        }

        public virtual void UnDisableAll(TreeNode<T> node)
        {
            RecursiveDisabled(false, node, true, true);
        }

        public virtual void UnSelect(TreeNode<T> node)
        {
            RecursiveSelect(false, node, true, false);
        }

        public virtual void UnSelectectAll(TreeNode<T> node)
        {
            RecursiveSelect(false, node, true, true);
        }

        public TreeNode<T>? FindNode(string uniqueidnode)
        {
            if (_treedic.TryGetValue(uniqueidnode, out TreeNode<T> value))
            {
                return value;
            }
            return null;
        }

        private void RecursiveCollapse(TreeNode<T> node, bool check, bool recursive)
        {
            if (check)
            {
                if (_treeRoot == null)
                {
                    throw new PromptPlusException("Root not exists");
                }
                if (!ExistNode(node))
                {
                    throw new PromptPlusException("Node not exists");
                }
            }
            if (node.Childrens != null)
            {
                if (!node.IsRoot)
                {
                    node.IsExpanded = false;
                }
                if (recursive)
                {
                    foreach (TreeNode<T> child in node.Childrens)
                    {
                        RecursiveCollapse(child, false,true);
                    }
                }
            }
        }

        private void RecursiveExpand(TreeNode<T> node, bool check, bool recursive)
        {
            if (check)
            {
                if (_treeRoot == null)
                {
                    throw new PromptPlusException("Root not exists");
                }
                if (!ExistNode(node))
                {
                    throw new PromptPlusException("Node not exists");
                }
            }
            if (node.Childrens != null)
            {
                node.IsExpanded = true;
                if (recursive)
                {
                    foreach (TreeNode<T> child in node.Childrens)
                    {
                        RecursiveExpand(child, false,true);
                    }
                }
            }
        }

        private void RecursiveSelect(bool value, TreeNode<T> node, bool check, bool recursive, Func<T, bool> selectedexpression = null)
        {
            if (check)
            {
                if (_treeRoot == null)
                {
                    throw new PromptPlusException("Root not exists");
                }
                if (!ExistNode(node))
                {
                    throw new PromptPlusException("Node not exists");
                }
            }
            node.IsMarked = value;
            if (!node.IsDisabled)
            {
                if (selectedexpression?.Invoke(node.Value) ?? true)
                {
                    if (_validselect?.Invoke(node.Value) ?? true)
                    {
                        node.IsSelected = value;
                    }
                }
            }
            if (recursive) 
            {
                if (node.IsHasChild)
                {
                    foreach (var child in node.Childrens)
                    {
                        RecursiveSelect(value, child,false,true);
                    }
                }
            }
        }

        private void RecursiveDisabled(bool value, TreeNode<T> node, bool check, bool recursive)
        {
            if (check)
            {
                if (_treeRoot == null)
                {
                    throw new PromptPlusException("Root not exists");
                }
                if (!ExistNode(node))
                {
                    throw new PromptPlusException("Node not exists");
                }
            }
            node.IsDisabled = value;
            if (node.IsSelected && node.IsDisabled)
            { 
                node.IsSelected = false;
            }
            if (recursive)
            {
                if (node.IsHasChild)
                {
                    foreach (var child in node.Childrens)
                    {
                        RecursiveDisabled(value, child, false, true);
                    }
                }
            }
        }

        private bool ExistNode(TreeNode<T> node)
        {
            if (node == null)
            { 
                return false;
            }
            return _treedic.ContainsKey(node.UniqueId);
        }
    }
}
