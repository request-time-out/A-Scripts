// Decompiled with JetBrains decompiler
// Type: AIProject.Pee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Pee : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      PoseKeyPair peeId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.PeeID;
      agent.ActionID = peeId.postureID;
      agent.PoseID = peeId.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[peeId.postureID][peeId.poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(peeId.postureID, peeId.poseID, info);
      agent.LoadActionFlag(peeId.postureID, peeId.poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => this.Agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), 1), (System.Action<M0>) (_ => this.Complete()));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, "おもらし", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
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
      this.Agent.ActivateNavMeshAgent();
      this.Agent.SetActiveOnEquipedItem(true);
      this.Agent.ClearItems();
      this.Agent.ClearParticles();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      int desireKey = Desire.GetDesireKey(Desire.Type.Toilet);
      agent.SetDesire(desireKey, 0.0f);
      agent.ApplySituationResultParameter(29);
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
    }
  }
}
