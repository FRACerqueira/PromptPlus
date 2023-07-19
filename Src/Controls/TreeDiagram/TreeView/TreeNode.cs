// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the tree node
    /// </summary>
    /// <typeparam name="T">type of Node</typeparam>
    public class TreeNode<T>
    {
        private readonly string _uniqueId;

        /// <summary>
        /// Create a tree node
        /// </summary>
        public TreeNode()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Node Level 
        /// </summary>
        public int Level { get; internal set; } = 0;

        /// <summary>
        /// Text Node
        /// </summary>
        public string Text { get; internal set; } = string.Empty;

        /// <summary>
        /// Value of node
        /// </summary>
        public T Value { get; internal set; } = default;

        /// <summary>
        /// Node Is root. Top Level
        /// </summary>
        public bool IsRoot => (Parent == null);

        /// <summary>
        /// Node has Child
        /// </summary>
        public bool IsHasChild => Childrens?.Any()??false;

        /// <summary>
        /// Node expandend
        /// </summary>
        public bool IsExpanded { get; internal set; } = false;

        /// <summary>
        /// Node Current.  
        /// </summary>
        public bool IsSelected { get; internal set; } = false;

        /// <summary>
        /// Node disabled
        /// </summary>
        public bool IsDisabled { get; internal set; } = false;

        /// <summary>
        /// Node Marked 
        /// </summary>
        public bool IsMarked { get; internal set; } = false;

        /// <summary>
        /// List of Children's nodes of node
        /// </summary>
        public List<TreeNode<T>> Childrens { get; internal set; } = null;

        /// <summary>
        /// Parent node
        /// </summary>
        public TreeNode<T> Parent { get; internal set; } = null;

        /// <summary>
        /// Next node
        /// </summary>
        public TreeNode<T> NextNode { get; internal set; } = null;

        /// <summary>
        /// Previus node
        /// </summary>
        public TreeNode<T> PrevNode { get; internal set; } = null;

        internal string UniqueId => _uniqueId;
    }
}
