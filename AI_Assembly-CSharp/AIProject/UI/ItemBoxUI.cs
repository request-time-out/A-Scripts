// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemBoxUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Illusion.Game.Extensions;
using Manager;
using Sirenix.OdinInspector;
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
  public class ItemBoxUI : MenuUIBehaviour
  {
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private ReactiveProperty<ItemBoxUI.SelectedElement> _focusElement = new ReactiveProperty<ItemBoxUI.SelectedElement>(ItemBoxUI.SelectedElement.Inventory);
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ItemBoxUI.ItemBoxDataPack _inventoryUI;
    [SerializeField]
    private ItemBoxUI.ItemBoxDataPack _itemBoxUI;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private ItemSendPanelUI _itemSendPanel;
    [SerializeField]
    private Button _allSendButton;
    private ItemBoxUI.ItemBoxDataPack[] _itemPacks;
    private ItemBoxUI.SelectedElement currentSelected;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuUIList;

    public PlaySE playSE { get; } = new PlaySE(false);

    [DebuggerHidden]
    public virtual IEnumerator SetStorage(
      ItemBoxUI.ItemBoxDataPack pack,
      Action<List<StuffItem>> action)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemBoxUI.\u003CSetStorage\u003Ec__Iterator0()
      {
        pack = pack,
        action = action
      };
    }

    public virtual void ViewCategorize(
      out int[] categorize,
      out List<StuffItem> viewList,
      List<StuffItem> itemList)
    {
      categorize = new int[0];
      viewList = itemList;
    }

    private int _selectedIndexOf { get; set; }

    private bool isSendAll { get; set; }

    private ItemBoxUI.ItemBoxDataPack[] itemPacks
    {
      get
      {
        return ((object) this).GetCache<ItemBoxUI.ItemBoxDataPack[]>(ref this._itemPacks, (Func<ItemBoxUI.ItemBoxDataPack[]>) (() => new ItemBoxUI.ItemBoxDataPack[2]
        {
          this._inventoryUI,
          this._itemBoxUI
        }));
      }
    }

    private ItemBoxUI.ItemBoxDataPack currentPack
    {
      get
      {
        return this.itemPacks[(int) this.currentSelected];
      }
    }

    private MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._itemSendPanel
        }).Concat<MenuUIBehaviour>(((IEnumerable<ItemBoxUI.ItemBoxDataPack>) this.itemPacks).SelectMany<ItemBoxUI.ItemBoxDataPack, MenuUIBehaviour>((Func<ItemBoxUI.ItemBoxDataPack, IEnumerable<MenuUIBehaviour>>) (p => (IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[3]
        {
          (MenuUIBehaviour) p.categoryUI,
          (MenuUIBehaviour) p.itemListUI,
          (MenuUIBehaviour) p.itemSortUI
        }))).Where<MenuUIBehaviour>((Func<MenuUIBehaviour, bool>) (p => Object.op_Inequality((Object) p, (Object) null))).ToArray<MenuUIBehaviour>()));
      }
    }

    private void ViewerCursorOFF()
    {
      foreach (ItemBoxUI.ItemBoxDataPack itemPack in this.itemPacks)
        ((Behaviour) itemPack.cursor).set_enabled(false);
    }

    private void SortUIClose()
    {
      foreach (ItemBoxUI.ItemBoxDataPack itemPack in this.itemPacks)
        itemPack.itemSortUI.Close();
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemBoxUI.\u003CBindingUI\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SetActiveControl(bool isActive)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (isActive)
      {
        MapUIContainer.SetVisibleHUD(false);
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        MapUIContainer.SetVisibleHUD(true);
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.DoClose();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    private void Select(ItemBoxUI.ItemBoxDataPack itemPack)
    {
      ItemNodeUI selectedOption = itemPack.itemListUI.SelectedOption;
      if (Object.op_Equality((Object) selectedOption, (Object) null) || !selectedOption.IsInteractable)
        return;
      this._selectedIndexOf = itemPack.itemListUI.CurrentID;
      if (selectedOption.Item == null)
        return;
      this._itemSendPanel.takeout = itemPack.sel == ItemBoxUI.SelectedElement.ItemBox;
      foreach (ItemBoxUI.ItemBoxDataPack itemBoxDataPack in ((IEnumerable<ItemBoxUI.ItemBoxDataPack>) this.itemPacks).Where<ItemBoxUI.ItemBoxDataPack>((Func<ItemBoxUI.ItemBoxDataPack, bool>) (p => p != itemPack)))
        itemBoxDataPack.itemListUI.ForceSetNonSelect();
      this.SortUIClose();
      this._itemSendPanel.Open(selectedOption);
    }

    private void Close()
    {
      Time.set_timeScale(1f);
      this.IsActiveControl = false;
      this.playSE.Play(SoundPack.SystemSE.BoxClose);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemBoxUI.\u003CDoOpen\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemBoxUI.\u003CDoClose\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void Send(int count, ItemBoxUI.SelectedElement element)
    {
      if (element != ItemBoxUI.SelectedElement.ItemBox)
      {
        if (element != ItemBoxUI.SelectedElement.Inventory)
          return;
        this.SendToItemBox(count);
      }
      else
        this.SendToInventory(count);
    }

    private void SendToItemBox(int count)
    {
      this.Send(this._inventoryUI, this._itemBoxUI, count);
    }

    private void SendToInventory(int count)
    {
      this.Send(this._itemBoxUI, this._inventoryUI, count);
    }

    private void Send(
      ItemBoxUI.ItemBoxDataPack sender,
      ItemBoxUI.ItemBoxDataPack receiver,
      int count)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(this._selectedIndexOf);
      StuffItem stuffItem1 = new StuffItem(node.Item);
      int possible;
      if (!((IReadOnlyCollection<StuffItem>) receiver.itemList).CanAddItem(receiver.slotCounter.y, stuffItem1, count, out possible))
        count = this.isSendAll ? possible : 0;
      this.isSendAll = false;
      if (count <= 0)
        return;
      if (receiver.itemList.AddItem(stuffItem1, count))
      {
        receiver.ItemListAddNode(receiver.itemListUI.SearchNotUsedIndex, stuffItem1);
        receiver.ItemListNodeFilter(receiver.categoryUI.CategoryID, true);
      }
      node.Item.Count -= count;
      if (node.Item.Count <= 0)
      {
        count = Mathf.Abs(node.Item.Count);
        sender.itemList.Remove(node.Item);
        sender.itemListUI.RemoveItemNode(this._selectedIndexOf);
        List<StuffItem> stuffItemList = new List<StuffItem>();
        while (count > 0)
        {
          StuffItem[] array = ((IEnumerable<StuffItem>) sender.itemList.FindItems(stuffItem1)).OrderBy<StuffItem, int>((Func<StuffItem, int>) (x => x.Count)).ToArray<StuffItem>();
          if (!((IEnumerable<StuffItem>) array).Any<StuffItem>())
          {
            Debug.LogError((object) string.Format("RemoveCountOver:{0}", (object) Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem1.CategoryID, stuffItem1.ID).Name));
            break;
          }
          foreach (StuffItem stuffItem2 in array)
          {
            int count1 = stuffItem2.Count;
            stuffItem2.Count -= count;
            count -= count1;
            if (stuffItem2.Count <= 0)
              stuffItemList.Add(stuffItem2);
            if (count <= 0)
              break;
          }
        }
        foreach (StuffItem stuffItem2 in stuffItemList)
        {
          sender.itemList.Remove(stuffItem2);
          foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in (IEnumerable<KeyValuePair<int, ItemNodeUI>>) sender.itemListUI.optionTable)
          {
            if (stuffItem2 == keyValuePair.Value.Item)
            {
              sender.itemListUI.RemoveItemNode(keyValuePair.Key);
              break;
            }
          }
        }
        this.SetFocusLevel(sender.itemListUI.FocusLevel);
        sender.itemListUI.ForceSetNonSelect();
      }
      receiver.Refresh();
      sender.Refresh();
      this._itemSendPanel.Refresh();
    }

    private void CategoryAllSend(
      ItemBoxUI.ItemBoxDataPack sender,
      ItemBoxUI.ItemBoxDataPack receiver)
    {
      List<ItemNodeUI> itemNodeUiList = new List<ItemNodeUI>();
      foreach (ItemNodeUI itemVisible in sender.itemListUI.ItemVisibles)
      {
        StuffItem stuffItem = new StuffItem(itemVisible.Item);
        int count = stuffItem.Count;
        int possible;
        if (!((IReadOnlyCollection<StuffItem>) receiver.itemList).CanAddItem(receiver.slotCounter.y, stuffItem, count, out possible))
          count = possible;
        if (count > 0)
        {
          if (receiver.itemList.AddItem(stuffItem, count))
          {
            receiver.ItemListAddNode(receiver.itemListUI.SearchNotUsedIndex, stuffItem);
            receiver.ItemListNodeFilter(receiver.categoryUI.CategoryID, true);
          }
          itemVisible.Item.Count -= count;
          if (itemVisible.Item.Count <= 0)
          {
            itemVisible.Item.Count = 0;
            itemNodeUiList.Add(itemVisible);
          }
        }
      }
      foreach (ItemNodeUI node in itemNodeUiList)
      {
        sender.itemList.Remove(node.Item);
        sender.itemListUI.RemoveItemNode(sender.itemListUI.SearchIndex(node));
      }
      this.SetFocusLevel(sender.itemListUI.FocusLevel);
      sender.itemListUI.ForceSetNonSelect();
      receiver.Refresh();
      sender.Refresh();
      this._itemSendPanel.Refresh();
    }

    private void OnUpdate()
    {
      if ((double) this._canvasGroup.get_alpha() == 0.0 || this._itemSendPanel.isOpen || ((IEnumerable<ItemBoxUI.ItemBoxDataPack>) this.itemPacks).Any<ItemBoxUI.ItemBoxDataPack>((Func<ItemBoxUI.ItemBoxDataPack, bool>) (item => item.itemSortUI.isOpen)))
        return;
      bool flag = false;
      if (Singleton<Input>.Instance.IsPressedAxis(ActionID.LeftShoulder2))
      {
        this._focusElement.set_Value(ItemBoxUI.SelectedElement.Inventory);
        flag = true;
      }
      else if (Singleton<Input>.Instance.IsPressedAxis(ActionID.RightShoulder2))
      {
        this._focusElement.set_Value(ItemBoxUI.SelectedElement.ItemBox);
        flag = true;
      }
      if (!flag)
        return;
      this.ViewerCursorOFF();
      ItemFilterCategoryUI categoryUi = this.currentPack.categoryUI;
      this.SetFocusLevel(categoryUi.FocusLevel);
      categoryUi.SelectedID = categoryUi.CategoryID;
      categoryUi.useCursor = true;
    }

    private void SetCursorFocus(
      Image cursor,
      ItemFilterCategoryUI categoryUI,
      Selectable selectable)
    {
      categoryUI.useCursor = false;
      CursorFrame.Set(((Graphic) cursor).get_rectTransform(), (RectTransform) ((Component) selectable).GetComponent<RectTransform>(), (RectTransform) null);
      foreach (ItemBoxUI.ItemBoxDataPack itemBoxDataPack in ((IEnumerable<ItemBoxUI.ItemBoxDataPack>) this.itemPacks).Where<ItemBoxUI.ItemBoxDataPack>((Func<ItemBoxUI.ItemBoxDataPack, bool>) (p => Object.op_Inequality((Object) p.cursor, (Object) cursor))))
        ((Behaviour) itemBoxDataPack.cursor).set_enabled(false);
      ((Behaviour) cursor).set_enabled(true);
      this.SetFocusLevel(categoryUI.FocusLevel);
    }

    private new void SetFocusLevel(int level)
    {
      Singleton<Input>.Instance.FocusLevel = level;
      foreach (ItemBoxUI.ItemBoxDataPack itemPack in this.itemPacks)
      {
        if (itemPack.sel == this._focusElement.get_Value())
        {
          itemPack.itemSortUI.EnabledInput = level == itemPack.itemSortUI.FocusLevel;
          itemPack.itemListUI.EnabledInput = level == itemPack.itemListUI.FocusLevel;
          itemPack.categoryUI.EnabledInput = level == itemPack.categoryUI.FocusLevel;
        }
        else
        {
          itemPack.itemSortUI.EnabledInput = false;
          itemPack.itemListUI.EnabledInput = false;
          itemPack.categoryUI.EnabledInput = false;
        }
      }
      this._itemSendPanel.EnabledInput = level == this._itemSendPanel.FocusLevel;
      ((Behaviour) this._itemSendPanel.cursor).set_enabled(this._itemSendPanel.EnabledInput);
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }

    [Serializable]
    public class ItemBoxDataPack
    {
      [SerializeField]
      private Image _itemListPanel;
      [SerializeField]
      private Transform _sortUIPanel;
      [SerializeField]
      private Sprite _iconSprite;
      [SerializeField]
      private string _iconText;
      [SerializeField]
      private InventoryViewer _viewer;

      public event Action<int, ItemNodeUI, ItemBoxUI.SelectedElement> ItemNodeOnDoubleClick;

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

      public ItemSortUI itemSortUI
      {
        get
        {
          return this._viewer.sortUI;
        }
      }

      public Toggle sorter
      {
        get
        {
          return this._viewer.sorter;
        }
      }

      public Image cursor
      {
        get
        {
          return this._viewer.cursor;
        }
      }

      public Button sortButton
      {
        get
        {
          return this._viewer.sortButton;
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

      public List<StuffItem> itemList { get; private set; }

      public ItemBoxUI.SelectedElement sel { get; private set; }

      public Image itemListPanel
      {
        get
        {
          return this._itemListPanel;
        }
      }

      private ItemBoxUI itemBoxUI { get; set; }

      [DebuggerHidden]
      public IEnumerator Initialize(ItemBoxUI itemBoxUI, ItemBoxUI.SelectedElement sel)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new ItemBoxUI.ItemBoxDataPack.\u003CInitialize\u003Ec__Iterator0()
        {
          itemBoxUI = itemBoxUI,
          sel = sel,
          \u0024this = this
        };
      }

      public void Refresh()
      {
        this._viewer.itemListUI.Refresh();
        this._viewer.slotCounter.x = this.itemList.Count;
      }

      public void ItemListNodeCreate()
      {
        int[] categorize;
        List<StuffItem> viewList;
        this.itemBoxUI.ViewCategorize(out categorize, out viewList, this.itemList);
        this._viewer.itemListUI.ClearItems();
        this._viewer.sortUI.SetDefault();
        this._viewer.sortUI.Close();
        this._viewer.sorter.set_isOn(true);
        this._viewer.categoryUI.Filter(categorize);
        // ISSUE: object of a compiler-generated type is created
        foreach (\u003C\u003E__AnonType20<StuffItem, int> anonType20 in Enumerable.Range(0, viewList.Count).Where<int>((Func<int, bool>) (i => !((IEnumerable<int>) categorize).Any<int>() || ((IEnumerable<int>) categorize).Contains<int>(viewList[i].CategoryID))).Select<int, \u003C\u003E__AnonType20<StuffItem, int>>((Func<int, int, \u003C\u003E__AnonType20<StuffItem, int>>) ((i, index) => new \u003C\u003E__AnonType20<StuffItem, int>(viewList[i], index))))
          this.ItemListAddNode(anonType20.index, anonType20.item);
        int category = ((IEnumerable<int>) this._viewer.categoryUI.Visibles).FirstOrDefault<int>();
        this._viewer.categoryUI.SetSelectAndCategory(category);
        this.ItemListNodeFilter(category, true);
      }

      public ItemNodeUI ItemListAddNode(int index, StuffItem item)
      {
        ItemNodeUI node = this._viewer.itemListUI.AddItemNode(index, item);
        if (Object.op_Inequality((Object) node, (Object) null))
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<IList<double>>((IObservable<M0>) UnityEventExtensions.AsObservable((UnityEvent) node.OnClick).DoubleInterval<Unit>(250f, false), (Action<M0>) (_ =>
          {
            if (this.ItemNodeOnDoubleClick == null)
              return;
            this.ItemNodeOnDoubleClick(index, node, this.sel);
          })), (Component) node);
        return node;
      }

      public void ItemListNodeFilter(int category, bool isSort)
      {
        this._viewer.itemListUI.Filter(category);
        this._viewer.itemListUI.ForceSetNonSelect();
        if (!isSort)
          return;
        this._viewer.SortItemList();
      }
    }

    public enum SelectedElement
    {
      Inventory,
      ItemBox,
    }
  }
}
