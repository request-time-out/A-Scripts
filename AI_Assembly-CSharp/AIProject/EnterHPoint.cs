// Decompiled with JetBrains decompiler
// Type: AIProject.EnterHPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterHPoint : AgentAction
  {
    private int _stopCount;
    private NavMeshPath _path;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._path = new NavMeshPath();
      AgentActor agent = this.Agent;
      agent.ActivateTransfer(true);
      this.SetDestinationForce(agent.DestPosition.Value);
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (!agent.DestPosition.HasValue)
        return (TaskStatus) 1;
      if (agent.DestPosition.HasValue)
        this.SetDestination(agent.DestPosition.Value);
      agent.NavMeshAgent.CalculatePath(agent.DestPosition.Value, this._path);
      if (this._path.get_status() != null)
      {
        agent.DestPosition = new Vector3?();
        return (TaskStatus) 1;
      }
      if ((double) Vector3.Distance(agent.DestPosition.Value, agent.Position) <= (double) Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPoint)
        return (TaskStatus) 2;
      Vector3 desiredVelocity = agent.NavMeshAgent.get_desiredVelocity();
      if (Mathf.Approximately(((Vector3) ref desiredVelocity).get_magnitude(), 0.0f))
      {
        ++this._stopCount;
        if (this._stopCount >= 10 && agent.DestPosition.HasValue)
        {
          this._stopCount = 0;
          this.SetDestinationForce(agent.DestPosition.Value);
        }
      }
      return (TaskStatus) 3;
    }

    private bool SetDestinationForce(Vector3 destination)
    {
      bool flag = false;
      NavMeshAgent navMeshAgent = this.Agent.NavMeshAgent;
      if (!navMeshAgent.get_isOnNavMesh())
        return flag;
      if (this._path == null)
        this._path = new NavMeshPath();
      if (navMeshAgent.CalculatePath(destination, this._path))
      {
        navMeshAgent.ResetPath();
        if (!navMeshAgent.SetPath(this._path) || !navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>())
          ;
      }
      return flag;
    }

    private bool SetDestination(Vector3 destination)
    {
      bool flag = false;
      NavMeshAgent navMeshAgent = this.Agent.NavMeshAgent;
      if (!navMeshAgent.get_isOnNavMesh() || !navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>())
        return flag;
      if (this._path == null)
        this._path = new NavMeshPath();
      if (!navMeshAgent.CalculatePath(destination, this._path) || !navMeshAgent.SetPath(this._path))
        ;
      return flag;
    }

    public virtual void OnPause(bool paused)
    {
      if (paused)
        return;
      this.Agent.ActivateTransfer(true);
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      if (agent.DestPosition.HasValue)
        agent.DestPosition = new Vector3?();
      this._path = (NavMeshPath) null;
    }
  }
}
