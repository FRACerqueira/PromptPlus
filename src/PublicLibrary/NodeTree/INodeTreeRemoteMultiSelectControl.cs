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
    /// Provides functionality for configuring and interacting with a Remote Node Multi Select Control.
    /// This interface enables building and managing hierarchical tree structures with customizable nodes,
    /// styles, and interactive selection capabilities.
    /// </summary>
    /// <typeparam name="T1">Type of Node that represents the data in the tree structure</typeparam>
    /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
    public interface INodeTreeRemoteMultiSelectControl<T1, T2> where T1 : class, new() where T2 : class
    {
        /// <summary>
        /// Dynamically changes the description of the Select based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> ChangeDescription(Func<T1, string> value);

        /// <summary>
        /// Applies custom options to configure the control's behavior.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites the visual styles for specific elements of the Node MultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="NodeTreeStyles"/> element to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> Styles(NodeTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the function to display text for items in the list.
        /// </summary>
        /// <param name="value">Function to convert an item to its display text. Must not be <c>null</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <remarks>If not set, defaults to using <c>Item.ToString()</c>.</remarks>
        INodeTreeRemoteMultiSelectControl<T1, T2> TextSelector(Func<T1, string> value);

        /// <summary>
        /// Disables or enables recursive counting of child nodes in the selection control. Defauts to disabled.
        /// </summary>
        /// <remarks>Disabling recursive counting may improve performance for large node trees by avoiding deep traversal when counting child nodes.</remarks>
        /// <param name="value">A value indicating whether recursive counting should be disabled. If <see langword="true"/>, recursive
        /// counting is disabled; otherwise, it remains enabled. </param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> DisableRecursiveCount(bool value = true);

        /// <summary>
        /// Configures the control to provide show additional information for node.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes a node of type T and returns a string containing extra information.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1, T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> ExtraInfo(Func<T1, string?> extraInfoNode);

        /// <summary>
        /// Sets an expression that defines the uniqueId field to string type. This expression is required for operation.
        /// </summary>
        /// <param name="uniquevalue">An function that defines the unique identification stringfor item.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uniquevalue"/> is null.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> UniqueId(Func<T1, string> uniquevalue);

        /// <summary>
        /// Sets a predicate that determines whether a node of type T1 is allowed to own other nodes. This expression is required for operation.
        /// </summary>
        /// <param name="childallowed">A function that receives a node of type T1 and returns <see langword="true"/> if it contains other child nodes.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> PredicateChildAllowed(Func<T1, bool> childallowed);


        /// <summary>
        /// Adds a root node to the tree structure.
        /// The root node serves as the top-level entry point for the tree hierarchy.
        /// </summary>
        /// <param name="value">The node value to be added as root.</param>
        /// <param name="initialvalue">Initial state or cursor of type <typeparamref name="T2"/> provided to the search function. Cannot be <c>null</c>.</param>
        /// <param name="valuechecked">If <c>true</c>, the node is initially checked. Default is <c>false</c>.</param>
        /// <param name="nodeseparator">The separator character used to build the node path. Defaults to "|".</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> already exists.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nodeseparator"/> is empty or null.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> AddRootNode(T1 value, T2 initialvalue, bool valuechecked = false, string nodeseparator = "|");

        /// <summary>
        /// Creates a multi-select control that searches for additional items using the specified value selector
        /// function.
        /// </summary>
        /// <param name="values">A function that determines which items to include in the search results. The function receives a value of
        /// type T1 and a state of type T2, and returns a tuple containing a boolean indicating whether to continue
        /// searching, an updated state, and a collection of result pairs where each pair contains a boolean flag(checked) and an
        /// item of type T1.</param>
        /// <param name="erroMessage">An optional function that generates a custom error message from an exception encountered during the search
        /// operation. If null, a default error message is used.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> SearchMoreItems(Func<T1, T2, (bool, T2, IEnumerable<(bool, T1)>)> values, Func<Exception, string>? erroMessage = null);

        /// <summary>
        /// Sets the maximum number of items displayed per page in the control.
        /// </summary>
        /// <param name="value">Number of items per page. Must be greater than 0.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        /// <remarks>Default value is 10 items per page.</remarks>
        INodeTreeRemoteMultiSelectControl<T1, T2> PageSize(byte value);

        /// <summary>
        /// Sets a validation predicate to determine if an item can be selected.
        /// </summary>
        /// <param name="validselect">A function that evaluates if an item is valid for selection.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom message for item selection.
        /// </summary>
        /// <param name="validselect">A function returning a tuple of (isValid, errorMessage) for selection validation.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> PredicateSelected(Func<T1, (bool, string?)> validselect);

        /// <summary>
        /// Sets a predicate to determine if an item should be disabled.
        /// </summary>
        /// <param name="validdisabled">A function that evaluates if an item should be disabled.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> PredicateDisabled(Func<T1, bool> validdisabled);

        /// <summary>
        /// Defines the allowed range for the number of selected items.
        /// </summary>
        /// <param name="minvalue">Minimum number of items that must be selected.</param>
        /// <param name="maxvalue">Optional maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        INodeTreeRemoteMultiSelectControl<T1, T2> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Controls the visibility of the selected items count tip.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the selected items count. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets the maximum width for displaying selected items.
        /// </summary>
        /// <param name="maxWidth">Maximum number of characters to display. Must be at least 10.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 10.</exception>
        /// <remarks>Default value is 30 characters.</remarks>
        INodeTreeRemoteMultiSelectControl<T1, T2> MaxWidth(byte maxWidth);

        /// <summary>
        /// Controls the visibility of the children count display.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the children count. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="INodeTreeRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> HideCount(bool value = true);

        /// <summary>
        /// Executes the Node MultiSelect Control operation.
        /// </summary>
        /// <param name="token">Optional cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="ResultPrompt{T1}"/> containing an array of selected items.</returns>
        ResultPrompt<T1[]> Run(CancellationToken token = default);
    }
}
