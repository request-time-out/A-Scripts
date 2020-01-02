// Decompiled with JetBrains decompiler
// Type: AIProject.DeactivatePairing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class DeactivatePairing : AgentAction
  {
    [SerializeField]
    private bool _changeToPartner;

    public virtual TaskStatus OnUpdate()
    {
      Actor partner = this.Agent.Partner;
      this.Agent.Partner = (Actor) null;
      if (Object.op_Inequality((Object) partner, (Object) null))
        partner.Partner = (Actor) null;
      if (this._changeToPartner)
      {
        switch (partner)
        {
          case AgentActor _:
            (partner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
            break;
          case MerchantActor _:
            MerchantActor merchantActor = partner as MerchantActor;
            merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
            break;
        }
      }
      this.Agent.TargetInSightActor = (Actor) null;
      return (TaskStatus) 2;
    }
  }
}
