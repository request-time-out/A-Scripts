// Decompiled with JetBrains decompiler
// Type: AIProject.Scene.Title
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace AIProject.Scene
{
  public class Title : BaseLoader
  {
    private void Start()
    {
      IConnectableObservable<Unit> iconnectableObservable = (IConnectableObservable<Unit>) Observable.Publish<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.InputWait()), false));
      iconnectableObservable.Connect();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.WhenAll(new IObservable<Unit>[1]
      {
        (IObservable<Unit>) iconnectableObservable
      }), (Action<M0>) (_ => Singleton<Manager.Scene>.Instance.LoadReserve(new Manager.Scene.Data()
      {
        levelName = Singleton<Resources>.Instance.DefinePack.SceneNames.MapScene,
        isAdd = false,
        isFade = true,
        isAsync = true
      }, false)));
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
    }

    [DebuggerHidden]
    private IEnumerator InputWait()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Title.\u003CInputWait\u003Ec__Iterator0 inputWaitCIterator0 = new Title.\u003CInputWait\u003Ec__Iterator0();
      return (IEnumerator) inputWaitCIterator0;
    }
  }
}
