// Decompiled with JetBrains decompiler
// Type: AIProject.AddPointBooking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class AddPointBooking : AgentAction
  {
    private ActionPoint _bookingPoint;
    private AgentActor _agent;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
    }

    public virtual TaskStatus OnUpdate()
    {
      NavMeshAgent navMeshAgent = this._agent.NavMeshAgent;
      M0 m0;
      if (navMeshAgent == null)
      {
        m0 = (M0) null;
      }
      else
      {
        OffMeshLinkData currentOffMeshLinkData = navMeshAgent.get_currentOffMeshLinkData();
        m0 = ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink())?.GetComponent<ActionPoint>();
      }
      this._bookingPoint = (ActionPoint) m0;
      if (Object.op_Equality((Object) this._bookingPoint, (Object) null))
        return (TaskStatus) 1;
      this._bookingPoint.AddBooking((Actor) this._agent);
      this._agent.BookingActionPoint = this._bookingPoint;
      return (TaskStatus) 2;
    }

    public virtual void OnBehaviorComplete()
    {
      if (Object.op_Equality((Object) this._bookingPoint, (Object) null))
        return;
      this._bookingPoint.RemoveBooking((Actor) this._agent);
      if (Object.op_Equality((Object) this.Agent.BookingActionPoint, (Object) this._bookingPoint))
        this.Agent.BookingActionPoint = (ActionPoint) null;
      this._bookingPoint = (ActionPoint) null;
    }
  }
}
