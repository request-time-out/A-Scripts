// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Fishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.MiniGames.Fishing;
using AIProject.Scene;
using AIProject.UI;
using IllusionUtility.GetUtility;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Fishing : PlayerStateBase
  {
    private List<PlayState.Info> StateInInfos = new List<PlayState.Info>();
    private bool initFlag;
    private const string exitStateName = "Normal";
    private Action<PlayerActor> OnFinished;
    private static GameObject fishingSystemRoot;
    private float horizontalValue;
    private PlayState currentPlayState;
    private RuntimeAnimatorController fishingBeforeAnimController;
    private RuntimeAnimatorController fishingAnimController;
    private ShotType _prevShotType;
    private bool inputFlag;
    private float prevAxisX;
    private IDisposable animationDisposable;
    public Action MissAnimationEndEvent;
    public Action MissEndAnimationEndEvent;
    public Action StopAnimationEndEvent;

    private FishingDefinePack.SystemParamGroup SystemParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.SystemParam;
      }
    }

    private FishingDefinePack.PlayerParamGroup PlayerParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.PlayerParam;
      }
    }

    private FishingManager fishingSystem
    {
      get
      {
        return Singleton<Manager.Map>.Instance?.FishingSystem;
      }
      set
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        Singleton<Manager.Map>.Instance.FishingSystem = value;
      }
    }

    public float MoveAreaPosY { get; private set; }

    public GameObject hand { get; private set; }

    public bool EndAnimation { get; private set; }

    public bool LastAnimation { get; private set; }

    public float HorizontalValue
    {
      get
      {
        return this.horizontalValue;
      }
      set
      {
        this.horizontalValue = Mathf.Clamp(value, -1f, 1f);
        this.SetHorizontal(this.horizontalValue);
      }
    }

    public AIProject.Player.Fishing.PoseID currentPoseID { get; private set; }

    public PlayerActor player { get; set; }

    private Resources.FishingTable FishingInfos
    {
      get
      {
        return Singleton<Resources>.Instance.Fishing;
      }
    }

    private Dictionary<int, PlayState> AnimTable
    {
      get
      {
        return this.FishingInfos.PlayerAnimStateTable;
      }
    }

    private FishingUI fishingUI
    {
      get
      {
        return MapUIContainer.FishingUI;
      }
    }

    protected override void OnAwake(PlayerActor actor)
    {
      this.player = actor;
      this.player.SetActiveOnEquipedItem(false);
      this.player.ChaControl.setAllLayerWeight(0.0f);
      this.currentPoseID = AIProject.Player.Fishing.PoseID.Idle;
      this.currentPlayState = (PlayState) null;
      this.fishingBeforeAnimController = (RuntimeAnimatorController) null;
      if (Object.op_Equality((Object) this.player, (Object) null))
      {
        actor.Controller.ChangeState("Normal");
      }
      else
      {
        this.InitFishingSystem();
        this.LoadFishingRod();
        if (!this.fishingSystem.playerInfo.ActiveFishingRodInfo)
        {
          this.SetActive(AIProject.Player.Fishing.fishingSystemRoot, false);
          this.player.PlayerController.ChangeState("Normal");
        }
        else
        {
          this.MoveAreaPosY = (float) this.player.PlayerController.CommandArea.BobberPosition.y;
          MapUIContainer.SetVisibleHUD(false);
          MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
          this.player.CameraControl.Mode = CameraMode.Fishing;
          this.LoadFishingAnimator();
          this.PlayStandbyMotion(false);
          this.HaveFishingRod();
          MapUIContainer.SetActiveFishingUI(true);
          this.fishingSystem.Initialize(this);
          this.OnFinished = (Action<PlayerActor>) (x => this.Finished(x));
          this._prevShotType = this.player.CameraControl.ShotType;
          this.player.CameraControl.SetShotTypeForce(ShotType.Near);
          this.initFlag = true;
        }
      }
    }

    private void InitFishingSystem()
    {
      if (Object.op_Equality((Object) this.fishingSystem, (Object) null))
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab, this.PlayerParam.FishingGamePrefabName, false, Singleton<Resources>.Instance.DefinePack.ABManifests.Default), ((Component) this.player).get_transform());
        ((Object) gameObject).set_name(this.PlayerParam.FishingGamePrefabName);
        AIProject.Player.Fishing.fishingSystemRoot = gameObject;
        this.fishingSystem = (FishingManager) gameObject.GetComponentInChildren<FishingManager>();
        this.fishingSystem.FishGetEvent = (Action) (() => this.AcquirementFish());
        MapScene.AddAssetBundlePath(Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab, Singleton<Resources>.Instance.DefinePack.ABManifests.Default);
      }
      AIProject.Player.Fishing.fishingSystemRoot.get_transform().set_position(this.player.Position);
      AIProject.Player.Fishing.fishingSystemRoot.get_transform().set_rotation(this.player.Rotation);
      this.FitMoveAreaOnWater();
      this.SetActive(AIProject.Player.Fishing.fishingSystemRoot, true);
    }

    private void FitMoveAreaOnWater()
    {
      Vector3 _hitPosition;
      FishingManager.GetWaterPosition(this.fishingSystem.MoveArea.get_transform().get_position(), out _hitPosition);
      this.fishingSystem.MoveArea.get_transform().set_position(_hitPosition);
      this.fishingSystem.MoveArea.get_transform().set_localEulerAngles(Vector3.get_zero());
      Vector3 originLocalPosition = this.fishingSystem.moveAreaOriginLocalPosition;
      originLocalPosition.y = this.fishingSystem.MoveArea.get_transform().get_localPosition().y;
      this.fishingSystem.moveAreaOriginLocalPosition = originLocalPosition;
    }

    public void SetHorizontal(float _value)
    {
      this.player?.Animation.Animator.SetFloat(this.PlayerParam.PlayerAnimParamName, _value);
      this.fishingSystem.playerInfo.fishingRodAnimator.SetFloat(this.PlayerParam.RodAnimParamName, _value);
    }

    private void LoadFishingRod()
    {
      this.hand = ((Component) this.player.ChaControl).get_transform().FindLoop("k_f_handR_00");
      PlayerInfo playerInfo = this.fishingSystem?.playerInfo;
      if (playerInfo == null)
        return;
      Dictionary<int, FishingRodInfo> rodInfos = this.FishingInfos.RodInfos;
      if (!playerInfo.ActiveFishingRodInfo && !rodInfos.IsNullOrEmpty<int, FishingRodInfo>())
      {
        FishingRodInfo fishingRodInfo = rodInfos[0];
        playerInfo.fishingRod = (GameObject) Object.Instantiate<GameObject>((M0) fishingRodInfo.Rod);
        playerInfo.fishingRodAnimController = fishingRodInfo.AnimController;
        playerInfo.lurePos = playerInfo.fishingRod.get_transform().FindLoop(fishingRodInfo.TipName);
        playerInfo.fishingRodAnimator = (Animator) playerInfo.fishingRod.GetComponent<Animator>();
        playerInfo.fishingRodAnimator.set_runtimeAnimatorController(playerInfo.fishingRodAnimController);
      }
      if (!playerInfo.ActiveFishingRodInfo)
        return;
      this.SetActive(playerInfo.fishingRod, false);
      Transform transform1 = playerInfo.fishingRod.get_transform();
      transform1.set_parent(this.fishingSystem.RootObject.get_transform());
      Transform transform2 = transform1;
      Vector3 zero = Vector3.get_zero();
      transform1.set_localEulerAngles(zero);
      Vector3 vector3 = zero;
      transform2.set_localPosition(vector3);
    }

    private void LoadFishingAnimator()
    {
      this.fishingBeforeAnimController = this.player.Animation.Animator.get_runtimeAnimatorController();
      if (!Object.op_Equality((Object) this.fishingAnimController, (Object) null))
        return;
      this.FishingInfos.PlayerAnimatorTable.TryGetValue((int) this.player.ChaControl.sex, out this.fishingAnimController);
    }

    public void HaveFishingRod()
    {
      Transform transform1 = this.fishingSystem?.playerInfo?.fishingRod?.get_transform();
      if (Object.op_Equality((Object) transform1, (Object) null) || Object.op_Equality((Object) this.hand, (Object) null))
        return;
      transform1.set_parent(this.hand.get_transform());
      Transform transform2 = transform1;
      Vector3 zero = Vector3.get_zero();
      transform1.set_localEulerAngles(zero);
      Vector3 vector3 = zero;
      transform2.set_localPosition(vector3);
      this.SetActive((Component) transform1, true);
      this.SetActive((Component) this.fishingSystem.lure, true);
      this.HorizontalValue = 0.0f;
    }

    public void ReleaseFishingRod()
    {
      Transform transform = this.fishingSystem?.playerInfo?.fishingRod?.get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      transform.set_parent(this.fishingSystem.RootObject.get_transform());
      this.SetActive((Component) this.fishingSystem.lure, false);
      this.SetActive((Component) transform, false);
    }

    private void Finished(PlayerActor player)
    {
      this.fishingSystem.Release();
      this.SetActive(AIProject.Player.Fishing.fishingSystemRoot, false);
      player.PlayerController.ChangeState("Normal");
    }

    private void AcquirementFish()
    {
    }

    protected override void OnRelease(PlayerActor player)
    {
      this.ReleaseFishingRod();
      this.ChangeNormalAnimation();
      player.CameraControl.Mode = CameraMode.Normal;
      player.CameraControl.SetShotTypeForce(this._prevShotType);
      MapUIContainer.SetActiveFishingUI(false);
      if (MapUIContainer.CommandLabel.Acception != CommandLabel.AcceptionState.InvokeAcception)
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      MapUIContainer.SetVisibleHUD(true);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      if (!this.initFlag)
        return;
      Input instance = Singleton<Input>.Instance;
      this.inputFlag = false;
      this.HorizontalUpdate(instance);
      this.UpdateRodAngle();
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    private void UpdateRodAngle()
    {
      Transform transform1 = ((Component) this.fishingSystem.lure).get_transform();
      Transform transform2 = this.fishingSystem.playerInfo.fishingRod.get_transform();
      Transform transform3 = ((Component) this.player.Controller).get_transform();
      if (Object.op_Equality((Object) transform2, (Object) null) || Object.op_Equality((Object) transform1, (Object) null) || Object.op_Equality((Object) transform3, (Object) null))
        return;
      switch (this.fishingSystem.scene)
      {
        case FishingManager.FishingScene.WaitHit:
          Vector3 localEulerAngles = transform2.get_localEulerAngles();
          localEulerAngles.x = (__Null) (double) (localEulerAngles.z = (__Null) 0.0f);
          if (this.inputFlag)
          {
            Vector2 vector2_1;
            ((Vector2) ref vector2_1).\u002Ector((float) transform3.get_forward().x, (float) transform3.get_forward().z);
            Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
            Vector2 vector2_2;
            ((Vector2) ref vector2_2).\u002Ector((float) (transform1.get_position().x - transform3.get_position().x), (float) (transform1.get_position().z - transform3.get_position().z));
            Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
            float time = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
            float num1 = Mathf.Sign((float) Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y)).y);
            float num2 = EasingFunctions.EaseOutExpo(time, 90f);
            float num3 = time * num2 * num1 * this.PlayerParam.RodAngleScale;
            localEulerAngles.y = (__Null) (double) this.Angle360To180((float) localEulerAngles.y);
            float num4 = this.PlayerParam.RodHitWaitAngleSpeed * Time.get_deltaTime() * num1;
            localEulerAngles.y = (localEulerAngles.y + (double) num4) * (double) num1 >= (double) num3 * (double) num1 ? (__Null) (double) num3 : (__Null) (localEulerAngles.y + (double) num4);
            localEulerAngles.y = (__Null) (double) this.AngleAbs((float) localEulerAngles.y);
            transform2.set_localEulerAngles(localEulerAngles);
            break;
          }
          if (localEulerAngles.y == 0.0)
            break;
          localEulerAngles.y = (__Null) (double) this.Angle360To180((float) localEulerAngles.y);
          float num5 = Mathf.Sign((float) localEulerAngles.y);
          ref Vector3 local = ref localEulerAngles;
          local.y = (__Null) (local.y - (double) this.PlayerParam.RodHitWaitAngleSpeed * (double) Time.get_deltaTime() * (double) num5);
          if (localEulerAngles.y * (double) num5 < 0.0)
            localEulerAngles.y = (__Null) 0.0;
          localEulerAngles.y = (__Null) (double) this.AngleAbs((float) localEulerAngles.y);
          transform2.set_localEulerAngles(localEulerAngles);
          break;
        case FishingManager.FishingScene.Fishing:
          Vector2 vector2_3;
          ((Vector2) ref vector2_3).\u002Ector((float) transform3.get_forward().x, (float) transform3.get_forward().z);
          Vector2 normalized3 = ((Vector2) ref vector2_3).get_normalized();
          Vector2 vector2_4;
          ((Vector2) ref vector2_4).\u002Ector((float) (transform1.get_position().x - transform3.get_position().x), (float) (transform1.get_position().z - transform3.get_position().z));
          Vector2 normalized4 = ((Vector2) ref vector2_4).get_normalized();
          float time1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized3, normalized4), -1f, 1f)) * 57.29578f;
          Vector3 vector3 = Vector3.Cross(new Vector3((float) normalized3.x, 0.0f, (float) normalized3.y), new Vector3((float) normalized4.x, 0.0f, (float) normalized4.y));
          float num6 = EasingFunctions.EaseOutExpo(time1, 90f);
          float num7 = this.AngleAbs(time1 * num6 * Mathf.Sign((float) vector3.y) * this.PlayerParam.RodAngleScale);
          transform2.set_localEulerAngles(new Vector3(0.0f, num7, 0.0f));
          break;
        default:
          transform2.set_localEulerAngles(Vector3.get_zero());
          break;
      }
    }

    private float AngleAbs(float _angle)
    {
      if ((double) _angle < 0.0)
        _angle = (float) ((double) _angle % 360.0 + 360.0);
      else if ((double) _angle > 360.0)
        _angle %= 360f;
      return _angle;
    }

    private Vector3 AngleAbs(Vector3 _angle)
    {
      _angle.x = (__Null) (double) this.AngleAbs((float) _angle.x);
      _angle.y = (__Null) (double) this.AngleAbs((float) _angle.y);
      _angle.z = (__Null) (double) this.AngleAbs((float) _angle.z);
      return _angle;
    }

    private float Angle360To180(float _angle)
    {
      _angle = this.AngleAbs(_angle);
      if (180.0 < (double) _angle)
        _angle -= 360f;
      return _angle;
    }

    private void HorizontalUpdate(Input _input)
    {
      if (Object.op_Equality((Object) _input, (Object) null) || _input.State != Input.ValidType.Action || !this.LastAnimation)
        return;
      switch (this.fishingSystem.scene)
      {
        case FishingManager.FishingScene.WaitHit:
          float x1 = (float) _input.LeftStickAxis.x;
          Vector2 mouseAxis = this.fishingSystem.MouseAxis;
          float x2 = (float) ((Vector2) ref mouseAxis).get_normalized().x;
          float num1 = (double) Mathf.Abs(x2) > (double) Mathf.Abs(x1) ? x2 : x1;
          if (!Mathf.Approximately(num1, 0.0f) || !Mathf.Approximately(this.prevAxisX, 0.0f))
          {
            this.horizontalValue += this.PlayerParam.WaitHitMoveHorizontalScale * Mathf.Sign((float) (((double) num1 + (double) this.prevAxisX) / 2.0)) * Time.get_deltaTime();
            this.inputFlag = true;
          }
          else if (!Mathf.Approximately(this.horizontalValue, 0.0f))
          {
            bool flag = 0.0 <= (double) this.horizontalValue;
            this.horizontalValue += this.PlayerParam.WaitHitReturnHorizontalScale * (!flag ? 1f : -1f) * Time.get_deltaTime();
            if (flag && (double) this.horizontalValue <= 0.0 || !flag && 0.0 <= (double) this.horizontalValue)
              this.horizontalValue = 0.0f;
          }
          else
            this.HorizontalValue = 0.0f;
          this.prevAxisX = num1;
          this.horizontalValue = Mathf.Clamp(this.horizontalValue, -1f, 1f);
          this.SetHorizontal(this.horizontalValue);
          break;
        case FishingManager.FishingScene.Fishing:
          float num2 = (float) this.fishingSystem.ArrowPowerVector.x / this.SystemParam.ArrowMaxPower;
          if ((double) this.horizontalValue != (double) num2)
          {
            bool flag = (double) this.horizontalValue <= (double) num2;
            this.horizontalValue += (!flag ? -1f : 1f) * Time.get_deltaTime() * this.PlayerParam.FishingHorizontalScale;
            if (flag && (double) num2 <= (double) this.horizontalValue || !flag && (double) this.horizontalValue < (double) num2)
              this.horizontalValue = num2;
          }
          this.horizontalValue = Mathf.Clamp(this.horizontalValue, -1f, 1f);
          this.SetHorizontal(this.horizontalValue);
          break;
        default:
          this.prevAxisX = 0.0f;
          break;
      }
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnAnimatorStateEnterInternal(
      PlayerController control,
      AnimatorStateInfo stateInfo)
    {
    }

    protected override void OnAnimatorStateExitInternal(
      PlayerController control,
      AnimatorStateInfo stateInfo)
    {
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AIProject.Player.Fishing.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new AIProject.Player.Fishing.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }

    public float CurrentAnimatorNormalizedTime
    {
      get
      {
        PlayerActor player = this.player;
        float? nullable1;
        if (player == null)
        {
          nullable1 = new float?();
        }
        else
        {
          AnimatorStateInfo animatorStateInfo = player.Animation.Animator.GetCurrentAnimatorStateInfo(0);
          nullable1 = new float?(((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime());
        }
        float? nullable2 = nullable1;
        return nullable2.HasValue ? nullable2.Value : 0.0f;
      }
    }

    public bool IsInTransition
    {
      get
      {
        PlayerActor player = this.player;
        bool? nullable1;
        if (player == null)
        {
          nullable1 = new bool?();
        }
        else
        {
          Animator animator = player.Animation.Animator;
          int? nullable2 = this.currentPlayState != null ? new int?(this.currentPlayState.Layer) : new int?();
          int num = !nullable2.HasValue ? 0 : nullable2.Value;
          nullable1 = new bool?(animator.IsInTransition(num));
        }
        bool? nullable3 = nullable1;
        return nullable3.HasValue && nullable3.Value;
      }
    }

    private bool SetStateInfo(int _poseID, ref PlayState _getPlayState)
    {
      this.StateInInfos.Clear();
      PlayState playState = (PlayState) null;
      if (!this.AnimTable.TryGetValue(_poseID, out playState) || playState.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        return false;
      foreach (PlayState.Info stateInfo in playState.MainStateInfo.InStateInfo.StateInfos)
        this.StateInInfos.Add(stateInfo);
      _getPlayState = playState;
      return true;
    }

    public void PlayAnimation(AIProject.Player.Fishing.PoseID _poseType)
    {
      this.currentPoseID = _poseType;
      int _poseID = (int) _poseType;
      PlayState _getPlayState = (PlayState) null;
      if (!this.SetStateInfo(_poseID, ref _getPlayState))
        return;
      this.currentPlayState = _getPlayState;
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      IEnumerator _coroutine = this.StartInAnimation(_getPlayState);
      this.animationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    private bool AnimLoopActive(List<PlayState.Info> _animStates, PlayState _playState)
    {
      return 0 < _animStates.Count && this.currentPlayState == _playState;
    }

    private void CrossFadeInFixedTime(
      int _stateHashName,
      float _fixedTransitionDuration,
      int _layer)
    {
      if (((Behaviour) this.player.Animation.Animator).get_isActiveAndEnabled())
        this.player.Animation.Animator.CrossFadeInFixedTime(_stateHashName, _fixedTransitionDuration, _layer);
      if (!((Behaviour) this.fishingSystem.playerInfo.fishingRodAnimator).get_isActiveAndEnabled())
        return;
      this.fishingSystem.playerInfo.fishingRodAnimator.CrossFadeInFixedTime(_stateHashName, _fixedTransitionDuration, _layer);
    }

    private void CrossFadeInFixedTime(
      string _animName,
      float _fixedTransitionDuration,
      int _layer,
      float _fixedTimeOffset)
    {
      if (((Behaviour) this.player.Animation.Animator).get_isActiveAndEnabled())
        this.player.Animation.Animator.CrossFadeInFixedTime(_animName, _fixedTransitionDuration, _layer, _fixedTimeOffset);
      if (!((Behaviour) this.fishingSystem.playerInfo.fishingRodAnimator).get_isActiveAndEnabled())
        return;
      this.fishingSystem.playerInfo.fishingRodAnimator.CrossFadeInFixedTime(_animName, _fixedTransitionDuration, _layer, _fixedTimeOffset);
    }

    private void Play(int _stateNameHash, int _layer, float _normalizedTime)
    {
      if (((Behaviour) this.player.Animation.Animator).get_isActiveAndEnabled())
        this.player.Animation.Animator.Play(_stateNameHash, _layer, _normalizedTime);
      if (!((Behaviour) this.fishingSystem.playerInfo.fishingRodAnimator).get_isActiveAndEnabled())
        return;
      this.fishingSystem.playerInfo.fishingRodAnimator.Play(_stateNameHash, _layer, _normalizedTime);
    }

    private void Play(string _stateName, int _layer, float _normalizedTime)
    {
      if (((Behaviour) this.player.Animation.Animator).get_isActiveAndEnabled())
        this.player.Animation.Animator.Play(_stateName, _layer, _normalizedTime);
      if (!((Behaviour) this.fishingSystem.playerInfo.fishingRodAnimator).get_isActiveAndEnabled())
        return;
      this.fishingSystem.playerInfo.fishingRodAnimator.Play(_stateName, _layer, _normalizedTime);
    }

    private bool AnimWaitAcitve(
      AnimatorStateInfo _animStateInfo,
      Animator _animator,
      PlayState.Info _stateInfo)
    {
      if (_animator.IsInTransition(_stateInfo.layer))
        return true;
      return ((AnimatorStateInfo) ref _animStateInfo).IsName(_stateInfo.stateName) && (double) ((AnimatorStateInfo) ref _animStateInfo).get_normalizedTime() < 1.0;
    }

    [DebuggerHidden]
    private IEnumerator StartInAnimation(PlayState _playState)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AIProject.Player.Fishing.\u003CStartInAnimation\u003Ec__Iterator1()
      {
        _playState = _playState,
        \u0024this = this
      };
    }

    public void EndFishing()
    {
      if (this.OnFinished == null)
        return;
      this.OnFinished(this.player);
      this.OnFinished = (Action<PlayerActor>) null;
    }

    public void PlayMissAnimation()
    {
      AIProject.Player.Fishing.PoseID poseId = AIProject.Player.Fishing.PoseID.Miss;
      this.currentPoseID = poseId;
      int _poseID = (int) poseId;
      PlayState _getPlayState = (PlayState) null;
      if (!this.SetStateInfo(_poseID, ref _getPlayState))
        return;
      this.currentPlayState = _getPlayState;
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      IEnumerator _coroutine = this.StartMissAnimation(_getPlayState);
      this.animationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator StartMissAnimation(PlayState _playState)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AIProject.Player.Fishing.\u003CStartMissAnimation\u003Ec__Iterator2()
      {
        _playState = _playState,
        \u0024this = this
      };
    }

    public void PlayMissEndAnimation()
    {
      AIProject.Player.Fishing.PoseID poseId = AIProject.Player.Fishing.PoseID.Miss;
      this.currentPoseID = poseId;
      int _poseID = (int) poseId;
      PlayState _getPlayState = (PlayState) null;
      if (!this.SetStateInfo(_poseID, ref _getPlayState))
        return;
      this.currentPlayState = _getPlayState;
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      IEnumerator _coroutine = this.StartMissEndAnimation(_getPlayState);
      this.animationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator StartMissEndAnimation(PlayState _playState)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AIProject.Player.Fishing.\u003CStartMissEndAnimation\u003Ec__Iterator3()
      {
        _playState = _playState,
        \u0024this = this
      };
    }

    public void PlayStopAnimation()
    {
      AIProject.Player.Fishing.PoseID poseId = AIProject.Player.Fishing.PoseID.Stop;
      this.currentPoseID = poseId;
      int _poseID = (int) poseId;
      PlayState _getPlayState = (PlayState) null;
      if (!this.SetStateInfo(_poseID, ref _getPlayState))
        return;
      this.currentPlayState = _getPlayState;
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      IEnumerator _coroutine = this.StartStopAnimation(_getPlayState);
      this.animationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator StartStopAnimation(PlayState _playState)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AIProject.Player.Fishing.\u003CStartStopAnimation\u003Ec__Iterator4()
      {
        _playState = _playState,
        \u0024this = this
      };
    }

    public void PlayStandbyMotion(bool _crossFadePlay = true)
    {
      if (Object.op_Equality((Object) this.player, (Object) null))
        return;
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      bool flag = true;
      this.EndAnimation = flag;
      this.LastAnimation = flag;
      this.currentPlayState = (PlayState) null;
      string str = (string) null;
      if (Singleton<Resources>.IsInstance())
        str = Singleton<Resources>.Instance.FishingDefinePack.PlayerParam.AnimStandbyName;
      if (str.IsNullOrEmpty())
        str = "Locomotion";
      Animator animator = this.player.Animation.Animator;
      if (Object.op_Equality((Object) animator, (Object) null))
        return;
      RuntimeAnimatorController animatorController = animator.get_runtimeAnimatorController();
      if (Object.op_Equality((Object) animatorController, (Object) null))
      {
        animator.set_runtimeAnimatorController(this.fishingAnimController);
        animator.CrossFadeInFixedTime(str, 0.2f, 0, 0.0f);
      }
      else
      {
        if (((Object) animatorController).get_name() == ((Object) this.fishingAnimController).get_name())
        {
          AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
          if (((AnimatorStateInfo) ref animatorStateInfo).IsName(str))
            return;
        }
        if (((Object) animatorController).get_name() != ((Object) this.fishingAnimController).get_name())
          this.player.Animation.Animator.set_runtimeAnimatorController(this.fishingAnimController);
        if (_crossFadePlay)
          this.CrossFadeInFixedTime(str, 0.2f, 0, 0.0f);
        else
          this.Play(str, 0, 0.0f);
      }
    }

    public void ChangeNormalAnimation()
    {
      if (this.animationDisposable != null)
        this.animationDisposable.Dispose();
      bool flag = true;
      this.EndAnimation = flag;
      this.LastAnimation = flag;
      this.currentPlayState = (PlayState) null;
      if (((Object) this.fishingBeforeAnimController).get_name() == ((Object) this.player.Animation.Animator.get_runtimeAnimatorController()).get_name())
      {
        AnimatorStateInfo animatorStateInfo = this.player.Animation.Animator.GetCurrentAnimatorStateInfo(0);
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Locomotion"))
          return;
      }
      if (((Object) this.fishingAnimController).get_name() != ((Object) this.fishingBeforeAnimController).get_name())
        this.player.Animation.Animator.set_runtimeAnimatorController(this.fishingBeforeAnimController);
      this.player.Animation.Animator.CrossFadeInFixedTime("Locomotion", 0.2f, 0, 0.0f);
    }

    private void EventExecute(ref AIProject.Player.Fishing.Schedule _s)
    {
      if (!_s.enable)
        return;
      switch (_s.eventID)
      {
        case 0:
          this.fishingSystem.lure.StartThrow();
          break;
        case 1:
          this.fishingSystem.lure.StartReturn(ParticleType.LureInOut);
          break;
        case 2:
          this.fishingSystem.PlaySE(SEType.FishGet, ((Component) this.fishingSystem.lure).get_transform(), false, 0.0f);
          this.fishingSystem.lure.StartReturn(ParticleType.FishGet);
          break;
        case 3:
          this.fishingSystem.PlaySE(SEType.FishEscape, ((Component) this.fishingSystem.lure).get_transform(), false, 0.0f);
          this.fishingSystem.lure.StartReturn(ParticleType.LureInOut);
          break;
        case 4:
          this.fishingSystem.PlaySE(SEType.LureThrow, ((Component) this.fishingSystem.lure).get_transform(), false, 0.0f);
          break;
      }
      _s.enable = false;
    }

    private void SetActive(Component c, bool a)
    {
      if (Object.op_Equality((Object) c, (Object) null) || c.get_gameObject().get_activeSelf() == a)
        return;
      c.get_gameObject().SetActive(a);
    }

    private void SetActive(GameObject o, bool a)
    {
      if (Object.op_Equality((Object) o, (Object) null) || o.get_activeSelf() == a)
        return;
      o.SetActive(a);
    }

    public enum PoseID
    {
      Idle,
      Hit,
      Success,
      MissToIdle,
      Miss,
      Stop,
    }

    public struct Schedule
    {
      public string animName;
      public int eventID;
      public float startTime;
      public string memo;
      public bool enable;

      public Schedule(string _animName, int _eventID, float _startTime)
      {
        this.animName = _animName;
        this.eventID = _eventID;
        this.startTime = _startTime;
        this.memo = (string) null;
        this.enable = true;
      }

      public Schedule(string _animName, int _eventID, float _startTime, string _memo)
      {
        this.animName = _animName;
        this.eventID = _eventID;
        this.startTime = _startTime;
        this.memo = _memo;
        this.enable = true;
      }

      public override string ToString()
      {
        return string.Format("Fishing.Schedule animName[{0}] eventID[{1}] startTime[{2}] memo[{3}] enable[{4}]", (object) this.animName, (object) this.eventID, (object) this.startTime, (object) this.memo, (object) this.enable);
      }

      public void DebugLog()
      {
        Debug.Log((object) this.ToString());
      }
    }

    public class ScheduleTimeCompare : IComparer<AIProject.Player.Fishing.Schedule>
    {
      private static AIProject.Player.Fishing.ScheduleTimeCompare instance_;

      public static AIProject.Player.Fishing.ScheduleTimeCompare Get
      {
        get
        {
          return AIProject.Player.Fishing.ScheduleTimeCompare.instance_ ?? (AIProject.Player.Fishing.ScheduleTimeCompare.instance_ = new AIProject.Player.Fishing.ScheduleTimeCompare());
        }
      }

      public static void Clear()
      {
        AIProject.Player.Fishing.ScheduleTimeCompare.instance_ = (AIProject.Player.Fishing.ScheduleTimeCompare) null;
      }

      public int Compare(AIProject.Player.Fishing.Schedule _s1, AIProject.Player.Fishing.Schedule _s2)
      {
        return (double) _s1.startTime <= (double) _s2.startTime ? -1 : 1;
      }
    }
  }
}
