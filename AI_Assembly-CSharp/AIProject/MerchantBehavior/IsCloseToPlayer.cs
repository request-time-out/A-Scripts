// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.IsCloseToPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class IsCloseToPlayer : MerchantConditional
  {
    private PlayerActor Player
    {
      get
      {
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.Player : (PlayerActor) null;
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      PlayerActor player = this.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return (TaskStatus) 1;
      return (double) Vector3.Distance(player.Position, this.Merchant.Position) <= (double) Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
