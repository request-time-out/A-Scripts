// Decompiled with JetBrains decompiler
// Type: AIProject.Walk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  public class Walk : AgentMovement
  {
    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      ((Task) this).OnStart();
      agent.StateType = AIProject.Definitions.State.Type.Normal;
      this.Replay(agent);
    }

    private void Replay(AgentActor agent)
    {
      agent.ResetLocomotionAnimation(true);
      agent.SetOriginalDestination();
      agent.StartPatrol();
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.LivesCalc)
        return (TaskStatus) 3;
      return agent.WalkRoute.Count == 0 && Object.op_Equality((Object) this.Agent.DestWaypoint, (Object) null) || !agent.LivesPatrol ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      this.Agent.StopPatrol();
    }

    public virtual void OnPause(bool paused)
    {
      AgentActor agent = this.Agent;
      if (paused)
      {
        agent.StopPatrol();
      }
      else
      {
        agent.ResetLocomotionAnimation(true);
        agent.SetOriginalDestination();
        agent.ResumePatrol();
      }
    }

    public virtual void OnBehaviorRestart()
    {
    }

    protected override bool SetDestination(Vector3 destination)
    {
      this.Agent.NavMeshAgent.set_isStopped(false);
      return this.Agent.NavMeshAgent.SetDestination(destination);
    }

    protected override void UpdateRotation(bool update)
    {
      this.Agent.NavMeshAgent.set_updateRotation(update);
    }

    protected override bool HasPath()
    {
      return this.Agent.NavMeshAgent.get_hasPath() && (double) this.Agent.NavMeshAgent.get_remainingDistance() > (double) Singleton<Resources>.Instance.AgentProfile.WalkSetting.arrivedDistance;
    }

    protected override Vector3 Velocity()
    {
      return this.Agent.NavMeshAgent.get_velocity();
    }

    protected override bool HasArrived()
    {
      return ((Behaviour) this.Agent.NavMeshAgent).get_enabled() && (!this.Agent.NavMeshAgent.get_pathPending() ? (double) this.Agent.NavMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) Singleton<Resources>.Instance.AgentProfile.WalkSetting.arrivedDistance;
    }

    protected override void Stop()
    {
      if (!this.Agent.NavMeshAgent.get_hasPath())
        return;
      this.Agent.NavMeshAgent.set_isStopped(true);
    }
  }
}
