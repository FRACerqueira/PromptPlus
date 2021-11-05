// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;

namespace PromptPlusControls.Internal
{
    internal class ScreenBuffer : List<IList<TextInfo>>
    {
        public ScreenBuffer() : base(new List<IList<TextInfo>>() { new List<TextInfo>() })
        {
        }
    }
}
