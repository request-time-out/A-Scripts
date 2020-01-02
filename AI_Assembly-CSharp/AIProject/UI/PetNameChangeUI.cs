// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PetNameChangeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

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
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class PetNameChangeUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private InputField _nameInputField;
    [SerializeField]
    private Button _submitButton;
    private IDisposable _activeChangeDisposable;
    private IDisposable _openDisposable;
    private IDisposable _closeDisposable;

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this._rectTransform;
      }
    }

    public Button CloseButton
    {
      get
      {
        return this._closeButton;
      }
    }

    public InputField NameInputField
    {
      get
      {
        return this._nameInputField;
      }
    }

    public Button SubmitButton
    {
      get
      {
        return this._submitButton;
      }
    }

    public Action<string> SubmitAction { get; set; }

    public Action<string> CancelAction { get; set; }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._rectTransform, (Object) null))
        this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.EnabledInput = false;
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        return;
      this.CanvasAlpha = 0.0f;
      this.SetBlockRaycast(false);
      this.SetInteractable(false);
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (this._closeButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      }
      if (this._submitButton == null)
        return;
      // ISSUE: method pointer
      ((UnityEvent) this._submitButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
    }

    public void Dispose()
    {
      if (this._activeChangeDisposable != null)
      {
        this._activeChangeDisposable.Dispose();
        this._activeChangeDisposable = (IDisposable) null;
      }
      if (this._openDisposable != null)
      {
        this._openDisposable.Dispose();
        this._openDisposable = (IDisposable) null;
      }
      if (this._closeDisposable == null)
        return;
      this._closeDisposable.Dispose();
      this._closeDisposable = (IDisposable) null;
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._activeChangeDisposable != null)
        this._activeChangeDisposable.Dispose();
      this._activeChangeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    public bool IsOpening
    {
      get
      {
        return this._openDisposable != null;
      }
    }

    public bool IsClosing
    {
      get
      {
        return this._closeDisposable != null;
      }
    }

    public void Open()
    {
      if (this.IsOpening)
        return;
      if (this._closeDisposable != null)
        this._closeDisposable.Dispose();
      IEnumerator coroutine = this.OpenCoroutine();
      this._openDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PetNameChangeUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Close()
    {
      if (this.IsClosing)
        return;
      if (this._openDisposable != null)
        this._openDisposable.Dispose();
      IEnumerator coroutine = this.CloseCoroutine();
      this._closeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PetNameChangeUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void DoSubmit()
    {
      this.PlaySystemSE(SoundPack.SystemSE.OK_L);
      Action<string> submitAction = this.SubmitAction;
      if (submitAction != null)
        submitAction(this._nameInputField.get_text());
      this._nameInputField.set_text(string.Empty);
      this.IsActiveControl = false;
    }

    private void DoCancel()
    {
      this.PlaySystemSE(SoundPack.SystemSE.Cancel);
      Action<string> cancelAction = this.CancelAction;
      if (cancelAction != null)
        cancelAction(this._nameInputField.get_text());
      this._nameInputField.set_text(string.Empty);
      this.IsActiveControl = false;
    }

    public void QuickOpen()
    {
      this.CanvasAlpha = 1f;
      this.SetBlockRaycast(true);
      this.SetInteractable(true);
      this.EnabledInput = true;
    }

    public void QuickClose()
    {
      this.IsActiveControl = false;
      this.Dispose();
      this.EnabledInput = false;
      this.SetBlockRaycast(false);
      this.SetInteractable(false);
      this.CanvasAlpha = 0.0f;
    }

    private void SetBlockRaycast(bool active)
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_blocksRaycasts() == active)
        return;
      this._canvasGroup.set_blocksRaycasts(active);
    }

    private void SetInteractable(bool active)
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_interactable() == active)
        return;
      this._canvasGroup.set_interactable(active);
    }

    private void PlaySystemSE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
