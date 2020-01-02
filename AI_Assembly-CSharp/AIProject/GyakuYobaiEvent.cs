// Decompiled with JetBrains decompiler
// Type: AIProject.GyakuYobaiEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class GyakuYobaiEvent : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return (TaskStatus) 1;
      player.CurrentPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
      player.StartGyakuYobaiEvent(agent);
      return (TaskStatus) 2;
    }
  }
}
