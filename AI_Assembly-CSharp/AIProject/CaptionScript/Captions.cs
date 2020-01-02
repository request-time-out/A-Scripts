// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.Captions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using ReMotion;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.CaptionScript
{
  [RequireComponent(typeof (CaptionSystem))]
  [RequireComponent(typeof (CommandSystem))]
  public class Captions : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CaptionSystem _captionSystem;
    [SerializeField]
    private CommandSystem _commandSystem;
    private IDisposable _subscriber;

    public Captions()
    {
      base.\u002Ector();
    }

    public bool Active { get; private set; }

    public CaptionSystem CaptionSystem
    {
      get
      {
        return this._captionSystem;
      }
      set
      {
        this._captionSystem = value;
      }
    }

    public CommandSystem CommandSystem
    {
      get
      {
        return this._commandSystem;
      }
      set
      {
        this._commandSystem = value;
      }
    }

    public bool IsProcEndADV
    {
      get
      {
        return this._subscriber != null;
      }
    }

    private void Start()
    {
      Singleton<ADV>.Instance.Captions = this;
    }

    private void OnDestroy()
    {
      this.StopAllCoroutines();
      if (!Singleton<Manager.Voice>.IsInstance())
        return;
      Singleton<Manager.Voice>.Instance.StopAll(true);
    }

    public void EndADV(Action onCompleted = null)
    {
      this._canvasGroup.set_blocksRaycasts(false);
      if (this._subscriber != null)
        this._subscriber.Dispose();
      IObservable<TimeInterval<float>> observable = (IObservable<TimeInterval<float>>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.3f, true), true);
      float startAlpha = this._canvasGroup.get_alpha();
      if (onCompleted != null)
        this._subscriber = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.DoOnCompleted<TimeInterval<float>>((IObservable<M0>) observable, (Action) (() =>
        {
          this.Active = false;
          this._subscriber = (IDisposable) null;
        })), (Action<M0>) (x => this._canvasGroup.set_alpha(Mathf.Lerp(startAlpha, 0.0f, ((TimeInterval<float>) ref x).get_Value()))), (Action<Exception>) (ex => Debug.LogException(ex)), onCompleted);
      else
        this._subscriber = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.DoOnCompleted<TimeInterval<float>>((IObservable<M0>) observable, (Action) (() =>
        {
          this.Active = false;
          this._subscriber = (IDisposable) null;
        })), (Action<M0>) (x => this._canvasGroup.set_alpha(Mathf.Lerp(startAlpha, 0.0f, ((TimeInterval<float>) ref x).get_Value()))), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public void CanvasGroupON()
    {
      this.Active = true;
      this._canvasGroup.set_alpha(1f);
      this._canvasGroup.set_blocksRaycasts(true);
    }

    public void CanvasGroupOFF()
    {
      this.Active = false;
      this._canvasGroup.set_alpha(0.0f);
      this._canvasGroup.set_blocksRaycasts(false);
    }
  }
}
