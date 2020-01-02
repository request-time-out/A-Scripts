// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.HarvestListViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class HarvestListViewer : MonoBehaviour
  {
    [SerializeField]
    private ItemListUI _itemListUI;
    [SerializeField]
    private Text _infoText;
    [SerializeField]
    private Button _allGetButton;
    [SerializeField]
    private Button _getButton;
    [SerializeField]
    private Image _cursor;

    public HarvestListViewer()
    {
      base.\u002Ector();
    }

    public event Action<IReadOnlyCollection<StuffItem>> GetItemAction;

    public event Action AllGetListAction;

    public event Action<IReadOnlyCollection<StuffItem>> SyncListAction;

    public ICollection<StuffItem> AddFailedList { get; }

    public bool initialized { get; private set; }

    public void Refresh()
    {
      this._itemListUI.ForceSetNonSelect();
    }

    public void AddList(StuffItem item)
    {
      ((ICollection<StuffItem>) this.harvestList).AddItem(item);
      this._itemListUI.AddItemNode(this._itemListUI.SearchNotUsedIndex, item);
    }

    public void ClearList()
    {
      ((Collection<StuffItem>) this.harvestList).Clear();
      this._itemListUI.ClearItems();
    }

    public bool InStorageCheck(StuffItem item)
    {
      List<StuffItem> storage = this.storage;
      int capacity = this.capacity;
      StuffItem stuffItem = item;
      int? count1 = item?.Count;
      int count2 = !count1.HasValue ? 0 : count1.Value;
      int possible;
      ref int local = ref possible;
      bool flag = ((IReadOnlyCollection<StuffItem>) storage).CanAddItem(capacity, stuffItem, count2, out local);
      if (item != null)
        ((IReadOnlyCollection<StuffItem>) this.storage).CanAddItem(this.capacity, (StuffItem) null, 0, out possible);
      ((ReactiveProperty<bool>) this.inStorage).set_Value(possible > 0);
      return flag;
    }

    public ItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    private ReactiveCollection<StuffItem> harvestList { get; }

    private int capacity
    {
      get
      {
        return Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax;
      }
    }

    private List<StuffItem> storage
    {
      get
      {
        return Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList;
      }
    }

    private BoolReactiveProperty inStorage { get; }

    private CompositeDisposable disposables { get; }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HarvestListViewer.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      this.disposables.Clear();
    }
  }
}
