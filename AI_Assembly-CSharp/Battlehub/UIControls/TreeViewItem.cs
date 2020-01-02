// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.TreeViewItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class TreeViewItem : ItemContainer
  {
    private TreeViewExpander m_expander;
    [SerializeField]
    private HorizontalLayoutGroup m_itemLayout;
    private Toggle m_toggle;
    private TreeView m_treeView;
    private int m_indent;
    private TreeViewItem m_parent;
    private bool m_canExpand;
    private bool m_isExpanded;

    public static event EventHandler<ParentChangedEventArgs> ParentChanged;

    private TreeView TreeView
    {
      get
      {
        if (Object.op_Equality((Object) this.m_treeView, (Object) null))
          this.m_treeView = (TreeView) ((Component) this).GetComponentInParent<TreeView>();
        return this.m_treeView;
      }
    }

    public int Indent
    {
      get
      {
        return this.m_indent;
      }
    }

    public TreeViewItem Parent
    {
      get
      {
        return this.m_parent;
      }
      set
      {
        if (Object.op_Equality((Object) this.m_parent, (Object) value))
          return;
        TreeViewItem parent = this.m_parent;
        this.m_parent = value;
        if (Object.op_Inequality((Object) this.m_parent, (Object) null) && Object.op_Inequality((Object) this.TreeView, (Object) null) && Object.op_Inequality((Object) this.m_itemLayout, (Object) null))
        {
          this.m_indent = this.m_parent.m_indent + this.TreeView.Indent;
          ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
          int siblingIndex = ((Component) this).get_transform().GetSiblingIndex();
          this.SetIndent(this, ref siblingIndex);
        }
        else
          this.ZeroIndent();
        if (!Object.op_Inequality((Object) this.TreeView, (Object) null) || TreeViewItem.ParentChanged == null)
          return;
        TreeViewItem.ParentChanged((object) this, new ParentChangedEventArgs(parent, this.m_parent));
      }
    }

    public void UpdateIndent()
    {
      if (Object.op_Inequality((Object) this.m_parent, (Object) null) && Object.op_Inequality((Object) this.TreeView, (Object) null) && Object.op_Inequality((Object) this.m_itemLayout, (Object) null))
      {
        this.m_indent = this.m_parent.m_indent + this.TreeView.Indent;
        ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
        int siblingIndex = ((Component) this).get_transform().GetSiblingIndex();
        this.SetIndent(this, ref siblingIndex);
      }
      else
        this.ZeroIndent();
    }

    private void ZeroIndent()
    {
      this.m_indent = 0;
      if (!Object.op_Inequality((Object) this.m_itemLayout, (Object) null))
        return;
      ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
    }

    private void SetIndent(TreeViewItem parent, ref int siblingIndex)
    {
      while (true)
      {
        TreeViewItem itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex + 1);
        if (!Object.op_Equality((Object) itemContainer, (Object) null) && !Object.op_Inequality((Object) itemContainer.Parent, (Object) parent))
        {
          itemContainer.m_indent = parent.m_indent + this.TreeView.Indent;
          ((LayoutGroup) itemContainer.m_itemLayout).get_padding().set_left(itemContainer.m_indent);
          ++siblingIndex;
          this.SetIndent(itemContainer, ref siblingIndex);
        }
        else
          break;
      }
    }

    public override bool IsSelected
    {
      get
      {
        return base.IsSelected;
      }
      set
      {
        if (base.IsSelected == value)
          return;
        this.m_toggle.set_isOn(value);
        base.IsSelected = value;
      }
    }

    public bool CanExpand
    {
      get
      {
        return this.m_canExpand;
      }
      set
      {
        if (this.m_canExpand == value)
          return;
        this.m_canExpand = value;
        if (Object.op_Inequality((Object) this.m_expander, (Object) null))
          this.m_expander.CanExpand = this.m_canExpand;
        if (this.m_canExpand)
          return;
        this.IsExpanded = false;
      }
    }

    public bool IsExpanded
    {
      get
      {
        return this.m_isExpanded;
      }
      set
      {
        if (this.m_isExpanded == value)
          return;
        this.m_isExpanded = value && this.m_canExpand;
        if (Object.op_Inequality((Object) this.m_expander, (Object) null))
          this.m_expander.IsOn = value && this.m_canExpand;
        if (!Object.op_Inequality((Object) this.TreeView, (Object) null))
          return;
        if (this.m_isExpanded)
          this.TreeView.Expand(this);
        else
          this.TreeView.Collapse(this);
      }
    }

    public bool HasChildren
    {
      get
      {
        int siblingIndex = ((Component) this).get_transform().GetSiblingIndex();
        if (Object.op_Equality((Object) this.TreeView, (Object) null))
          return false;
        TreeViewItem itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex + 1);
        return Object.op_Inequality((Object) itemContainer, (Object) null) && Object.op_Equality((Object) itemContainer.Parent, (Object) this);
      }
    }

    public bool IsDescendantOf(TreeViewItem ancestor)
    {
      if (Object.op_Equality((Object) ancestor, (Object) null))
        return true;
      if (Object.op_Equality((Object) ancestor, (Object) this))
        return false;
      for (TreeViewItem treeViewItem = this; Object.op_Inequality((Object) treeViewItem, (Object) null); treeViewItem = treeViewItem.Parent)
      {
        if (Object.op_Equality((Object) ancestor, (Object) treeViewItem))
          return true;
      }
      return false;
    }

    public TreeViewItem FirstChild()
    {
      return !this.HasChildren ? (TreeViewItem) null : (TreeViewItem) this.TreeView.GetItemContainer(((Component) this).get_transform().GetSiblingIndex() + 1);
    }

    public TreeViewItem NextChild(TreeViewItem currentChild)
    {
      if (Object.op_Equality((Object) currentChild, (Object) null))
        throw new ArgumentNullException(nameof (currentChild));
      int siblingIndex = ((Component) currentChild).get_transform().GetSiblingIndex() + 1;
      for (TreeViewItem itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex); Object.op_Inequality((Object) itemContainer, (Object) null) && itemContainer.IsDescendantOf(this); itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex))
      {
        if (Object.op_Equality((Object) itemContainer.Parent, (Object) this))
          return itemContainer;
        ++siblingIndex;
      }
      return (TreeViewItem) null;
    }

    public TreeViewItem LastChild()
    {
      if (!this.HasChildren)
        return (TreeViewItem) null;
      int siblingIndex = ((Component) this).get_transform().GetSiblingIndex();
      TreeViewItem treeViewItem = (TreeViewItem) null;
      while (true)
      {
        TreeViewItem itemContainer;
        do
        {
          ++siblingIndex;
          itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex);
          if (Object.op_Equality((Object) itemContainer, (Object) null) || !itemContainer.IsDescendantOf(this))
            return treeViewItem;
        }
        while (!Object.op_Equality((Object) itemContainer.Parent, (Object) this));
        treeViewItem = itemContainer;
      }
    }

    public TreeViewItem LastDescendant()
    {
      if (!this.HasChildren)
        return (TreeViewItem) null;
      int siblingIndex = ((Component) this).get_transform().GetSiblingIndex();
      TreeViewItem treeViewItem = (TreeViewItem) null;
      while (true)
      {
        ++siblingIndex;
        TreeViewItem itemContainer = (TreeViewItem) this.TreeView.GetItemContainer(siblingIndex);
        if (!Object.op_Equality((Object) itemContainer, (Object) null) && itemContainer.IsDescendantOf(this))
          treeViewItem = itemContainer;
        else
          break;
      }
      return treeViewItem;
    }

    protected override void AwakeOverride()
    {
      this.m_toggle = (Toggle) ((Component) this).GetComponent<Toggle>();
      ((Selectable) this.m_toggle).set_interactable(false);
      this.m_toggle.set_isOn(this.IsSelected);
      this.m_expander = (TreeViewExpander) ((Component) this).GetComponentInChildren<TreeViewExpander>();
      if (!Object.op_Inequality((Object) this.m_expander, (Object) null))
        return;
      this.m_expander.CanExpand = this.m_canExpand;
    }

    protected override void StartOverride()
    {
      if (Object.op_Inequality((Object) this.TreeView, (Object) null))
      {
        this.m_toggle.set_isOn(this.TreeView.IsItemSelected(this.Item));
        this.m_isSelected = this.m_toggle.get_isOn();
      }
      if (Object.op_Inequality((Object) this.Parent, (Object) null))
      {
        this.m_indent = this.Parent.m_indent + this.TreeView.Indent;
        ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
      }
      if (!this.CanExpand || !this.TreeView.AutoExpand)
        return;
      this.IsExpanded = true;
    }

    public override void Clear()
    {
      base.Clear();
      this.m_parent = (TreeViewItem) null;
      this.ZeroIndent();
      this.m_isSelected = false;
      this.m_toggle.set_isOn(this.m_isSelected);
      this.m_isExpanded = false;
      this.m_canExpand = false;
      this.m_expander.IsOn = false;
      this.m_expander.CanExpand = false;
    }
  }
}
