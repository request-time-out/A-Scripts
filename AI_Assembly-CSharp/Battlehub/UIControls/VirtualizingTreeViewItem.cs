// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeViewItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class VirtualizingTreeViewItem : VirtualizingItemContainer
  {
    private TreeViewExpander m_expander;
    [SerializeField]
    private HorizontalLayoutGroup m_itemLayout;
    private Toggle m_toggle;
    private TreeViewItemContainerData m_treeViewItemData;

    private VirtualizingTreeView TreeView
    {
      get
      {
        return this.ItemsControl as VirtualizingTreeView;
      }
    }

    public float Indent
    {
      get
      {
        return (float) this.m_treeViewItemData.Indent;
      }
    }

    public override object Item
    {
      get
      {
        return base.Item;
      }
      set
      {
        base.Item = value;
        this.m_treeViewItemData = (TreeViewItemContainerData) this.TreeView.GetItemContainerData(value);
        if (this.m_treeViewItemData == null)
        {
          this.m_treeViewItemData = new TreeViewItemContainerData();
          ((Object) this).set_name("Null");
        }
        else
        {
          this.UpdateIndent();
          if (Object.op_Inequality((Object) this.m_expander, (Object) null))
          {
            this.m_expander.CanExpand = this.m_treeViewItemData.CanExpand;
            this.m_expander.IsOn = this.m_treeViewItemData.IsExpanded && this.m_treeViewItemData.CanExpand;
          }
          ((Object) this).set_name(base.Item.ToString() + " " + this.m_treeViewItemData.ToString());
        }
      }
    }

    public TreeViewItemContainerData TreeViewItemData
    {
      get
      {
        return this.m_treeViewItemData;
      }
    }

    public TreeViewItemContainerData Parent
    {
      get
      {
        return this.m_treeViewItemData != null ? this.m_treeViewItemData.Parent : (TreeViewItemContainerData) null;
      }
      set
      {
        if (this.m_treeViewItemData == null || this.m_treeViewItemData.Parent == value)
          return;
        this.m_treeViewItemData.Parent = value;
        this.UpdateIndent();
      }
    }

    public void UpdateIndent()
    {
      if (this.Parent != null && Object.op_Inequality((Object) this.TreeView, (Object) null) && Object.op_Inequality((Object) this.m_itemLayout, (Object) null))
      {
        this.m_treeViewItemData.Indent = this.Parent.Indent + this.TreeView.Indent;
        ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_treeViewItemData.Indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
        int itemIndex = this.TreeView.IndexOf(this.Item);
        this.SetIndent(this, ref itemIndex);
      }
      else
      {
        this.ZeroIndent();
        int itemIndex = this.TreeView.IndexOf(this.Item);
        if (!this.HasChildren)
          return;
        this.SetIndent(this, ref itemIndex);
      }
    }

    private void ZeroIndent()
    {
      if (this.m_treeViewItemData != null)
        this.m_treeViewItemData.Indent = 0;
      if (!Object.op_Inequality((Object) this.m_itemLayout, (Object) null))
        return;
      ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(0, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
    }

    private void SetIndent(VirtualizingTreeViewItem parent, ref int itemIndex)
    {
      while (true)
      {
        VirtualizingTreeViewItem itemContainer = (VirtualizingTreeViewItem) this.TreeView.GetItemContainer(this.TreeView.GetItemAt(itemIndex + 1));
        if (!Object.op_Equality((Object) itemContainer, (Object) null) && itemContainer.Item != null && itemContainer.Parent == parent.m_treeViewItemData)
        {
          itemContainer.m_treeViewItemData.Indent = parent.m_treeViewItemData.Indent + this.TreeView.Indent;
          ((LayoutGroup) itemContainer.m_itemLayout).get_padding().set_left(itemContainer.m_treeViewItemData.Indent);
          ++itemIndex;
          this.SetIndent(itemContainer, ref itemIndex);
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
        if (Object.op_Inequality((Object) this.m_toggle, (Object) null))
          this.m_toggle.set_isOn(value);
        base.IsSelected = value;
      }
    }

    public bool CanExpand
    {
      get
      {
        return this.m_treeViewItemData != null && this.m_treeViewItemData.CanExpand;
      }
      set
      {
        if (this.m_treeViewItemData == null || this.m_treeViewItemData.CanExpand == value)
          return;
        this.m_treeViewItemData.CanExpand = value;
        if (Object.op_Inequality((Object) this.m_expander, (Object) null))
          this.m_expander.CanExpand = this.m_treeViewItemData.CanExpand;
        if (this.m_treeViewItemData.CanExpand)
          return;
        this.IsExpanded = false;
      }
    }

    public bool IsExpanded
    {
      get
      {
        return this.m_treeViewItemData != null && this.m_treeViewItemData.IsExpanded;
      }
      set
      {
        if (this.m_treeViewItemData == null || this.m_treeViewItemData.IsExpanded == value)
          return;
        if (Object.op_Inequality((Object) this.m_expander, (Object) null))
          this.m_expander.IsOn = value && this.CanExpand;
        if (!Object.op_Inequality((Object) this.TreeView, (Object) null))
          return;
        if (value && this.CanExpand)
          this.TreeView.Internal_Expand(this.m_treeViewItemData.Item);
        else
          this.TreeView.Internal_Collapse(this.m_treeViewItemData.Item);
      }
    }

    public bool HasChildren
    {
      get
      {
        return this.m_treeViewItemData != null && this.m_treeViewItemData.HasChildren(this.TreeView);
      }
    }

    public TreeViewItemContainerData FirstChild()
    {
      return this.m_treeViewItemData.FirstChild(this.TreeView);
    }

    public TreeViewItemContainerData NextChild(
      TreeViewItemContainerData currentChild)
    {
      return this.m_treeViewItemData.NextChild(this.TreeView, currentChild);
    }

    public TreeViewItemContainerData LastChild()
    {
      return this.m_treeViewItemData.LastChild(this.TreeView);
    }

    public TreeViewItemContainerData LastDescendant()
    {
      return this.m_treeViewItemData.LastDescendant(this.TreeView);
    }

    protected override void AwakeOverride()
    {
      this.m_toggle = (Toggle) ((Component) this).GetComponent<Toggle>();
      ((Selectable) this.m_toggle).set_interactable(false);
      this.m_toggle.set_isOn(this.IsSelected);
      this.m_expander = (TreeViewExpander) ((Component) this).GetComponentInChildren<TreeViewExpander>();
      if (!Object.op_Inequality((Object) this.m_expander, (Object) null))
        return;
      this.m_expander.CanExpand = this.CanExpand;
    }

    protected override void StartOverride()
    {
      if (Object.op_Inequality((Object) this.TreeView, (Object) null))
      {
        this.m_toggle.set_isOn(this.TreeView.IsItemSelected(this.Item));
        this.m_isSelected = this.m_toggle.get_isOn();
      }
      if (this.Parent != null)
      {
        this.m_treeViewItemData.Indent = this.Parent.Indent + this.TreeView.Indent;
        ((LayoutGroup) this.m_itemLayout).set_padding(new RectOffset(this.m_treeViewItemData.Indent, ((LayoutGroup) this.m_itemLayout).get_padding().get_right(), ((LayoutGroup) this.m_itemLayout).get_padding().get_top(), ((LayoutGroup) this.m_itemLayout).get_padding().get_bottom()));
      }
      if (!this.CanExpand || !this.TreeView.AutoExpand)
        return;
      this.IsExpanded = true;
    }

    public override void Clear()
    {
      base.Clear();
    }
  }
}
