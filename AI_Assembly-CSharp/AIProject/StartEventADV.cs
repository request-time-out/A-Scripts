// Decompiled with JetBrains decompiler
// Type: AIProject.StartEventADV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using AIProject.UI;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class StartEventADV : AgentAction
  {
    [SerializeField]
    private int _eventID;
    private Actor _prevPartner;

    private OpenData openData { get; } = new OpenData();

    private PackData packData { get; } = new PackData();

    private int charaID
    {
      get
      {
        return this.Agent.AgentData.param.charaID;
      }
    }

    public virtual void OnStart()
    {
      Debug.Log((object) string.Format("開始するEvent = {0}", (object) this._eventID));
      ((Task) this).OnStart();
      Singleton<MapUIContainer>.Instance.MinimapUI.ChangeCamera(false, false);
      MapUIContainer.SetVisibleHUD(false);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      AgentActor agent = this.Agent;
      this._prevPartner = agent.CommandPartner;
      agent.CommandPartner = agent.TargetInSightActor;
      agent.TargetInSightActor = (Actor) null;
      agent.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
      agent.ChaControl.ChangeLookEyesPtn(1);
      agent.ChaControl.ChangeLookNeckPtn(3, 1f);
      agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      Singleton<Manager.Map>.Instance.Player.CameraControl.OnCameraBlended = (System.Action) (() => ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.SkipWhile<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => agent.Animation.PlayingTurnAnimation)), (Func<M0, bool>) (_ => !Singleton<Manager.ADV>.IsInstance() || Singleton<Manager.ADV>.Instance.Captions.IsProcEndADV)), 1), (System.Action<M0>) (_ => this.OpenADV(player, agent))));
      player.CommCompanion = (Actor) agent;
      player.Controller.ChangeState("Communication");
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[agent.ChaControl.fileParam.personality];
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
      AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
      agent.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
      agent.Animation.PlayTurnAnimation(player.Position, 1f, playState.MainStateInfo.InStateInfo, false);
      agent.DisableBehavior();
      Manager.ADV.ChangeADVCamera((Actor) agent);
      MapUIContainer.SetVisibleHUD(false);
      Singleton<Manager.ADV>.Instance.TargetCharacter = agent;
    }

    private void OpenADV(PlayerActor player, AgentActor agent)
    {
      this.packData.Init();
      this.openData.FindLoad(string.Format("{0}", (object) this._eventID), this.charaID, 2);
      this.packData.SetCommandData((ADV.ICommandData) Singleton<Game>.Instance?.WorldData?.Environment);
      this.packData.SetParam((IParams) agent.AgentData, (IParams) Singleton<Game>.Instance?.WorldData?.PlayerData);
      if (this._eventID == 1)
      {
        bool flag = player.IsBirthday(agent);
        this.packData.isBirthday = flag;
        if (flag)
          agent.AgentData.IsPlayerForBirthdayEvent = true;
      }
      this.packData.onComplete = (System.Action) (() =>
      {
        if (agent.IsEvent)
          agent.IsEvent = false;
        agent.EnableBehavior();
        switch (this._eventID)
        {
          case 0:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            this.Complete();
            agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
            agent.SetDesire(Desire.GetDesireKey(Desire.Type.Game), 0.0f);
            agent.ChangeBehavior(Desire.ActionType.Normal);
            goto case 4;
          case 1:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            this.Complete();
            agent.SetDesire(Desire.GetDesireKey(Desire.Type.Gift), 0.0f);
            agent.ChangeBehavior(Desire.ActionType.Normal);
            goto case 4;
          case 2:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            this.Complete();
            agent.SetDesire(Desire.GetDesireKey(Desire.Type.Want), 0.0f);
            agent.ChangeBehavior(Desire.ActionType.Normal);
            goto case 4;
          case 3:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            if (this.packData.isSuccessH)
            {
              if (player.ChaControl.sex == (byte) 1 && !player.ChaControl.fileParam.futanari)
              {
                if (agent.CanSelectHCommand(Singleton<Resources>.Instance.DefinePack.MapDefines.LesTypeHMeshTag))
                {
                  Singleton<HSceneManager>.Instance.nInvitePtn = 0;
                  agent.InitiateHScene(HSceneManager.HEvent.Normal);
                  goto case 4;
                }
                else
                {
                  this.ChangeTakeMode(agent, player, false);
                  goto case 4;
                }
              }
              else if (agent.CanSelectHCommand())
              {
                Singleton<HSceneManager>.Instance.nInvitePtn = 0;
                agent.InitiateHScene(HSceneManager.HEvent.Normal);
                goto case 4;
              }
              else
              {
                this.ChangeTakeMode(agent, player, false);
                goto case 4;
              }
            }
            else if (agent.CanMasturbation)
            {
              MapUIContainer.SetVisibleHUD(true);
              switch (player.Mode)
              {
                case Desire.ActionType.Normal:
                case Desire.ActionType.Date:
                  player.CameraControl.Mode = CameraMode.Normal;
                  player.Controller.ChangeState("Normal");
                  player.CameraControl.OnCameraBlended = (System.Action) (() => Singleton<Manager.Map>.Instance.Player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView);
                  break;
              }
              agent.Animation.ResetDefaultAnimatorController();
              agent.CommandPartner = this._prevPartner;
              agent.AddMotivation(Desire.GetDesireKey(Desire.Type.H), 50f);
              agent.ChangeBehavior(Desire.ActionType.SearchMasturbation);
              goto case 4;
            }
            else
            {
              this.Complete();
              agent.SetDesire(Desire.GetDesireKey(Desire.Type.H), 0.0f);
              agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
              agent.ChangeBehavior(Desire.ActionType.Normal);
              MapUIContainer.SetVisibleHUD(true);
              goto case 4;
            }
          case 4:
            this.packData.Release();
            Singleton<Manager.ADV>.Instance.Captions.EndADV((System.Action) null);
            break;
          case 5:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            if (agent.CanSelectHCommand(Singleton<Resources>.Instance.DefinePack.MapDefines.FloorTypeHMeshTag))
            {
              agent.InitiateHScene(HSceneManager.HEvent.FromFemale);
              goto case 4;
            }
            else
            {
              this.ChangeTakeMode(agent, player, true);
              goto case 4;
            }
          case 6:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            if (this.packData.isSuccessH)
            {
              this.ChangeAbductMode(agent, player, Desire.ActionType.TakeSleepPoint);
              goto case 4;
            }
            else
            {
              this.Complete();
              agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
              agent.ChangeBehavior(Desire.ActionType.Normal);
              MapUIContainer.SetVisibleHUD(true);
              goto case 4;
            }
          case 7:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            agent.FromFemale = true;
            this.ChangeAbductMode(agent, player, Desire.ActionType.TakeSleepHPoint);
            goto case 4;
          case 8:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            if (this.packData.isSuccessH)
            {
              this.ChangeAbductMode(agent, player, Desire.ActionType.TakeEatPoint);
              goto case 4;
            }
            else
            {
              this.Complete();
              agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
              agent.ChangeBehavior(Desire.ActionType.Normal);
              MapUIContainer.SetVisibleHUD(true);
              goto case 4;
            }
          case 9:
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            if (this.packData.isSuccessH)
            {
              this.ChangeAbductMode(agent, player, Desire.ActionType.TakeBreakPoint);
              goto case 4;
            }
            else
            {
              this.Complete();
              agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
              agent.ChangeBehavior(Desire.ActionType.Normal);
              MapUIContainer.SetVisibleHUD(true);
              goto case 4;
            }
          default:
            agent.EnableBehavior();
            agent.ChaControl.ChangeLookEyesPtn(0);
            agent.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
            agent.ChaControl.ChangeLookNeckPtn(3, 1f);
            agent.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            goto case 4;
        }
      });
      this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }

    private void Complete()
    {
      MapUIContainer.SetVisibleHUD(true);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      switch (player.Mode)
      {
        case Desire.ActionType.Normal:
        case Desire.ActionType.Date:
          player.CameraControl.Mode = CameraMode.Normal;
          player.Controller.ChangeState("Normal");
          player.CameraControl.OnCameraBlended = (System.Action) (() => Singleton<Manager.Map>.Instance.Player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView);
          break;
      }
      this.Agent.Animation.ResetDefaultAnimatorController();
      if (Object.op_Equality((Object) this._prevPartner, (Object) player))
        this.Agent.CommandPartner = (Actor) null;
      else
        this.Agent.CommandPartner = this._prevPartner;
      MapUIContainer.SetVisibleHUD(true);
    }

    private void ChangeTakeMode(AgentActor agent, PlayerActor player, bool fromFemale)
    {
      agent.FromFemale = fromFemale;
      this.ChangeAbductMode(agent, player, Desire.ActionType.TakeHPoint);
    }

    private void ChangeAbductMode(AgentActor agent, PlayerActor player, Desire.ActionType mode)
    {
      player.CameraControl.Mode = CameraMode.Normal;
      player.CameraControl.OnCameraBlended = (System.Action) (() => Singleton<Manager.Map>.Instance.Player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView);
      agent.Animation.ResetDefaultAnimatorController();
      agent.Partner = (Actor) player;
      if (Object.op_Inequality((Object) player.Partner, (Object) null))
      {
        AgentActor partner = player.Partner as AgentActor;
        if (Object.op_Inequality((Object) partner, (Object) null))
          partner.DeactivatePairing(0);
      }
      player.Partner = (Actor) agent;
      player.PlayerController.ChangeState("Follow");
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => MapUIContainer.ChoiceUI.IsActiveControl)), 1), (System.Action<M0>) (_ => MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None)));
      agent.DestPosition = new Vector3?();
      agent.ChangeBehavior(mode);
    }
  }
}
