// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ResultMessageElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class ResultMessageElement : UIBehaviour
  {
    [SerializeField]
    private RectTransform myTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    [Tooltip("表示開始時の大きさ")]
    private float startScale;
    [SerializeField]
    [Tooltip("表示時間")]
    private float displayTime;
    [SerializeField]
    [FoldoutGroup("各透明度", 0)]
    private float startAlpha;
    [SerializeField]
    [FoldoutGroup("各透明度", 0)]
    private float displayAlpha;
    [SerializeField]
    [FoldoutGroup("各透明度", 0)]
    private float endAlpha;
    [SerializeField]
    [Tooltip("フェードインに使用する時間")]
    private float fadeInTime;
    [SerializeField]
    [Tooltip("フェードインの補間タイプ")]
    private ResultMessageElement.FadeType fadeInType;
    [SerializeField]
    [Tooltip("フェードアウトに使用する時間")]
    private float fadeOutTime;
    [SerializeField]
    [Tooltip("フェードアウトの補間タイプ")]
    private ResultMessageElement.FadeType fadeOutType;
    private Vector3 OriginPosition;
    private Vector3 OriginScale;
    private CompositeDisposable fadeInDisposable;
    private CompositeDisposable displayDisposable;
    private CompositeDisposable fadeOutDisposable;

    public ResultMessageElement()
    {
      base.\u002Ector();
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

    public string Message
    {
      get
      {
        return ((TMP_Text) this.messageText).get_text();
      }
    }

    public Vector3 LocalPosition
    {
      get
      {
        return ((Transform) this.myTransform).get_localPosition();
      }
      private set
      {
        ((Transform) this.myTransform).set_localPosition(value);
      }
    }

    public Vector3 LocalScale
    {
      get
      {
        return ((Transform) this.myTransform).get_localScale();
      }
      private set
      {
        ((Transform) this.myTransform).set_localScale(value);
      }
    }

    public ResultMessageUI Root { get; set; }

    public Action<ResultMessageElement> EndAction { get; set; }

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

    protected virtual void Awake()
    {
      base.Awake();
      ((Graphic) this.messageText).set_color((Color) this.Root.White);
      this.OriginPosition = ((Transform) this.myTransform).get_localPosition();
      this.OriginScale = ((Transform) this.myTransform).get_localScale();
      this.CanvasAlpha = 0.0f;
    }

    protected virtual void OnDisable()
    {
      this.Dispose();
      base.OnDisable();
    }

    public bool IsForcedClose { get; private set; }

    public void ShowMessage(string _mes)
    {
      this.Dispose();
      this.IsForcedClose = false;
      ((TMP_Text) this.messageText).set_text(_mes);
      if (this.fadeInDisposable != null)
        this.fadeInDisposable.Dispose();
      this.fadeInDisposable = new CompositeDisposable();
      this.CanvasAlpha = 0.0f;
      IEnumerator _fadeInCoroutine = this.FadeIn();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _fadeInCoroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action) (() => this.fadeInDisposable = (CompositeDisposable) null)), (ICollection<IDisposable>) this.fadeInDisposable);
    }

    public void StartDisplay()
    {
      if (this.displayDisposable != null)
        this.displayDisposable.Dispose();
      this.displayDisposable = new CompositeDisposable();
      this.CanvasAlpha = this.displayAlpha;
      IEnumerator _coroutine = this.OnDisplay();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action) (() => this.displayDisposable = (CompositeDisposable) null)), (ICollection<IDisposable>) this.displayDisposable);
    }

    public void CloseMessage()
    {
      if (this.PlayingFadeOut)
        return;
      this.Dispose();
      this.fadeOutDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.FadeOut();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action) (() => this.fadeOutDisposable = (CompositeDisposable) null)), (ICollection<IDisposable>) this.fadeOutDisposable);
    }

    public void CancelMessage()
    {
      this.Dispose();
      Action<ResultMessageElement> endAction = this.EndAction;
      if (endAction == null)
        return;
      endAction(this);
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

    private IConnectableObservable<TimeInterval<float>> GetObservableEasing(
      ResultMessageElement.FadeType _fadeType,
      float _duration)
    {
      switch (_fadeType)
      {
        case ResultMessageElement.FadeType.Linear:
          return (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDisable<float>((IObservable<M0>) ObservableEasing.Linear(_duration, true), ((Component) this).get_gameObject()), false));
        case ResultMessageElement.FadeType.EaseOutQuint:
          return (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDisable<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(_duration, true), ((Component) this).get_gameObject()), false));
        case ResultMessageElement.FadeType.EaseInQuint:
          return (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDisable<float>((IObservable<M0>) ObservableEasing.EaseInQuint(_duration, true), ((Component) this).get_gameObject()), false));
        default:
          return (IConnectableObservable<TimeInterval<float>>) null;
      }
    }

    [DebuggerHidden]
    private IEnumerator FadeIn()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ResultMessageElement.\u003CFadeIn\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator OnDisplay()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ResultMessageElement.\u003COnDisplay\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator FadeOut()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ResultMessageElement.\u003CFadeOut\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    public enum FadeType
    {
      None,
      Linear,
      EaseOutQuint,
      EaseInQuint,
    }
  }
}
