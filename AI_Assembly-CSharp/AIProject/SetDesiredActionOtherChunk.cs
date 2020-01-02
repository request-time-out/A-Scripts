// Decompiled with JetBrains decompiler
// Type: AIProject.SetDesiredActionOtherChunk
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
  public class SetDesiredActionOtherChunk : AgentAction
  {
    [SerializeField]
    private bool _checkFollowType;
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
      Vector3 position = agent.Position;
      List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
      PointManager pointAgent = Singleton<Manager.Map>.Instance.PointAgent;
      switch (Singleton<Manager.Map>.Instance.Simulator.Weather)
      {
        case Weather.Rain:
        case Weather.Storm:
          if (!agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(16))
          {
            SetDesiredActionOtherChunk.CreateList(agent, pointAgent.AppendActionPoints, actionPointList, this._eventType, this._checkFollowType, true);
            if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
            if (Object.op_Equality((Object) this._destination, (Object) null))
            {
              actionPointList.Clear();
              SetDesiredActionOtherChunk.CreateList(agent, pointAgent.ActionPoints, actionPointList, this._eventType, this._checkFollowType, true);
              if (!actionPointList.IsNullOrEmpty<ActionPoint>())
              {
                this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
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
        SetDesiredActionOtherChunk.CreateList(agent, pointAgent.AppendActionPoints, actionPointList, this._eventType, this._checkFollowType, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
      }
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        actionPointList.Clear();
        SetDesiredActionOtherChunk.CreateList(agent, pointAgent.ActionPoints, actionPointList, this._eventType, this._checkFollowType, false);
        if (!actionPointList.IsNullOrEmpty<ActionPoint>())
          this._destination = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
      }
      ListPool<ActionPoint>.Release(actionPointList);
    }

    private static void CreateList(
      AgentActor agent,
      List<ActionPoint> source,
      List<ActionPoint> destination,
      EventType eventType,
      bool isFollow,
      bool isRain)
    {
      int chunkId = agent.ChunkID;
      Dictionary<int, bool> dictionary = DictionaryPool<int, bool>.Get();
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
      foreach (ActionPoint pt in source)
      {
        if (SetDesiredActionOtherChunk.CheckNeutral(agent, pt, dictionary, searchCount, chunkId, eventType, isFollow, isRain, meshSampleDistance))
          destination.Add(pt);
      }
      DictionaryPool<int, bool>.Release(dictionary);
    }

    private static void CreateList(
      AgentActor agent,
      ActionPoint[] source,
      List<ActionPoint> destination,
      EventType eventType,
      bool isFollow,
      bool isRain)
    {
      int chunkId = agent.ChunkID;
      Dictionary<int, bool> dictionary = DictionaryPool<int, bool>.Get();
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
      foreach (ActionPoint pt in source)
      {
        if (SetDesiredActionOtherChunk.CheckNeutral(agent, pt, dictionary, searchCount, chunkId, eventType, isFollow, isRain, meshSampleDistance))
          destination.Add(pt);
      }
      DictionaryPool<int, bool>.Release(dictionary);
    }

    private static bool CheckNeutral(
      AgentActor agent,
      ActionPoint pt,
      Dictionary<int, bool> availableArea,
      int searchCount,
      int chunkID,
      EventType eventType,
      bool isFollow,
      bool isRain,
      float sampleDistance)
    {
      if (Object.op_Equality((Object) pt, (Object) null) || Object.op_Equality((Object) pt.OwnerArea, (Object) null) || (!pt.IsNeutralCommand || pt.IsReserved(agent)))
        return false;
      List<ActionPoint> connectedActionPoints = pt.ConnectedActionPoints;
      if (!connectedActionPoints.IsNullOrEmpty<ActionPoint>())
      {
        foreach (ActionPoint actionPoint in connectedActionPoints)
        {
          if (!Object.op_Equality((Object) actionPoint, (Object) null) && (!actionPoint.IsNeutralCommand || actionPoint.IsReserved(agent)))
            return false;
        }
      }
      if (isRain && pt.AreaType != MapArea.AreaType.Indoor)
        return false;
      MapArea ownerArea = pt.OwnerArea;
      if (ownerArea.ChunkID == chunkID)
        return false;
      bool flag1;
      if (!availableArea.TryGetValue(ownerArea.AreaID, out flag1))
        availableArea[ownerArea.AreaID] = flag1 = Singleton<Manager.Map>.Instance.CheckAvailableMapArea(ownerArea.AreaID);
      if (!flag1 || !(!isFollow ? pt.AgentEventType : pt.AgentDateEventType).Contains(eventType))
        return false;
      switch (eventType)
      {
        case EventType.Eat:
          StuffItem carryingItem = agent.AgentData.CarryingItem;
          AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
          ItemIDKeyPair[] canStandEatItems = Singleton<Resources>.Instance.AgentProfile.CanStandEatItems;
          bool flag2 = false;
          foreach (ItemIDKeyPair itemIdKeyPair in canStandEatItems)
          {
            if (carryingItem.CategoryID == itemIdKeyPair.categoryID && carryingItem.ID == itemIdKeyPair.itemID)
            {
              flag2 = true;
              break;
            }
          }
          if (flag2)
          {
            PoseKeyPair eatDeskId1 = agentProfile.PoseIDTable.EatDeskID;
            PoseKeyPair eatDeskId2 = agentProfile.PoseIDTable.EatDeskID;
            ActionPointInfo outInfo;
            if (!pt.FindAgentActionPointInfo(EventType.Eat, eatDeskId1.poseID, out outInfo) && !pt.FindAgentActionPointInfo(EventType.Eat, eatDeskId2.poseID, out outInfo))
              return false;
            break;
          }
          PoseKeyPair eatDishId = agentProfile.PoseIDTable.EatDishID;
          if (!pt.FindAgentActionPointInfo(EventType.Eat, eatDishId.poseID, out ActionPointInfo _))
            return false;
          break;
        case EventType.Search:
          SearchActionPoint searchActionPoint = pt as SearchActionPoint;
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
            if (searchActionInfo1.Count >= searchCount)
              return false;
            int tableId = searchActionPoint.TableID;
            StuffItem itemInfo = agent.AgentData.EquipedSearchItem(tableId);
            if (agent.SearchAreaID == 0)
            {
              if (tableId != 0 && tableId != 1 && tableId != 2 || !searchActionPoint.CanSearch(EventType.Search, itemInfo))
                return false;
              break;
            }
            if (agent.SearchAreaID != searchActionPoint.TableID || !searchActionPoint.CanSearch(EventType.Search, itemInfo))
              return false;
            break;
          }
          break;
        case EventType.Warp:
          WarpPoint warpPoint = pt as WarpPoint;
          Dictionary<int, List<WarpPoint>> dictionary;
          List<WarpPoint> warpPointList;
          if (!Object.op_Inequality((Object) warpPoint, (Object) null) || !Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(ownerArea.ChunkID, out dictionary) || (!dictionary.TryGetValue(warpPoint.TableID, out warpPointList) || warpPointList.Count < 2))
            return false;
          break;
      }
      if (SetDesiredActionOtherChunk._navMeshPath == null)
        SetDesiredActionOtherChunk._navMeshPath = new NavMeshPath();
      NavMeshHit navMeshHit;
      return agent.NavMeshAgent.CalculatePath(pt.LocatedPosition, SetDesiredActionOtherChunk._navMeshPath) && SetDesiredActionOtherChunk._navMeshPath.get_status() == null && NavMesh.SamplePosition(pt.LocatedPosition, ref navMeshHit, sampleDistance, agent.NavMeshAgent.get_areaMask());
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (Object.op_Inequality((Object) agent.TargetInSightActionPoint, (Object) null))
      {
        if (this._checkFollowType)
        {
          if (agent.TargetInSightActionPoint.AgentDateEventType.Contains(this._eventType))
            return (TaskStatus) 2;
        }
        else if (agent.TargetInSightActionPoint.AgentEventType.Contains(this._eventType))
          return (TaskStatus) 2;
      }
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
