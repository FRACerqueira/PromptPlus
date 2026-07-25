using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Grupo 1 (FASE2-CONTROLS-PLAN.md) — KeyPressControl, sem ModeView, sem History/filesystem.
    // Confirmado por leitura: cancelamento JÁ seta ResultCtrl corretamente (TryResult.cs:103-107),
    // diferente do bug #8 do InputControl — não é um gap a testar, é um "já está certo".
    public class KeyPressControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IKeyPressControl MakeKeyPress(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).KeyPress("Press");

        [Fact]
        public void Any_key_confirms_when_no_valid_keys_are_registered()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.X);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content!.Value.Key.Should().Be(ConsoleKey.X);
        }

        [Fact]
        public void Escape_aborts_and_returns_the_escape_key_itself()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content!.Value.Key.Should().Be(ConsoleKey.Escape);
        }

        [Fact]
        public void Cancellation_with_no_key_returns_a_null_aborted_result()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void AddValidKey_accepts_a_registered_key_with_its_display_text()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt).AddValidKey(ConsoleKey.Y, displayText: "Yes");
            _ = vt.Keys.Enqueue(ConsoleKey.Y);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content!.Value.Key.Should().Be(ConsoleKey.Y);
            _ = vt.Find("Yes").Should().NotBeNull();
        }

        [Fact]
        public void AddValidKey_requires_the_exact_registered_modifiers()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt).AddValidKey(ConsoleKey.Y, ConsoleModifiers.Control, "Confirm");
            // Plain Y (no Ctrl) does not match the Ctrl+Y registration, so it's an invalid key with no
            // message configured — silently ignored, waiting for the next key; Escape ends the run.
            _ = vt.Keys.Enqueue(ConsoleKey.Y).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
        }

        [Fact]
        public void Invalid_key_without_a_message_is_silently_ignored()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt).AddValidKey(ConsoleKey.Y);
            _ = vt.Keys.Enqueue(ConsoleKey.X);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Error").Should().BeNull();
        }

        [Fact]
        public void Invalid_key_with_ShowMessage_displays_the_message_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt).AddValidKey(ConsoleKey.Y).ShowMessage(k => $"'{k.Key}' is not valid");
            _ = vt.Keys.Enqueue(ConsoleKey.X);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("'X' is not valid").Should().NotBeNull();
        }

        [Fact]
        public void Invalid_key_with_ShowMessageAsync_displays_the_message()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt).AddValidKey(ConsoleKey.Y)
                .ShowMessageAsync((k, _) => Task.FromResult($"async: '{k.Key}' is not valid"));
            _ = vt.Keys.Enqueue(ConsoleKey.X);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("async: 'X' is not valid").Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Abort").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeKeyPress(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("Show/Hide").Should().NotBeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeKeyPress(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeKeyPress(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }
    }
}
