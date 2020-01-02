// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingDecidedItemSlotUI
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

namespace AIProject.UI.Recycling
{
  public class RecyclingDecidedItemSlotUI : UIBehaviour
  {
    [SerializeField]
    private RecyclingUI _recyclingUI;
    [SerializeField]
    private RecyclingCreatePanelUI _createPanelUI;
    [SerializeField]
    private Text _countText;
    [SerializeField]
    private Text _maxCountText;
    [SerializeField]
    private Text _playNowText;
    [SerializeField]
    private Image _countBarImage;
    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private ItemListUI _itemListUI;
    private CompositeDisposable _updateDisposable;

    public RecyclingDecidedItemSlotUI()
    {
      base.\u002Ector();
    }

    public List<StuffItem> ItemList { get; private set; }

    public Subject<StuffItem> CreateEvent { get; }

    public Subject<StuffItem> DeleteEvent { get; }

    public ItemListUI ItemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public ItemListController ListController { get; }

    public BoolReactiveProperty IsCreate { get; }

    private List<ItemNodeUI> ItemNodeUIList { get; }

    public int SlotMaxNum { get; private set; }

    public float CountLimit { get; private set; }

    public int CraftPointID { get; set; }

    public int CreatedItemSlotMaxNum { get; private set; }

    public int NeedNumber { get; private set; }

