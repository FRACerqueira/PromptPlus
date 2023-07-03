// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class ItemTreeViewFlatNode<T> : IEquatable<ItemTreeViewFlatNode<T>>
    {
        public string UniqueId { get; set; }
        public ItemShowTreeView MessagesNodes { get; set; }
        public T Value { get; set; }
        public bool IsRoot { get; set; }
        public bool IsDisabled { get; set; }

        public bool Equals(ItemTreeViewFlatNode<T> other)
        {
            return UniqueId == other?.UniqueId;
        }

        public static bool operator ==(ItemTreeViewFlatNode<T> left, ItemTreeViewFlatNode<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemTreeViewFlatNode<T> left, ItemTreeViewFlatNode<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UniqueId);
        }

        public override bool Equals(object obj)
        {
            return obj is ItemTreeViewFlatNode<T> item && Equals(item);
        }
    }
}
