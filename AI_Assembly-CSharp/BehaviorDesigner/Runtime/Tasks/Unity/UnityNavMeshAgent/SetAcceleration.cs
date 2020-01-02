// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent.SetAcceleration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
  [TaskCategory("Unity/NavMeshAgent")]
  [TaskDescription("Sets the maximum acceleration of an agent as it follows a path, given in units / sec^2. Returns Success.")]
  public class SetAcceleration : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The NavMeshAgent acceleration")]
    public SharedFloat acceleration;
    private NavMeshAgent navMeshAgent;
    private GameObject prevGameObject;

    public SetAcceleration()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.navMeshAgent = (NavMeshAgent) defaultGameObject.GetComponent<NavMeshAgent>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.navMeshAgent, (Object) null))
      {
        Debug.LogWarning((object) "NavMeshAgent is null");
        return (TaskStatus) 1;
      }
      this.navMeshAgent.set_acceleration(this.acceleration.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.acceleration = (SharedFloat) 0.0f;
    }
  }
}
