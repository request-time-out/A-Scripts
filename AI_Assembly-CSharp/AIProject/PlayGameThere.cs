// Decompiled with JetBrains decompiler
// Type: AIProject.PlayGameThere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class PlayGameThere : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    [SerializeField]
    private bool _unchangeParamState;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Play;
      ((Task) this).OnStart();
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      List<PoseKeyPair> poseKeyPairList = ListPool<PoseKeyPair>.Get();
      AgentProfile.PoseIDCollection poseIdTable = Singleton<Resources>.Instance.AgentProfile.PoseIDTable;
      poseKeyPairList.AddRange((IEnumerable<PoseKeyPair>) poseIdTable.PlayGameStandIDList);
      Weather weather = Singleton<Manager.Map>.Instance.Simulator.Weather;
      if (agent.AreaType == MapArea.AreaType.Normal)
      {
        poseKeyPairList.AddRange((IEnumerable<PoseKeyPair>) Singleton<Resources>.Instance.AgentProfile.PoseIDTable.PlayGameStandOutdoorIDList);
        if (weather == Weather.Cloud1 || weather == Weather.Cloud2)
          poseKeyPairList.Add(Singleton<Resources>.Instance.AgentProfile.PoseIDTable.ClearPoseID);
      }
      PoseKeyPair element = poseKeyPairList.GetElement<PoseKeyPair>(Random.Range(0, poseKeyPairList.Count));
      ListPool<PoseKeyPair>.Release(poseKeyPairList);
      agent.ActionID = element.postureID;
      agent.ActionID = element.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[element.postureID][element.poseID];
      agent.Animation.RecoveryPoint = (Transform) null;
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(element.postureID, element.poseID, info);
      agent.LoadActionFlag(element.postureID, element.poseID);
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ => agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (System.Action<M0>) (_ => this.Complete()));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, "その場で遊ぶ", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
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
      agent.ChangeDynamicNavMeshAgentAvoidance();
      agent.SetActiveOnEquipedItem(true);
      agent.ClearItems();
      agent.ClearParticles();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      if (!agent.Animation.AnimInfo.outEnableBlend)
        agent.Animation.CrossFadeScreen(-1f);
      agent.Animation.RefsActAnimInfo = true;
      if (!this._unchangeParamState)
      {
        agent.UpdateStatus(agent.ActionID, agent.PoseID);
        int desireKey = Desire.GetDesireKey(Desire.Type.Game);
        agent.SetDesire(desireKey, 0.0f);
        agent.CauseSick();
      }
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
      agent.Animation.EndStates();
    }
  }
}
