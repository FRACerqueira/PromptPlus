using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Groups every test class that spawns a real background Task (ProgressBar/TaskExec/MultiTasks's
    // caller-supplied handler, or MultiFile's background wildcard folder check) and depends on real
    // wall-clock margins (ManualResetEventSlim handshakes, fixed TestContext.Current.CancellationToken.WaitHandle.WaitOne waits) to observe
    // in-flight state deterministically. xUnit runs different test CLASSES in parallel by default;
    // under the full suite's ~600-test parallel load, thread-pool contention from dozens of OTHER
    // classes running at the same time was observed to occasionally push these margins past their
    // limit (confirmed flaky in isolation-vs-full-suite comparisons for both ProgressBarControlTests
    // and MultiFileControlRealFilesystemTests). Forcing all such classes into this single
    // non-parallel collection serializes them relative to EACH OTHER, so the tests that most need
    // real time to elapse are never competing against one another for CPU/thread-pool slots — a
    // targeted fix for the actual cause (scheduling contention) instead of just enlarging sleep
    // margins over and over. See FASE2-CONTROLS-PLAN.md's Grupo 6 section for the fuller rationale.
    [CollectionDefinition(Name, DisableParallelization = true)]
    public class BackgroundTimingCollection
    {
        public const string Name = "Background task timing";
    }
}
