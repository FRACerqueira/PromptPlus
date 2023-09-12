// ***************************************************************************************
// MIT LICENCE
// Copyright (c) 2019 shibayan.
// https://github.com/shibayan/Sharprompt
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{

    public class PaginatorTest : BaseTest
    {

        [Fact]
        public void Should_have_5_pages()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 50; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>( PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.s_empty,null);
            // Then
            Assert.Equal(50, pg.TotalCount);
            Assert.Equal(5, pg.PageCount);
        }

        [Fact]
        public void Should_have_1_item_lastpage()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 7; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 3, Optional<string>.s_empty, null);
            pg.EnsureVisibleIndex(6);
            // Then
            Assert.Equal(2, pg.SelectedPage);
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(1, pg.Count);
            Assert.True(pg.IsLastPageItem);
        }

        [Fact]
        public void Should_have_3_items_CurrentPage()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 7; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 3, Optional<string>.s_empty, null);
            pg.ToSubset();
            // Then
            Assert.Equal(0, pg.SelectedPage);
            Assert.Equal(3, pg.Count);
        }

        [Fact]
        public void Should_have_selectedItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.Create("3"),null);
            // Then
            Assert.Equal(3, pg.SelectedIndex);
            Assert.Equal("3", pg.SelectedItem);
        }

        [Fact]
        public void Should_have_UnSelected()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.Create("3"), null);
            // When
            pg.UnSelected();
            // Then
            Assert.Equal(-1, pg.SelectedIndex);
            Assert.Null(pg.SelectedItem);
            Assert.True(pg.IsUnSelected);
        }



        [Fact]
        public void Should_have_selectedItem_with_FistItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.Create("3"), null);
            // When
            pg.FirstItem();
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal("0", pg.SelectedItem);
        }

        [Fact]
        public void Should_have_selectedItem_with_LastItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.Create("0"), null);
            // When
            pg.LastItem();
            // Then
            Assert.Equal(4, pg.SelectedIndex);
            Assert.Equal("4", pg.SelectedItem);
        }

        [Fact]
        public void Should_have_selectedItem_with_NextPage_unselect()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty,null);
            // When
            pg.NextPage(IndexOption.None);
            // Then
            Assert.Equal(-1, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.FirstItem)]
        [InlineData(IndexOption.FirstItemWhenHasPages)]
        internal void Should_have_selectedItem_with_NextPage_FistSelect(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty, null);
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.LastItem)]
        [InlineData(IndexOption.LastItemWhenHasPages)]
        internal void Should_have_selectedItem_with_NextPage_LastSelect(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty, null);
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(4, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Fact]
        public void Should_have_selectedItem_with_FistItem_and_validator_Select()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 3, Optional<string>.Create("0"), null,null, (x) => x != "0");
            // When
            pg.FirstItem();
            // Then
            Assert.Equal(1, pg.SelectedIndex);
            Assert.Equal("1", pg.SelectedItem);
        }
        [Fact]
        public void Should_have_selectedItem_with_LastItem_and_validator_Select()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 10, Optional<string>.Create("0"), null, null, (x) => x != "4");
            // When
            pg.LastItem();
            // Then
            Assert.Equal(3, pg.SelectedIndex);
            Assert.Equal("3", pg.SelectedItem);
        }

        [Theory]
        [InlineData(IndexOption.FirstItem)]
        [InlineData(IndexOption.FirstItemWhenHasPages)]
        internal void Should_have_selectedItem_with_NextPage_FistSelect_and_validator_Select(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.Create("0"), null, null, (x) => x != "5");
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(1, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.LastItem)]
        [InlineData(IndexOption.LastItemWhenHasPages)]
        internal void Should_have_selectedItem_with_NextPage_LastSelect_and_validator_Selec(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty, null, null, (x) => x != "9");
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(3, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Fact]
        internal void Should_have_selectedItem_with_UpdateFilter()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty, null);
            // When
            pg.UpdateFilter("10");
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(0, pg.SelectedPage);
            Assert.Equal(1, pg.PageCount);
            Assert.Equal(1, pg.Count);
        }

        [Fact]
        internal void Should_have_selectedItem_with_UpdateFilter_and_tryget()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(PPlus.Controls.FilterMode.Contains, items, 5, Optional<string>.s_empty, null);
            // When
            pg.UpdateFilter("10");
            var ok = pg.TryGetSelectedItem(out var result);
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(0, pg.SelectedPage);
            Assert.Equal(1, pg.PageCount);
            Assert.Equal(1, pg.Count);
            Assert.True(ok);
            Assert.Equal("10", result);
        }
    }
}
