// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.MatchPartnerMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class MatchPartnerMode : MerchantConditional
  {
    [SerializeField]
    private Desire.ActionType _mode;

    public virtual TaskStatus OnUpdate()
    {
      Actor partner = this.Merchant.Partner;
      return Object.op_Inequality((Object) partner, (Object) null) && partner.Mode == this._mode ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
