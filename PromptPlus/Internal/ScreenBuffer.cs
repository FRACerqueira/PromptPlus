// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

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