    public bool IsCreateable
    {
      get
      {
        RecyclingData recyclingData = this._recyclingUI.RecyclingData;
        return recyclingData != null && recyclingData.CreatedItemList.Count < this.CreatedItemSlotMaxNum && this.NeedNumber <= this.ItemCount();
      }
    }

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
      this.ListController.RefreshEvent += (Action) (() => this.RefreshUI());
      if (this._updateDisposable != null)
        this._updateDisposable.Clear();
      this._updateDisposable = new CompositeDisposable();
      if (Singleton<Resources>.IsInstance())
      {
        DefinePack.RecyclingSetting recycling = Singleton<Resources>.Instance.DefinePack.Recycling;
        this.CountLimit = recycling.ItemCreateTime;
        this.SlotMaxNum = recycling.DecidedItemCapacity;
        this.CreatedItemSlotMaxNum = recycling.CreateItemCapacity;
        this.NeedNumber = recycling.NeedNumber;
      }
      else
      {
        this.CountLimit = 0.0f;
        this.SlotMaxNum = 0;
        this.CreatedItemSlotMaxNum = 0;
        this.NeedNumber = 0;
      }
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Action<M0>) (_ => this.OnRecyclingDataUpdate())), (ICollection<IDisposable>) this._updateDisposable);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Merge<Unit>((IObservable<M0>[]) new IObservable<Unit>[2]
      {
        (IObservable<Unit>) Observable.Do<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._createButton), (Action<M0>) (_ => this.OnStartClick())),
        (IObservable<Unit>) Observable.Do<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._cancelButton), (Action<M0>) (_ => this.OnCancelClick()))
      }), (Action<M0>) (_ => this._recyclingUI.PlaySE(SoundPack.SystemSE.OK_S))), (Component) this);
      this._recyclingUI.CreateItemStockUI.ChangeCreateableEvent += (Action<bool>) (flag => ((Selectable) this._createButton).set_interactable(flag));
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.IsCreate, (Action<M0>) (flag =>
      {
        this.SetActive((Component) this._cancelButton, flag);
        this.SetActive((Component) this._createButton, !flag);
        this.SetActive((Component) this._playNowText, flag);
      })), (ICollection<IDisposable>) this._updateDisposable);
    }

    protected virtual void OnDestroy()
    {
      if (this._updateDisposable != null)
        this._updateDisposable.Clear();
      base.OnDestroy();
    }

    public void SettingUI(List<StuffItem> itemList)
    {
      this.ItemList = itemList;
      this.ListController.SetItemList(itemList);
      this.ListController.Create((IReadOnlyCollection<StuffItem>) itemList);
      this._maxCountText.set_text(string.Format("{0}", (object) this.SlotMaxNum));
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData == null)
        return;
      ((ReactiveProperty<bool>) this.IsCreate).set_Value(recyclingData.CreateCountEnabled);
    }

    public void RefreshCountText()
    {
      int num = this.ItemCount();
      this._countText.set_text(num.ToString());
      ((Graphic) this._countText).set_color(Define.Get(this.SlotMaxNum > num ? Colors.White : Colors.Red));
    }

    public void RefreshButtonUI()
    {
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData == null)
        return;
      bool createCountEnabled = recyclingData.CreateCountEnabled;
      this.SetActive((Component) this._createButton, !createCountEnabled);
      this.SetActive((Component) this._cancelButton, createCountEnabled);
      if (!createCountEnabled)
        recyclingData.CreateCounter = 0.0f;
      ((Selectable) this._createButton).set_interactable(this.IsCreateable);
    }

    public void RefreshCreatePanelUI()
    {
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData != null)
      {
        if (recyclingData.CreateCountEnabled)
          recyclingData.CreateCountEnabled = this.CheckCreateable(recyclingData);
        if (!recyclingData.CreateCountEnabled)
          recyclingData.CreateCounter = 0.0f;
        if (this.NeedNumber <= this.ItemCount(recyclingData))
          this._createPanelUI.DoOpen();
        else
          this._createPanelUI.DoClose();
        ((ReactiveProperty<bool>) this.IsCreate).set_Value(recyclingData.CreateCountEnabled);
      }
      else
      {
        this._createPanelUI.DoForceClose();
        ((ReactiveProperty<bool>) this.IsCreate).set_Value(false);
      }
    }

    public void RefreshUI()
    {
      this._itemListUI.Refresh();
      this.RefreshCountText();
      this.RefreshCreatePanelUI();
      this.RefreshButtonUI();
    }

    public int ItemCount()
    {
      if (this.ItemList.IsNullOrEmpty<StuffItem>())
        return 0;
      int num = 0;
      foreach (StuffItem stuffItem in this.ItemList)
      {
        if (stuffItem != null)
          num += stuffItem.Count;
      }
      return num;
    }

    public int ItemCount(RecyclingData data)
    {
      if (data == null || data.DecidedItemList.IsNullOrEmpty<StuffItem>())
        return 0;
      int num = 0;
      foreach (StuffItem decidedItem in data.DecidedItemList)
      {
        if (decidedItem != null)
          num += decidedItem.Count;
      }
      return num;
    }

    public int FreeCount()
    {
      int num = 0;
      this.ItemNodeUIList.RemoveAll((Predicate<ItemNodeUI>) (x => Object.op_Equality((Object) x, (Object) null)));
      foreach (ItemNodeUI itemNodeUi in this.ItemNodeUIList)
      {
        if (!Object.op_Equality((Object) itemNodeUi, (Object) null) && itemNodeUi.Item != null)
          num += itemNodeUi.Item.Count;
      }
      return Mathf.Max(0, this.SlotMaxNum - num);
    }

    private void OnStartClick()
    {
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData == null || this.ItemCount(recyclingData) < this.NeedNumber || !this._recyclingUI.CreateItemStockUI.IsCreateable)
        return;
      ((ReactiveProperty<bool>) this.IsCreate).set_Value(true);
      recyclingData.CreateCountEnabled = true;
      recyclingData.CreateCounter = 0.0f;
    }

    private void OnCancelClick()
    {
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData == null)
        return;
      ((ReactiveProperty<bool>) this.IsCreate).set_Value(false);
      recyclingData.CreateCountEnabled = false;
      recyclingData.CreateCounter = 0.0f;
    }

    private void OnRecyclingDataUpdate()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      Dictionary<int, RecyclingData> source = environment == null ? (Dictionary<int, RecyclingData>) null : environment.RecyclingDataTable;
      if (source.IsNullOrEmpty<int, RecyclingData>())
        return;
      foreach (KeyValuePair<int, RecyclingData> keyValuePair in source)
      {
        RecyclingData data = keyValuePair.Value;
        if (data != null)
        {
          if (!data.CreateCountEnabled)
            data.CreateCounter = 0.0f;
          else if ((double) this.CountLimit <= (double) data.CreateCounter)
            this.CreateItem(keyValuePair.Key, data);
        }
      }
    }

    private void RemoveItem(int pointID, RecyclingData data)
    {
      if (data == null || data.DecidedItemList.IsNullOrEmpty<StuffItem>())
        return;
      if (pointID == this._recyclingUI.CraftPointID)
      {
        this.RemoveItem();
      }
      else
      {
        int needNumber = this.NeedNumber;
        for (int index = 0; index < data.DecidedItemList.Count; ++index)
        {
          StuffItem element = data.DecidedItemList.GetElement<StuffItem>(index);
          if (element == null)
          {
            data.DecidedItemList.RemoveAt(index);
            --index;
          }
          else
          {
            int num = Mathf.Min(element.Count, needNumber);
            needNumber -= num;
            element.Count -= num;
            if (element.Count <= 0)
            {
              data.DecidedItemList.RemoveAt(index);
              --index;
            }
            if (needNumber <= 0)
              break;
          }
        }
      }
    }

    private void RemoveItem()
    {
      if (this.ItemList.IsNullOrEmpty<StuffItem>() && this.ItemCount() < this.NeedNumber)
        return;
      int needNumber = this.NeedNumber;
      for (int index1 = 0; index1 < this.ItemList.Count; ++index1)
      {
        StuffItem element = this.ItemList.GetElement<StuffItem>(index1);
        if (element == null)
        {
          this.ItemList.RemoveAt(index1);
          --index1;
        }
        else
        {
          int num = Mathf.Min(needNumber, element.Count);
          needNumber -= num;
          element.Count -= num;
          if (element.Count <= 0)
          {
            this.ItemList.Remove(element);
            --index1;
            int index2 = this._itemListUI.SearchIndex(element);
            if (index2 != -1)
              this._itemListUI.RemoveItemNode(index2);
          }
          this.DeleteEvent.OnNext(element);
          if (needNumber <= 0)
            break;
        }
      }
      Action refreshEvent = this.ListController.RefreshEvent;
      if (refreshEvent == null)
        return;
      refreshEvent();
    }

    private void CreateItem(int pointID, RecyclingData data)
    {
      if (data == null)
        return;
      data.CreateCounter = 0.0f;
      if (this.CheckCreateable(data))
      {
        this.RemoveItem(pointID, data);
        StuffItem randomCreate = this.GetRandomCreate();
        if (randomCreate != null)
        {
          if (this._recyclingUI.CraftPointID == pointID)
            this.CreateEvent.OnNext(randomCreate);
          else
            data.CreatedItemList.AddItem(randomCreate);
        }
      }
      if (data.CreateCountEnabled)
        data.CreateCountEnabled = this.CheckCreateable(data);
      if (this._recyclingUI.CraftPointID != pointID)
        return;
      ((ReactiveProperty<bool>) this.IsCreate).set_Value(data.CreateCountEnabled);
    }

    public bool CheckCreateable(RecyclingData data)
    {
      return data != null && data.CreatedItemList.Count < this.CreatedItemSlotMaxNum && this.NeedNumber <= this.ItemCount(data);
    }

    private StuffItem GetRandomCreate()
    {
      if (!Singleton<Resources>.IsInstance())
        return (StuffItem) null;
      Resources instance = Singleton<Resources>.Instance;
      List<RecyclingItemInfo> createableItemList = instance.GameInfo?.RecyclingCreateableItemList;
      if (createableItemList.IsNullOrEmpty<RecyclingItemInfo>())
        return (StuffItem) null;
      int index = Random.Range(0, createableItemList.Count);
      RecyclingItemInfo element = createableItemList.GetElement<RecyclingItemInfo>(index);
      return instance.GameInfo.GetItem(element.CategoryID, element.ItemID) == null ? (StuffItem) null : new StuffItem(element.CategoryID, element.ItemID, 1);
    }

    private void SetActive(GameObject obj, bool flag)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == flag)
        return;
      obj.SetActive(flag);
    }

    private void SetActive(Component com, bool flag)
    {
      if (!Object.op_Inequality((Object) com, (Object) null) || !Object.op_Inequality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == flag)
        return;
      com.get_gameObject().SetActive(flag);
    }
  }
}
