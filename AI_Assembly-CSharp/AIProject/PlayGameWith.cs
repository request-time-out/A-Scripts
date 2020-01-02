// Decompiled with JetBrains decompiler
// Type: AIProject.PlayGameWith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class PlayGameWith : AgentAction
  {
    protected Subject<Unit> _onActionPlay = new Subject<Unit>();
    protected Subject<Unit> _onEndAction = new Subject<Unit>();

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Play;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      DateActionPointInfo outInfo;
      agent.CurrentPoint.TryGetAgentDateActionPointInfo(EventType.Play, out outInfo);
      Transform t1 = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      Transform t2 = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameB)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop1 = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA);
      agent.Animation.RecoveryPoint = loop1?.get_transform();
      GameObject loop2 = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameB);
      agent.Animation.RecoveryPoint = loop2?.get_transform();
      int eventId = outInfo.eventID;
      agent.ActionID = eventId;
      int eventID = eventId;
      int poseIda = outInfo.poseIDA;
      agent.PoseID = poseIda;
      int poseID1 = poseIda;
      PlayState playState1 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eventID][poseID1];
      agent.Animation.LoadEventKeyTable(eventID, poseID1);
      agent.LoadEventItems(playState1);
      agent.LoadEventParticles(eventID, poseID1);
      agent.Animation.InitializeStates(playState1);
      AgentActor partner = agent.Partner as AgentActor;
      partner.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      partner.Mode = Desire.ActionType.Idle;
      partner.ActionID = eventID;
      int poseIdb = outInfo.poseIDB;
      partner.PoseID = poseIdb;
      int poseID2 = poseIdb;
      partner.Animation.LoadEventKeyTable(eventID, poseID2);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eventID][poseID2];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(eventID, poseID2);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        inFadeOutTime = playState1.MainStateInfo.FadeOutTime,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        endEnableBlend = playState1.EndEnableBlend,
        endBlendSec = playState1.EndBlendRate,
        isLoop = playState1.MainStateInfo.IsLoop,
        loopMinTime = playState1.MainStateInfo.LoopMin,
        loopMaxTime = playState1.MainStateInfo.LoopMax,
        hasAction = playState1.ActionInfo.hasAction,
        loopStateName = playState1.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState1.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName,
        randomCount = playState1.ActionInfo.randomCount,
        oldNormalizedTime = 0.0f,
        layer = playState1.MainStateInfo.InStateInfo.StateInfos[0].layer
      };
      agent.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo animInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop,
        endEnableBlend = playState1.EndEnableBlend,
        endBlendSec = playState1.EndBlendRate
      };
      partner.Animation.AnimInfo = actorAnimInfo2;
      ActorAnimInfo actorAnimInfo3 = actorAnimInfo2;
      agent.DisableActionFlag();
      partner.DisableActionFlag();
      agent.DeactivateNavMeshAgent();
      agent.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, playState1.MainStateInfo.FadeOutTime, animInfo.layer);
      agent.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, animInfo.layer);
      partner.Animation.PlayInAnimation(actorAnimInfo3.inEnableBlend, actorAnimInfo3.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo3.layer);
      partner.SetStand(t2, playState2.MainStateInfo.InStateInfo.EnableFade, playState2.MainStateInfo.InStateInfo.FadeSecond, actorAnimInfo3.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
        partner.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) this._onActionPlay, (System.Action<M0>) (_ =>
        {
          agent.Animation.PlayActionAnimation(animInfo.layer);
          partner.Animation.PlayActionAnimation(animInfo.layer);
        }));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, outInfo.actionName, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      ActorAnimInfo animInfo = this.Agent.Animation.AnimInfo;
      if (animInfo.isLoop)
      {
        if (this.Agent.Animation.PlayingActAnimation)
          return (TaskStatus) 3;
        if (!this.Agent.Schedule.enabled)
        {
          if (this._onEndAction != null)
            this._onEndAction.OnNext(Unit.get_Default());
          if (this.Agent.Animation.PlayingOutAnimation)
            return (TaskStatus) 3;
          this.Complete();
          return (TaskStatus) 2;
        }
        AnimatorStateInfo animatorStateInfo = this.Agent.Animation.Animator.GetCurrentAnimatorStateInfo(0);
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName(animInfo.loopStateName) && (double) (((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() - animInfo.oldNormalizedTime) > 1.0)
        {
          animInfo.oldNormalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
          if (Random.Range(0, animInfo.randomCount) == 0)
          {
            if (this._onActionPlay != null)
              this._onActionPlay.OnNext(Unit.get_Default());
            animInfo.oldNormalizedTime = 0.0f;
          }
        }
        return (TaskStatus) 3;
      }
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (this.Agent.Animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      this.Agent.ActivateNavMeshAgent();
      this.Agent.SetActiveOnEquipedItem(true);
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      AgentActor partner = agent.Partner as AgentActor;
      ActorAnimInfo animInfo1 = agent.Animation.AnimInfo;
      if (!animInfo1.outEnableBlend)
        agent.Animation.CrossFadeScreen(-1f);
      agent.SetStand(agent.Animation.RecoveryPoint, animInfo1.endEnableBlend, animInfo1.endBlendSec, animInfo1.directionType);
      agent.Animation.RefsActAnimInfo = true;
      partner.SetStand(partner.Animation.RecoveryPoint, animInfo1.endEnableBlend, animInfo1.endBlendSec, animInfo1.directionType);
      partner.Animation.RefsActAnimInfo = true;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      partner.UpdateStatus(partner.ActionID, partner.PoseID);
      agent.CauseSick();
      agent.ApplySituationResultParameter(28);
      partner.ApplySituationResultParameter(28);
      int desireKey1 = Desire.GetDesireKey(Desire.Type.Game);
      agent.SetDesire(desireKey1, 0.0f);
      partner.SetDesire(desireKey1, 0.0f);
      int desireKey2 = Desire.GetDesireKey(Desire.Type.Lonely);
      agent.SetDesire(desireKey2, 0.0f);
      partner.SetDesire(desireKey2, 0.0f);
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      partner.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      agent.CurrentPoint.SetActiveMapItemObjs(true);
      agent.Partner = (Actor) null;
      partner.Partner = (Actor) null;
      partner.ActivateNavMeshAgent();
      partner.SetActiveOnEquipedItem(true);
      ActorAnimInfo animInfo2 = partner.Animation.AnimInfo;
      partner.BehaviorResources.ChangeMode(Desire.ActionType.Normal);
      partner.Mode = Desire.ActionType.Normal;
      agent.TargetInSightActor = (Actor) null;
      agent.CurrentPoint.ReleaseSlot((Actor) this.Agent);
      agent.CurrentPoint = (ActionPoint) null;
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = this.Agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }
  }
}
