// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Objects
{
    public struct ProgressBarInfo
    {
        public ProgressBarInfo()
        {
            PercentValue = 0;
            Finished = false;
            Message = "";
            InterationId = null;
        }

        public ProgressBarInfo(int percentValue, bool finished, string message, object interationId)
        {
            if (percentValue < 0 || percentValue > 100)
            {
                throw new ArgumentNullException(nameof(percentValue));
            }
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
