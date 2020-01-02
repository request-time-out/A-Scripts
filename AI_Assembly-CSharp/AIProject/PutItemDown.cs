// Decompiled with JetBrains decompiler
// Type: AIProject.PutItemDown
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
  public class PutItemDown : AgentAction
  {
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.SetActiveOnEquipedItem(false);
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.EventKey = EventType.PutItem;
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
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(index, poseID, info);
      agent.LoadActionFlag(index, poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (Action<M0>) (_ => this.Complete()));
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, actionPointInfo2.actionName, animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onActionPlayDisposable != null)
        this._onActionPlayDisposable.Dispose();
      if (this._onCompleteActionDisposable == null)
        return;
      this._onCompleteActionDisposable.Dispose();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AnimationAgent.OnUpdateActionState();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.ActivateNavMeshAgent();
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      agent.CurrentPoint.SetActiveMapItemObjs(true);
      agent.ReleaseEquipedEventItem();
      agent.EventKey = (EventType) 0;
    }
  }
}
