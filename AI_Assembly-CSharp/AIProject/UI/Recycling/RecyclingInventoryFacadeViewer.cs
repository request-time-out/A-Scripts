// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingInventoryFacadeViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Recycling
{
  [Serializable]
  public class RecyclingInventoryFacadeViewer : InventoryFacadeViewer
  {
    [SerializeField]
    private InventoryType _inventoryType;
    [SerializeField]
    private CanvasGroup _rootCanvasGroup;
    [SerializeField]
    private PanelType _panelType;

    public PanelType PanelType
    {
      get
      {
        return this._panelType;
      }
    }

    public InventoryType InventoryType
    {
      get
      {
        return this._inventoryType;
      }
    }

    public CanvasGroup RootCanvasGroup
    {
      get
      {
        return this._rootCanvasGroup;
      }
    }

    public ItemSortUI ItemSortUI
    {
      get
      {
        return this.viewer.sortUI;
      }
    }

    public Toggle Sorter
    {
      get
      {
        return this.viewer.sorter;
      }
    }

    public Button SortButton
    {
      get
      {
        return this.viewer.sortButton;
      }
    }

    public ItemListController ListController { get; } = new ItemListController();

    [DebuggerHidden]
    public override IEnumerator Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingInventoryFacadeViewer.\u003CInitialize\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public int AddItem(StuffItem item)
    {
      int count = item.Count;
      int possible;
      if (!((IReadOnlyCollection<StuffItem>) this.itemList).CanAddItem(this.slotCounter.y, item, count, out possible))
        count = possible;
      if (count <= 0)
        return 0;
      this.itemList.AddItem(new StuffItem(item), count);
      item.Count -= count;
      List<StuffItem> list = ((IEnumerable<StuffItem>) this.itemList.FindItems(item)).ToList<StuffItem>();
      foreach (ItemNodeUI itemNodeUi in this.itemListUI)
        list.Remove(itemNodeUi.Item);
      foreach (StuffItem stuffItem in list)
        this.ItemListAddNode(this.itemListUI.SearchNotUsedIndex, stuffItem);
      this.ItemListNodeFilter(this.categoryUI.CategoryID, true);
      return count;
    }

    public bool AddItemCondition(StuffItem item)
    {
      int count = item.Count;
      int possible;
      if (!((IReadOnlyCollection<StuffItem>) this.itemList).CanAddItem(this.slotCounter.y, item, count, out possible))
        count = possible;
      if (count <= 0)
        return false;
      this.itemList.AddItem(new StuffItem(item), count);
      item.Count -= count;
      List<StuffItem> list = ((IEnumerable<StuffItem>) this.itemList.FindItems(item)).ToList<StuffItem>();
      foreach (ItemNodeUI itemNodeUi in this.itemListUI)
        list.Remove(itemNodeUi.Item);
      foreach (StuffItem stuffItem in list)
        this.ItemListAddNode(this.itemListUI.SearchNotUsedIndex, stuffItem);
      this.ItemListNodeFilter(this.categoryUI.CategoryID, true);
      return item.Count <= 0;
    }

    public void SetVisible(bool visible)
    {
      if (Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null))
      {
        this._rootCanvasGroup.set_alpha(!visible ? 0.0f : 1f);
        this._rootCanvasGroup.set_blocksRaycasts(visible);
        this._rootCanvasGroup.set_interactable(visible);
      }
      else if (Object.op_Inequality((Object) this.viewer, (Object) null) && ((Component) this.viewer).get_gameObject().get_activeSelf() != visible)
        ((Component) this.viewer).get_gameObject().SetActive(visible);
      if (visible || !Object.op_Inequality((Object) this.viewer, (Object) null) || !Object.op_Inequality((Object) this.viewer.sortUI, (Object) null))
        return;
      this.viewer.sortUI.Close();
    }
  }
}
