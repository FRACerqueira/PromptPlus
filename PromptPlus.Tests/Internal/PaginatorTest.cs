using System;
using System.Collections.Generic;


using PPlus.Drivers;
using PPlus.Internal;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class PaginatorTest : IDisposable
    {
        public PaginatorTest()
        {
            PromptPlus.ExclusiveDriveConsole(new MemoryConsoleDriver(20, 80));
        }

        public void Dispose()
        {
            PromptPlus.ExclusiveMode = false;
            PromptPlus.MinBufferHeight = PromptPlus.DefaultMinBufferHeight;
        }


        [Fact]
        public void Should_have_exception_when_BufferHeight_less_than_MinBufferHeight()
        {
            // Given
            PromptPlus.MinBufferHeight = 21;
            //when
            var ex = Record.Exception(() =>
            {
                var pg = new Paginator<string>(new string[] { "" }, null, Optional<string>.Create(null));
            });
            //then
            Assert.NotNull(ex);
        }

        [Fact]
        public void Should_have_Ajust_terminal_Height_to_5_pages()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 50; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>(items, null, Optional<string>.Create(null));
            // Given
            Assert.Equal(5, pg.PageCount);
        }

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
            var pg = new Paginator<string>(items, 10, Optional<string>.Create(null));
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
            var pg = new Paginator<string>(items, 3, Optional<string>.Create(null));
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
            var pg = new Paginator<string>(items, 3, Optional<string>.Create(null));
            pg.ToSubset();
            // Then
            Assert.Equal(0, pg.SelectedPage);
            Assert.Equal(3, pg.Count);
        }

        [Fact]
        public void Should_have_seletedItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            // When
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("3"));
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
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("3"));
            // When
            pg.UnSelected();
            // Then
            Assert.Equal(-1, pg.SelectedIndex);
            Assert.Null(pg.SelectedItem);
            Assert.True(pg.IsUnSelected);
        }



        [Fact]
        public void Should_have_seletedItem_with_FistItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("3"));
            // When
            pg.FirstItem();
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal("0", pg.SelectedItem);
        }

        [Fact]
        public void Should_have_seletedItem_with_LastItem()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("0"));
            // When
            pg.LastItem();
            // Then
            Assert.Equal(4, pg.SelectedIndex);
            Assert.Equal("4", pg.SelectedItem);
        }

        [Fact]
        public void Should_have_seletedItem_with_NextPage_unselect()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null));
            // When
            pg.NextPage(IndexOption.None);
            // Then
            Assert.Equal(-1, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.FirstItem)]
        [InlineData(IndexOption.FirstItemWhenHasPages)]
        internal void Should_have_seletedItem_with_NextPage_FistSelect(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null));
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.LastItem)]
        [InlineData(IndexOption.LastItemWhenHasPages)]
        internal void Should_have_seletedItem_with_NextPage_LastSelect(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null));
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(4, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Fact]
        public void Should_have_seletedItem_with_FistItem_and_validator_Select()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("3"), null, (x) => x != "0");
            // When
            pg.FirstItem();
            // Then
            Assert.Equal(1, pg.SelectedIndex);
            Assert.Equal("1", pg.SelectedItem);
        }
        [Fact]
        public void Should_have_seletedItem_with_LastItem_and_validator_Select()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 10, Optional<string>.Create("0"), null, (x) => x != "4");
            // When
            pg.LastItem();
            // Then
            Assert.Equal(3, pg.SelectedIndex);
            Assert.Equal("3", pg.SelectedItem);
        }

        [Theory]
        [InlineData(IndexOption.FirstItem)]
        [InlineData(IndexOption.FirstItemWhenHasPages)]
        internal void Should_have_seletedItem_with_NextPage_FistSelect_and_validator_Select(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create("0"), null, (x) => x != "5");
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(1, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Theory]
        [InlineData(IndexOption.LastItem)]
        [InlineData(IndexOption.LastItemWhenHasPages)]
        internal void Should_have_seletedItem_with_NextPage_LastSelect_and_validator_Selec(IndexOption opc)
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null), null, (x) => x != "9");
            // When
            pg.NextPage(opc);
            // Then
            Assert.Equal(3, pg.SelectedIndex);
            Assert.Equal(1, pg.SelectedPage);
        }

        [Fact]
        internal void Should_have_seletedItem_with_UpdateFilter()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null));
            // When
            pg.UpdateFilter("10");
            // Then
            Assert.Equal(0, pg.SelectedIndex);
            Assert.Equal(0, pg.SelectedPage);
            Assert.Equal(1, pg.PageCount);
            Assert.Equal(1, pg.Count);
        }

        [Fact]
        internal void Should_have_seletedItem_with_UpdateFilter_and_tryget()
        {
            // Given
            var items = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                items.Add(i.ToString());
            }
            var pg = new Paginator<string>(items, 5, Optional<string>.Create(null));
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
