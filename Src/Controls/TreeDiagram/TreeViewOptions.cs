// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class TreeViewOptions<T> : BaseOptions
    {

        private TreeViewOptions()
        {
            throw new PromptPlusException("TreeViewOptions CTOR NotImplemented");
        }

        internal TreeViewOptions(bool showcursor) : base(showcursor)
        {
        }

        public FilterMode FilterType { get; set; } = FilterMode.Contains;
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
        public List<T> FixedSelected { get; set; } = new();
        public Func<T, bool>? ExpressionSeleted { get; set; } = null;
        public Func<T, bool>? ExpressionDisabled { get; set; } = null;
        public bool SelectAll { get; set; } = false;
        public Func<T, bool> SelectAllExpression { get; set; } = null;
        public Style CurrentNodeStyle { get; set; } = PromptPlus.StyleSchema.TaggedInfo().Overflow(Overflow.Crop);
        public Style LineStyle { get; set; } = PromptPlus.StyleSchema.Prompt().Overflow(Overflow.Crop);
        public Style ExpandStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style RootStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style ParentStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style ChildStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style SelectedRootStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedParentStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedChildStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedExpandStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public bool ShowLines { get; set; } = true;
        public bool ShowExpand { get; set; } = true;
        public int PageSize { get; set; } = PromptPlus.Config.PageSize;
        public Optional<T> DefautNode { get; set; } = Optional<T>.s_empty;
        public Func<T, string>? TextNode { get; set; } = null;
        public char SeparatePath { get; set; } = '/';
        public bool ShowCurrentNode { get; set; } = true;
        public bool ShowCurrentFulPathNode { get; set; } = false;
        public HotKey HotKeyFullPathNodePress { get; set; } = PromptPlus.Config.FullPathPress;
        public HotKey HotKeyToggleExpandPress { get; set; } = PromptPlus.Config.ToggleExpandPress;
        public HotKey HotKeyToggleExpandAllPress { get; set; } = PromptPlus.Config.ToggleExpandAllPress;
        public Action<T>? BeforeExpanded { get; set; } = null;
        public Action<T>? AfterExpanded { get; set; } = null;
        public Action<T>? BeforeCollapsed { get; set; } = null;
        public Action<T>? AfterCollapsed { get; set; } = null;
        public TreeOption<T> Nodes { get; set; } = new TreeOption<T>();
        public Func<T, string>? UniqueNode { get; set; } = null;
        public bool ExpandAll { get; set; } = false;
        public TreeOption<T>? FindNode(TreeOption<T>? start, T value)
        {
            start ??= Nodes;
            TreeOption<T> currentnode = start;
            while (currentnode != null)
            {
                if (UniqueNode != null)
                {
                    if (UniqueNode(value) == UniqueNode(currentnode.Node))
                    {
                        return currentnode;
                    }

                }
                else if (currentnode.Node.Equals(value))
                {
                    return currentnode;
                }
                if (currentnode.Childrens != null)
                {
                    foreach (var child in currentnode.Childrens)
                    {
                        var aux = FindNode(child,value);
                        if (aux != null)
                        {
                            return aux;
                        }
                    }
                }
                break;
            }
            return null;
        }
    }
}
