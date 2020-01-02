// Decompiled with JetBrains decompiler
// Type: AIProject.IdleWhileFreeLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class IdleWhileFreeLink : AgentAction
  {
    private AgentActor _agent;
    private bool _prevStopped;

    public virtual void OnStart()
    {
      this._agent = this.Agent;
      this._prevStopped = this._agent.NavMeshAgent.get_isStopped();
      if (this._prevStopped)
        return;
      this._agent.NavMeshAgent.set_isStopped(true);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this._agent.IsInvalidMoveDestination(this._agent.TargetOffMeshLink) ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      if (this._agent.NavMeshAgent.get_isStopped() != this._prevStopped)
        this._agent.NavMeshAgent.set_isStopped(this._prevStopped);
      this.Agent.TargetOffMeshLink = (OffMeshLink) null;
      ((Task) this).OnEnd();
    }
  }
}
