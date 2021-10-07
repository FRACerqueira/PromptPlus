// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System.Collections.Generic;

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Options
{
    public class WaitProcessOptions : BaseOptions
    {
        public int SpeedAnimation { get; set; } = PromptPlus.SpeedAnimation;
        public IEnumerable<SingleProcess> Process { get; set; }
    }
}
