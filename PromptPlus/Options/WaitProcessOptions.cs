// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromptPlusControls.Options
{
    public class WaitProcessOptions<T> : BaseOptions
    {
        public int SpeedAnimation { get; set; } = PromptPlus.SpeedAnimation;
        public IEnumerable<SingleProcess<T>> Process { get; set; }
        public Func<T, string> ProcessTextResult { get; set; } = x => x.ToString();
    }

    public class SingleProcess<T>
    {
        public string ProcessId { get; set; } = Guid.NewGuid().ToString();
        public Func<Task<T>> ProcessToRun { get; set; }
    }

}
