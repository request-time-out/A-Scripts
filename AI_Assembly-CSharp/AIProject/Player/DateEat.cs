// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DateEat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class DateEat : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = AIProject.EventType.Eat;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(1.0)), (System.Action<M0>) (_ =>
      {
        MapUIContainer.RefreshCommands(0, player.DateEatCommandInfos);
        MapUIContainer.SetActiveCommandList(true, "食事");
        MapUIContainer.CommandList.CancelEvent = (System.Action) null;
      }));
      int type = (int) AIProject.Definitions.Action.NameTable[AIProject.EventType.Eat].Item1;
      DateActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerDateActionPointInfo(player.ChaControl.sex, AIProject.EventType.Eat, out outInfo);
      AgentActor partner = player.Partner as AgentActor;
      int poseIda = outInfo.poseIDA;
      player.PoseID = poseIda;
      int index = poseIda;
      int poseIdb = outInfo.poseIDB;
      partner.PoseID = poseIdb;
      int poseIDB = poseIdb;
      Transform t1 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      Transform t2 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameB)?.get_transform() ?? ((Component) player.CurrentPoint).get_transform();
      GameObject loop1 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA);
      player.Animation.RecoveryPoint = loop1?.get_transform();
      GameObject loop2 = ((Component) player.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameB);
      player.Partner.Animation.RecoveryPoint = loop2?.get_transform();
      PlayState playState1 = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) player.ChaControl.sex][type][index];
      player.Animation.LoadEventKeyTable(type, outInfo.poseIDA);
      player.LoadEventItems(playState1);
      player.LoadEventParticles(type, outInfo.poseIDA);
      player.Animation.InitializeStates(playState1);
      partner.Animation.LoadEventKeyTable(type, outInfo.poseIDB);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[type][outInfo.poseIDB];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(type, outInfo.poseIDB);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState1.Layer,
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop,
        endEnableBlend = playState1.EndEnableBlend,
        endBlendSec = playState1.EndBlendRate
      };
      player.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo3 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop,
        endEnableBlend = playState1.EndEnableBlend,
        endBlendSec = playState1.EndBlendRate,
        loopMinTime = playState2.MainStateInfo.LoopMin,
        loopMaxTime = playState2.MainStateInfo.LoopMax,
        hasAction = playState2.ActionInfo.hasAction
      };
      partner.Animation.AnimInfo = actorAnimInfo3;
      ActorAnimInfo actorAnimInfo4 = actorAnimInfo3;
      List<int> intList = ListPool<int>.Get();
      foreach (KeyValuePair<int, Dictionary<int, int>> foodDateEventItem in Singleton<Resources>.Instance.Map.FoodDateEventItemList)
      {
        foreach (KeyValuePair<int, int> keyValuePair in foodDateEventItem.Value)
        {
          if (keyValuePair.Value != -1)
            intList.Add(keyValuePair.Value);
        }
      }
      int num = -1;
      if (!intList.IsNullOrEmpty<int>())
        num = intList.GetElement<int>(Random.Range(0, intList.Count));
      ListPool<int>.Release(intList);
      ActionItemInfo eventItemInfo;
      if (Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(num, out eventItemInfo))
      {
        string rootParentName = Singleton<Resources>.Instance.LocomotionProfile.RootParentName;
        GameObject gameObject1 = player.LoadEventItem(num, rootParentName, false, eventItemInfo);
        if (Object.op_Inequality((Object) gameObject1, (Object) null))
        {
          foreach (Renderer componentsInChild in (Renderer[]) gameObject1.GetComponentsInChildren<Renderer>(true))
            componentsInChild.set_enabled(true);
        }
        GameObject gameObject2 = partner.LoadEventItem(num, rootParentName, false, eventItemInfo);
        if (Object.op_Inequality((Object) gameObject2, (Object) null))
        {
          foreach (Renderer componentsInChild in (Renderer[]) gameObject2.GetComponentsInChildren<Renderer>(true))
            componentsInChild.set_enabled(true);
        }
      }
      player.DeactivateNavMeshAgent();
      player.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this._hasAction = playState1.ActionInfo.hasAction;
      if (this._hasAction)
      {
        this._loopStateName = playState1.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState1.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
        this._randomCount = playState1.ActionInfo.randomCount;
        this._oldNormalizedTime = 0.0f;
      }
      player.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState1.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      player.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, playState1.DirectionType);
      partner.Animation.PlayInAnimation(actorAnimInfo4.inEnableBlend, actorAnimInfo4.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo4.layer);
      partner.SetStand(t2, playState2.MainStateInfo.InStateInfo.EnableFade, playState2.MainStateInfo.InStateInfo.FadeSecond, actorAnimInfo4.layer);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => this.ChangeCamera(type, poseIDB, (Actor) partner)));
      player.OldEnabledHoldingHand = ((Behaviour) player.HandsHolder).get_enabled();
      if (!player.OldEnabledHoldingHand)
        return;
      ((Behaviour) player.HandsHolder).set_enabled(false);
      if (!player.HandsHolder.EnabledHolding)
        return;
      player.HandsHolder.EnabledHolding = false;
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
      player.Animation.RefsActAnimInfo = true;
      player.ClearItems();
      player.ClearParticles();
      player.Partner?.ClearItems();
      player.Partner?.ClearParticles();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DateEat.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new DateEat.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }

    private void ChangeCamera(int actionID, int poseID, Actor actor)
    {
      Dictionary<int, ActAnimFlagData> dictionary;
      ActAnimFlagData actAnimFlagData;
      if (!Singleton<Resources>.Instance.Action.AgentActionFlagTable.TryGetValue(actionID, out dictionary) || !dictionary.TryGetValue(poseID, out actAnimFlagData))
        return;
      switch (actAnimFlagData.attitudeID)
      {
        case 0:
          ADV.ChangeADVCamera(actor);
          break;
        case 1:
          ADV.ChangeADVCameraDiagonal(actor);
          break;
        default:
          ADV.ChangeADVFixedAngleCamera(actor, actAnimFlagData.attitudeID);
          break;
      }
    }
  }
}
