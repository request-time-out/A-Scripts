// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.CanLesbianWithPartner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class CanLesbianWithPartner : MerchantConditional
  {
    private MerchantActor _merchant;

    public virtual void OnAwake()
    {
      ((Task) this).OnAwake();
      this._merchant = this.Merchant;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._merchant, (Object) null))
        return (TaskStatus) 1;
      AgentActor partner = this._merchant.Partner as AgentActor;
      bool flag = Object.op_Inequality((Object) partner, (Object) null);
      if (flag)
        flag = Object.op_Equality((Object) partner.Partner, (Object) this._merchant);
      if (flag)
      {
        Desire.ActionType mode = partner.Mode;
        switch (mode)
        {
          case Desire.ActionType.GotoLesbianSpot:
          case Desire.ActionType.EndTaskLesbianH:
            flag = true;
            break;
          default:
            if (mode != Desire.ActionType.EndTaskLesbianMerchantH)
            {
              flag = false;
              break;
            }
            goto case Desire.ActionType.GotoLesbianSpot;
        }
      }
      return flag ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
