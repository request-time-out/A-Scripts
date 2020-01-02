// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Search
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using IllusionUtility.GetUtility;
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
  public class Search : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Search;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(AIProject.EventType.Search, out valueTuple);
      int index = (int) valueTuple.Item1;
      ActionPointInfo outInfo;
      if (player.CurrentPoint is TutorialSearchActionPoint)
        outInfo = (player.CurrentPoint as TutorialSearchActionPoint).GetPlayerActionPointInfo();
      else
        player.CurrentPoint.TryGetPlayerActionPointInfo(AIProject.EventType.Search, out outInfo);
      int poseId = outInfo.poseID;
      player.PoseID = poseId;
      int _poseID = poseId;
      Transform t = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      player.Animation.RecoveryPoint = loop?.get_transform();
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index][_poseID];
      player.Animation.LoadEventKeyTable(index, outInfo.poseID);
      player.LoadEventItems(playState);
      player.LoadEventParticles(index, outInfo.poseID);
      player.Animation.InitializeStates(playState.MainStateInfo.InStateInfo.StateInfos, playState.MainStateInfo.OutStateInfo.StateInfos, playState.MainStateInfo.AssetBundleInfo);
      ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
      {
        layer = playState.Layer,
        inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
        inFadeOutTime = playState.MainStateInfo.FadeOutTime,
        outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState.DirectionType,
        endEnableBlend = playState.EndEnableBlend,
        endBlendSec = playState.EndBlendRate
      };
      player.Animation.AnimInfo = actorAnimInfo;
      ActorAnimInfo animInfo = actorAnimInfo;
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.inFadeOutTime, animInfo.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      player.SetStand(t, animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.directionType);
      player.CameraControl.Mode = CameraMode.ActionFreeLook;
      player.CameraControl.SetShotTypeForce(ShotType.Near);
      player.CameraControl.LoadActionCameraFile(index, _poseID, (Transform) null);
    }

    private void Elapsed(PlayerActor player)
    {
      ActionPoint currentPoint = player.CurrentPoint;
      System.Type type = ((object) currentPoint).GetType();
      if (Singleton<Game>.IsInstance() && type == typeof (SearchActionPoint))
      {
        Dictionary<int, AIProject.SaveData.Environment.SearchActionInfo> searchActionLockTable = Singleton<Game>.Instance.Environment.SearchActionLockTable;
        AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
        if (!searchActionLockTable.TryGetValue(currentPoint.RegisterID, out searchActionInfo))
          searchActionInfo = new AIProject.SaveData.Environment.SearchActionInfo();
        ++searchActionInfo.Count;
        searchActionLockTable[currentPoint.RegisterID] = searchActionInfo;
      }
      Actor.SearchInfo searchInfo = new Actor.SearchInfo()
      {
        IsSuccess = false
      };
      if (currentPoint is SearchActionPoint)
      {
        SearchActionPoint searchActionPoint = currentPoint as SearchActionPoint;
        Dictionary<int, ItemTableElement> itemTableInArea = Singleton<Resources>.Instance.GameInfo.GetItemTableInArea(searchActionPoint.IDList.IsNullOrEmpty<int>() ? searchActionPoint.ID : searchActionPoint.IDList.GetElement<int>(0));
        searchInfo = player.RandomAddItem(itemTableInArea, true);
      }
      else if (currentPoint is TutorialSearchActionPoint)
        searchInfo = (currentPoint as TutorialSearchActionPoint).GetSearchInfo();
      if (type == typeof (OnceSearchActionPoint))
      {
        OnceSearchActionPoint searchActionPoint = currentPoint as OnceSearchActionPoint;
        if (searchActionPoint.HaveMapItems)
          Manager.Map.FadeStart(-1f);
        searchActionPoint.SetAvailable(false);
      }
      else if (type == typeof (DropSearchActionPoint))
      {
        DropSearchActionPoint searchActionPoint = currentPoint as DropSearchActionPoint;
        if (searchActionPoint.HaveMapItems)
          Manager.Map.FadeStart(-1f);
        searchActionPoint.SetCoolTime();
      }
      if (searchInfo.IsSuccess)
      {
        foreach (Actor.ItemSearchInfo itemSearchInfo in searchInfo.ItemList)
        {
          StuffItem stuffItem = new StuffItem(itemSearchInfo.categoryID, itemSearchInfo.id, itemSearchInfo.count);
          if (player.PlayerData.ItemList.Count < player.PlayerData.InventorySlotMax)
            player.PlayerData.ItemList.AddItem(stuffItem, stuffItem.Count, player.PlayerData.InventorySlotMax);
          MapUIContainer.AddSystemItemLog(Singleton<Resources>.Instance.GameInfo.GetItem(itemSearchInfo.categoryID, itemSearchInfo.id), itemSearchInfo.count, true);
        }
        player.Controller.ChangeState("Normal");
      }
      else
      {
        MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
        player.Controller.ChangeState("Normal");
      }
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.ClearItems();
      player.ClearParticles();
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      player.SetStand(player.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
      player.Animation.RefsActAnimInfo = true;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (actor.Animation.PlayingInAnimation)
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
      Search.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new Search.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
