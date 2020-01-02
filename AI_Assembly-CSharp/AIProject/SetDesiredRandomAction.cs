// Decompiled with JetBrains decompiler
// Type: AIProject.SetDesiredRandomAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

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
  public class SetDesiredRandomAction : AgentAction
  {
    [SerializeField]
    private bool _checkFollowType;
    [SerializeField]
    private EventType _eventType;
    [SerializeField]
    private Desire.Type _desireIfNotFound;
    private ActionPoint _destination;
    private static NavMeshPath _navMeshPath;
    private bool _changeWarp;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      Chunk chunk;
      Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(agent.ChunkID, out chunk);
      Vector3 position = agent.Position;
      List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
      this._changeWarp = false;
      switch (Singleton<Manager.Map>.Instance.Simulator.Weather)
      {
        case Weather.Rain:
        case Weather.Storm:
          if (!agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(16))
          {
            SetDesiredRandomAction.CreateList(agent, chunk.AppendActionPoints, actionPointList, this._eventType, this._checkFollowType, true);
            this._destination = SetDesiredRandomAction.RandomDestination(agent, this._eventType, chunk.AppendActionPoints, actionPointList, this._checkFollowType, true, out this._changeWarp);
            if (Object.op_Equality((Object) this._destination, (Object) null) && !this._changeWarp)
            {
              actionPointList.Clear();
              SetDesiredRandomAction.CreateList(agent, chunk.ActionPoints, actionPointList, this._eventType, this._checkFollowType, true);
              this._destination = SetDesiredRandomAction.RandomDestination(agent, this._eventType, chunk.ActionPoints, actionPointList, this._checkFollowType, true, out this._changeWarp);
              break;
            }
            break;
          }
          break;
      }
      if (Object.op_Equality((Object) this._destination, (Object) null) && !this._changeWarp && !agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(17))
      {
        actionPointList.Clear();
        SetDesiredRandomAction.CreateList(agent, chunk.AppendActionPoints, actionPointList, this._eventType, this._checkFollowType, false);
        this._destination = SetDesiredRandomAction.RandomDestination(agent, this._eventType, chunk.AppendActionPoints, actionPointList, this._checkFollowType, false, out this._changeWarp);
      }
      if (Object.op_Equality((Object) this._destination, (Object) null) && !this._changeWarp)
      {
        actionPointList.Clear();
        SetDesiredRandomAction.CreateList(agent, chunk.ActionPoints, actionPointList, this._eventType, this._checkFollowType, false);
        this._destination = SetDesiredRandomAction.RandomDestination(agent, this._eventType, chunk.ActionPoints, actionPointList, this._checkFollowType, false, out this._changeWarp);
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
      Dictionary<int, bool> toRelease = DictionaryPool<int, bool>.Get();
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
      float meshSampleDistance = Singleton<Resources>.Instance.LocomotionProfile.ActionPointNavMeshSampleDistance;
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
            bool flag1;
            if (!toRelease.TryGetValue(ownerArea.AreaID, out flag1))
              toRelease[ownerArea.AreaID] = flag1 = Singleton<Manager.Map>.Instance.CheckAvailableMapArea(ownerArea.AreaID);
            if (flag1 && (!isFollow ? actionPoint1.AgentEventType : actionPoint1.AgentDateEventType).Contains(eventType))
            {
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
                  ActionPointInfo outInfo;
                  if (flag2)
                  {
                    PoseKeyPair eatDeskId1 = agentProfile.PoseIDTable.EatDeskID;
                    PoseKeyPair eatDeskId2 = agentProfile.PoseIDTable.EatDeskID;
                    if (actionPoint1.FindAgentActionPointInfo(EventType.Eat, eatDeskId1.poseID, out outInfo) || actionPoint1.FindAgentActionPointInfo(EventType.Eat, eatDeskId2.poseID, out outInfo))
                      break;
                    continue;
                  }
                  PoseKeyPair eatDishId = agentProfile.PoseIDTable.EatDishID;
                  if (actionPoint1.FindAgentActionPointInfo(EventType.Eat, eatDishId.poseID, out outInfo))
                    break;
                  continue;
                case EventType.Search:
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
                        break;
                      }
                      if (agent.SearchAreaID != searchActionPoint.TableID || !searchActionPoint.CanSearch(EventType.Search, itemInfo))
                        continue;
                      break;
                    }
                    continue;
                  }
                  break;
                case EventType.Warp:
                  WarpPoint warpPoint = actionPoint1 as WarpPoint;
                  Dictionary<int, List<WarpPoint>> dictionary;
                  List<WarpPoint> warpPointList;
                  if (!Object.op_Inequality((Object) warpPoint, (Object) null) || !Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(ownerArea.ChunkID, out dictionary) || (!dictionary.TryGetValue(warpPoint.TableID, out warpPointList) || warpPointList.Count < 2))
                    continue;
                  break;
              }
              if (SetDesiredRandomAction._navMeshPath == null)
                SetDesiredRandomAction._navMeshPath = new NavMeshPath();
              NavMeshHit navMeshHit;
              if (agent.NavMeshAgent.CalculatePath(actionPoint1.LocatedPosition, SetDesiredRandomAction._navMeshPath) && SetDesiredRandomAction._navMeshPath.get_status() == null && NavMesh.SamplePosition(actionPoint1.LocatedPosition, ref navMeshHit, meshSampleDistance, agent.NavMeshAgent.get_areaMask()))
                destination.Add(actionPoint1);
            }
          }
        }
      }
      DictionaryPool<int, bool>.Release(toRelease);
    }

    private static ActionPoint RandomDestination(
      AgentActor agent,
      EventType eventType,
      List<ActionPoint> sourceList,
      List<ActionPoint> list,
      bool isFollow,
      bool isRain,
      out bool changeWarp)
    {
      changeWarp = false;
      if (eventType == EventType.Play && !isFollow)
      {
        List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
        SetDesiredRandomAction.CreateList(agent, sourceList, actionPointList, EventType.Warp, false, isRain);
        if (list.IsNullOrEmpty<ActionPoint>() && actionPointList.IsNullOrEmpty<ActionPoint>())
          return (ActionPoint) null;
        int index = Random.Range(0, list.Count + actionPointList.Count);
        ListPool<ActionPoint>.Release(actionPointList);
        changeWarp = index >= list.Count;
        return !changeWarp ? list.GetElement<ActionPoint>(index) : (ActionPoint) null;
      }
      return list.IsNullOrEmpty<ActionPoint>() ? (ActionPoint) null : list.GetElement<ActionPoint>(Random.Range(0, list.Count));
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
        if (this._changeWarp)
        {
          agent.ChangeBehavior(Desire.ActionType.SearchWarpPoint);
          return (TaskStatus) 2;
        }
        Actor partner = agent.Partner;
        if (Object.op_Inequality((Object) partner, (Object) null))
        {
          agent.Partner = (Actor) null;
          partner.Partner = (Actor) null;
          if (partner is AgentActor)
            (partner as AgentActor).BehaviorResources.ChangeMode(Desire.ActionType.Normal);
          else if (partner is MerchantActor)
          {
            MerchantActor merchantActor = partner as MerchantActor;
            merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
          }
        }
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
