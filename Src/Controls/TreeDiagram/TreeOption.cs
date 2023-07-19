// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class TreeOption<T>
    {
        public TreeOption()
        {
        }
        public bool Selected { get; set; } = false;
        public bool Disabled { get; set; } = false;
        public T Node { get; set; }
        public List<TreeOption<T>> Childrens { get; set; } = null;
    }
}
