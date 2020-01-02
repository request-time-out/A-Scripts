// Decompiled with JetBrains decompiler
// Type: AIProject.UI.WarningMessageElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class WarningMessageElement : UIBehaviour
  {
    [SerializeField]
    private RectTransform myTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private UnityEngine.UI.Text messageText;
    private FadeInfo fadeInInfo;
    private FadeInfo displayInfo;
    private FadeInfo fadeOutInfo;
    private CompositeDisposable fadeInDisposable;
    private CompositeDisposable displayDisposable;
    private CompositeDisposable fadeOutDisposable;

    public WarningMessageElement()
    {
      base.\u002Ector();
    }

    public WarningMessageUI Root { get; set; }

    public Action<WarningMessageElement> EndAction { get; set; }

    public Action ClosedAction { get; set; }

    public bool PlayingFadeIn
    {
      get
      {
        return this.fadeInDisposable != null;
      }
    }

    public bool PlayingDisplay
    {
      get
      {
        return this.displayDisposable != null;
      }
    }

    public bool PlayingFadeOut
    {
      get
      {
        return this.fadeOutDisposable != null;
      }
    }

    public bool isFadeInForOutWait { get; set; }

    protected virtual void OnEnable()
    {
      this.Dispose();
      base.OnEnable();
    }

    protected virtual void OnDisable()
    {
      this.Dispose();
      base.OnDisable();
    }

    public string Text
    {
      get
      {
        return this.messageText.get_text();
      }
      set
      {
        this.messageText.set_text(value);
      }
    }

    public Color Color
    {
      set
      {
        ((Graphic) this.messageText).set_color(value);
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this.canvasGroup, (Object) null) ? this.canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this.canvasGroup, (Object) null))
          return;
        this.canvasGroup.set_alpha(value);
      }
    }

    public Vector3 LocalScale
    {
      get
      {
        return ((Transform) this.myTransform).get_localScale();
      }
      set
      {
        ((Transform) this.myTransform).set_localScale(value);
      }
    }

    public void SetFadeInfo(FadeInfo _fadeInInfo, FadeInfo _displayInfo, FadeInfo _fadeOutInfo)
    {
      this.fadeInInfo = _fadeInInfo;
      this.displayInfo = _displayInfo;
      this.fadeOutInfo = _fadeOutInfo;
    }

    public void Dispose()
    {
      if (this.fadeInDisposable != null)
        this.fadeInDisposable.Dispose();
      if (this.displayDisposable != null)
        this.displayDisposable.Dispose();
      if (this.fadeOutDisposable != null)
        this.fadeOutDisposable.Dispose();
      this.fadeInDisposable = (CompositeDisposable) null;
      this.displayDisposable = (CompositeDisposable) null;
      this.fadeOutDisposable = (CompositeDisposable) null;
    }

    public void StartFadeIn()
    {
      this.Dispose();
      this.fadeInDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.FadeInCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.fadeInDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeInCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningMessageElement.\u003CFadeInCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void StartDisplay()
    {
      this.Dispose();
      this.displayDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.DisplayCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.displayDisposable);
    }

    [DebuggerHidden]
    private IEnumerator DisplayCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningMessageElement.\u003CDisplayCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void StartFadeOut()
    {
      this.Dispose();
      this.fadeOutDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.FadeOutCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this)), (ICollection<IDisposable>) this.fadeOutDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeOutCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WarningMessageElement.\u003CFadeOutCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }
  }
}
