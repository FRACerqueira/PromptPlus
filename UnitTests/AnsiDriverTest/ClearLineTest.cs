using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class ClearLineTest : BaseTest
    {
        [Fact]
        public void Should_ClearLine()
        {
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
            Assert.Equal("\u001b[48;5;0m\u001b[0K", output);
        }

        [Fact]
        public void Should_ClearLine_byRow()
        {
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
            Assert.Equal("\u001b[48;5;0m\u001b[0K", output);
        }

        [Fact]
        public void Should_ClearLine_withStyle()
        {
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.ClearLine(null, Style.Plain.Foreground(Color.Blue).Background(Color.Red));
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(PromptPlus.Console.PadLeft, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal("\u001b[38;5;12;48;5;9m\u001b[38;5;7;48;5;0m\u001b[48;5;0m\u001b[0K", output);
        }


        [Fact]
        public void ClearRestOfLine_withstyle()
        {
            int cursorleft = 0;
            int cursortop = 0;
            string output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.Write("test");
                PromptPlus.Console.ClearRestOfLine(Style.Plain.Foreground(Color.Blue).Background(Color.Red));
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(4, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal("test\u001b[38;5;12;48;5;9m\u001b[38;5;7;48;5;0m\u001b[48;5;0m\u001b[0K", output);
        }


        [Fact]
        public void ClearRestOfLine()
        {
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
            Assert.Equal("\u001b[48;5;0m\u001b[0K", output);
        }

        [Fact]
        public void ClearRestOfLine_withpads()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
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
            Assert.Equal("\u001b[48;5;0m\u001b[0K", output);
        }
    }
}
