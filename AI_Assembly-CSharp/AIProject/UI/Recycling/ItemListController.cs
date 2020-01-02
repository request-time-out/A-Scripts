// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.ItemListController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Illusion.Game.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject.UI.Recycling
{
  public class ItemListController
  {
    private ItemListUI _itemListUI;
    [NonSerialized]
    public Action RefreshEvent;

    public ItemListController()
    {
    }

    public ItemListController(PanelType panelType)
    {
      this.PanelType = panelType;
    }

    public void SetPanelType(PanelType panelType)
    {
      this.PanelType = panelType;
    }

    public void SetItemList(List<StuffItem> itemList)
    {
      this.ItemList = itemList;
    }

    public void SetInventoryUI(RecyclingInventoryFacadeViewer inventoryUI)
    {
      this.InventoryUI = inventoryUI;
    }

    public PanelType PanelType { get; private set; }

    public List<StuffItem> ItemList { get; private set; }

    public RecyclingInventoryFacadeViewer InventoryUI { get; private set; }

    public event Action<int, ItemNodeUI> DoubleClick;

    public ItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public void Bind(ItemListUI itemListUI)
    {
      this._itemListUI = itemListUI;
    }

    public void Clear()
    {
      this._itemListUI.ClearItems();
      if (this.RefreshEvent == null)
        return;
      this.RefreshEvent();
    }

    public void Create(IReadOnlyCollection<StuffItem> itemCollection)
    {
      this._itemListUI.ClearItems();
      foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) itemCollection)
        this.ItemListAddNode(stuffItem, new ExtraPadding(stuffItem, this));
      if (this.RefreshEvent == null)
        return;
      this.RefreshEvent();
    }

    public int EmptySlotNum()
    {
      int num;
      switch (this.PanelType)
      {
        case PanelType.Pouch:
          WorldData worldData = !Singleton<Manager.Game>.IsInstance() ? (WorldData) null : Singleton<Manager.Game>.Instance.WorldData;
          num = worldData == null ? 0 : worldData.PlayerData.InventorySlotMax;
          break;
        case PanelType.Chest:
          Resources resources1 = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
          num = !Object.op_Inequality((Object) resources1, (Object) null) ? 0 : resources1.DefinePack.ItemBoxCapacityDefines.StorageCapacity;
          break;
        case PanelType.CreatedItem:
          Resources resources2 = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
          num = !Object.op_Inequality((Object) resources2, (Object) null) ? 0 : resources2.DefinePack.Recycling.CreateItemCapacity;
          break;
        default:
          return 0;
      }
      if (num <= 0)
        return 0;
      return this.ItemList.IsNullOrEmpty<StuffItem>() ? num : Mathf.Max(0, num - this.ItemList.Count);
    }

    public int PossibleCount(StuffItem item)
    {
      if (item == null)
        return 0;
      int capacity;
      switch (this.PanelType)
      {
        case PanelType.Pouch:
          WorldData worldData = !Singleton<Manager.Game>.IsInstance() ? (WorldData) null : Singleton<Manager.Game>.Instance.WorldData;
          capacity = worldData == null ? 0 : worldData.PlayerData.InventorySlotMax;
          break;
        case PanelType.Chest:
          Resources resources1 = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
          capacity = !Object.op_Inequality((Object) resources1, (Object) null) ? 0 : resources1.DefinePack.ItemBoxCapacityDefines.StorageCapacity;
          break;
        case PanelType.DecidedItem:
          Resources resources2 = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
          capacity = !Object.op_Inequality((Object) resources2, (Object) null) ? 0 : resources2.DefinePack.Recycling.DecidedItemCapacity;
          break;
        case PanelType.CreatedItem:
          Resources resources3 = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
          capacity = !Object.op_Inequality((Object) resources3, (Object) null) ? 0 : resources3.DefinePack.Recycling.CreateItemCapacity;
          break;
        default:
          return 0;
      }
      int possible = 0;
      if (this.PanelType == PanelType.DecidedItem)
      {
        int num = 0;
        foreach (StuffItem stuffItem in this.ItemList)
        {
          if (stuffItem != null)
            num += stuffItem.Count;
        }
        possible = Mathf.Min(capacity - num, item.Count);
      }
      else
      {
        int count = item.Count;
        possible = !((IReadOnlyCollection<StuffItem>) this.ItemList).CanAddItem(capacity, item, count, out possible) ? possible : count;
      }
      return Mathf.Max(possible, 0);
    }

    public int AddItem(StuffItem item)
    {
      if (this.InventoryUI != null)
        return this.InventoryUI.AddItem(item);
      if (this.ItemList == null)
        return 0;
      int count = this.PossibleCount(item);
      if (count <= 0)
        return 0;
      this.ItemList.AddItem(new StuffItem(item), count);
      item.Count -= count;
      List<StuffItem> list = ((IEnumerable<StuffItem>) this.ItemList.FindItems(item)).ToList<StuffItem>();
      foreach (ItemNodeUI itemNodeUi in this._itemListUI)
        list.Remove(itemNodeUi.Item);
      foreach (StuffItem stuffItem in list)
        this.ItemListAddNode(stuffItem, new ExtraPadding(stuffItem, this));
      if (this.RefreshEvent != null)
        this.RefreshEvent();
      return count;
    }

    public int RemoveItem(int sel, StuffItem item)
    {
      ItemNodeUI node = this._itemListUI.GetNode(sel);
      node.Item.Count -= item.Count;
      int count = node.Item.Count;
      if (count <= 0)
      {
        this.ItemList.Remove(node.Item);
        this._itemListUI.RemoveItemNode(sel);
        this._itemListUI.ForceSetNonSelect();
      }
      if (this.RefreshEvent != null)
        this.RefreshEvent();
      return count;
    }

    private ItemNodeUI ItemListAddNode(StuffItem item, ExtraPadding padding)
    {
      int index = this._itemListUI.SearchNotUsedIndex;
      ItemNodeUI node = this._itemListUI.AddItemNode(index, item);
      if (Object.op_Inequality((Object) node, (Object) null))
      {
        node.extraData = (ItemNodeUI.ExtraData) padding;
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<IList<double>>((IObservable<M0>) UnityEventExtensions.AsObservable((UnityEvent) node.OnClick).DoubleInterval<Unit>(250f, false), (Action<M0>) (_ =>
        {
          if (this.DoubleClick == null)
            return;
          this.DoubleClick(index, node);
        })), (Component) node);
      }
      return node;
    }
  }
}
