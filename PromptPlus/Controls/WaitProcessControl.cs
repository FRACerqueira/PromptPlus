// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class WaitProcessControl : ControlBase<IEnumerable<ResultProcess>>
    {
        private const string Twirl = "|/-\\";
        private bool _notstart = true;
        private int _countRotate;
        private int _indexvisible;
        private int _pagevisible;
        private readonly WaitProcessOptions _options;
        private readonly Dictionary<string, ResultProcess> _lastresult = new();
        private readonly List<int> _index = new();
        private readonly List<Task> _localTask = new();

        public WaitProcessControl(WaitProcessOptions options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes, true)
        {
            if (options.Process == null)
            {
                throw new ArgumentException(nameof(options.Process), Exceptions.Ex_WaitTaskToRun);
            }
            foreach (var item in options.Process)
            {
                if (item == null)
                {
                    throw new ArgumentException(nameof(SingleProcess), Exceptions.Ex_WaitTaskToRun);
                }
            }

            _options = options;
            if (_options.SpeedAnimation < 10)
            {
                _options.SpeedAnimation = 10;
            }
            if (_options.SpeedAnimation > 1000)
            {
                _options.SpeedAnimation = 1000;
            }
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out IEnumerable<ResultProcess> result)
        {
            if (!summary && CheckDefaultKey(GetKeyAvailable(cancellationToken)))
            {
                result = _lastresult.Values;
                if (!SummaryPipeLine)
                {
                    Task.WaitAll(_localTask.ToArray());
                }
                if (PipeId != null || AbortedAll)
                {
                    return null;
                }
                return false;
            }
            if (_notstart)
            {
                foreach (var item in _options.Process)
                {
                    _index.Add(-1);
                    _lastresult.Add(item.ProcessId, default);
                    _localTask.Add(Task.Run(() =>
                    {
                        var itemtask = item.ProcessToRun.Invoke(cancellationToken).Result;
                        //for asyn task
                        if (itemtask as Task<object> != null)
                        {
                            itemtask = (itemtask as Task<object>).Result;
                        }
                        var aux = item.ProcessTextResult(itemtask);
                        _lastresult[item.ProcessId] = new ResultProcess(item.ProcessId, itemtask, cancellationToken.IsCancellationRequested, aux);
                    }, cancellationToken));
                }

                _notstart = false;
            }
            if (!summary)
            {
                Thread.Sleep(_options.SpeedAnimation);
            }
            if (IsListEndStaus(_localTask.Select(x => x.Status)))
            {
                result = _lastresult.Values;
                _localTask.ForEach(x => x.Dispose());
                _localTask.Clear();
                _index.Clear();
                return true;
            }
            result = _lastresult.Values;
            return false;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            if (_options.Process.Count() == 1)
            {
                screenBuffer.WriteAnswer(Messages.WaittingText);
                if (_localTask.Count() > 0)
                {
                    _index[0]++;
                    if (_index[0] > 3)
                    {
                        _index[0] = 0;
                    }
                    screenBuffer.WriteAnswer($" {Twirl[_index[0]]}");
                }
                else
                {
                    screenBuffer.Write("...");
                }
                screenBuffer.PushCursor();
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey);
                }
                return;
            }

            var runningtasks = _localTask.Count();
            var waittasks = runningtasks - CountEndStaus(_localTask.Select(x => x.Status));
            screenBuffer.WriteAnswer(string.Format(Messages.WaittingProcess, waittasks, runningtasks));
            screenBuffer.PushCursor();
            screenBuffer.ClearRestOfLine();
            if (_options.EnabledPromptTooltip)
            {
                screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey);
            }

            for (var i = 0; i < PromptPlus.MaxShowTasks; i++)
            {
                var pos = _pagevisible * PromptPlus.MaxShowTasks + _indexvisible + i;
                if (pos > _localTask.Count() - 1)
                {
                    _indexvisible = 0;
                    _pagevisible = 0;
                    pos = i;
                }
                var item = _options.Process.Skip(pos).First();
                if (_localTask.Count() > 0)
                {
                    if (!IsEndStaus(_localTask[pos].Status))
                    {
                        screenBuffer.WriteLineTaskRun($"{item.ProcessId} : ");
                        _index[pos]++;
                        if (_index[pos] > 3)
                        {
                            _index[pos] = 0;
                        }
                        screenBuffer.WriteAnswer($" {Twirl[_index[pos]]}");
                    }
                    else
                    {
                        screenBuffer.WriteLineSymbolsDone();
                        screenBuffer.Write($" {item.ProcessId} : ");
                        screenBuffer.WriteAnswer(_lastresult[item.ProcessId].TextResult);
                    }
                }
                else
                {
                    screenBuffer.WriteLineTaskRun($"{item.ProcessId} : ");
                    screenBuffer.Write("...");
                }
                screenBuffer.ClearRestOfLine();


                _countRotate++;
                if (_countRotate > int.MaxValue - 1)
                {
                    _countRotate = 0;
                }
                if (_countRotate % PromptPlus.RollupFactor == 0)
                {
                    _indexvisible++;
                    if (_indexvisible >= PromptPlus.MaxShowTasks)
                    {
                        _indexvisible = 0;
                        _pagevisible++;
                    }
                }
            }
        }

#pragma warning disable IDE0066 // Convert switch statement to expression
        private bool IsListEndStaus(IEnumerable<TaskStatus> status)
        {
            return CountEndStaus(status) == status.Count();
        }

        private int CountEndStaus(IEnumerable<TaskStatus> status)
        {
            var qtd = 0;
            foreach (var item in status)
            {
                if (IsEndStaus(item))
                {
                    qtd++;
                }
            }
            return qtd;
        }

        private bool IsEndStaus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                case TaskStatus.RanToCompletion:
                    return true;
                default:
                    return false;
            }
        }
#pragma warning restore IDE0066 // Convert switch statement to expression

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<ResultProcess> result)
        {
            screenBuffer.WriteDone(_options.Message);
            if (result.Count() == 1)
            {
                FinishResult = result.First().TextResult;
            }
            else
            {
                FinishResult = string.Join(", ", result.Select(x => x.TextResult));
            }
            screenBuffer.WriteAnswer(FinishResult);
        }

    }
}
