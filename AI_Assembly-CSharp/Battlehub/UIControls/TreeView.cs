// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.TreeView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.UIControls
{
  public class TreeView : ItemsControl<TreeViewItemDataBindingArgs>
  {
    public int Indent = 20;
    public bool CanReparent = true;
    public bool AutoExpand;
    private bool m_expandSilently;

    public event EventHandler<ItemExpandingArgs> ItemExpanding;

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
      TreeViewItem.ParentChanged += new EventHandler<ParentChangedEventArgs>(this.OnTreeViewItemParentChanged);
    }

    protected override void OnDisableOverride()
    {
      base.OnDisableOverride();
      TreeViewItem.ParentChanged -= new EventHandler<ParentChangedEventArgs>(this.OnTreeViewItemParentChanged);
    }

    public TreeViewItem GetTreeViewItem(int siblingIndex)
    {
      return (TreeViewItem) this.GetItemContainer(siblingIndex);
    }

    public TreeViewItem GetTreeViewItem(object obj)
    {
      return (TreeViewItem) this.GetItemContainer(obj);
    }

    public void AddChild(object parent, object item)
    {
      if (parent == null)
      {
        this.Add(item);
      }
      else
      {
        TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(parent);
        if (Object.op_Equality((Object) itemContainer, (Object) null))
          return;
        int index = -1;
        if (itemContainer.IsExpanded)
          index = !itemContainer.HasChildren ? this.IndexOf(itemContainer.Item) + 1 : this.IndexOf(itemContainer.LastDescendant().Item) + 1;
        else
          itemContainer.CanExpand = true;
        if (index <= -1)
          return;
        ((TreeViewItem) this.Insert(index, item)).Parent = itemContainer;
      }
    }

    public override void Remove(object item)
    {
      throw new NotSupportedException("This method is not supported for TreeView use RemoveChild instead");
    }

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
        TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(parent);
        if (!Object.op_Implicit((Object) itemContainer))
          return;
        itemContainer.CanExpand = false;
      }
    }

    public void ChangeParent(object parent, object item)
    {
      if (this.IsDropInProgress)
        return;
      ItemContainer itemContainer1 = this.GetItemContainer(item);
      if (Object.op_Equality((Object) itemContainer1, (Object) null))
        return;
      ItemContainer itemContainer2 = this.GetItemContainer(parent);
      ItemContainer[] dragItems = new ItemContainer[1]
      {
        itemContainer1
      };
      if (!this.CanDrop(dragItems, itemContainer2))
        return;
      this.Drop(dragItems, itemContainer2, ItemDropAction.SetLastChild);
    }

    public bool IsExpanded(object item)
    {
      TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(item);
      return !Object.op_Equality((Object) itemContainer, (Object) null) && itemContainer.IsExpanded;
    }

    public void Expand(TreeViewItem item)
    {
      if (this.m_expandSilently || this.ItemExpanding == null)
        return;
      ItemExpandingArgs e = new ItemExpandingArgs(item.Item);
      this.ItemExpanding((object) this, e);
      IEnumerable children = e.Children;
      int siblingIndex = ((Component) item).get_transform().GetSiblingIndex();
      int index = this.IndexOf(item.Item);
      item.CanExpand = children != null;
      if (!item.CanExpand)
        return;
      foreach (object obj in children)
      {
        ++siblingIndex;
        ++index;
        this.InsertItem(index, obj);
        TreeViewItem treeViewItem = (TreeViewItem) this.InstantiateItemContainer(siblingIndex);
        treeViewItem.Item = obj;
        treeViewItem.Parent = item;
        this.DataBindItem(obj, (ItemContainer) treeViewItem);
      }
      this.UpdateSelectedItemIndex();
    }

    public void Collapse(TreeViewItem item)
    {
      int siblingIndex = ((Component) item).get_transform().GetSiblingIndex();
      int num = this.IndexOf(item.Item);
      this.Collapse(item, siblingIndex + 1, num + 1);
    }

    private void Unselect(
      List<object> selectedItems,
      TreeViewItem item,
      ref int containerIndex,
      ref int itemIndex)
    {
      while (true)
      {
        TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(containerIndex);
        if (!Object.op_Equality((Object) itemContainer, (Object) null) && !Object.op_Inequality((Object) itemContainer.Parent, (Object) item))
        {
          ++containerIndex;
          ++itemIndex;
          selectedItems.Remove(itemContainer.Item);
          this.Unselect(selectedItems, itemContainer, ref containerIndex, ref itemIndex);
        }
        else
          break;
      }
    }

    private void Collapse(TreeViewItem item, int containerIndex, int itemIndex)
    {
      while (true)
      {
        TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(containerIndex);
        if (!Object.op_Equality((Object) itemContainer, (Object) null) && !Object.op_Inequality((Object) itemContainer.Parent, (Object) item))
        {
          this.Collapse(itemContainer, containerIndex + 1, itemIndex + 1);
          this.RemoveItemAt(itemIndex);
          this.DestroyItemContainer(containerIndex);
        }
        else
          break;
      }
    }

    protected override ItemContainer InstantiateItemContainerOverride(
      GameObject container)
    {
      TreeViewItem treeViewItem = (TreeViewItem) container.GetComponent<TreeViewItem>();
      if (Object.op_Equality((Object) treeViewItem, (Object) null))
      {
        treeViewItem = (TreeViewItem) container.AddComponent<TreeViewItem>();
        ((Object) ((Component) treeViewItem).get_gameObject()).set_name("TreeViewItem");
      }
      return (ItemContainer) treeViewItem;
    }

    protected override void DestroyItem(object item)
    {
      TreeViewItem itemContainer = (TreeViewItem) this.GetItemContainer(item);
      if (!Object.op_Inequality((Object) itemContainer, (Object) null))
        return;
      this.Collapse(itemContainer);
      base.DestroyItem(item);
      if (!Object.op_Inequality((Object) itemContainer.Parent, (Object) null) || itemContainer.Parent.HasChildren)
        return;
      itemContainer.Parent.CanExpand = false;
    }

    public override void DataBindItem(object item, ItemContainer itemContainer)
    {
      TreeViewItemDataBindingArgs args = new TreeViewItemDataBindingArgs();
      args.Item = item;
      args.ItemPresenter = !Object.op_Equality((Object) itemContainer.ItemPresenter, (Object) null) ? itemContainer.ItemPresenter : ((Component) this).get_gameObject();
      args.EditorPresenter = !Object.op_Equality((Object) itemContainer.EditorPresenter, (Object) null) ? itemContainer.EditorPresenter : ((Component) this).get_gameObject();
      this.RaiseItemDataBinding(args);
      TreeViewItem treeViewItem = (TreeViewItem) itemContainer;
      treeViewItem.CanExpand = args.HasChildren;
      treeViewItem.CanEdit = args.CanEdit;
      treeViewItem.CanDrag = args.CanDrag;
      treeViewItem.CanDrop = args.CanBeParent;
    }

    protected override bool CanDrop(ItemContainer[] dragItems, ItemContainer dropTarget)
    {
      if (!base.CanDrop(dragItems, dropTarget))
        return false;
      TreeViewItem treeViewItem = (TreeViewItem) dropTarget;
      if (Object.op_Equality((Object) treeViewItem, (Object) null))
        return true;
      foreach (TreeViewItem dragItem in dragItems)
      {
        if (treeViewItem.IsDescendantOf(dragItem))
          return false;
      }
      return true;
    }

    private void OnTreeViewItemParentChanged(object sender, ParentChangedEventArgs e)
    {
      TreeViewItem treeViewItem1 = (TreeViewItem) sender;
      if (!this.CanHandleEvent((object) treeViewItem1))
        return;
      TreeViewItem oldParent = e.OldParent;
      if (this.DropMarker.Action != ItemDropAction.SetLastChild && this.DropMarker.Action != ItemDropAction.None)
      {
        if (!Object.op_Inequality((Object) oldParent, (Object) null) || oldParent.HasChildren)
          return;
        oldParent.CanExpand = false;
      }
      else
      {
        TreeViewItem newParent = e.NewParent;
        if (Object.op_Inequality((Object) newParent, (Object) null))
        {
          if (newParent.CanExpand)
          {
            newParent.IsExpanded = true;
          }
          else
          {
            newParent.CanExpand = true;
            try
            {
              this.m_expandSilently = true;
              newParent.IsExpanded = true;
            }
            finally
            {
              this.m_expandSilently = false;
            }
          }
        }
        TreeViewItem child = treeViewItem1.FirstChild();
        TreeViewItem treeViewItem2;
        if (Object.op_Inequality((Object) newParent, (Object) null))
        {
          treeViewItem2 = newParent.LastChild();
          if (Object.op_Equality((Object) treeViewItem2, (Object) null))
            treeViewItem2 = newParent;
        }
        else
          treeViewItem2 = (TreeViewItem) this.LastItemContainer();
        if (Object.op_Inequality((Object) treeViewItem2, (Object) treeViewItem1))
        {
          TreeViewItem treeViewItem3 = treeViewItem2.LastDescendant();
          if (Object.op_Inequality((Object) treeViewItem3, (Object) null))
            treeViewItem2 = treeViewItem3;
          if (!treeViewItem2.IsDescendantOf(treeViewItem1))
            base.SetNextSibling((ItemContainer) treeViewItem2, (ItemContainer) treeViewItem1);
        }
        if (Object.op_Inequality((Object) child, (Object) null))
          this.MoveSubtree(treeViewItem1, child);
        if (!Object.op_Inequality((Object) oldParent, (Object) null) || oldParent.HasChildren)
          return;
        oldParent.CanExpand = false;
      }
    }

    private void MoveSubtree(TreeViewItem parent, TreeViewItem child)
    {
      int siblingIndex1 = ((Component) parent).get_transform().GetSiblingIndex();
      int siblingIndex2 = ((Component) child).get_transform().GetSiblingIndex();
      bool flag = false;
      if (siblingIndex1 < siblingIndex2)
        flag = true;
      for (TreeViewItem treeViewItem = parent; Object.op_Inequality((Object) child, (Object) null) && child.IsDescendantOf(parent) && !Object.op_Equality((Object) treeViewItem, (Object) child); child = (TreeViewItem) this.GetItemContainer(siblingIndex2))
      {
        base.SetNextSibling((ItemContainer) treeViewItem, (ItemContainer) child);
        treeViewItem = child;
        if (flag)
          ++siblingIndex2;
      }
    }

    protected override void Drop(
      ItemContainer[] dragItems,
      ItemContainer dropTarget,
      ItemDropAction action)
    {
      TreeViewItem treeViewItem = (TreeViewItem) dropTarget;
      switch (action)
      {
        case ItemDropAction.SetLastChild:
          for (int index = 0; index < dragItems.Length; ++index)
          {
            TreeViewItem dragItem = (TreeViewItem) dragItems[index];
            if (Object.op_Inequality((Object) treeViewItem, (Object) dragItem))
              dragItem.Parent = treeViewItem;
          }
          break;
        case ItemDropAction.SetPrevSibling:
          for (int index = 0; index < dragItems.Length; ++index)
            this.SetPrevSibling((ItemContainer) treeViewItem, dragItems[index]);
          break;
        case ItemDropAction.SetNextSibling:
          for (int index = dragItems.Length - 1; index >= 0; --index)
            this.SetNextSibling((ItemContainer) treeViewItem, dragItems[index]);
          break;
      }
      this.UpdateSelectedItemIndex();
    }

    protected override void SetNextSibling(ItemContainer sibling, ItemContainer nextSibling)
    {
      TreeViewItem treeViewItem1 = (TreeViewItem) sibling;
      TreeViewItem treeViewItem2 = treeViewItem1.LastDescendant();
      if (Object.op_Equality((Object) treeViewItem2, (Object) null))
        treeViewItem2 = treeViewItem1;
      TreeViewItem parent = (TreeViewItem) nextSibling;
      TreeViewItem child = parent.FirstChild();
      base.SetNextSibling((ItemContainer) treeViewItem2, nextSibling);
      if (Object.op_Inequality((Object) child, (Object) null))
        this.MoveSubtree(parent, child);
      parent.Parent = treeViewItem1.Parent;
    }

    protected override void SetPrevSibling(ItemContainer sibling, ItemContainer prevSibling)
    {
      TreeViewItem treeViewItem = (TreeViewItem) sibling;
      TreeViewItem parent = (TreeViewItem) prevSibling;
      TreeViewItem child = parent.FirstChild();
      base.SetPrevSibling(sibling, prevSibling);
      if (Object.op_Inequality((Object) child, (Object) null))
        this.MoveSubtree(parent, child);
      parent.Parent = treeViewItem.Parent;
    }

    public void UpdateIndent(object obj)
    {
      TreeViewItem treeViewItem = this.GetTreeViewItem(obj);
      if (Object.op_Equality((Object) treeViewItem, (Object) null))
        return;
      treeViewItem.UpdateIndent();
    }

    public void FixScrollRect()
    {
      Canvas.ForceUpdateCanvases();
      RectTransform component = (RectTransform) ((Component) this.Panel).GetComponent<RectTransform>();
      RectTransform rectTransform = component;
      Rect rect = component.get_rect();
      double num = (double) ((Rect) ref rect).get_height() - 0.00999999977648258;
      rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) num);
    }
  }
}
