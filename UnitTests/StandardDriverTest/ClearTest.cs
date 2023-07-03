using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdClearTest : BaseTest
    {
        [Fact]
        public void Should_Position_Default()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            int cursorleft = 0;
            int cursortop = 0;
            int afterclearcursorleft = 0;
            int afterclearcursortop = 0;

            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.Write("TEST\nNEWLINE");
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
                PromptPlus.Console.Clear();
                afterclearcursorleft = PromptPlus.Console.CursorLeft;
                afterclearcursortop = PromptPlus.Console.CursorTop;

            });
            // Then
            Assert.Equal(7, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal(0, afterclearcursorleft);
            Assert.Equal(0, afterclearcursortop);
        }

        [Fact]
        public void Should_Position_padleft()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
                cfg.PadLeft = 2;
            });
            // When
            int cursorleft = 0;
            int cursortop = 0;
            int afterclearcursorleft = 0;
            int afterclearcursortop = 0;
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.Write("TEST\nNEWLINE");
                cursorleft = PromptPlus.Console.CursorLeft;
                cursortop = PromptPlus.Console.CursorTop;
                PromptPlus.Console.Clear();
                afterclearcursorleft = PromptPlus.Console.CursorLeft;
                afterclearcursortop = PromptPlus.Console.CursorTop;
            });
            // Then
            Assert.Equal(9, cursorleft);
            Assert.Equal(1, cursortop);
            Assert.Equal(2, afterclearcursorleft);
            Assert.Equal(0, afterclearcursortop);

        }
    }
}
