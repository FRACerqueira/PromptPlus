// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a Node MultiSelect Control.
    /// </summary>
    /// <typeparam name="T">The type of node in the tree structure.</typeparam>
    public interface INodeTreeMultiSelectControl<T>
    {
        /// <summary>
        /// Dynamically changes the description of the Select based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeMultiSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        INodeTreeMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, INodeTreeMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Applies custom options to configure the control's behavior.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        INodeTreeMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites the visual styles for specific elements of the Node MultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="NodeTreeStyles"/> element to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        INodeTreeMultiSelectControl<T> Styles(NodeTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the function to display text for items in the list.
        /// </summary>
        /// <param name="value">Function to convert an item to its display text. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <remarks>If not set, defaults to using <c>Item.ToString()</c>.</remarks>
        INodeTreeMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Disables or enables recursive counting of child nodes in the selection control. Defauts to disabled.
        /// </summary>
        /// <remarks>Disabling recursive counting may improve performance for large node trees by avoiding deep traversal when counting child nodes.</remarks>
        /// <param name="value">A value indicating whether recursive counting should be disabled. If <see langword="true"/>, recursive
        /// counting is disabled; otherwise, it remains enabled. </param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> DisableRecursiveCount(bool value = true);

        /// <summary>
        /// Configures the control to provide show additional information for node.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes a node of type T and returns a string containing extra information.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Adds a root node to the tree structure.
        /// </summary>
        /// <param name="value">The value for the root node. Must be unique.</param>
        /// <param name="valuechecked">If <c>true</c>, the node is initially checked. Default is <c>false</c>.</param>
        /// <param name="nodeseparator">The separator character for the node path. Must not be empty. Default is "|".</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> already exists in the tree.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nodeseparator"/> is empty or null.</exception>
        INodeTreeMultiSelectControl<T> AddRootNode(T value, bool valuechecked = false, string nodeseparator = "|");

        /// <summary>
        /// Adds a child node to an existing parent node in the tree.
        /// </summary>
        /// <param name="parent">The parent node to add the child to. Must exist in the tree.</param>
        /// <param name="value">The value for the child node.</param>
        /// <param name="valuechecked">If <c>true</c>, the node is initially checked. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="parent"/> is not found in the tree.</exception>
        INodeTreeMultiSelectControl<T> AddChildNode(T parent, T value, bool valuechecked = false);

        /// <summary>
        /// Sets the maximum number of items displayed per page in the control.
        /// </summary>
        /// <param name="value">Number of items per page. Must be greater than 0.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        /// <remarks>Default value is 10 items per page.</remarks>
        INodeTreeMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets a validation predicate to determine if an item can be selected.
        /// </summary>
        /// <param name="validselect">A function that evaluates if an item is valid for selection.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom message for item selection.
        /// </summary>
        /// <param name="validselect">A function returning a tuple of (isValid, errorMessage) for selection validation.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets a predicate to determine if an item should be disabled.
        /// </summary>
        /// <param name="validdisabled">A function that evaluates if an item should be disabled.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> PredicateDisabled(Func<T, bool> validdisabled);

        /// <summary>
        /// Defines the allowed range for the number of selected items.
        /// </summary>
        /// <param name="minvalue">Minimum number of items that must be selected.</param>
        /// <param name="maxvalue">Optional maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        INodeTreeMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Controls the visibility of the selected items count tip.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the selected items count. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets the maximum width for displaying selected items.
        /// </summary>
        /// <param name="maxWidth">Maximum number of characters to display. Must be at least 10.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 10.</exception>
        /// <remarks>Default value is 30 characters.</remarks>
        INodeTreeMultiSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Controls the visibility of the children count display.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the children count. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="INodeTreeMultiSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeMultiSelectControl<T> HideCount(bool value = true);

        /// <summary>
        /// Executes the Node MultiSelect Control operation.
        /// </summary>
        /// <param name="token">Optional cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing an array of selected items.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);
    }
}
