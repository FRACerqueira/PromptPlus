using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // BaseControlPrompt's external-cancellation cleanup path (the `else if
    // (cts.Token.IsCancellationRequested ...)` branch in Run(), Controls/Common/
    // BaseControlPrompt.cs) that reacts to Ctrl+C / an external CancellationToken (as opposed to
    // an internal Escape-abort, which goes through the finish-block branch instead and is covered
    // by each control's own tests). There is no real Console.CancelKeyPress in a headless test, so
    // cancellation is simulated with the same "safety-net CancellationTokenSource" technique
    // already used across the suite (e.g. InputControlTests): no terminal key is queued, so
    // WaitKeypress spins until the token fires. Input is used as the vehicle; the cleanup logic
    // itself lives entirely in the base class and applies to every interactive (non-Live) control.
    public class CancelAbortRenderTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void External_cancellation_clears_the_frame_when_HideOnAbort_is_true()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .Options(o => o.HideOnAbort());
            _ = vt.Keys.Type("Joe");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Joe").Should().BeNull();
            _ = vt.Find("Name").Should().BeNull();
        }

        [Fact]
        public void External_cancellation_leaves_the_frame_when_HideOnAbort_is_false()
        {
            var vt = MakeTerminal();
            // Default HideOnAbortValue (no .Options call at all) is already false
            // (PromptConfig.HideOnAbort defaults to false) — set explicitly here for clarity.
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .Options(o => o.HideOnAbort(false));
            _ = vt.Keys.Type("Joe");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.TextAt(0, 0, 9).Should().Be("Name: Joe");
        }
    }
}
