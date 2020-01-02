// Decompiled with JetBrains decompiler
// Type: AIProject.IsStandbyMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class IsStandbyMove : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      OffMeshLink nearOffMeshLink = this.Agent.NearOffMeshLink;
      if (!this.Agent.IsInvalidMoveDestination(nearOffMeshLink))
        return (TaskStatus) 1;
      this.Agent.TargetOffMeshLink = nearOffMeshLink;
      return (TaskStatus) 2;
    }

    public virtual void OnBehaviorComplete()
    {
      this.Agent.TargetOffMeshLink = (OffMeshLink) null;
      ((Task) this).OnBehaviorComplete();
    }
  }
}
