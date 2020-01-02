// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.TreeViewItemContainerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Battlehub.UIControls
{
  public class TreeViewItemContainerData : ItemContainerData
  {
    private TreeViewItemContainerData m_parent;

    public static event EventHandler<VirtualizingParentChangedEventArgs> ParentChanged;

    public TreeViewItemContainerData Parent
    {
      get
      {
        return this.m_parent;
      }
      set
      {
        if (this.m_parent == value)
          return;
        TreeViewItemContainerData parent = this.m_parent;
        this.m_parent = value;
        if (TreeViewItemContainerData.ParentChanged == null)
          return;
        TreeViewItemContainerData.ParentChanged((object) this, new VirtualizingParentChangedEventArgs(parent, this.m_parent));
      }
    }

    public object ParentItem
    {
      get
      {
        return this.m_parent == null ? (object) null : this.m_parent.Item;
      }
    }

    public int Indent { get; set; }

    public bool CanExpand { get; set; }

    public bool IsExpanded { get; set; }

    public bool IsDescendantOf(TreeViewItemContainerData ancestor)
    {
      if (ancestor == null)
        return true;
      if (ancestor == this)
        return false;
      for (TreeViewItemContainerData itemContainerData = this; itemContainerData != null; itemContainerData = itemContainerData.Parent)
      {
        if (ancestor == itemContainerData)
          return true;
      }
      return false;
    }

    public bool HasChildren(VirtualizingTreeView treeView)
    {
      if (Object.op_Equality((Object) treeView, (Object) null))
        return false;
      int num = treeView.IndexOf(this.Item);
      TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) treeView.GetItemContainerData(num + 1);
      return itemContainerData != null && itemContainerData.Parent == this;
    }

    public TreeViewItemContainerData FirstChild(
      VirtualizingTreeView treeView)
    {
      if (!this.HasChildren(treeView))
        return (TreeViewItemContainerData) null;
      int siblingIndex = treeView.IndexOf(this.Item) + 1;
      return (TreeViewItemContainerData) treeView.GetItemContainerData(siblingIndex);
    }

    public TreeViewItemContainerData NextChild(
      VirtualizingTreeView treeView,
      TreeViewItemContainerData currentChild)
    {
      if (currentChild == null)
        throw new ArgumentNullException(nameof (currentChild));
      int siblingIndex = treeView.IndexOf(currentChild.Item) + 1;
      for (TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) treeView.GetItemContainerData(siblingIndex); itemContainerData != null && itemContainerData.IsDescendantOf(this); itemContainerData = (TreeViewItemContainerData) treeView.GetItemContainerData(siblingIndex))
      {
        if (itemContainerData.Parent == this)
          return itemContainerData;
        ++siblingIndex;
      }
      return (TreeViewItemContainerData) null;
    }

    public TreeViewItemContainerData LastChild(
      VirtualizingTreeView treeView)
    {
      if (!this.HasChildren(treeView))
        return (TreeViewItemContainerData) null;
      int siblingIndex = treeView.IndexOf(this.Item);
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) null;
      while (true)
      {
        TreeViewItemContainerData itemContainerData2;
        do
        {
          ++siblingIndex;
          itemContainerData2 = (TreeViewItemContainerData) treeView.GetItemContainerData(siblingIndex);
          if (itemContainerData2 == null || !itemContainerData2.IsDescendantOf(this))
            return itemContainerData1;
        }
        while (itemContainerData2.Parent != this);
        itemContainerData1 = itemContainerData2;
      }
    }

    public TreeViewItemContainerData LastDescendant(
      VirtualizingTreeView treeView)
    {
      if (!this.HasChildren(treeView))
        return (TreeViewItemContainerData) null;
      int siblingIndex = treeView.IndexOf(this.Item);
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) null;
      while (true)
      {
        ++siblingIndex;
        TreeViewItemContainerData itemContainerData2 = (TreeViewItemContainerData) treeView.GetItemContainerData(siblingIndex);
        if (itemContainerData2 != null && itemContainerData2.IsDescendantOf(this))
          itemContainerData1 = itemContainerData2;
        else
          break;
      }
      return itemContainerData1;
    }

    public override string ToString()
    {
      return "Data: " + this.Item;
    }
  }
}
