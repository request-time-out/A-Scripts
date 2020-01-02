// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.Move
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class Move : MerchantMoveAction
  {
    public override void OnStart()
    {
      this.Merchant.EventKey = EventType.Move;
      OffMeshLinkData currentOffMeshLinkData = this.Merchant.NavMeshAgent.get_currentOffMeshLinkData();
      this.CurrentPoint = (ActionPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<ActionPoint>();
      base.OnStart();
      this.Merchant.NavMeshObstacle.set_carveOnlyStationary(false);
    }

    public override TaskStatus OnUpdate()
    {
      this.Merchant.ObstaclePosition = this.Merchant.Position;
      this.Merchant.ObstacleRotation = this.Merchant.Rotation;
      return base.OnUpdate();
    }

    public override void OnEnd()
    {
      MerchantActor merchant = this.Merchant;
      ActorAnimation animation = merchant.Animation;
      animation.Targets.Clear();
      animation.Animator.InterruptMatchTarget(false);
      merchant.NavMeshObstacle.set_carveOnlyStationary(true);
      base.OnEnd();
    }
  }
}
