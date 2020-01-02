// Decompiled with JetBrains decompiler
// Type: AIProject.Player.RequestEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class RequestEvent : PlayerStateBase
  {
    private EventPoint _eventPoint;

    protected override void OnAwake(PlayerActor player)
    {
      this._eventPoint = player.CurrentEventPoint;
      if (Object.op_Inequality((Object) this._eventPoint, (Object) null))
      {
        CommonDefine.EventStoryInfoGroup playInfo = !Singleton<Resources>.IsInstance() ? (CommonDefine.EventStoryInfoGroup) null : Singleton<Resources>.Instance.CommonDefine?.EventStoryInfo;
        Dictionary<int, List<string>> textTable = !Singleton<Resources>.IsInstance() ? (Dictionary<int, List<string>>) null : Singleton<Resources>.Instance.Map?.EventPointCommandLabelTextTable;
        MapUIContainer.RequestUI.CancelEvent = (System.Action) (() => EventPoint.ChangePrevPlayerMode());
        MapUIContainer.RequestUI.ClosedEvent = (System.Action) (() => player.CurrentEventPoint = (EventPoint) null);
        MapUIContainer.RequestUI.SubmitEvent = (System.Action) (() =>
        {
          if (Object.op_Equality((Object) this._eventPoint, (Object) null))
          {
            EventPoint.ChangePrevPlayerMode();
          }
          else
          {
            this._eventPoint.RemoveConsiderationCommand();
            int eventId = this._eventPoint.EventID;
            int groupId = this._eventPoint.GroupID;
            int pointId = this._eventPoint.PointID;
            switch (eventId)
            {
              case 0:
                if (Object.op_Equality((Object) player, (Object) null) || playInfo == null)
                {
                  EventPoint.ChangePrevPlayerMode();
                  break;
                }
                player.PlayerController.ChangeState("Idle");
                EventPoint.OpenEventStart(player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.Generator.SEID, playInfo.Generator.SoundPlayDelayTime, playInfo.Generator.EndIntervalTime, (System.Action) (() =>
                {
                  if (Singleton<Manager.Map>.IsInstance())
                    Singleton<Manager.Map>.Instance.SetTimeRelationAreaOpenState(0, true);
                  Manager.Map.ForcedSetTutorialProgress(25);
                }), (System.Action) (() =>
                {
                  if (!textTable.IsNullOrEmpty<int, List<string>>())
                  {
                    List<string> source;
                    textTable.TryGetValue(6, out source);
                    MapUIContainer.PushMessageUI(source.GetElement<string>(EventPoint.LangIdx) ?? string.Empty, 0, 0, (System.Action) null);
                  }
                  EventPoint.ChangePrevPlayerMode();
                }));
                break;
              case 1:
                if (Object.op_Equality((Object) player, (Object) null) || playInfo == null)
                {
                  EventPoint.ChangePrevPlayerMode();
                  break;
                }
                player.PlayerController.ChangeState("Idle");
                EventPoint.OpenEventStart(player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.ShipRepair.SEID, playInfo.ShipRepair.SoundPlayDelayTime, playInfo.ShipRepair.EndIntervalTime, (System.Action) (() =>
                {
                  if (!Object.op_Inequality((Object) this._eventPoint, (Object) null))
                    return;
                  this._eventPoint.SetDedicatedNumber(1);
                  this.ChangeGameCleared();
                }), (System.Action) (() =>
                {
                  (!Singleton<Manager.Map>.IsInstance() ? (StoryPointEffect) null : Singleton<Manager.Map>.Instance.StoryPointEffect)?.FadeOutAndDestroy();
                  if (!textTable.IsNullOrEmpty<int, List<string>>())
                  {
                    List<string> source;
                    textTable.TryGetValue(7, out source);
                    MapUIContainer.PushMessageUI(source.GetElement<string>(EventPoint.LangIdx) ?? string.Empty, 0, 0, (System.Action) null);
                  }
                  Manager.Map.ForcedSetTutorialProgress(28);
                  DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(!Singleton<Resources>.IsInstance() ? 5.0 : (double) Singleton<Resources>.Instance.CommonDefine.EventStoryInfo.StoryCompleteNextSupportChangeTime)), (System.Action<M0>) (_ => Manager.Map.ForcedSetTutorialProgressAndUIUpdate(29))), (Component) Singleton<MapScene>.Instance);
                  EventPoint.ChangePrevPlayerMode();
                }));
                break;
              case 2:
                if (Object.op_Equality((Object) player, (Object) null) || playInfo == null || pointId != 4 && pointId != 5 && pointId != 6)
                {
                  EventPoint.ChangePrevPlayerMode();
                  break;
                }
                player.PlayerController.ChangeState("Idle");
                EventPoint.OpenEventStart(player, playInfo.StartEventFadeTime, playInfo.EndEventFadeTime, playInfo.PodDevice.SEID, playInfo.PodDevice.SoundPlayDelayTime, playInfo.PodDevice.EndIntervalTime, (System.Action) (() => this._eventPoint.SetAgentOpenState(true)), (System.Action) (() =>
                {
                  if (!textTable.IsNullOrEmpty<int, List<string>>())
                  {
                    List<string> source;
                    textTable.TryGetValue(9, out source);
                    MapUIContainer.PushMessageUI(source.GetElement<string>(EventPoint.LangIdx) ?? string.Empty, 0, 0, (System.Action) null);
                  }
                  EventPoint.ChangePrevPlayerMode();
                }));
                break;
              case 3:
                EventPoint.ChangePrevPlayerMode();
                break;
              case 4:
              case 5:
              case 6:
                EventPoint.ChangePrevPlayerMode();
                break;
            }
          }
        });
        MapUIContainer.OpenRequestUI((Popup.Request.Type) this._eventPoint.EventID);
        if (0 > this._eventPoint.WarningID || !MapUIContainer.RequestUI.IsImpossible)
          return;
        MapUIContainer.PushWarningMessage((Popup.Warning.Type) this._eventPoint.WarningID);
      }
      else
        player.PlayerController.ChangeState("Normal");
    }

    private void ChangeGameCleared()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Game instance = Singleton<Game>.Instance;
      GlobalSaveData globalData = instance.GlobalData;
      WorldData worldData = instance.WorldData;
      if (globalData != null)
        globalData.Cleared = true;
      if (worldData != null)
        worldData.Cleared = true;
      instance.SaveGlobalData();
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (Object.op_Equality((Object) this._eventPoint, (Object) null))
        return;
      player.CurrentEventPoint = (EventPoint) null;
    }

    ~RequestEvent()
    {
    }
  }
}
