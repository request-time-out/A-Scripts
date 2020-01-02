// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingInfoPanelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI.Recycling
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class RecyclingInfoPanelUI : MenuUIBehaviour
  {
    [SerializeField]
    private Button[] _buttons = new Button[3];
    [SerializeField]
    private Button[] _numChangeButtons = new Button[4];
    [NonSerialized]
    private ValueTuple<ItemListController, ItemListController, ItemNodeUI, int, ButtonType> ItemInfo = new ValueTuple<ItemListController, ItemListController, ItemNodeUI, int, ButtonType>((ItemListController) null, (ItemListController) null, (ItemNodeUI) null, -1, ButtonType.None);
    private int[] _numChangeValues = new int[4]
    {
      -10,
      -1,
      1,
      10
    };
    [SerializeField]
    private RecyclingUI _recyclingUI;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Text _itemNameText;
    [SerializeField]
    private Text _flavorText;
    [SerializeField]
    private InputField _numInputField;
    private const int ItemMinCount = 0;
    private IDisposable _fadeDisposable;

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    public bool BlockRaycast
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) && this._canvasGroup.get_blocksRaycasts();
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_blocksRaycasts() == value)
          return;
        this._canvasGroup.set_blocksRaycasts(value);
      }
    }

    public bool Interactable
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) && this._canvasGroup.get_interactable();
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_interactable() == value)
          return;
        this._canvasGroup.set_interactable(value);
      }
    }

    public bool IsActiveItemInfo
    {
      get
      {
        return this.ItemInfo.Item1 != null && this.ItemInfo.Item2 != null && Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null);
      }
    }

    public ValueTuple<ItemListController, ItemListController, ItemNodeUI, int, ButtonType> GetItemInfo()
    {
      return this.ItemInfo;
    }

    public void SetItemInfo(
      ItemListController sender,
      ItemListController receiver,
      ItemNodeUI nodeUI,
      int currentID,
      ButtonType buttonType)
    {
      this.ItemInfo.Item1 = (__Null) sender;
      this.ItemInfo.Item2 = (__Null) receiver;
      this.ItemInfo.Item3 = (__Null) nodeUI;
      this.ItemInfo.Item4 = (__Null) currentID;
      this.ItemInfo.Item5 = (__Null) buttonType;
    }

    public void ClearItemInfo()
    {
      this.ItemInfo.Item1 = null;
      this.ItemInfo.Item2 = null;
      this.ItemInfo.Item3 = null;
      this.ItemInfo.Item4 = (__Null) -1;
      this.ItemInfo.Item5 = (__Null) -1;
    }

    public int ItemMaxCount { get; set; }

    public IObservable<Unit> ClickDecide
    {
      get
      {
        Button element = this._buttons.GetElement<Button>(0);
        return element == null ? (IObservable<Unit>) null : UnityUIComponentExtensions.OnClickAsObservable(element);
      }
    }

    public IObservable<Unit> ClickReturn
    {
      get
      {
        Button element = this._buttons.GetElement<Button>(1);
        return element == null ? (IObservable<Unit>) null : UnityUIComponentExtensions.OnClickAsObservable(element);
      }
    }

    public IObservable<Unit> ClickDelete
    {
      get
      {
        Button element = this._buttons.GetElement<Button>(2);
        return element == null ? (IObservable<Unit>) null : UnityUIComponentExtensions.OnClickAsObservable(element);
      }
    }

    public int InputNumber
    {
      get
      {
        int result;
        return Object.op_Equality((Object) this._numInputField, (Object) null) || !int.TryParse(this._numInputField.get_text() ?? string.Empty, out result) ? 0 : result;
      }
      private set
      {
        int num = Mathf.Clamp(value, 0, this.ItemMaxCount);
        this._numInputField.set_text(string.Format("{0}", (object) num));
        this._numInputField.get_textComponent().set_text(string.Format("{0}", (object) num));
      }
    }

    private void SetInputNumber(string numStr)
    {
      int result;
      if (!int.TryParse(numStr ?? string.Empty, out result))
        return;
      this._numInputField.set_text(string.Format("{0}", (object) result));
      this._numInputField.get_textComponent().set_text(string.Format("{0}", (object) result));
    }

    public bool IsNumberInput
    {
      get
      {
        return !Object.op_Equality((Object) this._numInputField, (Object) null) && int.TryParse(this._numInputField.get_text() ?? string.Empty, out int _);
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this._numInputField.set_contentType((InputField.ContentType) 2);
      if (!Object.op_Equality((Object) this._recyclingUI, (Object) null))
        return;
      this._recyclingUI = (RecyclingUI) ((Component) this).GetComponentInParent<RecyclingUI>();
    }

    protected override void OnBeforeStart()
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this._numInputField), (Action<M0>) (str => this.OnInputFieldValueChanged(str))), (Component) this);
      for (int index = 0; index < this._numChangeValues.Length; ++index)
      {
        Button element = this._numChangeButtons.GetElement<Button>(index);
        if (!Object.op_Equality((Object) element, (Object) null))
        {
          int changeValue = this._numChangeValues[index];
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(element), (Action<M0>) (_ => this.OnNumChangeClick(changeValue))), (Component) this);
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(element), (Action<M0>) (_ => this._recyclingUI.PlaySE(SoundPack.SystemSE.OK_S))), (Component) this);
        }
      }
      if (!this._buttons.IsNullOrEmpty<Button>())
      {
        foreach (Button button in this._buttons)
        {
          if (!Object.op_Equality((Object) button, (Object) null))
            DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(button), (Action<M0>) (_ => this._recyclingUI.PlaySE(SoundPack.SystemSE.OK_S))), (Component) this);
        }
      }
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) this.ClickDelete, (Action<M0>) (_ => this.OnClickDeleteButton())), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<RecyclingInventoryFacadeViewer>(Observable.Where<RecyclingInventoryFacadeViewer>(Observable.Where<RecyclingInventoryFacadeViewer>((IObservable<M0>) this._recyclingUI.OnInventoryChanged, (Func<M0, bool>) (_ => this.ItemInfo.Item5 == 1)), (Func<M0, bool>) (_ => this.ItemInfo.Item2 != null)), (Action<M0>) (receiver => this.ItemInfo.Item2 = (__Null) receiver.ListController)), (Component) this);
      IObservable<Unit> observable1 = (IObservable<Unit>) Observable.Select<RecyclingInventoryFacadeViewer, Unit>((IObservable<M0>) this._recyclingUI.OnInventoryChanged, (Func<M0, M1>) (_ => Unit.get_Default()));
      IObservable<Unit> observable2 = (IObservable<Unit>) Observable.Select<StuffItem, Unit>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>((IObservable<M0>) this._recyclingUI.DecidedItemSlotUI.DeleteEvent, (Func<M0, bool>) (_ => this.ItemInfo.Item5 == 1)), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Func<M0, bool>) (item => item != null)), (Func<M0, bool>) (item => ((ItemNodeUI) this.ItemInfo.Item3).Item == item)), (Func<M0, bool>) (item => 0 < item.Count)), (Func<M0, M1>) (_ => Unit.get_Default()));
      IObservable<Unit> observable3 = (IObservable<Unit>) Observable.Select<StuffItem, Unit>(Observable.Where<StuffItem>(Observable.Where<StuffItem>((IObservable<M0>) this._recyclingUI.DecidedItemSlotUI.DeleteEvent, (Func<M0, bool>) (_ => this.ItemInfo.Item5 == 0)), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Func<M0, M1>) (_ => Unit.get_Default()));
      IObservable<Unit> observable4 = (IObservable<Unit>) Observable.DelayFrame<Unit>((IObservable<M0>) Observable.Select<PanelType, Unit>(Observable.Where<PanelType>((IObservable<M0>) this._recyclingUI.OnDoubleClicked, (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Func<M0, M1>) (_ => Unit.get_Default())), 1, (FrameCountType) 0);
      IObservable<Unit> observable5 = (IObservable<Unit>) Observable.Select<StuffItem, Unit>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>((IObservable<M0>) this._recyclingUI.CreateItemStockUI.OnAddCreateItem, (Func<M0, bool>) (item => item != null)), (Func<M0, bool>) (_ => this.ItemInfo.Item5 == 2)), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Func<M0, M1>) (_ => Unit.get_Default()));
      IObservable<Unit> observable6 = (IObservable<Unit>) Observable.DelayFrame<Unit>((IObservable<M0>) this._recyclingUI.CreateItemStockUI.OnClickDeleteSubmit, 1, (FrameCountType) 0);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Merge<Unit>((IObservable<M0>[]) new IObservable<Unit>[9]
      {
        (IObservable<Unit>) Observable.DelayFrame<Unit>((IObservable<M0>) this.ClickDecide, 1, (FrameCountType) 0),
        (IObservable<Unit>) Observable.DelayFrame<Unit>((IObservable<M0>) this.ClickReturn, 1, (FrameCountType) 0),
        (IObservable<Unit>) Observable.DelayFrame<Unit>((IObservable<M0>) this.ClickDelete, 1, (FrameCountType) 0),
        observable1,
        observable3,
        observable2,
        observable4,
        observable5,
        observable6
      }), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Action<M0>) (_ => this.RefreshUI())), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>(Observable.Where<StuffItem>((IObservable<M0>) this._recyclingUI.DecidedItemSlotUI.DeleteEvent, (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.IsActiveControl)), (Func<M0, bool>) (_ => this.ItemInfo.Item5 == 1)), (Func<M0, bool>) (item => item != null)), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null))), (Func<M0, bool>) (item => ((ItemNodeUI) this.ItemInfo.Item3).Item == item)), (Func<M0, bool>) (item => item.Count <= 0)), (Action<M0>) (_ => this.DetachItem())), (Component) this);
    }

    private void OnClickDeleteButton()
    {
      if (!((Behaviour) this).get_isActiveAndEnabled() || !this.IsNumberInput)
        return;
      this._recyclingUI.DeleteREquestUI.DoOpen();
    }

    private void OnInputFieldValueChanged(string str)
    {
      int result;
      if (!int.TryParse(str ?? string.Empty, out result))
      {
        this.RefreshInputNumberChangeUI();
      }
      else
      {
        if (this.ItemMaxCount <= 0)
        {
          this._numInputField.set_text("0");
          this._numInputField.get_textComponent().set_text("0");
        }
        else if (result < 1)
        {
          this._numInputField.set_text("1");
          this._numInputField.get_textComponent().set_text("1");
        }
        else if (this.ItemMaxCount < result)
        {
          this._numInputField.set_text(string.Format("{0}", (object) this.ItemMaxCount));
          this._numInputField.get_textComponent().set_text(string.Format("{0}", (object) this.ItemMaxCount));
        }
        this.RefreshInputNumberChangeUI();
      }
    }

    protected override void Start()
    {
      base.Start();
      this.CanvasAlpha = 0.0f;
      bool flag = false;
      this.Interactable = flag;
      this.BlockRaycast = flag;
    }

    private void RefreshItemMaxCount()
    {
      StuffItem stuffItem = !Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null) ? (StuffItem) null : ((ItemNodeUI) this.ItemInfo.Item3).Item;
      this.ItemMaxCount = stuffItem == null ? 0 : stuffItem.Count;
    }

    private void RefreshButtonInteractable()
    {
      int inputNumber = this.InputNumber;
      if (!this._buttons.IsNullOrEmpty<Button>())
      {
        foreach (Selectable button in this._buttons)
          this.SetInteractable(button, 0 < inputNumber);
      }
      this.RefreshInputNumberChangeUI();
    }

    private void RefreshInputNumberChangeUI()
    {
      this.RefreshSendButtonInteractable();
      this.RefreshNumChangeButtonInteractable();
    }

    private void RefreshSendButtonInteractable()
    {
      if (this._buttons.IsNullOrEmpty<Button>())
        return;
      if (!this.IsNumberInput)
      {
        this.SetInteractableSendButton(false);
      }
      else
      {
        StuffItem stuffItem = !Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null) ? (StuffItem) null : ((ItemNodeUI) this.ItemInfo.Item3).Item;
        ItemListController itemListController = (ItemListController) this.ItemInfo.Item2;
        if (stuffItem == null || itemListController == null)
          this.SetInteractableSendButton(false);
        else
          this.SetInteractableSendButton(this.InputNumber <= itemListController.PossibleCount(stuffItem));
      }
    }

    private void SetInteractableSendButton(bool active)
    {
      if (this._buttons.IsNullOrEmpty<Button>())
        return;
      for (int index = 0; index < this._buttons.Length; ++index)
      {
        Button element = this._buttons.GetElement<Button>(index);
        if (!Object.op_Equality((Object) element, (Object) null))
        {
          bool flag = index == 2 ? 0 < this.InputNumber : active;
          this.SetInteractable((Selectable) element, flag);
        }
      }
    }

    private void RefreshNumChangeButtonInteractable()
    {
      if (this._numChangeButtons.IsNullOrEmpty<Button>())
        return;
      if (0 < this.ItemMaxCount)
      {
        if (this.IsNumberInput)
        {
          int inputNumber = this.InputNumber;
          for (int index = 0; index < this._numChangeValues.Length; ++index)
          {
            Button element = this._numChangeButtons.GetElement<Button>(index);
            if (!Object.op_Equality((Object) element, (Object) null))
            {
              int numChangeValue = this._numChangeValues[index];
              bool flag = numChangeValue >= 0 ? 0 < numChangeValue && inputNumber < this.ItemMaxCount : 1 < inputNumber;
              this.SetInteractable((Selectable) element, flag);
            }
          }
        }
        else
        {
          foreach (Selectable numChangeButton in this._numChangeButtons)
            this.SetInteractable(numChangeButton, true);
        }
      }
      else
      {
        foreach (Selectable numChangeButton in this._numChangeButtons)
          this.SetInteractable(numChangeButton, false);
      }
    }

    private void OnNumChangeClick(int addValue)
    {
      int num = Mathf.Min(Mathf.Max(1, (!this.IsNumberInput ? 1 : this.InputNumber) + addValue), !Object.op_Inequality((Object) this.ItemInfo.Item3, (Object) null) || ((ItemNodeUI) this.ItemInfo.Item3).Item == null ? 1 : ((ItemNodeUI) this.ItemInfo.Item3).Item.Count);
      this._numInputField.set_text(string.Format("{0}", (object) num));
      this._numInputField.get_textComponent().set_text(string.Format("{0}", (object) num));
      this.RefreshInputNumberChangeUI();
    }

    public void Refresh()
    {
      this.CanvasAlpha = 0.0f;
      bool flag = false;
      this.Interactable = flag;
      this.BlockRaycast = flag;
      this._itemNameText.set_text(string.Empty);
      this._flavorText.set_text(string.Empty);
      this.ClearItemInfo();
      this.RefreshUI();
    }

    public void RefreshUI()
    {
      this.RefreshItemMaxCount();
      this.RefreshInputFieldUI();
      this.RefreshButtonInteractable();
    }

    private void RefreshInputFieldUI()
    {
      int num1 = Mathf.Min(Mathf.Max(1, this.InputNumber), this.ItemMaxCount);
      ItemNodeUI itemNodeUi = (ItemNodeUI) this.ItemInfo.Item3;
      StuffItem stuffItem = !Object.op_Inequality((Object) itemNodeUi, (Object) null) ? (StuffItem) null : itemNodeUi.Item;
      int num2 = stuffItem == null ? 0 : stuffItem.Count;
      int num3 = Mathf.Clamp(num1, 0, num2);
      this._numInputField.set_text(string.Format("{0}", (object) num3));
      this._numInputField.get_textComponent().set_text(string.Format("{0}", (object) num3));
    }

    public void AttachDeleteItem(ItemListController sender, ItemNodeUI itemUI, int currentID)
    {
      this.ClearItemInfo();
      if (Object.op_Equality((Object) itemUI, (Object) null) || itemUI.Item == null || itemUI.Item.Count <= 0)
      {
        this.IsActiveControl = false;
      }
      else
      {
        StuffItem stuffItem = itemUI.Item;
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, stuffItem.ID);
        if (stuffItemInfo == null)
        {
          this.IsActiveControl = false;
        }
        else
        {
          ButtonType buttonType = ButtonType.Delete;
          this.SetItemInfo(sender, (ItemListController) null, itemUI, currentID, buttonType);
          this.InputNumber = 1;
          this.RefreshUI();
          this._itemNameText.set_text(stuffItemInfo.Name);
          this._flavorText.set_text(stuffItemInfo.Explanation);
          int num = (int) buttonType;
          for (int index = 0; index < this._buttons.Length; ++index)
          {
            Button element = this._buttons.GetElement<Button>(index);
            if (!Object.op_Equality((Object) element, (Object) null))
              this.SetActive((Component) element, index == num);
          }
          this.IsActiveControl = true;
        }
      }
    }

    public void AttachItem(
      ItemListController sender,
      ItemListController receiver,
      int currentID,
      ItemNodeUI itemUI,
      ButtonType buttonType)
    {
      this.ClearItemInfo();
      if (sender == null || receiver == null)
        this.IsActiveControl = false;
      else if (!Singleton<Resources>.IsInstance() || Object.op_Equality((Object) itemUI, (Object) null) || (itemUI.Item == null || itemUI.Item.Count <= 0))
      {
        this.IsActiveControl = false;
      }
      else
      {
        StuffItem stuffItem = itemUI.Item;
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, stuffItem.ID);
        if (stuffItemInfo == null)
        {
          this.IsActiveControl = false;
        }
        else
        {
          this.SetItemInfo(sender, receiver, itemUI, currentID, buttonType);
          this.InputNumber = 1;
          this.RefreshUI();
          this._itemNameText.set_text(stuffItemInfo.Name);
          this._flavorText.set_text(stuffItemInfo.Explanation);
          int num = (int) buttonType;
          for (int index = 0; index < this._buttons.Length; ++index)
          {
            Button element = this._buttons.GetElement<Button>(index);
            if (!Object.op_Equality((Object) element, (Object) null) && ((Component) element).get_gameObject().get_activeSelf() != (index == num))
              ((Component) element).get_gameObject().SetActive(index == num);
          }
          this.IsActiveControl = true;
        }
      }
    }

    public void DetachItem()
    {
      this.ClearItemInfo();
      this.ItemMaxCount = 0;
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      this._fadeDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingInfoPanelUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingInfoPanelUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
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

    private void SetInteractable(Selectable obj, bool flag)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_interactable() == flag)
        return;
      obj.set_interactable(flag);
    }
  }
}
