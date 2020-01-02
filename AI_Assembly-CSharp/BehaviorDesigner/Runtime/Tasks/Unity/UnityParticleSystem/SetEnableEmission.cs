// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem.SetEnableEmission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
  [TaskCategory("Unity/ParticleSystem")]
  [TaskDescription("Enables or disables the Particle System emission.")]
  public class SetEnableEmission : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Enable the ParticleSystem emissions?")]
    public SharedBool enable;
    private ParticleSystem particleSystem;
    private GameObject prevGameObject;

    public SetEnableEmission()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.particleSystem = (ParticleSystem) defaultGameObject.GetComponent<ParticleSystem>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.particleSystem, (Object) null))
      {
        Debug.LogWarning((object) "ParticleSystem is null");
        return (TaskStatus) 1;
      }
      ParticleSystem.EmissionModule emission = this.particleSystem.get_emission();
      ((ParticleSystem.EmissionModule) ref emission).set_enabled(this.enable.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.enable = (SharedBool) false;
    }
  }
}
