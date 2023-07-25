using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal class PipelineControl<T> : BaseControl<ResultPipeline<T>>, IControlPipeline<T>
    {
        private readonly PipelineOptions<T> _options;
        private EventPipe<T> _currentevent;
        private ReadOnlyCollection<string> _pipes;
        private List<PipeRunningStatus> _runpipes;

        public PipelineControl(IConsoleControl console, PipelineOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        #region IControlPipeline

        public IControlPipeline<T> AddPipe(string idpipe, Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, bool> condition = null)
        {
            _options.AddPipe(idpipe, command);
            if (condition != null)
            {
                _options.Conditions.Add(idpipe, condition);
            }
            return this;
        }

        public IControlPipeline<T> AddPipe<TID>(TID idpipe, Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, bool> condition = null) where TID : Enum
        {
            _options.AddPipe(idpipe.ToString(), command);
            if (condition != null)
            {
                _options.Conditions.Add(idpipe.ToString(), condition);
            }
            return this;
        }

        public IControlPipeline<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        #endregion

        public override ResultPrompt<ResultPipeline<T>> TryResult(CancellationToken cancellationToken)
        {
            var index = _runpipes.FindIndex(x => x.Pipe == _currentevent.CurrentPipe);
            if (index != -1)
            {
                for (int i = index+1; i < _pipes.Count; i++)
                {
                    _runpipes[i] = new PipeRunningStatus(_runpipes[i].Pipe, PipeStatus.Waiting, TimeSpan.Zero);
                }
            }
            if (!_currentevent.CancelPipeLine)
            {
                _runpipes[index] = new PipeRunningStatus(_runpipes[index].Pipe, PipeStatus.Executed, _runpipes[index].Elapsedtime);
            }
            else
            {
                _runpipes[index] = new PipeRunningStatus(_runpipes[index].Pipe, PipeStatus.Canceled, _runpipes[index].Elapsedtime);
            }
            _currentevent = NextPipe(_currentevent, cancellationToken);
            return new ResultPrompt<ResultPipeline<T>>(new ResultPipeline<T>(_currentevent.Input,_runpipes.ToArray()), _currentevent.CancelPipeLine, _currentevent.CurrentPipe != null, false, false);
        }

        private EventPipe<T> NextPipe(EventPipe<T> curevent, CancellationToken cancellationToken)
        {
            if (curevent.ToPipe == null || curevent.CurrentPipe == null)
            {
                return new EventPipe<T>(curevent.Input, curevent.CurrentPipe, null, null, _pipes);
            }
            var from = curevent.CurrentPipe;
            var cur = curevent.ToPipe;
            string to = null;
            if (_pipes.IndexOf(cur) + 1 < _pipes.Count)
            {
                to = _pipes[_pipes.IndexOf(cur) + 1];
            }
            var newevent = new EventPipe<T>(curevent.Input, from, cur, to, _pipes);
            while (cur != null && _options.Conditions.ContainsKey(cur))
            {
                var sw = new Stopwatch();
                sw.Start();
                if (!_options.Conditions[cur].Invoke(newevent, cancellationToken))
                {
                    sw.Stop();
                    var index = _runpipes.FindIndex(x => x.Pipe == cur);
                    _runpipes[index] = new PipeRunningStatus(cur, PipeStatus.Jumped, sw.Elapsed);
                    newevent = NextPipe(newevent,cancellationToken);
                    cur = newevent.CurrentPipe;
                }
                else
                {
                    sw.Stop();
                    break;
                }
            }
            return newevent;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.Pipes.Count == 0)
            {
                _currentevent = new EventPipe<T>(_options.CurrentValue, null, null, null, _pipes);
                return string.Empty;
            }
            _runpipes = new List<PipeRunningStatus>();
            _pipes = _options.Pipes.Keys.ToList().AsReadOnly();
            _runpipes.AddRange(_pipes.Select(x => new PipeRunningStatus(x, PipeStatus.Waiting, TimeSpan.Zero)));
            var first = _pipes[0];
            string next = null;
            if (_pipes.Count > 1)
            {
                next = _pipes[1];
            }
            _currentevent = new EventPipe<T>(_options.CurrentValue, null, first, next, _pipes);
            while (first != null &&  _options.Conditions.ContainsKey(first))
            {
                var sw = new Stopwatch();
                sw.Start();
                if (!_options.Conditions[first].Invoke(_currentevent, cancellationToken))
                {
                    sw.Stop();
                    var index = _runpipes.FindIndex(x => x.Pipe == _currentevent.CurrentPipe);
                    _runpipes[index] = new PipeRunningStatus(_currentevent.CurrentPipe, PipeStatus.Jumped, sw.Elapsed);
                    _currentevent = NextPipe(_currentevent, cancellationToken);
                    first = _currentevent.CurrentPipe;
                }
                else
                {
                    sw.Stop();
                    break;
                }
            }
            return string.Empty;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_currentevent.CurrentPipe != null)
            {
                var sw = new Stopwatch();
                sw.Start();
                _options.Pipes[_currentevent.CurrentPipe].Invoke(_currentevent, CancellationToken);
                sw.Stop();
                var index = _runpipes.FindIndex(x => x.Pipe == _currentevent.CurrentPipe);
                _runpipes[index] = new PipeRunningStatus(_runpipes[index].Pipe, _runpipes[index].Status, sw.Elapsed);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultPipeline<T> result, bool aborted)
        {
            //none
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            //none
        }
    }
}
