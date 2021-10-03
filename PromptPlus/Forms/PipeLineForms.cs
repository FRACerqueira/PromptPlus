// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlus.Internal;
using PromptPlus.ValueObjects;

namespace PromptPlus.Forms
{
    internal class PipeLineForms : IDisposable
    {

        private string _currentPipe;
        private int _currentIndex;
        private readonly Dictionary<string, IFormPPlusBase> _steps = new();
        private readonly Dictionary<string, ResultPPlus<ResultPipe>> _resultpipeline = new();
        private readonly Paginator<ResultPPlus<ResultPipe>> _summaryPipePaginator;
        private readonly CancellationTokenSource _esckeyCts;

        public void Dispose()
        {
            if (_esckeyCts != null)
            {
                _esckeyCts.Dispose();
            }
            if (_summaryPipePaginator != null)
            {
                _summaryPipePaginator.Dispose();
            }
        }

        public PipeLineForms(IList<IFormPPlusBase> steps)
        {
            foreach (var item in steps)
            {
                _steps.Add(item.PipeId, item);
                _resultpipeline.Add(item.PipeId, new ResultPPlus<ResultPipe>(new ResultPipe(item.PipeId, item.PipeTitle, null, item.PipeCondition), false));
            }

            var resultpipes = new ResultPPlus<ResultPipe>[_steps.Count];
            _resultpipeline.Values.CopyTo(resultpipes, 0);
            _summaryPipePaginator = new Paginator<ResultPPlus<ResultPipe>>(resultpipes, null, Optional<ResultPPlus<ResultPipe>>.Create(null), (_) => string.Empty);
            _esckeyCts = new CancellationTokenSource();
        }

        public ResultPPlus<IEnumerable<ResultPipe>> Start(CancellationToken? stoptoken = null)
        {
            _currentIndex = 0;
            var abortedall = false;

            foreach (var item in _steps)
            {
                _currentPipe = item.Key;
                if ((stoptoken ?? CancellationToken.None).IsCancellationRequested || abortedall)
                {
                    _resultpipeline[item.Key].Value.Status = StatusPipe.Aborted;
                    _resultpipeline[item.Key].IsAborted = true;
                }
                else
                {
                    if (!_resultpipeline[_currentPipe].Value.Condition?.Invoke(_resultpipeline.Values.Select(x => x.Value).ToArray(), item.Value.ContextState) ?? false)
                    {
                        _resultpipeline[item.Key].Value.Status = StatusPipe.Skiped;
                    }
                }
                if (_resultpipeline[item.Key].Value.Status == StatusPipe.Waitting)
                {
                    _resultpipeline[item.Key].Value.Status = StatusPipe.Running;
                    using (item.Value)
                    {
                        var start = item.Value.GetType().UnderlyingSystemType.GetMethod("StartPipeline");
                        var result = start.Invoke(item.Value, new object[]
                        {
                                new Action<ScreenBuffer>( (screen) => { SummaryPipelineTemplate(screen); }),
                                _summaryPipePaginator,
                                _currentIndex,
                                stoptoken
                        });
                        _resultpipeline[_currentPipe].Value.ValuePipe = result;
                        abortedall = (bool)item.Value.GetType().UnderlyingSystemType.GetProperty("AbortedAll").GetValue(item.Value);
                        if (abortedall)
                        {
                            _resultpipeline[item.Key].IsAborted = true;
                            _resultpipeline[item.Key].Value.Status = StatusPipe.Aborted;
                        }
                        else
                        {
                            _resultpipeline[item.Key].Value.Status = StatusPipe.Done;
                        }
                    }
                }
                else
                {
                    item.Value.Dispose();
                }
                _currentIndex++;
            }
            return new ResultPPlus<IEnumerable<ResultPipe>>(_resultpipeline.Values.Select(x => x.Value).ToArray(), abortedall);
        }

        private void SummaryPipelineTemplate(ScreenBuffer screenBuffer)
        {

            screenBuffer.WritePrompt(Messages.PipelineText);
            screenBuffer.WriteAnswer($"{_currentIndex}/{_resultpipeline.Count}");

            screenBuffer.PushCursor();
            if (PPlus.EnabledStandardTooltip)
            {
                if (_summaryPipePaginator.PageCount > 1)
                {
                    screenBuffer.WriteLineHint($"{Messages.KeyNavPaging}{PPlus.ResumePipesKeyPress}: {Messages.SummaryPipelineReturnText} {_steps[_currentPipe].PipeTitle}");
                }
                else
                {
                    screenBuffer.WriteLineHint($"{PPlus.ResumePipesKeyPress}: {Messages.SummaryPipelineReturnText} {_steps[_currentPipe].PipeTitle}");
                }
            }

            var pagesteps = _summaryPipePaginator.ToSubset();
            foreach (var item in pagesteps)
            {
                if (item.Value.Status == StatusPipe.Skiped)
                {
                    screenBuffer.WriteLinePipeSkiped();
                    screenBuffer.Write($" {item.Value.Title} : ", PPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.SkipedText, PPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Aborted)
                {
                    screenBuffer.WriteLinePipeSkiped();
                    screenBuffer.Write($" {item.Value.Title} : ", PPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.CanceledText, PPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Running)
                {
                    screenBuffer.WriteLinePipeSelect();
                    screenBuffer.Write($" {item.Value.Title} : ", PPlus.ColorSchema.Select);
                    screenBuffer.Write(Messages.RunningText, PPlus.ColorSchema.Select);
                }
                else if (item.Value.Status == StatusPipe.Waitting)
                {
                    screenBuffer.WriteLinePipeDisabled();
                    screenBuffer.Write($" {item.Value.Title} : ", PPlus.ColorSchema.Disabled);
                    screenBuffer.Write(Messages.WaittingText, PPlus.ColorSchema.Disabled);
                }
                else if (item.Value.Status == StatusPipe.Done)
                {
                    screenBuffer.WriteLineSymbolsDone();
                    screenBuffer.Write($" {item.Value.Title} : ");
                    screenBuffer.Write(_steps[item.Value.PipeId].GetType().GetProperty("FinishResult").GetValue(_steps[item.Value.PipeId]).ToString(), PPlus.ColorSchema.Answer);
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
    }
}
