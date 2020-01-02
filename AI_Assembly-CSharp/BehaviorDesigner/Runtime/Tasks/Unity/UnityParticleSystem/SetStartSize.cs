// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem.SetStartSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
  [TaskCategory("Unity/ParticleSystem")]
  [TaskDescription("Sets the start size of the Particle System.")]
  public class SetStartSize : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The start size of the ParticleSystem")]
    public SharedFloat startSize;
    private ParticleSystem particleSystem;
    private GameObject prevGameObject;

    public SetStartSize()
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
      ParticleSystem.MainModule main = this.particleSystem.get_main();
      ((ParticleSystem.MainModule) ref main).set_startSize(ParticleSystem.MinMaxCurve.op_Implicit(this.startSize.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.startSize = (SharedFloat) 0.0f;
    }
  }
}
