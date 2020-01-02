// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.TalkWithAgent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class TalkWithAgent : MerchantAction
  {
    private AgentActor agent;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Merchant.ActivateNavMeshObstacle(this.Merchant.Position);
      this.agent = this.Merchant.CommandPartner as AgentActor;
      this.Merchant.CrossFade();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.agent, (Object) null))
        return (TaskStatus) 1;
      return this.agent.LivesTalkSequence ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      if (Object.op_Inequality((Object) this.agent, (Object) null) && Object.op_Equality((Object) this.Merchant.CommandPartner, (Object) this.agent))
        this.Merchant.CommandPartner = (Actor) null;
      ((Task) this).OnEnd();
    }
  }
}
