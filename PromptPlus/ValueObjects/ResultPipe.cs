// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusControls.ValueObjects
{
    public struct ResultPipe
    {
        internal ResultPipe(string id, string title, object value, Func<ResultPipe[], object, bool> condition, StatusPipe? status = null)
        {
            PipeId = id;
            Title = title;
            ValuePipe = value;
            Status = status ?? StatusPipe.Waitting;
            Condition = condition;
        }

        internal ResultPipe UpdateStatus(StatusPipe value)
        {
            return new ResultPipe(PipeId, Title, ValuePipe, Condition, value);
        }

        internal ResultPipe UpdateValue(object value, StatusPipe status)
        {
            return new ResultPipe(PipeId, Title, value, Condition, status);
        }

        public string PipeId { get; }

        public string Title { get; }

        public StatusPipe Status { get; private set; }

        public object ValuePipe { get; }

        internal Func<ResultPipe[], object, bool> Condition { get; }
    }
}
