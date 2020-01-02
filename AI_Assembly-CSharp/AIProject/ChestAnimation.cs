// Decompiled with JetBrains decompiler
// Type: AIProject.ChestAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (ActionPoint))]
  public class ChestAnimation : ActionPointAnimation
  {
    private Queue<PlayState.Info> _inQueue = new Queue<PlayState.Info>();
    private Queue<PlayState.Info> _outQueue = new Queue<PlayState.Info>();
    private IEnumerator _itemInAnimEnumerator;
    private IDisposable _itemInAnimDisposable;
    private IEnumerator _itemOutAnimEnumerator;
    private IDisposable _itemOutAnimDisposable;

    protected override void OnStart()
    {
      this._animator = ChestAnimData.Table.get_Item(this._id);
      if (!Object.op_Inequality((Object) this._animator, (Object) null))
        return;
      this._animator.set_runtimeAnimatorController(Singleton<Resources>.Instance.Animation.GetItemAnimator(Singleton<Resources>.Instance.CommonDefine.ItemAnims.ChestAnimatorID));
      this._animator.Play(Singleton<Resources>.Instance.CommonDefine.ItemAnims.ChestDefaultState, 0, 0.0f);
    }

    public void PlayInAnimation()
    {
      this._inQueue.Clear();
      foreach (string chestInState in Singleton<Resources>.Instance.CommonDefine.ItemAnims.ChestInStates)
        this._inQueue.Enqueue(new PlayState.Info(chestInState, 0));
      this._itemInAnimEnumerator = this.StartInAnimation();
      this._itemInAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._itemInAnimEnumerator), false));
    }

    public void StopInAnimation()
    {
      if (this._itemInAnimDisposable == null)
        return;
      this._itemInAnimDisposable.Dispose();
      this._itemInAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartInAnimation()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChestAnimation.\u003CStartInAnimation\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void PlayOutAnimation()
    {
      this._outQueue.Clear();
      foreach (string chestOutState in Singleton<Resources>.Instance.CommonDefine.ItemAnims.ChestOutStates)
        this._outQueue.Enqueue(new PlayState.Info(chestOutState, 0));
      this._itemOutAnimEnumerator = this.StartOutAnimation();
      this._itemOutAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._itemOutAnimEnumerator), false));
    }

    public void StopOutAnimation()
    {
      if (this._itemOutAnimDisposable == null)
        return;
      this._itemOutAnimDisposable.Dispose();
      this._itemOutAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartOutAnimation()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChestAnimation.\u003CStartOutAnimation\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public bool PlayingInAniamtion
    {
      get
      {
        return this._itemInAnimEnumerator != null;
      }
    }

    public bool PlayingOutAnimation
    {
      get
      {
        return this._itemOutAnimEnumerator != null;
      }
    }
  }
}
