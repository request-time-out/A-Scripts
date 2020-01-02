// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PlantInfoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class PlantInfoUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _completeStr = string.Empty;
    [Header("Infomation Setting")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _itemIcon;
    [SerializeField]
    private Text _itemName;
    [SerializeField]
    private Text _timeText;
    [SerializeField]
    private Slider _progressBar;
    [SerializeField]
    private Button _cancelButton;
    private IDisposable _fadeDisposable;
    private IDisposable _updateDisposable;

    public IObservable<bool> OnComplete
    {
      get
      {
        return (IObservable<bool>) Observable.AsObservable<bool>(Observable.Where<bool>((IObservable<M0>) this._finished, (Func<M0, bool>) (finish => finish)));
      }
    }

    public void ItemCancelInteractable(bool interactable)
    {
      ((Selectable) this._cancelButton).set_interactable(interactable);
    }

    public UnityEvent OnSubmit { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public PlantIcon currentIcon
    {
      get
      {
        return this._icon;
      }
    }

    private PlantIcon _icon { get; set; }

    private BoolReactiveProperty _finished { get; } = new BoolReactiveProperty(false);

    public bool isOpen
    {
      get
      {
        return this.IsActiveControl;
      }
    }

    public virtual void Open(PlantIcon icon)
    {
      this.Refresh(icon);
      this.IsActiveControl = true;
    }

    public virtual void Close()
    {
      this._icon = (PlantIcon) null;
      if (!this.isOpen)
        return;
      this.IsActiveControl = false;
    }

    public virtual void Refresh(PlantIcon icon)
    {
      this._icon = icon;
      this._itemName.set_text(icon.itemName);
      this._itemIcon.set_sprite(icon.itemIcon);
      ((ReactiveProperty<bool>) this._finished).set_Value(false);
    }

    private void OnUpdate()
    {
      AIProject.SaveData.Environment.PlantInfo info = this._icon.info;
      Slider progressBar = this._progressBar;
      float? progress = info?.progress;
      double num = !progress.HasValue ? 1.0 : (double) progress.Value;
      progressBar.set_value((float) num);
      bool? isEnd = info?.isEnd;
      bool flag = !isEnd.HasValue || isEnd.Value;
      this._timeText.set_text(!flag ? info.ToString() : this._completeStr);
      ((ReactiveProperty<bool>) this._finished).set_Value(flag);
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      this._updateDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.isOpen)), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this._icon, (Object) null))), (Func<M0, bool>) (_ => !((ReactiveProperty<bool>) this._finished).get_Value())), (Action<M0>) (_ => this.OnUpdate()));
      if (Object.op_Inequality((Object) this._cancelButton, (Object) null))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._cancelButton), (Action<M0>) (_ => this.OnInputSubmit()));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__7)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__8)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.SquareX
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      this._actionCommands.Add(actionIdDownCommand4);
      base.Start();
    }

    protected virtual void OnDestroy()
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = (IDisposable) null;
      if (this._updateDisposable != null)
        this._updateDisposable.Dispose();
      this._updateDisposable = (IDisposable) null;
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
      return (IEnumerator) new PlantInfoUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlantInfoUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
      if (!this.isOpen)
        return;
      this.OnSubmit?.Invoke();
    }

    private void OnInputCancel()
    {
      if (!this.isOpen)
        return;
      this.Close();
      this.OnCancel?.Invoke();
    }
  }
}
