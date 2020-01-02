// Decompiled with JetBrains decompiler
// Type: AIProject.StorageIn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class StorageIn : AgentAction
  {
    private Subject<Unit> _onStartChestAction = new Subject<Unit>();
    private Subject<Unit> _onStartAction = new Subject<Unit>();
    protected Subject<Unit> _onActionPlay = new Subject<Unit>();
    protected Subject<Unit> _onEndAction = new Subject<Unit>();
    private Subject<Unit> _onEndChestAction = new Subject<Unit>();
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    private ChestAnimation _chestAnimation;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.StorageIn;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(agent.EventKey, out valueTuple);
      agent.ActionID = (int) valueTuple.Item1;
      ActionPointInfo actionPointInfo1 = agent.TargetInSightActionPoint.GetActionPointInfo(agent);
      agent.Animation.ActionPointInfo = actionPointInfo1;
      ActionPointInfo actionPointInfo2 = actionPointInfo1;
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      agent.PoseID = actionPointInfo2.poseID;
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[agent.ActionID][actionPointInfo2.poseID];
      agent.Animation.LoadEventKeyTable(agent.ActionID, actionPointInfo2.poseID);
      agent.LoadEventItems(playState);
      agent.LoadEventParticles(agent.ActionID, actionPointInfo2.poseID);
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
      Resources instance = Singleton<Resources>.Instance;
      agent.LoadActionFlag(actionPointInfo2.eventID, actionPointInfo2.poseID);
      agent.DeactivateNavMeshAgent();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onStartChestAction, 1), (System.Action<M0>) (_ =>
      {
        if (!Object.op_Inequality((Object) this._chestAnimation, (Object) null))
          return;
        this._chestAnimation.PlayInAnimation();
      }));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onStartAction, 1), (System.Action<M0>) (_ => agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.inFadeOutTime, animInfo.layer)));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onActionPlay, 1), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
      if (animInfo.isLoop)
        this.Agent.SetCurrentSchedule(animInfo.isLoop, (string) valueTuple.Item2, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
      if (!Object.op_Inequality((Object) this.Agent.CurrentPoint, (Object) null))
        return;
      this._chestAnimation = (ChestAnimation) ((Component) this.Agent.CurrentPoint).GetComponent<ChestAnimation>();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Inequality((Object) this._chestAnimation, (Object) null) && this._chestAnimation.PlayingOutAnimation)
        return (TaskStatus) 3;
      if (this._onStartChestAction != null)
        this._onStartChestAction.OnNext(Unit.get_Default());
      if (Object.op_Inequality((Object) this._chestAnimation, (Object) null) && this._chestAnimation.PlayingInAniamtion)
        return (TaskStatus) 3;
      if (this._onStartAction != null)
        this._onStartAction.OnNext(Unit.get_Default());
      if (this.Agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (this.Agent.Animation.AnimInfo.isLoop)
      {
        if (this.Agent.Animation.PlayingActAnimation)
          return (TaskStatus) 3;
        if (this.Agent.Schedule.enabled)
          return (TaskStatus) 3;
        if (this._onEndAction != null)
          this._onEndAction.OnNext(Unit.get_Default());
        if (this.Agent.Animation.PlayingOutAnimation)
          return (TaskStatus) 3;
        this.Complete();
        return (TaskStatus) 2;
      }
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (this.Agent.Animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
      this.OnCompletedStateTask();
      this.Agent.ActivateNavMeshAgent();
      this.Agent.SetActiveOnEquipedItem(true);
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }

    private void OnCompletedStateTask()
    {
      if (Object.op_Inequality((Object) this._chestAnimation, (Object) null))
        this._chestAnimation.PlayOutAnimation();
      AgentActor agent = this.Agent;
      foreach (StuffItem stuffItem in agent.AgentData.ItemList)
        Singleton<Game>.Instance.WorldData.Environment.ItemListInStorage.AddItem(stuffItem);
      agent.AgentData.ItemList.Clear();
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
    }
  }
}
