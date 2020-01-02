// Decompiled with JetBrains decompiler
// Type: AIProject.Player.CharaEnter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using AIProject.SaveData;
using Cinemachine;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Player
{
  public class CharaEnter : PlayerStateBase
  {
    protected int _currentState = -1;
    private Subject<Unit> _onEndFadeIn = new Subject<Unit>();
    private Subject<Unit> _onEndFadeOut = new Subject<Unit>();
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private Queue<PlayState.Info> _inQueue = new Queue<PlayState.Info>();
    private AgentActor _agent;
    private Transform _eventCamera;
    private Animator _locator;
    private bool _registeredBefore;
    private bool _completeWait;
    private CinemachineBlendDefinition.Style _prevStyle;
    private IEnumerator _cameraAnimEnumerator;
    private IDisposable _cameraAnimDisposable;

    private OpenData openData { get; } = new OpenData();

    private PackData packData { get; } = new PackData();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = (AIProject.EventType) 0;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeIn, 1), (System.Action<M0>) (_ =>
      {
        this.Refresh(player);
        ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(100.0)), (System.Action<M0>) (__ =>
        {
          this._completeWait = true;
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, false), (System.Action<M0>) (___ => {}), (System.Action) (() => this._onEndFadeOut.OnNext(Unit.get_Default())));
        }));
      }));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeOut, 1), (System.Action<M0>) (_ => this.StartEventSeq(player)));
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      this._prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() => this._onEndFadeIn.OnNext(Unit.get_Default())));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.CameraControl.EventCameraLocator.set_runtimeAnimatorController((RuntimeAnimatorController) null);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) this._prevStyle;
      player.CameraControl.Mode = CameraMode.Normal;
      player.ChaControl.visibleAll = true;
      if (Object.op_Inequality((Object) this._agent, (Object) null))
      {
        AssetBundleInfo outInfo = (AssetBundleInfo) null;
        this._agent.ChangeAnimator(Singleton<Resources>.Instance.Animation.GetCharaAnimator(0, ref outInfo));
        if (!this._registeredBefore)
        {
          this._agent.RefreshWalkStatus(Singleton<Manager.Map>.Instance.PointAgent.Waypoints);
          Singleton<Manager.Map>.Instance.InitSearchActorTargets(this._agent);
          player.PlayerController.CommandArea.AddCommandableObject((ICommandable) this._agent);
          using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, AgentActor> current = enumerator.Current;
              if (!Object.op_Equality((Object) current.Value, (Object) this._agent))
                current.Value.AddActor((Actor) this._agent);
            }
          }
        }
        this._agent.ClearItems();
        this._agent.ClearParticles();
        this._agent.ActivateNavMeshAgent();
        this._agent.EnableBehavior();
        this._agent.ChangeBehavior(Desire.ActionType.Normal);
      }
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.Action);
      Singleton<Manager.Input>.Instance.SetupState();
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.FadeCanvas.IsFadeIn || !this._completeWait || MapUIContainer.FadeCanvas.IsFadeOut)
        return;
      DevicePoint currentDevicePoint = player.CurrentDevicePoint;
      if (Object.op_Inequality((Object) this._agent, (Object) null) && (this._agent.Animation.PlayingInLocoAnimation || currentDevicePoint.PlayingInAnimation || this.PlayingCameraAnimation) || this._onEndAction == null)
        return;
      this._onEndAction.OnNext(Unit.get_Default());
    }

    private void Elapsed(PlayerActor player)
    {
      player.CurrentDevicePoint = (DevicePoint) null;
      MapUIContainer.SetVisibleHUD(true);
      MapUIContainer.StorySupportUI.Open();
      player.PlayerController.CommandArea.UpdateCollision(player);
      player.Controller.ChangeState("Normal");
    }

    private void Refresh(PlayerActor player)
    {
      int id = player.CurrentDevicePoint.ID;
      AgentData agentData = Singleton<Game>.Instance.WorldData.AgentTable[id];
      MapUIContainer.SetVisibleHUD(false);
      AgentActor agentActor = Singleton<Manager.Map>.Instance.AddAgent(id, agentData);
      agentActor.DisableBehavior();
      Actor.BehaviorSchedule schedule = agentActor.Schedule;
      schedule.enabled = false;
      agentActor.Schedule = schedule;
      agentActor.TargetInSightActor = (Actor) null;
      agentActor.DeactivateNavMeshAgent();
      if (Object.op_Inequality((Object) agentActor.CurrentPoint, (Object) null))
      {
        agentActor.CurrentPoint.SetActiveMapItemObjs(true);
        agentActor.CurrentPoint.ReleaseSlot((Actor) agentActor);
        agentActor.CurrentPoint = (ActionPoint) null;
      }
      agentActor.TargetInSightActionPoint = (ActionPoint) null;
      player.ChaControl.visibleAll = false;
      Transform pivotPoint = player.CurrentDevicePoint.PivotPoint;
      agentActor.Position = pivotPoint.get_position();
      agentActor.Rotation = pivotPoint.get_rotation();
      agentData.Position = player.CurrentDevicePoint.RecoverPoints[0].get_position();
      agentData.Rotation = player.CurrentDevicePoint.RecoverPoints[0].get_rotation();
      Animator animator = this._locator = player.CameraControl.EventCameraLocator;
      ((Component) animator).get_transform().set_position(pivotPoint.get_position());
      ((Component) animator).get_transform().set_rotation(pivotPoint.get_rotation());
      CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
      RuntimeAnimatorController itemAnimator = Singleton<Resources>.Instance.Animation.GetItemAnimator(commonDefine.ItemAnims.AppearCameraAnimatorID);
      animator.set_runtimeAnimatorController(itemAnimator);
      ((Component) animator).get_transform().set_position(pivotPoint.get_position());
      ((Component) animator).get_transform().set_rotation(pivotPoint.get_rotation());
      animator.set_speed(0.0f);
      animator.Play(commonDefine.AppearCameraInStates[agentActor.ChaControl.fileParam.personality][0]);
      this._eventCamera = ((Component) player.CameraControl.EventCamera).get_transform();
      ((Component) this._eventCamera).get_transform().SetParent(player.CameraControl.EventCameraParent, false);
      ((Component) this._eventCamera).get_transform().set_localPosition(Vector3.get_zero());
      ((Component) this._eventCamera).get_transform().set_localRotation(Quaternion.Euler(0.0f, 180f, 0.0f));
      player.SetActiveOnEquipedItem(false);
      player.CameraControl.Mode = CameraMode.Event;
      Transform playerRecoverPoint = player.CurrentDevicePoint.PlayerRecoverPoint;
      if (Object.op_Inequality((Object) playerRecoverPoint, (Object) null))
      {
        player.NavMeshAgent.Warp(playerRecoverPoint.get_position());
        player.Rotation = playerRecoverPoint.get_rotation();
      }
      this._agent = agentActor;
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    private void StartEventSeq(PlayerActor player)
    {
      if (Object.op_Inequality((Object) this._agent, (Object) null))
      {
        DevicePoint devicePoint = player.CurrentDevicePoint;
        AgentData agentData = Singleton<Game>.Instance.WorldData.AgentTable[devicePoint.ID];
        AgentActor agent = this._agent;
        if (agentData.PlayEnterScene)
          return;
        agentData.PlayEnterScene = true;
        int personality = agent.ChaControl.fileParam.personality;
        PoseKeyPair appearId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.AppearIDList[personality];
        PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[appearId.postureID][appearId.poseID];
        ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
        {
          layer = playState.Layer,
          inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
          inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
          outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
          outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
          directionType = playState.DirectionType,
          endEnableBlend = playState.EndEnableBlend,
          endBlendSec = playState.EndBlendRate
        };
        agent.Animation.AnimInfo = actorAnimInfo1;
        ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
        AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
        agent.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        float shapeBodyValue = agent.ChaControl.GetShapeBodyValue(0);
        string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
        agent.Animation.Animator.SetFloat(heightParameterName, shapeBodyValue);
        this._locator.SetFloat(heightParameterName, shapeBodyValue);
        agent.Animation.LoadEventKeyTable(appearId.postureID, appearId.poseID);
        agent.LoadEventItems(playState);
        agent.LoadEventParticles(appearId.postureID, appearId.poseID);
        agent.Animation.InitializeStates(playState.MainStateInfo.InStateInfo.StateInfos, playState.MainStateInfo.OutStateInfo.StateInfos, playState.MainStateInfo.AssetBundleInfo);
        Transform basePoint = devicePoint.PivotPoint;
        agent.Position = basePoint.get_position();
        agent.Rotation = basePoint.get_rotation();
        agent.Animation.PlayInLocoAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.layer);
        devicePoint.PlayInAnimation();
        this._locator.set_speed(1f);
        this.PlayCameraAnimation(this._locator, personality);
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (__ =>
        {
          this.packData.Init();
          this.openData.FindLoad("0", agent.AgentData.param.charaID, 6);
          this.packData.SetParam((IParams) agent.AgentData, (IParams) player.PlayerData);
          this.packData.onComplete = (System.Action) (() =>
          {
            if (agent.IsEvent)
              agent.IsEvent = false;
            if (!Manager.Config.GraphicData.CharasEntry[agent.ID])
            {
              this.packData.Release();
              Singleton<Manager.ADV>.Instance.Captions.EndADV((System.Action) null);
              devicePoint.PlayOutAnimation();
              this.PlayCloseSE(player, basePoint);
              ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
              {
                Singleton<Manager.Map>.Instance.RemoveAgent(agent);
                agent = (AgentActor) null;
                ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() => this.Elapsed(player)));
              }));
            }
            else
            {
              this.packData.Release();
              Singleton<Manager.ADV>.Instance.Captions.EndADV((System.Action) null);
              this.Elapsed(player);
              devicePoint.PlayOutAnimation();
              this.PlayCloseSE(player, basePoint);
            }
          });
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
        }));
      }
      else
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (__ =>
        {
          player.CurrentDevicePoint = (DevicePoint) null;
          player.PlayerController.CommandArea.UpdateCollision(player);
          player.Controller.ChangeState("Normal");
        }));
    }

    private void PlayCameraAnimation(Animator animator, int personality)
    {
      this._inQueue.Clear();
      foreach (string name_ in Singleton<Resources>.Instance.CommonDefine.AppearCameraInStates[personality])
        this._inQueue.Enqueue(new PlayState.Info(name_, 0));
      this._cameraAnimEnumerator = this.StartCameraAnimation(animator);
      this._cameraAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._cameraAnimEnumerator), false));
    }

    private void StopCameraAnimation()
    {
      if (this._cameraAnimDisposable == null)
        return;
      this._cameraAnimDisposable.Dispose();
      this._cameraAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartCameraAnimation(Animator animator)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaEnter.\u003CStartCameraAnimation\u003Ec__Iterator0()
      {
        animator = animator,
        \u0024this = this
      };
    }

    public bool PlayingCameraAnimation
    {
      get
      {
        return this._cameraAnimEnumerator != null;
      }
    }

    private void PlayCloseSE(PlayerActor player, Transform playRoot)
    {
      if (Object.op_Equality((Object) player, (Object) null) || Singleton<Resources>.IsInstance())
        return;
      if (Object.op_Equality((Object) playRoot, (Object) null))
        playRoot = ((Component) player.Locomotor).get_transform();
      SoundPack.DoorSEIDInfo doorSeidInfo;
      if (!Singleton<Resources>.Instance.SoundPack.DoorIDTable.TryGetValue(DoorMatType.Capsule, out doorSeidInfo))
        return;
      AudioSource audioSource = Singleton<Resources>.Instance.SoundPack.Play(doorSeidInfo.CloseID, Sound.Type.GameSE3D, 0.0f);
      audioSource.Stop();
      ((Component) audioSource).get_transform().SetPositionAndRotation(playRoot.get_position(), playRoot.get_rotation());
      audioSource.Play();
    }
  }
}
