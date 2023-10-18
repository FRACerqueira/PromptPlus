// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPlus.Controls
{
    internal class ListOptions : BaseOptions
    {
        private ListOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("AlternateScreenOtions CTOR NotImplemented");
        }

        internal ListOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console,bool showcursor) : base(styleSchema, config, console,showcursor)
        {
            RemoveItemPress = config.RemoveItemPress;
            EditItemPress = config.EditItemPress;
            PageSize = config.PageSize;
        }

        public string DefaultValue { get; set; }
        public HotKey RemoveItemPress { get; set; }
        public HotKey EditItemPress { get; set; }
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; }
        public ushort MaxLength { get; set; } = ushort.MaxValue;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<SuggestionInput, SuggestionOutput>? SuggestionHandler { get; set; }
        public IList<ItemListControl> Items { get; set; } = new List<ItemListControl>();
        public bool AllowDuplicate { get; set; }
        public int PageSize { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
        public Func<string, string> ChangeDescription { get; set; }
    }
}
