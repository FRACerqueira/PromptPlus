// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.WaitProcess
{
    internal sealed class WaitProcessControl : BaseControlPrompt<StateProcess[]>, IWaitProcessControl, IDisposable
    {
        private readonly Dictionary<WaitProcessStyles, Style> _optStyles = BaseControlOptions.LoadStyle<WaitProcessStyles>();
        private readonly List<ItemWaitTask> _tasks = [];
        private readonly List<int> _currenttasks = [];
        private readonly List<string> _toggerTooptips = [];
        private Func<IEnumerable<StateProcess>, string>? _finish;
        private Func<IEnumerable<StateProcess>, string>? _changeDescription;
        private bool _showElapsedTime;
        private int _intervalShowElapsedTime = 100;
        private Spinners? _spinner;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string? _currentspinnerFrame;
        private bool _hasupdateTaks;
        private byte _maxDegreeProcess = byte.MaxValue;
        private Timer? _timer;
        private CancellationTokenSource? _cancellationTokenSource;
        private DateTime _dateTimeref;
        private int _lasttask;
        private bool _isCompletedTaskRuning;
        private int _maxlabel;
        private bool _disposed;
        private bool _isUpdateTask;

        private readonly struct Job
        {
            public Action<object?, ExtraInfoProcess, CancellationToken> ActionJob { get; init; }
            public object? Paramjob { get; init; }
            public int IndexTask { get; init; }
        }


#pragma warning disable IDE0290 // Use primary constructor
        public WaitProcessControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
        }
#pragma warning restore IDE0290 // Use primary constructor


        #region IWaitProcessControl

        public IWaitProcessControl ChangeDescription(Func<IEnumerable<StateProcess>, string> value)
        {
            _changeDescription = value;
            return this;
        }

        public IWaitProcessControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IWaitProcessControl Styles(WaitProcessStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IWaitProcessControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        public IWaitProcessControl Finish(Func<IEnumerable<StateProcess>, string> finishtext)
        {
            _finish = finishtext;
            return this;
        }

        public IWaitProcessControl IntervalUpdate(int mileseconds = 100)
        {
            if (mileseconds < 100 || mileseconds > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(mileseconds), "Mileseconds must be between 100 and 1000.");
            }
            _intervalShowElapsedTime = mileseconds;
            return this;
        }
        public IWaitProcessControl ShowElapsedTime(bool value = true)
        {

            _showElapsedTime = value;
            return this;
        }

        public IWaitProcessControl MaxDegreeProcess(byte value)
        {
            if (value < 1)
            {
                _maxDegreeProcess = byte.MaxValue;
            }
            _maxDegreeProcess = value;
            if (_maxDegreeProcess > Environment.ProcessorCount)
            {
                _maxDegreeProcess = (byte)Environment.ProcessorCount;
            }
            if (_maxDegreeProcess > 10)
            {
                _maxDegreeProcess = 10;
            }
            return this;
        }

        public IWaitProcessControl Interaction<T>(IEnumerable<T> items, Action<T, IWaitProcessControl> interactionaction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionaction);

            foreach (T? item in items)
            {
                interactionaction.Invoke(item, this);
            }
            return this;
        }

        public IWaitProcessControl AddTask(TaskMode mode, string id, Action<object?, ExtraInfoProcess, CancellationToken> process, string? label = null, object? parameter = null)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(process);
            _tasks.Add(new ItemWaitTask(
                new StateProcess(mode, id, label, TaskStatus.Created, null, TimeSpan.Zero)
                , parameter, process));
            return this;
        }

        #endregion


        public void Dispose()
        {
            if (!_disposed)
            {
                FinalizeControl();
                _disposed = true;
            }
        }

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_maxDegreeProcess > Environment.ProcessorCount)
            {
                _maxDegreeProcess = (byte)Environment.ProcessorCount;
            }
            if (_maxDegreeProcess > 10)
            {
                _maxDegreeProcess = 10;
            }
            _tooltipModeInput = string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip);
            LoadTooltipToggle();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _lasttask = -1;
            LoadRunTasks();
            _timer = new Timer(UpdateElapsedTime, null, 0, _intervalShowElapsedTime);
        }

        private void LoadRunTasks()
        {
            _isUpdateTask = true;
            _lasttask++;
            if (_lasttask > _tasks.Count - 1)
            {
                _isUpdateTask = false;
                return;
            }
            int start = _lasttask;
            _hasupdateTaks = false;
            _maxlabel = 0;
            _currenttasks.Clear();
            for (int i = start; i < _tasks.Count; i++)
            {
                string label = _tasks[i].State.Label ?? string.Empty;
                if (string.IsNullOrEmpty(label))
                {
                    label = string.Format(Messages.TaskSeqName, "00");
                }
                if (_maxlabel < label.Length)
                {
                    _maxlabel = label.Length;
                }
                Job job = new()
                {
                    ActionJob = _tasks[i].ProcessAction,
                    Paramjob = _tasks[i].Parameter,
                    IndexTask = i
                };
                var hasinstance = false;
                _currenttasks.Add(i);
                _tasks[i].TaskRunning = Task.Run(() =>
                {
                    var upd = new ExtraInfoProcess(_tasks[i].State);
                    hasinstance = true;
                    try
                    {

                        job.ActionJob.Invoke(job.Paramjob, upd, _cancellationTokenSource!.Token);
                    }
                    catch (Exception ex)
                    {
                        _tasks[job.IndexTask].State.ExceptionProcess = ex;
                        _isUpdateTask = false;
                        throw;
                    }
                }, _cancellationTokenSource!.Token);
                while (!hasinstance)
                {
                    _cancellationTokenSource.Token.WaitHandle.WaitOne(10);
                }
                _lasttask = i;
                if (_tasks[i].State.RunMode == TaskMode.Sequential)
                {
                    break;
                }
                else
                {
                    if (_currenttasks.Count >= _maxDegreeProcess)
                    {
                        break;
                    }
                }
            }
            _isCompletedTaskRuning = false;
            _dateTimeref = DateTime.Now;
            _isUpdateTask = false;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTasks(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsoleKeyInfo? keyinfo = WaitKeypressTimer(cancellationToken);
                    if (_lasttask >= _tasks.Count - 1 && _isCompletedTaskRuning)
                    {
                        keyinfo = new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, true);
                    }

                    if (!keyinfo.HasValue)
                    {
                        UpdateStatusToCancel();
                        ResultCtrl = new ResultPrompt<StateProcess[]>([.. _tasks.Select(x => x.State)], true);
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.None)
                    {
                        if (_lasttask < _tasks.Count - 1 && _isCompletedTaskRuning)
                        {
                            _indexTooptip = 0;
                            LoadRunTasks();
                            CheckTooltipShowHideKeyPress(keyinfo.Value);
                            break;
                        }
                        break;
                    }
                    else if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.Control)
                    {
                        ResultCtrl = new ResultPrompt<StateProcess[]>([.. _tasks.Select(x => x.State)], false);
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo.Value))
                    {
                        UpdateStatusToCancel();
                        ResultCtrl = new ResultPrompt<StateProcess[]>([.. _tasks.Select(x => x.State)], true);
                        break;
                    }

                    if (IsTooltipToggerKeyPress(keyinfo!.Value))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo.Value))
                    {
                        _indexTooptip = 0;
                        break;
                    }

                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            else if (_finish is not null)
            {
                answer = _finish.Invoke(_tasks.Select(x => x.State));
            }
            else
            {
                int ok = _tasks.Count(x => x.TaskRunning is not null && x.TaskRunning.IsCompletedSuccessfully);
                int nok = _tasks.Count(x => x.TaskRunning is not null && x.State.ExceptionProcess is not null);
                int can = _tasks.Count(x => x.TaskRunning is null || x.State.Status == TaskStatus.Canceled);
                answer = string.Format(Messages.TaskFiniched, ok, nok, can);
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitProcessStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[WaitProcessStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            if (!_disposed)
            {
                _disposed = true;
                _timer?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }

        private void UpdateStatusToCancel()
        {
            _cancellationTokenSource!.Cancel();
            _cancellationTokenSource.Token.WaitHandle.WaitOne(1000);
            foreach (ItemWaitTask item in _tasks)
            {
                if (item.TaskRunning != null)
                {
                    if (!item.TaskRunning!.IsCompleted)
                    {
                        item.State.Status = item.TaskRunning.Status;
                    }
                }
                else
                {
                    item.State.Status = TaskStatus.Canceled;
                }
            }
        }

        private void UpdateStatusTasks()
        {
            if (_currenttasks.Count == 0 || _cancellationTokenSource!.Token.IsCancellationRequested)
            {
                return;
            }
            bool haschange = false;
            int qtd = 0;
            var localtask = _currenttasks.ToList();
            foreach (int pos in localtask)
            {
                if (_tasks[pos].TaskRunning is null)
                {
                    continue;
                }
                if (_tasks[pos].State.Status != _tasks[pos].TaskRunning!.Status)
                {
                    _tasks[pos].State.ElapsedTime = DateTime.Now.Subtract(_dateTimeref);
                    _tasks[pos].State.Status = _tasks[pos].TaskRunning!.Status;
                    haschange = true;
                }
                else if (_tasks[pos].TaskRunning!.Status == TaskStatus.Running)
                {
                    _tasks[pos].State.ElapsedTime = DateTime.Now.Subtract(_dateTimeref);
                    haschange = true;
                }
                if (_tasks[pos].TaskRunning!.IsCompleted)
                {
                    qtd++;
                }
            }
            if (_spinner?.HasNextFrame(out string? newframe) == true)
            {
                _currentspinnerFrame = newframe;
                haschange = true;
            }
            if (!_hasupdateTaks || (!_isCompletedTaskRuning && qtd == _currenttasks.Count))
            {
                haschange = true;
            }
            _hasupdateTaks = haschange;
            _isCompletedTaskRuning = (qtd == _currenttasks.Count);

        }

        private void UpdateElapsedTime(object? state)
        {
            if (!_isUpdateTask)
            {
                UpdateStatusTasks();
            }
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
                ];

            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            _toggerTooptips.Clear();
            _toggerTooptips.AddRange(lsttooltips);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[WaitProcessStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            string answer = string.Format(Messages.TaksRunning, _currenttasks.Count);
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft), _optStyles[WaitProcessStyles.Answer]);
            screenBuffer.Write(answer, _optStyles[WaitProcessStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight), _optStyles[WaitProcessStyles.Answer]);
            if (_currentspinnerFrame != null)
            {
                screenBuffer.Write($" {_currentspinnerFrame} ", _optStyles[WaitProcessStyles.Spinner]);
            }
            screenBuffer.WriteLine("", _optStyles[WaitProcessStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string desc = GeneralOptions.DescriptionValue ?? string.Empty;
            if (_changeDescription != null)
            {
                desc = _changeDescription!.Invoke(_tasks.Select(x => x.State));
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[WaitProcessStyles.Description]);
            }
        }

        private void WriteTasks(BufferScreen screenBuffer)
        {
            if (_currenttasks.Count == 0)
            {
                return;
            }
            foreach (int pos in _currenttasks)
            {
                Style styleitem = _tasks[pos].State.Status switch
                {
                    TaskStatus.Running => _optStyles[WaitProcessStyles.Selected],
                    TaskStatus.RanToCompletion => _optStyles[WaitProcessStyles.Answer],
                    TaskStatus.Faulted => _optStyles[WaitProcessStyles.Error],
                    _ => _optStyles[WaitProcessStyles.UnSelected],
                };
                if (_tasks[pos].State.Status == TaskStatus.RanToCompletion)
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Done)}", styleitem);
                }
                else if (_tasks[pos].State.Status == TaskStatus.Canceled)
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Canceled)}", styleitem);
                }
                else if (_tasks[pos].State.Status == TaskStatus.Faulted)
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Error)}", styleitem);
                }
                else
                {
                    screenBuffer.Write(" ", styleitem);
                }

                string label = _tasks[pos].State.Label ?? string.Empty;
                if (string.IsNullOrEmpty(label))
                {
                    label = string.Format(Messages.TaskSeqName, _tasks[pos].TaskRunning!.Id);
                }
                screenBuffer.Write($" {label.PadRight(_maxlabel, ' ')}", styleitem);

                if (_showElapsedTime)
                {
                    screenBuffer.Write($" ({_tasks[pos].State.ElapsedTime:hh\\:mm\\:ss\\:ff})", _optStyles[WaitProcessStyles.ElapsedTime]);
                }
                if (!string.IsNullOrEmpty(_tasks[pos].State.DynamicInfo))
                {
                    screenBuffer.Write($" ({_tasks[pos].State.DynamicInfo})", _optStyles[WaitProcessStyles.TaggedInfo]);
                }
                screenBuffer.WriteLine("", styleitem);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip = _tooltipModeInput;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            screenBuffer.Write(tooltip!, _optStyles[WaitProcessStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private ConsoleKeyInfo? WaitKeypressTimer(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_hasupdateTaks)
                {
                    _hasupdateTaks = false;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(true) : null;
        }
    }
}
