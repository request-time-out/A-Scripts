// Decompiled with JetBrains decompiler
// Type: AIProject.Player.OpenHarbordoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class OpenHarbordoor : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private Subject<Unit> _onComplete = new Subject<Unit>();
    private PoseKeyPair _poseInfo = new PoseKeyPair();
    private EventPoint _eventPoint;
    private HarborDoorAnimData _animData;
    private PlayState _info;
    private bool _initSuccess;
    private CommandLabel.AcceptionState _prevAcceptionState;

    protected override void OnAwake(PlayerActor player)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) "バルブドア 開放処理開始");
      this._eventPoint = player.CurrentEventPoint;
      if (Object.op_Equality((Object) this._eventPoint, (Object) null))
      {
        this.ErrorEnd(player, "イベントポイント持っていないのにバルブ扉を開こうとした");
      }
      else
      {
        if (Object.op_Equality((Object) this._eventPoint, (Object) EventPoint.GetTargetPoint()))
          EventPoint.SetTargetID(-1, -1);
        HarborDoorAnimation component = (HarborDoorAnimation) ((Component) this._eventPoint).GetComponent<HarborDoorAnimation>();
        if (Object.op_Equality((Object) component, (Object) null))
        {
          this.ErrorEnd(player, "イベントポイントからHarborDoorAnimationが取得できなかった");
        }
        else
        {
          this._prevAcceptionState = MapUIContainer.CommandLabel.Acception;
          if (this._prevAcceptionState != CommandLabel.AcceptionState.None)
            MapUIContainer.CommandLabel.Acception = CommandLabel.AcceptionState.None;
          this._animData = component.AnimData;
          player.EventKey = EventType.DoorOpen;
          this._poseInfo = component.PoseInfo;
          player.SetActiveOnEquipedItem(false);
          player.ChaControl.setAllLayerWeight(0.0f);
          Transform t = component.BasePoint ?? ((Component) this._eventPoint).get_transform();
          player.Animation.RecoveryPoint = component.RecoveryPoint;
          int sex = (int) player.ChaControl.sex;
          int postureId = this._poseInfo.postureID;
          int poseId = this._poseInfo.poseID;
          this._info = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[sex][postureId][poseId];
          ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
          {
            layer = this._info.Layer,
            inEnableBlend = this._info.MainStateInfo.InStateInfo.EnableFade,
            inBlendSec = this._info.MainStateInfo.InStateInfo.FadeSecond,
            outEnableBlend = this._info.MainStateInfo.OutStateInfo.EnableFade,
            outBlendSec = this._info.MainStateInfo.OutStateInfo.FadeSecond,
            directionType = this._info.DirectionType,
            endEnableBlend = this._info.EndEnableBlend,
            endBlendSec = this._info.EndBlendRate
          };
          player.Animation.AnimInfo = actorAnimInfo;
          ActorAnimInfo animInfo = actorAnimInfo;
          player.Animation.LoadSEEventKeyTable(postureId, poseId);
          player.Animation.InitializeStates(this._info.MainStateInfo.InStateInfo.StateInfos, this._info.MainStateInfo.OutStateInfo.StateInfos, this._info.MainStateInfo.AssetBundleInfo);
          player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, this._info.MainStateInfo.FadeOutTime, animInfo.layer);
          ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
          player.DeactivateNavMeshAgent();
          player.IsKinematic = true;
          player.SetStand(t, this._info.MainStateInfo.InStateInfo.EnableFade, this._info.MainStateInfo.InStateInfo.FadeSecond, this._info.DirectionType);
          if (Object.op_Inequality((Object) this._animData, (Object) null))
          {
            this._animData.AnimEndAction = (Action) (() => this._animData.PlayOpenIdleAnimation(false, 0.0f, 0.0f, 0));
            this._animData.PlayToOpenAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, this._info.MainStateInfo.FadeOutTime, animInfo.layer);
          }
          ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onComplete, 1), (Action<M0>) (_ =>
          {
            if (this._animData is AreaOpenLinkedHarborDoorAnimData)
              Singleton<Manager.Map>.Instance.SetOpenAreaState((this._animData as AreaOpenLinkedHarborDoorAnimData).AreaOpenID, true);
            if (this._eventPoint.GroupID == 1)
            {
              switch (this._eventPoint.PointID)
              {
                case 2:
                  Manager.Map.ForcedSetTutorialProgress(26);
                  break;
              }
            }
            if (this._prevAcceptionState != MapUIContainer.CommandLabel.Acception)
              MapUIContainer.CommandLabel.Acception = this._prevAcceptionState;
            player.ActivateNavMeshAgent();
            player.PlayerController.ChangeState("Normal");
          }));
          player.CameraControl.Mode = CameraMode.ActionFreeLook;
          player.CameraControl.SetShotTypeForce(ShotType.Near);
          player.CameraControl.LoadActionCameraFile(postureId, poseId, (Transform) null);
          this._initSuccess = true;
        }
      }
    }

    private void ErrorEnd(PlayerActor player, string log)
    {
      player.PlayerController.ChangeState("Normal");
      if (!Debug.get_isDebugBuild())
        return;
      Debug.LogWarning((object) log);
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (player.Animation.PlayingInAnimation)
        return;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (player.Animation.PlayingOutAnimation)
        return;
      bool? nullable = this._animData != null ? new bool?(this._animData.PlayingAnimation) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 || this._onComplete == null)
        return;
      this._onComplete.OnNext(Unit.get_Default());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.CurrentEventPoint = (EventPoint) null;
      if (!this._initSuccess)
        return;
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
      player.Animation.RefsActAnimInfo = true;
    }
  }
}
