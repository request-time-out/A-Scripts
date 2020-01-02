// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeViewExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;

namespace Battlehub.UIControls
{
  public static class VirtualizingTreeViewExtension
  {
    public static void ExpandTo<T>(
      this VirtualizingTreeView treeView,
      T item,
      Func<T, T> getParent)
    {
      if ((object) item == null)
        return;
      if (treeView.GetItemContainerData((object) item) == null)
      {
        treeView.ExpandTo<T>(getParent(item), getParent);
        treeView.Expand((object) item);
      }
      else
        treeView.Expand((object) item);
    }

    public static void ExpandChildren<T>(
      this VirtualizingTreeView treeView,
      T item,
      Func<T, IEnumerable> getChildren)
    {
      IEnumerable enumerable = getChildren(item);
      if (enumerable == null)
        return;
      treeView.Expand((object) item);
      foreach (T obj in enumerable)
        treeView.ExpandChildren<T>(obj, getChildren);
    }

    public static void ExpandAll<T>(
      this VirtualizingTreeView treeView,
      T item,
      Func<T, T> getParent,
      Func<T, IEnumerable> getChildren)
    {
      treeView.ExpandTo<T>(getParent(item), getParent);
      treeView.ExpandChildren<T>(item, getChildren);
    }

    public static void ItemDropStdHandler<T>(
      this VirtualizingTreeView treeView,
      ItemDropArgs e,
      Func<T, T> getParent,
      Action<T, T> setParent,
      Func<T, T, int> indexOfChild,
      Action<T, T> removeChild,
      Action<T, T, int> insertChild)
      where T : class
    {
      T dropTarget = (T) e.DropTarget;
      if (e.Action == ItemDropAction.SetLastChild)
      {
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          T dragItem = (T) e.DragItems[index];
          removeChild(dragItem, getParent(dragItem));
          setParent(dragItem, dropTarget);
          insertChild(dragItem, getParent(dragItem), 0);
        }
      }
      else if (e.Action == ItemDropAction.SetNextSibling)
      {
        for (int index = e.DragItems.Length - 1; index >= 0; --index)
        {
          T dragItem = (T) e.DragItems[index];
          int num1 = indexOfChild(dropTarget, getParent(dropTarget));
          if ((object) getParent(dragItem) != (object) getParent(dropTarget))
          {
            removeChild(dragItem, getParent(dragItem));
            setParent(dragItem, getParent(dropTarget));
            insertChild(dragItem, getParent(dragItem), num1 + 1);
          }
          else
          {
            int num2 = indexOfChild(dragItem, getParent(dragItem));
            if (num1 < num2)
            {
              removeChild(dragItem, getParent(dragItem));
              insertChild(dragItem, getParent(dragItem), num1 + 1);
            }
            else
            {
              removeChild(dragItem, getParent(dragItem));
              insertChild(dragItem, getParent(dragItem), num1);
            }
          }
        }
      }
      else
      {
        if (e.Action != ItemDropAction.SetPrevSibling)
          return;
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          T dragItem = (T) e.DragItems[index];
          if ((object) getParent(dragItem) != (object) getParent(dropTarget))
          {
            removeChild(dragItem, getParent(dragItem));
            setParent(dragItem, getParent(dropTarget));
            insertChild(dragItem, getParent(dragItem), 0);
          }
          int num1 = indexOfChild(dropTarget, getParent(dropTarget));
          int num2 = indexOfChild(dragItem, getParent(dragItem));
          if (num1 > num2)
          {
            removeChild(dragItem, getParent(dragItem));
            insertChild(dragItem, getParent(dragItem), num1 - 1);
          }
          else
          {
            removeChild(dragItem, getParent(dragItem));
            insertChild(dragItem, getParent(dragItem), num1);
          }
        }
      }
    }
  }
}
