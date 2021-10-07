// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

namespace PromptPlusControls.ValueObjects
{
    public class ResultProcess
    {
        internal ResultProcess(string id, object value, bool iscanceled, string textresult)
        {
            ProcessId = id;
            ValueProcess = value;
            IsCanceled = iscanceled;
            TextResult = textresult;
        }
        public string ProcessId { get; private set; }
        public object ValueProcess { get; private set; }
        public bool IsCanceled { get; private set; }
        public string TextResult { get; private set; }

    }
}
