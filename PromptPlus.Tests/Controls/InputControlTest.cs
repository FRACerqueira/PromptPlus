using System;

using PPlus.Drivers;

using Xunit;

namespace PPlus.Tests.Controls
{
    public class InputControlTest : IDisposable
    {

        private readonly MemoryConsoleReader _reader;
        private readonly MemoryConsoleDriver _memoryconsole;

        public InputControlTest()
        {
            PromptPlus.ExclusiveDriveConsole(new MemoryConsoleDriver());
            _reader = (MemoryConsoleReader)PromptPlus.PPlusConsole.In;
            _memoryconsole = (MemoryConsoleDriver)PromptPlus.PPlusConsole;
        }

        public void Dispose()
        {
            _memoryconsole.Dispose();
            _reader.Dispose();
            PromptPlus.ExclusiveMode = false;
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
            var result = PromptPlus.Input("Prompt teste", "teste description")
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
            Assert.Contains("Prompt teste", viewstart[0]);
            Assert.Equal("teste description", viewstart[1]);
            Assert.False(result.IsAborted);
            Assert.False(result.IsAllAborted);
            Assert.Equal("default", finalvalue);
            Assert.Equal("teste", initialvalue);
        }


        [Fact]
        internal void Should_have_accept_ctrl_del_with_enter()
        {
            var first = true;
            string[] viewstart = null;
            string[] viewend = null;
            var initialvalue = string.Empty;
            var finalvalue = string.Empty;

            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Delete, false, false, true));
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, false));
            var result = PromptPlus.Input("Prompt teste", "teste description")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnStartControl, (object value) =>
                    {
                        initialvalue = value.ToString();
                    });
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender, (object value) =>
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
            Assert.Equal("", result.Value);
            Assert.Contains("Prompt teste", viewstart[0]);
            Assert.Equal("teste description", viewstart[1]);
            Assert.False(result.IsAborted);
            Assert.False(result.IsAllAborted);
            Assert.Equal("", finalvalue);
            Assert.Equal("teste", initialvalue);
        }

        [Fact]
        internal void Should_have_show_tooltips()
        {
            var first = true;
            string[] viewstart = null;
            string[] viewAfterF1 = null;

            _reader.WaitRender(); //PPLUS custom code for KeyAvailable = false => ConsoleKeyInfo((char)0, 0, true, true, true)
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.F1, false, false, false));
            _reader.WaitRender(); //PPLUS custom code for KeyAvailable = false => ConsoleKeyInfo((char)0, 0, true, true, true)
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, false));
            var result = PromptPlus.Input("Prompt teste", "teste description")
                .Default("default")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender, (object value) =>
                    {
                        if (first)
                        {
                            first = false;
                            viewstart = _memoryconsole.GetScreen();
                        }
                        else
                        {
                            viewAfterF1 = _memoryconsole.GetScreen();
                        }

                    });
                })
                .Run();
            Assert.Equal("teste description", viewstart[1]);
            Assert.Equal(4, viewstart.Length);
            Assert.Contains("F1", viewstart[2]);
            Assert.Contains("F3", viewstart[2]);
            Assert.Contains("Esc", viewstart[2]);
            Assert.Contains("Enter", viewstart[3]);
            Assert.Contains("Ctrl+Del", viewstart[3]);
            Assert.Equal(2, viewAfterF1.Length);
            Assert.Contains("Prompt teste", viewAfterF1[0]);
            Assert.Equal("teste description", viewstart[1]);
        }

        [Fact]
        internal void Should_have_hide_description()
        {
            var first = true;
            string[] viewstart = null;
            string[] viewAfterF3 = null;

            _reader.WaitRender(); //PPLUS custom code for KeyAvailable = false => ConsoleKeyInfo((char)0, 0, true, true, true)
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.F3, false, false, false));
            _reader.WaitRender(); //PPLUS custom code for KeyAvailable = false => ConsoleKeyInfo((char)0, 0, true, true, true)
            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Enter, false, false, false));
            var result = PromptPlus.Input("Prompt teste", "teste description")
                .Default("default")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender, (object value) =>
                    {
                        if (first)
                        {
                            first = false;
                            viewstart = _memoryconsole.GetScreen();
                        }
                        else
                        {
                            viewAfterF3 = _memoryconsole.GetScreen();
                        }

                    });
                })
                .Run();
            Assert.Equal("teste description", viewstart[1]);
            Assert.Equal(4, viewstart.Length);
            Assert.Contains("F1", viewstart[2]);
            Assert.Contains("F3", viewstart[2]);
            Assert.Contains("Esc", viewstart[2]);
            Assert.Contains("Enter", viewstart[3]);
            Assert.Contains("Ctrl+Del", viewstart[3]);
            Assert.Equal(3, viewAfterF3.Length);
            Assert.Contains("Prompt teste", viewAfterF3[0]);
            Assert.Contains("F1", viewAfterF3[1]);
            Assert.Contains("F3", viewAfterF3[1]);
            Assert.Contains("Esc", viewAfterF3[1]);
            Assert.Contains("Enter", viewAfterF3[2]);
            Assert.Contains("Ctrl+Del", viewAfterF3[2]);
        }

        [Fact]
        internal void Should_have_accept_esc()
        {
            var first = true;
            string[] viewstart = null;
            string[] viewend = null;

            _reader.LoadInput(new ConsoleKeyInfo((char)0, ConsoleKey.Escape, false, false, false));
            var result = PromptPlus.Input("Prompt teste", "teste description")
                .InitialValue("teste")
                .Config((ctx) =>
                {
                    ctx.AddExtraAction(PPlus.Objects.StageControl.OnInputRender, (object value) =>
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
                    });
                })
                .Run();
            Assert.Null(result.Value);
            Assert.True(result.IsAborted);
            Assert.False(result.IsAllAborted);
        }
    }
}
