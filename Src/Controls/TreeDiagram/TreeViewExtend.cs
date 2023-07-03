// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Linq;

namespace PPlus.Controls
{
    internal static class TreeViewExtend
    {
        public static bool IsParentLast<T>(this TreeNode<T> item, bool islast = false)
        {
            var node = item.Parent;
            if (node == null)
            {
                return islast;
            }
            if (node.Childrens.Last().UniqueId == item.UniqueId)
            {
                return IsParentLast(node, true);
            }
            return false;
        }

        public static bool HasAnyNotExpand<T>(this TreeNode<T> node)
        {
            var found = false;
            if (node.Childrens != null)
            {
                foreach (var child in node.Childrens.Where(x => x.Childrens != null))
                {
                    found = HasAnyNotExpand(child);
                    if (found)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (node.Childrens != null && !node.IsExpanded)
                {
                    found = true;
                }
            }
            return found;
        }

        public static TreeNode<T>? FindTreeByValue<T>(this TreeNode<T> node, T value, Func<T, string> UniqueNode= null)
        {
            TreeNode<T> currentnode = node;
            while (currentnode != null)
            {
                if (UniqueNode != null)
                {
                    if (UniqueNode(currentnode.Value) == UniqueNode(value))
                    {
                        return currentnode;
                    }
                }
                else
                {
                    if (currentnode.Value.Equals(value))
                    {
                        return currentnode;
                    }
                }
                if (currentnode.IsHasChild)
                {
                    foreach (var child in currentnode.Childrens)
                    {
                        var aux = FindTreeByValue(child,value);
                        if (aux != null)
                        {
                            return aux;
                        }
                    }
                }
                currentnode = currentnode.NextNode;
            }
            return null;
        }

        public static void UpdateTreeLenght<T>(this TreeNode<ItemBrowser> node)
        {
            var currentnode = node;
            while (currentnode.PrevNode != null)
            {
                if (currentnode.IsHasChild)
                {
                    currentnode.Value.Length = currentnode.CountAllChildrens();
                }
                currentnode = currentnode.PrevNode;
            }
        }

        public static string BytesToString(this TreeNode<ItemBrowser> node)
        {
            string[] suf = { "", " KB", " MB", " GB", " TB", " PB", " EB" }; //Longs run out around EB
            if (node.Value.Length == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(node.Value.Length);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(node.Value.Length) * num).ToString() + suf[place];
        }

        public static int CountAllChildrens<T>(this TreeNode<T> node)
        {
            int count = node.Childrens == null ? 0 : node.Childrens.Count;
            if (count > 0)
            {
                foreach (var item in node.Childrens.Where(x => x.IsHasChild))
                {
                    count += CountAllChildrens(item);
                }
            }
            return count;
        }
    }
}
