// Decompiled with JetBrains decompiler
// Type: AIProject.SetDesiredPDateAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class SetDesiredPDateAction : AgentAction
  {
    [SerializeField]
    private EventType _eventType;
    [SerializeField]
    private Desire.Type _desireIfNotFound;
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
            SetDesiredPDateAction.CreateList(agent, chunk.AppendActionPoints, actionPointList, this._eventType, true);
            if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
            if (Object.op_Equality((Object) this._destination, (Object) null))
            {
              actionPointList.Clear();
              SetDesiredPDateAction.CreateList(agent, chunk.ActionPoints, actionPointList, this._eventType, true);
              if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              {
                SetDesiredPDateAction.NearestPoint(position, actionPointList, out this._destination);
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
        SetDesiredPDateAction.CreateList(agent, chunk.AppendActionPoints, actionPointList, this._eventType, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
      }
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        actionPointList.Clear();
        SetDesiredPDateAction.CreateList(agent, chunk.ActionPoints, actionPointList, this._eventType, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          SetDesiredPDateAction.NearestPoint(position, actionPointList, out this._destination);
      }
      ListPool<ActionPoint>.Release(actionPointList);
    }

    private static void CreateList(
      AgentActor agent,
      List<ActionPoint> source,
      List<ActionPoint> destination,
      EventType eventType,
      bool isRain)
    {
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
      Dictionary<int, bool> toRelease = DictionaryPool<int, bool>.Get();
      foreach (ActionPoint actionPoint1 in source)
      {
        if (!Object.op_Equality((Object) actionPoint1, (Object) null) && !Object.op_Equality((Object) actionPoint1.OwnerArea, (Object) null) && (actionPoint1.IsNeutralCommand && !actionPoint1.IsReserved(agent)))
        {
          List<ActionPoint> connectedActionPoints = actionPoint1.ConnectedActionPoints;
          if (!connectedActionPoints.IsNullOrEmpty<ActionPoint>())
          {
            bool flag = false;
            foreach (ActionPoint actionPoint2 in connectedActionPoints)
            {
              if (!Object.op_Equality((Object) actionPoint2, (Object) null) && (!actionPoint2.IsNeutralCommand || actionPoint2.IsReserved(agent)))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              continue;
          }
          if (!isRain || actionPoint1.AreaType == MapArea.AreaType.Indoor)
          {
            MapArea ownerArea = actionPoint1.OwnerArea;
            bool flag;
            if (!toRelease.TryGetValue(ownerArea.AreaID, out flag))
              toRelease[ownerArea.AreaID] = flag = Singleton<Manager.Map>.Instance.CheckAvailableMapArea(ownerArea.AreaID);
            if (flag && actionPoint1.PlayerDateEventType[(int) Singleton<Manager.Map>.Instance.Player.ChaControl.sex].Contains(eventType))
            {
              if (eventType == EventType.Search)
              {
                SearchActionPoint searchActionPoint = actionPoint1 as SearchActionPoint;
                if (Object.op_Inequality((Object) searchActionPoint, (Object) null))
                {
                  int registerId = searchActionPoint.RegisterID;
                  Dictionary<int, AIProject.SaveData.Environment.SearchActionInfo> searchActionLockTable = agent.AgentData.SearchActionLockTable;
                  AIProject.SaveData.Environment.SearchActionInfo searchActionInfo1;
                  if (!searchActionLockTable.TryGetValue(registerId, out searchActionInfo1))
                  {
                    AIProject.SaveData.Environment.SearchActionInfo searchActionInfo2 = new AIProject.SaveData.Environment.SearchActionInfo();
                    searchActionLockTable[registerId] = searchActionInfo2;
                    searchActionInfo1 = searchActionInfo2;
                  }
                  if (searchActionInfo1.Count < searchCount)
                  {
                    int tableId = searchActionPoint.TableID;
                    StuffItem itemInfo = agent.AgentData.EquipedSearchItem(tableId);
                    if (agent.SearchAreaID == 0)
                    {
                      if (tableId != 0 && tableId != 1 && tableId != 2 || !searchActionPoint.CanSearch(EventType.Search, itemInfo))
                        continue;
                    }
                    else if (agent.SearchAreaID != searchActionPoint.TableID || !searchActionPoint.CanSearch(EventType.Search, itemInfo))
                      continue;
                  }
                  else
                    continue;
                }
              }
              if (SetDesiredPDateAction._navMeshPath == null)
                SetDesiredPDateAction._navMeshPath = new NavMeshPath();
              NavMeshHit navMeshHit;
              if (agent.NavMeshAgent.CalculatePath(actionPoint1.LocatedPosition, SetDesiredPDateAction._navMeshPath) && SetDesiredPDateAction._navMeshPath.get_status() == null && NavMesh.SamplePosition(actionPoint1.LocatedPosition, ref navMeshHit, meshSampleDistance, agent.NavMeshAgent.get_areaMask()))
                destination.Add(actionPoint1);
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
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent.TargetInSightActionPoint, (Object) null) && agent.TargetInSightActionPoint.PlayerDateEventType[(int) Singleton<Manager.Map>.Instance.Player.ChaControl.sex].Contains(this._eventType))
        return (TaskStatus) 2;
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        if (this._desireIfNotFound != Desire.Type.None)
        {
          int desireKey = Desire.GetDesireKey(this._desireIfNotFound);
          agent.SetDesire(desireKey, 0.0f);
          agent.ChangeBehavior(Desire.ActionType.Normal);
        }
        return (TaskStatus) 1;
      }
      agent.TargetInSightActionPoint = this._destination;
      agent.EventKey = this._eventType;
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
