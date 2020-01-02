// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalNicknameUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.Animal
{
  public class AnimalNicknameUI : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text _nicknameText;
    [SerializeField]
    private Image _backImage;
    [SerializeField]
    private Transform _rotateRoot;
    private CompositeDisposable _fadeDisposable;

    public AnimalNicknameUI()
    {
      base.\u002Ector();
    }

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    public Text NicknameText
    {
      get
      {
        return this._nicknameText;
      }
    }

    public Image BackImage
    {
      get
      {
        return this._backImage;
      }
    }

    public Transform RotateRoot
    {
      get
      {
        return this._rotateRoot;
      }
    }

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

    public string Nickname
    {
      get
      {
        return this._nicknameText.get_text();
      }
      set
      {
        this._nicknameText.set_text(value);
      }
    }

    public Color TextColor
    {
      get
      {
        return ((Graphic) this._nicknameText).get_color();
      }
      set
      {
        ((Graphic) this._nicknameText).set_color(value);
      }
    }

    public float BackImageAlpha
    {
      get
      {
        return (float) ((Graphic) this._backImage).get_color().a;
      }
      set
      {
        Color color = ((Graphic) this._backImage).get_color();
        color.a = (__Null) (double) value;
        ((Graphic) this._backImage).set_color(color);
      }
    }

    public Transform TargetObject { get; set; }

    public Camera TargetCamera { get; set; }

    public bool EnabledLateUpdate { get; private set; }

    public bool PlayingFade
    {
      get
      {
        return 0 < this._fadeDisposable.get_Count();
      }
    }

    public void PlayFadeIn(float fadeTime)
    {
      this._fadeDisposable.Clear();
      IEnumerator coroutine = this.FadeIn(fadeTime);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (ICollection<IDisposable>) this._fadeDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeIn(float fadeTime)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalNicknameUI.\u003CFadeIn\u003Ec__Iterator0()
      {
        fadeTime = fadeTime,
        \u0024this = this
      };
    }

    public void PlayFadeOut(float fadeTime)
    {
      this._fadeDisposable.Clear();
      IEnumerator coroutine = this.FadeOut(fadeTime);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (ICollection<IDisposable>) this._fadeDisposable);
    }

    [DebuggerHidden]
    private IEnumerator FadeOut(float fadeTime)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalNicknameUI.\u003CFadeOut\u003Ec__Iterator1()
      {
        fadeTime = fadeTime,
        \u0024this = this
      };
    }

    private void LateUpdate()
    {
      if (!this.EnabledLateUpdate || Object.op_Equality((Object) this.TargetCamera, (Object) null) || Object.op_Equality((Object) this.TargetObject, (Object) null))
        return;
      ((Component) this).get_transform().set_position(this.TargetCamera.WorldToScreenPoint(this.TargetObject.get_position()));
    }
  }
}
