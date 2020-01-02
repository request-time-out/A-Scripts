// Decompiled with JetBrains decompiler
// Type: AIProject.FadeCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (CanvasGroup))]
  public class FadeCanvas : SerializedMonoBehaviour
  {
    private static readonly FadeCanvas.PanelType[] _typeKeys = Enum.GetValues(typeof (FadeCanvas.PanelType)).Cast<FadeCanvas.PanelType>().ToArray<FadeCanvas.PanelType>();
    [SerializeField]
    private Dictionary<FadeCanvas.PanelType, FadeItem> _table;
    private FadeType _fadeType;
    private FadeItem _currentPanel;
    private IObservable<Unit> _stream;
    private IDisposable _disposable;

    public FadeCanvas()
    {
      base.\u002Ector();
    }

    public Dictionary<FadeCanvas.PanelType, FadeItem> Table
    {
      get
      {
        return this._table;
      }
    }

    public bool IsFading
    {
      get
      {
        return this._stream != null;
      }
    }

    public bool IsFadeIn
    {
      get
      {
        return this.IsFading && this._fadeType == 0;
      }
    }

    public bool IsFadeOut
    {
      get
      {
        return this.IsFading && this._fadeType == 1;
      }
    }

    public FadeItem GetPanel(FadeCanvas.PanelType type)
    {
      FadeItem fadeItem1 = (FadeItem) null;
      foreach (FadeCanvas.PanelType typeKey in FadeCanvas._typeKeys)
      {
        FadeItem fadeItem2;
        if (this._table.TryGetValue(typeKey, out fadeItem2))
        {
          ((Component) fadeItem2.Graphic).get_gameObject().SetActive(typeKey == type);
          if (((Component) fadeItem2.Graphic).get_gameObject().get_activeSelf())
            fadeItem1 = fadeItem2;
        }
      }
      return fadeItem1;
    }

    public IObservable<Unit> StartFade(
      FadeCanvas.PanelType type,
      FadeType fade,
      float duration,
      bool ignoreTimeScale)
    {
      if (this.IsFading)
        return (IObservable<Unit>) null;
      this._fadeType = fade;
      FadeItem fadeItem1 = (FadeItem) null;
      foreach (FadeCanvas.PanelType typeKey in FadeCanvas._typeKeys)
      {
        FadeItem fadeItem2;
        if (this._table.TryGetValue(typeKey, out fadeItem2))
        {
          ((Component) fadeItem2.Graphic).get_gameObject().SetActive(typeKey == type);
          if (((Component) fadeItem2.Graphic).get_gameObject().get_activeSelf())
            fadeItem1 = fadeItem2;
        }
      }
      if (Object.op_Equality((Object) fadeItem1, (Object) null))
        return (IObservable<Unit>) null;
      this._currentPanel = fadeItem1;
      IConnectableObservable<Unit> iconnectableObservable = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.FadeCoroutine(duration, ignoreTimeScale)), false));
      this._disposable = iconnectableObservable.Connect();
      return this._stream = (IObservable<Unit>) iconnectableObservable;
    }

    [DebuggerHidden]
    private IEnumerator FadeCoroutine(float duration, bool ignoreTimeScale)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FadeCanvas.\u003CFadeCoroutine\u003Ec__Iterator0()
      {
        duration = duration,
        ignoreTimeScale = ignoreTimeScale,
        \u0024this = this
      };
    }

    public void SkipFade()
    {
      if (this._disposable != null)
        this._disposable.Dispose();
      this._disposable = (IDisposable) null;
      this._stream = (IObservable<Unit>) null;
      if (this._fadeType == null)
      {
        if (Object.op_Inequality((Object) this._currentPanel, (Object) null))
          this._currentPanel.CanvasGroup.set_alpha(1f);
      }
      else if (Object.op_Inequality((Object) this._currentPanel, (Object) null))
        this._currentPanel.CanvasGroup.set_alpha(0.0f);
      this.Complete();
    }

    private void ChangeFade(float value)
    {
      if (this._fadeType == null)
        this._currentPanel.CanvasGroup.set_alpha(Mathf.Lerp(0.0f, 1f, value));
      else
        this._currentPanel.CanvasGroup.set_alpha(Mathf.Lerp(1f, 0.0f, value));
    }

    private void Complete()
    {
      if (this._fadeType == 1 && Object.op_Inequality((Object) this._currentPanel, (Object) null))
        ((Component) this._currentPanel.Graphic).get_gameObject().SetActive(false);
      this._stream = (IObservable<Unit>) null;
    }

    public enum PanelType
    {
      NowLoading,
      Blackout,
      Transition,
    }
  }
}
