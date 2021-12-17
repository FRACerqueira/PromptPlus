using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PPlus.Drivers;

using Xunit;

namespace PPlus.Tests.Controls
{
    public class InputControlTest : IDisposable
    {

        private MemoryConsoleReader _reader;
        private MemoryConsoleDriver _memoryconsole;

        public InputControlTest()
        {
            PromptPlus.ExclusiveDriveConsole(new MemoryConsoleDriver());
            _reader = (MemoryConsoleReader)PromptPlus.PPlusConsole.In;
            _memoryconsole = (MemoryConsoleDriver)PromptPlus.PPlusConsole;
        }

        public void Dispose()
        {
            PromptPlus.ExclusiveMode = false;
            _reader.Dispose();
        }

        [Fact]
        internal void Should_have_accept_defaultvalue_with_enter()
        {
            var first = true;
            string[] viewstart = null;
            string[] viewend = null;

            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.L, false, false, true));
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, false));
            var result = PromptPlus.Input("teste", "teste description")
                .Default("default")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender,(_) =>
                    {
                        if (first)
                        {
                            first = false;
                            viewstart = _memoryconsole.GetScreen();
                        }
                    });
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnFinishRender, (_) =>
                    {
                        viewend = _memoryconsole.GetScreen();
                    });
                })
                .Run();
            Assert.Equal("default", result.Value);
        }
    }
}
