// Decompiled with JetBrains decompiler
// Type: AIProject.ExistsDesiredActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class ExistsDesiredActionPoint : AgentConditional
  {
    private static NavMeshPath _navMeshPath;
    [SerializeField]
    private bool _checkFollowType;
    [SerializeField]
    private EventType _eventType;

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      Chunk chunk;
      Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(agent.ChunkID, out chunk);
      Vector3 position = agent.Position;
      List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
      ExistsDesiredActionPoint.CreateList(agent, chunk.AppendActionPoints, actionPointList, this._eventType, this._checkFollowType);
      bool flag = !actionPointList.IsNullOrEmpty<ActionPoint>();
      if (!flag)
      {
        ExistsDesiredActionPoint.CreateList(agent, chunk.ActionPoints, actionPointList, this._eventType, this._checkFollowType);
        flag = !actionPointList.IsNullOrEmpty<ActionPoint>();
      }
      ListPool<ActionPoint>.Release(actionPointList);
      return flag ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    private static void CreateList(
      AgentActor agent,
      List<ActionPoint> source,
      List<ActionPoint> destination,
      EventType eventType,
      bool isFollow)
    {
      int searchCount = Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount;
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
                  Dictionary<int, Environment.SearchActionInfo> searchActionLockTable = agent.AgentData.SearchActionLockTable;
                  Environment.SearchActionInfo searchActionInfo1;
                  if (!searchActionLockTable.TryGetValue(registerId, out searchActionInfo1))
                  {
                    Environment.SearchActionInfo searchActionInfo2 = new Environment.SearchActionInfo();
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
            }
            if (ExistsDesiredActionPoint._navMeshPath == null)
              ExistsDesiredActionPoint._navMeshPath = new NavMeshPath();
            if (agent.NavMeshAgent.CalculatePath(actionPoint1.LocatedPosition, ExistsDesiredActionPoint._navMeshPath) && ExistsDesiredActionPoint._navMeshPath.get_status() == null)
              destination.Add(actionPoint1);
          }
        }
      }
      DictionaryPool<int, bool>.Release(toRelease);
    }
  }
}
