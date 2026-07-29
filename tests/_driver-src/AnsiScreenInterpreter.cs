// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source)
// ***************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Text;
using ConsolePlusLibrary.Core;

namespace ConsolePlusLibrary.Testing
{
    /// <summary>
    /// Interprets the ANSI subset emitted by ConsolePlus's real <c>ConsoleWriter</c>/<c>AnsiCommands</c>
    /// (verified against <c>AnsiCommands.cs</c>/<c>AnsiColorBuilder.cs</c>) and stamps cells on a <see cref="VirtualScreen"/>.
    /// Fails fast on anything unrecognized, so an unmodeled sequence surfaces as a test failure instead of a false green.
    /// </summary>
    internal sealed class AnsiScreenInterpreter(VirtualScreen screen, VirtualTerminal owner) : TextWriter
    {
        private enum State { Normal, Esc, Csi }

        private State _state = State.Normal;
        private bool _private;
        private readonly StringBuilder _params = new();
        private char _pendingHighSurrogate = '\0';

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value) => Feed(value);

        public override void Write(string? value)
        {
            if (value != null)
            {
                foreach (char ch in value)
                {
                    Feed(ch);
                }
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Feed(buffer[index + i]);
            }
        }

        public override void WriteLine() => Feed('\n');

        private void Feed(char ch)
        {
            switch (_state)
            {
                case State.Normal:
                    if (ch == '\x1b') { _state = State.Esc; return; }
                    if (ch == '\n') { screen.NewLine(); return; }
                    if (ch == '\r') { screen.CarriageReturn(); return; }
                    PutChar(ch);
                    return;

                case State.Esc:
                    if (ch == '[') { _state = State.Csi; _private = false; _ = _params.Clear(); return; }
                    throw new NotSupportedException($"ESC {ch} is not supported by the test interpreter.");

                case State.Csi:
                    if (ch == '?' && _params.Length == 0) { _private = true; return; }
                    if ((ch >= '0' && ch <= '9') || ch == ';') { _ = _params.Append(ch); return; }
                    Dispatch(ch, _params.ToString());
                    _state = State.Normal;
                    return;
            }
        }

        private void PutChar(char ch)
        {
            if (char.IsHighSurrogate(ch)) { _pendingHighSurrogate = ch; return; }
            if (_pendingHighSurrogate != '\0')
            {
                screen.Put(new Rune(_pendingHighSurrogate, ch));
                _pendingHighSurrogate = '\0';
                return;
            }
            screen.Put(new Rune(ch));
        }

        private void Dispatch(char final, string prm)
        {
            switch (final)
            {
                case 'H': // CUP row;col (1-indexed) — AnsiCommands.CursorPosition(row, column)
                    {
                        var (row, column) = TwoParams(prm, 1, 1);
                        screen.SetCursor(left: column - 1, top: row - 1);
                        break;
                    }
                case 'K': screen.EraseInLine(FirstParam(prm, 0)); break;
                case 'J': screen.EraseInDisplay(FirstParam(prm, 0)); break;
                case 'm': ApplySgr(prm); break;
                case 'h':
                case 'l':
                    if (_private)
                    {
                        HandlePrivateMode(prm, set: final == 'h');
                    }
                    else
                    {
                        throw new NotSupportedException($"CSI {prm}{final} is not supported by the test interpreter.");
                    }
                    break;
                default:
                    throw new NotSupportedException($"CSI {prm}{final} is not supported by the test interpreter (surface it in the pilot).");
            }
        }

        private void HandlePrivateMode(string prm, bool set)
        {
            switch (prm)
            {
                case "25": owner.SetCursorVisibleInternal(set); break;
                case "1049": owner.SetAltScreenInternal(set); break;
                default: break; // other DEC-private modes are irrelevant to position/style
            }
        }

        private void ApplySgr(string prm)
        {
            // Production never emits a bare reset ("0m") today — AnsiColorBuilder.Build returns an
            // empty sequence for Color.Default and ConsoleWriter.WriteSgr skips the call entirely when
            // the buffer is empty (verified in AnsiColorBuilder.cs/ConsoleWriter.cs). The reset branch
            // below is kept for forward compatibility, not because it is exercised today.
            string[] p = prm.Length == 0 ? ["0"] : prm.Split(';');
            for (int i = 0; i < p.Length; i++)
            {
                int code = int.Parse(p[i], CultureInfo.InvariantCulture);
                if (code == 0)
                {
                    owner.ResetColorInternal();
                }
                else if ((code == 38 || code == 48) && i + 4 < p.Length && p[i + 1] == "2")
                {
                    var color = new Color(
                        (byte)int.Parse(p[i + 2], CultureInfo.InvariantCulture),
                        (byte)int.Parse(p[i + 3], CultureInfo.InvariantCulture),
                        (byte)int.Parse(p[i + 4], CultureInfo.InvariantCulture));
                    if (code == 38) { owner.SetForegroundInternal(color); } else { owner.SetBackgroundInternal(color); }
                    i += 4;
                }
                else if ((code == 38 || code == 48) && i + 2 < p.Length && p[i + 1] == "5")
                {
                    // 8-bit indexed SGR (38;5;n / 48;5;n) — AnsiColorBuilder.GetEightBit emits this
                    // whenever a Color has .Number set (named ConsoleColor constants, and anything
                    // produced by Style.FindStyle's contrast adjustment, which snaps the foreground to
                    // the "nearest palette color" — confirmed hit by SwitchStyles.Slider's default
                    // Style(ConsoleColor.White, ConsoleColor.DarkGray)). Resolve the index against
                    // ConsolePlus's own already-tested 256-color table (ColorPalette.EightBit) instead
                    // of re-deriving the xterm cube/grayscale formula here, so the driver can never
                    // drift from what production actually uses.
                    int index = int.Parse(p[i + 2], CultureInfo.InvariantCulture);
                    Color palette = ColorPalette.EightBit[index];
                    var color = new Color(palette.R, palette.G, palette.B);
                    if (code == 38) { owner.SetForegroundInternal(color); } else { owner.SetBackgroundInternal(color); }
                    i += 2;
                }
                else
                {
                    // 4-bit SGR (30-37/90-97, 40-47/100-107) — not modeled yet; surface it if hit.
                    throw new NotSupportedException(
                        $"SGR code {code} is not modeled by the test interpreter. Build colors with `new Color(r, g, b)`, " +
                        "not a named palette constant, so AnsiColorBuilder stays on the truecolor path.");
                }
            }
            screen.Current = new Style(owner.ForegroundRgbColor, owner.BackgroundRgbColor);
        }

        private static int FirstParam(string prm, int def)
            => prm.Length == 0 ? def : int.Parse(prm.Split(';')[0], CultureInfo.InvariantCulture);

        private static (int, int) TwoParams(string prm, int d1, int d2)
        {
            if (prm.Length == 0) { return (d1, d2); }
            string[] p = prm.Split(';');
            int a = p.Length > 0 && p[0].Length > 0 ? int.Parse(p[0], CultureInfo.InvariantCulture) : d1;
            int b = p.Length > 1 && p[1].Length > 0 ? int.Parse(p[1], CultureInfo.InvariantCulture) : d2;
            return (a, b);
        }
    }
}
