// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusObjects
{
    public struct ProgressBarInfo
    {
        public ProgressBarInfo(int percentValue, bool finished, string message, object interationId)
        {
            PercentValue = percentValue;
            Finished = finished;
            Message = message;
            InterationId = interationId;
        }
        public bool Finished { get; }
        public int PercentValue { get; }
        public string Message { get; }
        public object InterationId { get; }
    }
}
