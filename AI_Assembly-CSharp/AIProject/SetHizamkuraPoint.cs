// Decompiled with JetBrains decompiler
// Type: AIProject.SetHizamkuraPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class SetHizamkuraPoint : AgentAction
  {
    private ActionPoint _destination;
    private static NavMeshPath _navMeshPath;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      Chunk chunk;
      Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(agent.ChunkID, out chunk);
      Vector3 position = agent.Position;
      List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
      switch (Singleton<Manager.Map>.Instance.Simulator.Weather)
      {
        case Weather.Rain:
        case Weather.Storm:
          if (!agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(16))
          {
            SetHizamkuraPoint.CreateList(agent, chunk.AppendActionPoints, actionPointList, true);
            if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              SetHizamkuraPoint.NearestPoint(position, actionPointList, out this._destination);
            if (Object.op_Equality((Object) this._destination, (Object) null))
            {
              actionPointList.Clear();
              SetHizamkuraPoint.CreateList(agent, chunk.ActionPoints, actionPointList, true);
              if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              {
                SetHizamkuraPoint.NearestPoint(position, actionPointList, out this._destination);
                break;
              }
              break;
            }
            break;
          }
          break;
      }
      if (Object.op_Equality((Object) this._destination, (Object) null) && !agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(17))
      {
        actionPointList.Clear();
        SetHizamkuraPoint.CreateList(agent, chunk.AppendActionPoints, actionPointList, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          SetHizamkuraPoint.NearestPoint(position, actionPointList, out this._destination);
      }
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        actionPointList.Clear();
        SetHizamkuraPoint.CreateList(agent, chunk.ActionPoints, actionPointList, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          SetHizamkuraPoint.NearestPoint(position, actionPointList, out this._destination);
      }
      ListPool<ActionPoint>.Release(actionPointList);
    }

    private static void CreateList(
      AgentActor agent,
      List<ActionPoint> source,
      List<ActionPoint> destination,
      bool isRain)
    {
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
      Dictionary<int, bool> toRelease = DictionaryPool<int, bool>.Get();
      int hizamakuraID = Singleton<Resources>.Instance.PlayerProfile.HizamakuraPTID;
      foreach (ActionPoint actionPoint in source)
      {
        if (!Object.op_Equality((Object) actionPoint, (Object) null) && !Object.op_Equality((Object) actionPoint.OwnerArea, (Object) null) && (actionPoint.IsNeutralCommand && !actionPoint.IsReserved(agent)) && (!isRain || actionPoint.AreaType == MapArea.AreaType.Indoor))
        {
          MapArea ownerArea = actionPoint.OwnerArea;
          bool flag;
          if (!toRelease.TryGetValue(ownerArea.AreaID, out flag))
            toRelease[ownerArea.AreaID] = flag = Singleton<Manager.Map>.Instance.CheckAvailableMapArea(ownerArea.AreaID);
          if (flag)
          {
            EventType eventType = actionPoint.PlayerDateEventType[(int) Singleton<Manager.Map>.Instance.Player.ChaControl.sex];
            if (!actionPoint.IDList.IsNullOrEmpty<int>() && ((IEnumerable<int>) actionPoint.IDList).Any<int>((Func<int, bool>) (x => x == hizamakuraID)) || actionPoint.IDList.IsNullOrEmpty<int>() && actionPoint.ID == hizamakuraID)
            {
              if (SetHizamkuraPoint._navMeshPath == null)
                SetHizamkuraPoint._navMeshPath = new NavMeshPath();
              NavMeshHit navMeshHit;
              if (agent.NavMeshAgent.CalculatePath(actionPoint.LocatedPosition, SetHizamkuraPoint._navMeshPath) && SetHizamkuraPoint._navMeshPath.get_status() == null && NavMesh.SamplePosition(actionPoint.LocatedPosition, ref navMeshHit, meshSampleDistance, agent.NavMeshAgent.get_areaMask()))
                destination.Add(actionPoint);
            }
          }
        }
      }
      DictionaryPool<int, bool>.Release(toRelease);
    }

    private static void NearestPoint(
      Vector3 position,
      List<ActionPoint> actionPoints,
      out ActionPoint destination)
    {
      destination = (ActionPoint) null;
      float? nullable = new float?();
      Vector3 vector3 = position;
      foreach (ActionPoint actionPoint in actionPoints)
      {
        float num = Vector3.Distance(vector3, actionPoint.Position);
        if (!nullable.HasValue)
        {
          nullable = new float?(num);
          destination = actionPoint;
        }
        else if (!nullable.HasValue || (!nullable.HasValue ? 0 : ((double) nullable.GetValueOrDefault() <= (double) num ? 1 : 0)) == 0)
        {
          nullable = new float?(num);
          destination = actionPoint;
        }
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      int hizamakuraID = Singleton<Resources>.Instance.PlayerProfile.HizamakuraPTID;
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent.TargetInSightActionPoint, (Object) null))
      {
        ActionPoint sightActionPoint = agent.TargetInSightActionPoint;
        if (!sightActionPoint.IDList.IsNullOrEmpty<int>() && ((IEnumerable<int>) sightActionPoint.IDList).Any<int>((Func<int, bool>) (x => x == hizamakuraID)) || sightActionPoint.IDList.IsNullOrEmpty<int>() && sightActionPoint.ID == hizamakuraID)
          return (TaskStatus) 2;
      }
      if (Object.op_Equality((Object) this._destination, (Object) null))
        return (TaskStatus) 1;
      agent.TargetInSightActionPoint = this._destination;
      agent.EventKey = EventType.Break;
      this._destination.Reserver = (Actor) agent;
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      this._destination = (ActionPoint) null;
    }
  }
}
