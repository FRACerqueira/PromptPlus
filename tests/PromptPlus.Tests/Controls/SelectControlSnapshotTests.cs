using ConsolePlusLibrary.Testing;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // First Verify snapshot established for this suite: a golden file of the full grid for one
    // control's initial render.
    public class SelectControlSnapshotTests
    {
        [Fact]
        public Task Initial_render_matches_the_golden_grid()
        {
            var vt = VirtualTerminal.Create(o => { o.SupportsUnicode = false; });
            var control = new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose").AddItems(["A", "B", "C"]);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            return Verifier.Verify(vt.Snapshot());
        }
    }
}
