using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using PPlus.Internal;

namespace PPlus.Objects
{
    public struct ControlLog
    {
        public ControlLog()
        {
            Logs = new List<ItemPromptLog>();
        }

        internal void Add(LogLevel level,string key, string message, string source, LogKind logKind)
        {
            Logs.Add(new ItemPromptLog(level,key, message, source,logKind));
        }

        public IList<ItemPromptLog> Logs { get; private set; }
    }
}
