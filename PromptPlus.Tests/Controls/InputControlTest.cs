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
            var initialvalue = string.Empty;
            var finalvalue = string.Empty;

            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.L, false, false, true));
            _reader.WaitRender(); //PPLUS custom code for KeyAvailable = false => ConsoleKeyInfo((char)0, 0, true, true, true)
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, false));
            var result = PromptPlus.Input("teste", "teste description")
                .Default("default")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnStartControl, (object value) =>
                    {
                        initialvalue = value.ToString();
                    });
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender,(object value) =>
                    {
                        if (first)
                        {
                            first = false;
                            viewstart = _memoryconsole.GetScreen();
                        }
                    });
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnFinishControl, (object value) =>
                    {
                        viewend = _memoryconsole.GetScreen();
                        finalvalue = value.ToString();
                    });
                })
                .Run();
            Assert.Equal("default", result.Value);
            Assert.False(result.IsAborted);
            Assert.False(result.IsAllAborted);
            Assert.Equal("default", finalvalue);
            Assert.Equal("teste", initialvalue);
        }
    }
}
