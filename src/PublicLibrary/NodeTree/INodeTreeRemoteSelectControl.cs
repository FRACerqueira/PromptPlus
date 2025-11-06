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
    /// Provides functionality for configuring and interacting with a Remote Node Select Control.
    /// This interface enables building and managing hierarchical tree structures with customizable nodes,
    /// styles, and interactive selection capabilities.
    /// </summary>
    /// <typeparam name="T1">Type of Node that represents the data in the tree structure</typeparam>
    /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
    public interface INodeTreeRemoteSelectControl<T1, T2> where T1 : class, new() where T2 : class
    {
        /// <summary>
        /// Dynamically changes the description of the Remote Node Select Control based on its value.
        /// The description updates whenever the selected value changes during interaction.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeRemoteSelectControl<T1, T2> ChangeDescription(Func<T1, string> value);

        /// <summary>
        /// Applies custom options to the control to modify its behavior and appearance.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        INodeTreeRemoteSelectControl<T1, T2> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for specific elements of the Remote Node Select Control.
        /// Allows customization of visual appearance for different tree components.
        /// </summary>
        /// <param name="styleType">The <see cref="NodeTreeStyles"/> element to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        INodeTreeRemoteSelectControl<T1, T2> Styles(NodeTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the function to display text for items in the list.
        /// Determines how each node is displayed in the tree structure.
        /// </summary>
        /// <param name="value">Function to generate display text for each item.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeRemoteSelectControl<T1, T2> TextSelector(Func<T1, string> value);

        /// <summary>
        /// Sets an expression that defines the uniqueId field to string type. This expression is required for operation.
        /// </summary>
        /// <param name="uniquevalue">An function that defines the unique identification stringfor item.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uniquevalue"/> is null.</exception>
        INodeTreeRemoteSelectControl<T1, T2> UniqueId(Func<T1, string> uniquevalue);

        /// <summary>
        /// Sets a predicate that determines whether a node of type T1 is allowed to own other nodes. This expression is required for operation.
        /// </summary>
        /// <param name="childallowed">A function that receives a node of type T1 and returns <see langword="true"/> if it contains other child nodes.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> PredicateChildAllowed(Func<T1, bool> childallowed);

        /// <summary>
        /// Configures the control to provide show additional information for node.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes a node of type T1 and returns a string containing extra information.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> ExtraInfo(Func<T1, string?> extraInfoNode);

        /// <summary>
        /// Adds a root node to the tree structure.
        /// The root node serves as the top-level entry point for the tree hierarchy.
        /// </summary>
        /// <param name="value">The node value to be added as root.</param>
        /// <param name="initialvalue">Initial state or cursor of type <typeparamref name="T2"/> provided to the search function. Cannot be <c>null</c>.</param>
        /// <param name="nodeseparator">The separator character used to build the node path. Defaults to "|".</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> already exists.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nodeseparator"/> is empty or null.</exception>
        INodeTreeRemoteSelectControl<T1, T2> AddRootNode(T1 value, T2 initialvalue, string nodeseparator = "|");

        /// <summary>
        /// Registers the function responsible for searching and returning the next collection of nodes child items to add to the list.This expression is required for operation.
        /// </summary>
        /// <param name="values">
        /// A function that accepts The selected node parent and current <typeparamref name="T2"/> state and returns a tuple:
        /// <c>bool</c>: indicates whether additional pages are available (true if more pages exist),
        /// <c>T2</c>: the next state/cursor to use when requesting subsequent pages,
        /// <c>IEnumerable{T1}</c>: the collection of child items retrieved for the selected node parent.
        /// This function cannot be <c>null</c>.
        /// </param>
        /// <param name="erroMessage">
        /// Optional function to map an <see cref="Exception"/> thrown during execution of <paramref name="values"/> into a user-friendly error message.
        /// If omitted, default error handling is used.
        /// </param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        INodeTreeRemoteSelectControl<T1, T2> SearchMoreItems(Func<T1, T2, (bool, T2, IEnumerable<T1>)> values, Func<Exception, string>? erroMessage = null);

        /// <summary>
        /// Sets the maximum number of items to display per page in the tree view.
        /// Controls the pagination of large tree structures.
        /// </summary>
        /// <param name="value">Number of maximum items per page.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        INodeTreeRemoteSelectControl<T1, T2> PageSize(byte value);

        /// <summary>
        /// Sets a validation predicate for determining if an item can be selected.
        /// </summary>
        /// <param name="validselect">A predicate function returning true if the item should be selectable.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> PredicateSelected(Func<T1, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom message for item selection.
        /// Provides feedback when an item fails validation.
        /// </summary>
        /// <param name="validselect">A predicate function returning a tuple of (isValid, errorMessage).</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> PredicateSelected(Func<T1, (bool, string?)> validselect);

        /// <summary>
        /// Sets a validation predicate for determining if an item should be disabled.
        /// Disabled items cannot be selected or interacted with.
        /// </summary>
        /// <param name="validdisabled">A predicate function returning true if the item should be disabled.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> PredicateDisabled(Func<T1, bool> validdisabled);

        /// <summary>
        /// Controls the visibility of child node counts in the tree display.
        /// When enabled, hides the count indicator next to parent nodes.
        /// </summary>
        /// <param name="value">True to hide child counts, false to show them.</param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> HideCount(bool value = true);


        /// <summary>
        /// Disables or enables recursive counting of child nodes in the selection control. Defauts to disabled.
        /// </summary>
        /// <remarks>Disabling recursive counting may improve performance for large node trees by avoiding deep traversal when counting child nodes.</remarks>
        /// <param name="value">A value indicating whether recursive counting should be disabled. If <see langword="true"/>, recursive
        /// counting is disabled; otherwise, it remains enabled. </param>
        /// <returns>The current <see cref="INodeTreeRemoteSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteSelectControl<T1, T2> DisableRecursiveCount(bool value = true);

        /// <summary>
        /// Executes the Remote Node Select Control and provides the selection result.
        /// Handles user interaction and returns the final selected node.
        /// </summary>
        /// <param name="token">The cancellation token to observe while waiting for task completion.</param>
        /// <returns>A <see cref="ResultPrompt{T1}"/> containing the selected item.</returns>
        ResultPrompt<T1> Run(CancellationToken token = default);
    }
}
