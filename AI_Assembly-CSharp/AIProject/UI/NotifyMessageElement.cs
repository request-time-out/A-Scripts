// Decompiled with JetBrains decompiler
// Type: AIProject.UI.NotifyMessageElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

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
  public class NotifyMessageElement : UIBehaviour
  {
    [SerializeField]
    private RectTransform myTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image backPanel;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text messageText;
    private CompositeDisposable fadeInDisposable;
    private CompositeDisposable displayDisposable;
    private CompositeDisposable fadeOutDisposable;
    private CompositeDisposable slidUpDisposable;
    private float fadeInTime;
    private float displayTime;
    private float fadeOutTime;
    private float startAlpha;
    private float endAlpha;

    public NotifyMessageElement()
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

    public Vector3 LocalPosition
    {
      get
      {
        return ((Transform) this.myTransform).get_localPosition();
      }
      set
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
      set
      {
        ((Transform) this.myTransform).set_localScale(value);
      }
    }

    public float Height
    {
      get
      {
        return (float) this.myTransform.get_sizeDelta().y;
      }
    }

    public float Width
    {
      get
      {
        return (float) this.myTransform.get_sizeDelta().x;
      }
    }

    public string MessageText
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

    public Color MessageColor
    {
      get
      {
        return ((Graphic) this.messageText).get_color();
      }
      set
      {
        ((Graphic) this.messageText).set_color(value);
      }
    }

    public int NotifyID { get; set; }

    public Action<NotifyMessageElement> EndActionEvent { get; set; }

    public NotifyMessageList Root { get; set; }

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

    public bool PlayingSlidUp
    {
      get
      {
        return this.slidUpDisposable != null;
      }
    }

    public bool SpeechBubbleIconActive
    {
      get
      {
        return ((Component) this.icon).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this.icon).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this.icon).get_gameObject().SetActive(value);
      }
    }

    protected virtual void OnEnable()
    {
      base.OnEnable();
      this.Dispose();
    }

    protected virtual void OnDisable()
    {
      Action<NotifyMessageElement> endActionEvent = this.EndActionEvent;
      if (endActionEvent != null)
        endActionEvent(this);
      this.Dispose();
      base.OnDisable();
    }

    public void Dispose()
    {
      if (this.fadeInDisposable != null)
        this.fadeInDisposable.Dispose();
      if (this.displayDisposable != null)
        this.displayDisposable.Dispose();
      if (this.fadeOutDisposable != null)
        this.fadeOutDisposable.Dispose();
      if (this.slidUpDisposable != null)
        this.slidUpDisposable.Dispose();
      this.fadeInDisposable = (CompositeDisposable) null;
      this.displayDisposable = (CompositeDisposable) null;
      this.fadeOutDisposable = (CompositeDisposable) null;
      this.slidUpDisposable = (CompositeDisposable) null;
    }

    public void SetTime(float _fadeInTime, float _displayTime, float _fadeOutTime)
    {
      this.fadeInTime = _fadeInTime;
      this.displayTime = _displayTime;
      this.fadeOutTime = _fadeOutTime;
    }

    public void SetAlpha(float _startAlpha, float _endAlpha)
    {
      this.startAlpha = _startAlpha;
      this.endAlpha = _endAlpha;
    }

    public void StartFadeIn(float _posX)
    {
      if (this.fadeInDisposable != null)
        this.fadeInDisposable.Dispose();
      this.fadeInDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.FadeInCoroutine(_posX);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject())), (ICollection<IDisposable>) this.fadeInDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeInCoroutine(float _posX)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyMessageElement.\u003CFadeInCoroutine\u003Ec__Iterator0()
      {
        _posX = _posX,
        \u0024this = this
      };
    }

    public void StartDisplay()
    {
      if (this.displayDisposable != null)
        this.displayDisposable.Dispose();
      this.displayDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.DisplayCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject())), (ICollection<IDisposable>) this.displayDisposable);
    }

    [DebuggerHidden]
    private IEnumerator DisplayCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyMessageElement.\u003CDisplayCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void StartFadeOut()
    {
      if (this.fadeInDisposable != null)
        this.fadeInDisposable.Dispose();
      if (this.displayDisposable != null)
        this.displayDisposable.Dispose();
      if (this.fadeOutDisposable != null)
        this.fadeOutDisposable.Dispose();
      this.fadeInDisposable = (CompositeDisposable) null;
      this.displayDisposable = (CompositeDisposable) null;
      this.fadeOutDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.FadeOutCoroutine();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject())), (ICollection<IDisposable>) this.fadeOutDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeOutCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyMessageElement.\u003CFadeOutCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    public void StartSlidUp(float _posY)
    {
      if (this.slidUpDisposable != null)
        this.slidUpDisposable.Dispose();
      this.slidUpDisposable = new CompositeDisposable();
      IEnumerator _coroutine = this.SlidUpCoroutine(_posY);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDisable<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject())), (ICollection<IDisposable>) this.slidUpDisposable);
    }

    [DebuggerHidden]
    private IEnumerator SlidUpCoroutine(float _posY)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NotifyMessageElement.\u003CSlidUpCoroutine\u003Ec__Iterator3()
      {
        _posY = _posY,
        \u0024this = this
      };
    }
  }
}
