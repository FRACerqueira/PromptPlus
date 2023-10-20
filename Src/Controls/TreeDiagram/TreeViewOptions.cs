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

        private TreeViewOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("TreeViewOptions CTOR NotImplemented");
        }

        internal TreeViewOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            CurrentNodeStyle = styleSchema.TaggedInfo().Overflow(Overflow.Crop);
            ExpandStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            RootStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            ParentStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            ChildStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            SelectedRootStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedParentStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedChildStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedExpandStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            PageSize = config.PageSize;
            HotKeyFullPathNodePress = config.FullPathPress;
            HotKeyToggleExpandPress = config.ToggleExpandPress;
            HotKeyToggleExpandAllPress = config.ToggleExpandAllPress;
        }

        public FilterMode FilterType { get; set; } = FilterMode.Contains;
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
        public List<T> FixedSelected { get; set; } = new();
        public Func<T, bool>? ExpressionSelected { get; set; }
        public Func<T, bool>? ExpressionDisabled { get; set; }
        public bool SelectAll { get; set; }
        public Func<T, bool> SelectAllExpression { get; set; }
        public Style CurrentNodeStyle { get; set; }
        public Style ExpandStyle { get; set; }
        public Style RootStyle { get; set; }
        public Style ParentStyle { get; set; }
        public Style ChildStyle { get; set; }
        public Style SelectedRootStyle { get; set; }
        public Style SelectedParentStyle { get; set; }
        public Style SelectedChildStyle { get; set; }
        public Style SelectedExpandStyle { get; set; }
        public bool ShowLines { get; set; } = true;
        public bool ShowExpand { get; set; } = true;
        public int PageSize { get; set; }
        public Optional<T> DefautNode { get; set; } = Optional<T>.s_empty;
        public Func<T, string>? TextNode { get; set; }
        public char SeparatePath { get; set; } = '/';
        public bool ShowCurrentNode { get; set; } = true;
        public bool ShowCurrentFulPathNode { get; set; }
        public HotKey HotKeyFullPathNodePress { get; set; }
        public HotKey HotKeyToggleExpandPress { get; set; }
        public HotKey HotKeyToggleExpandAllPress { get; set; }
        public Action<T>? BeforeExpanded { get; set; }
        public Action<T>? AfterExpanded { get; set; }
        public Action<T>? BeforeCollapsed { get; set; }
        public Action<T>? AfterCollapsed { get; set; }
        public TreeOption<T> Nodes { get; set; } = new TreeOption<T>();
        public Func<T, string>? UniqueNode { get; set; }
        public bool ExpandAll { get; set; }
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
