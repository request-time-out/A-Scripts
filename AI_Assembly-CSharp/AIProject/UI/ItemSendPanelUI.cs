// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemSendPanelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ItemSendPanelUI : MenuUIBehaviour
  {
    [SerializeField]
    private Sprite[] _sprites = new Sprite[2];
    private BoolReactiveProperty _takeout = new BoolReactiveProperty(false);
    private IntReactiveProperty _selectID = new IntReactiveProperty(0);
    private ReactiveProperty<ItemNodeUI> selectionItem = new ReactiveProperty<ItemNodeUI>();
    [SerializeField]
    private ItemSendPanelUI.StorageType _storageType;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text _itemName;
    [SerializeField]
    private Text _flavorText;
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private GameObject _trashField;
    [SerializeField]
    private Button _sendButton;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private RectTransform _countViewerLayout;
    [SerializeField]
    private CountViewer _countViewer;
    private bool _hasItemNode;
    private IDisposable _fadeDisposable;

    public IObservable<int> SendToInventory
    {
      get
      {
        return (IObservable<int>) Observable.Select<Unit, int>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._sendButton), (Func<M0, bool>) (_ => ((ReactiveProperty<bool>) this._takeout).get_Value())), (Func<M0, M1>) (_ => this.Count));
      }
    }

    public IObservable<int> SendToItemBox
    {
      get
      {
        return (IObservable<int>) Observable.Select<Unit, int>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._sendButton), (Func<M0, bool>) (_ => !((ReactiveProperty<bool>) this._takeout).get_Value())), (Func<M0, M1>) (_ => this.Count));
      }
    }

    public IObservable<int> RemoveOnClick
    {
      get
      {
        return (IObservable<int>) Observable.Select<Unit, int>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._removeButton), (Func<M0, M1>) (_ => this.Count));
      }
    }

    public bool takeout
    {
      get
      {
        return ((ReactiveProperty<bool>) this._takeout).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._takeout).set_Value(value);
      }
    }

    public Image cursor
    {
      get
      {
        return this._cursor;
      }
    }

    public int BackFocusLevel { get; set; }

    public int Count
    {
      get
      {
        return this._countViewer.Count;
      }
    }

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public bool isOpen
    {
      get
      {
        return this.IsActiveControl;
      }
    }

    public void Open(ItemNodeUI node)
    {
      this.selectionItem.set_Value(node);
    }

    public void Close()
    {
      this.selectionItem.SetValueAndForceNotify((ItemNodeUI) null);
    }

    public void Refresh()
    {
      if (Object.op_Equality((Object) this.selectionItem.get_Value(), (Object) null))
        return;
      this.Refresh(this.selectionItem.get_Value().Item);
    }

    public void Refresh(StuffItem item)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID);
      this._itemName.set_text(stuffItemInfo.Name);
      this._flavorText.set_text(stuffItemInfo.Explanation);
      if (Object.op_Inequality((Object) this._trashField, (Object) null))
        this._trashField.SetActive(stuffItemInfo.isTrash);
      this._countViewer.MaxCount = Mathf.Max(item.Count, 1);
      this._countViewer.ForceCount = Mathf.Clamp(this._countViewer.Count, 1, this._countViewer.MaxCount);
    }

    [DebuggerHidden]
    private IEnumerator SlotBind()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemSendPanelUI.\u003CSlotBind\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadCountViewer()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemSendPanelUI.\u003CLoadCountViewer\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      if (Object.op_Equality((Object) this._countViewer, (Object) null))
        ((MonoBehaviour) this).StartCoroutine(this.LoadCountViewer());
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ObservableExtensions.Subscribe<ItemNodeUI>((IObservable<M0>) this.selectionItem, (Action<M0>) (node =>
      {
        this._hasItemNode = Object.op_Inequality((Object) node, (Object) null);
        if (this._hasItemNode)
        {
          this.IsActiveControl = true;
          this.Refresh(node.Item);
        }
        else
        {
          if (!this.isOpen)
            return;
          this.IsActiveControl = false;
        }
      }));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<bool, int>((IObservable<M0>) this._takeout, (Func<M0, M1>) (isOn => isOn ? 1 : 0)), (Action<M0>) (index => ((Selectable) this._sendButton).get_image().set_sprite(this._sprites.GetElement<Sprite>(index))));
      ((MonoBehaviour) this).StartCoroutine(this.SlotBind());
      ObservableExtensions.Subscribe<int>(Observable.Merge<int>((IObservable<M0>[]) new IObservable<int>[2]
      {
        (IObservable<int>) Observable.Select<PointerEventData, int>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._sendButton), (Func<M0, M1>) (_ => 0)),
        (IObservable<int>) Observable.Select<PointerEventData, int>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._removeButton), (Func<M0, M1>) (_ => 1))
      }), (Action<M0>) (id => ((ReactiveProperty<int>) this._selectID).set_Value(id)));
      ObservableExtensions.Subscribe<RectTransform>(Observable.Where<RectTransform>((IObservable<M0>) Observable.Select<int, RectTransform>((IObservable<M0>) this._selectID, (Func<M0, M1>) (i =>
      {
        if (i == 0)
          return (RectTransform) ((Component) this._sendButton).GetComponent<RectTransform>();
        return i == 1 ? (RectTransform) ((Component) this._removeButton).GetComponent<RectTransform>() : (RectTransform) null;
      })), (Func<M0, bool>) (rt => Object.op_Inequality((Object) rt, (Object) null))), (Action<M0>) (rt => CursorFrame.Set(((Graphic) this.cursor).get_rectTransform(), rt, (RectTransform) null)));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__11)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__12)));
      this._actionCommands.Add(actionIdDownCommand2);
      base.Start();
    }

    private void OnUpdate()
    {
      ((Behaviour) this.cursor).set_enabled(Singleton<Input>.Instance.FocusLevel == this.FocusLevel && this.EnabledInput);
      if (!this._hasItemNode)
        return;
      if (Object.op_Equality((Object) this.selectionItem.get_Value(), (Object) null))
      {
        this.selectionItem.SetValueAndForceNotify((ItemNodeUI) null);
      }
      else
      {
        if (this.selectionItem.get_Value().Visible)
          return;
        this.selectionItem.set_Value((ItemNodeUI) null);
      }
    }

    private void OnDestroy()
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = (IDisposable) null;
    }

    private void SetActiveControl(bool isActive)
    {
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemSendPanelUI.\u003CDoOpen\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemSendPanelUI.\u003CDoClose\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      if (!this.isOpen)
        return;
      switch (((ReactiveProperty<int>) this._selectID).get_Value())
      {
        case 0:
          ((UnityEvent) this._sendButton.get_onClick()).Invoke();
          break;
        case 1:
          ((UnityEvent) this._removeButton.get_onClick()).Invoke();
          break;
      }
    }

    private void OnInputCancel()
    {
      if (!this.isOpen)
        return;
      this.Close();
      this.OnCancel?.Invoke();
    }

    private enum StorageType
    {
      Storage,
      Pantry,
    }
  }
}
