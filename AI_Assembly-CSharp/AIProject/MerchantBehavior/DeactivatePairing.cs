// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.DeactivatePairing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class DeactivatePairing : MerchantAction
  {
    [SerializeField]
    private bool _changeToPartner;

    public virtual TaskStatus OnUpdate()
    {
      Actor partner = this.Merchant.Partner;
      this.Merchant.Partner = (Actor) null;
      if (Object.op_Inequality((Object) partner, (Object) null) && Object.op_Equality((Object) partner.Partner, (Object) this.Merchant))
        partner.Partner = (Actor) null;
      if (this._changeToPartner && partner is AgentActor)
        (partner as AgentActor).BehaviorResources.ChangeMode(Desire.ActionType.Normal);
      return (TaskStatus) 2;
    }
  }
}
