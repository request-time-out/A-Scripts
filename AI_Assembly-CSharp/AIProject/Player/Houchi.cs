// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Houchi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Houchi : PlayerStateBase
  {
    private Subject<Unit> _onEndInput = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = (EventType) 0;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      Resources instance = Singleton<Resources>.Instance;
      PoseKeyPair leftPoseId = instance.PlayerProfile.PoseIDData.LeftPoseID;
      PlayState info = instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][leftPoseId.postureID][leftPoseId.poseID];
      player.Animation.InitializeStates(info);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = info.Layer,
        inEnableBlend = info.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = info.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = info.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = info.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        isLoop = info.MainStateInfo.IsLoop,
        endEnableBlend = info.EndEnableBlend,
        endBlendSec = info.EndBlendRate
      };
      player.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      this._hasAction = info.ActionInfo.hasAction;
      if (this._hasAction)
      {
        this._loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
        this._randomCount = info.ActionInfo.randomCount;
        this._oldNormalizedTime = 0.0f;
      }
      bool enabled = ((Behaviour) player.HandsHolder).get_enabled();
      player.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) player.HandsHolder).set_enabled(false);
        if (player.HandsHolder.EnabledHolding)
          player.HandsHolder.EnabledHolding = false;
      }
      player.Animation.StopAllAnimCoroutine();
      player.Animation.PlayInLocoAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInput, 1), (Action<M0>) (__ =>
      {
        player.CameraControl.CrossFade.FadeStart(-1f);
        if (player.Animation.PlayingInLocoAnimation)
          player.Animation.StopInLocoAnimCoroutine();
        if (player.Animation.PlayingActAnimation)
          player.Animation.StopActionAnimCoroutine();
        player.CameraControl.Mode = CameraMode.Normal;
        player.Controller.ChangeState("Normal");
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      base.OnRelease(player);
      if (!player.OldEnabledHoldingHand)
        return;
      ((Behaviour) player.HandsHolder).set_enabled(true);
      player.OldEnabledHoldingHand = false;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      Singleton<Manager.Map>.Instance.CheckStoryProgress();
      Vector2 moveAxis = Singleton<Input>.Instance.MoveAxis;
      if (Math.Sqrt((double) (moveAxis.x * moveAxis.x + moveAxis.y * moveAxis.y)) > 0.5)
        this._onEndInput.OnNext(Unit.get_Default());
      if (actor.Animation.PlayingInLocoAnimation || actor.Animation.PlayingActAnimation)
        return;
      AnimatorStateInfo animatorStateInfo = actor.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      if (!((AnimatorStateInfo) ref animatorStateInfo).IsName(this._loopStateName) || (double) (((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() - this._oldNormalizedTime) <= 1.0)
        return;
      this._oldNormalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
      if (Random.Range(0, this._randomCount) != 0)
        return;
      actor.Animation.PlayActionAnimation(actor.Animation.AnimInfo.layer);
      this._oldNormalizedTime = 0.0f;
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Houchi.\u003COnEnd\u003Ec__Iterator0()
      {
        player = player
      };
    }
  }
}
