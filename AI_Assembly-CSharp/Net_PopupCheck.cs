// Decompiled with JetBrains decompiler
// Type: Net_PopupCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Net_PopupCheck : MonoBehaviour
{
  [SerializeField]
  private Canvas canvas;
  [SerializeField]
  private Button btnYes;
  [SerializeField]
  private Button btnNo;
  [SerializeField]
  private Text textMsg;
  private bool? answer;

  public Net_PopupCheck()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  public IEnumerator CheckAnswerCor(IObserver<bool> observer, string msg)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Net_PopupCheck.\u003CCheckAnswerCor\u003Ec__Iterator0()
    {
      msg = msg,
      observer = observer,
      \u0024this = this
    };
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) this.btnYes))
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnYes), (Action<M0>) (_ =>
      {
        this.answer = new bool?(true);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      })), (Component) this);
    if (!Object.op_Implicit((Object) this.btnNo))
      return;
    DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNo), (Action<M0>) (_ =>
    {
      this.answer = new bool?(false);
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
    })), (Component) this);
  }

  private void Update()
  {
    if (!Input.GetMouseButtonDown(1))
      return;
    Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
    this.answer = new bool?(false);
  }
}
