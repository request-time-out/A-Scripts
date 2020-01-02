// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Menu : PlayerStateBase
  {
    private Subject<Unit> _onEndInAnimation = new Subject<Unit>();
    private Subject<Unit> _onEndMenu = new Subject<Unit>();
    private Subject<Unit> _onEndInput = new Subject<Unit>();
    private Subject<Unit> _onEndOutAnimation = new Subject<Unit>();
    private IDisposable _onEndInputDisposable;
    private IDisposable _onEndOutAnimDisposable;

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = (EventType) 0;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      if (player.PlayerController.PrevStateName != "Onbu")
      {
        Resources instance = Singleton<Resources>.Instance;
        PoseKeyPair menuPoseId = instance.PlayerProfile.PoseIDData.MenuPoseID;
        PlayState playState = instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][menuPoseId.postureID][menuPoseId.poseID];
        player.Animation.LoadEventKeyTable(menuPoseId.postureID, menuPoseId.poseID);
        if (player.ChaControl.visibleAll)
        {
          player.LoadEventItems(playState);
          player.LoadEventParticles(menuPoseId.postureID, menuPoseId.poseID);
        }
        player.Animation.InitializeStates(playState);
        ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
        {
          layer = playState.Layer,
          inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
          inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
          outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
          outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
          directionType = playState.DirectionType,
          isLoop = playState.MainStateInfo.IsLoop,
          endEnableBlend = playState.EndEnableBlend,
          endBlendSec = playState.EndBlendRate
        };
        player.Animation.AnimInfo = actorAnimInfo;
        ActorAnimInfo animInfo = actorAnimInfo;
        player.OldEnabledHoldingHand = ((Behaviour) player.HandsHolder).get_enabled();
        if (player.OldEnabledHoldingHand)
        {
          ((Behaviour) player.HandsHolder).set_enabled(false);
          if (player.HandsHolder.EnabledHolding)
            player.HandsHolder.EnabledHolding = false;
        }
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
        player.Animation.StopAllAnimCoroutine();
        player.Animation.PlayInLocoAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.layer);
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
        this._onEndOutAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndOutAnimation, 1), (Action<M0>) (_ =>
        {
          if (this._onEndInputDisposable != null)
            this._onEndInputDisposable.Dispose();
          this.EndState(player);
        }));
      }
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInAnimation, 1), (Action<M0>) (_ => MapUIContainer.SetActiveSystemMenuUI(true)));
      this._onEndInputDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInput, 1), (Action<M0>) (_ =>
      {
        if (this._onEndOutAnimDisposable != null)
          this._onEndOutAnimDisposable.Dispose();
        this.EndState(player);
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.ClearItems();
      player.ClearParticles();
      if (!player.OldEnabledHoldingHand)
        return;
      ((Behaviour) player.HandsHolder).set_enabled(true);
      player.OldEnabledHoldingHand = false;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (actor.Animation.PlayingInLocoAnimation)
        return;
      if (this._onEndInAnimation != null)
        this._onEndInAnimation.OnNext(Unit.get_Default());
      if (MapUIContainer.SystemMenuUI.IsActiveControl)
        return;
      this._onEndMenu.OnNext(Unit.get_Default());
      if (actor.Animation.PlayingOutAnimation)
      {
        Vector2 moveAxis = Singleton<Input>.Instance.MoveAxis;
        if ((double) Mathf.Sqrt((float) (moveAxis.x * moveAxis.x + moveAxis.y * moveAxis.y)) <= 0.5)
          return;
        this._onEndInput.OnNext(Unit.get_Default());
      }
      else
        this._onEndInput.OnNext(Unit.get_Default());
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Menu.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new Menu.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }

    private void EndState(PlayerActor player)
    {
      StuffItem equipedLampItem = player.PlayerData.EquipedLampItem;
      ItemIDKeyPair torchId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.TorchID;
      ItemIDKeyPair maleLampId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.MaleLampID;
      ItemIDKeyPair flashlightId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.FlashlightID;
      if (equipedLampItem.CategoryID == torchId.categoryID && equipedLampItem.ID == torchId.itemID || equipedLampItem.CategoryID == maleLampId.categoryID && equipedLampItem.ID == maleLampId.itemID || equipedLampItem.CategoryID == flashlightId.categoryID && equipedLampItem.ID == flashlightId.itemID)
        player.CameraControl.CrossFade.FadeStart(-1f);
      if (player.PlayerController.PrevStateName == "Onbu")
        player.PlayerController.ChangeState("Onbu");
      else
        player.PlayerController.ChangeState("Normal");
    }
  }
}
