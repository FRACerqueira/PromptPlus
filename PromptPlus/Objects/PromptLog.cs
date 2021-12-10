using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using PPlus.Internal;

namespace PPlus.Objects
{
    public struct ItemPromptLog
    {
        internal ItemPromptLog(LogLevel level,string key, string message, string source, LogKind logKind)
        {
            LogDate = DateTime.Now;
            Level = level;
            Key = key;
            Message = message;
            Source = source;
            Kind = logKind;
        }
        public DateTime LogDate { get; }
        public LogLevel Level { get; }
        public string Key { get; }
        public string Message { get; }
        public string Source { get; }
        internal LogKind Kind { get; }
    }
}
