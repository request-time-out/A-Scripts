// Decompiled with JetBrains decompiler
// Type: AIProject.Ovation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Ovation : AgentAction
  {
    private Subject<Unit> _onEndTurn;
    private IDisposable _onEndTurnDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      PoseKeyPair ovationId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.OvationID;
      agent.ActionID = ovationId.postureID;
      agent.PoseID = ovationId.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[ovationId.postureID][ovationId.poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(ovationId.postureID, ovationId.poseID, info);
      agent.LoadActionFlag(ovationId.postureID, ovationId.poseID);
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.Animation.StopAllAnimCoroutine();
      PlayerActor pPlayer = agent.Partner as PlayerActor;
      agent.Animation.PlayTurnAnimation(pPlayer.Position, 1f, (PlayState.AnimStateInfo) null, false);
      this._onEndTurnDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) (this._onEndTurn = new Subject<Unit>()), 1), (Action<M0>) (_ =>
      {
        if (Object.op_Inequality((Object) pPlayer, (Object) null))
        {
          agent.ChaControl.ChangeLookEyesTarget(1, ((Component) pPlayer.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
          agent.ChaControl.ChangeLookEyesPtn(1);
          agent.ChaControl.ChangeLookNeckTarget(1, ((Component) pPlayer.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 0.8f);
          agent.ChaControl.ChangeLookNeckPtn(1, 1f);
        }
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      }));
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), 1), (Action<M0>) (_ => this.Complete()));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, "拍手", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      if (this._onEndTurnDisposable != null)
        this._onEndTurnDisposable.Dispose();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onCompleteActionDisposable != null)
        this._onCompleteActionDisposable.Dispose();
      AgentActor agent = this.Agent;
      agent.ChangeDynamicNavMeshAgentAvoidance();
      agent.SetActiveOnEquipedItem(true);
      agent.ChaControl.ChangeLookEyesPtn(0);
      agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      agent.ChaControl.ChangeLookNeckPtn(3, 1f);
      agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      PlayerActor partner = agent.Partner as PlayerActor;
      if (!Object.op_Inequality((Object) partner, (Object) null) || !partner.OldEnabledHoldingHand)
        return;
      ((Behaviour) partner.HandsHolder).set_enabled(true);
      partner.OldEnabledHoldingHand = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.Animation.PlayingTurnAnimation)
        return (TaskStatus) 3;
      this._onEndTurn.OnNext(Unit.get_Default());
      return agent.AnimationAgent.OnUpdateActionState();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
    }
  }
}
