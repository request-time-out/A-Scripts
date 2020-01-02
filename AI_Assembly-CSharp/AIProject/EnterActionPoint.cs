// Decompiled with JetBrains decompiler
// Type: AIProject.EnterActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterActionPoint : AgentAction
  {
    [SerializeField]
    private bool _enterClose;
    private bool _rejected;
    private int _stopCount;
    private NavMeshPath _path;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.ActivateTransfer(true);
      ActionPoint sightActionPoint = agent.TargetInSightActionPoint;
      if (!Object.op_Inequality((Object) sightActionPoint, (Object) null))
        return;
      Vector3? nullable = new Vector3?(sightActionPoint.LocatedPosition);
      agent.DestPosition = nullable;
      this.SetDestinationForce(nullable.Value);
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Equality((Object) agent.TargetInSightActionPoint, (Object) null))
        return (TaskStatus) 1;
      if (this._rejected)
      {
        this._rejected = false;
        if (agent.Mode == Desire.ActionType.TakeSleepPoint || agent.Mode == Desire.ActionType.TakeSleepHPoint || (agent.Mode == Desire.ActionType.TakeEatPoint || agent.Mode == Desire.ActionType.TakeBreakPoint))
          return (TaskStatus) 2;
        agent.TargetInSightActionPoint = (ActionPoint) null;
        return (TaskStatus) 1;
      }
      if (!agent.TargetInSightActionPoint.IsNeutralCommand)
      {
        if (Object.op_Equality((Object) agent.TargetInSightActionPoint.Reserver, (Object) agent))
          agent.TargetInSightActionPoint.Reserver = (Actor) null;
        agent.TargetInSightActionPoint = (ActionPoint) null;
        return (TaskStatus) 1;
      }
      if (agent.Mode != Desire.ActionType.TakeSleepPoint && agent.Mode != Desire.ActionType.TakeSleepHPoint && (agent.Mode != Desire.ActionType.TakeEatPoint && agent.Mode != Desire.ActionType.TakeBreakPoint))
      {
        List<ActionPoint> connectedActionPoints = agent.TargetInSightActionPoint.ConnectedActionPoints;
        if (!connectedActionPoints.IsNullOrEmpty<ActionPoint>())
        {
          foreach (ActionPoint actionPoint in connectedActionPoints)
          {
            if (!Object.op_Equality((Object) actionPoint, (Object) null) && !actionPoint.IsNeutralCommand)
            {
              if (Object.op_Equality((Object) agent.TargetInSightActionPoint.Reserver, (Object) agent))
                agent.TargetInSightActionPoint.Reserver = (Actor) null;
              agent.TargetInSightActionPoint = (ActionPoint) null;
              return (TaskStatus) 1;
            }
          }
        }
      }
      if (Object.op_Inequality((Object) agent.TargetInSightActionPoint.Reserver, (Object) agent))
      {
        agent.TargetInSightActionPoint = (ActionPoint) null;
        return (TaskStatus) 1;
      }
      if (agent.TargetInSightActionPoint is WarpPoint)
      {
        WarpPoint sightActionPoint = agent.TargetInSightActionPoint as WarpPoint;
        if (Object.op_Inequality((Object) sightActionPoint, (Object) null))
        {
          Dictionary<int, List<WarpPoint>> dictionary;
          List<WarpPoint> source;
          if (!Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(sightActionPoint.OwnerArea.ChunkID, out dictionary) || !dictionary.TryGetValue(sightActionPoint.TableID, out source) || (source.IsNullOrEmpty<WarpPoint>() || source.Count < 2))
          {
            agent.TargetInSightActionPoint = (ActionPoint) null;
            return (TaskStatus) 1;
          }
        }
        else
        {
          agent.TargetInSightActionPoint = (ActionPoint) null;
          return (TaskStatus) 1;
        }
      }
      if (!agent.DestPosition.HasValue)
        return (TaskStatus) 1;
      if (agent.DestPosition.HasValue)
        this.SetDestination(agent.DestPosition.Value);
      if ((double) Vector3.Distance(agent.DestPosition.Value, agent.Position) <= (!this._enterClose ? (double) Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPoint : (double) Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPointCloser))
        return (TaskStatus) 2;
      Vector3 desiredVelocity = agent.NavMeshAgent.get_desiredVelocity();
      if (Mathf.Approximately(((Vector3) ref desiredVelocity).get_magnitude(), 0.0f) && (double) Time.get_timeScale() > 0.0)
      {
        ++this._stopCount;
        if (this._stopCount >= 10 && agent.DestPosition.HasValue)
        {
          this._stopCount = 0;
          agent.NavMeshAgent.ResetPath();
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
        if (this._path.get_status() != null)
          this._rejected = true;
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
      if (navMeshAgent.CalculatePath(destination, this._path))
      {
        if (this._path.get_status() != null)
          this._rejected = true;
        if (!navMeshAgent.SetPath(this._path))
          ;
      }
      return flag;
    }

    public virtual void OnPause(bool paused)
    {
      if (paused)
        return;
      AgentActor agent = this.Agent;
      agent.ActivateTransfer(true);
      ActionPoint sightActionPoint = agent.TargetInSightActionPoint;
      if (!Object.op_Inequality((Object) sightActionPoint, (Object) null))
        return;
      Vector3? nullable = new Vector3?(sightActionPoint.LocatedPosition);
      agent.DestPosition = nullable;
      this.SetDestinationForce(nullable.Value);
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
