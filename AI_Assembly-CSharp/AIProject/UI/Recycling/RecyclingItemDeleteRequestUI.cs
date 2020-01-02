// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingItemDeleteRequestUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Recycling
{
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class RecyclingItemDeleteRequestUI : MenuUIBehaviour
  {
    private int _prevFocusLevel = -1;
    [SerializeField]
    private RecyclingUI _recyclingUI;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Text _titleText;
    [SerializeField]
    private Button _submitButton;
    [SerializeField]
    private Button _cancelButton;
    private IDisposable _fadeDisposable;

    public IObservable<Unit> OnSubmitClick
    {
      get
      {
        return UnityUIComponentExtensions.OnClickAsObservable(this._submitButton);
      }
    }

    public IObservable<Unit> OnCancelClick
    {
      get
      {
        return UnityUIComponentExtensions.OnClickAsObservable(this._cancelButton);
      }
    }

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

    protected override void Awake()
    {
      base.Awake();
      if (!Object.op_Equality((Object) this._recyclingUI, (Object) null))
        return;
      this._recyclingUI = (RecyclingUI) ((Component) this).GetComponentInParent<RecyclingUI>();
    }

    protected override void Start()
    {
      base.Start();
      bool flag1 = false;
      this.BlockRaycast = flag1;
      bool flag2 = flag1;
      this.Interactable = flag2;
      this.EnabledInput = flag2;
      this.SetActive(((Component) this).get_gameObject(), false);
    }

    protected override void OnBeforeStart()
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (_ => this.SetActiveControl(_))), (Component) this);
    }

    public void DoOpen()
    {
      this.IsActiveControl = true;
    }

    public void DoClose()
    {
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
      return (IEnumerator) new RecyclingItemDeleteRequestUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingItemDeleteRequestUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void ForcedClose()
    {
      if (this.IsActiveControl && Singleton<Input>.IsInstance())
        Singleton<Input>.Instance.FocusLevel = this._prevFocusLevel;
      this.CanvasAlpha = 0.0f;
      this.EnabledInput = false;
      this.Interactable = false;
      this.BlockRaycast = false;
      this.IsActiveControl = false;
      this.SetActive(((Component) this).get_gameObject(), false);
    }

    private bool SetActive(GameObject obj, bool active)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return false;
      obj.SetActive(active);
      return true;
    }

    private bool SetActive(Component com, bool active)
    {
      if (!Object.op_Inequality((Object) com, (Object) null) || !Object.op_Inequality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return false;
      com.get_gameObject().SetActive(active);
      return true;
    }
  }
}
