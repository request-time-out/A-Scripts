// Decompiled with JetBrains decompiler
// Type: AIProject.Player.CharaChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject.Definitions;
using AIProject.SaveData;
using Cinemachine;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class CharaChange : PlayerStateBase
  {
    protected int _currentState = -1;
    private Subject<Unit> _onEndFadeIn = new Subject<Unit>();
    private Subject<Unit> _onEndFadeOut = new Subject<Unit>();
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private bool _completeWait;
    private CinemachineBlendDefinition.Style _prevStyle;

    private OpenData openData { get; } = new OpenData();

    private PackData packData { get; } = new PackData();

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = (AIProject.EventType) 0;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeIn, 1), (System.Action<M0>) (_ =>
      {
        this.Refresh(player);
        ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(100.0)), (System.Action<M0>) (__ =>
        {
          this._completeWait = true;
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, false), (System.Action<M0>) (___ => {}), (System.Action) (() => this._onEndFadeOut.OnNext(Unit.get_Default())));
        }));
      }));
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeOut, 1), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (System.Action<M0>) (__ => this.Elapsed(player)))));
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      this._prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() => this._onEndFadeIn.OnNext(Unit.get_Default())));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.CameraControl.EventCameraLocator.set_runtimeAnimatorController((RuntimeAnimatorController) null);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) this._prevStyle;
      player.CameraControl.Mode = CameraMode.Normal;
      player.ChaControl.visibleAll = true;
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.Action);
      Singleton<Manager.Input>.Instance.SetupState();
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.FadeCanvas.IsFadeIn || !this._completeWait || (MapUIContainer.FadeCanvas.IsFadeOut || this._onEndAction == null))
        return;
      this._onEndAction.OnNext(Unit.get_Default());
    }

    private void Elapsed(PlayerActor player)
    {
      player.CurrentDevicePoint = (DevicePoint) null;
      MapUIContainer.SetVisibleHUD(true);
      MapUIContainer.StorySupportUI.Open();
      player.PlayerController.CommandArea.UpdateCollision(player);
      player.Controller.ChangeState("Normal");
    }

    private void Refresh(PlayerActor player)
    {
      ChaFileControl chaFileControl = new ChaFileControl();
      int id = player.CurrentDevicePoint.ID;
      int num = 0;
      foreach (KeyValuePair<int, string> changedCharaFile in Singleton<Manager.Map>.Instance.ChangedCharaFiles)
      {
        string str = changedCharaFile.Value;
        AgentData agentData = Singleton<Game>.Instance.WorldData.AgentTable[changedCharaFile.Key];
        if (agentData.ParameterLock)
          agentData.ParameterLock = false;
        bool flag = !str.IsNullOrEmpty() && Manager.Config.GraphicData.CharasEntry[changedCharaFile.Key] && chaFileControl.LoadCharaFile(str, (byte) 1, false, true) && agentData.MapID == Singleton<Manager.Map>.Instance.MapID;
        AgentActor agent;
        if (Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(changedCharaFile.Key, ref agent))
        {
          agent.DisableBehavior();
          agent.ClearItems();
          agent.ClearParticles();
          Actor.BehaviorSchedule schedule = agent.Schedule;
          schedule.enabled = false;
          agent.Schedule = schedule;
          agent.TargetInSightActor = (Actor) null;
          if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
          {
            agent.CurrentPoint.SetActiveMapItemObjs(true);
            agent.CurrentPoint.ReleaseSlot((Actor) agent);
            agent.CurrentPoint = (ActionPoint) null;
          }
          agent.TargetInSightActionPoint = (ActionPoint) null;
          if (Object.op_Inequality((Object) agent.Partner, (Object) null))
          {
            if (agent.Partner is PlayerActor)
              (agent.Partner as PlayerActor).PlayerController.ChangeState("Normal");
            else if (agent.Partner is AgentActor)
            {
              AgentActor partner = agent.Partner as AgentActor;
              agent.StopLesbianSequence();
              partner.StopLesbianSequence();
              partner.Animation.EndIgnoreEvent();
              Singleton<Game>.Instance.GetExpression(partner.ChaControl.fileParam.personality, "標準")?.Change(partner.ChaControl);
              partner.Animation.ResetDefaultAnimatorController();
              partner.ChangeBehavior(Desire.ActionType.Normal);
            }
            else if (agent.Partner is MerchantActor)
            {
              MerchantActor partner = agent.Partner as MerchantActor;
              agent.StopLesbianSequence();
              partner.ResetState();
              partner.ChangeBehavior(partner.LastNormalMode);
            }
            agent.Partner = (Actor) null;
          }
          if (Object.op_Inequality((Object) agent.CommandPartner, (Object) null))
          {
            if (agent.CommandPartner is AgentActor)
              (agent.CommandPartner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
            else if (agent.CommandPartner is MerchantActor)
            {
              MerchantActor commandPartner = agent.CommandPartner as MerchantActor;
              commandPartner.ChangeBehavior(commandPartner.LastNormalMode);
            }
            agent.CommandPartner = (Actor) null;
          }
          Singleton<Manager.Map>.Instance.RemoveAgent(agent);
          if (flag)
          {
            agent = Singleton<Manager.Map>.Instance.AddAgent(changedCharaFile.Key, agentData);
            agentData.PrevMapID = new int?();
          }
          else
            agent = (AgentActor) null;
        }
        else if (flag)
        {
          agent = Singleton<Manager.Map>.Instance.AddAgent(changedCharaFile.Key, agentData);
          agentData.PrevMapID = new int?();
        }
        else
          agent = (AgentActor) null;
        if (Object.op_Inequality((Object) agent, (Object) null))
        {
          agent.RefreshWalkStatus(Singleton<Manager.Map>.Instance.PointAgent.Waypoints);
          Singleton<Manager.Map>.Instance.InitSearchActorTargets(agent);
          player.PlayerController.CommandArea.AddCommandableObject((ICommandable) agent);
          using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, AgentActor> current = enumerator.Current;
              if (!Object.op_Equality((Object) current.Value, (Object) agent))
                current.Value.AddActor((Actor) agent);
            }
          }
          agent.ActivateNavMeshAgent();
          Transform recoverPoint = player.CurrentDevicePoint.RecoverPoints[num++];
          agent.NavMeshAgent.Warp(recoverPoint.get_position());
          agent.Rotation = recoverPoint.get_rotation();
          agent.EnableBehavior();
          agent.ChangeBehavior(Desire.ActionType.Normal);
        }
      }
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }
  }
}
