// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ButterflyBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.Animal
{
  public abstract class ButterflyBase : AnimalBase
  {
    protected ParticleSystem particle;

    public bool HasParticle
    {
      get
      {
        return Object.op_Inequality((Object) this.particle, (Object) null);
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return false;
      }
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      this.SetDestroyState();
      base.OnDestroy();
    }

    public override void Clear()
    {
      this.particle = (ParticleSystem) null;
      base.Clear();
    }

    public override void ReleaseBody()
    {
      this.particle = (ParticleSystem) null;
      base.ReleaseBody();
    }

    public override void CreateBody()
    {
      base.CreateBody();
      if (!Object.op_Implicit((Object) this.bodyObject))
        return;
      this.particle = (ParticleSystem) this.bodyObject.GetComponentInChildren<ParticleSystem>(true);
    }

    public void PlayParticle()
    {
      if (!Object.op_Inequality((Object) this.particle, (Object) null))
        return;
      this.particle.Play(true);
    }

    public void StopParticle()
    {
      if (!Object.op_Inequality((Object) this.particle, (Object) null))
        return;
      this.particle.Stop(true, (ParticleSystemStopBehavior) 1);
    }

    public override void SetState(AnimalState _nextState, Action _changeEvent = null)
    {
      this.CurrentState = _nextState;
      if (_changeEvent == null)
        return;
      _changeEvent();
    }

    public override void ChangeState(AnimalState _nextState, Action _changeEvent = null)
    {
      this.CurrentState = _nextState;
      if (_changeEvent == null)
        return;
      _changeEvent();
    }
  }
}
