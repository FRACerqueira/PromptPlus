// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class ProgressBarControl<T> : BaseControl<ResultProgessBar<T>>,IControlProgressBar<T>, IDisposable
    {
        private readonly ProgressBarOptions<T> _options;
        private double _ticketStep;
        private readonly object _root = new();
        private bool _disposed;
        private CancellationTokenSource _lnkcts;
        private CancellationTokenSource _ctsesc;
        private Task _process;
        private Task _taskspinner;
        private UpdateProgressBar<T> _handler;
        private (int CursorLeft, int CursorTop) _initialCursor;
        private (int CursorLeft, int CursorTop) _spinnerCursor;
        private (int CursorLeft, int CursorTop) _progressbarCursor;
        private int lastline = -1;
        private T _context;

        public ProgressBarControl(IConsoleControl console, ProgressBarOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            _options.CurrentCulture ??= _options.Config.AppCulture;

            if (_options.UpdateHandler == null)
            {
                throw new PromptPlusException("Not have UpdateHandler to run");
            }

            _context = _options.ValueResult;
            _ticketStep = double.Parse(_options.Witdth.ToString()) / (int.Parse(_options.Maxvalue.ToString()) - int.Parse(_options.Minvalue.ToString()));
            _ctsesc = new CancellationTokenSource();
            _lnkcts = CancellationTokenSource.CreateLinkedTokenSource(_ctsesc.Token, cancellationToken);
            _handler = new UpdateProgressBar<T>(ref _context, _options.StartWith, _options.Minvalue, _options.Maxvalue, "");
            return _handler.Value.ToString();
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            Dispose();
        }


        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctsesc?.Cancel();
                    if (_taskspinner != null && !_taskspinner.IsCompleted)
                    {
                        _taskspinner.Wait(CancellationToken.None);
                    }
                    if (_process != null && !_process.IsCompleted)
                    {
                        _process.Wait(CancellationToken.None);
                    }
                    _taskspinner?.Dispose();
                    _process?.Dispose();
                    _lnkcts?.Dispose();
                    _ctsesc?.Dispose();
                    _handler?.Dispose();

                }
                _disposed = true;
            }
        }

        #endregion

        #region IControlProgressBar<T>

        public IControlProgressBar<T> Styles(ProgressBarStyles content, Style value)
        {
            _options.StyleControl(content, value);
            return this;
        }

        public IControlProgressBar<T> HideElements(HideProgressBar value)
        {
            _options.ShowPercent = true;
            _options.ShowDelimit = true;
            _options.ShowRanger = true;
            if (value.HasFlag(HideProgressBar.Percent))
            {
                _options.ShowPercent = false;
            }
            if (value.HasFlag(HideProgressBar.Delimit))
            {
                _options.ShowDelimit = false;
            }
            if (value.HasFlag(HideProgressBar.Ranger))
            {
                _options.ShowRanger = false;
            }
            return this;
        }

        public IControlProgressBar<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlProgressBar<T> Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlProgressBar<T> CharBar(char value)
        {
            _options.CharBar = value;
            return this;
        }

        public IControlProgressBar<T> Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlProgressBar<T> ChangeColor(Func<double, Style> value)
        {
            _options.Gradient = null;
            _options.ChangeColor = value;
            return this;
        }

        public IControlProgressBar<T> ChangeGradient(params Color[] colors)
        {
            _options.ChangeColor = null;
            _options.Gradient= colors;
            return this;
        }

        public IControlProgressBar<T> Finish(string text)
        {
            _options.Finish = text;
            return this;
        }

        public IControlProgressBar<T> FracionalDig(int value)
        {
            if (value < 0)
            {
                throw new PromptPlusException("FracionalDig must be greater than 0");
            }
            _options.FracionalDig = value;
            return this;
        }

        public IControlProgressBar<T> Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
        {
            if (spinnersType == SpinnersType.Custom && customspinner.Any())
            {
                throw new PromptPlusException("Custom spinner not have data");
            }
            if (spinnersType == SpinnersType.Custom)
            {
                _options.Spinner = new Spinners(SpinnersType.Custom, ConsolePlus.IsUnicodeSupported, speedAnimation ?? 80, customspinner);
            }
            else
            {
                _options.Spinner = new Spinners(spinnersType, ConsolePlus.IsUnicodeSupported);
            }
            return this;
        }


        public IControlProgressBar<T> Default(double value)
        {
            if (value < 0)
            {
                throw new PromptPlusException("Default must be greater than 0");
            }
            _options.StartWith = value;
            return this;
        }

        public IControlProgressBar<T> UpdateHandler(Action<UpdateProgressBar<T>, CancellationToken> value)
        {
            _options.UpdateHandler = value;
            return this;
        }

        public IControlProgressBar<T> Width(int value)
        {
            if (value < 10)
            {
                throw new PromptPlusException("Default must be greater than 10");
            }
            _options.Witdth = value;
            return this;
        }

        #endregion

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            ConsolePlus.SetCursorPosition(0, ConsolePlus.CursorTop);
            _initialCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
            _spinnerCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);

            int qtd = 0;
            var top = ConsolePlus.CursorTop;
            var hasspinner = false;
            var hasprompt = false;
            if (!_options.OptMinimalRender && !string.IsNullOrEmpty(_options.OptPrompt))
            {
                hasprompt = true;
                qtd = ConsolePlus.Write($"{_options.OptPrompt}: ", _options.StyleContent(StyleControls.Prompt), false);
                if (_options.Spinner != null)
                {
                    hasspinner = true;
                    _spinnerCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                }
                qtd += ConsolePlus.WriteLine();
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                    if (hasspinner)
                    {
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    }
                }
            }

            if (!hasspinner && _options.Spinner != null)
            {
                top = ConsolePlus.CursorTop;
                if (hasprompt)
                {
                    qtd = ConsolePlus.WriteLine();
                    _spinnerCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                }
                else
                {
                    _spinnerCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                    qtd = ConsolePlus.WriteLine();
                }
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                }
            }

            if (!string.IsNullOrEmpty(_options.OptDescription) && !_options.OptMinimalRender)
            {
                top = ConsolePlus.CursorTop;
                qtd = ConsolePlus.WriteLine(_options.OptDescription, _options.StyleContent(StyleControls.Description));
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                }
            }

            _progressbarCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
            _process = Task.Run(() =>
            {
                if (_options.Spinner != null)
                {
                    _taskspinner = Task.Run(() =>
                    {
                        WriteSpinner(_handler, _lnkcts.Token);
                    }, CancellationToken.None);
                }
                _options.UpdateHandler(_handler, _lnkcts.Token);
            }, CancellationToken.None);
        }

        public override ResultPrompt<ResultProgessBar<T>> TryResult(CancellationToken cancellationToken)
        {
            var abort = false;
            while (!_lnkcts.IsCancellationRequested && !_handler.Finish)
            {
                if (_options.Spinner == null)
                {
                    _lnkcts.Token.WaitHandle.WaitOne(10);
                }
                if (_options.OptEnabledAbortKey && KeyAvailable)
                {
                    while (KeyAvailable && !_lnkcts.Token.IsCancellationRequested)
                    {
                        var keyinfo = WaitKeypress(_lnkcts.Token);
                        if (keyinfo.HasValue && keyinfo.Value.IsPressEscKey())
                        {
                            _ctsesc.Cancel();
                            if (!_process.IsCompleted)
                            {
                                _process.Wait(CancellationToken.None);
                            }
                            abort = true;
                            _handler.Finish = true;
                        }
                    }
                }
                else
                {
                    while (KeyAvailable)
                    {
                        _ = WaitKeypress(_lnkcts.Token);
                    }
                }
                if (_handler.HasChange())
                {
                    WriteProgressBar(_handler.Value, _handler.Description, _progressbarCursor,false);
                }
                else if (_process.IsCompleted || abort)
                {
                    break;
                }
            }
            if (_process != null && !_process.IsCompleted)
            {
                _process.Wait(CancellationToken.None);
            }
            _process?.Dispose();
            if (_taskspinner != null)
            {
                if (!_taskspinner.IsCompleted)
                {
                    _taskspinner.Wait(CancellationToken.None);
                }
                _taskspinner.Dispose();
            }
            if (_lnkcts.IsCancellationRequested)
            {
                abort = true;
            }
            return new ResultPrompt<ResultProgessBar<T>>(new ResultProgessBar<T>(_handler.Context,_handler.Value), abort);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultProgessBar<T> result, bool aborted)
        {
            _ctsesc.Cancel();

            ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);
            for (int i = _initialCursor.CursorTop; i < lastline; i++)
            {
                ConsolePlus.WriteLine("");
            }
            ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);

            var endvalue = result.Lastvalue;
            if (aborted)
            {
                if (_options.OptMinimalRender)
                {
                    return;
                }
                var aux = "";
                if (!_options.OptMinimalRender)
                {
                    aux = Messages.CanceledKey.Replace("[", "[[").Replace("]", "]]");
                    if (!string.IsNullOrEmpty(_options.OptPrompt))
                    {
                        ConsolePlus.Write($"{_options.OptPrompt}: ", _options.StyleContent(StyleControls.Prompt), false);
                    }
                }
                ConsolePlus.WriteLine(aux, _options.StyleContent(StyleControls.Answer), false);
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.Finish))
                {
                    ConsolePlus.Write($"{_options.OptPrompt ?? string.Empty}: ", _options.StyleContent(StyleControls.Prompt), false);
                    ConsolePlus.Write(_options.Finish, _options.StyleContent(StyleControls.Answer), true);
                    ConsolePlus.WriteLine("", _options.StyleContent(StyleControls.Answer), true);
                }
            }
            _progressbarCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
            WriteProgressBar(endvalue, null, _progressbarCursor,true);
        }

        private void WriteSpinner(UpdateProgressBar<T> handler, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !handler.Finish)
            {
                lock (_root)
                {
                    var (CursorLeft, CursorTop) = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                    ConsolePlus.SetCursorPosition(_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop);
                    if (_options.Spinner != null)
                    {
                        var spn = _options.Spinner.NextFrame(cancellationToken);
                        var top = ConsolePlus.CursorTop;
                        int qtd = ConsolePlus.Write($"{spn}", _options.StyleContent(StyleControls.Spinner));
                        if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                        {
                            var dif = top + qtd - ConsolePlus.BufferHeight;
                            _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                            _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                            _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                            CursorTop -= dif;
                        }
                    }
                    ConsolePlus.SetCursorPosition(CursorLeft, CursorTop);
                }
            }
        }

        private void WriteProgressBar(double value,string handlerdescription, (int CursorLeft, int CursorTop) cursor, bool isend)
        {
            lock (_root)
            {
                var qtd = 0;
                
                ConsolePlus.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                if (_options.ShowRanger)
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write($"{ValueToString(_options.Minvalue)} ", _options.StyleContent(StyleControls.Ranger));
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }

                }
                char charbarOn = ' ';
                char charbarOff = ' ';
                string delimitbar = "│";
                Style OnStyle = Style.Default.Foreground(_options.StyleContent(StyleControls.Slider).Foreground);
                if (_options.ChangeColor != null)
                {
                    OnStyle = _options.ChangeColor(value);
                }
                if (!ConsolePlus.IsUnicodeSupported)
                {
                    delimitbar = "|";
                }
                switch (_options.BarType)
                {
                    case ProgressBarType.Fill:
                        {
                            charbarOff = charbarOn;
                            OnStyle = OnStyle.Background(OnStyle.Foreground);
                            if (!ConsolePlus.IsUnicodeSupported)
                            {
                                charbarOn = _options.CharBar;
                                charbarOff = _options.CharBar;
                            }
                        }
                        break;
                    case ProgressBarType.Light:
                        {
                            charbarOn = '─';
                            if (!ConsolePlus.IsUnicodeSupported)
                            {
                                charbarOn = '-';
                            }
                        }
                        break;
                    case ProgressBarType.Heavy:
                        {
                            charbarOn = '━';
                            if (!ConsolePlus.IsUnicodeSupported)
                            {
                                charbarOn = '=';
                            }
                        }
                        break;
                    case ProgressBarType.Square:
                        {
                            charbarOn = '■';
                            if (!ConsolePlus.IsUnicodeSupported)
                            {
                                charbarOn = '#';
                            }
                        }
                        break;
                    case ProgressBarType.Bar:
                        {
                            charbarOn = '▐';
                            if (!ConsolePlus.IsUnicodeSupported)
                            {
                                charbarOn = '|';
                            }
                        }
                        break;
                    case ProgressBarType.AsciiSingle:
                        charbarOn = '-';
                        break;
                    case ProgressBarType.AsciiDouble:
                        charbarOn = '=';
                        break;
                    case ProgressBarType.Dot:
                        charbarOn = '.';
                        break;
                    case ProgressBarType.Char:
                        charbarOn = _options.CharBar;
                        break;
                    default:
                        throw new PromptPlusException($"BarType: {_options.BarType} Not Implemented");
                }
                if (_options.ShowDelimit)
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(delimitbar, _options.StyleContent(StyleControls.Ranger));
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }
                if (_options.Gradient != null)
                {
                    var txt = new string(charbarOn, _options.Witdth);
                    var aux = Gradient(txt, _options.Gradient);
                    var valuestep = CurrentValueStep(value);
                    for (int i = 0; i < aux.Length; i++)
                    {
                        if (i < valuestep && valuestep > 0)
                        {
                            var top = ConsolePlus.CursorTop;
                            if (aux[i].Text[0].Equals(' '))
                            {
                                qtd = ConsolePlus.Write(aux[i].Text[0].ToString(), OnStyle.Background(aux[i].Style.Foreground));
                            }
                            else
                            {
                                qtd = ConsolePlus.Write(aux[i].Text[0].ToString(), OnStyle.Foreground(aux[i].Style.Foreground));
                            }
                            if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                            {
                                var dif = top + qtd - ConsolePlus.BufferHeight;
                                _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                                _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                                _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                                cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                            }
                        }
                    }
                }
                else
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(new string(charbarOn, CurrentValueStep(value)), OnStyle);
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }
                var offlength = _options.Witdth - CurrentValueStep(value);
                if (offlength > 0)
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(new string(charbarOff, offlength), Style.Default.Foreground(_options.StyleContent(StyleControls.Slider).Background));
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }
                if (_options.ShowDelimit)
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(delimitbar, _options.StyleContent(StyleControls.Ranger));
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }
                if (_options.ShowRanger)
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write($" {ValueToString(_options.Maxvalue)}", _options.StyleContent(StyleControls.Ranger));
                    if (_options.ShowPercent)
                    {
                        qtd += ConsolePlus.WriteLine($" ({ValueToString(value)}%)", _options.StyleContent(StyleControls.Answer));
                    }
                    else
                    {
                        qtd = ConsolePlus.WriteLine("", _options.StyleContent(StyleControls.Prompt));
                    }
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }
                else
                {
                    var top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.Write(" ", _options.StyleContent(StyleControls.Prompt));
                    if (_options.ShowPercent)
                    {
                        qtd += ConsolePlus.WriteLine($" ({ValueToString(value)}%)", _options.StyleContent(StyleControls.Answer));
                    }
                    else
                    {
                        qtd = ConsolePlus.WriteLine("", _options.StyleContent(StyleControls.Prompt));
                    }
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                        cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                    }
                }

                var topd = ConsolePlus.CursorTop;
                qtd = ConsolePlus.Write(handlerdescription ?? string.Empty, _options.StyleContent(StyleControls.TaggedInfo), true);
                if (ConsolePlus.IsTerminal && topd + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = topd + qtd - ConsolePlus.BufferHeight;
                    _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                    _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                    cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                }

                if (_options.OptShowTooltip && !isend)
                {
                    var tp = _options.OptToolTip;
                    if (string.IsNullOrEmpty(tp))
                    {
                        tp = ScreenBufferExtensions.EscTooltip(_options);
                    }
                    if (!string.IsNullOrEmpty(tp))
                    {
                        var top = ConsolePlus.CursorTop;
                        if (handlerdescription != null)
                        {
                            qtd = ConsolePlus.WriteLine();
                        }
                        qtd += ConsolePlus.WriteLine(tp, _options.StyleContent(StyleControls.Tooltips), true);
                        if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                        {
                            var dif = top + qtd - ConsolePlus.BufferHeight;
                            _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                            _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                            _progressbarCursor = (_progressbarCursor.CursorLeft, _progressbarCursor.CursorTop - dif);
                            cursor = (cursor.CursorLeft, cursor.CursorTop - dif);
                        }
                    }
                }

                lastline = ConsolePlus.CursorTop;
            }
        }

        private int CurrentValueStep(double value) => (int)(_ticketStep * double.Parse(value.ToString()));

        private string ValueToString(double value)
        {
            var tmp = Math.Round(value, _options.FracionalDig).ToString(_options.CurrentCulture);
            var decsep = _options.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp.Substring(index + 1);
                if (dec.Length > _options.FracionalDig)
                {
                    dec = dec.Substring(0, _options.FracionalDig);
                }
                if (dec.Length < _options.FracionalDig)
                {
                    dec += new string('0', _options.FracionalDig - dec.Length);
                }
                if (_options.FracionalDig > 0)
                {
                    tmp = tmp.Substring(0, index) + decsep + dec;
                }
                else
                {
                    tmp = tmp.Substring(0, index);
                }
            }
            else
            {
                if (_options.FracionalDig > 0)
                {
                    tmp += decsep + new string('0', _options.FracionalDig);
                }
            }
            return tmp;
        }

        private static StringStyle[] Gradient(string text, params Color[] colors)
        {
            var result = new List<StringStyle>();
            for (int i = 0; i < text.Length; i++)
            {
                float percentage = (colors.Length - 1) * ((float)i / text.Length);
                int colorPrevIndex = (int)percentage;
                int colorNextIndex = (int)Math.Ceiling(percentage);
                Color colorPrev = colors[colorPrevIndex];
                Color colorNext = colors[colorNextIndex];
                float ltrOffset = percentage - colorPrevIndex;
                float rtlOffset = 1 - ltrOffset;

                byte r = (byte)(rtlOffset * colorPrev.R + ltrOffset * colorNext.R);
                byte g = (byte)(rtlOffset * colorPrev.G + ltrOffset * colorNext.G);
                byte b = (byte)(rtlOffset * colorPrev.B + ltrOffset * colorNext.B);

                var color = new Color(r, g, b);
                result.Add(new StringStyle(text[i].ToString(), new Style(color, color)));
            }
            return result.ToArray();
        }
    }
}
