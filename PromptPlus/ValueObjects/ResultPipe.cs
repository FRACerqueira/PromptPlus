// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;

namespace PromptPlus.ValueObjects
{
    public class ResultPipe
    {
        internal ResultPipe(string id, string title, object value, Func<ResultPipe[], object, bool> condition)
        {
            PipeId = id;
            Title = title;
            ValuePipe = value;
            Status = StatusPipe.Waitting;
            Condition = condition;
        }

        public string PipeId { get; private set; }

        public string Title { get; private set; }

        public StatusPipe Status { get; internal set; }

        public object ValuePipe { get; internal set; }

        internal Func<ResultPipe[], object, bool> Condition { get; set; }
    }
}
