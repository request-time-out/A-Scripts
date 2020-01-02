// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Sleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Player
{
  public class Sleep : PlayerStateBase
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
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      if (player.PlayerController.PrevStateName != "Lie")
      {
        player.PlayActionMotion(AIProject.EventType.Sleep);
        ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(1000.0)), (System.Action<M0>) (_ =>
        {
          if (MapUIContainer.OpenOnceTutorial(7, false))
            MapUIContainer.TutorialUI.ClosedEvent = (System.Action) (() => this.OnStart(player));
          else
            this.OnStart(player);
        }));
        player.CameraControl.SetShotTypeForce(ShotType.Near);
      }
      else
        this.OnStart(player);
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(AIProject.EventType.Sleep, out valueTuple);
      int _eventTypeID = (int) valueTuple.Item1;
      player.CameraControl.Mode = CameraMode.ActionFreeLook;
      player.CameraControl.LoadActionCameraFile(_eventTypeID, player.PoseID, (Transform) null);
    }

    private void OnStart(PlayerActor player)
    {
      MapUIContainer.RefreshCommands(0, player.SleepCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (System.Action) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
        player.Animation.RecoveryPoint = (Transform) null;
        player.Animation.RefsActAnimInfo = true;
        player.Controller.ChangeState("Normal");
        player.ReleaseCurrentPoint();
        if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
          ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        player.ActivateNavMeshAgent();
        player.IsKinematic = false;
        MapUIContainer.SetActiveCommandList(false);
      });
      MapUIContainer.SetActiveCommandList(true, "睡眠");
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (__ =>
      {
        Dictionary<int, Dictionary<int, Dictionary<int, PlayState>>> playerActionAnimTable = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable;
        PoseKeyPair wakeupPoseId = Singleton<Resources>.Instance.PlayerProfile.PoseIDData.WakeupPoseID;
        PlayState info = playerActionAnimTable[(int) player.ChaControl.sex][wakeupPoseId.postureID][wakeupPoseId.poseID];
        player.Animation.StopAllAnimCoroutine();
        player.Animation.InitializeStates(info);
        player.ActivateNavMeshAgent();
        player.IsKinematic = false;
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, 0);
        player.Animation.PlayInAnimation(info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.MainStateInfo.FadeOutTime, info.Layer);
        player.CameraControl.Mode = CameraMode.Normal;
        player.CameraControl.RecoverShotType();
        player.CameraControl.EnabledInput = true;
      }));
      this._onEndInAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInAnim, 1), (System.Action<M0>) (__ =>
      {
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
        player.Animation.RefsActAnimInfo = true;
        player.Controller.ChangeState("Normal");
        player.ReleaseCurrentPoint();
        if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
          ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
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
        if ((double) Mathf.Sqrt((float) (moveAxis.x * moveAxis.x + moveAxis.y * moveAxis.y)) <= 0.5)
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
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Sleep.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new Sleep.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
