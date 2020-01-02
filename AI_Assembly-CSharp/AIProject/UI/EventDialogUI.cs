// Decompiled with JetBrains decompiler
// Type: AIProject.UI.EventDialogUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class EventDialogUI : MenuUIBehaviour
  {
    [SerializeField]
    private Color _defaultColor = Color.get_white();
    [SerializeField]
    private Color[] _colors = new Color[0];
    private CommandLabel.AcceptionState _prevCommandAcception = CommandLabel.AcceptionState.None;
    private float _prevTimeScale = 1f;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text _messageText;
    [SerializeField]
    private Text _submitText;
    [SerializeField]
    private Text _cancelText;
    [SerializeField]
    private Button _submitButton;
    [SerializeField]
    private Button _cancelButton;
    private Action _submitEvent;
    private Action _cancelEvent;
    private Action _closedEvent;
    private bool _submit;
    private bool _cancel;
    private IObservable<TimeInterval<float>> _lerpStream;
    private MenuUIBehaviour[] _uiElements;
    private IDisposable _fadeDisposable;
    private Input.ValidType _prevInputValid;
    private bool _prevPlayerScheduleInteraction;

    public Action SubmitEvent
    {
      get
      {
        return this._submitEvent;
      }
      set
      {
        this._submitEvent = value;
      }
    }

    public Action CancelEvent
    {
      get
      {
        return this._cancelEvent;
      }
      set
      {
        this._cancelEvent = value;
      }
    }

    public Action ClosedEvent
    {
      get
      {
        return this._closedEvent;
      }
      set
      {
        this._closedEvent = value;
      }
    }

    public float TimeScale { get; set; }

    public string MessageText
    {
      get
      {
        return this._messageText.get_text();
      }
      set
      {
        this._messageText.set_text(value);
      }
    }

    public string SubmitButtonText
    {
      get
      {
        return this._submitText.get_text();
      }
      set
      {
        this._submitText.set_text(value);
      }
    }

    public string CancelButtonText
    {
      get
      {
        return this._cancelText.get_text();
      }
      set
      {
        this._cancelText.set_text(value);
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      protected set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this.SubmitEvent = (Action) null;
      this.CancelEvent = (Action) null;
      this.ClosedEvent = (Action) null;
      this._submit = false;
      this._cancel = false;
      Text messageText = this._messageText;
      Color defaultColor = this._defaultColor;
      ((Graphic) this._cancelText).set_color(defaultColor);
      Color color1 = defaultColor;
      ((Graphic) this._submitText).set_color(color1);
      Color color2 = color1;
      ((Graphic) messageText).set_color(color2);
    }

    public override bool IsActiveControl
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActive).get_Value();
      }
      set
      {
        if (((ReactiveProperty<bool>) this._isActive).get_Value() == value)
          return;
        ((ReactiveProperty<bool>) this._isActive).set_Value(value);
      }
    }

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] uiElements = this._uiElements;
        if (uiElements != null)
          return uiElements;
        return this._uiElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      this._lerpStream = (IObservable<TimeInterval<float>>) Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._alphaAccelerationTime, true), true), (Component) this);
      // ISSUE: method pointer
      ((UnityEvent) this._submitButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      // ISSUE: method pointer
      ((UnityEvent) this._cancelButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      this._keyCommands.Add(keyCodeDownCommand);
    }

    protected override void OnAfterStart()
    {
      this.SetCanvasGroupParam(false, false);
      this.CanvasAlpha = 0.0f;
      this.EnabledInput = false;
      this.SetActive(false);
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    private void DoSubmit()
    {
      this.PlaySystemSE(SoundPack.SystemSE.OK_L);
      this._submit = true;
      this.IsActiveControl = false;
    }

    private void DoCancel()
    {
      this.PlaySystemSE(SoundPack.SystemSE.Cancel);
      this._cancel = true;
      this.IsActiveControl = false;
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new EventDialogUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new EventDialogUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void SetCanvasGroupParam(bool blockRaycasts, bool interactable)
    {
      CanvasGroup canvasGroup = this._canvasGroup;
      if (Object.op_Equality((Object) canvasGroup, (Object) null))
        return;
      if (canvasGroup.get_blocksRaycasts() != blockRaycasts)
        canvasGroup.set_blocksRaycasts(blockRaycasts);
      if (canvasGroup.get_interactable() == interactable)
        return;
      canvasGroup.set_interactable(interactable);
    }

    private void SetBlockRaycasts(bool blockRaycasts)
    {
      CanvasGroup canvasGroup = this._canvasGroup;
      if (Object.op_Equality((Object) canvasGroup, (Object) null) || canvasGroup.get_blocksRaycasts() == blockRaycasts)
        return;
      canvasGroup.set_blocksRaycasts(blockRaycasts);
    }

    private void SetInteractable(bool interactable)
    {
      CanvasGroup canvasGroup = this._canvasGroup;
      if (Object.op_Equality((Object) canvasGroup, (Object) null) || canvasGroup.get_interactable() == interactable)
        return;
      canvasGroup.set_interactable(interactable);
    }

    public Color GetListColor(int id)
    {
      return this._colors.IsNullOrEmpty<Color>() || 0 > id || id >= this._colors.Length ? this._defaultColor : this._colors[id];
    }

    public void SetMessageColor(Color color)
    {
      ((Graphic) this._messageText).set_color(color);
    }

    public void SetMessageColor(int id)
    {
      ((Graphic) this._messageText).set_color(this.GetListColor(id));
    }

    private void EventInvoke(ref Action action)
    {
      if (action == null)
        return;
      Action action1 = action;
      action = (Action) null;
      action1();
    }

    private void SetActive(bool active)
    {
      if (((Component) this).get_gameObject().get_activeSelf() == active)
        return;
      ((Component) this).get_gameObject().SetActive(active);
    }

    private void PlaySystemSE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
