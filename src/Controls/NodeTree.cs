// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary
{
    internal sealed class NodeTree<T>
    {

        private readonly string _uniqueId;

        /// <summary>
        /// Create a instance
        /// </summary>
        public NodeTree()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public string UniqueId => _uniqueId;

        /// <summary>
        /// node
        /// </summary>
        public required T Node { get; set; }

        /// <summary>
        /// List of Children's nodes of node
        /// </summary>
        public List<NodeTree<T>> Childrens { get; set; } = [];

        /// <summary>
        /// Parent node
        /// </summary>
        public string? ParentiId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        public bool Checked { get; set; }
    }
}
