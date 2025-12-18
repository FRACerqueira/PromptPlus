// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;

namespace PromptPlusLibrary.Controls.ReadLine
{
    internal sealed class ReadLineEmacs(IConsoleExtend console, PromptConfig promptConfig, string initialValue = "") : IEmacs
    {
        private CaseOptions _caseOptions = PromptPlusLibrary.CaseOptions.Any;
        private bool _escabort;
        private int _maxlength = int.MaxValue;
        private int? _maxwidth;
        private bool _readonly;
        private Func<char, bool> _validateKey = (c) => true;

        public IEmacs CaseOptions(CaseOptions value)
        {
            _caseOptions = value;
            return this;
        }

        public IEmacs EscAbort(bool value = true)
        {
            _escabort = value;
            return this;
        }

        public IEmacs ReadOnly(bool value = true)
        {
            _readonly = value;
            return this;
        }

        public IEmacs MaxLength(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "MaxLength must be greater than 1");
            }
            _maxlength = value;
            return this;
        }

        public IEmacs MaxWidth(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "MaxWidth must be greater than 1");
            }
            _maxwidth = value;
            return this;
        }

        public IEmacs ValidateKey(Func<char, bool> value)
        {
            _validateKey = value ?? throw new ArgumentNullException(nameof(value), "ValidateKey cannot be null");
            return this;
        }

        public string? ReadLine()
        {
            bool oldcurvisible = console.CursorVisible;
            console.CursorVisible = false;
            (int initleft, int inittop) = console.GetCursorPosition();
            EmacsBuffer _inputBuffer = new(_readonly, _caseOptions, _validateKey, _maxlength, _maxwidth);
            _inputBuffer.LoadPrintable(initialValue);

            if (_inputBuffer.IsVirtualBuffer)
            {
                string str = _inputBuffer.IsHideLeftBuffer
                    ? promptConfig.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : promptConfig.GetSymbol(SymbolType.InputDelimiterLeft);
                console.RawWrite(str);
            }

            (int Left, int Top) savedcur = console.RawWrite(_inputBuffer.ToBackward(), clearrestofline: true);
            console.RawWrite(_inputBuffer.ToForward());

            if (_inputBuffer.IsVirtualBuffer)
            {
                string str = _inputBuffer.IsHideRightBuffer
                    ? promptConfig.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : promptConfig.GetSymbol(SymbolType.InputDelimiterRight);
                console.RawWrite(str);
            }

            console.SetCursorPosition(savedcur.Left, savedcur.Top);

            bool endinput = false;
            do
            {
                console.CursorVisible = true;
                ConsoleKeyInfo keyInfo = console.ReadKey(true);
                console.CursorVisible = false;
                int oldlen = _inputBuffer.Length;
                if (keyInfo.IsPressEscKey() && _escabort)
                {
                    _inputBuffer.Clear();
                    endinput = true;
                }
                else if (!_inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo))
                {
                    if (keyInfo.IsPressEnterKey())
                    {
                        endinput = true;
                    }
                    continue;
                }

                console.SetCursorPosition(initleft, inittop);
                if (_inputBuffer.IsVirtualBuffer)
                {
                    string str = _inputBuffer.IsHideLeftBuffer
                        ? promptConfig.GetSymbol(SymbolType.InputDelimiterLeftMost)
                        : promptConfig.GetSymbol(SymbolType.InputDelimiterLeft);
                    console.RawWrite(str);
                }
                savedcur = console.RawWrite(_inputBuffer.ToBackward(), clearrestofline: oldlen != _inputBuffer.Length);
                console.RawWrite(_inputBuffer.ToForward());

                if (_inputBuffer.IsVirtualBuffer)
                {
                    string str = _inputBuffer.IsHideRightBuffer
                        ? promptConfig.GetSymbol(SymbolType.InputDelimiterRightMost)
                        : promptConfig.GetSymbol(SymbolType.InputDelimiterRight);
                    console.RawWrite(str);
                }

                console.SetCursorPosition(savedcur.Left, savedcur.Top);

            } while (!endinput);
            console.CursorVisible = oldcurvisible;
            return _inputBuffer.ToString();
        }
    }
}
