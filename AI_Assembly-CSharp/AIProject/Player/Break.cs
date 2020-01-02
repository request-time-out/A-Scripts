// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Break
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Break : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private Subject<Unit> _onElapseTime = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Break;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      player.CurrentPoint.SetActiveMapItemObjs(false);
      int index = (int) AIProject.Definitions.Action.NameTable[AIProject.EventType.Break].Item1;
      ActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerActionPointInfo(AIProject.EventType.Break, out outInfo);
      int poseId = outInfo.poseID;
      player.PoseID = poseId;
      int _poseID = poseId;
      Transform t = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      player.Animation.RecoveryPoint = loop?.get_transform();
      PlayState info = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index][_poseID];
      player.Animation.LoadEventKeyTable(index, outInfo.poseID);
      player.LoadEventItems(info);
      player.LoadEventParticles(index, outInfo.poseID);
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
      ActorAnimInfo animInfo = actorAnimInfo1;
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      this._hasAction = info.ActionInfo.hasAction;
      if (this._hasAction)
      {
        this._loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
        this._randomCount = info.ActionInfo.randomCount;
        this._oldNormalizedTime = 0.0f;
      }
      player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onElapseTime, 1), (System.Action<M0>) (_ =>
      {
        if (info.SubStateInfos.IsNullOrEmpty<PlayState.PlayStateInfo>())
          return;
        player.Animation.InStates.Clear();
        PlayState.PlayStateInfo element = info.SubStateInfos.GetElement<PlayState.PlayStateInfo>(Random.Range(0, info.SubStateInfos.Count));
        foreach (PlayState.Info stateInfo in element.InStateInfo.StateInfos)
          player.Animation.InStates.Enqueue(stateInfo);
        ActorAnimInfo actorAnimInfo2 = new ActorAnimInfo()
        {
          layer = info.Layer,
          inEnableBlend = element.InStateInfo.EnableFade,
          inBlendSec = element.InStateInfo.FadeSecond,
          outEnableBlend = element.OutStateInfo.EnableFade,
          outBlendSec = element.OutStateInfo.FadeSecond,
          directionType = info.DirectionType,
          isLoop = element.IsLoop
        };
        player.Animation.AnimInfo = actorAnimInfo2;
        ActorAnimInfo actorAnimInfo3 = actorAnimInfo2;
        player.Animation.AnimABInfo = info.MainStateInfo.AssetBundleInfo;
        player.Animation.PlayInAnimation(actorAnimInfo3.inEnableBlend, actorAnimInfo3.inBlendSec, element.FadeOutTime, actorAnimInfo3.layer);
      }));
      Transform transform = player.CurrentPoint.SetSlot((Actor) player).Item1;
      player.SetStand(t, animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.directionType);
      player.CameraControl.Mode = CameraMode.ActionFreeLook;
      player.CameraControl.SetShotTypeForce(ShotType.Near);
      player.CameraControl.LoadActionCameraFile(index, _poseID, (Transform) null);
      MapUIContainer.StorySupportUI.Close();
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(1000.0)), (System.Action<M0>) (_ =>
      {
        Sprite sprite;
        Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.PlayerProfile.CommonActionIconID, out sprite);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.CancelAcception);
        MapUIContainer.CommandLabel.CancelCommand = new CommandLabel.CommandInfo()
        {
          Text = "立ち上がる",
          Icon = sprite,
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
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
              MapUIContainer.StorySupportUI.Open();
            }));
          })
        };
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.ClearItems();
      player.ClearParticles();
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null))
        player.CurrentPoint.SetActiveMapItemObjs(true);
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
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
        return;
      this._onElapseTime.OnNext(Unit.get_Default());
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Break.\u003COnEnd\u003Ec__Iterator0()
      {
        player = player,
        \u0024this = this
      };
    }
  }
}
