// Decompiled with JetBrains decompiler
// Type: AIProject.GoToNextOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class GoToNextOffMeshLink : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      NavMeshAgent navMeshAgent = this.Agent.NavMeshAgent;
      OffMeshLinkData nextOffMeshLinkData = navMeshAgent.get_nextOffMeshLinkData();
      OffMeshLink offMeshLink = ((OffMeshLinkData) ref nextOffMeshLinkData).get_offMeshLink();
      if (!Object.op_Inequality((Object) offMeshLink, (Object) null))
        return (TaskStatus) 1;
      if (Object.op_Inequality((Object) offMeshLink.get_startTransform(), (Object) null))
      {
        navMeshAgent.SetDestination(offMeshLink.get_startTransform().get_position());
        return (TaskStatus) 2;
      }
      Debug.LogError((object) "オフメッシュリンクにstartTransformが設定されてない", (Object) ((Component) offMeshLink).get_gameObject());
      return (TaskStatus) 1;
    }
  }
}
