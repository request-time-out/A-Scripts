// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Move
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Player
{
  public class Move : PlayerStateBase
  {
    protected int _currentState = -1;
    private Subject<Unit> _onEndAction = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(AIProject.EventType.Move, out valueTuple);
      int index1 = (int) valueTuple.Item1;
      ActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerActionPointInfo(AIProject.EventType.Move, out outInfo);
      player.CurrentPoint.SetBookingUser((Actor) player);
      Transform transform = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      player.Animation.RecoveryPoint = loop?.get_transform();
      int poseId = outInfo.poseID;
      player.PoseID = poseId;
      int index2 = poseId;
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index1][index2];
      player.DeactivateNavMeshAgent();
      ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
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
      player.Animation.AnimInfo = actorAnimInfo;
      ActorAnimInfo animInfo = actorAnimInfo;
      player.Animation.LoadSEEventKeyTable(index1, index2);
      player.Animation.InitializeStates(playState.MainStateInfo.InStateInfo.StateInfos, playState.MainStateInfo.OutStateInfo.StateInfos, playState.MainStateInfo.AssetBundleInfo);
      player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, playState.MainStateInfo.FadeOutTime, animInfo.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      player.SetStand(transform, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
      player.CameraControl.Mode = CameraMode.ActionNotMove;
      player.CameraControl.SetShotTypeForce(ShotType.Near);
      player.CameraControl.LoadActionCameraFile(index1, index2, transform);
    }

    protected override void OnRelease(PlayerActor player)
    {
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null) && player.CurrentPoint.RemoveBooking((Actor) player))
      {
        player.CurrentPoint.SetImpossible(false, (Actor) player);
        CommandArea commandArea = player.PlayerController.CommandArea;
        commandArea.RemoveConsiderationObject((ICommandable) player.CurrentPoint);
        commandArea.RefreshCommands();
      }
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
      player.Animation.RefsActAnimInfo = true;
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      if (actor.Animation.PlayingInAnimation)
        return;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (actor.Animation.PlayingOutAnimation)
        return;
      this.Elapsed(actor);
    }

    private void Elapsed(PlayerActor player)
    {
      if (Manager.Map.TutorialMode && Singleton<Resources>.IsInstance() && Manager.Map.GetTutorialProgress() == 14)
      {
        ActionPoint currentPoint = player.CurrentPoint;
        int? yotunbaiRegisterId = Singleton<Resources>.Instance.CommonDefine?.Tutorial?.YotunbaiRegisterID;
        if (Object.op_Inequality((Object) currentPoint, (Object) null) && yotunbaiRegisterId.HasValue && currentPoint.RegisterID == yotunbaiRegisterId.Value)
          Manager.Map.SetTutorialProgress(15);
      }
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null))
      {
        player.CurrentPoint.RemoveBooking((Actor) player);
        CommandArea commandArea = player.PlayerController.CommandArea;
        commandArea.RemoveConsiderationObject((ICommandable) player.CurrentPoint);
        commandArea.RefreshCommands();
      }
      if (player.PlayerController.PrevStateName == "Follow")
        player.PlayerController.ChangeState("Follow");
      else
        player.Controller.ChangeState("Normal");
    }
  }
}
