// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class KeyPressOptions : BaseOptions
    {
        private KeyPressOptions()
        {
            throw new PromptPlusException("KeyPressOptions CTOR NotImplemented");
        }

        internal KeyPressOptions(bool showcursor) : base(showcursor)
        {
        }

        public Spinners? Spinner { get; set; } = null;
        public Style SpinnerStyle { get; set; } = PromptPlus.StyleSchema.Prompt().Overflow(Overflow.Crop);
        public IList<ConsoleKeyInfo> KeyValids { get; set; } = new List<ConsoleKeyInfo>();
        public Func<ConsoleKeyInfo, string> TextKey { get; set; } = null;
        public bool HotkeysIsKeypress { get; set; }
    }
}
