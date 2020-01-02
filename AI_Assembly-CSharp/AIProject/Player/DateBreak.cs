// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DateBreak
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class DateBreak : PlayerStateBase
  {
    private Subject<Unit> _onElapseTime = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Break;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      int index = (int) AIProject.Definitions.Action.NameTable[AIProject.EventType.Break].Item1;
      DateActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerDateActionPointInfo(player.ChaControl.sex, AIProject.EventType.Break, out outInfo);
      int poseIda = outInfo.poseIDA;
      player.PoseID = poseIda;
      int _poseID = poseIda;
      Transform transform = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      Transform t = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameB)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop1 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA);
      player.Animation.RecoveryPoint = loop1?.get_transform();
      GameObject loop2 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameB);
      player.Partner.Animation.RecoveryPoint = loop2?.get_transform();
      PlayState info = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index][_poseID];
      player.Animation.LoadEventKeyTable(index, outInfo.poseIDA);
      player.LoadEventItems(info);
      player.LoadEventParticles(index, outInfo.poseIDA);
      player.Animation.InitializeStates(info);
      Actor partner = player.Partner;
      partner.Animation.LoadEventKeyTable(index, outInfo.poseIDB);
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[index][outInfo.poseIDB];
      partner.LoadEventItems(playState);
      partner.LoadEventParticles(index, outInfo.poseIDB);
      partner.Animation.InitializeStates(playState);
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
      ActorAnimInfo actorAnimInfo3 = new ActorAnimInfo()
      {
        layer = playState.Layer,
        inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        isLoop = info.MainStateInfo.IsLoop,
        loopMinTime = playState.MainStateInfo.LoopMin,
        loopMaxTime = playState.MainStateInfo.LoopMax,
        hasAction = playState.ActionInfo.hasAction
      };
      partner.Animation.AnimInfo = actorAnimInfo3;
      ActorAnimInfo actorAnimInfo4 = actorAnimInfo3;
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this._hasAction = info.ActionInfo.hasAction;
      if (this._hasAction)
      {
        this._loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
        this._randomCount = info.ActionInfo.randomCount;
        this._oldNormalizedTime = 0.0f;
      }
      ((MonoBehaviour) player).StopAllCoroutines();
      player.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, info.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      player.SetStand(transform, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      ((MonoBehaviour) partner).StopAllCoroutines();
      partner.Animation.PlayInAnimation(actorAnimInfo4.inEnableBlend, actorAnimInfo4.inBlendSec, playState.MainStateInfo.FadeOutTime, actorAnimInfo4.layer);
      partner.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, actorAnimInfo4.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onElapseTime, 1), (System.Action<M0>) (_ =>
      {
        if (info.SubStateInfos.IsNullOrEmpty<PlayState.PlayStateInfo>())
          return;
        player.Animation.InStates.Clear();
        foreach (PlayState.Info stateInfo in info.SubStateInfos.GetElement<PlayState.PlayStateInfo>(Random.Range(0, info.SubStateInfos.Count)).InStateInfo.StateInfos)
          player.Animation.InStates.Enqueue(stateInfo);
      }));
      player.OldEnabledHoldingHand = ((Behaviour) player.HandsHolder).get_enabled();
      if (player.OldEnabledHoldingHand)
      {
        ((Behaviour) player.HandsHolder).set_enabled(false);
        if (player.HandsHolder.EnabledHolding)
          player.HandsHolder.EnabledHolding = false;
      }
      player.CameraControl.Mode = CameraMode.ActionFreeLook;
      player.CameraControl.SetShotTypeForce(ShotType.Near);
      player.CameraControl.LoadActionCameraFile(index, _poseID, transform);
      Sprite actionIcon;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.PlayerProfile.CommonActionIconID, out actionIcon);
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(1.0)), (System.Action<M0>) (_ =>
      {
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.CancelAcception);
        MapUIContainer.CommandLabel.CancelCommand = new CommandLabel.CommandInfo()
        {
          Text = "立ち上がる",
          Icon = actionIcon,
          TargetSpriteInfo = (CommandTargetSpriteInfo) null,
          Transform = (Transform) null,
          Event = (System.Action) (() =>
          {
            MapUIContainer.CommandLabel.CancelCommand = (CommandLabel.CommandInfo) null;
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => player.Controller.State.End((Actor) player)), false), 1), (System.Action<M0>) (__ =>
            {
              player.PlayerController.ChangeState("Normal");
              player.CameraControl.Mode = CameraMode.Normal;
              player.CameraControl.RecoverShotType();
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
              AgentActor agentPartner = player.AgentPartner;
              agentPartner.ActivateNavMeshAgent();
              agentPartner.SetActiveOnEquipedItem(true);
              ActorAnimInfo animInfo = agentPartner.Animation.AnimInfo;
              agentPartner.SetStand(agentPartner.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
              if (player.OldEnabledHoldingHand)
              {
                ((Behaviour) player.HandsHolder).set_enabled(true);
                player.OldEnabledHoldingHand = false;
              }
              agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Date);
            }));
          })
        };
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      Vector3 locatedPosition = player.CurrentPoint.LocatedPosition;
      locatedPosition.y = ((Component) player.Locomotor).get_transform().get_position().y;
      RaycastHit raycastHit;
      Physics.Raycast(locatedPosition, Vector3.op_Multiply(Vector3.get_down(), 10f), ref raycastHit);
      locatedPosition.y = ((RaycastHit) ref raycastHit).get_point().y;
      ((Component) player.Locomotor).get_transform().set_position(locatedPosition);
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
      player.ClearParticles();
      player.Partner?.ClearParticles();
      player.SetActiveOnEquipedItem(true);
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (player.Animation.PlayingInAnimation)
        return;
      AnimatorStateInfo animatorStateInfo = player.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      if (!((AnimatorStateInfo) ref animatorStateInfo).IsName(this._loopStateName))
        return;
      this._elapsedTime += Time.get_deltaTime();
      if ((double) this._elapsedTime <= (double) Singleton<Resources>.Instance.LocomotionProfile.TimeToLeftState)
        ;
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
      DateBreak.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new DateBreak.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
