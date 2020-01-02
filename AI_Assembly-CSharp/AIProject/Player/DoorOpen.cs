// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DoorOpen
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
  public class DoorOpen : PlayerStateBase
  {
    protected int _currentState = -1;
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private DoorAnimation _doorAnimation;

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.DoorOpen;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(AIProject.EventType.DoorOpen, out valueTuple);
      int index1 = (int) valueTuple.Item1;
      ActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerActionPointInfo(AIProject.EventType.DoorOpen, out outInfo);
      player.CurrentPoint.SetBookingUser((Actor) player);
      int poseId = outInfo.poseID;
      player.PoseID = poseId;
      int index2 = poseId;
      PlayState info = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index1][index2];
      ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
      {
        layer = info.Layer,
        inEnableBlend = info.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = info.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = info.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = info.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        endEnableBlend = info.EndEnableBlend,
        endBlendSec = info.EndBlendRate
      };
      player.Animation.AnimInfo = actorAnimInfo;
      ActorAnimInfo animInfo = actorAnimInfo;
      if (player.PlayerController.PrevStateName != "Onbu")
      {
        Transform t = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
        GameObject loop = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
        player.Animation.RecoveryPoint = loop?.get_transform();
        player.Animation.InitializeStates(info.MainStateInfo.InStateInfo.StateInfos, info.MainStateInfo.OutStateInfo.StateInfos, info.MainStateInfo.AssetBundleInfo);
        player.Animation.LoadAnimatorIfNotEquals(info);
        player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
        player.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      }
      player.Animation.LoadSEEventKeyTable(outInfo.eventID, outInfo.poseID);
      if (!Object.op_Inequality((Object) player.CurrentPoint, (Object) null))
        return;
      this._doorAnimation = (DoorAnimation) ((Component) player.CurrentPoint).GetComponent<DoorAnimation>();
      if (!Object.op_Inequality((Object) this._doorAnimation, (Object) null))
        return;
      this._doorAnimation.Load(info.MainStateInfo.InStateInfo.StateInfos);
      this._doorAnimation.PlayAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null) && player.CurrentPoint.RemoveBooking((Actor) player))
      {
        player.CurrentPoint.SetImpossible(false, (Actor) player);
        CommandArea commandArea = player.PlayerController.CommandArea;
        commandArea.RemoveConsiderationObject((ICommandable) player.CurrentPoint);
        commandArea.RefreshCommands();
      }
      player.Animation.ResetDefaultAnimatorController();
      int index = (int) AIProject.Definitions.Action.NameTable[AIProject.EventType.DoorOpen].Item1;
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index][player.PoseID];
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, playState.DirectionType);
      player.Animation.RefsActAnimInfo = true;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (actor.Animation.PlayingInAnimation || Object.op_Inequality((Object) this._doorAnimation, (Object) null) && this._doorAnimation.PlayingOpenAnim)
        return;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (actor.Animation.PlayingOutAnimation)
        return;
      this.Elapsed(actor);
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    private void Elapsed(PlayerActor player)
    {
      DoorPoint currentPoint = player.CurrentPoint as DoorPoint;
      if (Object.op_Inequality((Object) currentPoint, (Object) null))
      {
        if (currentPoint.OpenType == DoorPoint.OpenTypeState.Right || currentPoint.OpenType == DoorPoint.OpenTypeState.Right90)
          currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenRight, true);
        else
          currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenLeft, true);
      }
      player.PlayerController.CommandArea.RefreshCommands();
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null))
      {
        player.CurrentPoint.RemoveBooking((Actor) player);
        CommandArea commandArea = player.PlayerController.CommandArea;
        commandArea.RemoveConsiderationObject((ICommandable) player.CurrentPoint);
        commandArea.RefreshCommands();
      }
      if (player.PlayerController.PrevStateName == "Onbu")
        player.Controller.ChangeState(player.PlayerController.PrevStateName);
      else if (player.PlayerController.PrevStateName == "Follow")
        player.PlayerController.ChangeState("Follow");
      else
        player.Controller.ChangeState("Normal");
    }
  }
}
