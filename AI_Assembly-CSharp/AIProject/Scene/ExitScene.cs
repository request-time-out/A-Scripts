// Decompiled with JetBrains decompiler
// Type: AIProject.Scene.ExitScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using ReMotion;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AIProject.Scene
{
  public class ExitScene : MenuUIBehaviour
  {
    private float _timeScale = 1f;
    private IntReactiveProperty _selectedID = new IntReactiveProperty(1);
    [SerializeField]
    private ConfirmationButton _runButton;
    [SerializeField]
    private ConfirmationButton _cancelButton;
    [SerializeField]
    private CanvasGroup _backgroundCanvasGroup;
    [SerializeField]
    private CanvasGroup _panelCanvasGroup;
    private Input.ValidType _validType;
    private ConfirmationButton _selectedButton;

    private void DisableRaycast()
    {
      this._runButton.DisableRaycast();
      this._cancelButton.DisableRaycast();
    }

    protected override void Awake()
    {
      if (Singleton<Game>.IsInstance())
      {
        if (Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null))
          Singleton<Game>.Instance.DestroyDialog();
        Singleton<Game>.Instance.ExitScene = this;
      }
      this._timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
      if (!Singleton<Input>.IsInstance())
        return;
      this._validType = Singleton<Input>.Instance.State;
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
    }

    protected override void Start()
    {
      this._runButton.AddListener((Action) (() => this.Exit()));
      this._runButton.AddListener((Action) (() =>
      {
        if (!Singleton<Resources>.IsInstance())
          return;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      }));
      this._runButton.AddListener((Action) (() => this.DisableRaycast()));
      this._cancelButton.AddListener((Action) (() => this.Cancel()));
      this._cancelButton.AddListener((Action) (() =>
      {
        if (!Singleton<Resources>.IsInstance())
          return;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      }));
      this._cancelButton.AddListener((Action) (() => this.DisableRaycast()));
      this.Open();
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand();
      actionIdDownCommand1.ActionID = ActionID.Submit;
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__6)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand();
      actionIdDownCommand2.ActionID = ActionID.Cancel;
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__7)));
      this._actionCommands.Add(actionIdDownCommand2);
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._selectedID, (Action<M0>) (x => this.UpdateSelectedFrame(x)));
      this.UpdateSelectedFrame(1);
      // ISSUE: method pointer
      this._runButton.OnPointerEnterEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      // ISSUE: method pointer
      this._cancelButton.OnPointerEnterEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      base.Start();
    }

    private void UpdateSelectedFrame(int x)
    {
      this._runButton.IsActiveSelectedFrame = x == 0;
      this._cancelButton.IsActiveSelectedFrame = x == 1;
      if (x == 0)
        this._selectedButton = this._runButton;
      else
        this._selectedButton = this._cancelButton;
    }

    protected override void OnDisable()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Singleton<Game>.Instance.ExitScene = (ExitScene) null;
    }

    private void Open()
    {
      this._panelCanvasGroup.set_blocksRaycasts(false);
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x =>
      {
        CanvasGroup backgroundCanvasGroup = this._backgroundCanvasGroup;
        float num1 = ((TimeInterval<float>) ref x).get_Value();
        this._panelCanvasGroup.set_alpha(num1);
        double num2 = (double) num1;
        backgroundCanvasGroup.set_alpha((float) num2);
      }), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() => this._panelCanvasGroup.set_blocksRaycasts(true)));
    }

    private void Close(Action onCompleted)
    {
      this._panelCanvasGroup.set_blocksRaycasts(false);
      Time.set_timeScale(this._timeScale);
      if (Singleton<Input>.IsInstance())
      {
        Singleton<Input>.Instance.ReserveState(this._validType);
        Singleton<Input>.Instance.SetupState();
      }
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, true), true), (Action<M0>) (x =>
      {
        CanvasGroup backgroundCanvasGroup = this._backgroundCanvasGroup;
        float num1 = 1f - ((TimeInterval<float>) ref x).get_Value();
        this._panelCanvasGroup.set_alpha(num1);
        double num2 = (double) num1;
        backgroundCanvasGroup.set_alpha((float) num2);
      }), (Action<Exception>) (ex => Debug.LogException(ex)), onCompleted);
    }

    private void Exit()
    {
      this.Close((Action) (() =>
      {
        Singleton<Manager.Scene>.Instance.GameEnd(false);
        Singleton<Manager.Scene>.Instance.isSkipGameExit = false;
      }));
    }

    private void Cancel()
    {
      this.Close((Action) (() =>
      {
        Object.Destroy((Object) ((Component) this).get_gameObject());
        Singleton<Manager.Scene>.Instance.isGameEndCheck = true;
        Singleton<Manager.Scene>.Instance.isSkipGameExit = false;
        GC.Collect();
        Resources.UnloadUnusedAssets();
      }));
    }

    private void OnInputSubmit()
    {
      this._selectedButton.Invoke();
    }

    private void OnInputCancel()
    {
      this._cancelButton.Invoke();
    }

    public override void OnInputMoveDirection(MoveDirection moveDir)
    {
      if (moveDir != null)
      {
        if (moveDir != 2)
          return;
        IntReactiveProperty selectedId = this._selectedID;
        ((ReactiveProperty<int>) selectedId).set_Value(((ReactiveProperty<int>) selectedId).get_Value() + 1);
        if (((ReactiveProperty<int>) this._selectedID).get_Value() <= 1)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(0);
      }
      else
      {
        IntReactiveProperty selectedId = this._selectedID;
        ((ReactiveProperty<int>) selectedId).set_Value(((ReactiveProperty<int>) selectedId).get_Value() - 1);
        if (((ReactiveProperty<int>) this._selectedID).get_Value() >= 0)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(1);
      }
    }
  }
}
