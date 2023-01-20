// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class SelectOptions<T> : BaseOptions
    {
        public IList<T> Items { get; set; } = new List<T>();
        public IList<T> HideItems { get; set; } = new List<T>();
        public IList<T> DisableItems { get; set; } = new List<T>();
        public T DefaultValue { get; set; }

        Func<T, T, bool> _founddefault = (x1,x2) => x1.ToString().ToLower() == x2.ToString().ToLower();
        public Func<T, T, bool> DefaultSelector
        {
            get { return _founddefault; }
            set
            {
                if (value == null)
                {
                    value =  (x1, x2) => x1.ToString().ToLower() == x2.ToString().ToLower(); 
                }
                _founddefault = value;
            }
        }
        public int? PageSize { get; set; }

        Func<T, string> _textSelector = x => x?.ToString();
        public Func<T, string> TextSelector
        {
            get { return _textSelector; }
            set
            {
                if (value == null)
                {
                    value = x => x?.ToString();
                }
                _textSelector = value;
            }
        }

        public Func<T, string> DescriptionSelector { get; set; } = null;
        public bool AutoSelectIfOne { get; set; }
    }
}
