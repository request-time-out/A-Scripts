// Decompiled with JetBrains decompiler
// Type: AIProject.StandAction
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
  public class StandAction : AgentAction
  {
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;
    private PoseKeyPair? _poseInfo;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.ElectNextPoint();
      this._poseInfo = new PoseKeyPair?();
      if (Random.Range(0, 2) == 0 || agent.PrevMode == Desire.ActionType.Encounter)
        return;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      float num1 = agent.AgentData.StatsTable[0];
      float num2 = agent.AgentData.StatsTable[2];
      int desireKey1 = Desire.GetDesireKey(Desire.Type.Bath);
      float? desire1 = agent.GetDesire(desireKey1);
      int desireKey2 = Desire.GetDesireKey(Desire.Type.Sleep);
      float? desire2 = agent.GetDesire(desireKey2);
      if (agent.AgentData.SickState.ID == 0)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.CoughID);
      else if ((!desire2.HasValue ? 0 : ((double) desire2.GetValueOrDefault() >= 70.0 ? 1 : 0)) != 0)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.YawnID);
      else if ((!desire1.HasValue ? 0 : ((double) desire1.GetValueOrDefault() >= 70.0 ? 1 : 0)) != 0)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.GrossID);
      else if ((double) num1 <= (double) agentProfile.ColdTempBorder)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.ColdPoseID);
      else if ((double) num1 >= (double) agentProfile.HotTempBorder)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.HotPoseID);
      else if ((double) num2 <= 0.0)
        this._poseInfo = new PoseKeyPair?(agentProfile.PoseIDTable.HungryID);
      if (!this._poseInfo.HasValue)
        return;
      PoseKeyPair poseKeyPair = this._poseInfo.Value;
      agent.ActionID = poseKeyPair.postureID;
      agent.PoseID = poseKeyPair.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(poseKeyPair.postureID, poseKeyPair.poseID, info);
      agent.LoadActionFlag(poseKeyPair.postureID, poseKeyPair.poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.RecoveryPoint = (Transform) null;
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
      agent.SetCurrentSchedule(animInfo.isLoop, "立ちアクション", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      return !this._poseInfo.HasValue ? (TaskStatus) 2 : this.Agent.AnimationAgent.OnUpdateActionState();
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
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      AgentActor agentActor = agent;
      int num1 = -1;
      agent.PoseID = num1;
      int num2 = num1;
      agentActor.ActionID = num2;
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
    }
  }
}
