using ConsolePlusLibrary;
using FluentAssertions;
using PromptPlusLibrary.Controls.Common;
using System.Linq;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // BufferScreen/BufferState/LineScreen (Controls/Common/{BufferScreen,BufferState,LineScreen}.cs)
    // — pure unit-level (no VirtualTerminal), the render-diff engine that BaseControlPrompt.RenderBuffer
    // uses for every control. Exact expected values for the reflow math (PhysicalLineCount) and the
    // diff edge cases were confirmed with a throwaway probe.
    public class BufferScreenTests
    {
        private static readonly Style S = new(new Color(1, 1, 1), new Color(2, 2, 2));

        [Fact]
        public void Write_appends_to_the_current_line_without_starting_a_new_one()
        {
            var b = new BufferState();
            b.Write("hello", S);
            b.Write(" world", S);

            _ = b.Count.Should().Be(1);
            _ = b.GetLines()[0].ContentSize.Should().Be(11);
        }

        [Fact]
        public void WriteLine_leaves_a_trailing_empty_line_ready_for_the_next_write()
        {
            // WriteLine adds a line-break to the current line AND appends a new (empty) LineScreen
            // for whatever comes next — mirroring where a real cursor would sit after a newline.
            var b = new BufferState();
            b.WriteLine("hello", S);

            _ = b.Count.Should().Be(2);
            _ = b.GetLines()[0].ContentSize.Should().Be(5);
            _ = b.GetLines()[1].ContentSize.Should().Be(0);
        }

        [Fact]
        public void ContentEquals_is_true_for_lines_with_the_same_fragments()
        {
            var a = new LineScreen([new Fragment("hi", S)]);
            var b = new LineScreen([new Fragment("hi", S)]);

            _ = a.ContentEquals(b).Should().BeTrue();
        }

        [Fact]
        public void ContentEquals_is_false_when_the_text_differs()
        {
            var a = new LineScreen([new Fragment("hi", S)]);
            var b = new LineScreen([new Fragment("bye", S)]);

            _ = a.ContentEquals(b).Should().BeFalse();
        }

        [Fact]
        public void ContentEquals_is_false_when_only_the_style_differs()
        {
            var other = new Style(new Color(9, 9, 9), new Color(8, 8, 8));
            var a = new LineScreen([new Fragment("hi", S)]);
            var b = new LineScreen([new Fragment("hi", other)]);

            _ = a.ContentEquals(b).Should().BeFalse();
        }

        [Fact]
        public void ContentEquals_against_null_is_false()
        {
            var a = new LineScreen([new Fragment("hi", S)]);
            _ = a.ContentEquals(null).Should().BeFalse();
        }

        [Fact]
        public void UpdateBufferDiff_on_the_first_frame_reports_every_line_as_changed()
        {
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);

            var diff = bs.UpdateBufferDiff();

            _ = diff.Select(d => d.Row).Should().Equal(0, 1, 2); // WriteLine x2 -> 3 LineScreen entries
        }

        [Fact]
        public void UpdateBufferDiff_reports_nothing_when_the_next_frame_is_identical()
        {
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);
            _ = bs.UpdateBufferDiff();

            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);
            var diff = bs.UpdateBufferDiff();

            _ = diff.Should().BeEmpty();
        }

        [Fact]
        public void UpdateBufferDiff_reports_only_the_rows_that_actually_changed()
        {
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);
            _ = bs.UpdateBufferDiff();

            bs.WriteLine("line0", S);
            bs.WriteLine("CHANGED", S);
            var diff = bs.UpdateBufferDiff();

            _ = diff.Select(d => d.Row).Should().Equal(1);
        }

        [Fact]
        public void UpdateBufferDiff_includes_brand_new_rows_beyond_the_previous_frames_length()
        {
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            _ = bs.UpdateBufferDiff(); // 2 rows (content + trailing)

            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);
            bs.WriteLine("line2", S);
            var diff = bs.UpdateBufferDiff();

            _ = diff.Select(d => d.Row).Should().Contain([2, 3]); // rows beyond the old 2-row frame
        }

        [Fact]
        public void UpdateBufferDiff_does_not_report_rows_removed_by_a_shorter_frame()
        {
            // A frame with FEWER lines than before is not flagged for the removed rows — the caller
            // (BaseControlPrompt.RenderBuffer) clears those separately by comparing OriginalLineCount
            // vs CurrentLineCount itself; UpdateBufferDiff only ever compares the overlapping range
            // plus genuinely new rows.
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            bs.WriteLine("line1", S);
            bs.WriteLine("line2", S);
            _ = bs.UpdateBufferDiff(); // 4 rows total

            bs.WriteLine("line0", S); // only 2 rows this frame
            var diff = bs.UpdateBufferDiff();

            _ = diff.Select(d => d.Row).Should().NotContain([2, 3]);
        }

        [Fact]
        public void PhysicalLineCount_of_a_short_line_that_fits_is_one_row()
        {
            var b = new BufferState();
            b.Write("hello", S);

            _ = b.PhysicalLineCount(startLeft: 0, width: 80, renderedWidth: 80).Should().Be(1);
        }

        [Fact]
        public void PhysicalLineCount_wraps_a_long_line_clipped_at_the_old_render_width()
        {
            var b = new BufferState();
            b.Write(new string('x', 100), S); // ContentSize 100, but clipped to 80 at the old render width

            // Clipped to 80 columns (old renderedWidth), then wrapped at the NEW width of 40 -> ceil(80/40)=2.
            _ = b.PhysicalLineCount(startLeft: 0, width: 40, renderedWidth: 80).Should().Be(2);
        }

        [Fact]
        public void PhysicalLineCount_accounts_for_a_nonzero_start_column()
        {
            var b = new BufferState();
            b.Write("hello", S);

            _ = b.PhysicalLineCount(startLeft: 5, width: 80, renderedWidth: 80).Should().Be(1);
        }

        [Fact]
        public void PhysicalLineCount_with_width_zero_or_negative_falls_back_to_the_raw_line_count()
        {
            var b = new BufferState();
            b.WriteLine("a", S);
            b.WriteLine("b", S);

            _ = b.PhysicalLineCount(startLeft: 0, width: 0, renderedWidth: 80).Should().Be(b.Count);
        }

        [Fact]
        public void PhysicalLineCount_with_no_prior_render_width_uses_the_full_content_size_unclipped()
        {
            var b = new BufferState();
            b.Write(new string('y', 50), S);

            // renderedWidth<=0 means "no clipping info" -> the full 50-char content wraps at width 20:
            // ceil(50/20) = 3.
            _ = b.PhysicalLineCount(startLeft: 0, width: 20, renderedWidth: 0).Should().Be(3);
        }

        [Fact]
        public void PhysicalLineCount_sums_across_multiple_lines()
        {
            var b = new BufferState();
            b.WriteLine(new string('a', 10), S);
            b.WriteLine(new string('b', 25), S);

            // line0=10 -> 1 row; line1=25 -> ceil(25/10)=3 rows; trailing empty line -> 1 row. Total 5.
            _ = b.PhysicalLineCount(startLeft: 0, width: 10, renderedWidth: 80).Should().Be(5);
        }

        [Fact]
        public void SetPromptCursor_and_SavePromptCursor_expose_the_last_recorded_position()
        {
            var bs = new BufferScreen();
            bs.SetPromptCursor(3, 1);
            _ = bs.PromptCursor.Should().Be((3, 1));

            bs.WriteLine("hello", S);
            bs.SavePromptCursor();

            // SavePromptCursor parks at (ContentSize of the last line, line index of the last line).
            _ = bs.PromptCursor.Should().Be((0, 1)); // last line is the trailing empty one (index 1, size 0)
        }

        [Fact]
        public void Reset_clears_both_the_current_and_original_buffers()
        {
            var bs = new BufferScreen();
            bs.WriteLine("line0", S);
            _ = bs.UpdateBufferDiff();
            bs.WriteLine("line1", S);

            bs.Reset();

            _ = bs.OriginalLineCount.Should().Be(0);
            _ = bs.CurrentLineCount.Should().Be(0);
        }
    }
}
