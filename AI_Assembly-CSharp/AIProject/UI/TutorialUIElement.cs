// Decompiled with JetBrains decompiler
// Type: AIProject.UI.TutorialUIElement
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
  [RequireComponent(typeof (Image))]
  public class TutorialUIElement : UIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Image _backImage;
    [SerializeField]
    private Text _myPageText;
    [SerializeField]
    private Text _pageMaxText;
    private CompositeDisposable _allDisposable;
    private int _myPageNum;
    private int _maxPageNum;
    private IEnumerator _openEnumerator;
    private IEnumerator _closeEnumerator;

    public TutorialUIElement()
    {
      base.\u002Ector();
    }

    public Image BackImage
    {
      get
      {
        return this._backImage;
      }
    }

    public int MyPageNum
    {
      get
      {
        return this._myPageNum;
      }
      set
      {
        this._myPageNum = value;
        this._myPageText.set_text(string.Format("{0}", (object) this._myPageNum));
      }
    }

    public int MaxPageNum
    {
      get
      {
        return this._maxPageNum;
      }
      set
      {
        this._maxPageNum = value;
        this._pageMaxText.set_text(string.Format("{0}", (object) this._maxPageNum));
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

    protected virtual void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._rectTransform, (Object) null))
        this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      if (Object.op_Equality((Object) this._backImage, (Object) null))
        this._backImage = (Image) ((Component) this).GetComponent<Image>();
      this._canvasGroup.set_blocksRaycasts(false);
    }

    public void CloseForced()
    {
      this.Dispose();
      this.CanvasAlpha = 0.0f;
    }

    public bool IsOpening
    {
      get
      {
        return this._openEnumerator != null;
      }
    }

    public bool IsClosing
    {
      get
      {
        return this._closeEnumerator != null;
      }
    }

    public void Dispose()
    {
      this._allDisposable.Clear();
      this._openEnumerator = (IEnumerator) null;
      this._closeEnumerator = (IEnumerator) null;
    }

    public void Open(float fadeTime, float moveX)
    {
      if (this.IsOpening)
        return;
      IEnumerator enumerator;
      if (this.IsClosing)
      {
        this.Dispose();
        enumerator = this.ReOpenCoroutine(fadeTime);
      }
      else
        enumerator = this.OpenCoroutine(fadeTime, moveX);
      this._openEnumerator = enumerator;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._openEnumerator), false)), (ICollection<IDisposable>) this._allDisposable);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine(float fadeTime, float moveX)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUIElement.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        moveX = moveX,
        fadeTime = fadeTime,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator ReOpenCoroutine(float fadeTime)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUIElement.\u003CReOpenCoroutine\u003Ec__Iterator1()
      {
        fadeTime = fadeTime,
        \u0024this = this
      };
    }

    public void Close(float fadeTime, float moveX)
    {
      if (this.IsClosing)
        return;
      if (this.IsOpening)
        this.Dispose();
      this._closeEnumerator = this.CloseCoroutine(fadeTime, moveX);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._closeEnumerator), false)), (ICollection<IDisposable>) this._allDisposable);
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine(float fadeTime, float moveX)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUIElement.\u003CCloseCoroutine\u003Ec__Iterator2()
      {
        fadeTime = fadeTime,
        moveX = moveX,
        \u0024this = this
      };
    }

    private float GetPosX()
    {
      return (float) ((Transform) this._rectTransform).get_localPosition().x;
    }

    private void SetPosX(float posX)
    {
      Vector3 localPosition = ((Transform) this._rectTransform).get_localPosition();
      localPosition.x = (__Null) (double) posX;
      ((Transform) this._rectTransform).set_localPosition(localPosition);
    }

    private float GetPosY()
    {
      return (float) ((Transform) this._rectTransform).get_localPosition().y;
    }

    private void SetPosY(float posY)
    {
      Vector3 localPosition = ((Transform) this._rectTransform).get_localPosition();
      localPosition.y = (__Null) (double) posY;
      ((Transform) this._rectTransform).set_localPosition(localPosition);
    }
  }
}
