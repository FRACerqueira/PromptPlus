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
    /// </summary>
    /// <typeparam name="T">type of Node</typeparam>
    public interface INodeTreeSelectControl<T>
    {
        /// <summary>
        /// Dynamically changes the description of the Select based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="interactionAction">The interaction action.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Interaction(IEnumerable<T> items, Action<T, INodeTreeSelectControl<T>> interactionAction);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Node Select control.
        /// </summary>
        /// <param name="styleType">The <see cref="FileStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> Styles(NodeTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the function to display text for items in the list. Default is <c>Item.ToString()</c>.
        /// </summary>
        /// <param name="value">Function to display item text.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Add a node 
        /// </summary>
        /// <param name="value">value node</param>
        /// <param name="nodeseparator">The separator character used to build the node path. Defaults to "|".</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> already exists.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nodeseparator"/> is empty or null.</exception>
        INodeTreeSelectControl<T> AddRootNode(T value, string nodeseparator = "|");

        /// <summary>
        /// Add a Child node
        /// </summary>
        /// <param name="parent">value parent</param>
        /// <param name="value">value node</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="parent"/> not found.</exception>
        INodeTreeSelectControl<T> AddChildNode(T parent, T value);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        INodeTreeSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable with custom message.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Set validation predicate for disabled item.
        /// </summary>
        /// <param name="validdisabled">A predicate function that determines whether an Item is considered disable.</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> PredicateDisabled(Func<T, bool> validdisabled);

        /// <summary>
        /// Hide count children. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="INodeTreeSelectControl{T}"/> instance for chaining.</returns>
        INodeTreeSelectControl<T> HideSize(bool value = true);

        /// <summary>
        /// Runs the Node Select Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the Node Select Control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
