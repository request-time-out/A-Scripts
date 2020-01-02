// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.Accompany
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class Accompany : MerchantMovement
  {
    private bool _moved;
    private MerchantActor _merchant;
    private NavMeshAgent _navMeshAgent;
    private Actor _partner;

    public virtual void OnAwake()
    {
      ((Task) this).OnAwake();
      this._merchant = this.Merchant;
      this._navMeshAgent = this._merchant.NavMeshAgent;
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._partner = this._merchant.Partner;
      if (Object.op_Equality((Object) this._partner, (Object) null))
        return;
      this._merchant.ActivateLocomotionMotion();
      this._merchant.ActivateNavMeshAgent();
      this.SetDestination(this.DesiredPosition(this._partner));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._partner, (Object) null))
        return (TaskStatus) 1;
      if (!Singleton<Resources>.IsInstance())
        return (TaskStatus) 3;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      Vector3 destination = this.DesiredPosition(this._partner);
      if ((double) agentProfile.RestDistance <= (double) Vector3.Distance(destination, this._merchant.Position))
      {
        this.SetDestination(destination);
        this._moved = true;
      }
      else
      {
        NavMeshPathStatus pathStatus = this._navMeshAgent.get_pathStatus();
        if (pathStatus == 1 || pathStatus == 2)
        {
          if ((double) Vector3.Distance(this._merchant.Position, this._partner.Position) < (double) agentProfile.RestDistance)
          {
            this.Stop();
            if (this._merchant.IsRunning)
              this._merchant.IsRunning = false;
          }
        }
        else if (!this._navMeshAgent.get_pathPending())
        {
          if ((double) this._navMeshAgent.get_remainingDistance() < (double) agentProfile.RestDistance && this._merchant.IsRunning)
            this._merchant.IsRunning = false;
          if (this._moved && (double) this._navMeshAgent.get_remainingDistance() < (double) this._navMeshAgent.get_stoppingDistance())
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
      float shapeBodyValue = this.Merchant.ChaControl.GetShapeBodyValue(0);
      return Vector3.op_Addition(partner.Position, Quaternion.op_Multiply(partner.Rotation, Singleton<Resources>.Instance.AgentProfile.GetOffsetInParty(shapeBodyValue)));
    }

    protected override bool SetDestination(Vector3 destination)
    {
      if (this._navMeshAgent.get_isStopped())
        this._navMeshAgent.set_isStopped(false);
      return this._navMeshAgent.SetDestination(destination);
    }

    protected override void UpdateRotation(bool update)
    {
      this._navMeshAgent.set_updateRotation(update);
    }

    protected override bool HasPath()
    {
      return this._navMeshAgent.get_hasPath() && (double) this._navMeshAgent.get_stoppingDistance() < (double) this._navMeshAgent.get_remainingDistance();
    }

    protected override Vector3 Velocity()
    {
      return this._navMeshAgent.get_velocity();
    }

    protected override bool HasArrived()
    {
      return ((Behaviour) this._merchant).get_enabled() && (!this._navMeshAgent.get_pathPending() ? (double) this._navMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) Singleton<Resources>.Instance.AgentProfile.RestDistance;
    }

    protected override void Stop()
    {
      if (!this._navMeshAgent.get_hasPath())
        return;
      this._navMeshAgent.set_isStopped(true);
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
