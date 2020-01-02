// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.NavMeshMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  public abstract class NavMeshMovement : BehaviorDesigner.Runtime.Tasks.Movement.Movement
  {
    [Tooltip("The speed of the agent")]
    public SharedFloat speed = (SharedFloat) 10f;
    [Tooltip("The angular speed of the agent")]
    public SharedFloat angularSpeed = (SharedFloat) 120f;
    [Tooltip("The agent has arrived when the destination is less than the specified amount")]
    public SharedFloat arriveDistance = (SharedFloat) 0.2f;
    [Tooltip("Should the NavMeshAgent be stopped when the task ends?")]
    public SharedBool stopOnTaskEnd = (SharedBool) true;
    [Tooltip("Should the NavMeshAgent rotation be updated when the task ends?")]
    public SharedBool updateRotation = (SharedBool) true;
    protected NavMeshAgent navMeshAgent;

    public virtual void OnAwake()
    {
      this.navMeshAgent = (NavMeshAgent) ((Task) this).GetComponent<NavMeshAgent>();
    }

    public virtual void OnStart()
    {
      this.navMeshAgent.set_speed(this.speed.get_Value());
      this.navMeshAgent.set_angularSpeed(this.angularSpeed.get_Value());
      this.navMeshAgent.set_isStopped(false);
      if (this.updateRotation.get_Value())
        return;
      this.UpdateRotation(true);
    }

    protected override bool SetDestination(Vector3 destination)
    {
      this.navMeshAgent.set_isStopped(false);
      return this.navMeshAgent.SetDestination(destination);
    }

    protected override void UpdateRotation(bool update)
    {
      this.navMeshAgent.set_updateRotation(update);
    }

    protected override bool HasPath()
    {
      return this.navMeshAgent.get_hasPath() && (double) this.navMeshAgent.get_remainingDistance() > (double) this.arriveDistance.get_Value();
    }

    protected override Vector3 Velocity()
    {
      return this.navMeshAgent.get_velocity();
    }

    protected bool SamplePosition(Vector3 position)
    {
      NavMeshHit navMeshHit;
      return NavMesh.SamplePosition(position, ref navMeshHit, float.MaxValue, -1);
    }

    protected override bool HasArrived()
    {
      return (!this.navMeshAgent.get_pathPending() ? (double) this.navMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) this.arriveDistance.get_Value();
    }

    protected override void Stop()
    {
      this.UpdateRotation(this.updateRotation.get_Value());
      if (!this.navMeshAgent.get_hasPath())
        return;
      this.navMeshAgent.set_isStopped(true);
    }

    public virtual void OnEnd()
    {
      if (this.stopOnTaskEnd.get_Value())
        this.Stop();
      else
        this.UpdateRotation(this.updateRotation.get_Value());
    }

    public virtual void OnBehaviorComplete()
    {
      this.Stop();
    }

    public virtual void OnReset()
    {
      this.speed = (SharedFloat) 10f;
      this.angularSpeed = (SharedFloat) 120f;
      this.arriveDistance = (SharedFloat) 1f;
      this.stopOnTaskEnd = (SharedBool) true;
    }
  }
}
