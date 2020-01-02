// Decompiled with JetBrains decompiler
// Type: Net_PopupMsg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Net_PopupMsg : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup cgrp;
  [SerializeField]
  private Text txt;
  private int endMode;
  private float looptime;
  private bool exitCommand;
  private CompositeDisposable disposables;

  public Net_PopupMsg()
  {
    base.\u002Ector();
  }

  public bool active { get; private set; }

  public void StartMessage(float st, float lt, float et, string msg, int mode)
  {
    if (Object.op_Equality((Object) null, (Object) this.cgrp))
      return;
    this.endMode = mode;
    this.looptime = lt;
    this.exitCommand = false;
    IObservable<float> observable1 = (IObservable<float>) Observable.Scan<float>((IObservable<M0>) Observable.Select<Unit, float>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, M1>) (_ => Time.get_deltaTime())), (Func<M0, M0, M0>) ((acc, current) => acc + current));
    IObservable<float> observable2 = (IObservable<float>) Observable.TakeWhile<float>((IObservable<M0>) observable1, (Func<M0, bool>) (t => (double) t < (double) st));
    IObservable<float> loopStream = (IObservable<float>) Observable.TakeWhile<float>((IObservable<M0>) observable1, (Func<M0, bool>) (t => !this.CheckEnd(t)));
    IObservable<float> endStream = (IObservable<float>) Observable.TakeWhile<float>((IObservable<M0>) observable1, (Func<M0, bool>) (t => (double) t < (double) et));
    this.disposables.Clear();
    if (Object.op_Implicit((Object) this.txt))
      this.txt.set_text(msg);
    this.cgrp.set_blocksRaycasts(true);
    this.active = true;
    DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) observable2, (Action<M0>) (t => this.cgrp.set_alpha(Mathf.Lerp(0.0f, 1f, Mathf.InverseLerp(0.0f, st, t)))), (Action) (() =>
    {
      this.cgrp.set_alpha(1f);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) loopStream, (Action<M0>) (t => {}), (Action) (() => DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) endStream, (Action<M0>) (t => this.cgrp.set_alpha(Mathf.Lerp(1f, 0.0f, Mathf.InverseLerp(0.0f, et, t)))), (Action) (() =>
      {
        this.cgrp.set_alpha(0.0f);
        this.cgrp.set_blocksRaycasts(false);
        this.active = false;
      })), (ICollection<IDisposable>) this.disposables))), (ICollection<IDisposable>) this.disposables);
    })), (ICollection<IDisposable>) this.disposables);
  }

  public void EndMessage()
  {
    this.exitCommand = true;
  }

  public bool CheckEnd(float time)
  {
    switch (this.endMode)
    {
      case 0:
        return (double) time >= (double) this.looptime || Input.get_anyKeyDown();
      case 1:
        return Input.get_anyKeyDown();
      case 2:
        return this.exitCommand;
      default:
        return false;
    }
  }

  private void Start()
  {
    this.cgrp.set_alpha(0.0f);
    this.cgrp.set_blocksRaycasts(false);
    this.active = false;
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) this), (Action<M0>) (_ =>
    {
      this.disposables.Clear();
      this.cgrp.set_alpha(0.0f);
      this.cgrp.set_blocksRaycasts(false);
      this.active = false;
    }));
  }
}
