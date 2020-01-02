// Decompiled with JetBrains decompiler
// Type: AIProject.TargetChangeMerchantBehaviorMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class TargetChangeMerchantBehaviorMode : AgentAction
  {
    [SerializeField]
    private Merchant.ActionType _mode = Merchant.ActionType.Idle;

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.Agent, (Object) null))
        return (TaskStatus) 1;
      MerchantActor targetInSightActor = this.Agent.TargetInSightActor as MerchantActor;
      if (Object.op_Equality((Object) targetInSightActor, (Object) null))
        return (TaskStatus) 1;
      targetInSightActor.ChangeBehavior(this._mode);
      return (TaskStatus) 2;
    }
  }
}
