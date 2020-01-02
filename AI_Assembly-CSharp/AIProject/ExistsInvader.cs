// Decompiled with JetBrains decompiler
// Type: AIProject.ExistsInvader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ExistsInvader : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      CollisionState collisionState;
      return (double) Vector3.Distance(Singleton<Manager.Map>.Instance.Player.Position, this.Agent.Position) < (double) Singleton<Resources>.Instance.LocomotionProfile.AccessInvasionRange && agent.ActorCollisionStateTable.TryGetValue(player.InstanceID, out collisionState) && (collisionState == CollisionState.Enter || collisionState == CollisionState.Stay) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
