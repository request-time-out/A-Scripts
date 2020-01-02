// Decompiled with JetBrains decompiler
// Type: AIProject.RecoverPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class RecoverPoint : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.ActivateNavMeshAgent();
      agent.Animation.EndStates();
      agent.SetActiveOnEquipedItem(true);
      return (TaskStatus) 2;
    }
  }
}
