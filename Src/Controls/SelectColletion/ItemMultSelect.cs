﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class ItemMultSelect<T>
    {
        private readonly string _uniqueId;
        public ItemMultSelect()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }
        public string UniqueId => _uniqueId;
        public T Value { get; set; }
        public string Text { get; set; }
        public bool Disabled { get; set; }
        public string Group { get; set; }
        public bool IsGroupHeader { get; set; }
        public bool IsLastItemGroup { get; set; }
        public bool IsCheck { get; set; }

    }
}
