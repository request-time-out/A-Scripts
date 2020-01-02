// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DateSearch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
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
  public class DateSearch : PlayerStateBase
  {
    private Subject<Unit> _onEndAction = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Search;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ValueTuple<int, string> valueTuple;
      AIProject.Definitions.Action.NameTable.TryGetValue(AIProject.EventType.Search, out valueTuple);
      int index = (int) valueTuple.Item1;
      DateActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerDateActionPointInfo(player.ChaControl.sex, AIProject.EventType.Search, out outInfo);
      int poseIda = outInfo.poseIDA;
      player.PoseID = poseIda;
      int _poseID = poseIda;
      Transform t = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA);
      player.Animation.RecoveryPoint = loop?.get_transform();
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][index][_poseID];
      player.Animation.LoadEventKeyTable(index, outInfo.poseIDA);
      player.LoadEventItems(playState);
      player.LoadEventParticles(index, outInfo.poseIDA);
      player.Animation.InitializeStates(playState);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
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
      player.Partner.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      player.Animation.AnimInfo = actorAnimInfo2;
      ActorAnimInfo animInfo = actorAnimInfo2;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      player.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, playState.MainStateInfo.FadeOutTime, animInfo.layer);
      player.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (_ => player.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      player.OldEnabledHoldingHand = ((Behaviour) player.HandsHolder).get_enabled();
      if (player.OldEnabledHoldingHand)
      {
        ((Behaviour) player.HandsHolder).set_enabled(false);
        player.HandsHolder.Weight = 0.0f;
        if (player.HandsHolder.EnabledHolding)
          player.HandsHolder.EnabledHolding = false;
      }
      player.CameraControl.Mode = CameraMode.ActionFreeLook;
      player.CameraControl.SetShotTypeForce(ShotType.Near);
      player.CameraControl.LoadActionCameraFile(index, _poseID, (Transform) null);
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

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DateSearch.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new DateSearch.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }

    private void Elapsed(PlayerActor player)
    {
      ActionPoint currentPoint = player.CurrentPoint;
      System.Type type = ((object) currentPoint).GetType();
      if (Singleton<Game>.IsInstance() && ((object) player.CurrentPoint).GetType() == typeof (SearchActionPoint))
      {
        Dictionary<int, AIProject.SaveData.Environment.SearchActionInfo> searchActionLockTable = Singleton<Game>.Instance.Environment.SearchActionLockTable;
        AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
        if (!searchActionLockTable.TryGetValue(player.CurrentPoint.RegisterID, out searchActionInfo))
          searchActionInfo = new AIProject.SaveData.Environment.SearchActionInfo();
        ++searchActionInfo.Count;
        searchActionLockTable[player.CurrentPoint.RegisterID] = searchActionInfo;
      }
      Dictionary<int, ItemTableElement> itemTableInArea = Singleton<Resources>.Instance.GameInfo.GetItemTableInArea(currentPoint.IDList.IsNullOrEmpty<int>() ? currentPoint.ID : currentPoint.IDList.GetElement<int>(0));
      if (itemTableInArea != null)
        ;
      Actor.SearchInfo searchInfo = player.RandomAddItem(itemTableInArea, true);
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
          player.PlayerData.ItemList.AddItem(stuffItem);
          MapUIContainer.AddSystemItemLog(Singleton<Resources>.Instance.GameInfo.GetItem(itemSearchInfo.categoryID, itemSearchInfo.id), itemSearchInfo.count, true);
        }
        player.Controller.ChangeState("Normal");
      }
      else
      {
        MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
        player.Controller.ChangeState("Normal");
      }
      AgentActor agentPartner = player.AgentPartner;
      if (Object.op_Equality((Object) agentPartner.CurrentPoint, (Object) null))
      {
        agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Ovation);
      }
      else
      {
        if (!player.OldEnabledHoldingHand)
          return;
        ((Behaviour) player.HandsHolder).set_enabled(true);
        player.OldEnabledHoldingHand = false;
      }
    }
  }
}
