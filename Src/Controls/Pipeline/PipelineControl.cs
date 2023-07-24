using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PPlus.Controls.Objects;

namespace PPlus.Controls.Pipeline
{
    internal class PipelineControl<T> : BaseControl<T>, IControlPipeline<T>
    {
        private readonly PipelineOptions<T> _options;
        private EventPipe<T> _currentevent;
        private ReadOnlyCollection<string> _pipes;

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

        public override ResultPrompt<T> TryResult(CancellationToken cancellationToken)
        {
            _currentevent = NextPipe(_currentevent,cancellationToken);
            return new ResultPrompt<T>(_currentevent.Input, _currentevent.CancelPipeLine, _currentevent.CurrentPipe != null, false, false);
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
                if (!_options.Conditions[cur].Invoke(newevent, cancellationToken))
                {
                    newevent = NextPipe(newevent,cancellationToken);
                    cur = newevent.CurrentPipe;
                }
                else
                {
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

            _pipes = _options.Pipes.Keys.ToList().AsReadOnly();
            var first = _pipes[0];
            string next = null;
            if (_pipes.Count > 1)
            {
                next = _pipes[1];
            }
            _currentevent = new EventPipe<T>(_options.CurrentValue, null, first, next, _pipes);
            while (first != null &&  _options.Conditions.ContainsKey(first))
            {
                if (!_options.Conditions[first].Invoke(_currentevent, cancellationToken))
                {
                    _currentevent = NextPipe(_currentevent, cancellationToken);
                    first = _currentevent.CurrentPipe;
                }
                else
                {
                    break;
                }
            }
            return string.Empty;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_currentevent.CurrentPipe != null)
            {
                _options.Pipes[_currentevent.CurrentPipe].Invoke(_currentevent, CancellationToken);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T result, bool aborted)
        {
            //none
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            //none
        }
    }
}
