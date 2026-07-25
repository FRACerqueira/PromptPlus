// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the final state of a MultiTasks control execution.
    /// </summary>
    /// <param name="elapsedtime">Total elapsed execution time of the whole run.</param>
    /// <param name="results">The per-task results.</param>
    /// <param name="aborted">Whether the run was aborted.</param>
    public readonly struct StateMultiTasks(TimeSpan elapsedtime, IReadOnlyList<MultiTaskResult> results, bool aborted)
    {
        /// <summary>
        /// Gets the total elapsed execution time of the whole run.
        /// </summary>
        public TimeSpan ElapsedTime { get; } = elapsedtime;

        private readonly IReadOnlyList<MultiTaskResult> _results = results ?? [];

        /// <summary>
        /// Gets the per-task results. Never <c>null</c>, even for a <c>default</c> instance.
        /// </summary>
        public IReadOnlyList<MultiTaskResult> Results => _results ?? [];

        /// <summary>
        /// Gets whether the run was aborted before all tasks finished.
        /// </summary>
        public bool Aborted { get; } = aborted;

        /// <summary>
        /// Gets whether every task finished successfully.
        /// </summary>
        public bool AllSucceeded => Results.Count > 0 && Results.All(r => r.State == MultiTaskState.Success);

        /// <summary>
        /// Gets whether at least one task finished with a failure.
        /// </summary>
        public bool AnyFailed => Results.Any(r => r.State == MultiTaskState.Failed);
    }
}
