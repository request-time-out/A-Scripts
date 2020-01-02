// Decompiled with JetBrains decompiler
// Type: AIProject.WashHand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class WashHand : AgentAction
  {
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Wash;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      Resources instance = Singleton<Resources>.Instance;
      PoseKeyPair handWash = instance.AgentProfile.PoseIDTable.HandWash;
      ActionPointInfo actionPointInfo1 = agent.TargetInSightActionPoint.GetActionPointInfo(agent);
      agent.Animation.ActionPointInfo = actionPointInfo1;
      ActionPointInfo actionPointInfo2 = actionPointInfo1;
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      agent.ActionID = handWash.postureID;
      agent.PoseID = handWash.poseID;
      PlayState playState = instance.Animation.AgentActionAnimTable[agent.ActionID][agent.PoseID];
      agent.Animation.LoadEventKeyTable(agent.ActionID, agent.PoseID);
      agent.LoadEventItems(playState);
      agent.LoadEventParticles(agent.ActionID, agent.PoseID);
      agent.Animation.InitializeStates(playState);
      ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
      {
        inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
        inFadeOutTime = playState.MainStateInfo.FadeOutTime,
        outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState.DirectionType,
        endEnableBlend = playState.EndEnableBlend,
        endBlendSec = playState.EndBlendRate,
        isLoop = playState.MainStateInfo.IsLoop,
        loopMinTime = playState.MainStateInfo.LoopMin,
        loopMaxTime = playState.MainStateInfo.LoopMax,
        hasAction = playState.ActionInfo.hasAction,
        layer = playState.MainStateInfo.InStateInfo.StateInfos[0].layer
      };
      agent.Animation.AnimInfo = actorAnimInfo;
      ActorAnimInfo animInfo = actorAnimInfo;
      agent.LoadActionFlag(agent.ActionID, agent.PoseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, playState.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (Action<M0>) (_ => agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (Action<M0>) (_ => this.Complete()));
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, actionPointInfo2.actionName, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AnimationAgent.OnUpdateActionState();
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
    }

    protected virtual void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
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
        agent.CurrentPoint.CreateByproduct(agent.ActionID, agent.PoseID);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }
  }
}
