// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class WaitProcessControl : ControlBase<IEnumerable<ResultProcess>>, IControlWaitProcess
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
        private const string Namecontrol = "PromptPlus.WaitProcess";

        public WaitProcessControl(WaitProcessOptions options) : base(Namecontrol, options, false, true)
        {
            _options = options;
        }

        public override string InitControl()
        {
            if (_options.Process == null)
            {
                throw new ArgumentException(nameof(_options.Process), Exceptions.Ex_WaitTaskToRun);
            }
            foreach (var item in _options.Process)
            {
                if (item.ProcessToRun == null)
                {
                    throw new ArgumentException(nameof(SingleProcess), Exceptions.Ex_WaitTaskToRun);
                }
            }

            if (_options.SpeedAnimation < 10)
            {
                _options.SpeedAnimation = 10;
            }
            if (_options.SpeedAnimation > 1000)
            {
                _options.SpeedAnimation = 1000;
            }

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("SpeedAnimation", _options.SpeedAnimation.ToString(), LogKind.Property);
                AddLog("Process", _options.Process.Count.ToString(), LogKind.Property);
            }
            return null;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out IEnumerable<ResultProcess> result)
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
                cancellationToken.WaitHandle.WaitOne(_options.SpeedAnimation);
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

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message, _options.HideSymbolPromptAndResult);
            if (_options.UpdateDescription != null)
            {
                _options.Description = _options.UpdateDescription.Invoke();
            }
            if (_options.Process.Count == 1)
            {
                screenBuffer.WriteAnswer(Messages.WaittingText);
                if (_localTask.Count > 0)
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
                if (HasDescription)
                {
                    if (!HideDescription)
                    {
                        screenBuffer.WriteLineDescription(_options.Description);
                    }
                    else
                    {
                        screenBuffer.WriteLineDescription(" ");
                    }
                    screenBuffer.ClearRestOfLine();
                }
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, !HasDescription);
                    screenBuffer.ClearRestOfLine();
                }
                return null;
            }

            var runningtasks = _localTask.Count;
            var waittasks = runningtasks - CountEndStaus(_localTask.Select(x => x.Status));
            screenBuffer.WriteAnswer(string.Format(Messages.WaittingProcess, waittasks, runningtasks));
            screenBuffer.PushCursor();
            screenBuffer.ClearRestOfLine();

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
                else
                {
                    screenBuffer.WriteLineDescription(" ");
                }
                screenBuffer.ClearRestOfLine();
            }
            if (_options.EnabledPromptTooltip)
            {
                screenBuffer.WriteLineProcessStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, !HasDescription);
                screenBuffer.ClearRestOfLine();
            }

            for (var i = 0; i < PromptPlus.MaxShowTasks; i++)
            {
                var pos = _pagevisible * PromptPlus.MaxShowTasks + _indexvisible + i;
                if (pos > _localTask.Count - 1)
                {
                    _indexvisible = 0;
                    _pagevisible = 0;
                    pos = i;
                }
                var item = _options.Process.Skip(pos).First();
                if (_localTask.Count > 0)
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
                        if (_options.HideSymbolPromptAndResult)
                        {
                            screenBuffer.Write($"{item.ProcessId} : ");
                        }
                        else
                        {
                            screenBuffer.WriteLineSymbolsDone();
                            screenBuffer.Write($" {item.ProcessId} : ");
                        }
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
            return null;
        }

        private static bool IsListEndStaus(IEnumerable<TaskStatus> status)
        {
            return CountEndStaus(status) == status.Count();
        }

        private static int CountEndStaus(IEnumerable<TaskStatus> status)
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

        private static bool IsEndStaus(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion => true,
                _ => false,
            };
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<ResultProcess> result)
        {
            screenBuffer.WriteDone(_options.Message, _options.HideSymbolPromptAndResult);
            if (result.Count() == 1)
            {
                FinishResult = result.First().TextResult;
            }
            else
            {
                FinishResult = string.Format(Messages.FinishResultTasks, result.Count());
            }
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region 

        public IControlWaitProcess Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlWaitProcess RefreshDescription(Func<string> value)
        {
            _options.UpdateDescription = value;
            return this;
        }


        public IControlWaitProcess AddProcess(SingleProcess process)
        {
            _options.Process.Merge(process);
            return this;
        }

        public IControlWaitProcess SpeedAnimation(int value)
        {
            _options.SpeedAnimation = value;
            if (_options.SpeedAnimation < 10)
            {
                _options.SpeedAnimation = 10;
            }
            if (_options.SpeedAnimation > 1000)
            {
                _options.SpeedAnimation = 1000;
            }
            return this;
        }

        public IControlWaitProcess Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion

    }
}
