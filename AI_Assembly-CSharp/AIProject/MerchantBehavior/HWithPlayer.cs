// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.HWithPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class HWithPlayer : MerchantAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Merchant.DeactivateNavMeshElement();
      this.Merchant.Partner = (Actor) Map.GetPlayer();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      PlayerActor player = Map.GetPlayer();
      if (Object.op_Inequality((Object) player, (Object) null) && Object.op_Equality((Object) this.Merchant.Partner, (Object) player))
        this.Merchant.Partner = (Actor) null;
      ((Task) this).OnEnd();
    }
  }
}
