using ConsolePlusLibrary;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Common;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // PromptPlusLibrary.Controls.Common.DisplayWidthHelpers — centralized text layout helpers built
    // on ConsolePlusLibrary.StringExtensions.GetDisplayLength/TruncateToDisplayWidth. Regression for
    // several real bugs found across controls: column/cell width, chart title alignment, chart item
    // label truncation, and calendar month/weekday headers were all computed from string.Length
    // (character count) instead of display width (terminal columns). A CJK character is 1 char but 2
    // columns per rune, so char-count math let content overflow its column budget and misalign
    // everything to the right of it. Used by TableControl/MultiTableControl (Truncate/AlignCell),
    // ChartBarControl (AlignLine/CountRunes/TruncateToRuneCount), and CalendarControl
    // (PadToDisplayWidth/FitToDisplayWidth) — previously near-identical logic duplicated per control
    // (Truncate/AlignCell were byte-identical between Table and MultiTable), now a single source.
    public class DisplayWidthHelpersTests
    {
        // 8 Hangul syllables, each 2 display columns = 16 columns total, 8 characters/runes.
        private const string Wide = "가나다라마바사아";

        [Fact]
        public void Truncate_keeps_ascii_text_unchanged_when_it_fits()
        {
            DisplayWidthHelpers.Truncate("abc", 10).Should().Be("abc");
        }

        [Fact]
        public void Truncate_stops_before_splitting_a_wide_rune()
        {
            // Budget of 5 columns must stop at 2 whole syllables (4 columns), not include half of a third.
            DisplayWidthHelpers.Truncate(Wide, 5).Should().Be("가나");
        }

        [Fact]
        public void AlignCell_left_pads_by_display_width_not_character_count()
        {
            // "가나다" is 3 characters but 6 display columns. Padding to a 10-column budget must add
            // 4 spaces (10-6), not 7 (10-3, the old .Length-based bug) — 7 would overflow the column
            // by 6 real columns once rendered.
            string result = DisplayWidthHelpers.AlignCell("가나다", 10, ColumnAlignment.Left);

            result.Should().Be("가나다    ");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignCell_right_pads_by_display_width_not_character_count()
        {
            string result = DisplayWidthHelpers.AlignCell("가나다", 10, ColumnAlignment.Right);

            result.Should().Be("    가나다");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignCell_center_pads_by_display_width_not_character_count()
        {
            string result = DisplayWidthHelpers.AlignCell("가나다", 10, ColumnAlignment.Center);

            result.Should().Be("  가나다  ");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignCell_truncates_wide_content_that_exceeds_the_budget_without_splitting_a_rune()
        {
            string result = DisplayWidthHelpers.AlignCell(Wide, 5, ColumnAlignment.Left);

            // 2 syllables (4 columns) + 1 padding column to reach the 5-column budget.
            result.Should().Be("가나 ");
            result.GetDisplayLength().Should().Equal(5);
        }

        [Fact]
        public void AlignLine_left_pads_ascii_text_by_character_count_as_before()
        {
            DisplayWidthHelpers.AlignLine("abc", 10, TextAlignment.Left).Should().Be("abc       ");
        }

        [Fact]
        public void AlignLine_left_pads_wide_text_by_display_width_not_character_count()
        {
            string result = DisplayWidthHelpers.AlignLine("가나다", 10, TextAlignment.Left);

            result.Should().Be("가나다    ");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignLine_center_pads_wide_text_by_display_width_not_character_count()
        {
            string result = DisplayWidthHelpers.AlignLine("가나다", 10, TextAlignment.Center);

            result.Should().Be("  가나다  ");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignLine_right_pads_wide_text_by_display_width_not_character_count()
        {
            string result = DisplayWidthHelpers.AlignLine("가나다", 10, TextAlignment.Right);

            result.Should().Be("    가나다");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void AlignLine_truncates_a_title_wider_than_the_chart_instead_of_overflowing_it()
        {
            // Old bug: PadRight(maxWidth) never truncates, so an 8-syllable (16-column) title into a
            // 10-column chart used to overflow the chart's width instead of fitting it.
            string result = DisplayWidthHelpers.AlignLine(Wide, 10, TextAlignment.Left);

            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void CountRunes_counts_cjk_syllables_as_one_symbol_each()
        {
            DisplayWidthHelpers.CountRunes(Wide).Should().Be(8);
        }

        [Fact]
        public void TruncateToRuneCount_keeps_the_maxlengthlabel_contract_as_symbol_count()
        {
            // Retention stays a count of runes/symbols (ChartBar's MaxLengthLabel documented
            // contract), independent of how many display columns those runes occupy.
            DisplayWidthHelpers.TruncateToRuneCount(Wide, 3).Should().Be("가나다");
        }

        [Fact]
        public void TruncateToRuneCount_returns_input_unchanged_when_within_budget()
        {
            DisplayWidthHelpers.TruncateToRuneCount("abc", 10).Should().Be("abc");
        }

        [Fact]
        public void PadToDisplayWidth_pads_ascii_text_by_character_count_as_before()
        {
            DisplayWidthHelpers.PadToDisplayWidth("July", 10).Should().Be("July      ");
        }

        [Fact]
        public void PadToDisplayWidth_pads_wide_text_by_display_width_not_character_count()
        {
            // "가나다" is 3 characters but 6 display columns. Padding to a 10-column budget must add
            // 4 spaces (10-6), not 7 (10-3, the old .Length-based bug).
            string result = DisplayWidthHelpers.PadToDisplayWidth("가나다", 10);

            result.Should().Be("가나다    ");
            result.GetDisplayLength().Should().Equal(10);
        }

        [Fact]
        public void PadToDisplayWidth_never_truncates_text_already_wider_than_the_budget()
        {
            DisplayWidthHelpers.PadToDisplayWidth(Wide, 5).Should().Be(Wide);
        }

        [Fact]
        public void FitToDisplayWidth_pads_left_when_narrower_than_the_budget()
        {
            DisplayWidthHelpers.FitToDisplayWidth("Mo", 3).Should().Be(" Mo");
        }

        [Fact]
        public void FitToDisplayWidth_pads_left_by_display_width_for_a_single_wide_rune()
        {
            // "월" (Korean "Mon" abbreviation) is 1 character but 2 display columns; fitting to a
            // 3-column budget must add 1 space (3-2), not 2 (3-1, the old .Length-based bug).
            string result = DisplayWidthHelpers.FitToDisplayWidth("월", 3);

            result.Should().Be(" 월");
            result.GetDisplayLength().Should().Equal(3);
        }

        [Fact]
        public void FitToDisplayWidth_truncates_without_splitting_a_wide_rune()
        {
            // 2 syllables (4 columns) already exceed a 3-column budget, so only 1 syllable (2 columns)
            // fits; the remaining 1 column is padded, never splitting the second syllable in half.
            string result = DisplayWidthHelpers.FitToDisplayWidth("가나", 3);

            result.Should().Be(" 가");
            result.GetDisplayLength().Should().Equal(3);
        }
    }
}
