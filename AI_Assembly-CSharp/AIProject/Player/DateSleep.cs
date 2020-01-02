// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DateSleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class DateSleep : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private Subject<Unit> _onEndInAnim = new Subject<Unit>();
    private IDisposable _onEndActionDisposable;
    private IDisposable _onEndInAnimDisposable;

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Sleep;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(1000.0)), (System.Action<M0>) (_ => this.OnStart(player)));
      int eventID = (int) AIProject.Definitions.Action.NameTable[AIProject.EventType.Sleep].Item1;
      DateActionPointInfo apInfo;
      player.CurrentPoint.TryGetPlayerDateActionPointInfo(player.ChaControl.sex, AIProject.EventType.Sleep, out apInfo);
      int poseIda = apInfo.poseIDA;
      player.PoseID = poseIda;
      int index = poseIda;
      Transform t1 = ((Component) player.CurrentPoint).get_transform().FindLoop(apInfo.baseNullNameA)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      Transform t2 = ((Component) player.CurrentPoint).get_transform().FindLoop(apInfo.baseNullNameB)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop1 = ((Component) player.CurrentPoint).get_transform().FindLoop(apInfo.recoveryNullNameA);
      player.Animation.RecoveryPoint = loop1?.get_transform();
      GameObject loop2 = ((Component) player.CurrentPoint).get_transform().FindLoop(apInfo.recoveryNullNameB);
      ActorAnimation animation = player.Partner.Animation;
      Transform transform1 = loop2?.get_transform();
      player.Partner.Animation.RecoveryPoint = transform1;
      Transform transform2 = transform1;
      animation.RecoveryPoint = transform2;
      PlayState playState1 = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][eventID][index];
      player.Animation.LoadEventKeyTable(eventID, apInfo.poseIDA);
      player.LoadEventItems(playState1);
      player.LoadEventParticles(eventID, apInfo.poseIDA);
      player.Animation.InitializeStates(playState1);
      Actor partner = player.Partner;
      partner.Animation.LoadEventKeyTable(eventID, apInfo.poseIDB);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eventID][apInfo.poseIDB];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(eventID, apInfo.poseIDB);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState1.Layer,
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop
      };
      player.Partner.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      player.Animation.AnimInfo = actorAnimInfo2;
      ActorAnimInfo actorAnimInfo3 = actorAnimInfo2;
      ActorAnimInfo actorAnimInfo4 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState2.DirectionType,
        isLoop = playState2.MainStateInfo.IsLoop,
        loopMinTime = playState1.MainStateInfo.LoopMin,
        loopMaxTime = playState1.MainStateInfo.LoopMax,
        hasAction = playState1.ActionInfo.hasAction
      };
      partner.Animation.AnimInfo = actorAnimInfo4;
      ActorAnimInfo actorAnimInfo5 = actorAnimInfo4;
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this._hasAction = playState1.ActionInfo.hasAction;
      if (this._hasAction)
      {
        this._loopStateName = playState1.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState1.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
        this._randomCount = playState1.ActionInfo.randomCount;
        this._oldNormalizedTime = 0.0f;
      }
      player.Animation.PlayInAnimation(actorAnimInfo3.inEnableBlend, actorAnimInfo3.inBlendSec, playState1.MainStateInfo.FadeOutTime, actorAnimInfo3.layer);
      player.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, playState1.DirectionType);
      partner.Animation.PlayInAnimation(actorAnimInfo3.inEnableBlend, actorAnimInfo3.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo3.layer);
      partner.SetStand(t2, actorAnimInfo5.inEnableBlend, actorAnimInfo5.inBlendSec, actorAnimInfo3.layer);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ =>
      {
        if (apInfo.pointID == 501)
        {
          ADV.ChangeADVFixedAngleCamera((Actor) player, 5);
        }
        else
        {
          if (apInfo.pointID != 500)
            return;
          ADV.ChangeADVFixedAngleCamera(partner, 5);
        }
      }));
      bool enabled = ((Behaviour) player.HandsHolder).get_enabled();
      player.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) player.HandsHolder).set_enabled(false);
        if (player.HandsHolder.EnabledHolding)
          player.HandsHolder.EnabledHolding = false;
      }
      player.CameraControl.SetShotTypeForce(ShotType.Near);
    }

    private void OnStart(PlayerActor player)
    {
      MapUIContainer.RefreshCommands(0, player.CoSleepCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (System.Action) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        player.CancelCommand();
        MapUIContainer.SetVisibleHUDExceptStoryUI(true);
        MapUIContainer.StorySupportUI.Open();
      });
      MapUIContainer.SetActiveCommandList(true, "睡眠");
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ =>
      {
        Dictionary<int, Dictionary<int, Dictionary<int, PlayState>>> playerActionAnimTable = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable;
        PoseKeyPair wakeupPoseId = Singleton<Resources>.Instance.PlayerProfile.PoseIDData.WakeupPoseID;
        PlayState info = playerActionAnimTable[(int) player.ChaControl.sex][wakeupPoseId.postureID][wakeupPoseId.poseID];
        player.Animation.StopAllAnimCoroutine();
        player.Animation.InitializeStates(info);
        player.SetStand(player.Animation.RecoveryPoint, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, 0);
        player.Animation.PlayInAnimation(info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.MainStateInfo.FadeOutTime, info.Layer);
        player.CameraControl.Mode = CameraMode.Normal;
        player.CameraControl.RecoverShotType();
        player.CameraControl.EnabledInput = true;
      }));
      this._onEndInAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInAnim, 1), (System.Action<M0>) (_ =>
      {
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
        player.Animation.RefsActAnimInfo = true;
        player.Controller.ChangeState("Normal");
        player.ReleaseCurrentPoint();
        if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
          ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        player.ActivateNavMeshAgent();
        player.IsKinematic = false;
      }));
    }

    public override void Release(Actor actor, AIProject.EventType type)
    {
      this.OnRelease(actor as PlayerActor);
    }

    protected override void OnRelease(PlayerActor player)
    {
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.Action);
      Singleton<Manager.Input>.Instance.SetupState();
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
      player.ClearParticles();
      player.Partner?.ClearParticles();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onEndInAnimDisposable == null)
        return;
      this._onEndInAnimDisposable.Dispose();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.CommandList.IsActiveControl || player.ProcessingTimeSkip)
        return;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (player.Animation.PlayingInAnimation)
      {
        Vector2 moveAxis = Singleton<Manager.Input>.Instance.MoveAxis;
        if ((double) Mathf.Sqrt((float) (moveAxis.x * moveAxis.x + moveAxis.y + moveAxis.y)) <= 0.5)
          return;
        this._onEndInAnim.OnNext(Unit.get_Default());
      }
      else
      {
        if (this._onEndInAnim == null)
          return;
        this._onEndInAnim.OnNext(Unit.get_Default());
      }
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DateSleep.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new DateSleep.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
