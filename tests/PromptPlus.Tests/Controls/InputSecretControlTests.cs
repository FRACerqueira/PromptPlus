using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Camada 2 (render + estado via VirtualTerminal) — controle Secret (`PromptPlusControls.Secret`,
    // IInputSecretControl). Mesma classe de produção do Input (`InputControl`, `isSecret: true`) e
    // mesmas regras de segurança de teste (tecla terminal + CancellationToken; ver
    // SelectControlTests/InputControlTests). Suíte separada porque a interface pública é distinta
    // (IInputSecretControl não expõe History/Suggestions — InputControl.InitControl pula os dois
    // quando _isinputsecret é true) e o comportamento visível (mascaramento, F2) é específico daqui.
    public class InputSecretControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IInputSecretControl MakeSecret(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Secret("Password");

        [Fact]
        public void Typed_characters_are_masked_with_the_default_character()
        {
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("Joe");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 13).Should().Be("Password: ###");
            _ = vt.Find("Joe").Should().BeNull();
        }

        [Fact]
        public void MaskSecret_with_a_custom_character_masks_with_that_character()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Secret("Password").MaskSecret('*');
            _ = vt.Keys.Type("Joe");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 13).Should().Be("Password: ***");
        }

        [Fact]
        public void F2_reveals_the_plain_text_then_hides_it_again()
        {
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 13).Should().Be("Password: Joe");

            var vt2 = MakeTerminal();
            var control2 = MakeSecret(vt2);
            _ = vt2.Keys.Type("Joe").Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.TextAt(0, 0, 13).Should().Be("Password: ###");
        }

        [Fact]
        public void F2_does_nothing_when_MaskSecret_disables_the_view_toggle()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Secret("Password").MaskSecret(enabledView: false);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 13).Should().Be("Password: ###");
        }

        [Fact]
        public void Enter_confirms_with_the_real_unmasked_value()
        {
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Joe");
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_real_value_typed_so_far()
        {
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Joe");
        }

        [Fact]
        public void Backspace_removes_the_last_typed_character()
        {
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Backspace).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Jo");
        }

        [Fact]
        public void Enter_with_a_failing_predicate_shows_an_error_and_does_not_confirm()
        {
            // Also a regression check for the ResultCtrl-on-cancellation fix (InputControl.cs) —
            // same TryResult code path as InputControlTests' equivalent, exercised here via the
            // Secret entry point.
            var vt = MakeTerminal();
            var control = MakeSecret(vt).PredicateValid(_ => false);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Joe");
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Typed_wide_cjk_characters_are_masked_with_one_secret_char_per_display_column()
        {
            // Regression: WriteAnswer used to mask by visibleLeft.Length (rune count of the already
            // display-width-sliced viewport), not by its display width. "가나다" is 3 characters but
            // 6 display columns, so the old bug drew 3 '#' instead of 6, undersizing the mask.
            var vt = MakeTerminal();
            var control = MakeSecret(vt);
            _ = vt.Keys.Type("가나다");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 16).Should().Be("Password: ######");
            _ = vt.Find("가나다").Should().BeNull();
        }
    }
}
