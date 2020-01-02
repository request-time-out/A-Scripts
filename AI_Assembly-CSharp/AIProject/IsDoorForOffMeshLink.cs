// Decompiled with JetBrains decompiler
// Type: AIProject.IsDoorForOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class IsDoorForOffMeshLink : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      OffMeshLinkData currentOffMeshLinkData1 = this.Agent.NavMeshAgent.get_currentOffMeshLinkData();
      if (Object.op_Inequality((Object) ((OffMeshLinkData) ref currentOffMeshLinkData1).get_offMeshLink(), (Object) null))
      {
        OffMeshLinkData currentOffMeshLinkData2 = this.Agent.NavMeshAgent.get_currentOffMeshLinkData();
        if (Object.op_Inequality((Object) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData2).get_offMeshLink()).GetComponent<DoorPoint>(), (Object) null))
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
