﻿// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.IsDoorForOffMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class IsDoorForOffMeshLink : MerchantConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      OffMeshLinkData currentOffMeshLinkData = this.Merchant.NavMeshAgent.get_currentOffMeshLinkData();
      return Object.op_Inequality((Object) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<DoorPoint>(), (Object) null) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
