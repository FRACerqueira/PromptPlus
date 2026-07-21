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
    /// Provides a fluent API for configuring and running a generic multi-selection tree control
    /// that browses an arbitrary hierarchy of items of type <typeparamref name="T"/> as an
    /// expandable/collapsible tree with tri-state checkboxes (unchecked / checked / indeterminate).
    /// </summary>
    /// <typeparam name="T">The type of items in the tree.</typeparam>
    /// <remarks>
    /// The tree structure is built the same way as <see cref="ITreeControl{T}"/>: via
    /// <see cref="Root(T)"/>, <see cref="AddLast(T)"/>/<see cref="AddFirst(T)"/>,
    /// <see cref="AddAfter"/>/<see cref="AddBefore"/> and the <see cref="ITreeNode{T}"/> children
    /// helpers. Container nodes display a tri-state checkbox that reflects the aggregate check
    /// state of their descendants. Pressing the check key (Space) on a container cycles through
    /// Unchecked → Checked (all descendants) → Unchecked. Pressing Enter confirms the selection
    /// and returns all checked leaf (or all checked) values.
    /// </remarks>
    public interface IMultiTreeControl<T>
    {
        /// <summary>
        /// Runs the MultiTree control and returns the result.
        /// </summary>
        /// <param name="token">Cancellation token. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>
        /// A <see cref="ResultPrompt{T}"/> whose <c>Content</c> is the array of checked values,
        /// or an aborted result if the user cancelled.
        /// </returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);

        /// <summary>Applies custom options to the control.</summary>
        IMultiTreeControl<T> Options(Action<IControlOptions> options);

        /// <summary>Overrides a style region for the MultiTree control.</summary>
        IMultiTreeControl<T> Styles(MultiTreeStyles styleType, Style style);

        /// <summary>
        /// Sets the root value of the tree. Must be called before adding any children.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiTreeControl<T> Root(T value);

        /// <summary>
        /// Adds a new node as the last child of the root and returns it so children can be
        /// appended to it.
        /// </summary>
        ITreeNode<T> AddLast(T value);

        /// <summary>
        /// Adds a new node as the first child of the root and returns it.
        /// </summary>
        ITreeNode<T> AddFirst(T value);

        /// <summary>
        /// Inserts a new sibling immediately after <paramref name="node"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <paramref name="node"/> does not belong to this tree or is the root.
        /// </exception>
        ITreeNode<T> AddAfter(ITreeNode<T> node, T value);

        /// <summary>
        /// Inserts a new sibling immediately before <paramref name="node"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <paramref name="node"/> does not belong to this tree or is the root.
        /// </exception>
        ITreeNode<T> AddBefore(ITreeNode<T> node, T value);

        /// <summary>
        /// Sets the function used to obtain the display text for each node.
        /// </summary>
        IMultiTreeControl<T> TextSelector(Func<T, string> selector);

        /// <summary>
        /// Sets a function that returns optional extra information rendered next to each node label.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="extraInfoNode"/> is <c>null</c>.</exception>
        IMultiTreeControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ExtraInfo(Func{T, string?})"/>.
        /// The task is awaited synchronously (blocking) once per node, per render frame.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="extraInfoNode"/> is <c>null</c>.</exception>
        IMultiTreeControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode);

        /// <summary>
        /// Sets the path separator character used when showing full paths. Default is <c>'/'</c>.
        /// </summary>
        IMultiTreeControl<T> PathSeparator(char value);

        /// <summary>
        /// Sets the maximum number of visible rows per page.
        /// </summary>
        IMultiTreeControl<T> PageSize(byte value);

        /// <summary>
        /// When <c>true</c>, only leaf nodes (nodes without children) can be checked.
        /// Checking a container is blocked. Default is <c>false</c>.
        /// </summary>
        IMultiTreeControl<T> CheckLeafOnly(bool value = true);

        /// <summary>
        /// When <c>true</c>, the answer line shows the full ancestor path for each checked item
        /// instead of just its own name. Default is <c>false</c>.
        /// </summary>
        IMultiTreeControl<T> ShowFullPath(bool value = true);

        /// <summary>
        /// When <c>true</c> (default), checking/unchecking a container propagates the new state
        /// to all its descendants. When <c>false</c>, only the container itself is toggled.
        /// </summary>
        IMultiTreeControl<T> CascadeCheck(bool value = true);

        /// <summary>
        /// Enables using <c>Ctrl+Space</c> for the recursive container selection (check/uncheck
        /// the container and all descendants). When enabled, plain <c>Space</c> only toggles the
        /// checked state of the selected node itself, and the recursive action is moved to
        /// <c>Ctrl+Space</c>. When disabled (default), plain <c>Space</c> performs the recursive
        /// selection on containers (if <see cref="CascadeCheck"/> is <c>true</c>).
        /// </summary>
        /// <param name="value"><c>true</c> to use <c>Ctrl+Space</c> for recursive marking; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiTreeControl{T}"/> instance for chaining.</returns>
        IMultiTreeControl<T> RecursiveMarkWithCtrlSpace(bool value = true);

        /// <summary>
        /// Sets the equality comparer used to match items (e.g. for <see cref="Default"/> lookup).
        /// </summary>
        IMultiTreeControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>
        /// Pre-checks one or more items. The tree auto-expands to each pre-checked node.
        /// When <paramref name="useDefaultHistory"/> is <c>true</c> and history is enabled,
        /// the history values override <paramref name="values"/>.
        /// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMultiTreeControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables history persistence. Previously checked items are restored on next run.
        /// </summary>
        IMultiTreeControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Puts the control into view-only mode. The user can navigate and expand/collapse the
        /// tree but cannot check items. Enter returns the pre-checked defaults.
        /// </summary>
        IMultiTreeControl<T> ViewOnly(bool value = true);

        /// <summary>
        /// Dynamically updates the description area based on the node currently under the cursor.
        /// </summary>
        IMultiTreeControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Asynchronous variant of <see cref="ChangeDescription"/>.
        /// </summary>
        IMultiTreeControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Sets the filter strategy for the filter mode. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        IMultiTreeControl<T> Filter(FilterMode value);

        /// <summary>
        /// Sets a predicate that decides whether a node can be checked.
        /// Nodes that fail the predicate show an error when the user tries to check them.
        /// </summary>
        IMultiTreeControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Variant that also returns a custom error message when the node cannot be checked.
        /// </summary>
        IMultiTreeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>Asynchronous variant of <see cref="PredicateSelected(Func{T,bool})"/>.</summary>
        IMultiTreeControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>Asynchronous variant with custom error message.</summary>
        IMultiTreeControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Defines the valid range for the number of checked items.
        /// Confirmation is blocked until the count falls within <c>[minvalue, maxvalue]</c>.
        /// </summary>
        /// <param name="minvalue">Minimum number of checked items (≥ 0).</param>
        /// <param name="maxvalue">Optional maximum. When <c>null</c> there is no upper bound.</param>
        IMultiTreeControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Iterates over <paramref name="items"/> and invokes <paramref name="interactionAction"/>
        /// for each element, allowing bulk population of the tree.
        /// </summary>
        IMultiTreeControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiTreeControl<T>> interactionAction);

        /// <summary>
        /// Asynchronous variant of <see cref="Interaction{T1}"/>. Each callback is awaited
        /// synchronously so the tree is fully populated before <c>Run</c> is called.
        /// </summary>
        IMultiTreeControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiTreeControl<T>, Task> interactionAction);
    }
}
