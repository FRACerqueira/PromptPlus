// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class PipeLineControl : IDisposable, IControlPipeLine
    {

        private string _currentPipe;
        private int _currentIndex;
        private readonly Dictionary<string, IFormPlusBase> _steps = new();
        private readonly Dictionary<string, ResultPromptPlus<ResultPipe>> _resultpipeline = new();
        private Paginator<ResultPromptPlus<ResultPipe>> _summaryPipePaginator;
        private CancellationTokenSource _esckeyCts;

        public void Dispose()
        {
            if (_esckeyCts != null)
            {
                _esckeyCts.Dispose();
            }
        }

        public void InitControl()
        {
            _currentIndex = 0;
            var resultpipes = new ResultPromptPlus<ResultPipe>[_steps.Count];
            _resultpipeline.Values.CopyTo(resultpipes, 0);
            _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(resultpipes, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
            _esckeyCts = new CancellationTokenSource();
        }

        public ResultPromptPlus<IEnumerable<ResultPipe>> Start(CancellationToken? stoptoken = null)
        {
            var abortedall = false;
            foreach (var item in _steps)
            {
                _currentPipe = item.Key;
                if ((stoptoken ?? CancellationToken.None).IsCancellationRequested || abortedall)
                {
                    _resultpipeline[item.Key] = new ResultPromptPlus<ResultPipe>(_resultpipeline[item.Key].Value.UpdateStatus(StatusPipe.Aborted), true);
                    _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(_resultpipeline.Values, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
                }
                else
                {
                    if (!_resultpipeline[_currentPipe].Value.Condition?.Invoke(_resultpipeline.Values.Select(x => x.Value).ToArray(), item.Value.ContextState) ?? false)
                    {
                        _resultpipeline[item.Key] = new ResultPromptPlus<ResultPipe>(_resultpipeline[item.Key].Value.UpdateStatus(StatusPipe.Skiped), false);
                        _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(_resultpipeline.Values, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
                    }
                }
                if (_resultpipeline[item.Key].Value.Status == StatusPipe.Waitting)
                {
                    _resultpipeline[item.Key] = new ResultPromptPlus<ResultPipe>(_resultpipeline[item.Key].Value.UpdateStatus(StatusPipe.Running), false);
                    _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(_resultpipeline.Values, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
                    using (item.Value)
                    {
                        item.Value.GetType().UnderlyingSystemType.GetMethod("InitControl").Invoke(item.Value, null);

                        var start = item.Value.GetType().UnderlyingSystemType.GetMethod("StartPipeline");
                        var result = start.Invoke(item.Value, new object[]
                        {
                                new Action<ScreenBuffer>( (screen) => { SummaryPipelineTemplate(screen); }),
                                _summaryPipePaginator,
                                _currentIndex,
                                stoptoken
                        });
                        abortedall = (bool)item.Value.GetType().UnderlyingSystemType.GetProperty("AbortedAll").GetValue(item.Value);
                        if (abortedall)
                        {
                            _resultpipeline[item.Key] = new ResultPromptPlus<ResultPipe>(_resultpipeline[item.Key].Value.UpdateValue(result, StatusPipe.Aborted), true);
                        }
                        else
                        {
                            _resultpipeline[item.Key] = new ResultPromptPlus<ResultPipe>(_resultpipeline[item.Key].Value.UpdateValue(result, StatusPipe.Done), false);
                        }
                        _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(_resultpipeline.Values, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
                    }
                }
                else
                {
                    item.Value.Dispose();
                }
                _summaryPipePaginator = new Paginator<ResultPromptPlus<ResultPipe>>(_resultpipeline.Values, null, Optional<ResultPromptPlus<ResultPipe>>.Create(null), (_) => string.Empty);
                _currentIndex++;
            }
            return new ResultPromptPlus<IEnumerable<ResultPipe>>(_resultpipeline.Values.Select(x => x.Value).ToArray(), abortedall);
        }

        private void SummaryPipelineTemplate(ScreenBuffer screenBuffer)
        {

            screenBuffer.WritePrompt(Messages.PipelineText);
            screenBuffer.WriteAnswer($"{_currentIndex}/{_resultpipeline.Count}");

            screenBuffer.PushCursor();
            if (PromptPlus.EnabledStandardTooltip)
            {
                if (_summaryPipePaginator.PageCount > 1)
                {
                    screenBuffer.WriteLineHint($"{Messages.KeyNavPaging}{PromptPlus.ResumePipesKeyPress}: {Messages.SummaryPipelineReturnText} {_steps[_currentPipe].PipeTitle}");
                }
                else
                {
                    screenBuffer.WriteLineHint($"{PromptPlus.ResumePipesKeyPress}: {Messages.SummaryPipelineReturnText} {_steps[_currentPipe].PipeTitle}");
                }
            }

            var pagesteps = _summaryPipePaginator.ToSubset();
            foreach (var item in pagesteps)
            {
                if (item.Value.Status == StatusPipe.Skiped)
                {
                    screenBuffer.WriteLinePipeSkiped();
                    screenBuffer.Write($" {item.Value.Title} : ", PromptPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.SkipedText, PromptPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Aborted)
                {
                    screenBuffer.WriteLinePipeSkiped();
                    screenBuffer.Write($" {item.Value.Title} : ", PromptPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.CanceledText, PromptPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Running)
                {
                    screenBuffer.WriteLinePipeSelect();
                    screenBuffer.Write($" {item.Value.Title} : ", PromptPlus.ColorSchema.Select);
                    screenBuffer.Write(Messages.RunningText, PromptPlus.ColorSchema.Select);
                }
                else if (item.Value.Status == StatusPipe.Waitting)
                {
                    screenBuffer.WriteLinePipeDisabled();
                    screenBuffer.Write($" {item.Value.Title} : ", PromptPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.WaittingText, PromptPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Done)
                {
                    screenBuffer.WriteLineSymbolsDone();
                    screenBuffer.Write($" {item.Value.Title} : ");
                    screenBuffer.Write(_steps[item.Value.PipeId].GetType().GetProperty("FinishResult").GetValue(_steps[item.Value.PipeId]).ToString(), PromptPlus.ColorSchema.Answer);
                }
                else
                {
                }
            }
            if (_summaryPipePaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_summaryPipePaginator.PaginationMessage());
            }
        }

        #region IControlPipeLine

        public IControlPipeLine AddPipe(IFormPlusBase value)
        {
            _steps.Add(value.PipeId, value);
            _resultpipeline.Add(value.PipeId, new ResultPromptPlus<ResultPipe>(new ResultPipe(value.PipeId, value.PipeTitle, null, value.Condition), false));
            return this;
        }

        public IControlPipeLine AddPipes(IEnumerable<IFormPlusBase> value)
        {
            foreach (var item in value)
            {
                if (string.IsNullOrEmpty(item.PipeId))
                {
                    throw new ArgumentException(Exceptions.EX_PipeLineEmptyId);
                }
                _steps.Add(item.PipeId, item);
                _resultpipeline.Add(item.PipeId, new ResultPromptPlus<ResultPipe>(new ResultPipe(item.PipeId, item.PipeTitle, null, item.Condition), false));
            }
            return this;
        }

        public ResultPromptPlus<IEnumerable<ResultPipe>> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        #endregion
    }
}
