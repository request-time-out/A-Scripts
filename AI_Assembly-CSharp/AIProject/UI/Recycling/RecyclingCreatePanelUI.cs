// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.RecyclingCreatePanelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
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
  public class RecyclingCreatePanelUI : MenuUIBehaviour
  {
    [SerializeField]
    private RecyclingUI _recyclingUI;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Image _countBarImage;
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

    protected override void Start()
    {
      base.Start();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>(Observable.Where<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUIUpdate())), (Component) this);
    }

    public void DoOpen()
    {
      this.IsActiveControl = true;
    }

    public void DoClose()
    {
      this.IsActiveControl = false;
    }

    public void DoForceOpen()
    {
      this.IsActiveControl = true;
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this.CanvasAlpha = 1f;
      bool flag1 = true;
      this.Interactable = flag1;
      bool flag2 = flag1;
      this.EnabledInput = flag2;
      this.BlockRaycast = flag2;
    }

    public void DoForceClose()
    {
      this.IsActiveControl = false;
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this.CanvasAlpha = 0.0f;
      bool flag1 = false;
      this.Interactable = flag1;
      bool flag2 = flag1;
      this.EnabledInput = flag2;
      this.BlockRaycast = flag2;
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
      return (IEnumerator) new RecyclingCreatePanelUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecyclingCreatePanelUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnUIUpdate()
    {
      RecyclingData recyclingData = this._recyclingUI.RecyclingData;
      if (recyclingData == null)
        return;
      float countLimit = this._recyclingUI.DecidedItemSlotUI.CountLimit;
      if (!recyclingData.CreateCountEnabled)
      {
        Vector3 localScale = ((Component) this._countBarImage).get_transform().get_localScale();
        localScale.x = (__Null) 0.0;
        ((Component) this._countBarImage).get_transform().set_localScale(localScale);
      }
      else if ((double) countLimit <= 0.0)
      {
        Vector3 localScale = ((Component) this._countBarImage).get_transform().get_localScale();
        localScale.x = (__Null) 1.0;
        ((Component) this._countBarImage).get_transform().set_localScale(localScale);
      }
      else
      {
        Vector3 localScale = ((Component) this._countBarImage).get_transform().get_localScale();
        localScale.x = (__Null) (double) Mathf.Clamp01(recyclingData.CreateCounter / countLimit);
        ((Component) this._countBarImage).get_transform().set_localScale(localScale);
      }
    }

    private void SetActive(GameObject obj, bool flag)
    {
      if (!Object.op_Inequality((Object) obj, (Object) null) || obj.get_activeSelf() == flag)
        return;
      obj.SetActive(flag);
    }
  }
}
