using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls.AlternateScreen
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

    }
}
