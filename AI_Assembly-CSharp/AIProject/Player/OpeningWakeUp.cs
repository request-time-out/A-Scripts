// Decompiled with JetBrains decompiler
// Type: AIProject.Player.OpeningWakeUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject.Scene;
using Cinemachine;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class OpeningWakeUp : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private Queue<PlayState.Info> _inQueue = new Queue<PlayState.Info>();
    private PlayerActor _player;
    private AgentActor _agent;
    private CinemachineVirtualCameraBase _eventCamera;
    private bool _fadeEnd;
    private CinemachineBlendDefinition.Style _prevStyle;
    private IDisposable _fadeTimerDisposable;
    private bool _isFinish;
    private IEnumerator _cameraAnimEnumerator;
    private IDisposable _cameraAnimDisposable;

    private OpenData openData { get; } = new OpenData();

    private OpeningWakeUp.PackData packData { get; set; }

    protected override void OnAwake(PlayerActor player)
    {
      this._player = player;
      this._agent = Singleton<Manager.Map>.Instance.TutorialAgent;
      this._isFinish = false;
      if (Object.op_Equality((Object) this._player, (Object) null) || Object.op_Equality((Object) this._agent, (Object) null))
      {
        Debug.LogError((object) "OpeningWakeUp: プレイヤー キャラが空っぽなのはおかしい");
      }
      else
      {
        AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
        CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
        Resources.AnimationTables animation = Singleton<Resources>.Instance.Animation;
        CommonDefine.TutorialSetting setting = commonDefine.Tutorial;
        player.ChaControl.visibleAll = false;
        this._agent.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.Idle);
        player.EventKey = (AIProject.EventType) 0;
        this._fadeEnd = false;
        int personality = this._agent.ChaControl.fileParam.personality;
        ChaControl chaControl1 = this._agent.ChaControl;
        chaControl1.ChangeLookEyesPtn(0);
        chaControl1.ChangeLookNeckPtn(3, 1f);
        PoseKeyPair pose;
        PlayState playState;
        this.TryGetWakeUpAnimState(personality, out pose, out playState);
        MapUIContainer.SetVisibleHUD(false);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
        Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
        Singleton<Manager.Input>.Instance.SetupState();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this._prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        this._eventCamera = player.CameraControl.EventCamera;
        ((Component) this._eventCamera).get_transform().SetParent(player.CameraControl.EventCameraParent, false);
        ((Component) this._eventCamera).get_transform().set_localPosition(Vector3.get_zero());
        ((Component) this._eventCamera).get_transform().set_localRotation(Quaternion.Euler(0.0f, 180f, 0.0f));
        Animator eventCameraLocator = player.CameraControl.EventCameraLocator;
        RuntimeAnimatorController itemAnimator = animation.GetItemAnimator(commonDefine.ItemAnims.OpeningWakeUpCameraAnimatorID);
        eventCameraLocator.set_runtimeAnimatorController(itemAnimator);
        ((Component) eventCameraLocator).get_transform().set_position(this._agent.Position);
        ((Component) eventCameraLocator).get_transform().set_rotation(this._agent.Rotation);
        eventCameraLocator.set_speed(0.0f);
        float shapeBodyValue = this._agent.ChaControl.GetShapeBodyValue(0);
        string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
        eventCameraLocator.SetFloat(heightParameterName, shapeBodyValue);
        player.CameraControl.Mode = CameraMode.Event;
        this._agent.Animation.LoadEventKeyTable(pose.postureID, pose.poseID);
        this._agent.Animation.InitializeStates(playState);
        this._agent.LoadEventItems(playState);
        this._agent.Animation.StopAllAnimCoroutine();
        this._agent.Animation.PlayInAnimation(playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.MainStateInfo.FadeOutTime, playState.Layer);
        this.PlayCameraAnimation(eventCameraLocator, personality);
        Transform transform = ((Component) player.CameraControl.CameraComponent).get_transform();
        ChaControl chaControl2 = this._agent.ChaControl;
        chaControl2.ChangeLookEyesTarget(1, transform, 0.5f, 0.0f, 1f, 2f);
        chaControl2.ChangeLookEyesPtn(1);
        this._fadeTimerDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) setting.OpeningWakeUpStartFadeTime)), (Component) player), (System.Action<M0>) (_ =>
        {
          if (Singleton<Sound>.IsInstance())
            Singleton<Sound>.Instance.StopBGM(setting.OpeningWakeUpFadeTime);
          this._fadeTimerDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, setting.OpeningWakeUpFadeTime, false), (System.Action<M0>) (__ => {}), (System.Action) (() => this._fadeEnd = true));
        }));
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ =>
        {
          this._isFinish = true;
          this.Elapsed(player);
        }));
      }
    }

    private bool TryGetWakeUpAnimState(
      int personalID,
      out PoseKeyPair pose,
      out PlayState playState)
    {
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      if (!agentProfile.TutorialWakeUpPoseTable.TryGetValue(personalID, out pose) && !agentProfile.TutorialWakeUpPoseTable.TryGetValue(0, out pose))
      {
        playState = (PlayState) null;
        return false;
      }
      Dictionary<int, PlayState> dictionary;
      if (Singleton<Resources>.Instance.Animation.AgentActionAnimTable.TryGetValue(pose.postureID, out dictionary) && dictionary.TryGetValue(pose.poseID, out playState))
        return true;
      playState = (PlayState) null;
      return false;
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      if (this._isFinish || !this._fadeEnd || (MapUIContainer.FadeCanvas.IsFadeIn || this.PlayingCameraAnimation) || (this._agent.Animation.PlayingInAnimation || this._onEndAction == null))
        return;
      this._onEndAction.OnNext(Unit.get_Default());
    }

    private void StartADV()
    {
      Debug.Log((object) "Opening:StartADV");
      if (Object.op_Equality((Object) this._player, (Object) null))
        return;
      AgentActor tutorialAgent = Singleton<Manager.Map>.Instance.TutorialAgent;
      this._player.CommCompanion = (Actor) tutorialAgent;
      this._player.PlayerController.ChangeState("Communication");
      if (!Object.op_Inequality((Object) tutorialAgent, (Object) null))
        return;
      Transform transform = ((Component) this._player.CameraControl.CameraComponent).get_transform();
      tutorialAgent.SetLookPtn(1, 3);
      tutorialAgent.SetLookTarget(1, 0, transform);
      this.openData.FindLoad("0", tutorialAgent.AgentData.param.charaID, 100);
      this.packData = new OpeningWakeUp.PackData();
      this.packData.Init();
      this.packData.SetParam((IParams) tutorialAgent.AgentData, (IParams) this._player.PlayerData);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.EndADV();
        this.packData.Release();
        this.packData = (OpeningWakeUp.PackData) null;
      });
      ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this._player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)));
    }

    private void EndADV()
    {
      Debug.Log((object) "Opening:EndADV");
      AgentActor tutorialAgent = Singleton<Manager.Map>.Instance.TutorialAgent;
      if (Object.op_Inequality((Object) tutorialAgent, (Object) null))
      {
        tutorialAgent.ClearItems();
        ((Component) this._player.CameraControl.CameraComponent).get_transform();
        tutorialAgent.SetLookPtn(0, 3);
        tutorialAgent.SetLookTarget(0, 0, (Transform) null);
      }
      MapUIContainer.SetVisibleHUD(true);
      if (this._player.CameraControl.ShotType == ShotType.PointOfView)
      {
        this._player.CameraControl.XAxisValue = (float) this._player.Rotation.y;
        this._player.CameraControl.YAxisValue = 0.5f;
      }
      else
      {
        ActorCameraControl cameraControl = this._player.CameraControl;
        Quaternion rotation = this._player.Rotation;
        double num = ((Quaternion) ref rotation).get_eulerAngles().y - 30.0;
        cameraControl.XAxisValue = (float) num;
        this._player.CameraControl.YAxisValue = 0.6f;
      }
      SoundPlayer instance1 = Singleton<SoundPlayer>.Instance;
      instance1.StartAllSubscribe();
      instance1.ActivateWideEnvSE(true);
      this._player.CameraControl.Mode = CameraMode.Normal;
      Manager.Map.SetTutorialProgress(1);
      this._player.PlayerController.ChangeState("Idle");
      this._agent.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.HeadToSandBeach);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this._player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ =>
      {
        this._player.PlayerController.ChangeState("Normal");
        if (Singleton<MapScene>.IsInstance())
        {
          MapScene instance2 = Singleton<MapScene>.Instance;
          instance2.SaveProfile(true);
          instance2.SaveProfile(false);
        }
        if (!Singleton<Game>.IsInstance())
          return;
        Singleton<Game>.Instance.SaveGlobalData();
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (this._fadeTimerDisposable == null)
        return;
      this._fadeTimerDisposable.Dispose();
    }

    private void Elapsed(PlayerActor player)
    {
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref this._player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) this._prevStyle;
      Vector3 position = ((Component) this._eventCamera).get_transform().get_position();
      player.CameraControl.EventCameraLocator.set_runtimeAnimatorController((RuntimeAnimatorController) null);
      ((Component) this._eventCamera).get_transform().set_position(position);
      this.StartADV();
    }

    private void PlayCameraAnimation(Animator animator, int personalID)
    {
      this._inQueue.Clear();
      Dictionary<int, string[]> upCameraInStates = Singleton<Resources>.Instance.CommonDefine.OpeningWakeUpCameraInStates;
      string[] source;
      if (!upCameraInStates.TryGetValue(personalID, out source) && !upCameraInStates.TryGetValue(0, out source) || source.IsNullOrEmpty<string>())
        return;
      foreach (string name_ in source)
        this._inQueue.Enqueue(new PlayState.Info(name_, 0));
      animator.set_speed(1f);
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
      return (IEnumerator) new OpeningWakeUp.\u003CStartCameraAnimation\u003Ec__Iterator0()
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

    private class PackData : CharaPackData
    {
    }
  }
}
