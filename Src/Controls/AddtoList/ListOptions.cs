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
        private ListOptions()
        {
            throw new NotImplementedException();
        }

        internal ListOptions(bool showcursor) : base(showcursor)
        {
        }

        public string DefaultValue { get; set; }
        public HotKey RemoveItemPress { get; set; } = PromptPlus.Config.RemoveItemPress;
        public HotKey EditItemPress { get; set; } = PromptPlus.Config.EditItemPress;
        public CaseOptions InputToCase { get; set; } = CaseOptions.Any;
        public Func<char, bool>? AcceptInput { get; set; } = null;
        public ushort MaxLenght { get; set; } = ushort.MaxValue;
        public IList<Func<object, ValidationResult>> Validators { get; } = new List<Func<object, ValidationResult>>();
        public Func<SugestionInput, SugestionOutput>? SuggestionHandler { get; set; }
        public IList<ItemListControl> Items { get; set; } = new List<ItemListControl>();
        public bool AllowDuplicate { get; set; } = false;
        public int PageSize { get; set; } = PromptPlus.Config.PageSize;
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = int.MaxValue;
    }
}
