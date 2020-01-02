// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.HWithAgent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class HWithAgent : MerchantAction
  {
    private AgentActor _agent;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Merchant.DeactivateNavMeshElement();
      this._agent = this.Merchant.Partner as AgentActor;
    }

    public virtual TaskStatus OnUpdate()
    {
      return Object.op_Inequality((Object) this._agent, (Object) null) ? (TaskStatus) 3 : (TaskStatus) 1;
    }

    public virtual void OnBehaviorComplete()
    {
      AgentActor partner = this.Merchant.Partner as AgentActor;
      if (Object.op_Inequality((Object) partner, (Object) null) && Object.op_Equality((Object) partner, (Object) this._agent))
      {
        switch (partner.Mode)
        {
          case Desire.ActionType.EndTaskLesbianMerchantH:
          case Desire.ActionType.EndTaskLesbianH:
            Debug.Log((object) "Merchant.HWithAgent.OnBehaviorComplete() パートナー解放せず");
            break;
          default:
            Debug.Log((object) "Merchant.HWithAgent.OnBehaviorComplete() パートナー解放する");
            this.Merchant.Partner = (Actor) null;
            break;
        }
      }
      ((Task) this).OnBehaviorComplete();
    }
  }
}
