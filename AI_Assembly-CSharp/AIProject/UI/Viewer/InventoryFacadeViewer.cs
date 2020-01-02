// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.InventoryFacadeViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Illusion.Game.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  [Serializable]
  public class InventoryFacadeViewer
  {
    [SerializeField]
    private bool _sorterVisible = true;
    [SerializeField]
    private bool _counterVisible = true;
    [Header("Filter Setting")]
    [SerializeField]
    private InventoryFacadeViewer.ItemFilter[] _itemFilter = new InventoryFacadeViewer.ItemFilter[0];
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _parent;
    [SerializeField]
    private Sprite _iconSprite;
    [SerializeField]
    private string _iconText;
    [SerializeField]
    private InventoryViewer _viewer;
    [SerializeField]
    private Transform _sortUIPanel;

    public bool sorterVisible
    {
      set
      {
        this._sorterVisible = value;
      }
    }

    public bool counterVisible
    {
      set
      {
        this._counterVisible = value;
      }
    }

    public Action<InventoryFacadeViewer.DoubleClickData> ItemNodeOnDoubleClick { get; set; }

    public InventoryViewer viewer
    {
      get
      {
        return this._viewer;
      }
    }

    public ItemFilterCategoryUI categoryUI
    {
      get
      {
        return this._viewer.categoryUI;
      }
    }

    public ItemListUI itemListUI
    {
      get
      {
        return this._viewer.itemListUI;
      }
    }

    public Image cursor
    {
      get
      {
        return this._viewer.cursor;
      }
    }

    public ConditionalTextXtoYViewer slotCounter
    {
      get
      {
        return this._viewer.slotCounter;
      }
    }

    public bool initialized { get; private set; }

    public void SetItemList(List<StuffItem> itemList)
    {
      this.itemList = itemList;
    }

    public List<StuffItem> itemList { get; private set; }

    public void SetItemList_System(List<StuffItem> itemList_System)
    {
      this.itemList_System = itemList_System;
    }

    public List<StuffItem> itemList_System { get; private set; } = new List<StuffItem>();

    public bool Visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this.visible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.visible).set_Value(value);
      }
    }

    public void SetItemFilter(InventoryFacadeViewer.ItemFilter[] itemFilter)
    {
      this._itemFilter = itemFilter;
    }

    public void SetParent(RectTransform parent)
    {
      this._parent = parent;
    }

    public CanvasGroup canvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    private BoolReactiveProperty visible { get; } = new BoolReactiveProperty(true);

    [DebuggerHidden]
    public virtual IEnumerator Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryFacadeViewer.\u003CInitialize\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Refresh()
    {
      this._viewer.itemListUI.Refresh();
      this._viewer.slotCounter.x = this.itemList.Count;
    }

    public virtual void ItemListNodeCreate()
    {
      Dictionary<int, int[]> table = ((IEnumerable<InventoryFacadeViewer.ItemFilter>) this._itemFilter).ToLookup<InventoryFacadeViewer.ItemFilter, int, int[]>((Func<InventoryFacadeViewer.ItemFilter, int>) (v => v.category), (Func<InventoryFacadeViewer.ItemFilter, int[]>) (v => v.IDs)).ToDictionary<IGrouping<int, int[]>, int, int[]>((Func<IGrouping<int, int[]>, int>) (v => v.Key), (Func<IGrouping<int, int[]>, int[]>) (v => v.SelectMany<int[], int>((Func<int[], IEnumerable<int>>) (x => (IEnumerable<int>) x)).ToArray<int>()));
      IEnumerable<StuffItem> source = this.itemList_System.Concat<StuffItem>((IEnumerable<StuffItem>) this.itemList);
      List<StuffItem> viewList = !table.Any<KeyValuePair<int, int[]>>() || table.Count == 1 && table.First<KeyValuePair<int, int[]>>().Key == 0 ? source.ToList<StuffItem>() : source.Where<StuffItem>((Func<StuffItem, bool>) (item =>
      {
        if (item.CategoryID <= 0)
          return true;
        int[] numArray;
        if (!table.TryGetValue(item.CategoryID, out numArray))
          return false;
        return !((IEnumerable<int>) numArray).Any<int>() || ((IEnumerable<int>) numArray).Contains<int>(item.ID);
      })).ToList<StuffItem>();
      this._viewer.itemListUI.ClearItems();
      this._viewer.categoryUI.Filter(table.Keys.ToArray<int>());
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType20<StuffItem, int> anonType20 in Enumerable.Range(0, viewList.Count).Select<int, \u003C\u003E__AnonType20<StuffItem, int>>((Func<int, int, \u003C\u003E__AnonType20<StuffItem, int>>) ((i, index) => new \u003C\u003E__AnonType20<StuffItem, int>(viewList[i], index))))
        this.ItemListAddNode(anonType20.index, anonType20.item);
      int category = ((IEnumerable<int>) this._viewer.categoryUI.Visibles).FirstOrDefault<int>();
      this._viewer.categoryUI.SetSelectAndCategory(category);
      this.ItemListNodeFilter(category, true);
    }

    public virtual ItemNodeUI ItemListAddNode(int index, StuffItem item)
    {
      ItemNodeUI node = this._viewer.itemListUI.AddItemNode(index, item);
      if (Object.op_Inequality((Object) node, (Object) null) && this.ItemNodeOnDoubleClick != null)
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<IList<double>>((IObservable<M0>) UnityEventExtensions.AsObservable((UnityEvent) node.OnClick).DoubleInterval<Unit>(250f, false), (Action<M0>) (_ =>
        {
          Action<InventoryFacadeViewer.DoubleClickData> nodeOnDoubleClick = this.ItemNodeOnDoubleClick;
          if (nodeOnDoubleClick == null)
            return;
          nodeOnDoubleClick(new InventoryFacadeViewer.DoubleClickData(index, node));
        })), (Component) node);
      return node;
    }

    public virtual void ItemListNodeFilter(int category, bool isSort)
    {
      this._viewer.itemListUI.Filter(category);
      this._viewer.itemListUI.ForceSetNonSelect();
      if (!isSort)
        return;
      this._viewer.SortItemList();
    }

    public bool AddItem(StuffItem item)
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

    protected virtual void CanvasGroupSetting()
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
      {
        ((ReactiveProperty<bool>) this.visible).Dispose();
      }
      else
      {
        ((ReactiveProperty<bool>) this.visible).set_Value(!Mathf.Approximately(this._canvasGroup.get_alpha(), 0.0f));
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.visible, (Action<M0>) (isOn =>
        {
          this._canvasGroup.set_alpha(!isOn ? 0.0f : 1f);
          this._canvasGroup.set_blocksRaycasts(isOn);
        }));
      }
    }

    [Serializable]
    public class ItemFilter
    {
      [SerializeField]
      private int[] _IDs = new int[0];
      [SerializeField]
      private int _category;

      public ItemFilter()
      {
      }

      public ItemFilter(int category)
      {
        this._category = category;
      }

      public ItemFilter(int category, int[] IDs)
      {
        this._category = category;
        this._IDs = IDs;
      }

      public int category
      {
        get
        {
          return this._category;
        }
      }

      public int[] IDs
      {
        get
        {
          return this._IDs;
        }
      }
    }

    public struct DoubleClickData
    {
      public DoubleClickData(int ID, ItemNodeUI node)
      {
        this.ID = ID;
        this.node = node;
      }

      public int ID { get; }

      public ItemNodeUI node { get; }
    }
  }
}
