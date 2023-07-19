using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdClearLineTest : BaseTest
    {
        [Fact]
        public void Should_ClearLine()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.ClearLine();
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(PromptPlus.Console.PadLeft, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal(132,output.Length);
        }

        [Fact]
        public void Should_ClearLine_byRow()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.ClearLine(1);
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(PromptPlus.Console.PadLeft, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal(132, output.Length);
        }

        [Fact]
        public void ClearRestOfLine()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.Console.Write("test");
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.ClearRestOfLine();
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(4, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal(128, output.Length);
        }

        [Fact]
        public void ClearRestOfLine_withpads()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
                cfg.PadLeft = 2;
                cfg.PadRight = 2;
            });
            // When
            PromptPlus.Console.Write("test");
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.ClearRestOfLine();
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(6, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal(122, output.Length);
        }
    }
}
