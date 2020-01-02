// Decompiled with JetBrains decompiler
// Type: AIProject.MoveSearchAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class MoveSearchAnimal : AgentAction
  {
    private AgentActor _agent;

    public virtual void OnStart()
    {
      this._agent = this.Agent;
      ((Task) this).OnStart();
      this._agent.StateType = AIProject.Definitions.State.Type.Normal;
      this.Replay(this._agent);
    }

    private void Replay(AgentActor agent)
    {
      agent.SearchAnimalEmpty = false;
      agent.ResetLocomotionAnimation(true);
      agent.SetOriginalDestination();
      agent.StartAnimalPatrol();
    }

    public virtual TaskStatus OnUpdate()
    {
      PointManager pointManager = !Singleton<Manager.Map>.IsInstance() ? (PointManager) null : Singleton<Manager.Map>.Instance.PointAgent;
      if ((!Object.op_Inequality((Object) pointManager, (Object) null) ? (Waypoint[]) null : pointManager.Waypoints).IsNullOrEmpty<Waypoint>())
      {
        bool flag = false;
        Dictionary<int, List<Waypoint>> source = !Object.op_Inequality((Object) pointManager, (Object) null) ? (Dictionary<int, List<Waypoint>>) null : pointManager.HousingWaypointTable;
        if (!source.IsNullOrEmpty<int, List<Waypoint>>())
        {
          foreach (KeyValuePair<int, List<Waypoint>> keyValuePair in source)
          {
            if (flag = !keyValuePair.Value.IsNullOrEmpty<Waypoint>())
              break;
          }
        }
        if (!flag)
          return (TaskStatus) 3;
      }
      if (this._agent.SearchAnimalEmpty)
        return (TaskStatus) 1;
      if (this._agent.LivesAnimalCalc)
        return (TaskStatus) 3;
      return this._agent.SearchAnimalRoute.Count == 0 && Object.op_Equality((Object) this._agent.DestWaypoint, (Object) null) || !this._agent.LivesAnimalPatrol ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      this._agent.StopAnimalPatrol();
      ((Task) this).OnEnd();
    }

    public virtual void OnPause(bool paused)
    {
      if (paused)
        this._agent.StopAnimalPatrol();
      else
        this.Replay(this._agent);
    }

    public virtual void OnBehaviorRestart()
    {
      if (Object.op_Inequality((Object) this._agent, (Object) null))
      {
        this._agent.StopForcedAnimalPatrol();
        this._agent.ClearAnimalRoutePoints();
      }
      ((Task) this).OnBehaviorComplete();
    }
  }
}
