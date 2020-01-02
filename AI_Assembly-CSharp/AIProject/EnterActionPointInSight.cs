// Decompiled with JetBrains decompiler
// Type: AIProject.EnterActionPointInSight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterActionPointInSight : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      Dictionary<int, CollisionState> collisionStateTable = agent.ActionPointCollisionStateTable;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      foreach (ActionPoint searchTarget in agent.SearchTargets)
      {
        CollisionState collisionState;
        if (collisionStateTable.TryGetValue(searchTarget.InstanceID, out collisionState) && collisionState == CollisionState.Enter)
          toRelease.Add(searchTarget);
      }
      if (toRelease.Count > 0)
      {
        List<ActionPoint> actionPointList = ListPool<ActionPoint>.Get();
        foreach (ActionPoint actionPoint in toRelease)
        {
          if (actionPoint.IsNeutralCommand)
            actionPointList.Add(actionPoint);
        }
        Desire.Type requestedDesire = agent.RequestedDesire;
        EventType type = (EventType) 0;
        foreach (ValueTuple<EventType, Desire.Type> valuePair in Desire.ValuePairs)
        {
          if ((Desire.Type) valuePair.Item2 == requestedDesire)
          {
            type = (EventType) valuePair.Item1;
            break;
          }
        }
        ActionPoint point = (ActionPoint) null;
        foreach (ActionPoint actionPoint in actionPointList)
        {
          if (Object.op_Inequality((Object) agent.Partner, (Object) null))
          {
            if (actionPoint.AgentDateEventType.Contains(type))
              point = actionPoint;
          }
          else if (actionPoint.AgentEventType.Contains(type))
            point = actionPoint;
        }
        if (Object.op_Equality((Object) point, (Object) null))
        {
          point = actionPointList.GetElement<ActionPoint>(Random.Range(0, actionPointList.Count));
          if (Object.op_Equality((Object) point, (Object) null))
          {
            ListPool<ActionPoint>.Release(actionPointList);
            ListPool<ActionPoint>.Release(toRelease);
            return (TaskStatus) 1;
          }
        }
        ListPool<ActionPoint>.Release(actionPointList);
        if (Debug.get_isDebugBuild())
        {
          if (Object.op_Inequality((Object) agent.Partner, (Object) null))
            Debug.Log((object) string.Format("{0} :: Enter Point In Sight: {1} <{2}>", (object) ((Object) ((Component) agent).get_gameObject()).get_name(), (object) ((Object) point).get_name(), (object) point.AgentDateEventType));
          else
            Debug.Log((object) string.Format("{0} :: Enter Point In Sight: {1} <{2}>", (object) ((Object) ((Component) agent).get_gameObject()).get_name(), (object) ((Object) point).get_name(), (object) point.AgentEventType));
        }
        if (requestedDesire == Desire.Type.Bath && type == EventType.DressIn && (double) agent.ChaControl.fileGameInfo.flavorState[2] < (double) Singleton<Resources>.Instance.StatusProfile.CanDressBorder)
          type = EventType.Bath;
        if (type == EventType.Eat)
        {
          StuffItem carryingItem = agent.AgentData.CarryingItem;
          AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
          ItemIDKeyPair[] canStandEatItems = Singleton<Resources>.Instance.AgentProfile.CanStandEatItems;
          bool flag = false;
          foreach (ItemIDKeyPair itemIdKeyPair in canStandEatItems)
          {
            if (carryingItem.CategoryID == itemIdKeyPair.categoryID && carryingItem.ID == itemIdKeyPair.itemID)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            PoseKeyPair eatDeskId = agentProfile.PoseIDTable.EatDeskID;
            PoseKeyPair eatChairId = agentProfile.PoseIDTable.EatChairID;
            ActionPointInfo outInfo;
            if (!point.FindAgentActionPointInfo(EventType.Eat, eatDeskId.poseID, out outInfo) && !point.FindAgentActionPointInfo(EventType.Eat, eatChairId.poseID, out outInfo))
              return (TaskStatus) 1;
          }
          else
          {
            PoseKeyPair eatDishId = agentProfile.PoseIDTable.EatDishID;
            if (!point.FindAgentActionPointInfo(EventType.Eat, eatDishId.poseID, out ActionPointInfo _))
              return (TaskStatus) 1;
          }
        }
        switch (agent.AgentController.GetPermission(point))
        {
          case AgentController.PermissionStatus.Prohibition:
            Debug.Log((object) string.Format("目的地落選: {0}", (object) ((Object) point).get_name()));
            break;
          case AgentController.PermissionStatus.Permission:
            Debug.Log((object) string.Format("目的地当選: {0}", (object) ((Object) point).get_name()));
            if (type == (EventType) 0)
              Debug.LogError((object) string.Format("EventType該当なし: {0}", (object) requestedDesire));
            agent.EventKey = type;
            agent.TargetInSightActionPoint = point;
            agent.RuntimeDesire = agent.RequestedDesire;
            break;
        }
      }
      ListPool<ActionPoint>.Release(toRelease);
      if (!Object.op_Inequality((Object) agent.TargetInSightActionPoint, (Object) null))
        return (TaskStatus) 1;
      agent.ClearReservedNearActionWaypoints();
      agent.AbortActionPatrol();
      return (TaskStatus) 2;
    }

    public virtual void OnBehaviorComplete()
    {
      MovementUtility.ClearCache();
    }
  }
}
