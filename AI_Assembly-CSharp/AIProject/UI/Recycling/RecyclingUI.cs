// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI.Recycling
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class RecyclingUI : MenuUIBehaviour
  {
    [SerializeField]
    private List<int> _adultCategoryIDList = new List<int>();
    [SerializeField]
    private List<ToggleElement> _toggleElements = new List<ToggleElement>();
    private int _craftPointID = -1;
    [SerializeField]
    private CanvasGroup _rootCanvasGroup;
    [SerializeField]
    private RectTransform _rootRectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private RecyclingInventoryFacadeViewer _pouchInventoryUI;
    [SerializeField]
    private RecyclingInventoryFacadeViewer _chestInventoryUI;
    [SerializeField]
    private RecyclingInfoPanelUI _infoPanelUI;
    [SerializeField]
    private RecyclingDecidedItemSlotUI _decidedItemSlotUI;
    [SerializeField]
    private RecyclingCreateItemStockUI _createItemStockUI;
    [SerializeField]
    private RecyclingCreatePanelUI _createPanelUI;
    [SerializeField]
    private RecyclingItemDeleteRequestUI _deleteRequestUI;
    [SerializeField]
    private RectTransform _warningViewerLayout;
    [SerializeField]
    private WarningViewer _pouchWarningViewer;
    [SerializeField]
    private WarningViewer _chestWarningViewer;
    [SerializeField]
    private WarningViewer _pouchAndChestWarningViewer;
    private RecyclingInventoryFacadeViewer[] _vieweres;
    private int _prevFocusLevel;
    private IDisposable _fadeDisposable;

    public WarningViewer[] WarningUIs { get; private set; } = new WarningViewer[0];

    public RecyclingInfoPanelUI InfoPanelUI
    {
      get
      {
        return this._infoPanelUI;
      }
    }

    public RecyclingDecidedItemSlotUI DecidedItemSlotUI
    {
      get
      {
        return this._decidedItemSlotUI;
      }
    }

    public RecyclingCreateItemStockUI CreateItemStockUI
    {
      get
      {
        return this._createItemStockUI;
      }
    }

    public RecyclingCreatePanelUI CreatePanelUI
    {
      get
      {
        return this._createPanelUI;
      }
    }

    public RecyclingItemDeleteRequestUI DeleteREquestUI
    {
      get
      {
        return this._deleteRequestUI;
      }
    }

    private void CursorOff(RecyclingInventoryFacadeViewer viewer)
    {
      ((Behaviour) viewer.cursor).set_enabled(false);
    }

    public RecyclingInventoryFacadeViewer[] InventoryUIs
    {
      get
      {
        return this._vieweres;
      }
    }

    public RecyclingInventoryFacadeViewer SelectedInventoryUI { get; private set; }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null) ? this._rootCanvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null))
          return;
        this._rootCanvasGroup.set_alpha(value);
      }
    }

    public bool BlockRaycast
    {
      get
      {
        return Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null) && this._rootCanvasGroup.get_blocksRaycasts();
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null) || this._rootCanvasGroup.get_blocksRaycasts() == value)
          return;
        this._rootCanvasGroup.set_blocksRaycasts(value);
      }
    }

    public bool Interactable
    {
      get
      {
        return Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null) && this._rootCanvasGroup.get_interactable();
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._rootCanvasGroup, (Object) null) || this._rootCanvasGroup.get_interactable() == value)
          return;
        this._rootCanvasGroup.set_interactable(value);
      }
    }

    public MenuUIBehaviour[] MenuUIBehaviours { get; private set; } = new MenuUIBehaviour[0];

    public ItemListUI[] ItemListUIs { get; private set; } = new ItemListUI[0];

    public MenuUIBehaviour[] ItemListUIBehaviours { get; private set; } = new MenuUIBehaviour[0];

    public ItemListController[] ListControllers { get; private set; } = new ItemListController[0];

    public InventoryFacadeViewer.ItemFilter[] _itemFilter { get; private set; }

    public bool Initialized { get; private set; }

    public Action OnClosedEvent { get; set; }

    public RecyclingData RecyclingData { get; private set; }

    public int CraftPointID { get; private set; } = -1;

    public Subject<RecyclingInventoryFacadeViewer> OnInventoryChanged { get; } = new Subject<RecyclingInventoryFacadeViewer>();

    public Subject<StuffItem> OnSendItem { get; } = new Subject<StuffItem>();

    public Subject<PanelType> OnDoubleClicked { get; } = new Subject<PanelType>();

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._rootCanvasGroup, (Object) null))
        this._rootCanvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._rootRectTransform, (Object) null))
        this._rootRectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      if (Object.op_Equality((Object) this._infoPanelUI, (Object) null))
        this._infoPanelUI = (RecyclingInfoPanelUI) ((Component) this).GetComponentInChildren<RecyclingInfoPanelUI>(true);
      if (Object.op_Equality((Object) this._decidedItemSlotUI, (Object) null))
        this._decidedItemSlotUI = (RecyclingDecidedItemSlotUI) ((Component) this).GetComponentInChildren<RecyclingDecidedItemSlotUI>(true);
      if (Object.op_Equality((Object) this._createItemStockUI, (Object) null))
        this._createItemStockUI = (RecyclingCreateItemStockUI) ((Component) this).GetComponentInChildren<RecyclingCreateItemStockUI>(true);
      if (Object.op_Equality((Object) this._createPanelUI, (Object) null))
        this._createPanelUI = (RecyclingCreatePanelUI) ((Component) this).GetComponentInChildren<RecyclingCreatePanelUI>(true);
      if (Object.op_Equality((Object) this._rootCanvasGroup, (Object) null) || Object.op_Equality((Object) this._rootRectTransform, (Object) null) || (Object.op_Equality((Object) this._infoPanelUI, (Object) null) || Object.op_Equality((Object) this._decidedItemSlotUI, (Object) null)) || (Object.op_Equality((Object) this._createItemStockUI, (Object) null) || !Object.op_Equality((Object) this._createPanelUI, (Object) null)))
        ;
      if (!Singleton<Resources>.IsInstance())
        return;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Singleton<Resources>.Instance.LoadMapResourceStream, (Action<M0>) (__ =>
      {
        IEnumerator settingCoroutine = this.ItemFilterSettingCoroutine();
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => settingCoroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (_ => {}), (Action) (() => this.InventoryUISetting())), (Component) this);
      })), (Component) this);
    }

    protected override void Start()
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Action<M0>) (_ => this.DoClose())), (Component) this);
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._keyCommands.Add(keyCodeDownCommand);
      if (!this._toggleElements.IsNullOrEmpty<ToggleElement>())
      {
        for (int index = 0; index < this._toggleElements.Count; ++index)
        {
          ToggleElement element = this._toggleElements.GetElement<ToggleElement>(index);
          Toggle toggle = element == null ? (Toggle) null : element.Toggle;
          if (!Object.op_Equality((Object) toggle, (Object) null))
          {
            element.Index = index;
            element.Start();
            DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(toggle)), (Func<M0, bool>) (flag => flag)), (Action<M0>) (_ =>
            {
              this.ChangedSelecteInventory(element.Index);
              this.PlaySE(SoundPack.SystemSE.OK_S);
            })), (Component) toggle);
          }
        }
      }
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<StuffItem>(Observable.Where<StuffItem>((IObservable<M0>) this._decidedItemSlotUI.CreateEvent, (Func<M0, bool>) (item => item != null)), (Action<M0>) (item => this._createItemStockUI.AddItem(item))), (Component) this);
      base.Start();
      this.CanvasAlpha = 0.0f;
      bool flag1 = false;
      this.Interactable = flag1;
      this.BlockRaycast = flag1;
      this.SetEnabledInputAll(false);
    }

    [DebuggerHidden]
    private IEnumerator ItemFilterSettingCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003CItemFilterSettingCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void InventoryUISetting()
    {
      IEnumerator pouchCoroutine = this.InventoryUISettingCoroutine(this._pouchInventoryUI);
      IEnumerator chestCoroutine = this.InventoryUISettingCoroutine(this._chestInventoryUI);
      IConnectableObservable<Unit> iconnectableObservable1 = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => pouchCoroutine), false));
      IConnectableObservable<Unit> iconnectableObservable2 = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => chestCoroutine), false));
      iconnectableObservable1.Connect();
      iconnectableObservable2.Connect();
      IEnumerator decidedCoroutine = this.ItemListUISettingCoroutine(this._decidedItemSlotUI.ListController, PanelType.DecidedItem);
      IEnumerator createdCoroutine = this.ItemListUISettingCoroutine(this._createItemStockUI.ListController, PanelType.CreatedItem);
      IConnectableObservable<Unit> iconnectableObservable3 = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => decidedCoroutine), false));
      IConnectableObservable<Unit> iconnectableObservable4 = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => createdCoroutine), false));
      iconnectableObservable3.Connect();
      iconnectableObservable4.Connect();
      IEnumerator warningCoroutine = this.LoadWarningViewerCoroutine();
      IConnectableObservable<Unit> iconnectableObservable5 = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => warningCoroutine), false));
      iconnectableObservable5.Connect();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.WhenAll(new IObservable<Unit>[5]
      {
        (IObservable<Unit>) iconnectableObservable1,
        (IObservable<Unit>) iconnectableObservable2,
        (IObservable<Unit>) iconnectableObservable3,
        (IObservable<Unit>) iconnectableObservable4,
        (IObservable<Unit>) iconnectableObservable5
      }), (Action<M0>) (_ => this.FinishedInventoryUISetting()), (Action<Exception>) (ex => Debug.LogException(ex))), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator InventoryUISettingCoroutine(RecyclingInventoryFacadeViewer viewer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003CInventoryUISettingCoroutine\u003Ec__Iterator1()
      {
        viewer = viewer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator ItemListUISettingCoroutine(
      ItemListController listCon,
      PanelType panelType)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003CItemListUISettingCoroutine\u003Ec__Iterator2()
      {
        listCon = listCon
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadWarningViewerCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003CLoadWarningViewerCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void NonSelection(ItemListController con)
    {
      if (con == null || this.ListControllers.IsNullOrEmpty<ItemListController>())
        return;
      foreach (ItemListController listController in this.ListControllers)
      {
        if (listController != null && listController != con)
          listController.itemListUI.ForceSetNonSelect();
      }
    }

    private void NonEnabledInput(ItemListUI con)
    {
      if (Object.op_Equality((Object) con, (Object) null) || this.ItemListUIs.IsNullOrEmpty<ItemListUI>())
        return;
      con.EnabledInput = true;
      foreach (ItemListUI itemListUi in this.ItemListUIs)
      {
        if (Object.op_Inequality((Object) itemListUi, (Object) null) && Object.op_Inequality((Object) itemListUi, (Object) con))
          itemListUi.EnabledInput = false;
      }
    }

    private void FinishedInventoryUISetting()
    {
      this._vieweres = new RecyclingInventoryFacadeViewer[2]
      {
        this._pouchInventoryUI,
        this._chestInventoryUI
      };
      if (this.WarningUIs.IsNullOrEmpty<WarningViewer>())
      {
        List<WarningViewer> toRelease = ListPool<WarningViewer>.Get();
        toRelease.Add(this._pouchWarningViewer);
        toRelease.Add(this._chestWarningViewer);
        toRelease.Add(this._pouchAndChestWarningViewer);
        toRelease.RemoveAll((Predicate<WarningViewer>) (x => Object.op_Equality((Object) x, (Object) null)));
        this.WarningUIs = new WarningViewer[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          this.WarningUIs[index] = toRelease[index];
        ListPool<WarningViewer>.Release(toRelease);
      }
      if (this.MenuUIBehaviours.IsNullOrEmpty<MenuUIBehaviour>())
      {
        List<MenuUIBehaviour> toRelease = ListPool<MenuUIBehaviour>.Get();
        toRelease.Add((MenuUIBehaviour) this);
        toRelease.Add((MenuUIBehaviour) this._infoPanelUI);
        toRelease.Add((MenuUIBehaviour) this._deleteRequestUI);
        toRelease.AddRange((IEnumerable<MenuUIBehaviour>) this._pouchInventoryUI.viewer.MenuUIList);
        toRelease.AddRange((IEnumerable<MenuUIBehaviour>) this._chestInventoryUI.viewer.MenuUIList);
        toRelease.Add((MenuUIBehaviour) this._decidedItemSlotUI.ItemListUI);
        toRelease.Add((MenuUIBehaviour) this._createItemStockUI.ItemListUI);
        toRelease.Add((MenuUIBehaviour) this._createPanelUI);
        toRelease.RemoveAll((Predicate<MenuUIBehaviour>) (x => Object.op_Equality((Object) x, (Object) null)));
        this.MenuUIBehaviours = new MenuUIBehaviour[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          this.MenuUIBehaviours[index] = toRelease[index];
        ListPool<MenuUIBehaviour>.Release(toRelease);
      }
      if (this.ItemListUIBehaviours.IsNullOrEmpty<MenuUIBehaviour>())
      {
        List<MenuUIBehaviour> toRelease = ListPool<MenuUIBehaviour>.Get();
        toRelease.Add((MenuUIBehaviour) this._pouchInventoryUI.itemListUI);
        toRelease.Add((MenuUIBehaviour) this._chestInventoryUI.itemListUI);
        toRelease.Add((MenuUIBehaviour) this._decidedItemSlotUI.ItemListUI);
        toRelease.Add((MenuUIBehaviour) this._createItemStockUI.ItemListUI);
        toRelease.RemoveAll((Predicate<MenuUIBehaviour>) (x => Object.op_Equality((Object) x, (Object) null)));
        this.ItemListUIBehaviours = new MenuUIBehaviour[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          this.ItemListUIBehaviours[index] = toRelease[index];
        ListPool<MenuUIBehaviour>.Release(toRelease);
      }
      if (this.ItemListUIs.IsNullOrEmpty<ItemListUI>())
      {
        List<ItemListUI> toRelease = ListPool<ItemListUI>.Get();
        toRelease.Add(this._pouchInventoryUI.itemListUI);
        toRelease.Add(this._chestInventoryUI.itemListUI);
        toRelease.Add(this._decidedItemSlotUI.ItemListUI);
        toRelease.Add(this._createItemStockUI.ItemListUI);
        toRelease.RemoveAll((Predicate<ItemListUI>) (x => Object.op_Equality((Object) x, (Object) null)));
        this.ItemListUIs = new ItemListUI[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          this.ItemListUIs[index] = toRelease[index];
        ListPool<ItemListUI>.Release(toRelease);
      }
      if (this.ListControllers.IsNullOrEmpty<ItemListController>())
      {
        List<ItemListController> toRelease = ListPool<ItemListController>.Get();
        toRelease.Add(this._pouchInventoryUI.ListController);
        toRelease.Add(this._chestInventoryUI.ListController);
        toRelease.Add(this._decidedItemSlotUI.ListController);
        toRelease.Add(this._createItemStockUI.ListController);
        toRelease.RemoveAll((Predicate<ItemListController>) (x => x == null));
        this.ListControllers = new ItemListController[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          this.ListControllers[index] = toRelease[index];
        ListPool<ItemListController>.Release(toRelease);
      }
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) this._infoPanelUI.ClickDecide, (Action<M0>) (_ => this.SendItem(this._infoPanelUI))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) this._infoPanelUI.ClickReturn, (Action<M0>) (_ => this.SendItem(this._infoPanelUI))), (Component) this);
      this._pouchInventoryUI.itemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((currentID, node) => this.CurrentChanged(PanelType.Pouch, currentID, node));
      this._chestInventoryUI.itemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((currentID, node) => this.CurrentChanged(PanelType.Chest, currentID, node));
      this._decidedItemSlotUI.ItemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((currentID, node) => this.CurrentChanged(PanelType.DecidedItem, currentID, node));
      this._createItemStockUI.ItemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((currentID, node) =>
      {
        if (Object.op_Equality((Object) node, (Object) null))
          return;
        this.NonSelection(this._createItemStockUI.ListController);
        this._infoPanelUI.AttachDeleteItem(this._createItemStockUI.ListController, node, currentID);
      });
      this._pouchInventoryUI.ItemNodeOnDoubleClick = (Action<InventoryFacadeViewer.DoubleClickData>) (data => this.OnDoubleClick(PanelType.Pouch, data.ID, data.node));
      this._chestInventoryUI.ItemNodeOnDoubleClick = (Action<InventoryFacadeViewer.DoubleClickData>) (data => this.OnDoubleClick(PanelType.Chest, data.ID, data.node));
      this._decidedItemSlotUI.ListController.DoubleClick += (Action<int, ItemNodeUI>) ((currentID, nodeUI) => this.OnDoubleClick(PanelType.DecidedItem, currentID, nodeUI));
      this._createItemStockUI.ListController.DoubleClick += (Action<int, ItemNodeUI>) ((currentID, nodeUI) => this.OnDoubleClick(PanelType.CreatedItem, currentID, nodeUI));
      if (!this.ItemListUIs.IsNullOrEmpty<ItemListUI>())
      {
        foreach (ItemListUI itemListUi in this.ItemListUIs)
        {
          ItemListUI ui = itemListUi;
          RecyclingUI recyclingUi = this;
          if (!Object.op_Equality((Object) ui, (Object) null))
            DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) ui.OnEntered, (Func<M0, bool>) (_ => ((Behaviour) recyclingUi).get_isActiveAndEnabled())), (Action<M0>) (_ => recyclingUi.NonEnabledInput(ui))), (Component) this);
        }
      }
      if (!this.ListControllers.IsNullOrEmpty<ItemListController>())
      {
        int num = -1;
        foreach (ItemListController listController in this.ListControllers)
        {
          ++num;
          if (listController != null)
            listController.RefreshEvent += (Action) (() => this.UpdateWarningUI());
        }
      }
      this.SetActive(false, ((Component) this).get_gameObject());
      this.Initialized = true;
    }

    private void RefreshInventoryUI()
    {
      if (this._toggleElements.IsNullOrEmpty<ToggleElement>())
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < this._toggleElements.Count; ++index2)
      {
        Toggle toggle = this._toggleElements.GetElement<ToggleElement>(index2)?.Toggle;
        if (Object.op_Inequality((Object) toggle, (Object) null) && toggle.get_isOn())
        {
          index1 = index2;
          break;
        }
      }
      if (!this._vieweres.IsNullOrEmpty<RecyclingInventoryFacadeViewer>())
      {
        WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
        if (worldData != null)
        {
          for (int index2 = 0; index2 < this._vieweres.Length; ++index2)
          {
            RecyclingInventoryFacadeViewer element = this._vieweres.GetElement<RecyclingInventoryFacadeViewer>(index2);
            if (element != null)
            {
              InventoryType inventoryType = (InventoryType) index2;
              int num = 0;
              switch (inventoryType)
              {
                case InventoryType.Pouch:
                  PlayerData playerData = worldData.PlayerData;
                  int? nullable1;
                  int? nullable2;
                  if (playerData == null)
                  {
                    nullable1 = new int?();
                    nullable2 = nullable1;
                  }
                  else
                    nullable2 = new int?(playerData.InventorySlotMax);
                  nullable1 = nullable2;
                  num = !nullable1.HasValue ? 0 : nullable1.Value;
                  break;
                case InventoryType.Chest:
                  num = !Singleton<Resources>.IsInstance() ? 0 : Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.StorageCapacity;
                  break;
              }
              element.slotCounter.y = num;
              element.ItemListNodeCreate();
            }
          }
        }
      }
      if (this.RecyclingData != null)
      {
        this._decidedItemSlotUI.SettingUI(this.RecyclingData.DecidedItemList);
        this._createItemStockUI.SettingUI(this.RecyclingData.CreatedItemList);
      }
      this.ChangedSelecteInventory(index1);
    }

    private void ChangedSelecteInventory(int index)
    {
      if (this._vieweres.IsNullOrEmpty<RecyclingInventoryFacadeViewer>())
        return;
      bool flag1 = this.SelectedInventoryUI != null && this.SelectedInventoryUI.ItemSortUI.isOpen;
      RecyclingInventoryFacadeViewer inventoryFacadeViewer = (RecyclingInventoryFacadeViewer) null;
      for (int index1 = 0; index1 < this._vieweres.Length; ++index1)
      {
        RecyclingInventoryFacadeViewer viewere = this._vieweres[index1];
        if (viewere != null)
        {
          bool visible = index1 == index;
          viewere.SetVisible(visible);
          viewere.Visible = visible;
          if (index1 == index)
            inventoryFacadeViewer = viewere;
          else
            this.CursorOff(viewere);
        }
      }
      if (inventoryFacadeViewer == null)
        return;
      inventoryFacadeViewer.Refresh();
      if (flag1)
        inventoryFacadeViewer.ItemSortUI.Open();
      bool flag2 = inventoryFacadeViewer != this.SelectedInventoryUI;
      this.SelectedInventoryUI = inventoryFacadeViewer;
      if (!flag2)
        return;
      this.OnInventoryChanged.OnNext(inventoryFacadeViewer);
    }

    private void CurrentChanged(PanelType panelType, int currentID, ItemNodeUI nodeUI)
    {
      if (Object.op_Equality((Object) this._infoPanelUI, (Object) null) || Object.op_Equality((Object) nodeUI, (Object) null) || currentID < 0)
        return;
      ButtonType buttonType1 = ButtonType.Decide;
      ItemListController listController1;
      ItemListController listController2;
      ButtonType buttonType2;
      switch (panelType)
      {
        case PanelType.Pouch:
          listController1 = this._pouchInventoryUI.ListController;
          listController2 = this._decidedItemSlotUI.ListController;
          buttonType2 = ButtonType.Decide;
          break;
        case PanelType.Chest:
          listController1 = this._chestInventoryUI.ListController;
          listController2 = this._decidedItemSlotUI.ListController;
          buttonType2 = ButtonType.Decide;
          break;
        case PanelType.DecidedItem:
          listController1 = this._decidedItemSlotUI.ListController;
          listController2 = this.SelectedInventoryUI?.ListController;
          buttonType2 = ButtonType.Return;
          break;
        default:
          this._infoPanelUI.DetachItem();
          buttonType1 = ButtonType.Delete;
          this.NonSelection(this._createItemStockUI.ListController);
          return;
      }
      if (listController1 == null || listController2 == null)
      {
        this._infoPanelUI.DetachItem();
      }
      else
      {
        this.NonSelection(listController1);
        this._infoPanelUI.AttachItem(listController1, listController2, currentID, nodeUI, buttonType2);
      }
    }

    private void OnDoubleClick(PanelType panelType, int currnetID, ItemNodeUI nodeUI)
    {
      if (Object.op_Equality((Object) nodeUI, (Object) null))
        return;
      ItemListController listController1;
      ItemListController listController2;
      switch (panelType)
      {
        case PanelType.Pouch:
          listController1 = this._pouchInventoryUI.ListController;
          listController2 = this._decidedItemSlotUI.ListController;
          break;
        case PanelType.Chest:
          listController1 = this._chestInventoryUI.ListController;
          listController2 = this._decidedItemSlotUI.ListController;
          break;
        case PanelType.Info:
          return;
        case PanelType.DecidedItem:
          listController1 = this._decidedItemSlotUI.ListController;
          listController2 = this.SelectedInventoryUI?.ListController;
          break;
        case PanelType.CreatedItem:
          listController1 = this._createItemStockUI.ListController;
          listController2 = this.SelectedInventoryUI?.ListController;
          break;
        default:
          return;
      }
      this.OnDoubleClicked.OnNext(panelType);
      this.SendItem(nodeUI.Item.Count, panelType, listController1, listController2, currnetID, nodeUI);
    }

    private void SendItem(RecyclingInfoPanelUI panelUI)
    {
      if (Object.op_Equality((Object) panelUI, (Object) null) || !panelUI.IsActiveItemInfo || !panelUI.IsNumberInput)
        return;
      ValueTuple<ItemListController, ItemListController, ItemNodeUI, int, ButtonType> itemInfo = panelUI.GetItemInfo();
      this.SendItem(panelUI.InputNumber, ((ItemListController) itemInfo.Item1).PanelType, (ItemListController) itemInfo.Item1, (ItemListController) itemInfo.Item2, (int) itemInfo.Item4, (ItemNodeUI) itemInfo.Item3);
    }

    public void SendItem(
      int count,
      PanelType panelType,
      ItemListController sender,
      ItemListController receiver,
      int currentID,
      ItemNodeUI nodeUI)
    {
      if (sender == null || receiver == null || (Object.op_Equality((Object) nodeUI, (Object) null) || nodeUI.Item == null) || (count <= 0 || nodeUI.Item.Count <= 0))
        return;
      StuffItem stuffItem1 = new StuffItem(nodeUI.Item);
      stuffItem1.Count = Mathf.Min(count, nodeUI.Item.Count);
      int num = receiver.AddItem(stuffItem1);
      if (num <= 0)
        return;
      switch (panelType)
      {
        case PanelType.Pouch:
        case PanelType.Chest:
          if (this.SelectedInventoryUI != null)
          {
            List<StuffItem> itemList = this.SelectedInventoryUI.itemList;
            ItemListUI itemListUi = this.SelectedInventoryUI.itemListUI;
            StuffItem stuffItem2 = itemList.Find((Predicate<StuffItem>) (x => x == nodeUI.Item));
            if (stuffItem2 != null)
            {
              stuffItem2.Count -= num;
              if (stuffItem2.Count <= 0)
              {
                itemList.Remove(stuffItem2);
                itemListUi.RemoveItemNode(currentID);
                itemListUi.ForceSetNonSelect();
                this._infoPanelUI.DetachItem();
              }
              Action refreshEvent = sender.RefreshEvent;
              if (refreshEvent != null)
              {
                refreshEvent();
                break;
              }
              break;
            }
            break;
          }
          break;
        case PanelType.DecidedItem:
        case PanelType.CreatedItem:
          if (sender.RemoveItem(currentID, new StuffItem(nodeUI.Item)
          {
            Count = num
          }) <= 0)
            this._infoPanelUI.DetachItem();
          if (this.SelectedInventoryUI != null && Object.op_Inequality((Object) this.SelectedInventoryUI.itemListUI, (Object) null))
          {
            Action refreshEvent = this.SelectedInventoryUI.ListController.RefreshEvent;
            if (refreshEvent != null)
            {
              refreshEvent();
              break;
            }
            break;
          }
          break;
      }
      this.OnSendItem.OnNext(stuffItem1);
    }

    private void UpdateWarningUI()
    {
      bool flag = this._createItemStockUI.CheckInventoryEmpty();
      if (flag)
        this.HideAllWarning();
      else
        this.ShowWarning(WarningType.PouchAndChest);
      this.SetInteractable((Selectable) this._createItemStockUI.AllGetButton, flag && !this._createItemStockUI.ItemList.IsNullOrEmpty<StuffItem>());
    }

    public void ShowWarning(WarningType warningType)
    {
      if (this.WarningUIs.IsNullOrEmpty<WarningViewer>())
        return;
      int num = (int) warningType;
      for (int index = 0; index < this.WarningUIs.Length; ++index)
      {
        WarningViewer element = this.WarningUIs.GetElement<WarningViewer>(index);
        if (!Object.op_Equality((Object) element, (Object) null))
        {
          bool flag = num == index;
          if (element.visible != flag)
            element.visible = flag;
        }
      }
    }

    public void HideAllWarning()
    {
      if (this.WarningUIs.IsNullOrEmpty<WarningViewer>())
        return;
      foreach (WarningViewer warningUi in this.WarningUIs)
      {
        if (Object.op_Inequality((Object) warningUi, (Object) null) || warningUi.visible)
          warningUi.visible = false;
      }
    }

    private void SetRecyclingData()
    {
      this.RecyclingData = (RecyclingData) null;
      int num = -1;
      this.CraftPointID = num;
      this._craftPointID = num;
      CraftPoint currentCraftPoint = Manager.Map.GetPlayer()?.CurrentCraftPoint;
      if (!Object.op_Inequality((Object) currentCraftPoint, (Object) null))
        return;
      int key = this._craftPointID = currentCraftPoint.RegisterID;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      AIProject.SaveData.Environment environment = worldData == null ? (AIProject.SaveData.Environment) null : worldData.Environment;
      Dictionary<int, RecyclingData> dictionary = environment == null ? (Dictionary<int, RecyclingData>) null : environment.RecyclingDataTable;
      if (dictionary == null)
        return;
      RecyclingData recyclingData1 = (RecyclingData) null;
      if (dictionary.TryGetValue(key, out recyclingData1) && recyclingData1 != null)
      {
        this.RecyclingData = recyclingData1;
      }
      else
      {
        RecyclingData recyclingData2 = new RecyclingData();
        dictionary[key] = recyclingData2;
        this.RecyclingData = recyclingData2;
      }
    }

    private void ActiveInitialize()
    {
      this.HideAllWarning();
      this.RefreshInventoryUI();
      this._infoPanelUI.Refresh();
      Action refreshEvent1 = this._decidedItemSlotUI.ListController.RefreshEvent;
      if (refreshEvent1 != null)
        refreshEvent1();
      Action refreshEvent2 = this._createItemStockUI.ListController.RefreshEvent;
      if (refreshEvent2 == null)
        return;
      refreshEvent2();
    }

    private void SetActiveControl(bool active)
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      this._fadeDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex))), (Component) this);
    }

    public void DoClose()
    {
      this.PlaySE(SoundPack.SystemSE.Cancel);
      this.IsActiveControl = false;
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003COpenCoroutine\u003Ec__Iterator4()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingUI.\u003CCloseCoroutine\u003Ec__Iterator5()
      {
        \u0024this = this
      };
    }

    private void SetFocusLevelAll(int level)
    {
      if (this.MenuUIBehaviours.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour menuUiBehaviour in this.MenuUIBehaviours)
      {
        if (Object.op_Inequality((Object) menuUiBehaviour, (Object) null))
          menuUiBehaviour.SetFocusLevel(level);
      }
    }

    private void SetEnabledInputAll(bool flag)
    {
      if (this.MenuUIBehaviours.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour menuUiBehaviour in this.MenuUIBehaviours)
      {
        if (Object.op_Inequality((Object) menuUiBehaviour, (Object) null))
          menuUiBehaviour.EnabledInput = flag;
      }
    }

    private void SetFocusLevelAll(int level, MenuUIBehaviour[] uis)
    {
      if (uis.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour ui in uis)
      {
        if (Object.op_Inequality((Object) ui, (Object) null))
          ui.SetFocusLevel(level);
      }
    }

    private void SetEnabledInputAll(bool flag, MenuUIBehaviour[] uis)
    {
      if (uis.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour ui in uis)
      {
        if (Object.op_Inequality((Object) ui, (Object) null))
          ui.EnabledInput = flag;
      }
    }

    private void SetActive(bool flag, GameObject obj)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == flag)
        return;
      obj.SetActive(flag);
    }

    private void SetActive(bool flag, Component com)
    {
      if (!Object.op_Inequality((Object) com, (Object) null) || com.get_gameObject().get_activeSelf() == flag)
        return;
      com.get_gameObject().SetActive(flag);
    }

    private void SetInteractable(Selectable obj, bool active)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_interactable() == active)
        return;
      obj.set_interactable(active);
    }

    public void PlaySE(SoundPack.SystemSE se)
    {
      if (!this.Initialized || !this.IsActiveControl)
        return;
      SoundPack soundPack = !Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack;
      if (Object.op_Equality((Object) soundPack, (Object) null))
        return;
      soundPack.Play(se);
    }
  }
}
