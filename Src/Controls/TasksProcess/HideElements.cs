using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    [Flags]
    public enum HideProgressBar
    {
        None = 0,
        Percent = 1,
        Delimit = 2,
        Ranger = 4
    }
}
