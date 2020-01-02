// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.IsOnOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class IsOnOffMeshLink : MerchantConditional
  {
    private NavMeshAgent _agent;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Merchant.NavMeshAgent;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._agent.get_isOnOffMeshLink())
      {
        OffMeshLinkData currentOffMeshLinkData = this._agent.get_currentOffMeshLinkData();
        if (Object.op_Inequality((Object) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink(), (Object) null))
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
