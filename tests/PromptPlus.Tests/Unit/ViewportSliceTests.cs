using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Input;
using PromptPlusLibrary.Core;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // BaseControlPrompt.ViewportSlice/ViewportSliceCore (Controls/Common/BaseControlPrompt.cs) —
    // scroll+ellipsis engine shared by Input/Select/MultiSelect/Table/MultiTable/Tree/
    // MultiTree/Calendar/File/MultiFile. Only comes into play when the text does NOT fit the
    // viewport (FitsInWidth returns false); when the text fits, the method exits via a simple
    // early-return, out of scope for these tests. Called directly through an InputControl instance
    // (any BaseControlPrompt<T> works — ViewportSlice is public on the base) to test the scroll math
    // in isolation, without depending on the render loop via VirtualTerminal.
    public class ViewportSliceTests
    {
        // fullText has 30 chars (indices 0-9 = "0123456789", 10-29 = "A".."T"), well longer than the
        // small viewports used below — deliberately reproduces the "text longer than the screen"
        // condition where the reported bug occurred.
        private const string LongText = "0123456789ABCDEFGHIJKLMNOPQRST";

        private static InputControl MakeProbe()
        {
            var vt = VirtualTerminal.Create(o => { o.SupportsUnicode = false; o.Width = 200; o.Height = 24; });
            return (InputControl)new PromptPlusControls(vt, new PromptConfig()).Input("probe");
        }

        [Fact]
        public void Cursor_at_absolute_end_leaves_one_free_column_for_the_caret()
        {
            var probe = MakeProbe();

            // viewportWidth=13, cursor at the absolute end of the text (position 30 == Length).
            (string visibleLeft, string visibleRight) = probe.ViewportSlice(LongText, LongText.Length, 200 - 13);

            _ = visibleRight.Should().BeEmpty("nothing exists after the cursor when it sits at the true end");
            _ = visibleLeft.Should().Be("_JKLMNOPQRST");
            // Original bug: visibleLeft occupied the full 13 columns (no margin), forcing the
            // cursor (which is positioned right after visibleLeft) to land on the same column as the
            // last character instead of one column past it.
            _ = (visibleLeft.Length + visibleRight.Length).Should().Be(12, "one column must stay free for the caret");
        }

        [Fact]
        public void Stepping_one_position_back_from_the_end_reveals_the_hidden_last_character_with_an_ellipsis()
        {
            var probe = MakeProbe();

            // Same text/viewport as the previous test, cursor one position before the end (29): the
            // 'T' becomes hidden on the right and needs an ellipsis to signal that.
            (string visibleLeft, string visibleRight) = probe.ViewportSlice(LongText, LongText.Length - 1, 200 - 13);

            // Original bug: 'T' was discarded by TrimToBudget to make room for the left ellipsis,
            // and the right ellipsis was never added because the hasHiddenRight flag had been
            // computed BEFORE the trim (based on the original window, where nothing was hidden yet).
            _ = visibleRight.Should().NotBeEmpty("the 'T' character got pushed out of view and must be signaled");
            _ = visibleRight.Should().EndWith("_", "the ASCII ellipsis marks hidden content on the right");
            _ = (visibleLeft.Length + visibleRight.Length).Should().Be(13, "cursor is not at the end here, so the full viewport can be used");
        }

        [Fact]
        public void Text_that_fits_the_viewport_is_returned_verbatim_with_no_ellipsis()
        {
            var probe = MakeProbe();

            (string visibleLeft, string visibleRight) = probe.ViewportSlice("abc", 2, 200 - 10);

            _ = visibleLeft.Should().Be("ab");
            _ = visibleRight.Should().Be("c");
        }
    }
}
