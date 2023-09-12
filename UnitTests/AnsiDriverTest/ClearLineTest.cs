// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class ClearLineTest : BaseTest
    {
        [Fact]
        public void Should_ClearLine()
        {
            int inicursorleft = PromptPlus.CursorLeft;
            int inicursortop = PromptPlus.CursorTop;
            int cursorleft = -1;
            int cursortop = -1;

            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.ClearLine();
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
            });
            // Then
            Assert.Equal(PromptPlus.PadLeft, cursorleft);
            Assert.Equal(inicursortop, cursortop);
            Assert.Equal("\u001b[0K", output);
        }

        [Fact]
        public void Should_ClearLine_byRow()
        {
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.ClearLine(1);
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
            });
            // Then
            Assert.Equal(PromptPlus.PadLeft, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal("\u001b[0K", output);
        }

        [Fact]
        public void ClearRestOfLine()
        {
            PromptPlus.Write("test");
            int inicursorleft = PromptPlus.CursorLeft;
            int inicursortop = PromptPlus.CursorTop;
            int cursorleft = -1;
            int cursortop = -1;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.ClearRestOfLine();
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
            });
            // Then
            Assert.Equal(inicursorleft, cursorleft);
            Assert.Equal(inicursortop, cursortop);
            Assert.Equal("\u001b[0K", output);
        }

        [Fact]
        public void ClearRestOfLine_withpads()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
                cfg.PadRight = 2;
            });
            // When
            PromptPlus.Write("test");
            int cursorleft = 0;
            int cursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.ClearRestOfLine();
                cursorleft = PromptPlus.CursorLeft;
                cursortop = PromptPlus.CursorTop;
            });
            // Then
            Assert.Equal(6, cursorleft);
            Assert.Equal(0, cursortop);
            Assert.Equal("\u001b[0K", output);
        }
    }
}
