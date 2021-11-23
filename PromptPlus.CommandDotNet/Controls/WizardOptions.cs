using System;
using System.Collections.Generic;

using PPlus.Controls;
using PPlus.Objects;

namespace PPlus.CommandDotNet.Controls
{
    internal class WizardOptions: BaseOptions
    {
        public bool IsRootControl { get; set; } 
        public  HotKey Build { get; set; } = new HotKey(ConsoleKey.F5);
        public IFormPlusBase WizardControl { get; set; } = null;
        public IEnumerable<WizardArgs> TokenArgs { get; set; }
        public ConsoleColor? ForeColor { get; set; }
        public ConsoleColor? BackColor { get; set; }

    }
}
