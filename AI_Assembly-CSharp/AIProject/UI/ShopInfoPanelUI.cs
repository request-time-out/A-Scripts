// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ShopInfoPanelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ShopInfoPanelUI : MenuUIBehaviour
  {
    [SerializeField]
    private Sprite[] _backGrounds = new Sprite[2];
    [SerializeField]
    private Sprite[] _sprites = new Sprite[2];
    private ReactiveProperty<ShopInfoPanelUI.Mode> _mode = new ReactiveProperty<ShopInfoPanelUI.Mode>(ShopInfoPanelUI.Mode.Shop);
    private BoolReactiveProperty _decide = new BoolReactiveProperty(false);
    private ReactiveProperty<ItemNodeUI> selectionItem = new ReactiveProperty<ItemNodeUI>();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _backGround;
    [SerializeField]
    private Text _itemName;
    [SerializeField]
    private Text _flavorText;
    [SerializeField]
    private Button _decideButton;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private RectTransform _countViewerLayout;
    [SerializeField]
    private CountViewer _countViewer;
    private bool _hasItemNode;
    private IDisposable _fadeDisposable;

    public IObservable<Unit> Decide
    {
      get
      {
        return (IObservable<Unit>) Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._decideButton), (Func<M0, bool>) (_ => ((ReactiveProperty<bool>) this._decide).get_Value()));
      }
    }

    public IObservable<Unit> Return
    {
      get
      {
        return (IObservable<Unit>) Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._decideButton), (Func<M0, bool>) (_ => !((ReactiveProperty<bool>) this._decide).get_Value()));
      }
    }

    public ShopInfoPanelUI.Mode mode
    {
      get
      {
        return this._mode.get_Value();
      }
      set
      {
        this._mode.set_Value(value);
      }
    }

    public bool decide
    {
      get
      {
        return ((ReactiveProperty<bool>) this._decide).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._decide).set_Value(value);
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

    public Action OnClosed { get; set; }

    public int Count
    {
      get
      {
        return this._countViewer.Count;
      }
    }

    public int MaxCount
    {
      get
      {
        return this._countViewer.MaxCount;
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
      bool flag = true;
      if (this.mode == ShopInfoPanelUI.Mode.Inventory && !stuffItemInfo.isTrash)
        flag = false;
      ((Selectable) this._decideButton).set_interactable(flag);
      this._itemName.set_text(stuffItemInfo.Name);
      this._flavorText.set_text(stuffItemInfo.Explanation);
      this._countViewer.MaxCount = Mathf.Max(item.Count, 1);
      this._countViewer.ForceCount = Mathf.Clamp(this._countViewer.Count, 1, this._countViewer.MaxCount);
    }

    [DebuggerHidden]
    private IEnumerator LoadCountViewer()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopInfoPanelUI.\u003CLoadCountViewer\u003Ec__Iterator0()
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
      ObservableExtensions.Subscribe<ShopInfoPanelUI.Mode>((IObservable<M0>) this._mode, (Action<M0>) (mode => this._backGround.set_sprite(this._backGrounds.GetElement<Sprite>((int) mode))));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<bool, int>((IObservable<M0>) this._decide, (Func<M0, M1>) (isOn => isOn ? 0 : 1)), (Action<M0>) (index => ((Selectable) this._decideButton).get_image().set_sprite(this._sprites.GetElement<Sprite>(index))));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
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
        if (this.selectionItem.get_Value().Visible && this.selectionItem.get_Value().Item.Count != 0)
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
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine = !isActive ? this.DoClose() : this.DoOpen();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopInfoPanelUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopInfoPanelUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      if (!this.isOpen)
        return;
      ((UnityEvent) this._decideButton.get_onClick()).Invoke();
    }

    private void OnInputCancel()
    {
      this.OnCancel?.Invoke();
    }

    public enum Mode
    {
      Shop,
      Inventory,
    }
  }
}
