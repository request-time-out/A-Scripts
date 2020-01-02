// Decompiled with JetBrains decompiler
// Type: AIProject.ReachOffMeshLinkPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class ReachOffMeshLinkPoint : AgentAction
  {
    private bool _prevStopped = true;
    private AgentActor _agent;
    private NavMeshAgent _navMeshAgent;

    public virtual void OnStart()
    {
      this._agent = this.Agent;
      this._navMeshAgent = this._agent.NavMeshAgent;
      this._prevStopped = this._navMeshAgent.get_isStopped();
      if (Object.op_Inequality((Object) this._agent.TargetOffMeshLink, (Object) null) && this._prevStopped)
        this._navMeshAgent.set_isStopped(false);
      ((Task) this).OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      Transform startTransform = this._agent.TargetOffMeshLink?.get_startTransform();
      if (Object.op_Equality((Object) startTransform, (Object) null))
        return (TaskStatus) 1;
      if (this._agent.HasArrived())
        return (TaskStatus) 2;
      if (Object.op_Inequality((Object) startTransform, (Object) null))
        this._navMeshAgent.SetDestination(startTransform.get_position());
      return !this._navMeshAgent.get_pathPending() && !this._navMeshAgent.get_hasPath() ? (TaskStatus) 1 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      if (this._prevStopped != this._navMeshAgent.get_isStopped())
        this._navMeshAgent.set_isStopped(this._prevStopped);
      ((Task) this).OnEnd();
    }
  }
}
