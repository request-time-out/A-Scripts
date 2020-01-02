// Decompiled with JetBrains decompiler
// Type: AIProject.TargetChangeMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class TargetChangeMode : AgentAction
  {
    [SerializeField]
    private Desire.ActionType _mode;

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Equality((Object) agent.TargetInSightActor, (Object) null))
        return (TaskStatus) 1;
      if (!(agent.TargetInSightActor is AgentActor))
        return (TaskStatus) 1;
      (agent.TargetInSightActor as AgentActor).ChangeBehavior(this._mode);
      return (TaskStatus) 2;
    }
  }
}
