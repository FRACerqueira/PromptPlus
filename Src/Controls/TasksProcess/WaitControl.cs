// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class WaitControl<T> : BaseControl<ResultWaitProcess<T>>, IControlWait<T>, IDisposable
    {
        private readonly WaitOptions<T> _options;
        private CancellationTokenSource _lnkcts;
        private CancellationTokenSource _ctsesc;
        private Task _process;
        private bool _disposed;
        private (int CursorLeft, int CursorTop) _initialCursor;
        private (int CursorLeft, int CursorTop) _spinnerCursor;
        private int _promptlines;
        private EventWaitProcess<T> _event;
        private T _context;


        public WaitControl(IConsoleControl console, WaitOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.Steps.Count == 0)
            {
                throw new PromptPlusException("Not have process to run");
            }
            _context = _options.Context;
            _event = new EventWaitProcess<T>(ref _context, false);
            _ctsesc = new CancellationTokenSource();
            _lnkcts = CancellationTokenSource.CreateLinkedTokenSource(_ctsesc.Token, cancellationToken);
            return string.Empty;
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
                    if (_process != null && !_process.IsCompleted)
                    {
                        _process.Wait(CancellationToken.None);
                    }
                    _process?.Dispose();
                    _lnkcts?.Dispose();
                    _ctsesc?.Dispose();
                    _event?.Dispose();

                }
                _disposed = true;
            }
        }

        #endregion

        #region IControlWait

        public IControlWait<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlWait<T>, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }
        public IControlWait<T> ShowElapsedTime(bool value = true)
        {
            _options.ShowElapsedTime = value;
            return this;
        }

        public IControlWait<T> Context(T value)
        {
            _options.Context = value;
            return this;
        }


        public IControlWait<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }
        public IControlWait<T> Finish(string value)
        {
            _options.Finish = value;
            return this;
        }

        public IControlWait<T> TaskTitle(string value)
        {
            _options.OverWriteTitlekName = value;
            return this;
        }

        public IControlWait<T> Spinner(SpinnersType spinnersType, Style? spinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
        {
            if (spinnersType == SpinnersType.Custom && customspinner.Any())
            {
                throw new PromptPlusException("Custom spinner not have data");
            }
            _options.Spinner = new Spinners(spinnersType, ConsolePlus.IsUnicodeSupported);
            if (spinnersType == SpinnersType.Custom)
            {
                _options.Spinner = new Spinners(SpinnersType.Custom, ConsolePlus.IsUnicodeSupported, speedAnimation ?? 80, customspinner);
            }
            if (spinnerStyle.HasValue)
            {
                _options.SpinnerStyle = spinnerStyle.Value;
            }
            return this;
        }


        public IControlWait<T> MaxDegreeProcess(int value)
        {
            if (value < 1)
            {
                value = Environment.ProcessorCount;
            }
            _options.MaxDegreeProcess = value;
            return this;
        }

        public IControlWait<T> AddStep(StepMode stepMode, params Action<EventWaitProcess<T>, CancellationToken>[] process)
        {
            return AddStep(stepMode, string.Empty, string.Empty, process);
        }

        public IControlWait<T> AddStep(StepMode stepMode, string id, string description, params Action<EventWaitProcess<T>,CancellationToken>[] process)
        {
            foreach (var item in process)
            {
                _options.Steps.Add(item);
                _options.States.Add(new StateProcess
                    (id,
                    description,
                    TaskStatus.WaitingForActivation,
                    null,
                    TimeSpan.Zero,
                    stepMode));
            }
            return this;
        }

        #endregion

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            _initialCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
            _process ??= Task.Run(() => RunAllTasks(_lnkcts.Token), CancellationToken.None);
        }

        public override ResultPrompt<ResultWaitProcess<T>> TryResult(CancellationToken cancellationToken)
        {
            var abort = false;
            while (!_lnkcts.IsCancellationRequested)
            {
                if (_options.OptEnabledAbortKey && KeyAvailable)
                {
                    var keyinfo = WaitKeypress(_lnkcts.Token);
                    if (keyinfo.HasValue && keyinfo.Value.IsPressEscKey())
                    {
                        abort = true;
                        _ctsesc.Cancel();
                        if (!_process.IsCompleted)
                        {
                            _process.Wait(CancellationToken.None);
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
                _lnkcts.Token.WaitHandle.WaitOne(10);
                if (_process?.IsCompleted??true)
                {
                    break;
                }
            }
            if (!abort && _lnkcts.IsCancellationRequested)
            {
                abort = true;
                _ctsesc.Cancel();
            }
            return new ResultPrompt<ResultWaitProcess<T>>(new ResultWaitProcess<T>(_event.Context,_options.States.ToArray()), abort);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultWaitProcess<T> result, bool aborted)
        {
            _ctsesc.Cancel();
            if (aborted)
            {
                var aux = Messages.CanceledKey.Replace("[", "[[").Replace("]", "]]");
                ConsolePlus.Write($"{_options.OptPrompt ?? string.Empty}: ", _options.OptStyleSchema.Prompt(), false);
                ConsolePlus.WriteLine(aux, _options.OptStyleSchema.Answer(), true);
                if (!string.IsNullOrEmpty(_options.OptDescription))
                {
                    ConsolePlus.WriteLine(_options.OptDescription, _options.OptStyleSchema.Description(), true);
                }
            }
            else
            {
                var haspromt = false;
                if (!string.IsNullOrEmpty(_options.Finish) || _options.WaitTime)
                {
                    if (_options.WaitTime)
                    { 
                        _options.Finish = _options.TimeDelay.ToString();
                    }
                    haspromt = true;
                    ConsolePlus.Write($"{_options.OptPrompt ?? string.Empty}: ", _options.OptStyleSchema.Prompt(), false);
                    ConsolePlus.Write(_options.Finish, _options.OptStyleSchema.Answer(), true);
                }
                else
                {
                    if (!string.IsNullOrEmpty(_options.OptPrompt))
                    {
                        haspromt = true;
                        ConsolePlus.Write(_options.OptPrompt, _options.OptStyleSchema.Prompt(), true);
                    }
                }
                if (!string.IsNullOrEmpty(_options.OptDescription))
                {
                    if (haspromt)
                    {
                        ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                    }
                    ConsolePlus.WriteLine(_options.OptDescription, _options.OptStyleSchema.Description(), true);
                }
                else
                {
                    if (haspromt)
                    {
                        ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                    }
                }
            }
        }

        private void RunAllTasks(CancellationToken cancellationtoken)
        {
            var i = 0;
            var timerSpinner = new Stopwatch();
            do
            {
                if (i > 0)
                {
                    ClearLast();
                }
                var tasks = new List<(int, Task)>();
                var currentmode = _options.States[i].StepMode;
                var executelist = new List<int>();
                string firstdesc = _options.States[i].Description;
                bool samedesc = true;
                var detailsElapsedTime = new List<(int left, int top, Stopwatch sw)>();
                var degreecount = 0;
                do
                {
                    executelist.Add(i);
                    if (samedesc)
                    {
                        samedesc = (firstdesc == _options.States[i].Description);
                    }
                    if (!cancellationtoken.IsCancellationRequested && !_event.CancelAllNextTasks)
                    {
                        var act = _options.Steps[i];
                        using var waitHandle = new AutoResetEvent(false);
                        tasks.Add((i, Task.Run(() =>
                        {
                            var localpos = i;
                            TaskStatus actsta;
                            Exception actex = null;
                            var tm = new Stopwatch();
                            detailsElapsedTime.Add((-1, -1, tm));
                            waitHandle.Set();
                            tm.Start();
                            try
                            {
                                act.Invoke(_event,cancellationtoken);
                                actsta = cancellationtoken.IsCancellationRequested? TaskStatus.Canceled: TaskStatus.RanToCompletion;
                            }
                            catch (Exception ex)
                            {
                                actex = ex;
                                actsta = TaskStatus.Faulted;
                            }
                            tm.Stop();

                            var aux = _options.States[localpos];
                            _options.States[localpos] = new StateProcess(
                                aux.Id,
                                aux.Description,
                                actsta,
                                actex,
                                tm.Elapsed,
                                aux.StepMode);
                        }, CancellationToken.None)));
                        waitHandle.WaitOne();
                    }
                    else
                    {
                        using var waitHandlecanc = new AutoResetEvent(false);
                        tasks.Add((i, Task.Run(() =>
                        {
                            var localpos = i;
                            var tm = new Stopwatch();
                            tm.Start();
                            detailsElapsedTime.Add((-1, -1, new Stopwatch()));
                            waitHandlecanc.Set();
                            var aux = _options.States[localpos];
                            tm.Stop();
                            _options.States[localpos] = new StateProcess(
                                  aux.Id,
                                  aux.Description,
                                  TaskStatus.Canceled,
                                  null,
                                  tm.Elapsed,
                                  aux.StepMode);
                        }, CancellationToken.None)));
                        waitHandlecanc.WaitOne();
                    }
                    i++;
                    degreecount++;
                    if (degreecount >= Math.Min(_options.MaxDegreeProcess, 10))
                    {
                        break;
                    }
                }
                while (!cancellationtoken.IsCancellationRequested && currentmode == StepMode.Parallel && i < _options.Steps.Count && _options.States[i].StepMode == StepMode.Parallel);
                if (cancellationtoken.IsCancellationRequested)
                {
                    for (int pos = 0; i < tasks.Count; i++)
                    {
                        (int, Task) aux = tasks[pos];
                        if (!aux.Item2.IsCompleted)
                        {
                            aux.Item2.Wait(CancellationToken.None);
                        }
                        aux.Item2.Dispose();
                    }
                    tasks.Clear();
                    break;
                }

                string desc;
                if (samedesc)
                {
                    desc = firstdesc;
                }
                else
                {
                    var auxdesc = new List<string>();
                    foreach (var item in executelist)
                    {
                        var dj = _options.States[item].Description;
                        if (!string.IsNullOrEmpty(dj) && !auxdesc.Contains(dj))
                        {
                            auxdesc.Add(dj);
                        }
                    }
                    if (auxdesc.Any())
                    {
                        desc = string.Join(",", auxdesc.ToArray());
                    }
                }
                var qtdlines = 0;
                ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);
                var top = ConsolePlus.CursorTop;
                var qtd = ConsolePlus.Write($"{(string.IsNullOrEmpty(_options.OptPrompt) ? Messages.Wait : _options.OptPrompt)}: ", _options.OptStyleSchema.Prompt(), false);
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                }
                qtdlines += qtd;

                _spinnerCursor = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                if (_options.OptShowTooltip)
                {
                    var tp = _options.OptToolTip;
                    if (string.IsNullOrEmpty(tp))
                    {
                        tp = ScreenBufferExtensions.EscTooltip(_options);
                    }
                    if (!string.IsNullOrEmpty(tp))
                    {
                        top = ConsolePlus.CursorTop;
                        qtd = ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                        qtd += ConsolePlus.Write(tp, _options.OptStyleSchema.Tooltips(), true);
                        if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                        {
                            var dif = top + qtd - ConsolePlus.BufferHeight;
                            _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                            _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        }
                        qtdlines += qtd;
                    }
                }
                if (!string.IsNullOrEmpty(_options.OptDescription))
                {
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                    qtd += ConsolePlus.Write(_options.OptDescription, _options.OptStyleSchema.Description(), true);
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    }
                    qtdlines += qtd;
                }

                if (!_options.WaitTime)
                {
                    top = ConsolePlus.CursorTop;
                    qtd = ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                    if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                    {
                        var dif = top + qtd - ConsolePlus.BufferHeight;
                        _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                        _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    }
                    qtdlines += qtd;

                    var max = executelist.Count;
                    for (int pos = 0; pos < max; pos++)
                    {
                        string symb;
                        if (pos == max - 1)
                        {
                            symb = " └─";
                        }
                        else
                        {
                            symb = " ├─";
                            if (max == 1)
                            {
                                symb = " └─";
                            }
                        }
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            symb = " >>";
                        }
                        var tasknamecult = Messages.Task;
                        if (!string.IsNullOrEmpty(_options.OverWriteTitlekName))
                        {
                            tasknamecult = _options.OverWriteTitlekName;
                        }
                        desc = _options.States[executelist[pos]].Description;
                        if (string.IsNullOrEmpty(desc))
                        {
                            var index = (executelist[pos] + 1).ToString("00");
                            desc = $"{tasknamecult}({index})";
                        }
                        else if (samedesc)
                        {
                            var index = (executelist[pos] + 1).ToString("00");
                            desc = $"{tasknamecult}({index}) {desc}";
                        }
                        top = ConsolePlus.CursorTop;
                        qtd = ConsolePlus.Write($"{symb} {desc}", _options.OptStyleSchema.Description(), false);
                        if (!_options.ShowCountdown)
                        {
                            qtd += ConsolePlus.Write("               ", _options.OptStyleSchema.Prompt(), true);
                        }
                        if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                        {
                            var dif = top + qtd - ConsolePlus.BufferHeight;
                            _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                            _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                        }
                        qtdlines += qtd;

                        detailsElapsedTime[pos] = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop-qtd, detailsElapsedTime[pos].sw);
                        if (qtd > 0)
                        {
                            for (int iold = 0; iold < pos; iold++)
                            {
                                detailsElapsedTime[iold] = (detailsElapsedTime[iold].left, detailsElapsedTime[iold].top - qtd, detailsElapsedTime[iold].sw);
                            }
                        }
                        if (pos != max - 1)
                        {
                            top = ConsolePlus.CursorTop;
                            qtd = ConsolePlus.WriteLine("", _options.OptStyleSchema.Prompt(), true);
                            if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                            {
                                var dif = top + qtd - ConsolePlus.BufferHeight;
                                _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                                _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                                if (dif > 0)
                                {
                                    detailsElapsedTime[pos] = (detailsElapsedTime[pos].left, detailsElapsedTime[pos].top - dif, detailsElapsedTime[pos].sw);
                                    for (int iold = 0; iold < pos; iold++)
                                    {
                                        detailsElapsedTime[iold] = (detailsElapsedTime[iold].left, detailsElapsedTime[iold].top - dif, detailsElapsedTime[iold].sw);
                                    }
                                }
                            }
                            qtdlines += qtd;
                        }
                    }
                }
                if (_promptlines < qtdlines)
                {
                    _promptlines = qtdlines;
                }
                timerSpinner.Start();
                var tkspinner = Task.Run(() => ShowSpinner(detailsElapsedTime, timerSpinner, cancellationtoken), CancellationToken.None);
                Task.WaitAll(tasks.Select(x => x.Item2).Where(x => !x.IsCompleted).ToArray(), CancellationToken.None);
                timerSpinner.Stop();
                if (!tkspinner.IsCanceled && !tkspinner.IsCompleted)
                {
                    tkspinner.Wait(CancellationToken.None);
                }
                tkspinner.Dispose();

                ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);

                timerSpinner.Reset();
                foreach (var task in tasks)
                {
                    task.Item2.Dispose();
                }
                tasks.Clear();
                ClearLast();
            }
            while (i < _options.Steps.Count);
            ClearLast();
            ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);
        }

        private void ClearLast()
        {
            ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);
            for (int clsi = 0; clsi <= _promptlines; clsi++)
            {
                ConsolePlus.SetCursorPosition(0, _initialCursor.CursorTop+clsi);
                ConsolePlus.Write("", null, true);
            }
            ConsolePlus.SetCursorPosition(_initialCursor.CursorLeft, _initialCursor.CursorTop);
        }

        private void ShowSpinner(List<(int left, int top, Stopwatch sw)> elapsetm, Stopwatch timer, CancellationToken cancellationtoken)
        {
            while (timer.IsRunning && !cancellationtoken.IsCancellationRequested)
            {
                var qtdlines = 0;
                ConsolePlus.SetCursorPosition(_spinnerCursor.CursorLeft,_spinnerCursor.CursorTop);
                var spn = _options.Spinner.NextFrame(cancellationtoken);
                var top = ConsolePlus.CursorTop;
                var qtd = ConsolePlus.Write($"{spn}", _options.SpinnerStyle, false);
                //space for ShowElapsedTime
                qtd += ConsolePlus.Write("               ", _options.OptStyleSchema.Prompt(), true);
                if (ConsolePlus.IsTerminal && top + qtd >= ConsolePlus.BufferHeight)
                {
                    var dif = top + qtd - ConsolePlus.BufferHeight;
                    _initialCursor = (_initialCursor.CursorLeft, _initialCursor.CursorTop - dif);
                    _spinnerCursor = (_spinnerCursor.CursorLeft, _spinnerCursor.CursorTop - dif);
                    for (int i = 0; i < elapsetm.Count; i++)
                    {
                        elapsetm[i] = (elapsetm[i].left, elapsetm[i].top - dif, elapsetm[i].sw);
                    }
                }
                qtdlines += qtd;
                if (!_options.WaitTime)
                {
                    foreach (var item in elapsetm)
                    {
                        ConsolePlus.SetCursorPosition(item.left, item.top);
                        ConsolePlus.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b", _options.OptStyleSchema.Prompt(), false);
                        if (_options.ShowElapsedTime)
                        {
                            ConsolePlus.Write($" ({item.sw.Elapsed:hh\\:mm\\:ss\\:ff})", _options.OptStyleSchema.Description(), false);
                        }
                        if (!item.sw.IsRunning)
                        {
                            ConsolePlus.Write($" {_options.Symbol(SymbolType.Done)}", _options.OptStyleSchema.Description(), true);
                        }
                        else
                        {
                            ConsolePlus.Write($"  ", _options.OptStyleSchema.Description(), true);
                        }
                    }
                }
                else if(_options.ShowCountdown)
                {
                    var countdown = _options.TimeDelay - elapsetm[0].sw.Elapsed;
                    ConsolePlus.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b", _options.OptStyleSchema.Prompt(), false);
                    ConsolePlus.Write($"({countdown:hh\\:mm\\:ss\\:ff})", _options.OptStyleSchema.Description(), true);
                }
                if (_promptlines < _promptlines+qtdlines)
                {
                    _promptlines += qtdlines;
                }
            }
        }
    }
}
