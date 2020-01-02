// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeViewDemo_DataItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class VirtualizingTreeViewDemo_DataItems : MonoBehaviour
  {
    public VirtualizingTreeView TreeView;
    private List<DataItem> m_dataItems;
    [SerializeField]
    private GameObject m_buttons;
    private int m_counter;

    public VirtualizingTreeViewDemo_DataItems()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.TreeView.ItemDataBinding += new EventHandler<VirtualizingTreeViewItemDataBindingArgs>(this.OnItemDataBinding);
      this.TreeView.SelectionChanged += new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.TreeView.ItemsRemoved += new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.TreeView.ItemExpanding += new EventHandler<VirtualizingItemExpandingArgs>(this.OnItemExpanding);
      this.TreeView.ItemBeginDrag += new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.TreeView.ItemDrop += new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.TreeView.ItemBeginDrop += new EventHandler<ItemDropCancelArgs>(this.OnItemBeginDrop);
      this.TreeView.ItemEndDrag += new EventHandler<ItemArgs>(this.OnItemEndDrag);
      this.m_dataItems = new List<DataItem>();
      for (int index = 0; index < 100; ++index)
        this.m_dataItems.Add(new DataItem("DataItem " + (object) index));
      this.TreeView.Items = (IEnumerable) this.m_dataItems;
      if (!Object.op_Inequality((Object) this.m_buttons, (Object) null))
        return;
      this.m_buttons.SetActive(false);
    }

    private void OnDestroy()
    {
      this.TreeView.ItemDataBinding -= new EventHandler<VirtualizingTreeViewItemDataBindingArgs>(this.OnItemDataBinding);
      this.TreeView.SelectionChanged -= new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.TreeView.ItemsRemoved -= new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.TreeView.ItemExpanding -= new EventHandler<VirtualizingItemExpandingArgs>(this.OnItemExpanding);
      this.TreeView.ItemBeginDrag -= new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.TreeView.ItemBeginDrop -= new EventHandler<ItemDropCancelArgs>(this.OnItemBeginDrop);
      this.TreeView.ItemDrop -= new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.TreeView.ItemEndDrag -= new EventHandler<ItemArgs>(this.OnItemEndDrag);
    }

    private void OnItemExpanding(object sender, VirtualizingItemExpandingArgs e)
    {
      DataItem dataItem = (DataItem) e.Item;
      if (dataItem.Children.Count <= 0)
        return;
      e.Children = (IEnumerable) dataItem.Children;
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
      if (!Object.op_Inequality((Object) this.m_buttons, (Object) null))
        return;
      this.m_buttons.SetActive(this.TreeView.SelectedItem != null);
    }

    private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
    {
      for (int index = 0; index < e.Items.Length; ++index)
      {
        DataItem dataItem = (DataItem) e.Items[index];
        if (dataItem.Parent != null)
          dataItem.Parent.Children.Remove(dataItem);
        this.m_dataItems.Remove(dataItem);
      }
    }

    private void OnItemDataBinding(object sender, VirtualizingTreeViewItemDataBindingArgs e)
    {
      if (!(e.Item is DataItem dataItem))
        return;
      ((Text) e.ItemPresenter.GetComponentInChildren<Text>(true)).set_text(dataItem.Name);
      ((Image) e.ItemPresenter.GetComponentsInChildren<Image>()[4]).set_sprite((Sprite) Resources.Load<Sprite>("IconNew"));
      e.HasChildren = dataItem.Children.Count > 0;
    }

    private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
    {
    }

    private void OnItemBeginDrag(object sender, ItemArgs e)
    {
    }

    private void OnItemEndDrag(object sender, ItemArgs e)
    {
    }

    private List<DataItem> ChildrenOf(DataItem parent)
    {
      return parent == null ? this.m_dataItems : parent.Children;
    }

    private void OnItemDrop(object sender, ItemDropArgs args)
    {
      if (args.DropTarget == null)
        return;
      this.TreeView.ItemDropStdHandler<DataItem>(args, (Func<DataItem, DataItem>) (item => item.Parent), (Action<DataItem, DataItem>) ((item, parent) => item.Parent = parent), (Func<DataItem, DataItem, int>) ((item, parent) => this.ChildrenOf(parent).IndexOf(item)), (Action<DataItem, DataItem>) ((item, parent) => this.ChildrenOf(parent).Remove(item)), (Action<DataItem, DataItem, int>) ((item, parent, i) => this.ChildrenOf(parent).Insert(i, item)));
    }

    public void ScrollIntoView()
    {
      this.TreeView.ScrollIntoView(this.TreeView.SelectedItem);
    }

    public void Add()
    {
      foreach (DataItem selectedItem in this.TreeView.SelectedItems)
      {
        DataItem dataItem1 = new DataItem("New Item");
        selectedItem.Children.Add(dataItem1);
        dataItem1.Parent = selectedItem;
        this.TreeView.AddChild((object) selectedItem, (object) dataItem1);
        this.TreeView.Expand((object) selectedItem);
        DataItem dataItem2 = new DataItem("New Sub Item");
        dataItem1.Children.Add(dataItem2);
        dataItem2.Parent = dataItem1;
        this.TreeView.AddChild((object) dataItem1, (object) dataItem2);
        this.TreeView.Expand((object) dataItem1);
        ++this.m_counter;
      }
    }

    public void Remove()
    {
      foreach (DataItem dataItem in this.TreeView.SelectedItems.OfType<object>().ToArray<object>())
        this.TreeView.RemoveChild((object) dataItem.Parent, (object) dataItem);
    }

    public void Collapse()
    {
      foreach (DataItem selectedItem in this.TreeView.SelectedItems)
        this.TreeView.Collapse((object) selectedItem);
    }

    public void Expand()
    {
      foreach (DataItem selectedItem in this.TreeView.SelectedItems)
        this.TreeView.ExpandAll<DataItem>(selectedItem, (Func<DataItem, DataItem>) (item => item.Parent), (Func<DataItem, IEnumerable>) (item => (IEnumerable) item.Children));
    }
  }
}
