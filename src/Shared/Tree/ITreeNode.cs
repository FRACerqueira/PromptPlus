// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a node of the tree exposed by the <see cref="ITreeControl{T}"/> when it is being
    /// constructed. A node carries a user value and can have any number of children added lazily
    /// through <see cref="AddLast"/> and <see cref="AddFirst"/>.
    /// </summary>
    /// <typeparam name="T">The user item type held by the node.</typeparam>
    public interface ITreeNode<T>
    {
        /// <summary>The user value associated with this node.</summary>
        T Value { get; }

        /// <summary>The parent node, or <c>null</c> when this node is the root of the tree.</summary>
        ITreeNode<T>? Parent { get; }

        /// <summary>Appends a child at the end of this node's children collection.</summary>
        /// <param name="value">The value of the new child. Cannot be <c>null</c>.</param>
        /// <returns>The newly created child node.</returns>
        ITreeNode<T> AddLast(T value);

        /// <summary>Inserts a child at the beginning of this node's children collection.</summary>
        /// <param name="value">The value of the new child. Cannot be <c>null</c>.</param>
        /// <returns>The newly created child node.</returns>
        ITreeNode<T> AddFirst(T value);
    }
}
