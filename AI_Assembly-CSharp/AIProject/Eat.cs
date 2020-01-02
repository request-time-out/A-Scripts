// Decompiled with JetBrains decompiler
// Type: AIProject.Eat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Eat : AgentAction
  {
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Eat;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      StuffItem carryingItem = agent.AgentData.CarryingItem;
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
      ActionPointInfo outInfo;
      if (flag)
      {
        PoseKeyPair eatDeskId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.EatDeskID;
        PoseKeyPair eatChairId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.EatChairID;
        if (agent.TargetInSightActionPoint.FindAgentActionPointInfo(EventType.Eat, eatDeskId.poseID, out outInfo))
          agent.Animation.ActionPointInfo = outInfo;
        else if (agent.TargetInSightActionPoint.FindAgentActionPointInfo(EventType.Eat, eatChairId.poseID, out outInfo))
          agent.Animation.ActionPointInfo = outInfo;
      }
      else
      {
        PoseKeyPair eatDishId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.EatDishID;
        if (agent.TargetInSightActionPoint.FindAgentActionPointInfo(EventType.Eat, eatDishId.poseID, out outInfo))
          agent.Animation.ActionPointInfo = outInfo;
      }
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      int eventId = outInfo.eventID;
      agent.ActionID = eventId;
      int index = eventId;
      int poseId = outInfo.poseID;
      agent.PoseID = poseId;
      int poseID = poseId;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[index][poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(index, poseID, info);
      Dictionary<int, int> dictionary;
      int num;
      ActionItemInfo eventItemInfo;
      if (Singleton<Resources>.Instance.Map.FoodEventItemList.TryGetValue(carryingItem.CategoryID, out dictionary) && dictionary.TryGetValue(carryingItem.ID, out num) && Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(num, out eventItemInfo))
      {
        LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
        string parentName = !flag ? locomotionProfile.RootParentName : locomotionProfile.RightHandParentName;
        GameObject gameObject = agent.LoadEventItem(num, parentName, false, eventItemInfo);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          foreach (Renderer componentsInChild in (Renderer[]) gameObject.GetComponentsInChildren<Renderer>(true))
            componentsInChild.set_enabled(true);
        }
      }
      agent.LoadActionFlag(index, poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (System.Action<M0>) (_ => this.Complete()));
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, outInfo.actionName, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, true);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AnimationAgent.OnUpdateActionState();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
      int desireKey = Desire.GetDesireKey(Desire.Type.Eat);
      agent.SetDesire(desireKey, 0.0f);
      agent.ApplyFoodParameter(agent.AgentData.CarryingItem);
      agent.AgentData.CarryingItem = (StuffItem) null;
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onActionPlayDisposable != null)
        this._onActionPlayDisposable.Dispose();
      if (this._onCompleteActionDisposable != null)
        this._onCompleteActionDisposable.Dispose();
      AgentActor agent = this.Agent;
      agent.ClearItems();
      agent.ClearParticles();
    }
  }
}
