using FluentAssertions;
using PromptPlus.Tests.Controls;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // FileHistory (Controls/History/FileHistory.cs) — camada 1 unidade pura (AddHistory) + camada de
    // I/O testável via System.IO.Abstractions (added specifically for this suite, per user request,
    // so Load/Save/Clear never touch the real user profile folder). Each test swaps in a fresh
    // MockFileSystem and restores the real FileSystem in `finally`, mirroring the save/restore
    // pattern already used for HelperTests' global static state — MockFileSystem is a pure in-memory
    // implementation, so this runs identically on Windows and Linux.
    //
    // [Collection(SerializedGlobalStateCollection.Name)]: FileHistory.FileSystem is a static field shared by
    // the whole assembly. xUnit runs different test CLASSES in parallel by default (methods within
    // one class run sequentially already) — without this, this class racing against
    // Controls.InputControlHistoryModeTests (which swaps the same static) intermittently overwrote
    // each other's FileSystem mid-test. Found empirically: FileHistoryTests started failing only
    // after InputControlHistoryModeTests was added, never in isolation.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class FileHistoryTests : IDisposable
    {
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public FileHistoryTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        // ---- AddHistory: pure, no filesystem involved ----

        [Fact]
        public void AddHistory_inserts_the_new_value_at_the_front()
        {
            IList<ItemHistory> items = [];

            items = FileHistory.AddHistory("first", TimeSpan.FromDays(1), items);
            items = FileHistory.AddHistory("second", TimeSpan.FromDays(1), items);

            _ = items[0].History.Should().Be("second");
            _ = items[1].History.Should().Be("first");
        }

        [Fact]
        public void AddHistory_deduplicates_case_insensitively_and_moves_the_match_to_the_front()
        {
            IList<ItemHistory> items = [];
            items = FileHistory.AddHistory("Hello", TimeSpan.FromDays(1), items);
            items = FileHistory.AddHistory("World", TimeSpan.FromDays(1), items);

            items = FileHistory.AddHistory("HELLO", TimeSpan.FromDays(1), items);

            _ = items.Should().HaveCount(2);
            _ = items[0].History.Should().Be("HELLO");
            _ = items[1].History.Should().Be("World");
        }

        [Fact]
        public void AddHistory_with_an_empty_or_whitespace_value_is_a_no_op()
        {
            IList<ItemHistory> items = [];
            items = FileHistory.AddHistory("kept", TimeSpan.FromDays(1), items);

            IList<ItemHistory> result = FileHistory.AddHistory("   ", TimeSpan.FromDays(1), items);

            _ = result.Should().HaveCount(1);
            _ = result[0].History.Should().Be("kept");
        }

        [Fact]
        public void AddHistory_with_null_items_starts_a_new_list()
        {
            IList<ItemHistory> result = FileHistory.AddHistory("first", TimeSpan.FromDays(1), null);

            _ = result.Should().HaveCount(1);
            _ = result[0].History.Should().Be("first");
        }

        // ---- Save/Load/Clear: exercised against the MockFileSystem ----

        [Fact]
        public void SaveHistory_then_LoadHistory_round_trips_unexpired_items()
        {
            IList<ItemHistory> items = [];
            items = FileHistory.AddHistory("alpha", TimeSpan.FromDays(1), items);
            items = FileHistory.AddHistory("beta", TimeSpan.FromDays(1), items);

            FileHistory.SaveHistory("test-history", items);
            IList<ItemHistory> loaded = FileHistory.LoadHistory("test-history");

            _ = loaded.Should().HaveCount(2);
            _ = loaded.Should().Contain(x => x.History == "alpha");
            _ = loaded.Should().Contain(x => x.History == "beta");
        }

        [Fact]
        public void LoadHistory_excludes_expired_entries()
        {
            IList<ItemHistory> items = [
                new ItemHistory("expired", DateTime.Now.AddDays(-1).Ticks),
                ItemHistory.CreateItemHistory("valid", TimeSpan.FromDays(1)),
            ];
            FileHistory.SaveHistory("test-expiry", items);

            IList<ItemHistory> loaded = FileHistory.LoadHistory("test-expiry");

            _ = loaded.Should().ContainSingle();
            _ = loaded[0].History.Should().Be("valid");
        }

        [Fact]
        public void LoadHistory_orders_by_most_recent_timeout_first_and_respects_maxitem()
        {
            IList<ItemHistory> items = [
                ItemHistory.CreateItemHistory("soonest", TimeSpan.FromHours(1)),
                ItemHistory.CreateItemHistory("latest", TimeSpan.FromDays(10)),
                ItemHistory.CreateItemHistory("middle", TimeSpan.FromDays(5)),
            ];
            FileHistory.SaveHistory("test-order", items);

            IList<ItemHistory> loaded = FileHistory.LoadHistory("test-order", maxitem: 2);

            _ = loaded.Should().HaveCount(2);
            _ = loaded[0].History.Should().Be("latest");
            _ = loaded[1].History.Should().Be("middle");
        }

        [Fact]
        public void LoadHistory_for_a_file_that_does_not_exist_returns_an_empty_list()
        {
            IList<ItemHistory> loaded = FileHistory.LoadHistory("never-saved");
            _ = loaded.Should().BeEmpty();
        }

        [Fact]
        public void SaveHistory_with_an_empty_list_deletes_an_existing_file()
        {
            IList<ItemHistory> items = [];
            items = FileHistory.AddHistory("temp", TimeSpan.FromDays(1), items);
            FileHistory.SaveHistory("test-delete", items);
            _ = FileHistory.LoadHistory("test-delete").Should().ContainSingle();

            FileHistory.SaveHistory("test-delete", []);

            _ = FileHistory.LoadHistory("test-delete").Should().BeEmpty();
        }

        [Fact]
        public void SaveHistory_trims_items_beyond_maxitem_before_writing()
        {
            IList<ItemHistory> items = [
                ItemHistory.CreateItemHistory("one", TimeSpan.FromDays(1)),
                ItemHistory.CreateItemHistory("two", TimeSpan.FromDays(1)),
                ItemHistory.CreateItemHistory("three", TimeSpan.FromDays(1)),
            ];

            FileHistory.SaveHistory("test-trim", items, maxitem: 2);

            _ = FileHistory.LoadHistory("test-trim").Should().HaveCount(2);
        }

        [Fact]
        public void ClearHistory_removes_the_saved_file()
        {
            IList<ItemHistory> items = [];
            items = FileHistory.AddHistory("to-clear", TimeSpan.FromDays(1), items);
            FileHistory.SaveHistory("test-clear", items);

            FileHistory.ClearHistory("test-clear");

            _ = FileHistory.LoadHistory("test-clear").Should().BeEmpty();
        }

        [Fact]
        public void ClearHistory_for_a_file_that_does_not_exist_does_not_throw()
        {
            Action act = () => FileHistory.ClearHistory("never-existed");
            _ = act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LoadHistory_SaveHistory_and_ClearHistory_reject_null_or_blank_filenames(string? filename)
        {
            _ = ((Action)(() => FileHistory.LoadHistory(filename!))).Should().Throw<ArgumentException>();
            _ = ((Action)(() => FileHistory.SaveHistory(filename!, []))).Should().Throw<ArgumentException>();
            _ = ((Action)(() => FileHistory.ClearHistory(filename!))).Should().Throw<ArgumentException>();
        }
    }
}
