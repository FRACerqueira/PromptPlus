// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Controls.Table
{
    internal sealed class ColumnDefinition<T>(
        string header,
        Func<T, object> selector,
        Func<object, string>? formatter,
        int? width,
        ColumnAlignment alignment,
        bool isFilterable)
    {
        public string Header => header;
        public Func<T, object> Selector => selector;
        public Func<object, string>? Formatter => formatter;
        public int? Width => width;
        public int CalculatedWidth { get; set; }
        public ColumnAlignment Alignment => alignment;
        public bool IsFilterable => isFilterable;
    }
}
