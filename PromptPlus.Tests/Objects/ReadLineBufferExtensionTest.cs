using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PPlus.Objects;

using Xunit;

namespace PPlus.Tests.Objects
{
    public class ReadLineBufferExtensionTest
    {
        [Theory]
        [InlineData(ConsoleKey.F1,ConsoleKey.F)]
        public void Should_IsPressSpecialKey(ConsoleKey consoleKey, ConsoleKey other)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, true, true, true);
            // When
            Assert.True(k.IsPressSpecialKey(consoleKey, ConsoleModifiers.Control | ConsoleModifiers.Shift | ConsoleModifiers.Alt));
            Assert.False(k.IsPressSpecialKey(other, ConsoleModifiers.Control | ConsoleModifiers.Shift | ConsoleModifiers.Alt));
        }

        [Theory]
        [InlineData(true, ConsoleKey.LeftArrow, false, false, false)]
        [InlineData(true, ConsoleKey.B, false, false, true)]
        [InlineData(false, ConsoleKey.LeftArrow, true, false, false)]
        [InlineData(false, ConsoleKey.LeftArrow, false, true, false)]
        [InlineData(false, ConsoleKey.LeftArrow, false, false, true)]
        [InlineData(false, ConsoleKey.B, true, false, false)]
        [InlineData(false, ConsoleKey.B, false, true, false)]
        [InlineData(false, ConsoleKey.B, false, false, false)]
        public void Should_IsPressLeftArrowKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressLeftArrowKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.RightArrow, false, false, false)]
        [InlineData(true, ConsoleKey.F, false, false, true)]
        [InlineData(false, ConsoleKey.RightArrow, true, false, false)]
        [InlineData(false, ConsoleKey.RightArrow, false, true, false)]
        [InlineData(false, ConsoleKey.RightArrow, false, false, true)]
        [InlineData(false, ConsoleKey.F, true, false, false)]
        [InlineData(false, ConsoleKey.F, false, true, false)]
        [InlineData(false, ConsoleKey.F, false, false, false)]
        public void Should_IsPressRightArrowKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressRightArrowKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.C, false, false, true)]
        [InlineData(false, ConsoleKey.C, false, false, false)]
        [InlineData(false, ConsoleKey.C, true, false, false)]
        [InlineData(false, ConsoleKey.C, false, true, false)]
        public void Should_IsPressCtrlCKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressCtrlCKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.J, false, false, true)]
        [InlineData(true, ConsoleKey.Enter, false, false, false)]
        [InlineData(false, ConsoleKey.Enter, true, false, false)]
        [InlineData(false, ConsoleKey.Enter, false, true, false)]
        [InlineData(false, ConsoleKey.Enter, false, false, true)]
        [InlineData(false, ConsoleKey.J, true, false, false)]
        [InlineData(false, ConsoleKey.J, false, true, false)]
        [InlineData(false, ConsoleKey.J, false, false, false)]
        public void Should_IsPressEnterKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressEnterKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.H, false, false, true)]
        [InlineData(true, ConsoleKey.Backspace, false, false, false)]
        [InlineData(false, ConsoleKey.Backspace, true, false, false)]
        [InlineData(false, ConsoleKey.Backspace, false, true, false)]
        [InlineData(false, ConsoleKey.Backspace, false, false, true)]
        [InlineData(false, ConsoleKey.H, true, false, false)]
        [InlineData(false, ConsoleKey.H, false, true, false)]
        [InlineData(false, ConsoleKey.H, false, false, false)]
        public void Should_IsPressBackspaceKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressBackspaceKey() == result);
        }


        [Theory]
        [InlineData(true, ConsoleKey.D, false, false, true)]
        [InlineData(true, ConsoleKey.Delete, false, false, false)]
        [InlineData(false, ConsoleKey.Delete, true, false, false)]
        [InlineData(false, ConsoleKey.Delete, false, true, false)]
        [InlineData(false, ConsoleKey.Delete, false, false, true)]
        [InlineData(false, ConsoleKey.D, true, false, false)]
        [InlineData(false, ConsoleKey.D, false, true, false)]
        [InlineData(false, ConsoleKey.D, false, false, false)]
        public void Should_IsPressDeleteKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressDeleteKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.N, false, false, true)]
        [InlineData(true, ConsoleKey.DownArrow, false, false, false)]
        [InlineData(false, ConsoleKey.DownArrow, true, false, false)]
        [InlineData(false, ConsoleKey.DownArrow, false, true, false)]
        [InlineData(false, ConsoleKey.DownArrow, false, false, true)]
        [InlineData(false, ConsoleKey.N, true, false, false)]
        [InlineData(false, ConsoleKey.N, false, true, false)]
        [InlineData(false, ConsoleKey.N, false, false, false)]
        public void Should_IsPressDownArrowKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressDownArrowKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.P, false, false, true)]
        [InlineData(true, ConsoleKey.UpArrow, false, false, false)]
        [InlineData(false, ConsoleKey.UpArrow, true, false, false)]
        [InlineData(false, ConsoleKey.UpArrow, false, true, false)]
        [InlineData(false, ConsoleKey.UpArrow, false, false, true)]
        [InlineData(false, ConsoleKey.P, true, false, false)]
        [InlineData(false, ConsoleKey.P, false, true, false)]
        [InlineData(false, ConsoleKey.P, false, false, false)]
        public void Should_IsPressUpArrowKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressUpArrowKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.E, false, false, true)]
        [InlineData(true, ConsoleKey.End, false, false, false)]
        [InlineData(false, ConsoleKey.End, true, false, false)]
        [InlineData(false, ConsoleKey.End, false, true, false)]
        [InlineData(false, ConsoleKey.End, false, false, true)]
        [InlineData(false, ConsoleKey.E, true, false, false)]
        [InlineData(false, ConsoleKey.E, false, true, false)]
        [InlineData(false, ConsoleKey.E, false, false, false)]
        public void Should_IsPressEndKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressEndKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.Tab, false, false, false)]
        [InlineData(false, ConsoleKey.Tab, true, false, false)]
        [InlineData(false, ConsoleKey.Tab, false, true, false)]
        [InlineData(false, ConsoleKey.Tab, false, false, true)]
        public void Should_IsPressTabKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressTabKey() == result);
        }


        [Theory]
        [InlineData(true, ConsoleKey.Escape, false, false, false)]
        [InlineData(false, ConsoleKey.Escape, true, false, false)]
        [InlineData(false, ConsoleKey.Escape, false, true, false)]
        [InlineData(false, ConsoleKey.Escape, false, false, true)]
        public void Should_IsPressEscKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressEscKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.A, false, false, true)]
        [InlineData(true, ConsoleKey.Home, false, false, false)]
        [InlineData(false, ConsoleKey.Home, true, false, false)]
        [InlineData(false, ConsoleKey.Home, false, true, false)]
        [InlineData(false, ConsoleKey.Home, false, false, true)]
        [InlineData(false, ConsoleKey.A, true, false, false)]
        [InlineData(false, ConsoleKey.A, false, true, false)]
        [InlineData(false, ConsoleKey.A, false, false, false)]
        public void Should_IsPressHomeKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressHomeKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.N, false, true, false)]
        [InlineData(true, ConsoleKey.PageDown, false, false, false)]
        [InlineData(false, ConsoleKey.PageDown, true, false, false)]
        [InlineData(false, ConsoleKey.PageDown, false, true, false)]
        [InlineData(false, ConsoleKey.PageDown, false, false, true)]
        [InlineData(false, ConsoleKey.N, true, false, false)]
        [InlineData(false, ConsoleKey.N, false, false, true)]
        [InlineData(false, ConsoleKey.N, false, false, false)]
        public void Should_IsPressPageDownKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressPageDownKey() == result);
        }

        [Theory]
        [InlineData(true, ConsoleKey.P, false, true, false)]
        [InlineData(true, ConsoleKey.PageUp, false, false, false)]
        [InlineData(false, ConsoleKey.PageUp, true, false, false)]
        [InlineData(false, ConsoleKey.PageUp, false, true, false)]
        [InlineData(false, ConsoleKey.PageUp, false, false, true)]
        [InlineData(false, ConsoleKey.P, true, false, false)]
        [InlineData(false, ConsoleKey.P, false, false, true)]
        [InlineData(false, ConsoleKey.P, false, false, false)]
        public void Should_IsPressPageUpKey(bool result, ConsoleKey consoleKey, bool shift, bool alt, bool ctrl)
        {
            // Given
            var k = new ConsoleKeyInfo((char)consoleKey, consoleKey, shift, alt, ctrl);
            // When
            Assert.True(k.IsPressPageUpKey() == result);
        }

    }
}
