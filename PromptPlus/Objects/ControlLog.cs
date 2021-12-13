using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

using PPlus.Internal;

namespace PPlus.Objects
{
    public struct ControlLog
    {
        private string _source;

        public ControlLog()
        {
            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            _source = stackFrame.GetMethod().Name;
            Logs = new List<ItemPromptLog>();
        }

        public ControlLog([CallerMemberName] string? source = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            _source = source;
            Logs = new List<ItemPromptLog>();
        }

        internal void Add(LogLevel level,string key, string message, LogKind logKind)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (Logs.Any(x => x.Key == key && x.Kind == logKind))
            {
                var aux = Logs.First(x => x.Key == key && x.Kind == logKind);
                Logs.Remove(aux);
            }
            Logs.Add(new ItemPromptLog(level,key, message, _source,logKind));
        }

        public IList<ItemPromptLog> Logs { get; private set; }
    }
}
