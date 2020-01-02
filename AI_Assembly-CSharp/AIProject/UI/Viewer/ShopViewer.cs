// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.ShopViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Illusion.Game.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject.UI.Viewer
{
  public class ShopViewer : MonoBehaviour
  {
    [Header("Normal")]
    [SerializeField]
    private ItemListUI _normalItemListUI;
    [Header("Special")]
    [SerializeField]
    private ItemListUI _specialItemListUI;

    public ShopViewer()
    {
      base.\u002Ector();
    }

    public ItemListUI normals
    {
      get
      {
        return this._normalItemListUI;
      }
    }

    public ItemListUI specials
    {
      get
      {
        return this._specialItemListUI;
      }
    }

    public ShopViewer.ItemListController[] controllers { get; }

    public bool initialized { get; private set; }

    private void Awake()
    {
      ItemListUI[] itemListUiArray = new ItemListUI[2]
      {
        this._normalItemListUI,
        this._specialItemListUI
      };
      for (int index = 0; index < this.controllers.Length; ++index)
      {
        ShopViewer.ItemListController itemListController = new ShopViewer.ItemListController(index != 0 ? ShopViewer.ItemListController.Mode.VendorSpecial : ShopViewer.ItemListController.Mode.Vendor);
        itemListController.Bind(itemListUiArray[index]);
        this.controllers[index] = itemListController;
      }
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopViewer.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public class ExtraPadding : ItemNodeUI.ExtraData
    {
      public ExtraPadding(StuffItem item, ShopViewer.ItemListController source)
      {
        this.item = item;
        this.source = source;
      }

      public StuffItem item { get; }

      public ShopViewer.ItemListController source { get; }
    }

    public class ItemListController
    {
      private ItemListUI _itemListUI;

      public ItemListController()
      {
      }

      public ItemListController(ShopViewer.ItemListController.Mode mode)
      {
        this.mode = mode;
      }

      public event Action<int, ItemNodeUI> DoubleClick;

      public ItemListUI itemListUI
      {
        get
        {
          return this._itemListUI;
        }
      }

      public ShopViewer.ItemListController.Mode mode { get; }

      public void Bind(ItemListUI itemListUI)
      {
        this._itemListUI = itemListUI;
      }

      public void Clear()
      {
        this._itemListUI.ClearItems();
      }

      public void Create(IReadOnlyCollection<StuffItem> itemCollection)
      {
        this._itemListUI.ClearItems();
        foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) itemCollection)
          this.ItemListAddNode(stuffItem, new ShopViewer.ExtraPadding(stuffItem, this));
      }

      public void AddItem(StuffItem item, ShopViewer.ExtraPadding padding)
      {
        if (this.mode == ShopViewer.ItemListController.Mode.Normal)
        {
          StuffItem stuffItem = (StuffItem) null;
          foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in (IEnumerable<KeyValuePair<int, ItemNodeUI>>) this._itemListUI.optionTable)
          {
            if (keyValuePair.Value.extraData is ShopViewer.ExtraPadding extraData && extraData.item == padding.item)
            {
              stuffItem = keyValuePair.Value.Item;
              break;
            }
          }
          if (stuffItem != null)
            stuffItem.Count += item.Count;
          else
            this.ItemListAddNode(item, padding);
        }
        else
          padding.item.Count += item.Count;
        this._itemListUI.Refresh();
      }

      public void RemoveItem(int sel, StuffItem item)
      {
        ItemNodeUI node = this._itemListUI.GetNode(sel);
        node.Item.Count -= item.Count;
        if (node.Item.Count <= 0)
        {
          if (this.mode == ShopViewer.ItemListController.Mode.Normal)
            this._itemListUI.RemoveItemNode(sel);
          this._itemListUI.ForceSetNonSelect();
        }
        this._itemListUI.Refresh();
      }

      private ItemNodeUI ItemListAddNode(StuffItem item, ShopViewer.ExtraPadding padding)
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

      public enum Mode
      {
        Normal,
        Vendor,
        VendorSpecial,
      }
    }
  }
}
