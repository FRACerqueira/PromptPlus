// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a node of the tree exposed by <see cref="ITreeMultiSelectControl{T}"/> while it is
    /// being constructed. Extends <see cref="ITreeNode{T}"/> so that chaining off a node returned
    /// by <see cref="AddLast"/>/<see cref="AddFirst"/> keeps access to the TreeMultiSelect-specific
    /// <c>check</c> parameter, the same way the base <c>disable</c> parameter already works on
    /// <see cref="ITreeNode{T}"/>.
    /// </summary>
    /// <typeparam name="T">The user item type held by the node.</typeparam>
    public interface ITreeMultiSelectNode<T> : ITreeNode<T>
    {
        /// <summary>Appends a child at the end of this node's children collection.</summary>
        /// <param name="value">The value of the new child. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new child cannot be checked. Default is <c>false</c>.</param>
        /// <param name="check">
        /// When <c>true</c>, the new child starts pre-checked. Additive with
        /// <see cref="ITreeMultiSelectControl{T}.Default(System.Collections.Generic.IEnumerable{T}, bool)"/>
        /// and history — whichever mechanism marks a node checked, it stays checked, neither one
        /// clears the other. Subject to cascade the same way an interactive check would be (a
        /// checked container cascades to its descendants when <c>CascadeCheck</c> is on). Unlike
        /// <c>Default</c>, does not auto-expand the tree to reveal the node.
        /// </param>
        /// <returns>The newly created child node.</returns>
        ITreeMultiSelectNode<T> AddLast(T value, bool disable = false, bool check = false);

        /// <summary>Inserts a child at the beginning of this node's children collection.</summary>
        /// <param name="value">The value of the new child. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new child cannot be checked. Default is <c>false</c>.</param>
        /// <param name="check">Same semantics as in <see cref="AddLast"/>.</param>
        /// <returns>The newly created child node.</returns>
        ITreeMultiSelectNode<T> AddFirst(T value, bool disable = false, bool check = false);
    }
}
