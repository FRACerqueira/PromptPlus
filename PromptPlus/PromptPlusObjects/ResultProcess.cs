// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusObjects
{
    public struct ResultProcess
    {
        internal ResultProcess(string id, object value, bool iscanceled, string textresult)
        {
            ProcessId = id;
            ValueProcess = value;
            IsCanceled = iscanceled;
            TextResult = textresult;
        }
        public string ProcessId { get; }
        public object ValueProcess { get; }
        public bool IsCanceled { get; }
        public string TextResult { get; }

    }
}
