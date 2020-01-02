// Decompiled with JetBrains decompiler
// Type: AIProject.LookAround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class LookAround : AgentAction
  {
    [SerializeField]
    private int lookNum = 1;
    [SerializeField]
    private float aroundAngle = 45f;
    private float duration;
    private float elapsedTime;
    private int layer;
    private bool inEnableFade;
    private float inFadeTime;
    private bool outEnableFade;
    private float outFadeTime;
    private Subject<Unit> onEndEvent;
    private Action onStartEvent;
    private bool isSick;
    private bool isCold;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.StopNavMeshAgent();
      this.Agent.AnimalFovAngleOffsetY = 0.0f;
      int id = this.Agent.AgentData.SickState.ID;
      PlayState playState1 = new PlayState();
      Dictionary<int, Dictionary<int, PlayState>> agentActionAnimTable = Singleton<Resources>.Instance.Animation.AgentActionAnimTable;
      AgentProfile.PoseIDCollection poseIdTable = Singleton<Resources>.Instance.AgentProfile.PoseIDTable;
      this.isSick = -1 < id;
      this.isCold = false;
      PlayState playState2;
      if (this.isSick)
      {
        if (id == 0)
        {
          this.isCold = true;
          PoseKeyPair coughId = poseIdTable.CoughID;
          playState2 = agentActionAnimTable[coughId.postureID][coughId.poseID];
        }
        else
        {
          PoseKeyPair[] normalIdList = poseIdTable.NormalIDList;
          PoseKeyPair element = normalIdList.GetElement<PoseKeyPair>(Random.Range(0, normalIdList.Length));
          playState2 = agentActionAnimTable[element.postureID][element.poseID];
        }
      }
      else
      {
        List<PoseKeyPair> poseKeyPairList = ListPool<PoseKeyPair>.Get();
        poseKeyPairList.AddRange((IEnumerable<PoseKeyPair>) poseIdTable.NormalIDList);
        switch (Singleton<Manager.Map>.Instance.Simulator.Weather)
        {
          case Weather.Clear:
          case Weather.Cloud1:
            poseKeyPairList.Add(poseIdTable.ClearPoseID);
            break;
        }
        PoseKeyPair element = poseKeyPairList.GetElement<PoseKeyPair>(Random.Range(0, poseKeyPairList.Count));
        ListPool<PoseKeyPair>.Release(poseKeyPairList);
        playState2 = agentActionAnimTable[element.postureID][element.poseID];
      }
      this.layer = playState2.Layer;
      this.inEnableFade = playState2.MainStateInfo.InStateInfo.EnableFade;
      this.inFadeTime = playState2.MainStateInfo.InStateInfo.FadeSecond;
      this.Agent.Animation.OnceActionStates.Clear();
      if (!playState2.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info stateInfo in playState2.MainStateInfo.InStateInfo.StateInfos)
          this.Agent.Animation.OnceActionStates.Add(stateInfo);
      }
      this.Agent.Animation.OutStates.Clear();
      if (!playState2.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info stateInfo in playState2.MainStateInfo.OutStateInfo.StateInfos)
          this.Agent.Animation.OutStates.Enqueue(stateInfo);
      }
      this.duration = Singleton<Resources>.Instance.AgentProfile.StandDurationMinMax.RandomValue;
      this.onEndEvent = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this.onEndEvent, 1), (Action<M0>) (_ => this.Agent.Animation.PlayOutAnimation(this.outEnableFade, this.outFadeTime, this.layer)));
      this.onStartEvent = (Action) (() => this.Agent.Animation.PlayOnceActionAnimation(this.inEnableFade, this.inFadeTime, this.layer));
    }

    public virtual TaskStatus OnUpdate()
    {
      this.elapsedTime += Time.get_deltaTime();
      if ((double) this.elapsedTime < (double) this.duration)
        return (TaskStatus) 3;
      if (this.onStartEvent != null)
      {
        this.onStartEvent();
        this.onStartEvent = (Action) null;
      }
      if (this.Agent.Animation.PlayingOnceActionAnimation)
      {
        if (!this.isCold)
        {
          AnimatorStateInfo animatorStateInfo = this.Agent.Animation.Animator.GetCurrentAnimatorStateInfo(0);
          this.Agent.AnimalFovAngleOffsetY = Mathf.Sin(((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() * 360f * (float) this.lookNum * ((float) Math.PI / 180f)) * this.aroundAngle;
        }
        return (TaskStatus) 3;
      }
      if (this.onEndEvent != null)
        this.onEndEvent.OnNext(Unit.get_Default());
      return this.Agent.Animation.PlayingOutAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.onEndEvent = (Subject<Unit>) null;
      this.Agent.Animation.StopOnceActionAnimCoroutine();
      this.Agent.Animation.StopOutAnimCoroutine();
      this.Agent.AnimalFovAngleOffsetY = 0.0f;
    }
  }
}
