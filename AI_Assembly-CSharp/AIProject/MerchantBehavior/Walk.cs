// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.Walk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class Walk : MerchantAction
  {
    private bool _prevActiveEncount;
    private bool _walkActive;

    private NavMeshAgent Agent
    {
      get
      {
        return this.Merchant.NavMeshAgent;
      }
    }

    private bool AgentActive
    {
      get
      {
        return ((Behaviour) this.Agent).get_isActiveAndEnabled() && this.Agent.get_isOnNavMesh();
      }
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._prevActiveEncount = this.Merchant.ActiveEncount;
      if (!this._prevActiveEncount)
        this.Merchant.ActiveEncount = true;
      this.StartWalk();
      this._walkActive = true;
    }

    private void StartWalk()
    {
      if (Object.op_Equality((Object) this.TargetInSightMerchantPoint, (Object) null))
        return;
      this.Merchant.StartLocomotion(this.TargetInSightMerchantPoint.Destination);
    }

    private void StopWalk()
    {
      if (!this.AgentActive)
        return;
      this.Merchant.StopNavMeshAgent();
    }

    public virtual void OnPause(bool paused)
    {
      if (!this._walkActive)
        return;
      ((Task) this).OnPause(paused);
      if (paused)
        this.StopWalk();
      else
        this.StartWalk();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.TargetInSightMerchantPoint, (Object) null))
        return (TaskStatus) 1;
      if (this.AgentActive && !this.Merchant.NavMeshAgent.get_isStopped() && !this.Merchant.NavMeshAgent.get_pathPending())
        this.Merchant.NavMeshAgent.SetDestination(this.TargetInSightMerchantPoint.Destination);
      return (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      this._walkActive = false;
      this.StopWalk();
      if (!this._prevActiveEncount)
        this.Merchant.ActiveEncount = false;
      ((Task) this).OnEnd();
    }

    public virtual void OnBehaviorComplete()
    {
      this._walkActive = false;
      ((Task) this).OnBehaviorComplete();
    }
  }
}
