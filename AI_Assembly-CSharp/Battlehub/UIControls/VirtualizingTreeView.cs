// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battlehub.UIControls
{
  public class VirtualizingTreeView : VirtualizingItemsControl<VirtualizingTreeViewItemDataBindingArgs>
  {
    public int Indent = 20;
    public bool CanReparent = true;
    public bool AutoExpand;
    private bool m_expandSilently;

    public event EventHandler<VirtualizingItemExpandingArgs> ItemExpanding;

    public event EventHandler<VirtualizingItemCollapsedArgs> ItemCollapsed;

    protected override bool CanScroll
    {
      get
      {
        return base.CanScroll || this.CanReparent;
      }
    }

    protected override void OnEnableOverride()
    {
      base.OnEnableOverride();
      TreeViewItemContainerData.ParentChanged += new EventHandler<VirtualizingParentChangedEventArgs>(this.OnTreeViewItemParentChanged);
    }

    protected override void OnDisableOverride()
    {
      base.OnDisableOverride();
      TreeViewItemContainerData.ParentChanged -= new EventHandler<VirtualizingParentChangedEventArgs>(this.OnTreeViewItemParentChanged);
    }

    protected override ItemContainerData InstantiateItemContainerData(object item)
    {
      TreeViewItemContainerData itemContainerData = new TreeViewItemContainerData();
      itemContainerData.Item = item;
      return (ItemContainerData) itemContainerData;
    }

    public void AddChild(object parent, object item)
    {
      if (parent == null)
      {
        this.Add(item);
      }
      else
      {
        VirtualizingTreeViewItem itemContainer1 = (VirtualizingTreeViewItem) this.GetItemContainer(parent);
        int index = -1;
        TreeViewItemContainerData itemContainerData1;
        if (Object.op_Equality((Object) itemContainer1, (Object) null))
        {
          itemContainerData1 = (TreeViewItemContainerData) this.GetItemContainerData(parent);
          if (itemContainerData1 == null)
          {
            Debug.LogWarning((object) "Lost parent data");
            return;
          }
          if (itemContainerData1.IsExpanded)
            index = !itemContainerData1.HasChildren(this) ? this.IndexOf(itemContainerData1.Item) + 1 : this.IndexOf(itemContainerData1.LastDescendant(this).Item) + 1;
          else
            itemContainerData1.CanExpand = true;
        }
        else
        {
          if (itemContainer1.IsExpanded)
            index = !itemContainer1.HasChildren ? this.IndexOf(itemContainer1.Item) + 1 : this.IndexOf(itemContainer1.LastDescendant().Item) + 1;
          else
            itemContainer1.CanExpand = true;
          itemContainerData1 = itemContainer1.TreeViewItemData;
        }
        if (index <= -1)
          return;
        TreeViewItemContainerData itemContainerData2 = (TreeViewItemContainerData) this.Insert(index, item);
        VirtualizingTreeViewItem itemContainer2 = (VirtualizingTreeViewItem) this.GetItemContainer(item);
        if (Object.op_Inequality((Object) itemContainer2, (Object) null))
          itemContainer2.Parent = itemContainerData1;
        else
          itemContainerData2.Parent = itemContainerData1;
      }
    }

    public override void Remove(object item)
    {
      throw new NotSupportedException("Use Remove Child instead");
    }

    public void RemoveChild(object parent, object item)
    {
      base.Remove(item);
      this.DataBindItem(parent);
    }

    [Obsolete("Use RemoveChild(object parent, object item) instead")]
    public void RemoveChild(object parent, object item, bool isLastChild)
    {
      if (parent == null)
        base.Remove(item);
      else if (Object.op_Inequality((Object) this.GetItemContainer(item), (Object) null))
      {
        base.Remove(item);
      }
      else
      {
        if (!isLastChild)
          return;
        VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(parent);
        if (!Object.op_Implicit((Object) itemContainer))
          return;
        itemContainer.CanExpand = false;
      }
    }

    public void ChangeParent(object parent, object item)
    {
      if (this.IsDropInProgress)
        return;
      ItemContainerData itemContainerData1 = this.GetItemContainerData(item);
      if (parent == null)
      {
        if (itemContainerData1 == null)
        {
          this.Add(item);
        }
        else
        {
          ItemContainerData[] dragItems = new ItemContainerData[1]
          {
            itemContainerData1
          };
          if (!this.CanDrop(dragItems, (ItemContainerData) null))
            return;
          this.Drop(dragItems, (ItemContainerData) null, ItemDropAction.SetLastChild);
        }
      }
      else
      {
        ItemContainerData itemContainerData2 = this.GetItemContainerData(parent);
        if (itemContainerData2 == null)
        {
          this.DestroyItems(new object[1]{ item }, false);
        }
        else
        {
          ItemContainerData[] dragItems = new ItemContainerData[1]
          {
            itemContainerData1
          };
          if (!this.CanDrop(dragItems, itemContainerData2))
            return;
          this.Drop(dragItems, itemContainerData2, ItemDropAction.SetLastChild);
        }
      }
    }

    public bool IsExpanded(object item)
    {
      TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) this.GetItemContainerData(item);
      return itemContainerData != null && itemContainerData.IsExpanded;
    }

    public bool Expand(object item)
    {
      VirtualizingTreeViewItem treeViewItem = this.GetTreeViewItem(item);
      if (Object.op_Inequality((Object) treeViewItem, (Object) null))
      {
        treeViewItem.IsExpanded = true;
      }
      else
      {
        if ((TreeViewItemContainerData) this.GetItemContainerData(item) == null)
        {
          Debug.LogWarning((object) ("Unable find container data for item " + item));
          return false;
        }
        this.Internal_Expand(item);
      }
      return true;
    }

    public void Internal_Expand(object item)
    {
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) this.GetItemContainerData(item);
      if (itemContainerData1 == null)
        throw new ArgumentException("TreeViewItemContainerData not found", nameof (item));
      if (itemContainerData1.IsExpanded)
        return;
      itemContainerData1.IsExpanded = true;
      if (this.m_expandSilently || this.ItemExpanding == null)
        return;
      VirtualizingItemExpandingArgs e = new VirtualizingItemExpandingArgs(itemContainerData1.Item);
      this.ItemExpanding((object) this, e);
      IEnumerable enumerable = e.Children != null ? (IEnumerable) e.Children.OfType<object>().ToArray<object>() : (IEnumerable) (object[]) null;
      int index1 = this.IndexOf(itemContainerData1.Item);
      VirtualizingTreeViewItem itemContainer1 = (VirtualizingTreeViewItem) this.GetItemContainer(itemContainerData1.Item);
      if (Object.op_Inequality((Object) itemContainer1, (Object) null))
        itemContainer1.CanExpand = enumerable != null;
      else
        itemContainerData1.CanExpand = enumerable != null;
      if (!itemContainerData1.CanExpand)
        return;
      foreach (object obj in enumerable)
      {
        ++index1;
        TreeViewItemContainerData itemContainerData2 = (TreeViewItemContainerData) this.Insert(index1, obj);
        VirtualizingTreeViewItem itemContainer2 = (VirtualizingTreeViewItem) this.GetItemContainer(obj);
        if (Object.op_Inequality((Object) itemContainer2, (Object) null))
          itemContainer2.Parent = itemContainerData1;
        else
          itemContainerData2.Parent = itemContainerData1;
      }
      if (e.ChildrenExpand != null)
      {
        object[] array1 = e.Children.OfType<object>().ToArray<object>();
        bool[] array2 = e.ChildrenExpand.OfType<bool>().ToArray<bool>();
        for (int index2 = 0; index2 < array1.Length; ++index2)
        {
          if (array2[index2])
            this.Expand(array1[index2]);
        }
      }
      this.UpdateSelectedItemIndex();
    }

    public void Collapse(object item)
    {
      VirtualizingTreeViewItem treeViewItem = this.GetTreeViewItem(item);
      if (Object.op_Inequality((Object) treeViewItem, (Object) null))
        treeViewItem.IsExpanded = false;
      else
        this.Internal_Collapse(item);
    }

    public void Internal_Collapse(object item)
    {
      TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) this.GetItemContainerData(item);
      if (itemContainerData == null)
        throw new ArgumentException("TreeViewItemContainerData not found", nameof (item));
      if (!itemContainerData.IsExpanded)
        return;
      itemContainerData.IsExpanded = false;
      int num = this.IndexOf(itemContainerData.Item);
      List<object> itemsToDestroy = new List<object>();
      this.Collapse(itemContainerData, num + 1, itemsToDestroy);
      if (itemsToDestroy.Count > 0)
      {
        bool unselect = false;
        base.DestroyItems(itemsToDestroy.ToArray(), unselect);
      }
      this.SelectedItems = this.SelectedItems;
      if (this.ItemCollapsed == null)
        return;
      this.ItemCollapsed((object) this, new VirtualizingItemCollapsedArgs(item));
    }

    private void Collapse(object[] items)
    {
      List<object> itemsToDestroy = new List<object>();
      for (int index = 0; index < items.Length; ++index)
      {
        int siblingIndex = this.IndexOf(items[index]);
        if (siblingIndex >= 0)
          this.Collapse((TreeViewItemContainerData) this.GetItemContainerData(siblingIndex), siblingIndex + 1, itemsToDestroy);
      }
      if (itemsToDestroy.Count <= 0)
        return;
      bool unselect = false;
      base.DestroyItems(itemsToDestroy.ToArray(), unselect);
    }

    private void Collapse(
      TreeViewItemContainerData item,
      int itemIndex,
      List<object> itemsToDestroy)
    {
      while (true)
      {
        TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) this.GetItemContainerData(itemIndex);
        if (itemContainerData != null && itemContainerData.IsDescendantOf(item))
        {
          itemsToDestroy.Add(itemContainerData.Item);
          ++itemIndex;
        }
        else
          break;
      }
    }

    public override void DataBindItem(object item, VirtualizingItemContainer itemContainer)
    {
      itemContainer.Clear();
      if (item != null)
      {
        VirtualizingTreeViewItemDataBindingArgs args = new VirtualizingTreeViewItemDataBindingArgs();
        args.Item = item;
        args.ItemPresenter = !Object.op_Equality((Object) itemContainer.ItemPresenter, (Object) null) ? itemContainer.ItemPresenter : ((Component) this).get_gameObject();
        args.EditorPresenter = !Object.op_Equality((Object) itemContainer.EditorPresenter, (Object) null) ? itemContainer.EditorPresenter : ((Component) this).get_gameObject();
        this.RaiseItemDataBinding(args);
        VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem) itemContainer;
        virtualizingTreeViewItem.CanExpand = args.HasChildren;
        virtualizingTreeViewItem.CanEdit = this.CanEdit && args.CanEdit;
        virtualizingTreeViewItem.CanDrag = this.CanDrag && args.CanDrag;
        virtualizingTreeViewItem.CanBeParent = args.CanBeParent;
        virtualizingTreeViewItem.UpdateIndent();
      }
      else
      {
        VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem) itemContainer;
        virtualizingTreeViewItem.CanExpand = false;
        virtualizingTreeViewItem.CanEdit = false;
        virtualizingTreeViewItem.CanDrag = false;
        virtualizingTreeViewItem.UpdateIndent();
      }
    }

    private void OnTreeViewItemParentChanged(object sender, VirtualizingParentChangedEventArgs e)
    {
      if (!this.CanHandleEvent(sender))
        return;
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) sender;
      TreeViewItemContainerData oldParent = e.OldParent;
      if (this.DropMarker.Action != ItemDropAction.SetLastChild && this.DropMarker.Action != ItemDropAction.None)
      {
        if (oldParent == null || oldParent.HasChildren(this))
          return;
        VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(oldParent.Item);
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
          itemContainer.CanExpand = false;
        else
          oldParent.CanExpand = false;
      }
      else
      {
        TreeViewItemContainerData newParent = e.NewParent;
        VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem) null;
        if (newParent != null)
          virtualizingTreeViewItem = (VirtualizingTreeViewItem) this.GetItemContainer(newParent.Item);
        if (Object.op_Inequality((Object) virtualizingTreeViewItem, (Object) null))
        {
          if (virtualizingTreeViewItem.CanExpand)
          {
            virtualizingTreeViewItem.IsExpanded = true;
          }
          else
          {
            virtualizingTreeViewItem.CanExpand = true;
            try
            {
              this.m_expandSilently = true;
              virtualizingTreeViewItem.IsExpanded = true;
            }
            finally
            {
              this.m_expandSilently = false;
            }
          }
        }
        else if (newParent != null)
        {
          newParent.CanExpand = true;
          newParent.IsExpanded = true;
        }
        TreeViewItemContainerData child = itemContainerData1.FirstChild(this);
        TreeViewItemContainerData itemContainerData2 = newParent == null ? (TreeViewItemContainerData) this.LastItemContainerData() : newParent.LastChild(this) ?? newParent;
        if (itemContainerData2 != itemContainerData1)
        {
          TreeViewItemContainerData itemContainerData3 = itemContainerData2.LastDescendant(this);
          if (itemContainerData3 != null)
            itemContainerData2 = itemContainerData3;
          if (!itemContainerData2.IsDescendantOf(itemContainerData1))
            base.SetNextSiblingInternal((ItemContainerData) itemContainerData2, (ItemContainerData) itemContainerData1);
        }
        if (child != null)
          this.MoveSubtree(itemContainerData1, child);
        if (oldParent == null || oldParent.HasChildren(this))
          return;
        VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(oldParent.Item);
        if (Object.op_Inequality((Object) itemContainer, (Object) null))
          itemContainer.CanExpand = false;
        else
          oldParent.CanExpand = false;
      }
    }

    private void MoveSubtree(TreeViewItemContainerData parent, TreeViewItemContainerData child)
    {
      int num = this.IndexOf(parent.Item);
      int siblingIndex = this.IndexOf(child.Item);
      bool flag = false;
      if (num < siblingIndex)
        flag = true;
      TreeViewItemContainerData itemContainerData = parent;
      VirtualizingTreeViewItem itemContainer1 = (VirtualizingTreeViewItem) this.GetItemContainer(itemContainerData.Item);
      if (Object.op_Inequality((Object) itemContainer1, (Object) null))
        itemContainer1.UpdateIndent();
      for (; child != null && child.IsDescendantOf(parent) && itemContainerData != child; child = (TreeViewItemContainerData) this.GetItemContainerData(siblingIndex))
      {
        base.SetNextSiblingInternal((ItemContainerData) itemContainerData, (ItemContainerData) child);
        VirtualizingTreeViewItem itemContainer2 = (VirtualizingTreeViewItem) this.GetItemContainer(child.Item);
        if (Object.op_Inequality((Object) itemContainer2, (Object) null))
          itemContainer2.UpdateIndent();
        itemContainerData = child;
        if (flag)
          ++siblingIndex;
      }
    }

    protected override bool CanDrop(ItemContainerData[] dragItems, ItemContainerData dropTarget)
    {
      if (base.CanDrop(dragItems, dropTarget))
      {
        TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) dropTarget;
        for (int index = 0; index < dragItems.Length; ++index)
        {
          TreeViewItemContainerData dragItem = (TreeViewItemContainerData) dragItems[index];
          if (itemContainerData == dragItem || itemContainerData != null && itemContainerData.IsDescendantOf(dragItem))
            return false;
        }
      }
      return true;
    }

    protected override void Drop(
      ItemContainerData[] dragItems,
      ItemContainerData dropTarget,
      ItemDropAction action)
    {
      TreeViewItemContainerData parent = (TreeViewItemContainerData) dropTarget;
      switch (action)
      {
        case ItemDropAction.SetLastChild:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            TreeViewItemContainerData dragItem = (TreeViewItemContainerData) dragItems[index];
            if (parent == null || parent != dragItem && !parent.IsDescendantOf(dragItem))
              this.SetParent(parent, dragItem);
            else
              break;
          }
          break;
        case ItemDropAction.SetPrevSibling:
          for (int index = 0; index < dragItems.Length; ++index)
            this.SetPrevSiblingInternal((ItemContainerData) parent, dragItems[index]);
          break;
        case ItemDropAction.SetNextSibling:
          for (int index = dragItems.Length - 1; index >= 0; --index)
            this.SetNextSiblingInternal((ItemContainerData) parent, dragItems[index]);
          break;
      }
      this.UpdateSelectedItemIndex();
    }

    protected override void SetNextSiblingInternal(
      ItemContainerData sibling,
      ItemContainerData nextSibling)
    {
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) sibling;
      TreeViewItemContainerData itemContainerData2 = itemContainerData1.LastDescendant(this) ?? itemContainerData1;
      TreeViewItemContainerData itemContainerData3 = (TreeViewItemContainerData) nextSibling;
      TreeViewItemContainerData child = itemContainerData3.FirstChild(this);
      base.SetNextSiblingInternal((ItemContainerData) itemContainerData2, nextSibling);
      if (child != null)
        this.MoveSubtree(itemContainerData3, child);
      this.SetParent(itemContainerData1.Parent, itemContainerData3);
      VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(itemContainerData3.Item);
      if (!Object.op_Inequality((Object) itemContainer, (Object) null))
        return;
      itemContainer.UpdateIndent();
    }

    protected override void SetPrevSiblingInternal(
      ItemContainerData sibling,
      ItemContainerData prevSibling)
    {
      TreeViewItemContainerData itemContainerData1 = (TreeViewItemContainerData) sibling;
      TreeViewItemContainerData itemContainerData2 = (TreeViewItemContainerData) prevSibling;
      TreeViewItemContainerData child = itemContainerData2.FirstChild(this);
      base.SetPrevSiblingInternal(sibling, prevSibling);
      if (child != null)
        this.MoveSubtree(itemContainerData2, child);
      this.SetParent(itemContainerData1.Parent, itemContainerData2);
      VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(itemContainerData2.Item);
      if (!Object.op_Inequality((Object) itemContainer, (Object) null))
        return;
      itemContainer.UpdateIndent();
    }

    private void SetParent(TreeViewItemContainerData parent, TreeViewItemContainerData child)
    {
      VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(child.Item);
      if (Object.op_Inequality((Object) itemContainer, (Object) null))
        itemContainer.Parent = parent;
      else
        child.Parent = parent;
    }

    public void UpdateIndent(object obj)
    {
      VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(obj);
      if (Object.op_Equality((Object) itemContainer, (Object) null))
        return;
      itemContainer.UpdateIndent();
    }

    protected override void DestroyItems(object[] items, bool unselect)
    {
      TreeViewItemContainerData[] array = ((IEnumerable<TreeViewItemContainerData>) ((IEnumerable<object>) items).Select<object, ItemContainerData>((Func<object, ItemContainerData>) (item => this.GetItemContainerData(item))).OfType<TreeViewItemContainerData>().ToArray<TreeViewItemContainerData>()).Where<TreeViewItemContainerData>((Func<TreeViewItemContainerData, bool>) (container => container.Parent != null)).Select<TreeViewItemContainerData, TreeViewItemContainerData>((Func<TreeViewItemContainerData, TreeViewItemContainerData>) (container => container.Parent)).ToArray<TreeViewItemContainerData>();
      this.Collapse(items);
      base.DestroyItems(items, unselect);
      foreach (TreeViewItemContainerData itemContainerData in array)
      {
        if (!itemContainerData.HasChildren(this))
        {
          VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.GetItemContainer(itemContainerData.Item);
          if (Object.op_Inequality((Object) itemContainer, (Object) null))
            itemContainer.CanExpand = false;
        }
      }
    }

    public VirtualizingTreeViewItem GetTreeViewItem(object item)
    {
      return this.GetItemContainer(item) as VirtualizingTreeViewItem;
    }

    public void ScrollIntoView(object obj)
    {
      int num = this.IndexOf(obj);
      if (num < 0)
        throw new InvalidOperationException(string.Format("item {0} does not exist or not visible", obj));
      ((VirtualizingScrollRect) ((Component) this).GetComponentInChildren<VirtualizingScrollRect>()).Index = num;
    }
  }
}
