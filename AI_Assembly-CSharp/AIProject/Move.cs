// Decompiled with JetBrains decompiler
// Type: AIProject.Move
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class Move : AgentStateAction
  {
    private EventType _prevEventKey;
    private ActionPoint _prevTargetPoint;

    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      this._prevEventKey = agent.EventKey;
      agent.EventKey = EventType.Move;
      OffMeshLinkData currentOffMeshLinkData = this.Agent.NavMeshAgent.get_currentOffMeshLinkData();
      ActionPoint component = (ActionPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<ActionPoint>();
      this._prevTargetPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = component;
      base.OnStart();
    }

    public override void OnEnd()
    {
      base.OnEnd();
      AgentActor agent = this.Agent;
      if (!Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
        return;
      agent.CurrentPoint.RemoveBooking((Actor) agent);
      agent.CurrentPoint.SetActiveMapItemObjs(true);
      agent.CurrentPoint.CreateByproduct(agent.ActionID, agent.PoseID);
      agent.CurrentPoint.ReleaseSlot((Actor) agent);
      agent.CurrentPoint = (ActionPoint) null;
      agent.Animation.StopAllAnimCoroutine();
      agent.ActivateTransfer(false);
    }

    protected override void Complete()
    {
      base.Complete();
      this.Agent.EventKey = this._prevEventKey;
      this.Agent.TargetInSightActionPoint = this._prevTargetPoint;
    }
  }
}
