// Decompiled with JetBrains decompiler
// Type: AIProject.PhotoPose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Player;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class PhotoPose : AgentAction
  {
    private int _prevPriority = 50;
    private AIProject.Definitions.State.Type _prevStateType = AIProject.Definitions.State.Type.Normal;
    private ValueTuple<PoseKeyPair, bool> _poseInfo = (ValueTuple<PoseKeyPair, bool>) null;
    private float _loopTime = float.MaxValue;
    private int _lastPoseID = -1;
    private ShotType _prevShotType = ShotType.PointOfView;
    private PlayerActor _player;
    private AgentActor _agent;
    private bool _lookCamera;
    private float _loopCounter;
    private ReadOnlyDictionary<int, ValueTuple<PoseKeyPair, bool>> _poseStateTable;
    private List<int> _poseIDList;
    private ActorCameraControl _playerCameraCon;
    private Subject<Unit> _onEndAction;
    private Subject<Unit> _poseOutAnimAction;
    private Subject<Unit> _poseReplayAnimAction;
    private Subject<Unit> _poseLoopEndAnimAction;
    private Func<AgentActor, bool> _isWait;
    private bool _isFadeOut;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._agent.RuntimeMotivationInPhoto = this._agent.AgentData.StatsTable[5];
      this._prevStateType = this._agent.StateType;
      this._agent.StateType = AIProject.Definitions.State.Type.Immobility;
      this._poseIDList = ListPool<int>.Get();
      NavMeshAgent navMeshAgent = this._agent.NavMeshAgent;
      navMeshAgent.set_isStopped(true);
      this._prevPriority = navMeshAgent.get_avoidancePriority();
      navMeshAgent.set_avoidancePriority(Singleton<Resources>.Instance.AgentProfile.AvoidancePriorityStationary);
      this._player = Singleton<Manager.Map>.Instance.Player;
      this._playerCameraCon = this._player.CameraControl;
      this._prevShotType = this._playerCameraCon.ShotType;
      this._isFadeOut = false;
      this._poseStateTable = (ReadOnlyDictionary<int, ValueTuple<PoseKeyPair, bool>>) null;
      this._poseInfo = new ValueTuple<PoseKeyPair, bool>(new PoseKeyPair(), false);
      Dictionary<int, ValueTuple<PoseKeyPair, bool>> source;
      if (Singleton<Resources>.Instance.Animation.AgentGravurePoseTable.TryGetValue(this._agent.ChaControl.fileParam.personality, out source) && !source.IsNullOrEmpty<int, ValueTuple<PoseKeyPair, bool>>())
      {
        this._poseStateTable = new ReadOnlyDictionary<int, ValueTuple<PoseKeyPair, bool>>((IDictionary<int, ValueTuple<PoseKeyPair, bool>>) source);
        this._poseIDList.AddRange((IEnumerable<int>) this._poseStateTable.get_Keys());
        this._lastPoseID = this._poseIDList[Random.Range(0, this._poseIDList.Count)];
        this._poseStateTable.TryGetValue(this._lastPoseID, ref this._poseInfo);
      }
      this._poseOutAnimAction = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this._poseOutAnimAction, (Component) this._agent), 1), (System.Action<M0>) (_ =>
      {
        this._isFadeOut = true;
        this.PlayOutAnimation(this._poseInfo);
        this._isWait = (Func<AgentActor, bool>) (actor => actor.Animation.PlayingOutAnimation);
      }));
      this._poseReplayAnimAction = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this._poseReplayAnimAction, (Component) this._agent), (System.Action<M0>) (_ =>
      {
        if (this._poseIDList.IsNullOrEmpty<int>())
        {
          this._poseOutAnimAction.OnNext(Unit.get_Default());
        }
        else
        {
          if (this._poseIDList.Count == 1)
          {
            this._agent.ClearItems();
            this.PlayInAnimation(this._poseInfo);
          }
          else
          {
            List<int> toRelease = ListPool<int>.Get();
            toRelease.AddRange((IEnumerable<int>) this._poseIDList);
            toRelease.Remove(this._lastPoseID);
            this._lastPoseID = toRelease[Random.Range(0, toRelease.Count)];
            ListPool<int>.Release(toRelease);
            this._poseStateTable.TryGetValue(this._lastPoseID, ref this._poseInfo);
            this._agent.ClearItems();
            this.PlayInAnimation(this._poseInfo);
          }
          this._isWait = (Func<AgentActor, bool>) (actor => actor.Animation.PlayingInAnimation);
          this._onEndAction = (Subject<Unit>) null;
        }
      }));
      this._poseLoopEndAnimAction = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this._poseLoopEndAnimAction, (Component) this._agent), (System.Action<M0>) (_ =>
      {
        this.PlayOutAnimation(this._poseInfo);
        this._isWait = (Func<AgentActor, bool>) (actor => actor.Animation.PlayingOutAnimation);
        this._onEndAction = this._poseReplayAnimAction;
      }));
      this.PlayInAnimation(this._poseInfo);
      this._isWait = (Func<AgentActor, bool>) (actor => actor.Animation.PlayingInAnimation);
      this._onEndAction = (Subject<Unit>) null;
    }

    private PlayState GetPlayState(PoseKeyPair poseKey)
    {
      if (!Singleton<Resources>.IsInstance())
        return (PlayState) null;
      Dictionary<int, PlayState> dictionary;
      PlayState playState;
      return Singleton<Resources>.Instance.Animation.AgentActionAnimTable.TryGetValue(poseKey.postureID, out dictionary) && dictionary.TryGetValue(poseKey.poseID, out playState) ? playState : (PlayState) null;
    }

    private void PlayInAnimation(ValueTuple<PoseKeyPair, bool> poseInfo)
    {
      this._loopCounter = 0.0f;
      this._loopTime = 0.0f;
      PlayState playState = this.GetPlayState((PoseKeyPair) poseInfo.Item1);
      if (playState != null && Object.op_Inequality((Object) this._agent, (Object) null))
      {
        this._agent.Animation.StopAllAnimCoroutine();
        this._agent.Animation.InitializeStates(playState);
        this._agent.Animation.PlayInAnimation(playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.MainStateInfo.FadeOutTime, playState.Layer);
        this._loopTime = Random.Range((float) playState.MainStateInfo.LoopMin, (float) playState.MainStateInfo.LoopMax);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this._agent.Animation.LoadEventKeyTable((^(PoseKeyPair&) ref poseInfo.Item1).postureID, (^(PoseKeyPair&) ref poseInfo.Item1).poseID);
        this._agent.LoadEventItems(playState);
      }
      bool lookCamera = this._lookCamera;
      Transform trfTarg = this.CameraTarget(this._prevShotType);
      this._lookCamera = poseInfo.Item2 != null && Object.op_Inequality((Object) this._agent, (Object) null) && Object.op_Inequality((Object) trfTarg, (Object) null);
      if (this._lookCamera)
      {
        this._agent.ChaControl.ChangeLookNeckTarget(1, trfTarg, 0.5f, 0.0f, 1f, 0.8f);
        this._agent.ChaControl.ChangeLookNeckPtn(1, 1f);
        this._agent.ChaControl.ChangeLookEyesTarget(1, trfTarg, 0.5f, 0.0f, 1f, 2f);
        this._agent.ChaControl.ChangeLookEyesPtn(1);
      }
      else
      {
        if (!lookCamera || !Object.op_Inequality((Object) this._agent, (Object) null))
          return;
        this._agent.ChaControl.ChangeLookNeckPtn(3, 1f);
        this._agent.ChaControl.ChangeLookEyesPtn(3);
      }
    }

    private void PlayOutAnimation(ValueTuple<PoseKeyPair, bool> poseInfo)
    {
      PlayState playState = this.GetPlayState((PoseKeyPair) poseInfo.Item1);
      if (playState == null)
        return;
      this._agent.Animation.StopAllAnimCoroutine();
      this._agent.Animation.PlayOutAnimation(playState.MainStateInfo.OutStateInfo.EnableFade, playState.MainStateInfo.OutStateInfo.FadeSecond, playState.Layer);
    }

    private bool EndState()
    {
      if (this._agent.ReleasableCommand && this._agent.IsFarPlayerByPhotoMode || !(this._player.PlayerController.State is Photo))
        return true;
      return (double) this._agent.RuntimeMotivationInPhoto <= 0.0 && false;
    }

    private Transform CameraTarget()
    {
      return this._playerCameraCon.ShotType == ShotType.PointOfView ? ((Component) this._playerCameraCon.CameraComponent).get_transform() : this._player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head);
    }

    private Transform CameraTarget(ShotType shotType)
    {
      return shotType == ShotType.PointOfView ? ((Component) this._playerCameraCon.CameraComponent).get_transform() : this._player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head);
    }

    public virtual TaskStatus OnUpdate()
    {
      ShotType shotType = this._playerCameraCon.ShotType;
      if (this._prevShotType != shotType)
      {
        if (this._lookCamera)
        {
          Transform trfTarg = this.CameraTarget(shotType);
          this._agent.ChaControl.ChangeLookNeckTarget(1, trfTarg, 0.5f, 0.0f, 1f, 0.8f);
          this._agent.ChaControl.ChangeLookNeckPtn(1, 1f);
          this._agent.ChaControl.ChangeLookEyesTarget(1, trfTarg, 0.5f, 0.0f, 1f, 2f);
          this._agent.ChaControl.ChangeLookEyesPtn(1);
        }
        this._prevShotType = shotType;
      }
      TaskStatus taskStatus = this.Update();
      if (taskStatus != 3)
        this._agent.Animation.StopAllAnimCoroutine();
      return taskStatus;
    }

    private TaskStatus Update()
    {
      if (this._isFadeOut)
      {
        if (this._agent.Animation.PlayingOutAnimation)
          return (TaskStatus) 3;
        this._agent.ClearItems();
        return (TaskStatus) 2;
      }
      bool? nullable = this._isWait != null ? new bool?(this._isWait(this._agent)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0)
        return (TaskStatus) 3;
      if (this._onEndAction != null)
      {
        Subject<Unit> onEndAction = this._onEndAction;
        this._onEndAction = (Subject<Unit>) null;
        onEndAction.OnNext(Unit.get_Default());
        return (TaskStatus) 3;
      }
      if (this.EndState())
      {
        this._poseOutAnimAction.OnNext(Unit.get_Default());
        return (TaskStatus) 3;
      }
      this._loopCounter += Time.get_deltaTime();
      if ((double) this._loopTime > (double) this._loopCounter)
        return (TaskStatus) 3;
      this._loopCounter = this._loopTime + 1f;
      if (this._poseLoopEndAnimAction != null)
        this._poseLoopEndAnimAction.OnNext(Unit.get_Default());
      return (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      this._agent.NavMeshAgent.set_avoidancePriority(this._prevPriority);
      this._agent.StateType = this._prevStateType;
      if (this._lookCamera)
      {
        if (Object.op_Inequality((Object) this._agent, (Object) null))
        {
          this._agent.ChaControl.ChangeLookNeckPtn(3, 1f);
          this._agent.ChaControl.ChangeLookEyesPtn(3);
        }
        this._lookCamera = false;
      }
      this._agent.Animation.StopAllAnimCoroutine();
      ListPool<int>.Release(this._poseIDList);
      ((Task) this).OnEnd();
    }
  }
}
