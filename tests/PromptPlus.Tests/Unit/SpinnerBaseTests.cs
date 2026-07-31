using FluentAssertions;
using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // SpinnerBase (Core/SpinnerBase.cs) — pure unit-level. Frame-cycling base for every
    // spinner definition (Task/Progress controls). Uses a minimal test double since the class is
    // abstract.
    public class SpinnerBaseTests
    {
        private sealed class FakeSpinner : SpinnerBase
        {
            public IReadOnlyList<string>? FramesValue { get; set; }
            public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
            public override bool IsUnicode => false;
            public override IReadOnlyList<string> Frames => FramesValue!;
        }

        [Fact]
        public void CurrentFrame_before_any_NextFrame_call_is_empty()
        {
            var spinner = new FakeSpinner { FramesValue = ["a", "b", "c"] };
            _ = spinner.CurrentFrame.Should().BeEmpty();
        }

        [Fact]
        public void NextFrame_cycles_through_frames_in_order()
        {
            var spinner = new FakeSpinner { FramesValue = ["a", "b", "c"] };

            spinner.NextFrame();
            _ = spinner.CurrentFrame.Should().Be("a");

            spinner.NextFrame();
            _ = spinner.CurrentFrame.Should().Be("b");

            spinner.NextFrame();
            _ = spinner.CurrentFrame.Should().Be("c");
        }

        [Fact]
        public void NextFrame_wraps_around_after_the_last_frame()
        {
            var spinner = new FakeSpinner { FramesValue = ["a", "b"] };
            spinner.NextFrame();
            spinner.NextFrame();

            spinner.NextFrame();

            _ = spinner.CurrentFrame.Should().Be("a");
        }

        [Fact]
        public void NextFrame_with_an_empty_frame_list_sets_the_current_frame_to_empty()
        {
            var spinner = new FakeSpinner { FramesValue = [] };

            spinner.NextFrame();

            _ = spinner.CurrentFrame.Should().BeEmpty();
        }

        [Fact]
        public void NextFrame_with_null_frames_throws()
        {
            var spinner = new FakeSpinner { FramesValue = null };

            Action act = spinner.NextFrame;

            _ = act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void NextFrame_recovers_when_the_frame_list_shrinks_between_calls()
        {
            var spinner = new FakeSpinner { FramesValue = ["a", "b", "c", "d"] };
            spinner.NextFrame(); spinner.NextFrame(); spinner.NextFrame(); // cursor now at index 3 ("d" not yet shown)

            spinner.FramesValue = ["x", "y"]; // shrink while the cursor is out of range for the new list

            spinner.NextFrame();

            _ = spinner.CurrentFrame.Should().Be("x"); // cursor reset to 0 instead of throwing/staying out of range
        }
    }
}
