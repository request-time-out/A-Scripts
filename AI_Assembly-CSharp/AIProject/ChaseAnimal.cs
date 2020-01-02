// Decompiled with JetBrains decompiler
// Type: AIProject.ChaseAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using ReMotion;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ChaseAnimal : AgentMovement
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.StateType = AIProject.Definitions.State.Type.Normal;
      this.Agent.ActivateTransfer(true);
      float _speed = this.Agent.NavMeshAgent.get_speed();
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(1f, false), false), (System.Action<M0>) (x => this.Agent.NavMeshAgent.set_speed(Mathf.Lerp(_speed, Singleton<Resources>.Instance.LocomotionProfile.AgentSpeed.walkSpeed, ((TimeInterval<float>) ref x).get_Value()))));
    }

    public virtual TaskStatus OnUpdate()
    {
      AnimalBase targetInSightAnimal = this.Agent.TargetInSightAnimal;
      if (Object.op_Equality((Object) targetInSightAnimal, (Object) null))
        return (TaskStatus) 1;
      if (!targetInSightAnimal.IsWithAgentFree(this.Agent))
        return (TaskStatus) 1;
      AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
      if (!this.Agent.NavMeshAgent.get_pathPending())
        this.SetDestination(targetInSightAnimal.Position);
      return !this.HasArrived() ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Stop();
    }

    private float RemainingDistance
    {
      get
      {
        return ((Behaviour) this.Agent).get_enabled() ? (!this.Agent.NavMeshAgent.get_pathPending() ? this.Agent.NavMeshAgent.get_remainingDistance() : float.PositiveInfinity) : float.PositiveInfinity;
      }
    }

    protected override bool HasArrived()
    {
      return (double) this.RemainingDistance <= (double) Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance;
    }

    protected override bool HasPath()
    {
      return this.Agent.NavMeshAgent.get_hasPath() && (double) Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance < (double) this.Agent.NavMeshAgent.get_remainingDistance();
    }

    protected override bool SetDestination(Vector3 destination)
    {
      if (this.Agent.NavMeshAgent.get_isStopped())
        this.Agent.NavMeshAgent.set_isStopped(false);
      return this.Agent.NavMeshAgent.SetDestination(destination);
    }

    protected override void Stop()
    {
      if (!this.Agent.NavMeshAgent.get_hasPath())
        return;
      this.Agent.NavMeshAgent.set_isStopped(true);
    }

    protected override void UpdateRotation(bool update)
    {
      this.Agent.NavMeshAgent.set_updateRotation(update);
    }

    protected override Vector3 Velocity()
    {
      return this.Agent.NavMeshAgent.get_velocity();
    }
  }
}
