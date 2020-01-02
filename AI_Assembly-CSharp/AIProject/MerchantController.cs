// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (ActorLocomotion))]
  public class MerchantController : ActorController
  {
    [SerializeField]
    private ActorLocomotionMerchant _character;

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this._character, (Object) null))
        return;
      ((Behaviour) this._character).set_enabled(false);
    }

    public override void StartBehavior()
    {
      if (!Object.op_Equality((Object) this._character, (Object) null))
        return;
      this._character = (ActorLocomotionMerchant) ((Component) this).GetComponent<ActorLocomotionMerchant>();
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this._actor, (Object) null) || !this._actor.IsInit)
        return;
      if (this._character != null)
        this._character.Move(Vector3.get_zero());
      if (!Singleton<Scene>.Instance.IsNowLoadingFade)
        ;
    }

    protected override void SubFixedUpdate()
    {
      if (this._character == null)
        return;
      this._character.UpdateState();
    }

    public override void ChangeState(string stateName)
    {
    }
  }
}
