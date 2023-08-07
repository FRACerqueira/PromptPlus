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
        private KeyPressOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("KeyPressOptions CTOR NotImplemented");
        }

        internal KeyPressOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            SpinnerStyle = styleSchema.Prompt().Overflow(Overflow.Crop);
        }

        public Spinners? Spinner { get; set; }
        public Style SpinnerStyle { get; set; }
        public IList<ConsoleKeyInfo> KeyValids { get; set; } = new List<ConsoleKeyInfo>();
        public Func<ConsoleKeyInfo, string> TextKey { get; set; }
        public bool HotkeysIsKeypress { get; set; }
    }
}
