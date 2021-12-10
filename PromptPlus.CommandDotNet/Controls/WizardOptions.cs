using System;
using System.Collections.Generic;

using CommandDotNet;

using PPlus.Controls;
using PPlus.Objects;

namespace PPlus.CommandDotNet.Controls
{
    internal class WizardOptions: BaseOptions
    {
        public bool IsRootControl { get; set; }
        public HotKey Build { get; set; } = new HotKey(UserHotKey.F5);
        public HotKey BackCommand { get; set; } = new HotKey(UserHotKey.F4);
        public IFormPlusBase WizardControl { get; set; } = null;
        public IEnumerable<WizardArgs> TokenArgs { get; set; }
        public ConsoleColor ForeColor { get; set; }
        public ConsoleColor BackColor { get; set; }
        public ConsoleColor MissingForeColor { get; set; }
        public bool EnabledBackCommand { get; set; }
        public Command RootCommand { get; set; }

    }
}
