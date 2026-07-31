// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a generic tree control that browses an
    /// arbitrary hierarchy of items of type <typeparamref name="T"/> as an expandable/collapsible tree.
    /// </summary>
    /// <typeparam name="T">The type of items in the tree.</typeparam>
    /// <remarks>
    /// The tree structure is built explicitly by the caller through <see cref="Root(T, bool)"/>,
    /// <see cref="AddLast(T, bool)"/>/<see cref="AddFirst(T, bool)"/> (first-level nodes),
    /// <see cref="AddAfter(ITreeNode{T}, T, bool)"/>/<see cref="AddBefore(ITreeNode{T}, T, bool)"/>
    /// (sibling insertion) and <see cref="ITreeNode{T}.AddLast(T, bool)"/>/
    /// <see cref="ITreeNode{T}.AddFirst(T, bool)"/> (nested children). Whether a node is a
    /// container or a leaf is inferred from whether it has children. The rendered tree
    /// materializes visible rows lazily on expand and releases them on collapse, keeping memory
    /// proportional to what is visible. Nodes can be marked <c>disable</c> at creation time so
    /// they are shown and navigable but cannot be confirmed.
    /// </remarks>
    public interface ITreeControl<T>
    {
        /// <summary>Applies the shared control options (prompt, tooltips, abort behavior).</summary>
        /// <exception cref="ArgumentNullException">When <paramref name="options"/> is <c>null</c>.</exception>
        ITreeControl<T> Options(Action<IControlOptions> options);

        /// <summary>Overrides visual styles for a specific region of the Tree control.</summary>
        ITreeControl<T> Styles(TreeStyles styleType, Style style);

        /// <summary>Sets the root value shown as the top-level node. Required.</summary>
        /// <param name="value">The root value. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the root cannot be confirmed. Default is <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <c>null</c>.</exception>
        ITreeControl<T> Root(T value, bool disable = false);

        /// <summary>Adds a first-level node (child of the root) at the end.</summary>
        /// <param name="value">The value of the new node. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new node cannot be confirmed. Default is <c>false</c>.</param>
        /// <returns>The newly created node so children can be attached to it.</returns>
        /// <exception cref="InvalidOperationException">When the root has not been set yet.</exception>
        ITreeNode<T> AddLast(T value, bool disable = false);

        /// <summary>Adds a first-level node (child of the root) at the beginning.</summary>
        /// <param name="value">The value of the new node. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new node cannot be confirmed. Default is <c>false</c>.</param>
        /// <exception cref="InvalidOperationException">When the root has not been set yet.</exception>
        ITreeNode<T> AddFirst(T value, bool disable = false);

        /// <summary>Inserts a sibling immediately after <paramref name="node"/>.</summary>
        /// <param name="node">The reference sibling. Cannot be <c>null</c>.</param>
        /// <param name="value">The value of the new node. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new node cannot be confirmed. Default is <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">When <paramref name="node"/> does not belong to this tree.</exception>
        ITreeNode<T> AddAfter(ITreeNode<T> node, T value, bool disable = false);

        /// <summary>Inserts a sibling immediately before <paramref name="node"/>.</summary>
        /// <param name="node">The reference sibling. Cannot be <c>null</c>.</param>
        /// <param name="value">The value of the new node. Cannot be <c>null</c>.</param>
        /// <param name="disable">When <c>true</c>, the new node cannot be confirmed. Default is <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">When <paramref name="node"/> does not belong to this tree.</exception>
        ITreeNode<T> AddBefore(ITreeNode<T> node, T value, bool disable = false);

        /// <summary>Sets the display text selector. Required.</summary>
        /// <exception cref="ArgumentNullException">When <paramref name="selector"/> is <c>null</c>.</exception>
        ITreeControl<T> TextSelector(Func<T, string> selector);

        /// <summary>Sets an optional extra info selector rendered next to the node text.</summary>
        /// <exception cref="ArgumentNullException">When <paramref name="extraInfoNode"/> is <c>null</c>.</exception>
        ITreeControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ExtraInfo(Func{T, string?})"/>.
        /// The task is awaited synchronously (blocking) once per node, per render frame.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="extraInfoNode"/> is <c>null</c>.</exception>
        ITreeControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode);

        /// <summary>Sets the character used to compose the full path in the answer line. Default is <c>'/'</c>.</summary>
        ITreeControl<T> PathSeparator(char value);

        /// <summary>Sets the maximum number of visible rows per page (0 = auto-fit).</summary>
        ITreeControl<T> PageSize(byte value);

        /// <summary>When enabled, blocks selection of container nodes (only leaves can be confirmed).</summary>
        ITreeControl<T> SelectLeafOnly(bool value = true);

        /// <summary>Shows the full path (parent chain) instead of only the entry name in the answer.</summary>
        ITreeControl<T> ShowFullPath(bool value = true);

        /// <summary>
        /// Sets the item comparator used to locate the default value and the value restored from
        /// history within the tree. Required.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="comparer   "/> is <c>null</c>.</exception>
        ITreeControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>Pre-selects an item, expanding the tree down to it when reachable from the root.</summary>
        /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ITreeControl<T> Default(T value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables history: the selected value is serialized as JSON and stored, and on the next run
        /// the tree is searched (using <see cref="DefaultMatchBy"/>) for an item that equals the restored
        /// value so that it can be pre-selected.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="filename"/> is <c>null</c>.</exception>
        ITreeControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Configures the control for view-only mode, where nodes can be navigated but not selected.
        /// </summary>
        /// <param name="value">If <c>true</c>, enables view-only mode; otherwise, item selection is enabled.</param>
        ITreeControl<T> ViewOnly(bool value = true);

        /// <summary>
        /// Dynamically updates the prompt description based on the currently selected node.
        /// </summary>
        /// <param name="value">A function that receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <c>null</c>.</exception>
        ITreeControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ChangeDescription(Func{T, string})"/>. The task is
        /// awaited synchronously (blocking) each frame.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <c>null</c>.</exception>
        ITreeControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Enables interactive filtering. When the user types a printable character while the tree
        /// is in select mode the control switches to filter mode, flattens the whole tree once and
        /// applies the requested <see cref="FilterMode"/> against the node full path (parent chain
        /// joined by <see cref="PathSeparator(char)"/>). Clearing the filter restores the lazy tree
        /// view preserving the previous expand/collapse state.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        ITreeControl<T> Filter(FilterMode value);

        /// <summary>
        /// Sets a validation predicate evaluated when the user presses Enter. When it returns
        /// <c>false</c>, the selection is rejected and a generic error is shown.
        /// </summary>
        ITreeControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate that returns a tuple: the first value indicates whether the
        /// item is valid, and the second is an optional error message shown when it is rejected.
        /// </summary>
        ITreeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate evaluated (blocking) when the user presses Enter.
        /// </summary>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread.</remarks>
        ITreeControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that returns a tuple: validity and an optional error message.
        /// </summary>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread.</remarks>
        ITreeControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Iterates <paramref name="items"/> and invokes <paramref name="interactionAction"/> for each
        /// element, giving the caller a chance to add first-level nodes (and further descendants)
        /// programmatically. Equivalent to calling <see cref="AddLast(T, bool)"/> inside the loop.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> or <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ITreeControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITreeControl<T>> interactionAction);

        /// <summary>
        /// Asynchronous counterpart of <see cref="Interaction{T1}(IEnumerable{T1}, Action{T1, ITreeControl{T}})"/>.
        /// The tasks are awaited sequentially (blocking) so tree construction remains deterministic.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="items"/> or <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ITreeControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITreeControl<T>, Task> interactionAction);

        /// <summary>Displays the Tree control and blocks until the user confirms or cancels.</summary>
        ResultPrompt<T?> Run(CancellationToken token = default);
    }
}
