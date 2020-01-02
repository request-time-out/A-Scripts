// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.FlyingPetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class FlyingPetAnimal : MovingPetAnimal
  {
    [SerializeField]
    private float _standbyHeight = 3f;
    [SerializeField]
    private float _standbyRadius = 2f;
    [SerializeField]
    private float _movingHeight = 7f;
    [SerializeField]
    private float _heightMoveSpeed = 1f;
    private PlayerActor _player;
    private IDisposable _moveHeightDisposable;

    protected override void Initialize()
    {
      if (Object.op_Inequality((Object) this.bodyObject, (Object) null))
        this.bodyObject.get_transform().set_localPosition(new Vector3(0.0f, !this.ChaseActor ? this._standbyHeight : this._movingHeight, 0.0f));
      this.StartMoveHeightOffset();
    }

    private void StartMoveHeightOffset()
    {
      IEnumerator coroutine = this.MoveHeightOffsetCoroutine();
      if (this._moveHeightDisposable != null)
        this._moveHeightDisposable.Dispose();
      this._moveHeightDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this));
    }

    [DebuggerHidden]
    private IEnumerator MoveHeightOffsetCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FlyingPetAnimal.\u003CMoveHeightOffsetCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void StopMoveHeightOffset()
    {
      if (this._moveHeightDisposable == null)
        return;
      this._moveHeightDisposable.Dispose();
      this._moveHeightDisposable = (IDisposable) null;
    }

    protected override void AnimationLovelyFollow()
    {
      Vector3 velocity = this.Agent.get_velocity();
      float num1 = !Mathf.Approximately(((Vector3) ref velocity).get_magnitude(), 0.0f) ? 1f : 0.0f;
      if (!Mathf.Approximately(num1, this._animParam))
      {
        float num2 = Mathf.Sign(num1 - this._animParam);
        float num3 = this._animParam + this._animLerpValue * Time.get_deltaTime() * num2;
        if (0.0 < (double) num2)
        {
          if ((double) num1 < (double) num3)
            num3 = num1;
        }
        else if ((double) num3 < (double) num1)
          num3 = num1;
        this._animParam = num3;
      }
      this.SetFloat(this._locomotionParamName, this._animParam);
    }
  }
}
