// Decompiled with JetBrains decompiler
// Type: AIProject.AgentStateAction
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
using UnityEx;

namespace AIProject
{
  public abstract class AgentStateAction : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    [SerializeField]
    protected bool _unchangeParamState;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      if (Object.op_Equality((Object) agent.CurrentPoint, (Object) null))
        return;
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      ActionPointInfo actionPointInfo1 = agent.TargetInSightActionPoint.GetActionPointInfo(agent);
      agent.Animation.ActionPointInfo = actionPointInfo1;
      ActionPointInfo actionPointInfo2 = actionPointInfo1;
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      int eventId = actionPointInfo2.eventID;
      agent.ActionID = eventId;
      int index = eventId;
      int poseId = actionPointInfo2.poseID;
      agent.PoseID = poseId;
      int poseID = poseId;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[index][poseID];
      agent.CurrentPoint.DestroyByproduct(index, poseID);
      ValueTuple<int, string> valueTuple;
      if (AIProject.Definitions.Action.NameTable.TryGetValue(agent.EventKey, out valueTuple) && (int) valueTuple.Item1 != agent.ActionID)
        return;
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(index, poseID, info);
      agent.LoadActionFlag(index, poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ => agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (System.Action<M0>) (_ => this.Complete()));
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, actionPointInfo2.actionName, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      ValueTuple<int, string> valueTuple;
      if (AIProject.Definitions.Action.NameTable.TryGetValue(agent.EventKey, out valueTuple) && (int) valueTuple.Item1 != agent.ActionID)
      {
        agent.ChangeBehavior(Desire.ActionType.Normal);
        return (TaskStatus) 1;
      }
      return Object.op_Equality((Object) agent.CurrentPoint, (Object) null) ? (TaskStatus) 2 : agent.AnimationAgent.OnUpdateActionState();
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
      agent.SetActiveOnEquipedItem(true);
      agent.ClearItems();
      agent.ClearParticles();
    }

    protected virtual void Complete()
    {
      AgentActor agent = this.Agent;
      if (!this._unchangeParamState)
      {
        agent.UpdateStatus(agent.ActionID, agent.PoseID);
        agent.CauseSick();
      }
      this.OnCompletedStateTask();
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
      agent.ActivateNavMeshAgent();
      agent.Animation.EndStates();
    }

    protected virtual void OnCompletedStateTask()
    {
    }
  }
}
