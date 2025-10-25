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
    /// Provides functionality for configuring and interacting with a Node Select Control.
    /// This interface enables building and managing hierarchical tree structures with customizable nodes,
    /// styles, and interactive selection capabilities.
    /// </summary>
    /// <typeparam name="T">Type of Node that represents the data in the tree structure</typeparam>
    public interface INodeTreeSelectControl<T>
    {
        /// <summary>
        /// Dynamically changes the description of the Select based on its value.
        /// The description updates whenever the selected value changes during interaction.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// Enables custom processing or modification of items during tree construction.
        /// </summary>
        /// <param name="items">The collection to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Interaction(IEnumerable<T> items, Action<T, INodeTreeSelectControl<T>> interactionAction);

        /// <summary>
        /// Applies custom options to the control to modify its behavior and appearance.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for specific elements of the Node Select control.
        /// Allows customization of visual appearance for different tree components.
        /// </summary>
        /// <param name="styleType">The <see cref="NodeTreeStyles"/> element to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Styles(NodeTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the function to display text for items in the list.
        /// Determines how each node is displayed in the tree structure.
        /// </summary>
        /// <param name="value">Function to generate display text for each item.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Adds a root node to the tree structure.
        /// The root node serves as the top-level entry point for the tree hierarchy.
        /// </summary>
        /// <param name="value">The node value to be added as root.</param>
        /// <param name="nodeseparator">The separator character used to build the node path. Defaults to "|".</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> already exists.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nodeseparator"/> is empty or null.</exception>
        INodeTreeSelectControl<T> AddRootNode(T value, string nodeseparator = "|");

        /// <summary>
        /// Adds a child node to an existing parent node in the tree.
        /// Enables building the hierarchical structure of the tree.
        /// </summary>
        /// <param name="parent">The parent node to attach the child to.</param>
        /// <param name="value">The node value to be added as a child.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="parent"/> not found.</exception>
        INodeTreeSelectControl<T> AddChildNode(T parent, T value);

        /// <summary>
        /// Sets the maximum number of items to display per page in the tree view.
        /// Controls the pagination of large tree structures.
        /// </summary>
        /// <param name="value">Number of maximum items per page.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        INodeTreeSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets a validation predicate for determining if an item can be selected.
        /// </summary>
        /// <param name="validselect">A predicate function returning true if the item should be selectable.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom message for item selection.
        /// Provides feedback when an item fails validation.
        /// </summary>
        /// <param name="validselect">A predicate function returning a tuple of (isValid, errorMessage).</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets a validation predicate for determining if an item should be disabled.
        /// Disabled items cannot be selected or interacted with.
        /// </summary>
        /// <param name="validdisabled">A predicate function returning true if the item should be disabled.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateDisabled(Func<T, bool> validdisabled);

        /// <summary>
        /// Controls the visibility of child node counts in the tree display.
        /// When enabled, hides the count indicator next to parent nodes.
        /// </summary>
        /// <param name="value">True to hide child counts, false to show them.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> HideSize(bool value = true);

        /// <summary>
        /// Executes the Node Select Control and provides the selection result.
        /// Handles user interaction and returns the final selected node.
        /// </summary>
        /// <param name="token">The cancellation token to observe while waiting for task completion.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected item.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
