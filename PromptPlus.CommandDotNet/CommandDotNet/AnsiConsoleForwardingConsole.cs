using System;
#if NET5_0_OR_GREATER
using System.IO;
#endif
using System.Threading;
#if NETSTANDARD2_1
using CommandDotNet;
#endif
using CommandDotNet.Rendering;

using PPlus.Drivers;

namespace PPlus.CommandDotNet
{
#if NETSTANDARD2_1
    public class AnsiConsoleForwardingConsole : IConsole
    {
        private readonly IConsoleDriver _ansiConsole;

        public AnsiConsoleForwardingConsole(IConsoleDriver ansiConsole)
        {
            _ansiConsole = ansiConsole;
            Out = StandardStreamWriter.Create(Console.Out);

            Error = StandardStreamWriter.Create(Console.Error);
            // In is used to read piped input, which does not appear to be handled by Spectre.
            // ReadKey is implemented further down and delegates to IAnsiConsole.
            In = StandardStreamReader.Create(Console.In);
        }

#region Implementation of IStandardOut

        public IStandardStreamWriter Out { get; }
        public bool IsOutputRedirected => Console.IsOutputRedirected;

#endregion

#region Implementation of IStandardError

        public IStandardStreamWriter Error { get; }
        public bool IsErrorRedirected => Console.IsErrorRedirected;

#endregion

#region Implementation of IStandardIn

        public IStandardStreamReader In { get; }

        public bool IsInputRedirected => Console.IsInputRedirected;

#endregion

#region Implementation of IConsole

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return _ansiConsole.WaitKeypress(intercept, CancellationToken.None);
        }

        public bool TreatControlCAsInput
        {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

#endregion
    }
#endif
#if NET5_0_OR_GREATER
    public class AnsiConsoleForwardingConsole : SystemConsole
    {
        private readonly IConsoleDriver _ansiConsole;

        public AnsiConsoleForwardingConsole(IConsoleDriver ansiConsole)
        {
            _ansiConsole = ansiConsole;
            Out = new ForwardingTextWriter(ansiConsole.WriteAnsiConsole!);
        }

        public override TextWriter Out { get; }

        public override ConsoleKeyInfo? ReadKey(bool intercept = false)
        {
            return _ansiConsole.WaitKeypress(intercept, CancellationToken.None);
        }
    }
#endif
}
