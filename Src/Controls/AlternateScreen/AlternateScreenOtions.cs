// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PPlus.Controls
{
    internal class AlternateScreenOtions : BaseOptions
    {
        private AlternateScreenOtions()
        {
            throw new PromptPlusException("AlternateScreenOtions CTOR NotImplemented");
        }

        internal AlternateScreenOtions(bool showcursor) : base(showcursor)
        {
        }
        public Action<CancellationToken> CustomAction { get; set; }

        public ConsoleColor ForeColor { get; set; }

        public ConsoleColor BackColor { get; set; }

    }
}
