using System;

using PPlus.Drivers;
using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Objects
{
    public class ColorTokenTest : IDisposable
    {
        public ColorTokenTest()
        {
            PromptPlus.ExclusiveDriveConsole(new MemoryConsoleDriver());
        }
        public void Dispose()
        {
            PromptPlus.ExclusiveMode = false;
        }

        [Fact]
        public void Should_have_ansi_value_color()
        {
            // Given
            var ct = new ColorToken();
            // When
            //none
            // Then
            Assert.NotNull(ct.AnsiColor);
            Assert.StartsWith("\x1b", ct.AnsiColor);
            Assert.EndsWith("m", ct.AnsiColor);
            Assert.Contains("[", ct.AnsiColor);
            Assert.Contains(";", ct.AnsiColor);
        }

        [Fact]
        public void Should_have_mask_value()
        {
            // Given

            var ct = new ColorToken("", PromptPlus.ForegroundColor, PromptPlus.BackgroundColor);
            // When
            var maskct = ct.Mask(ConsoleColor.Red, ConsoleColor.Red);
            // Then
            Assert.True(maskct.Color == ConsoleColor.Red);
            Assert.True(maskct.BackgroundColor == ConsoleColor.Red);
        }

        [Fact]
        public void Should_have_same_forecolor_mask_value()
        {
            // Given

            var ct = new ColorToken("", PromptPlus.ForegroundColor, PromptPlus.BackgroundColor);
            // When
            var maskct = ct.Mask(null, ConsoleColor.Red);
            // Then
            Assert.True(maskct.Color == PromptPlus.ForegroundColor);
            Assert.True(maskct.BackgroundColor == ConsoleColor.Red);
        }

        [Fact]
        public void Should_have_same_backcolor_mask_value()
        {
            // Given

            var ct = new ColorToken("", PromptPlus.ForegroundColor, PromptPlus.BackgroundColor);
            // When
            var maskct = ct.Mask(ConsoleColor.Red, null);
            // Then
            Assert.True(maskct.Color == ConsoleColor.Red);
            Assert.True(maskct.BackgroundColor == PromptPlus.BackgroundColor);
        }

        [Fact]
        public void Should_have_same_mask_value()
        {
            // Given

            var ct = new ColorToken("", ConsoleColor.Red, ConsoleColor.Red);
            // When
            var maskct = ct.Mask(PromptPlus.ForegroundColor, PromptPlus.BackgroundColor);
            // Then
            Assert.True(maskct.Color == ConsoleColor.Red);
            Assert.True(maskct.BackgroundColor == ConsoleColor.Red);
        }

    }
}
