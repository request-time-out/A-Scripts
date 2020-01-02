// Decompiled with JetBrains decompiler
// Type: AIProject.Accompany
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
  public class Accompany : AgentMovement
  {
    private Vector3 _velocity = Vector3.get_zero();
    private Vector3 _prevDestination = Vector3.get_zero();
    private bool _moved;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      if (Object.op_Equality((Object) this.Agent.Partner, (Object) null))
        return;
      this.Agent.ActivateTransfer(true);
      this.SetDestination(this.DesiredPosition(this.Agent.Partner));
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Equality((Object) agent.Partner, (Object) null))
        return (TaskStatus) 1;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      NavMeshAgent navMeshAgent = agent.NavMeshAgent;
      Vector3 destination = this.DesiredPosition(agent.Partner);
      if ((double) Vector3.Distance(destination, agent.Position) >= (double) agentProfile.RestDistance)
      {
        this.SetDestination(destination);
        this._moved = true;
      }
      else
      {
        NavMeshPathStatus pathStatus = navMeshAgent.get_pathStatus();
        if (pathStatus == 1 || pathStatus == 2)
        {
          if ((double) Vector3.Distance(agent.Position, agent.Partner.Position) < (double) agentProfile.RestDistance)
          {
            this.Stop();
            if (agent.IsRunning)
              agent.IsRunning = false;
          }
        }
        else if (!navMeshAgent.get_pathPending())
        {
          if ((double) navMeshAgent.get_remainingDistance() < (double) agentProfile.RestDistance && agent.IsRunning)
            agent.IsRunning = false;
          if (this._moved && (double) navMeshAgent.get_remainingDistance() < (double) navMeshAgent.get_stoppingDistance())
          {
            this.Stop();
            this._moved = false;
          }
        }
      }
      return (TaskStatus) 3;
    }

    private Vector3 DesiredPosition(Actor partner)
    {
      float shapeBodyValue = this.Agent.ChaControl.GetShapeBodyValue(0);
      return Vector3.op_Addition(partner.Position, Quaternion.op_Multiply(partner.Rotation, Singleton<Resources>.Instance.AgentProfile.GetOffsetInParty(shapeBodyValue)));
    }

    protected override bool SetDestination(Vector3 destination)
    {
      if (this.Agent.NavMeshAgent.get_isStopped())
        this.Agent.NavMeshAgent.set_isStopped(false);
      return this.Agent.NavMeshAgent.SetDestination(destination);
    }

    protected override void UpdateRotation(bool update)
    {
      this.Agent.NavMeshAgent.set_updateRotation(update);
    }

    protected override bool HasPath()
    {
      NavMeshAgent navMeshAgent = this.Agent.NavMeshAgent;
      return navMeshAgent.get_hasPath() && (double) navMeshAgent.get_remainingDistance() > (double) navMeshAgent.get_stoppingDistance();
    }

    protected override Vector3 Velocity()
    {
      return this.Agent.NavMeshAgent.get_velocity();
    }

    protected override bool HasArrived()
    {
      return ((Behaviour) this.Agent).get_enabled() && (!this.Agent.NavMeshAgent.get_pathPending() ? (double) this.Agent.NavMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) Singleton<Resources>.Instance.AgentProfile.RestDistance;
    }

    protected override void Stop()
    {
      if (!this.Agent.NavMeshAgent.get_hasPath())
        return;
      this.Agent.NavMeshAgent.set_isStopped(true);
    }

    public virtual void OnPause(bool paused)
    {
      if (!paused)
        return;
      this._moved = false;
      this.Stop();
    }
  }
}
