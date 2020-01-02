// Decompiled with JetBrains decompiler
// Type: AIProject.Lesbian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class Lesbian : AgentAction
  {
    private bool _isReleased = true;
    private bool _updatedMotivation;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._isReleased = true;
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Lesbian;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      Actor partner = agent.Partner;
      if (Object.op_Equality((Object) partner, (Object) null))
        return;
      agent.DeactivateNavMeshAgent();
      agent.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      DateActionPointInfo dateActionPointInfo1 = agent.TargetInSightActionPoint.GetDateActionPointInfo(agent);
      agent.Animation.DateActionPointInfo = dateActionPointInfo1;
      DateActionPointInfo dateActionPointInfo2 = dateActionPointInfo1;
      Transform basePointA = ((Component) agent.CurrentPoint).get_transform().FindLoop(dateActionPointInfo2.baseNullNameA)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop1 = ((Component) agent.CurrentPoint).get_transform().FindLoop(dateActionPointInfo2.recoveryNullNameA);
      agent.Animation.RecoveryPoint = loop1?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      Transform basePointB = ((Component) agent.CurrentPoint).get_transform().FindLoop(dateActionPointInfo2.baseNullNameB)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop2 = ((Component) agent.CurrentPoint).get_transform().FindLoop(dateActionPointInfo2.recoveryNullNameB);
      partner.Animation.RecoveryPoint = loop2?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      int eventId = dateActionPointInfo2.eventID;
      agent.ActionID = eventId;
      int actionID = eventId;
      int poseIda = dateActionPointInfo2.poseIDA;
      agent.PoseID = poseIda;
      int poseID1 = poseIda;
      int poseIdb = dateActionPointInfo2.poseIDB;
      partner.PoseID = poseIdb;
      int poseID2 = poseIdb;
      PlayState playState1 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[actionID][poseID1];
      agent.LoadActionFlag(actionID, poseID1);
      if (partner is AgentActor)
        (partner as AgentActor).LoadActionFlag(actionID, poseID2);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[actionID][poseID2];
      HScene.AnimationListInfo info = Singleton<Resources>.Instance.HSceneTable.lstAnimInfo[4][dateActionPointInfo2.poseIDA];
      agent.Animation.BeginIgnoreEvent();
      partner.Animation.BeginIgnoreEvent();
      AssetBundleInfo assetBundleInfo1 = playState1.MainStateInfo.AssetBundleInfo;
      RuntimeAnimatorController rac1 = AssetUtility.LoadAsset<RuntimeAnimatorController>((string) assetBundleInfo1.assetbundle, (string) assetBundleInfo1.asset, string.Empty);
      agent.Animation.SetAnimatorController(rac1);
      AssetBundleInfo assetBundleInfo2 = playState2.MainStateInfo.AssetBundleInfo;
      RuntimeAnimatorController rac2 = AssetUtility.LoadAsset<RuntimeAnimatorController>((string) assetBundleInfo2.assetbundle, (string) assetBundleInfo2.asset, string.Empty);
      partner.Animation.SetAnimatorController(rac2);
      agent.StartLesbianSequence(partner, info);
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(basePointA, false, 0.0f, 0);
      partner.SetStand(basePointB, false, 0.0f, 0);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 5), (System.Action<M0>) (_ =>
      {
        agent.SetStand(basePointA, false, 0.0f, 0);
        partner.SetStand(basePointB, false, 0.0f, 0);
      }));
      agent.UpdateMotivation = true;
      this._isReleased = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.Agent.Partner, (Object) null))
        return (TaskStatus) 1;
      if (this.Agent.LivesLesbianHSequence)
        return (TaskStatus) 3;
      this.OnComplete2();
      return (TaskStatus) 2;
    }

    private void OnComplete()
    {
      AgentActor agent = this.Agent;
      Actor partner = agent.Partner;
      int desireKey = Desire.GetDesireKey(Desire.Type.H);
      agent.SetDesire(desireKey, 0.0f);
      if (partner is AgentActor)
        (partner as AgentActor).SetDesire(desireKey, 0.0f);
      agent.Animation.CrossFadeScreen(-1f);
      agent.SetStand(agent.Animation.RecoveryPoint, false, 0.0f, 0);
      agent.Animation.RecoveryPoint = (Transform) null;
      partner.SetStand(partner.Animation.RecoveryPoint, false, 0.0f, 0);
      partner.Animation.RecoveryPoint = (Transform) null;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      if (partner is AgentActor)
        (partner as AgentActor).UpdateStatus(partner.ActionID, partner.PoseID);
      agent.Partner = (Actor) null;
      partner.Partner = (Actor) null;
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      partner.ActivateNavMeshAgent();
      partner.SetActiveOnEquipedItem(true);
      if (partner is AgentActor)
      {
        Dictionary<int, int> relationShipTable = this.Agent.AgentData.FriendlyRelationShipTable;
        int num;
        if (!relationShipTable.TryGetValue(partner.ID, out num))
          num = 50;
        relationShipTable[partner.ID] = Mathf.Clamp(num + 2, 0, 100);
        agent.ApplySituationResultParameter(24);
        (partner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
      }
      else if (partner is MerchantActor)
      {
        int num1;
        if (!this.Agent.AgentData.FriendlyRelationShipTable.TryGetValue(partner.ID, out num1))
          num1 = 50;
        int num2;
        if (!Singleton<Resources>.Instance.MerchantProfile.ResultAddFriendlyRelationShipTable.TryGetValue(Merchant.ActionType.HWithAgent, out num2))
          num2 = 0;
        this.Agent.AgentData.FriendlyRelationShipTable[partner.ID] = Mathf.Clamp(num1 + num2, 0, 100);
        this.Agent.ApplySituationResultParameter(26);
        MerchantActor merchantActor = partner as MerchantActor;
        merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
        if (Object.op_Equality((Object) agent, (Object) merchantActor.CommandPartner))
          merchantActor.CommandPartner = (Actor) null;
      }
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.TargetInSightActor = (Actor) null;
      agent.EventKey = (EventType) 0;
      agent.Animation.EndIgnoreEvent();
      partner.Animation.EndIgnoreEvent();
      agent.Animation.ResetDefaultAnimatorController();
      partner.Animation.ResetDefaultAnimatorController();
      agent.ResetActionFlag();
      if (!(partner is AgentActor))
        return;
      (partner as AgentActor).ResetActionFlag();
    }

    private void OnComplete2()
    {
      AgentActor agent = this.Agent;
      Actor partner = agent.Partner;
      int desireKey = Desire.GetDesireKey(Desire.Type.H);
      agent.SetDesire(desireKey, 0.0f);
      if (partner is AgentActor)
        (partner as AgentActor).SetDesire(desireKey, 0.0f);
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      if (partner is AgentActor)
        (partner as AgentActor).UpdateStatus(partner.ActionID, partner.PoseID);
      if (partner is AgentActor)
      {
        Dictionary<int, int> relationShipTable = this.Agent.AgentData.FriendlyRelationShipTable;
        int num;
        if (!relationShipTable.TryGetValue(partner.ID, out num))
          num = 50;
        relationShipTable[partner.ID] = Mathf.Clamp(num + 2, 0, 100);
        agent.ApplySituationResultParameter(24);
        (partner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
      }
      else if (partner is MerchantActor)
      {
        int num1;
        if (!this.Agent.AgentData.FriendlyRelationShipTable.TryGetValue(partner.ID, out num1))
          num1 = 50;
        int num2;
        if (!Singleton<Resources>.Instance.MerchantProfile.ResultAddFriendlyRelationShipTable.TryGetValue(Merchant.ActionType.HWithAgent, out num2))
          num2 = 0;
        this.Agent.AgentData.FriendlyRelationShipTable[partner.ID] = Mathf.Clamp(num1 + num2, 0, 100);
        this.Agent.ApplySituationResultParameter(26);
        MerchantActor merchantActor = partner as MerchantActor;
        merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
        if (Object.op_Equality((Object) agent, (Object) merchantActor.CommandPartner))
          merchantActor.CommandPartner = (Actor) null;
      }
      this.OnReleaseProcessing();
    }

    private void OnReleaseProcessing()
    {
      if (this._isReleased)
        return;
      this._isReleased = true;
      AgentActor agent = this.Agent;
      Actor partner = agent.Partner;
      agent.Animation.CrossFadeScreen(-1f);
      agent.SetStand(agent.Animation.RecoveryPoint, false, 0.0f, 0);
      agent.Animation.RecoveryPoint = (Transform) null;
      if (Object.op_Inequality((Object) partner, (Object) null))
      {
        partner.SetStand(partner.Animation.RecoveryPoint, false, 0.0f, 0);
        partner.Animation.RecoveryPoint = (Transform) null;
      }
      agent.Partner = (Actor) null;
      if (Object.op_Inequality((Object) partner, (Object) null))
        partner.Partner = (Actor) null;
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      if (Object.op_Inequality((Object) partner, (Object) null))
      {
        partner.ActivateNavMeshAgent();
        partner.SetActiveOnEquipedItem(true);
      }
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.TargetInSightActor = (Actor) null;
      agent.EventKey = (EventType) 0;
      agent.Animation.EndIgnoreEvent();
      if (Object.op_Inequality((Object) partner, (Object) null))
        partner.Animation.EndIgnoreEvent();
      agent.Animation.ResetDefaultAnimatorController();
      partner.Animation.ResetDefaultAnimatorController();
      agent.ResetActionFlag();
      if (!(partner is AgentActor))
        return;
      (partner as AgentActor).ResetActionFlag();
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      agent.ClearItems();
      agent.ClearParticles();
      agent.StopLesbianSequence();
      agent.UpdateMotivation = false;
    }

    public virtual void OnPause(bool paused)
    {
      AgentActor agent = this.Agent;
      if (paused)
      {
        this._updatedMotivation = paused;
        agent.UpdateMotivation = false;
      }
      else
        agent.UpdateMotivation = this._updatedMotivation;
    }
  }
}
