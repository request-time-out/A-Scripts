// Decompiled with JetBrains decompiler
// Type: AIProject.HeadToStoryPoint
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
  public class HeadToStoryPoint : AgentAction
  {
    private AgentActor _agent;
    private NavMeshAgent _navMeshAgent;
    private StoryPoint _point;
    private bool _missing;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._navMeshAgent = this._agent != null ? this._agent.NavMeshAgent : (NavMeshAgent) null;
      this._point = this._agent != null ? this._agent.TargetStoryPoint : (StoryPoint) null;
      this._missing = Object.op_Equality((Object) this._agent, (Object) null) || Object.op_Equality((Object) this._navMeshAgent, (Object) null) || Object.op_Equality((Object) this._point, (Object) null);
      if (this._missing)
        return;
      Vector3? nullable1 = new Vector3?(this._point.Position);
      this._agent.DestPosition = nullable1;
      Vector3? nullable2 = nullable1;
      this._agent.ActivateNavMeshAgent();
      this._agent.PlayTutorialDefaultStateAnim();
      this.SetDestinationForce(nullable2.Value);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._missing || !this._agent.DestPosition.HasValue)
        return (TaskStatus) 1;
      if (!((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || !this._navMeshAgent.get_isOnNavMesh())
        return (TaskStatus) 1;
      this.SetDestination(this._agent.DestPosition.Value);
      float distanceStoryPoint = Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceStoryPoint;
      if (this._navMeshAgent.get_hasPath())
      {
        if ((double) this._navMeshAgent.get_remainingDistance() <= (double) distanceStoryPoint)
          return (TaskStatus) 2;
      }
      else if ((double) Vector3.Distance(this._agent.DestPosition.Value, this._agent.Position) <= (double) distanceStoryPoint)
        return (TaskStatus) 2;
      return (TaskStatus) 3;
    }

    private bool SetDestinationForce(Vector3 destination)
    {
      bool flag1 = false;
      if (!((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || !this._navMeshAgent.get_isOnNavMesh())
        return flag1;
      NavMeshPath navMeshPath = new NavMeshPath();
      bool flag2;
      if (!(flag2 = this._navMeshAgent.CalculatePath(destination, navMeshPath)) || !(flag2 = this._navMeshAgent.SetPath(navMeshPath)) || (flag2 = !this._navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>()))
        ;
      return flag2;
    }

    private bool SetDestination(Vector3 destination)
    {
      bool flag1 = false;
      if (!((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || !this._navMeshAgent.get_isOnNavMesh())
        return flag1;
      bool flag2;
      if (!(flag2 = !this._navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>()))
      {
        NavMeshPath navMeshPath = new NavMeshPath();
        if (!(flag2 = this._navMeshAgent.CalculatePath(destination, navMeshPath)) || !(flag2 = this._navMeshAgent.SetPath(navMeshPath)))
          ;
      }
      return flag2;
    }

    public virtual void OnEnd()
    {
      if (Object.op_Inequality((Object) this._agent, (Object) null) && this._agent.DestPosition.HasValue)
        this._agent.DestPosition = new Vector3?();
      ((Task) this).OnEnd();
    }
  }
}
