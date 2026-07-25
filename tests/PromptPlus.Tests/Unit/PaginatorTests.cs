using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.Common;
using System;
using System.Linq;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // Paginator<T> (Controls/Common/Paginator.cs) — camada 1 (unidade pura, sem VirtualTerminal),
    // classe de apoio mais reusada do PromptPlus (Select, MultiSelect, Tree, MultiTree, MultiTasks,
    // MultiFile, ChartBar, Table, MultiTable, FileExec, Calendar todas dependem dela via
    // ComputeEffectivePageSize). Exact expected values for the tricky navigation edges were
    // confirmed with a throwaway probe and observing the real output, not by hand-tracing.
    public class PaginatorTests
    {
        private static readonly string[] Items = ["A", "B", "C", "D", "E"]; // 5 items, pageSize 3 -> 2 pages (3 + 2)

        private static Paginator<string> Make(string[]? items = null, int pageSize = 3, Optional<string>? def = null, FilterMode filter = FilterMode.Disabled)
            => new(filter, items ?? Items, pageSize, def ?? Optional<string>.Empty(),
                foundDefault: (a, b) => a == b,
                textSelector: x => x,
                validatorAction: null,
                countValidator: null);

        [Fact]
        public void Construction_selects_the_first_item_by_default()
        {
            var p = Make();
            _ = p.SelectedIndex.Should().Be(0);
            _ = p.SelectedPage.Should().Be(0);
            _ = p.SelectedItem.Should().Be("A");
            _ = p.PageCount.Should().Be(2);
            _ = p.Count.Should().Be(3);
            _ = p.TotalCount.Should().Be(5);
        }

        [Fact]
        public void Construction_with_a_default_value_selects_that_item()
        {
            var p = Make(def: Optional<string>.Set("D"));
            _ = p.SelectedItem.Should().Be("D");
            _ = p.SelectedPage.Should().Be(1);
        }

        [Fact]
        public void NextItem_walks_forward_across_a_page_boundary()
        {
            var p = Make();
            _ = p.NextItem(); _ = p.NextItem(); // B, C (still page 0)
            _ = p.SelectedItem.Should().Be("C");
            _ = p.SelectedPage.Should().Be(0);

            _ = p.NextItem(); // crosses into page 1
            _ = p.SelectedItem.Should().Be("D");
            _ = p.SelectedPage.Should().Be(1);
        }

        [Fact]
        public void NextItem_at_the_absolute_last_item_does_not_wrap_around()
        {
            // NextItem/PreviousItem only cross PAGE boundaries; they never wrap past the ends of
            // the whole collection (NextPage/PreviousPage are the ones that wrap, via %PageCount).
            var p = Make();
            _ = p.End();
            _ = p.SelectedItem.Should().Be("E");

            bool moved = p.NextItem();

            _ = moved.Should().BeFalse();
            _ = p.SelectedItem.Should().Be("E");
        }

        [Fact]
        public void PreviousItem_at_the_absolute_first_item_does_not_wrap_around()
        {
            var p = Make();

            bool moved = p.PreviousItem();

            _ = moved.Should().BeFalse();
            _ = p.SelectedItem.Should().Be("A");
        }

        [Fact]
        public void NextPage_wraps_around_to_the_first_page()
        {
            var p = Make();
            _ = p.NextPage(IndexOption.FirstItem);
            _ = p.SelectedPage.Should().Be(1);

            _ = p.NextPage(IndexOption.FirstItem);

            _ = p.SelectedPage.Should().Be(0);
            _ = p.SelectedItem.Should().Be("A");
        }

        [Fact]
        public void PreviousPage_wraps_around_to_the_last_page_and_selects_its_actual_last_item()
        {
            // The last page is only partially filled (2 of 3 slots); PreviousPage's own wraparound
            // (distinct from FindValidItem's page-relative math) correctly lands on the real last
            // item (E), not an out-of-range slot.
            var p = Make();

            _ = p.PreviousPage(IndexOption.LastItem);

            _ = p.SelectedPage.Should().Be(1);
            _ = p.SelectedItem.Should().Be("E");
        }

        [Fact]
        public void End_and_Home_select_the_absolute_last_and_first_items()
        {
            var p = Make();
            _ = p.End();
            _ = p.SelectedItem.Should().Be("E");
            _ = p.SelectedPage.Should().Be(1);

            _ = p.Home();
            _ = p.SelectedItem.Should().Be("A");
            _ = p.SelectedPage.Should().Be(0);
        }

        [Fact]
        public void LastItem_called_directly_only_finds_the_last_item_of_the_CURRENT_page_not_the_whole_collection()
        {
            // Documents a real sharp edge: LastItem()/FirstItem() are page-relative helpers. End()
            // gets whole-collection semantics only because it explicitly jumps to the last page
            // (SelectedPage = (TotalCount-1)/_userPageSize) before calling LastItem(). Calling
            // LastItem() directly while parked on an earlier page does NOT do that jump.
            var p = Make(); // fresh, page 0

            _ = p.LastItem();

            _ = p.SelectedItem.Should().Be("C"); // last item of page 0, NOT the true last item (E)
            _ = p.SelectedPage.Should().Be(0);
        }

        [Fact]
        public void UpdatePageSize_preserves_the_global_position_of_the_selected_item()
        {
            var p = Make();
            _ = p.NextItem(); _ = p.NextItem(); _ = p.NextItem(); // D, global index 3
            _ = p.SelectedItem.Should().Be("D");

            p.UpdatePageSize(2);

            _ = p.PageCount.Should().Be(3); // ceil(5/2)
            _ = p.SelectedItem.Should().Be("D");
        }

        [Fact]
        public void UpdatePageSize_below_one_is_coerced_to_one()
        {
            var p = Make();
            p.UpdatePageSize(0);
            _ = p.PageCount.Should().Be(5); // one item per page
        }

        [Fact]
        public void GetPageData_returns_only_the_items_on_the_current_page()
        {
            var p = Make();
            _ = p.GetPageData().ToArray().Should().Equal("A", "B", "C");

            _ = p.NextPage(IndexOption.FirstItem);
            _ = p.GetPageData().ToArray().Should().Equal("D", "E");
        }

        [Fact]
        public void UpdateFilter_with_Contains_mode_narrows_the_collection_and_resets_to_page_zero()
        {
            var p = Make(filter: FilterMode.Contains);

            p.UpdateFilter("D");

            _ = p.TotalCount.Should().Be(1);
            _ = p.SelectedItem.Should().Be("D");
            _ = p.SelectedPage.Should().Be(0);
        }

        [Fact]
        public void UpdateFilter_with_an_empty_term_restores_the_full_collection()
        {
            var p = Make(filter: FilterMode.Contains);
            p.UpdateFilter("D");

            p.UpdateFilter("");

            _ = p.TotalCount.Should().Be(5);
        }

        [Fact]
        public void TotalCountValid_counts_only_items_accepted_by_the_count_validator()
        {
            var p = new Paginator<string>(FilterMode.Disabled, Items, 3, Optional<string>.Empty(),
                (a, b) => a == b, x => x, validatorAction: null, countValidator: x => x is "A" or "C" or "E");

            _ = p.TotalCountValid.Should().Be(3);
        }

        [Fact]
        public void EnsureVisibleIndex_moves_selection_to_the_page_containing_that_global_index()
        {
            var p = Make();

            p.EnsureVisibleIndex(4); // E, global index 4 -> page 1, local index 1

            _ = p.SelectedPage.Should().Be(1);
            _ = p.SelectedIndex.Should().Be(1);
            _ = p.SelectedItem.Should().Be("E");
        }

        [Fact]
        public void EnsureVisibleIndex_out_of_range_is_a_no_op()
        {
            var p = Make();

            p.EnsureVisibleIndex(99);

            _ = p.SelectedItem.Should().Be("A");
        }

        [Fact]
        public void Constructing_with_pageSize_zero_throws_instead_of_silently_misbehaving()
        {
            // Documents the contract: callers MUST pass a page size >= 1 (BaseControlPrompt's
            // ComputeEffectivePageSize already guarantees this in production, per TEST-PLAN.md).
            Action act = () => Make(pageSize: 0);
            _ = act.Should().Throw<DivideByZeroException>();
        }

        // Regression for a real bug found while writing this suite: UpdateCollection did not reset
        // SelectedPage before calling FirstItem(), so reloading a SHORTER collection while parked on
        // a later page left the paginator completely unselected (SelectedIndex=-1) even though the
        // new collection had valid items. UpdateFilter already reset SelectedPage correctly — this
        // brings UpdateCollection in line with it. Affects MultiSelectControl (~9 call sites) and
        // MultiTableControl (3 call sites), typically their "show selected only" / refresh features.
        [Fact]
        public void UpdateCollection_resets_to_the_first_item_even_when_parked_on_a_now_out_of_range_page()
        {
            var p = Make(); // 5 items, pageSize 3
            _ = p.NextPage(IndexOption.FirstItem); // page 1
            _ = p.SelectedPage.Should().Be(1);

            p.UpdateCollection(["X", "Y"]); // shorter collection: only 1 page now

            _ = p.IsUnselected.Should().BeFalse();
            _ = p.SelectedPage.Should().Be(0);
            _ = p.SelectedItem.Should().Be("X");
            _ = p.TotalCount.Should().Be(2);
        }

        [Fact]
        public void UpdateCollection_with_a_selected_value_positions_on_that_item_regardless_of_the_previous_page()
        {
            var p = Make();
            _ = p.NextPage(IndexOption.FirstItem); // page 1

            p.UpdateCollection(["X", "Y", "Z"], Optional<string>.Set("Z"));

            _ = p.SelectedItem.Should().Be("Z");
        }
    }
}
