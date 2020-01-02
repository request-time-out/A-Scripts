// Decompiled with JetBrains decompiler
// Type: AIProject.IsReachOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class IsReachOffMeshLink : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      OffMeshLink targetOffMeshLink = agent.TargetOffMeshLink;
      if (Object.op_Equality((Object) targetOffMeshLink, (Object) null))
        return (TaskStatus) 1;
      return agent.HasArrivedOffMeshLink(targetOffMeshLink) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
