// Decompiled with JetBrains decompiler
// Type: AIProject.Greet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using ReMotion;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Greet : AgentAction
  {
    private float _inFadeTime = 0.1f;
    private float _outFadeTime = 0.1f;
    private int _layer = -1;
    private Subject<Unit> _onEnd = new Subject<Unit>();
    private bool _inEnableFade;
    private bool _outEnableFade;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      if (agent.EquipedItem != null)
        return;
      ((Task) this).OnStart();
      agent.StateType = AIProject.Definitions.State.Type.Greet;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      this.StartActionAnimation();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEnd, 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(this._outEnableFade, this._outFadeTime, this._layer);
      }));
      Quaternion rotation = this.Agent.Rotation;
      Vector3 vector3 = Vector3.op_Subtraction(this.Agent.TargetInSightActor.Position, this.Agent.Position);
      vector3.y = (__Null) 0.0;
      Quaternion forwardRotation = Quaternion.LookRotation(Vector3.Normalize(vector3));
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, false), false), (System.Action<M0>) (x => this.Agent.Rotation = Quaternion.Slerp(rotation, forwardRotation, ((TimeInterval<float>) ref x).get_Value())));
      agent.AgentData.Greeted = true;
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.EquipedItem != null)
        return (TaskStatus) 2;
      if (agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (this._onEnd != null)
        this._onEnd.OnNext(Unit.get_Default());
      if (agent.Animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      agent.TargetInSightActor = (Actor) null;
      return (TaskStatus) 2;
    }

    private void StartActionAnimation()
    {
      AgentActor agent = this.Agent;
      PoseKeyPair greetPoseId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.GreetPoseID;
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[greetPoseId.postureID][greetPoseId.poseID];
      agent.Animation.LoadEventKeyTable(greetPoseId.postureID, greetPoseId.poseID);
      this._layer = playState.Layer;
      this._inEnableFade = playState.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeTime = playState.MainStateInfo.InStateInfo.FadeSecond;
      agent.Animation.InitializeStates(playState.MainStateInfo.InStateInfo.StateInfos, playState.MainStateInfo.OutStateInfo.StateInfos, playState.MainStateInfo.AssetBundleInfo);
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(this._inEnableFade, this._inFadeTime, 1f, this._layer);
    }
  }
}
