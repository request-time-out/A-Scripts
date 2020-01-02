// Decompiled with JetBrains decompiler
// Type: AIProject.IsOnOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class IsOnOffMeshLink : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      NavMeshAgent navMeshAgent = this.Agent.NavMeshAgent;
      if (navMeshAgent.get_isOnOffMeshLink())
      {
        OffMeshLinkData currentOffMeshLinkData = navMeshAgent.get_currentOffMeshLinkData();
        if (Object.op_Inequality((Object) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink(), (Object) null))
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
