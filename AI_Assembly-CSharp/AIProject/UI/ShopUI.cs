// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ShopUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ShopUI : MenuUIBehaviour
  {
    [Header("ShopUI Setting")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ShopTagSelectionUI _shopTagSelection;
    [Header("ShopViewer")]
    [SerializeField]
    private GameObject _shopVisible;
    [SerializeField]
    private ShopViewer _shopViewer;
    [Header("InventoryViewer")]
    [SerializeField]
    private GameObject _inventoryVisible;
    [SerializeField]
    private ShopUI.InventoryUI _inventoryUI;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private ShopInfoPanelUI _shopInfoPanelUI;
    [SerializeField]
    private ShopRateViewer _shopRateViewer;
    [SerializeField]
    private ShopSendViewer _shopSendViewer;
    [SerializeField]
    private Button _tradeButton;
    [Header("WarningViewer")]
    [SerializeField]
    private RectTransform _warningViewerLayout;
    [SerializeField]
    private WarningViewer _warningViewer;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuUIList;
    private MenuUIBehaviour[] _shopUIList;
    private MenuUIBehaviour[] _inventoryUIList;
    private MenuUIBehaviour[] _subUIList;
    private ShopViewer.ItemListController[] _controllers;

    public PlaySE playSE { get; } = new PlaySE(false);

    public Action OnClose { get; set; }

    public static bool RemoveItem(
      int count,
      int sel,
      StuffItem item,
      ShopViewer.ItemListController sender,
      ShopUI.InventoryUI inventoryUI)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(sel);
      if (sender != inventoryUI.controller)
      {
        sender.RemoveItem(sel, item);
        return false;
      }
      node.Item.Count -= count;
      if (node.Item.Count > 0)
        return false;
      int num = Mathf.Abs(node.Item.Count);
      List<StuffItem> itemList = inventoryUI.itemList;
      itemList.Remove(node.Item);
      sender.itemListUI.RemoveItemNode(sel);
      List<StuffItem> stuffItemList = new List<StuffItem>();
      while (num > 0)
      {
        StuffItem[] array = ((IEnumerable<StuffItem>) itemList.FindItems(item)).OrderBy<StuffItem, int>((Func<StuffItem, int>) (x => x.Count)).ToArray<StuffItem>();
        if (!((IEnumerable<StuffItem>) array).Any<StuffItem>())
        {
          Debug.LogError((object) string.Format("RemoveCountOver:{0}", (object) Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID).Name));
          break;
        }
        foreach (StuffItem stuffItem in array)
        {
          int count1 = stuffItem.Count;
          stuffItem.Count -= num;
          num -= count1;
          if (stuffItem.Count <= 0)
            stuffItemList.Add(stuffItem);
          if (num <= 0)
            break;
        }
      }
      foreach (StuffItem stuffItem in stuffItemList)
      {
        itemList.Remove(stuffItem);
        foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in (IEnumerable<KeyValuePair<int, ItemNodeUI>>) sender.itemListUI.optionTable)
        {
          if (stuffItem == keyValuePair.Value.Item)
          {
            sender.itemListUI.RemoveItemNode(keyValuePair.Key);
            break;
          }
        }
      }
      sender.itemListUI.ForceSetNonSelect();
      return true;
    }

    private MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._shopInfoPanelUI
        }).Concat<MenuUIBehaviour>((IEnumerable<MenuUIBehaviour>) this.shopUIList).Concat<MenuUIBehaviour>((IEnumerable<MenuUIBehaviour>) this.inventoryUIList).Concat<MenuUIBehaviour>((IEnumerable<MenuUIBehaviour>) this.subUIList).ToArray<MenuUIBehaviour>()));
      }
    }

    private MenuUIBehaviour[] shopUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._shopUIList, (Func<MenuUIBehaviour[]>) (() => new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this._shopViewer.controllers[0].itemListUI,
          (MenuUIBehaviour) this._shopViewer.controllers[1].itemListUI
        }));
      }
    }

    private MenuUIBehaviour[] inventoryUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._inventoryUIList, (Func<MenuUIBehaviour[]>) (() => new MenuUIBehaviour[3]
        {
          (MenuUIBehaviour) this._inventoryUI.categoryUI,
          (MenuUIBehaviour) this._inventoryUI.itemListUI,
          (MenuUIBehaviour) this._inventoryUI.itemSortUI
        }));
      }
    }

    private MenuUIBehaviour[] subUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._subUIList, (Func<MenuUIBehaviour[]>) (() => new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this._shopRateViewer.controller.itemListUI,
          (MenuUIBehaviour) this._shopSendViewer.controller.itemListUI
        }));
      }
    }

    private ShopViewer.ItemListController[] controllers
    {
      get
      {
        return ((object) this).GetCache<ShopViewer.ItemListController[]>(ref this._controllers, (Func<ShopViewer.ItemListController[]>) (() => new ShopViewer.ItemListController[5]
        {
          this._shopViewer.controllers[0],
          this._shopViewer.controllers[1],
          this._inventoryUI.controller,
          this._shopRateViewer.controller,
          this._shopSendViewer.controller
        }));
      }
    }

    private int openAreaID
    {
      get
      {
        return this.merchant.OpenAreaID;
      }
    }

    private MerchantActor merchant
    {
      get
      {
        return !Singleton<Manager.Map>.IsInstance() ? (MerchantActor) null : Singleton<Manager.Map>.Instance.Merchant;
      }
    }

    private bool initialized { get; set; }

    private bool isTradeCheckActive { get; set; }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__7)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__8)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopUI.\u003CBindingUI\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void ItemDecideProc(
      int count,
      int sel,
      ShopViewer.ItemListController sender,
      ShopViewer.ItemListController receiver)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(sel);
      MerchantData.VendorItem source = node.Item as MerchantData.VendorItem;
      StuffItem stuffItem = this._shopInfoPanelUI.mode != ShopInfoPanelUI.Mode.Shop ? new StuffItem(node.Item) : (StuffItem) new MerchantData.VendorItem(source);
      stuffItem.Count = count;
      receiver.AddItem(stuffItem, new ShopViewer.ExtraPadding(node.Item, sender));
      if (ShopUI.RemoveItem(count, sel, stuffItem, sender, this._inventoryUI))
        this.SetFocusLevel(sender.itemListUI.FocusLevel);
      bool flag1 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) receiver.itemListUI);
      if (!flag1)
        receiver.itemListUI.Refresh();
      bool flag2 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) sender.itemListUI);
      if (!flag2)
        sender.itemListUI.Refresh();
      if (flag1 || flag2)
        this._inventoryUI.Refresh();
      this._shopInfoPanelUI.Refresh();
      int num = node.Rate * count;
      if (receiver == this._shopSendViewer.controller)
      {
        this._shopRateViewer.rateCounter.x += num;
      }
      else
      {
        if (receiver != this._shopRateViewer.controller)
          return;
        this._shopRateViewer.rateCounter.y += num;
      }
    }

    private void ItemReturnProc(
      int count,
      int sel,
      ShopViewer.ItemListController sender,
      ShopViewer.ItemListController receiver)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(sel);
      MerchantData.VendorItem source = node.Item as MerchantData.VendorItem;
      StuffItem stuffItem = this._shopInfoPanelUI.mode != ShopInfoPanelUI.Mode.Shop ? new StuffItem(node.Item) : (StuffItem) new MerchantData.VendorItem(source);
      stuffItem.Count = count;
      sender.RemoveItem(sel, stuffItem);
      ShopViewer.ExtraPadding extraData = node.extraData as ShopViewer.ExtraPadding;
      receiver = extraData.source;
      if (receiver != this._inventoryUI.controller)
        receiver.AddItem(stuffItem, new ShopViewer.ExtraPadding(extraData.item, sender));
      else if (this._inventoryUI.itemList.AddItem(stuffItem))
      {
        this._inventoryUI.ItemListAddNode(this._inventoryUI.itemListUI.SearchNotUsedIndex, stuffItem);
        this._inventoryUI.ItemListNodeFilter(this._inventoryUI.categoryUI.CategoryID, true);
      }
      bool flag1 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) receiver.itemListUI);
      if (!flag1)
        receiver.itemListUI.Refresh();
      bool flag2 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) sender.itemListUI);
      if (!flag2)
        sender.itemListUI.Refresh();
      if (flag1 || flag2)
        this._inventoryUI.Refresh();
      this._shopInfoPanelUI.Refresh();
      int num = node.Rate * count;
      if (sender == this._shopSendViewer.controller)
      {
        this._shopRateViewer.rateCounter.x -= num;
      }
      else
      {
        if (sender != this._shopRateViewer.controller)
          return;
        this._shopRateViewer.rateCounter.y -= num;
      }
    }

    private bool InInventoryPossibleCheck()
    {
      List<StuffItem> list = this._inventoryUI.itemList.Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (item => new StuffItem(item))).ToList<StuffItem>();
      int y = this._inventoryUI.slotCounter.y;
      Resources.GameInfoTables gameInfo = Singleton<Resources>.Instance.GameInfo;
      // ISSUE: object of a compiler-generated type is created
      ILookup<int, \u003C\u003E__AnonType26<StuffItem, int>> lookup = list.Select<StuffItem, \u003C\u003E__AnonType26<StuffItem, int>>((Func<StuffItem, \u003C\u003E__AnonType26<StuffItem, int>>) (item => new \u003C\u003E__AnonType26<StuffItem, int>(item, gameInfo.GetItem(item.CategoryID, item.ID).nameHash))).ToLookup<\u003C\u003E__AnonType26<StuffItem, int>, int>((Func<\u003C\u003E__AnonType26<StuffItem, int>, int>) (p => p.nameHash));
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      List<StuffItem> stuffItemList = new List<StuffItem>();
      foreach (IGrouping<int, \u003C\u003E__AnonType26<StuffItem, int>> source in (IEnumerable<IGrouping<int, \u003C\u003E__AnonType26<StuffItem, int>>>) lookup)
      {
        int num = source.Sum<\u003C\u003E__AnonType26<StuffItem, int>>((Func<\u003C\u003E__AnonType26<StuffItem, int>, int>) (x => x.item.Count));
        foreach (\u003C\u003E__AnonType26<StuffItem, int> anonType26 in (IEnumerable<\u003C\u003E__AnonType26<StuffItem, int>>) source)
        {
          if (num > itemSlotMax)
          {
            anonType26.item.Count = itemSlotMax;
            num -= itemSlotMax;
          }
          else
          {
            anonType26.item.Count = num;
            num = 0;
          }
        }
        stuffItemList.AddRange(source.Where<\u003C\u003E__AnonType26<StuffItem, int>>((Func<\u003C\u003E__AnonType26<StuffItem, int>, bool>) (x => x.item.Count <= 0)).Select<\u003C\u003E__AnonType26<StuffItem, int>, StuffItem>((Func<\u003C\u003E__AnonType26<StuffItem, int>, StuffItem>) (x => x.item)));
      }
      foreach (StuffItem stuffItem in stuffItemList)
        list.Remove(stuffItem);
      foreach (ItemNodeUI itemNodeUi in this._shopRateViewer.itemListUI)
      {
        StuffItem source = itemNodeUi.Item;
        if (!((IReadOnlyCollection<StuffItem>) list).CanAddItem(y, source))
          return false;
        list.AddItem(new StuffItem(source));
      }
      return y >= list.Count;
    }

    private void Trade()
    {
      foreach (ItemNodeUI itemNodeUi in this._shopRateViewer.itemListUI)
      {
        StuffItem stuffItem = new StuffItem(itemNodeUi.Item);
        if (this._inventoryUI.itemList.AddItem(stuffItem))
        {
          this._inventoryUI.ItemListAddNode(this._inventoryUI.itemListUI.SearchNotUsedIndex, stuffItem);
          this._inventoryUI.ItemListNodeFilter(this._inventoryUI.categoryUI.CategoryID, true);
        }
      }
      ILookup<int, \u003C\u003E__AnonType24<StuffItem, int, int>> lookup = this._inventoryUI.itemList.Select<StuffItem, \u003C\u003E__AnonType24<StuffItem, int, int>>((Func<StuffItem, \u003C\u003E__AnonType24<StuffItem, int, int>>) (item =>
      {
        int id = -1;
        foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in (IEnumerable<KeyValuePair<int, ItemNodeUI>>) this._inventoryUI.itemListUI.optionTable)
        {
          if (keyValuePair.Value.Item == item)
          {
            id = keyValuePair.Key;
            break;
          }
        }
        // ISSUE: object of a compiler-generated type is created
        return new \u003C\u003E__AnonType24<StuffItem, int, int>(item, Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID).nameHash, id);
      })).ToLookup<\u003C\u003E__AnonType24<StuffItem, int, int>, int>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, int>) (p => p.nameHash));
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      List<StuffItem> stuffItemList = new List<StuffItem>();
      foreach (IGrouping<int, \u003C\u003E__AnonType24<StuffItem, int, int>> source in (IEnumerable<IGrouping<int, \u003C\u003E__AnonType24<StuffItem, int, int>>>) lookup)
      {
        int num = source.Sum<\u003C\u003E__AnonType24<StuffItem, int, int>>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, int>) (x => x.item.Count));
        foreach (\u003C\u003E__AnonType24<StuffItem, int, int> anonType24 in (IEnumerable<\u003C\u003E__AnonType24<StuffItem, int, int>>) source)
        {
          if (num > itemSlotMax)
          {
            anonType24.item.Count = itemSlotMax;
            num -= itemSlotMax;
          }
          else
          {
            anonType24.item.Count = num;
            num = 0;
          }
        }
        \u003C\u003E__AnonType24<StuffItem, int, int>[] array = source.Where<\u003C\u003E__AnonType24<StuffItem, int, int>>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, bool>) (x => x.item.Count <= 0)).ToArray<\u003C\u003E__AnonType24<StuffItem, int, int>>();
        foreach (\u003C\u003E__AnonType24<StuffItem, int, int> anonType24 in array)
          this._inventoryUI.itemListUI.RemoveItemNode(anonType24.id);
        stuffItemList.AddRange(((IEnumerable<\u003C\u003E__AnonType24<StuffItem, int, int>>) array).Select<\u003C\u003E__AnonType24<StuffItem, int, int>, StuffItem>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, StuffItem>) (x => x.item)));
      }
      foreach (StuffItem stuffItem in stuffItemList)
        this._inventoryUI.itemList.Remove(stuffItem);
      Singleton<Manager.Map>.Instance.Player.PlayerData.SpendMoney += this._shopRateViewer.rateCounter.x;
      this._shopRateViewer.controller.Clear();
      this._shopSendViewer.controller.Clear();
      this._shopRateViewer.rateCounter.x = 0;
      this._shopRateViewer.rateCounter.y = 0;
      this._inventoryUI.Refresh();
      this.playSE.Play(SoundPack.SystemSE.Shop);
    }

    private void Reverse()
    {
      foreach (ItemNodeUI itemNodeUi in this._shopRateViewer.itemListUI)
      {
        ShopViewer.ExtraPadding extraData = itemNodeUi.extraData as ShopViewer.ExtraPadding;
        ShopViewer.ItemListController source = extraData.source;
        StuffItem stuffItem = itemNodeUi.Item;
        if (source == this._shopViewer.controllers[0])
          this._shopViewer.controllers[0].AddItem(stuffItem, extraData);
        else if (source == this._shopViewer.controllers[1])
          this._shopViewer.controllers[1].AddItem(stuffItem, extraData);
        else
          Debug.LogError((object) string.Format("ItemNode ExtraData not ShopViewer:{0}", (object) source));
      }
      this._shopRateViewer.controller.Clear();
      foreach (ItemNodeUI itemNodeUi in this._shopSendViewer.itemListUI)
      {
        StuffItem stuffItem = itemNodeUi.Item;
        if (this._inventoryUI.itemList.AddItem(stuffItem))
        {
          this._inventoryUI.ItemListAddNode(this._inventoryUI.itemListUI.SearchNotUsedIndex, stuffItem);
          this._inventoryUI.ItemListNodeFilter(this._inventoryUI.categoryUI.CategoryID, true);
        }
      }
      this._shopSendViewer.controller.Clear();
      this._shopRateViewer.rateCounter.x = 0;
      this._shopRateViewer.rateCounter.y = 0;
    }

    private void SetActiveControl(bool isActive)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (isActive)
      {
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = 0;
        coroutine = this.DoClose();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    private void Close()
    {
      Time.set_timeScale(1f);
      this.IsActiveControl = false;
      Action onClose = this.OnClose;
      if (onClose != null)
        onClose();
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private new void SetFocusLevel(int level)
    {
      Singleton<Input>.Instance.FocusLevel = level;
      MenuUIBehaviour[] menuUiBehaviourArray1 = (MenuUIBehaviour[]) null;
      MenuUIBehaviour[] menuUiBehaviourArray2 = (MenuUIBehaviour[]) null;
      int index = this._shopTagSelection.Index;
      switch (index)
      {
        case 0:
          menuUiBehaviourArray1 = this.shopUIList;
          menuUiBehaviourArray2 = this.inventoryUIList;
          break;
        case 1:
          menuUiBehaviourArray1 = this.inventoryUIList;
          menuUiBehaviourArray2 = this.shopUIList;
          break;
        default:
          Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (SetFocusLevel), (object) index));
          break;
      }
      if (menuUiBehaviourArray1 == null || menuUiBehaviourArray2 == null)
        return;
      foreach (MenuUIBehaviour menuUiBehaviour in menuUiBehaviourArray1)
        menuUiBehaviour.EnabledInput = true;
      foreach (MenuUIBehaviour menuUiBehaviour in menuUiBehaviourArray2)
        menuUiBehaviour.EnabledInput = false;
      foreach (MenuUIBehaviour subUi in this.subUIList)
        subUi.EnabledInput = true;
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }

    [Serializable]
    public class InventoryUI : InventoryFacadeViewer
    {
      public ItemSortUI itemSortUI
      {
        get
        {
          return this.viewer.sortUI;
        }
      }

      public Toggle sorter
      {
        get
        {
          return this.viewer.sorter;
        }
      }

      public Button sortButton
      {
        get
        {
          return this.viewer.sortButton;
        }
      }

      public ShopViewer.ItemListController controller { get; } = new ShopViewer.ItemListController();

      [DebuggerHidden]
      public override IEnumerator Initialize()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new ShopUI.InventoryUI.\u003CInitialize\u003Ec__Iterator0()
        {
          \u0024this = this
        };
      }

      public override void ItemListNodeCreate()
      {
        int[] categorize = new int[0];
        this.viewer.itemListUI.ClearItems();
        this.viewer.sortUI.SetDefault();
        this.viewer.sortUI.Close();
        this.viewer.sorter.set_isOn(true);
        this.viewer.categoryUI.Filter(categorize);
        // ISSUE: object of a compiler-generated type is created
        List<StuffItem> viewList = this.itemList.Select<StuffItem, \u003C\u003E__AnonType25<StuffItem, StuffItemInfo>>((Func<StuffItem, \u003C\u003E__AnonType25<StuffItem, StuffItemInfo>>) (item => new \u003C\u003E__AnonType25<StuffItem, StuffItemInfo>(item, Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID)))).Select<\u003C\u003E__AnonType25<StuffItem, StuffItemInfo>, StuffItem>((Func<\u003C\u003E__AnonType25<StuffItem, StuffItemInfo>, StuffItem>) (p => p.item)).ToList<StuffItem>();
        // ISSUE: object of a compiler-generated type is created
        foreach (\u003C\u003E__AnonType20<StuffItem, int> anonType20 in Enumerable.Range(0, viewList.Count).Where<int>((Func<int, bool>) (i => !((IEnumerable<int>) categorize).Any<int>() || ((IEnumerable<int>) categorize).Contains<int>(viewList[i].CategoryID))).Select<int, \u003C\u003E__AnonType20<StuffItem, int>>((Func<int, int, \u003C\u003E__AnonType20<StuffItem, int>>) ((i, index) => new \u003C\u003E__AnonType20<StuffItem, int>(viewList[i], index))))
          this.ItemListAddNode(anonType20.index, anonType20.item);
        int category = 0;
        this.viewer.categoryUI.SetSelectAndCategory(category);
        this.ItemListNodeFilter(category, true);
      }

      protected override void CanvasGroupSetting()
      {
      }
    }
  }
}
