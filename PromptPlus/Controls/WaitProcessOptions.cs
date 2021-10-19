// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class WaitProcessOptions : BaseOptions
    {
        public int SpeedAnimation { get; set; } = PromptPlus.SpeedAnimation;
        public IList<SingleProcess> Process { get; set; } = new List<SingleProcess>();
    }
}
