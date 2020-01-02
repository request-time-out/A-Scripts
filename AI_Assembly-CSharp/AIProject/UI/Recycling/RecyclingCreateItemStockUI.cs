// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingCreateItemStockUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.ColorDefine;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI.Recycling
{
  public class RecyclingCreateItemStockUI : UIBehaviour
  {
    [SerializeField]
    private RecyclingUI _recyclingUI;
    [SerializeField]
    private RecyclingItemDeleteRequestUI _deleteRequestUI;
    [SerializeField]
    private Text _countText;
    [SerializeField]
    private Text _maxCountText;
    [SerializeField]
    private Text _notCreateableText;
    [SerializeField]
    private Button _allGetButton;
    [SerializeField]
    private Button _oneGetButton;
    [SerializeField]
    private Button _deleteSubmitButton;
    [SerializeField]
    private Button _deleteCancelButton;
    [SerializeField]
    private ItemListUI _itemListUI;

    public RecyclingCreateItemStockUI()
    {
      base.\u002Ector();
    }

    public ItemListUI ItemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public ItemListController ListController { get; }

    public Action<bool> ChangeCreateableEvent { get; set; }

    public int ItemSlotMax { get; private set; }

    public Button AllGetButton
    {
      get
      {
        return this._allGetButton;
      }
    }

    public IObservable<Unit> OnClickDeleteSubmit
    {
      get
      {
        return UnityUIComponentExtensions.OnClickAsObservable(this._deleteSubmitButton);
      }
    }

    public Subject<StuffItem> OnAddCreateItem { get; }

    public bool IsCreateable
    {
      get
      {
        return this.ItemList != null && this.ItemList.Count < this.ItemSlotMax;
      }
    }

    public bool IsCurrentNode
    {
      get
      {
        return Object.op_Inequality((Object) this._itemListUI.CurrentOption, (Object) null);
      }
    }

    public List<StuffItem> ItemList { get; private set; }

    protected virtual void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._recyclingUI, (Object) null))
        this._recyclingUI = (RecyclingUI) ((Component) this).GetComponentInParent<RecyclingUI>();
      this.ListController.Bind(this._itemListUI);
    }

    protected virtual void Start()
    {
      base.Start();
      this.ItemSlotMax = !Singleton<Resources>.IsInstance() ? 0 : Singleton<Resources>.Instance.DefinePack.Recycling.CreateItemCapacity;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._deleteSubmitButton), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
      {
        this.OnDeleteOKClick();
        this._recyclingUI.PlaySE(SoundPack.SystemSE.OK_L);
      })), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._deleteCancelButton), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
      {
        this.OnDeleteCancelClick();
        this._recyclingUI.PlaySE(SoundPack.SystemSE.Cancel);
      })), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Merge<Unit>((IObservable<M0>[]) new IObservable<Unit>[2]
      {
        (IObservable<Unit>) Observable.Do<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._oneGetButton), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnClickOneGet())),
        (IObservable<Unit>) Observable.Do<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._allGetButton), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnClickAllGet()))
      }), (Action<M0>) (_ => this._recyclingUI.PlaySE(SoundPack.SystemSE.OK_S))), (Component) this);
      this.ListController.RefreshEvent += (Action) (() => this.RefreshUI());
      this._itemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((currentID, nodeUI) => this.RefreshOneGetButtonInteractable(nodeUI));
    }

    public void SettingUI(List<StuffItem> itemList)
    {
      this.ItemList = itemList;
      this.ListController.SetItemList(itemList);
      this.ListController.Create((IReadOnlyCollection<StuffItem>) itemList);
    }

    public void RefreshUI()
    {
      this._itemListUI.Refresh();
      this.SetActiveNotCreateableText(!this.IsCreateable);
      this.RefreshCountText();
    }

    public void SetActiveNotCreateableText(bool flag)
    {
      if (!this.SetActive((Component) this._notCreateableText, flag))
        return;
      Action<bool> changeCreateableEvent = this.ChangeCreateableEvent;
      if (changeCreateableEvent == null)
        return;
      changeCreateableEvent(!flag);
    }

    public void RefreshCountText()
    {
      this._maxCountText.set_text(string.Format("{0}", (object) this.ItemSlotMax));
      int count = this.ItemList.Count;
      this._countText.set_text(string.Format("{0}", (object) count));
      ((Graphic) this._countText).set_color(Define.Get(count >= this.ItemSlotMax ? Colors.Red : Colors.White));
    }

    public void RefreshOneGetButtonInteractable()
    {
      this.RefreshOneGetButtonInteractable(this._itemListUI.CurrentOption);
    }

    public void RefreshOneGetButtonInteractable(ItemNodeUI nodeUI)
    {
      this.Setinteractable((Selectable) this._oneGetButton, !Object.op_Equality((Object) nodeUI, (Object) null) && this.AddableItemToInventory(nodeUI.Item));
    }

    public void AddItem(StuffItem item)
    {
      this.ListController.AddItem(item);
      this.OnAddCreateItem.OnNext(item);
    }

    private void OnDeleteOKClick()
    {
      if (this._deleteRequestUI.IsActiveControl)
        this._deleteRequestUI.DoClose();
      RecyclingInfoPanelUI infoPanelUi = this._recyclingUI.InfoPanelUI;
      if (Object.op_Equality((Object) infoPanelUi, (Object) null) || !infoPanelUi.IsNumberInput)
        return;
      ValueTuple<ItemListController, ItemListController, ItemNodeUI, int, ButtonType> itemInfo = infoPanelUi.GetItemInfo();
      ItemNodeUI itemNodeUi = (ItemNodeUI) itemInfo.Item3;
      StuffItem source = !Object.op_Inequality((Object) itemNodeUi, (Object) null) ? (StuffItem) null : itemNodeUi.Item;
      if (Object.op_Equality((Object) itemNodeUi, (Object) null) || source == null)
        return;
      int sel = (int) itemInfo.Item4;
      StuffItem stuffItem = new StuffItem(source);
      stuffItem.Count = Mathf.Min(source.Count, infoPanelUi.InputNumber);
      bool flag = source.Count <= stuffItem.Count;
      this.ListController.RemoveItem(sel, stuffItem);
      if (!flag)
        return;
      this._itemListUI.ForceSetNonSelect();
      infoPanelUi.DetachItem();
    }

    private void OnDeleteCancelClick()
    {
      if (!this._deleteRequestUI.IsActiveControl)
        return;
      this._deleteRequestUI.DoClose();
    }

    private List<StuffItem> AddFailedList { get; }

    private bool AddableItemToInventory(StuffItem item)
    {
      if (item == null)
        return false;
      List<StuffItem> stuffItemList = ListPool<StuffItem>.Get();
      stuffItemList.Add(item);
      bool inventory = this.AddableItemToInventory(stuffItemList);
      ListPool<StuffItem>.Release(stuffItemList);
      return inventory;
    }

    private bool AddableItemToInventory(List<StuffItem> itemList)
    {
      if (itemList.IsNullOrEmpty<StuffItem>())
        return false;
      List<RecyclingInventoryFacadeViewer> vieweres = this.GetVieweres();
      if (vieweres.IsNullOrEmpty<RecyclingInventoryFacadeViewer>())
      {
        this.ReturnVieweres(vieweres);
        return false;
      }
      bool flag = false;
      foreach (RecyclingInventoryFacadeViewer inventoryFacadeViewer in vieweres)
      {
        if (inventoryFacadeViewer != null)
        {
          foreach (StuffItem stuffItem in this.ItemList)
          {
            if (stuffItem != null)
            {
              if (flag = 0 < inventoryFacadeViewer.ListController.PossibleCount(stuffItem))
                break;
            }
          }
          if (flag)
            break;
        }
      }
      this.ReturnVieweres(vieweres);
      return flag;
    }

    public bool CheckInventoryEmpty()
    {
      if (this.ItemList.IsNullOrEmpty<StuffItem>())
        return true;
      foreach (RecyclingInventoryFacadeViewer inventoryUi in this._recyclingUI.InventoryUIs)
      {
        if (inventoryUi != null && 0 < inventoryUi.ListController.EmptySlotNum())
          return true;
      }
      return this.AddableItemToInventory(this.ItemList);
    }

    private void OnClickOneGet()
    {
      if (this.ItemList.IsNullOrEmpty<StuffItem>())
        return;
      ItemNodeUI currentOption = this._itemListUI.CurrentOption;
      StuffItem stuffItem = !Object.op_Inequality((Object) currentOption, (Object) null) ? (StuffItem) null : currentOption.Item;
      if (stuffItem == null)
        return;
      List<StuffItem> stuffItemList = ListPool<StuffItem>.Get();
      stuffItemList.Add(stuffItem);
      List<RecyclingInventoryFacadeViewer> vieweres = this.GetVieweres();
      foreach (RecyclingInventoryFacadeViewer viewer in vieweres)
      {
        this.GetItemAction(stuffItemList, viewer);
        if (!this.AddFailedList.Contains(stuffItem))
        {
          this.ItemList.Remove(stuffItem);
          this._itemListUI.RemoveItemNode(this._itemListUI.CurrentID);
          this._itemListUI.ForceSetNonSelect();
          stuffItem = (StuffItem) null;
          this._recyclingUI.InfoPanelUI.DetachItem();
        }
        if (stuffItem == null)
          break;
      }
      this.AddFailedList.Clear();
      this.ReturnVieweres(vieweres);
      ListPool<StuffItem>.Release(stuffItemList);
      Action refreshEvent = this.ListController.RefreshEvent;
      if (refreshEvent != null)
        refreshEvent();
      this.RefreshOneGetButtonInteractable();
    }

    private void OnClickAllGet()
    {
      if (this.ItemList.IsNullOrEmpty<StuffItem>())
        return;
      List<RecyclingInventoryFacadeViewer> vieweres = this.GetVieweres();
      foreach (RecyclingInventoryFacadeViewer viewer in vieweres)
      {
        this.GetItemAction(this.ItemList, viewer);
        for (int index = 0; index < this.ItemList.Count; ++index)
        {
          StuffItem stuffItem = this.ItemList[index];
          if (!this.AddFailedList.Contains(stuffItem))
          {
            this.ItemList.Remove(stuffItem);
            this._itemListUI.RemoveItemNode(this._itemListUI.SearchIndex(stuffItem));
            --index;
          }
        }
        if (this.ItemList.IsNullOrEmpty<StuffItem>())
          break;
      }
      Action refreshEvent = this.ListController.RefreshEvent;
      if (refreshEvent != null)
        refreshEvent();
      if (this._itemListUI.SearchIndex(this._itemListUI.CurrentOption) == -1)
      {
        this._itemListUI.ForceSetNonSelect();
        this._recyclingUI.InfoPanelUI.DetachItem();
      }
      this.AddFailedList.Clear();
      this.ReturnVieweres(vieweres);
    }

    public void GetItemAction(List<StuffItem> itemList, RecyclingInventoryFacadeViewer viewer)
    {
      this.AddFailedList.Clear();
      if (itemList.IsNullOrEmpty<StuffItem>() || viewer == null)
        return;
      foreach (StuffItem stuffItem in itemList)
      {
        if (!viewer.AddItemCondition(stuffItem))
          this.AddFailedList.Add(stuffItem);
      }
      Action refreshEvent = viewer.ListController.RefreshEvent;
      if (refreshEvent == null)
        return;
      refreshEvent();
    }

    private List<RecyclingInventoryFacadeViewer> GetVieweres()
    {
      List<RecyclingInventoryFacadeViewer> inventoryFacadeViewerList = ListPool<RecyclingInventoryFacadeViewer>.Get();
      inventoryFacadeViewerList.AddRange((IEnumerable<RecyclingInventoryFacadeViewer>) this._recyclingUI.InventoryUIs);
      RecyclingInventoryFacadeViewer selectedInventoryUi = this._recyclingUI.SelectedInventoryUI;
      if (selectedInventoryUi != null)
      {
        inventoryFacadeViewerList.Remove(selectedInventoryUi);
        inventoryFacadeViewerList.Insert(0, selectedInventoryUi);
      }
      inventoryFacadeViewerList.RemoveAll((Predicate<RecyclingInventoryFacadeViewer>) (x => x == null));
      return inventoryFacadeViewerList;
    }

    private void ReturnVieweres(List<RecyclingInventoryFacadeViewer> list)
    {
      ListPool<RecyclingInventoryFacadeViewer>.Release(list);
    }

    private bool SetActive(GameObject obj, bool flag)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == flag)
        return false;
      obj.SetActive(flag);
      return true;
    }

    private bool SetActive(Component com, bool flag)
    {
      if (!Object.op_Inequality((Object) com, (Object) null) || !Object.op_Inequality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == flag)
        return false;
      com.get_gameObject().SetActive(flag);
      return true;
    }

    private void Setinteractable(Selectable obj, bool active)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_interactable() == active)
        return;
      obj.set_interactable(active);
    }
  }
}
