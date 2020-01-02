// Decompiled with JetBrains decompiler
// Type: AIProject.PlayerActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject.Animal;
using AIProject.Definitions;
using AIProject.Player;
using AIProject.RootMotion;
using AIProject.SaveData;
using AIProject.Scene;
using AIProject.UI;
using Cinemachine;
using IllusionUtility.GetUtility;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class PlayerActor : Actor
  {
    private static readonly DateTime _midnightTime = new DateTime(1, 1, 1, 23, 59, 59);
    private TimeSpan _elapsedTimeInSleep = new TimeSpan();
    private Vector3 _defaultOfs = Vector3.get_zero();
    [SerializeField]
    private Vector3 _normalOffset = Vector3.get_zero();
    [SerializeField]
    private Vector3 _behaviourOffset = Vector3.get_zero();
    private bool? _scheduledInteractionState = new bool?(false);
    private float _coolTime;
    private bool _called;
    private float _callElapsedTime;
    private const float _callCoolTime = 2f;
    private IEnumerator _enumerator;
    private IDisposable _disposable;
    private IEnumerator _sleepEventEnumerator;
    private IDisposable _sleepEventDisposable;
    private IEnumerator _eatEventEnumerator;
    private IDisposable _eatEventDisposable;
    private IEnumerator _exitEatEventEnumerator;
    private IDisposable _exitEatEventDisposable;
    private IEnumerator _hizamakuraEventEnumerator;
    private IDisposable _hizamakuraEventDisposable;
    [SerializeField]
    protected ActorAnimationPlayer _animation;
    [SerializeField]
    private ActorCameraControl _cameraCtrl;
    [SerializeField]
    private ActorCameraControl.LocomotionSettingData _locomotionSetting;
    [SerializeField]
    private AQUAS_LensEffects _lensEffect;
    [SerializeField]
    private GameObject _cameraTarget;
    [SerializeField]
    private ActorLocomotionThirdPerson _character;
    [SerializeField]
    private PlayerController _controller;

    public bool IsBirthday(AgentActor agent)
    {
      if (Object.op_Inequality((Object) agent, (Object) null) && (agent.AgentData.IsPlayerForBirthdayEvent || agent.ChaControl.fileGameInfo.phase < 3))
        return false;
      DateTime now = DateTime.Now;
      int birthMonth = (int) this.ChaControl.fileParam.birthMonth;
      int birthDay = (int) this.ChaControl.fileParam.birthDay;
      return now.Month == birthMonth && now.Day == birthDay;
    }

    public CommCommandList.CommandInfo[] SleepCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] CookCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] ForgeCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] MixingCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] BaseCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] DeviceCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] ShipCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] ChickenCoopCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] CoSleepCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] DateEatCommandInfos { get; private set; }

    public CommCommandList.CommandInfo[] SpecialHCommandInfo { get; private set; }

    public CommCommandList.CommandInfo[] ExitEatEventCommandInfo { get; private set; }

    public CommCommandList.CommandInfo[] WarpCommandInfos { get; private set; }

    public override bool IsNeutralCommand
    {
      get
      {
        if (Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null) || Singleton<Manager.Map>.Instance.IsWarpProc)
          return false;
        bool flag1 = this.Mode != Desire.ActionType.Onbu;
        bool flag2 = this._controller.State is Normal;
        bool flag3 = this._controller.State is Houchi;
        bool flag4 = this._controller.State is WMap;
        if (!flag1)
          return false;
        return flag2 || flag3 || flag4;
      }
    }

    public void ResetCoolTime()
    {
      this._coolTime = 0.0f;
    }

    public int HPoseID { get; set; }

    private void InitializeLabels()
    {
      this.SleepCommandInfos = new CommCommandList.CommandInfo[3]
      {
        new CommCommandList.CommandInfo("そのまま寝転ぶ")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("Lie");
          })
        },
        new CommCommandList.CommandInfo("寝る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            ConfirmScene.Sentence = "一日を終了しますか？";
            ConfirmScene.OnClickedYes = (System.Action) (() =>
            {
              Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
              this.ElapseTime((Func<bool>) (() => this.InvokeRelatedSleepEvent() != 1), (Func<bool>) (() =>
              {
                using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    KeyValuePair<int, AgentActor> current = enumerator.Current;
                    if (current.Value.CheckEventADV(2))
                    {
                      current.Value.AdvEventStart_SleepingPlayer(this);
                      return false;
                    }
                  }
                }
                return true;
              }));
              MapUIContainer.SetActiveCommandList(false);
              Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
              Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            });
            ConfirmScene.OnClickedNo = (System.Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
            Singleton<Game>.Instance.LoadDialog();
          })
        },
        new CommCommandList.CommandInfo("起きる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            ActorAnimInfo animInfo = this.Animation.AnimInfo;
            this.ActivateNavMeshAgent();
            this.IsKinematic = false;
            this.SetStand(this.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
            this.Animation.RecoveryPoint = (Transform) null;
            this.Animation.RefsActAnimInfo = true;
            this.Controller.ChangeState("Normal");
            this.ReleaseCurrentPoint();
            if (Object.op_Inequality((Object) this.PlayerController.CommandArea, (Object) null))
              ((Behaviour) this.PlayerController.CommandArea).set_enabled(true);
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
            MapUIContainer.SetActiveCommandList(false);
          })
        }
      };
      this.CookCommandInfos = new CommCommandList.CommandInfo[3]
      {
        new CommCommandList.CommandInfo("料理をする")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.Controller.ChangeState("Cook");
            this.SetScheduledInteractionState(false);
          })
        },
        new CommCommandList.CommandInfo("貯蔵庫を見る")
        {
          Condition = (Func<bool>) (() => !this.ContainsExcludePantryID()),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.Controller.ChangeState("Pantry");
            this.SetScheduledInteractionState(false);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            MapUIContainer.SetActiveCommandList(false);
            MapUIContainer.SetVisibleHUDExceptStoryUI(true);
            MapUIContainer.StorySupportUI.Open();
            this.Controller.ChangeState("Normal");
            this.ReleaseCurrentPoint();
            if (Object.op_Inequality((Object) this.PlayerController.CommandArea, (Object) null))
              ((Behaviour) this.PlayerController.CommandArea).set_enabled(true);
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
            this.ActivateNavMeshAgent();
            this.IsKinematic = false;
          })
        }
      };
      this.BaseCommandInfos = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("ハウジング")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.Controller.ChangeState("Housing");
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            MapUIContainer.SetActiveCommandList(false);
            MapUIContainer.SetVisibleHUDExceptStoryUI(true);
            MapUIContainer.StorySupportUI.Open();
            this.Controller.ChangeState("Normal");
          })
        }
      };
      AgentData agentData1;
      AgentData agentData2;
      AgentData agentData3;
      AgentData agentData4;
      AgentData agentData5;
      this.DeviceCommandInfos = new CommCommandList.CommandInfo[7]
      {
        new CommCommandList.CommandInfo("女の子を登場")
        {
          Condition = (Func<bool>) (() => Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(Singleton<Manager.Map>.Instance.AccessDeviceID, out agentData1) && !agentData1.PlayEnterScene),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("EntryChara");
          })
        },
        new CommCommandList.CommandInfo("女の子を変更")
        {
          Condition = (Func<bool>) (() => !Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(Singleton<Manager.Map>.Instance.AccessDeviceID, out agentData2) || agentData2.PlayEnterScene),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("EditChara");
          })
        },
        new CommCommandList.CommandInfo("女の子の容姿変更")
        {
          Condition = (Func<bool>) (() => !Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(Singleton<Manager.Map>.Instance.AccessDeviceID, out agentData3) || agentData3.PlayEnterScene),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("CharaLookEdit");
          })
        },
        new CommCommandList.CommandInfo("女の子の住む島を変更")
        {
          Condition = (Func<bool>) (() => Singleton<Game>.Instance.WorldData.Cleared),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("CharaMigration");
          })
        },
        new CommCommandList.CommandInfo("主人公を変更")
        {
          Condition = (Func<bool>) (() => !Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(Singleton<Manager.Map>.Instance.AccessDeviceID, out agentData4) || agentData4.PlayEnterScene),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("EditPlayer");
          })
        },
        new CommCommandList.CommandInfo("主人公の容姿変更")
        {
          Condition = (Func<bool>) (() => !Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(Singleton<Manager.Map>.Instance.AccessDeviceID, out agentData5) || agentData5.PlayEnterScene),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("PlayerLookEdit");
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            Singleton<Manager.Map>.Instance.AccessDeviceID = -1;
            MapUIContainer.SetActiveCommandList(false);
            MapUIContainer.SetVisibleHUDExceptStoryUI(true);
            MapUIContainer.StorySupportUI.Open();
            this.Controller.ChangeState("Normal");
            this.CurrentDevicePoint = (DevicePoint) null;
          })
        }
      };
      List<CommCommandList.CommandInfo> toRelease = ListPool<CommCommandList.CommandInfo>.Get();
      using (Dictionary<int, AssetBundleInfo>.Enumerator enumerator = Singleton<Resources>.Instance.Map.MapList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AssetBundleInfo> current = enumerator.Current;
          int id = current.Key;
          string mapName = (string) current.Value.name;
          CommCommandList.CommandInfo commandInfo = new CommCommandList.CommandInfo(mapName)
          {
            Condition = (Func<bool>) (() => Singleton<Manager.Map>.IsInstance() && id != Singleton<Manager.Map>.Instance.MapID),
            Event = (System.Action<int>) (x =>
            {
              Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
              ConfirmScene.Sentence = string.Format("{0}に移動しますか？", (object) mapName);
              ConfirmScene.OnClickedYes = (System.Action) (() =>
              {
                Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
                MapUIContainer.SetActiveCommandList(false);
                this.SetScheduledInteractionState(false);
                this.ReleaseInteraction();
                MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
                Singleton<MapScene>.Instance.SaveProfile(true);
                Singleton<Manager.Map>.Instance.ChangeMap(id, (System.Action) null, (System.Action) (() =>
                {
                  this.PlayerController.ChangeState("Normal");
                  this.CameraControl.EnabledInput = true;
                  MapUIContainer.SetVisibleHUD(true);
                  MapUIContainer.StorySupportUI.Open();
                  if (Object.op_Inequality((Object) this.PlayerController.CommandArea, (Object) null))
                    ((Behaviour) this.PlayerController.CommandArea).set_enabled(true);
                  MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
                }));
              });
              ConfirmScene.OnClickedNo = (System.Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
              Singleton<Game>.Instance.LoadDialog();
            })
          };
          toRelease.Add(commandInfo);
        }
      }
      toRelease.Add(new CommCommandList.CommandInfo("立ち去る")
      {
        Condition = (Func<bool>) null,
        Event = (System.Action<int>) (x =>
        {
          MapUIContainer.SetActiveCommandList(false);
          MapUIContainer.StorySupportUI.Open();
          this.PlayerController.ChangeState("Normal");
        })
      });
      this.ShipCommandInfos = toRelease.ToArray();
      ListPool<CommCommandList.CommandInfo>.Release(toRelease);
      this.ChickenCoopCommandInfos = new CommCommandList.CommandInfo[3]
      {
        new CommCommandList.CommandInfo("タマゴ箱を確認")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            MapUIContainer.CommandList.Visibled = false;
            MapUIContainer.SetActiveChickenCoopUI(true, ChickenCoopUI.Mode.EggBox);
          })
        },
        new CommCommandList.CommandInfo("ニワトリを追加")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            MapUIContainer.CommandList.Visibled = false;
            MapUIContainer.SetActiveChickenCoopUI(true, ChickenCoopUI.Mode.Coop);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            MapUIContainer.SetActiveCommandList(false);
            this.PlayerController.ChangeState("Normal");
          })
        }
      };
      this.WarpCommandInfos = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("移動する")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            ConfirmScene.Sentence = "移動しますか";
            ConfirmScene.OnClickedYes = (System.Action) (() =>
            {
              MapUIContainer.SetActiveCommandList(false);
              this.SetScheduledInteractionState(false);
              this.ReleaseInteraction();
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
              string prevStateName = this.PlayerController.PrevStateName;
              Singleton<Manager.Map>.Instance.WarpToPairPoint((this.CurrentPoint as WarpPoint).PairPoint(), (System.Action) (() =>
              {
                MapUIContainer.SetVisibleHUDExceptStoryUI(true);
                MapUIContainer.StorySupportUI.Open();
                if (prevStateName == "Onbu")
                  this.Controller.ChangeState("Onbu");
                else
                  this.Controller.ChangeState("Normal");
                this.Controller.ChangeState("Idle");
                GC.Collect();
              }), (System.Action) (() =>
              {
                if (prevStateName == "Onbu")
                  this.Controller.ChangeState("Onbu");
                else
                  this.Controller.ChangeState("Normal");
                Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.Action);
                Singleton<Manager.Input>.Instance.SetupState();
                this.SetScheduledInteractionState(true);
                this.ReleaseInteraction();
                Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Warp_Out);
              }));
              Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Warp_In);
            });
            ConfirmScene.OnClickedNo = (System.Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
            Singleton<Game>.Instance.LoadDialog();
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.CancelWarp())
        }
      };
      this.CoSleepCommandInfos = new CommCommandList.CommandInfo[4]
      {
        new CommCommandList.CommandInfo("エッチがしたい")
        {
          Condition = (Func<bool>) (() =>
          {
            AgentActor agentPartner = this.AgentPartner;
            return agentPartner.CanSelectHCommand() && !agentPartner.IsBadMood();
          }),
          Event = (System.Action<int>) (x =>
          {
            AgentActor agentPartner = this.AgentPartner;
            int personality = agentPartner.ChaControl.fileParam.personality;
            agentPartner.Animation.StopAllAnimCoroutine();
            this.AgentPartner.openData.FindLoad("1", personality, 9);
            this.AgentPartner.packData.AttitudeID = 1;
            this.AgentPartner.packData.onComplete = (System.Action) (() =>
            {
              if (this.AgentPartner.packData.isSuccessH)
              {
                this.AgentPartner.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                HSceneManager.SleepStart = true;
                this.InitiateHScene();
              }
              else
                this.AgentPartner.packData.restoreCommands = this.CoSleepCommandInfos;
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.AgentPartner.openData, (IPack) this.AgentPartner.packData);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (this.ChaControl.sex == (byte) 1 && !this.ChaControl.fileParam.futanari)
              return false;
            AgentActor agentPartner = this.AgentPartner;
            return agentPartner.CanSelectHCommand() && agentPartner.IsBadMood();
          }),
          Event = (System.Action<int>) (x =>
          {
            AgentActor partner = this.AgentPartner;
            int personality = partner.ChaControl.fileParam.personality;
            partner.Animation.StopAllAnimCoroutine();
            this.AgentPartner.openData.FindLoad("1", personality, 9);
            this.AgentPartner.packData.AttitudeID = 1;
            this.AgentPartner.packData.onComplete = (System.Action) (() =>
            {
              if (partner.packData.isSuccessH)
              {
                partner.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                HSceneManager.SleepStart = true;
                Singleton<HSceneManager>.Instance.isForce = true;
                this.InitiateHScene();
              }
              else
                this.AgentPartner.packData.restoreCommands = this.CoSleepCommandInfos;
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.AgentPartner.openData, (IPack) this.AgentPartner.packData);
          })
        },
        new CommCommandList.CommandInfo("寝る")
        {
          Condition = (Func<bool>) (() => Singleton<Manager.Map>.Instance.CanSleepInTime()),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            ConfirmScene.Sentence = "一日を終了しますか？";
            ConfirmScene.OnClickedYes = (System.Action) (() =>
            {
              Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
              this.ElapseTime((System.Action) (() => this.OnElapsedTimeFromDateSleep()), true);
              MapUIContainer.SetActiveCommandList(false);
              this.SetScheduledInteractionState(false);
              this.ReleaseInteraction();
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            });
            ConfirmScene.OnClickedNo = (System.Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
            Singleton<Game>.Instance.LoadDialog();
          })
        },
        new CommCommandList.CommandInfo("起きる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            this.CameraControl.Mode = CameraMode.Normal;
            ActorAnimInfo animInfo1 = this.Animation.AnimInfo;
            this.ActivateNavMeshAgent();
            this.IsKinematic = false;
            this.SetStand(this.Animation.RecoveryPoint, animInfo1.endEnableBlend, animInfo1.endBlendSec, animInfo1.directionType);
            this.Animation.RecoveryPoint = (Transform) null;
            this.Animation.RefsActAnimInfo = true;
            this.Controller.ChangeState("Normal");
            this.ReleaseCurrentPoint();
            if (Object.op_Inequality((Object) this.PlayerController.CommandArea, (Object) null))
              ((Behaviour) this.PlayerController.CommandArea).set_enabled(true);
            AgentActor agentPartner = this.AgentPartner;
            if (Object.op_Inequality((Object) agentPartner, (Object) null))
            {
              agentPartner.ActivateNavMeshAgent();
              agentPartner.SetActiveOnEquipedItem(true);
              ActorAnimInfo animInfo2 = agentPartner.Animation.AnimInfo;
              agentPartner.SetStand(agentPartner.Animation.RecoveryPoint, animInfo2.outEnableBlend, animInfo2.outBlendSec, animInfo2.directionType);
              if (this.OldEnabledHoldingHand)
              {
                ((Behaviour) this.HandsHolder).set_enabled(true);
                this.OldEnabledHoldingHand = false;
              }
              agentPartner.ResetActionFlag();
              agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Date);
            }
            MapUIContainer.SetActiveCommandList(false);
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
            MapUIContainer.SetVisibleHUDExceptStoryUI(true);
            MapUIContainer.StorySupportUI.Open();
          })
        }
      };
      this.DateEatCommandInfos = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("一緒にご飯を食べる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            this.PlayerData.DateEatTrigger = true;
            MapUIContainer.CommandList.Visibled = false;
            AgentActor partner = this.AgentPartner;
            int personality = partner.ChaControl.fileParam.personality;
            partner.Animation.StopAllAnimCoroutine();
            partner.packData.Init();
            string asset = partner.ChaControl.fileGameInfo.phase < 2 ? "0" : "1";
            partner.openData.FindLoad(asset, personality, 8);
            partner.packData.onComplete = (System.Action) (() =>
            {
              partner.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              partner.packData.Release();
              this.PlayerData.DateEatTrigger = true;
              this.CameraControl.Mode = CameraMode.Normal;
              this.Controller.ChangeState("Normal");
              partner.ActivateNavMeshAgent();
              partner.SetActiveOnEquipedItem(true);
              ActorAnimInfo animInfo = partner.Animation.AnimInfo;
              partner.SetStand(partner.Animation.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
              if (this.OldEnabledHoldingHand)
              {
                ((Behaviour) this.HandsHolder).set_enabled(true);
                this.OldEnabledHoldingHand = false;
              }
              partner.BehaviorResources.ChangeMode(Desire.ActionType.Date);
              MapUIContainer.SetActiveCommandList(false);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(partner.openData, (IPack) partner.packData);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            this.CameraControl.Mode = CameraMode.Normal;
            this.CancelCommand();
          })
        }
      };
      this.SpecialHCommandInfo = new CommCommandList.CommandInfo[8]
      {
        new CommCommandList.CommandInfo("分娩台")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 2),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("木馬")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 3),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("拘束台鞍馬")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 4),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("ギロチン")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 5),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("拘束デンマ台")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 6),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("拘束機械姦")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 7),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("吊るし挿入")
        {
          Condition = (Func<bool>) (() => this.HPoseID == 8),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
            MapUIContainer.SetActiveCommandList(false);
            this.StartSpecialH();
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            this.Controller.ChangeState("Normal");
            MapUIContainer.SetActiveCommandList(false);
          })
        }
      };
      this.ExitEatEventCommandInfo = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("一緒にご飯を食べる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            AgentActor agentPartner = this.AgentPartner;
            int personality = agentPartner.ChaControl.fileParam.personality;
            agentPartner.Animation.StopAllAnimCoroutine();
            OpenData openData = agentPartner.openData;
            AgentActor.PackData packData = agentPartner.packData;
            packData.Init();
            string asset = agentPartner.ChaControl.fileGameInfo.phase < 2 ? "0" : "1";
            openData.FindLoad(asset, personality, 8);
            packData.onComplete = (System.Action) (() =>
            {
              packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              packData.Release();
              packData.Release();
              Singleton<Manager.ADV>.Instance.Captions.CanvasGroupOFF();
              ((Component) Singleton<MapUIContainer>.Instance.advScene).get_gameObject().SetActive(false);
              this.ExitEatEventADV();
            });
            Singleton<MapUIContainer>.Instance.OpenADV(agentPartner.openData, (IPack) agentPartner.packData);
            MapUIContainer.SetActiveCommandList(false);
            this.SetScheduledInteractionState(false);
            this.ReleaseInteraction();
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
            this.ExitEatEventADV();
            MapUIContainer.SetActiveCommandList(false);
            this.SetScheduledInteractionState(false);
            this.ReleaseInteraction();
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
          })
        }
      };
    }

    public void CancelCommand()
    {
      this.Controller.ChangeState("Normal");
      AgentActor agentPartner = this.AgentPartner;
      if (Object.op_Inequality((Object) agentPartner, (Object) null))
      {
        agentPartner.ActivateNavMeshAgent();
        agentPartner.SetActiveOnEquipedItem(true);
        ActorAnimInfo animInfo = agentPartner.Animation.AnimInfo;
        agentPartner.SetStand(agentPartner.Animation.RecoveryPoint, animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.directionType);
        if (this.OldEnabledHoldingHand)
        {
          ((Behaviour) this.HandsHolder).set_enabled(true);
          this.OldEnabledHoldingHand = false;
        }
        agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Date);
      }
      MapUIContainer.SetActiveCommandList(false);
    }

    public void CancelWarp()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      MapUIContainer.SetActiveCommandList(false);
      MapUIContainer.SetVisibleHUDExceptStoryUI(true);
      MapUIContainer.StorySupportUI.Open();
      if (this.PlayerController.PrevStateName == "Onbu")
        this.PlayerController.ChangeState(this.PlayerController.PrevStateName);
      else
        this.PlayerController.ChangeState("Normal");
      this.ReleaseCurrentPoint();
      if (Object.op_Inequality((Object) this.PlayerController.CommandArea, (Object) null))
        ((Behaviour) this.PlayerController.CommandArea).set_enabled(true);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      this.ActivateNavMeshAgent();
      this.IsKinematic = false;
    }

    public void CallProc()
    {
      if (this._called)
        return;
      List<Desire.ActionType> encounterWhitelist = Singleton<Resources>.Instance.AgentProfile.EncounterWhitelist;
      float durationCtForCall = Singleton<Resources>.Instance.AgentProfile.DurationCTForCall;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      int? chunkId1 = this.MapArea?.ChunkID;
      int num1 = !chunkId1.HasValue ? this.PlayerData.ChunkID : chunkId1.Value;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (current.Value.Mode != Desire.ActionType.Called && current.Value.Mode != Desire.ActionType.Date && (current.Value.Mode != Desire.ActionType.Onbu && current.Value.Mode != Desire.ActionType.Cold2A) && (current.Value.Mode != Desire.ActionType.Cold2B && current.Value.Mode != Desire.ActionType.Cold3A && (current.Value.Mode != Desire.ActionType.Cold3B && current.Value.Mode != Desire.ActionType.OverworkA)) && (current.Value.Mode != Desire.ActionType.OverworkB && current.Value.Mode != Desire.ActionType.Cold2BMedicated && (current.Value.Mode != Desire.ActionType.Cold3BMedicated && current.Value.Mode != Desire.ActionType.WeaknessA) && (current.Value.Mode != Desire.ActionType.WeaknessB && current.Value.Mode != Desire.ActionType.FoundPeeping && (current.Value.EventKey != EventType.Sleep && current.Value.EventKey != EventType.Toilet))) && (current.Value.EventKey != EventType.Bath && current.Value.EventKey != EventType.Move && (current.Value.EventKey != EventType.Masturbation && current.Value.EventKey != EventType.Lesbian) && !Object.op_Inequality((Object) current.Value.Partner, (Object) null)))
          {
            int? chunkId2 = current.Value.MapArea?.ChunkID;
            int num2 = !chunkId2.HasValue ? current.Value.AgentData.ChunkID : chunkId2.Value;
            ChaFileGameInfo fileGameInfo = current.Value.ChaControl.fileGameInfo;
            AgentData agentData = current.Value.AgentData;
            float num3 = statusProfile.CallProbBaseRate + statusProfile.CallProbPhaseRate * (float) (fileGameInfo.phase + 1);
            float num4 = 0.0f;
            int num5 = fileGameInfo.flavorState[1];
            int index = 0;
            foreach (int num6 in statusProfile.CallReliabilityBorder)
            {
              if (num5 >= num6)
                ++index;
              else
                break;
            }
            float num7 = num4 + statusProfile.CallReliabilityBuff[index];
            float num8 = agentData.StatsTable[1];
            if ((double) num8 < (double) fileGameInfo.moodBound.lower)
              num7 += statusProfile.CallLowerMoodProb;
            else if ((double) num8 > (double) fileGameInfo.moodBound.upper)
              num7 += statusProfile.CallUpperMoodProb;
            if (current.Value.ChaControl.fileGameInfo.normalSkill.ContainsValue(34))
              num7 += statusProfile.CallProbSuperSense;
            else if ((double) agentData.CallCTCount < (double) durationCtForCall)
            {
              ++agentData.CalledCount;
              if (agentData.CalledCount == 2)
                num7 += statusProfile.CallSecondTimeProb;
              else if (agentData.CalledCount >= 3)
                num7 += statusProfile.CallOverTimeProb;
            }
            else
            {
              agentData.CallCTCount = 0.0f;
              ++agentData.CalledCount;
            }
            float num9 = num3 + num7;
            if (!current.Value.ChaControl.fileGameInfo.normalSkill.ContainsValue(34) && num2 != num1)
              num9 *= 0.5f;
            if ((double) Random.get_value() < (double) num9)
            {
              AgentActor agentActor1 = current.Value;
              int num6 = -1;
              current.Value.PoseID = num6;
              int num10 = num6;
              agentActor1.ActionID = num10;
              agentData.CarryingItem = (StuffItem) null;
              current.Value.StateType = AIProject.Definitions.State.Type.Normal;
              if (Object.op_Inequality((Object) current.Value.CurrentPoint, (Object) null))
              {
                current.Value.CurrentPoint.SetActiveMapItemObjs(true);
                current.Value.CurrentPoint.ReleaseSlot((Actor) current.Value);
                current.Value.CurrentPoint = (ActionPoint) null;
              }
              if (Object.op_Inequality((Object) current.Value.CommandPartner, (Object) null))
              {
                Actor commandPartner = current.Value.CommandPartner;
                switch (commandPartner)
                {
                  case AgentActor _:
                    AgentActor agentActor2 = commandPartner as AgentActor;
                    agentActor2.CommandPartner = (Actor) null;
                    agentActor2.ChangeBehavior(Desire.ActionType.Normal);
                    break;
                  case MerchantActor _:
                    MerchantActor merchantActor = commandPartner as MerchantActor;
                    merchantActor.CommandPartner = (Actor) null;
                    merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
                    break;
                }
                current.Value.CommandPartner = (Actor) null;
              }
              current.Value.EventKey = (EventType) 0;
              current.Value.TargetInSightActionPoint = (ActionPoint) null;
              current.Value.TargetInSightActor = (Actor) this;
              current.Value.CommandPartner = (Actor) null;
              current.Value.ResetActionFlag();
              if (current.Value.Schedule.enabled)
              {
                Actor.BehaviorSchedule schedule = current.Value.Schedule;
                schedule.enabled = false;
                current.Value.Schedule = schedule;
              }
              current.Value.ActivateNavMeshAgent();
              current.Value.ActivateTransfer(true);
              current.Value.ClearItems();
              current.Value.ClearParticles();
              current.Value.Animation.ResetDefaultAnimatorController();
              current.Value.ChangeBehavior(Desire.ActionType.Called);
            }
          }
        }
      }
      this._called = true;
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Call);
      MapUIContainer.AddSystemLog("全員を呼び出しました", true);
    }

    public bool ContainsExcludePantryID()
    {
      if (Object.op_Equality((Object) this.CurrentPoint, (Object) null))
        return false;
      List<int> pantryCommandActPtiDs = Singleton<Resources>.Instance.PlayerProfile.ExPantryCommandActPTIDs;
      if (!this.CurrentPoint.IDList.IsNullOrEmpty<int>())
      {
        foreach (int id in this.CurrentPoint.IDList)
        {
          if (pantryCommandActPtiDs.Contains(id))
            return true;
        }
        return false;
      }
      return pantryCommandActPtiDs.Contains(this.CurrentPoint.ID);
    }

    public void InitiateHScene()
    {
      Singleton<HSceneManager>.Instance.HsceneEnter((Actor) this.AgentPartner, -1, (AgentActor) null, HSceneManager.HEvent.Normal);
    }

    public void ElapseTime(System.Action action, bool fadeOut = true)
    {
      if (this._disposable != null)
        this._disposable.Dispose();
      this._enumerator = this.ElapseTimeCoroutine(action, fadeOut);
      this._disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._enumerator), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    public bool ProcessingTimeSkip
    {
      get
      {
        return this._enumerator != null;
      }
    }

    [DebuggerHidden]
    private IEnumerator ElapseTimeCoroutine(System.Action action, bool fadeOut = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CElapseTimeCoroutine\u003Ec__Iterator0()
      {
        action = action,
        fadeOut = fadeOut,
        \u0024this = this
      };
    }

    public void ElapseTime(Func<bool> conditionBefore, Func<bool> conditionAfter = null)
    {
      if (this._disposable != null)
        this._disposable.Dispose();
      this._enumerator = this.ElapseTimeCoroutine(conditionBefore, conditionAfter);
      this._disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._enumerator), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    [DebuggerHidden]
    private IEnumerator ElapseTimeCoroutine(
      Func<bool> conditionBefore,
      Func<bool> conditionAfter)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CElapseTimeCoroutine\u003Ec__Iterator1()
      {
        conditionBefore = conditionBefore,
        conditionAfter = conditionAfter,
        \u0024this = this
      };
    }

    private int InvokeRelatedSleepEvent()
    {
      if (Game.isAdd01 && (this.ChaControl.sex == (byte) 0 || this.ChaControl.sex == (byte) 1 && this.ChaControl.fileParam.futanari))
      {
        List<AgentActor> agentActorList = ListPool<AgentActor>.Get();
        using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, AgentActor> current = enumerator.Current;
            if (current.Value.CanRevRape() && current.Value.ChunkID == this.ChunkID)
              agentActorList.Add(current.Value);
          }
        }
        if (!agentActorList.IsNullOrEmpty<AgentActor>())
        {
          AgentActor agent = (AgentActor) null;
          List<AgentActor> all = agentActorList.FindAll((Predicate<AgentActor>) (x => x.Mode == Desire.ActionType.SearchRevRape));
          if (!all.IsNullOrEmpty<AgentActor>())
            agent = all.GetElement<AgentActor>(Random.Range(0, all.Count));
          if (Object.op_Equality((Object) agent, (Object) null))
            agent = agentActorList.GetElement<AgentActor>(Random.Range(0, agentActorList.Count));
          ListPool<AgentActor>.Release(agentActorList);
          if (agent.Mode == Desire.ActionType.Normal || agent.Mode == Desire.ActionType.SearchSleep || agent.Mode == Desire.ActionType.Encounter)
          {
            if ((double) Random.Range(0.0f, 100f) < (double) Singleton<Resources>.Instance.StatusProfile.YobaiMinMax.Lerp(Mathf.InverseLerp(0.0f, 100f, agent.AgentData.StatsTable[6])))
            {
              this.StartSneakH(agent);
              return 1;
            }
            PoseKeyPair sleepTogetherRight = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherRight;
            PoseKeyPair sleepTogetherLeft = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherLeft;
            List<ActionPoint> groupActionPoints = this.CurrentPoint.GroupActionPoints;
            ActionPoint actionPoint1 = (ActionPoint) null;
            foreach (ActionPoint actionPoint2 in groupActionPoints)
            {
              if (actionPoint2.IsNeutralCommand)
              {
                actionPoint1 = actionPoint2;
                break;
              }
            }
            ActionPointInfo outInfo;
            if (Object.op_Inequality((Object) actionPoint1, (Object) null) && (actionPoint1.FindAgentActionPointInfo(EventType.Sleep, sleepTogetherRight.poseID, out outInfo) || actionPoint1.FindAgentActionPointInfo(EventType.Sleep, sleepTogetherLeft.poseID, out outInfo)))
            {
              Transform t = ((Component) actionPoint1).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) actionPoint1).get_transform();
              GameObject loop = ((Component) actionPoint1).get_transform().FindLoop(outInfo.recoveryNullName);
              agent.Animation.RecoveryPoint = loop?.get_transform();
              PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[outInfo.eventID][outInfo.poseID];
              agent.Animation.LoadEventKeyTable(outInfo.eventID, outInfo.poseID);
              agent.LoadEventItems(playState);
              agent.LoadEventParticles(outInfo.eventID, outInfo.poseID);
              agent.Animation.InitializeStates(playState);
              agent.Animation.LoadAnimatorIfNotEquals(playState);
              ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
              {
                layer = playState.Layer,
                inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
                inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
                outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
                outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
                directionType = playState.DirectionType,
                endEnableBlend = playState.EndEnableBlend,
                endBlendSec = playState.EndBlendRate,
                isLoop = playState.MainStateInfo.IsLoop,
                loopMinTime = playState.MainStateInfo.LoopMin,
                loopMaxTime = playState.MainStateInfo.LoopMax,
                hasAction = playState.ActionInfo.hasAction,
                loopStateName = playState.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName,
                randomCount = playState.ActionInfo.randomCount,
                oldNormalizedTime = 0.0f
              };
              agent.Animation.AnimInfo = actorAnimInfo1;
              ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
              agent.SetActiveOnEquipedItem(false);
              agent.ChaControl.setAllLayerWeight(0.0f);
              agent.DisableActionFlag();
              agent.DeactivateNavMeshAgent();
              agent.IsKinematic = true;
              agent.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
              agent.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
              agent.CurrentPoint = actionPoint1;
              agent.CurrentPoint.SetSlot((Actor) agent);
              agent.SetCurrentSchedule(actorAnimInfo2.isLoop, "添い寝", actorAnimInfo2.loopMinTime, actorAnimInfo2.loopMaxTime, actorAnimInfo2.hasAction, true);
              agent.ChangeBehavior(Desire.ActionType.EndTaskSleepAfterDate);
            }
          }
          else if (agent.Mode == Desire.ActionType.SearchRevRape)
          {
            this.StartSneakH(agent);
            return 1;
          }
        }
        else
          ListPool<AgentActor>.Release(agentActorList);
      }
      return 0;
    }

    public void StartSneakH(AgentActor agent)
    {
      agent.packData.Init();
      agent.Animation.StopAllAnimCoroutine();
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ChangeBehavior(Desire.ActionType.Idle);
      agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      agent.Mode = Desire.ActionType.Idle;
      agent.DisableBehavior();
      agent.DeactivateNavMeshAgent();
      MapUIContainer.SetVisibleHUD(false);
      agent.Position = this.Position;
      agent.Rotation = this.Rotation;
      this.PlayerController.ChangeState("Communication");
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
      PoseKeyPair yobai = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.Yobai;
      AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[yobai.postureID][yobai.poseID].MainStateInfo.AssetBundleInfo;
      agent.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
      Transform transform = ((Component) this.CameraControl.CameraComponent).get_transform();
      agent.SetLookPtn(1, 3);
      agent.SetLookTarget(1, 0, transform);
      agent.openData.FindLoad("4", agent.AgentData.param.charaID, 2);
      agent.packData.onComplete = (System.Action) (() =>
      {
        agent.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        agent.packData.Release();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle;
        agent.InitiateHScene(HSceneManager.HEvent.GyakuYobai);
      });
      ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => Singleton<MapUIContainer>.Instance.OpenADV(agent.openData, (IPack) agent.packData)));
    }

    private void StartSpecialH()
    {
      this.AgentPartner.packData.Init();
      ActionPoint currentPoint = this.CurrentPoint;
      this.AgentPartner.CommandPartner = (Actor) this;
      bool enabled = ((Behaviour) this.HandsHolder).get_enabled();
      this.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) this.HandsHolder).set_enabled(false);
        if (this.HandsHolder.EnabledHolding)
          this.HandsHolder.EnabledHolding = false;
      }
      this.AgentPartner.StartCommunication();
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this.Animation.PlayingTurnAnimation)), 1), (System.Action<M0>) (_ =>
      {
        this.AgentPartner.openData.FindLoad("3", this.AgentPartner.charaID, 9);
        this.AgentPartner.packData.onComplete = (System.Action) (() =>
        {
          this.AgentPartner.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
          HPoint hpoint = currentPoint.HPoint;
          this.AgentPartner.CommandPartner = (Actor) null;
          this.ReleaseCurrentPoint();
          this.AgentPartner.packData.Release();
          Singleton<HSceneManager>.Instance.HousingHEnter((Actor) this.AgentPartner, hpoint);
        });
        Singleton<MapUIContainer>.Instance.OpenADV(this.AgentPartner.openData, (IPack) this.AgentPartner.packData);
      }));
    }

    public void StartSleepTogetherEvent(AgentActor agent)
    {
      agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.PlayerController.ChangeState("Idle");
      this.SetActiveOnEquipedItem(false);
      this.ChaControl.setAllLayerWeight(0.0f);
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      int eventID = (int) AIProject.Definitions.Action.NameTable[EventType.Sleep].Item1;
      DateActionPointInfo apInfo;
      this.CurrentPoint.TryGetPlayerDateActionPointInfo(this.ChaControl.sex, EventType.Sleep, out apInfo);
      int poseIda = apInfo.poseIDA;
      this.PoseID = poseIda;
      int index = poseIda;
      Transform t1 = ((Component) this.CurrentPoint).get_transform().FindLoop(apInfo.baseNullNameA)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      Transform t2 = ((Component) this.CurrentPoint).get_transform().FindLoop(apInfo.baseNullNameB)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      this.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(apInfo.recoveryNullNameA)?.get_transform();
      this.Partner.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(apInfo.recoveryNullNameB)?.get_transform();
      PlayState playState1 = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) this.ChaControl.sex][eventID][index];
      this.Animation.LoadEventKeyTable(eventID, apInfo.poseIDA);
      this.LoadEventItems(playState1);
      this.LoadEventParticles(eventID, apInfo.poseIDA);
      this.Animation.InitializeStates(playState1);
      Actor partner = this.Partner;
      partner.Animation.LoadEventKeyTable(eventID, apInfo.poseIDB);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eventID][apInfo.poseIDB];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(eventID, apInfo.poseIDB);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState1.Layer,
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop
      };
      this.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo3 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState2.DirectionType,
        isLoop = playState2.MainStateInfo.IsLoop,
        loopMinTime = playState1.MainStateInfo.LoopMin,
        loopMaxTime = playState1.MainStateInfo.LoopMax,
        hasAction = playState1.ActionInfo.hasAction
      };
      partner.Animation.AnimInfo = actorAnimInfo3;
      ActorAnimInfo actorAnimInfo4 = actorAnimInfo3;
      this.DeactivateNavMeshAgent();
      this.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState1.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      this.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, playState1.DirectionType);
      partner.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      partner.SetStand(t2, actorAnimInfo4.inEnableBlend, actorAnimInfo4.inBlendSec, actorAnimInfo2.layer);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ =>
      {
        if (apInfo.pointID == 501)
        {
          Manager.ADV.ChangeADVFixedAngleCamera((Actor) this, 5);
        }
        else
        {
          if (apInfo.pointID != 500)
            return;
          Manager.ADV.ChangeADVFixedAngleCamera(partner, 5);
        }
      }));
      bool enabled = ((Behaviour) this.HandsHolder).get_enabled();
      this.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) this.HandsHolder).set_enabled(false);
        if (this.HandsHolder.EnabledHolding)
          this.HandsHolder.EnabledHolding = false;
      }
      this.CameraControl.SetShotTypeForce(ShotType.Near);
      this._sleepEventEnumerator = this.SleepEventCoroutine();
      this._sleepEventDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._sleepEventEnumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator SleepEventCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CSleepEventCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    public void StartGyakuYobaiEvent(AgentActor agent)
    {
      agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      agent.Mode = Desire.ActionType.Idle;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.DisableEntity((Actor) agent);
        this.EventKey = EventType.Sleep;
        this.SetActiveOnEquipedItem(false);
        this.ChaControl.setAllLayerWeight(0.0f);
        this.SetScheduledInteractionState(false);
        this.ReleaseInteraction();
        Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
        Singleton<Manager.Input>.Instance.SetupState();
        this.PlayActionMotion(EventType.Sleep);
        OpenData openData = agent.openData;
        AgentActor.PackData packData = agent.packData;
        packData.Init();
        agent.Animation.StopAllAnimCoroutine();
        agent.SetActiveOnEquipedItem(false);
        agent.ChaControl.setAllLayerWeight(0.0f);
        agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        agent.Mode = Desire.ActionType.Idle;
        agent.DisableBehavior();
        agent.DeactivateNavMeshAgent();
        MapUIContainer.SetVisibleHUD(false);
        agent.Position = this.Position;
        agent.Rotation = this.Rotation;
        this.PlayerController.ChangeState("Communication");
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        PoseKeyPair yobai = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.Yobai;
        AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[yobai.postureID][yobai.poseID].MainStateInfo.AssetBundleInfo;
        agent.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        Transform transform = ((Component) this.CameraControl.CameraComponent).get_transform();
        agent.SetLookPtn(1, 3);
        agent.SetLookTarget(1, 0, transform);
        openData.FindLoad("4", agent.AgentData.param.charaID, 13);
        packData.onComplete = (System.Action) (() =>
        {
          packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
          packData.Release();
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(CinemachineBlendDefinition&) ref this.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle;
          agent.SetDesire(Desire.GetDesireKey(Desire.Type.Lonely), 0.0f);
          agent.SetDesire(Desire.GetDesireKey(Desire.Type.Sleep), 0.0f);
          Singleton<Manager.Map>.Instance.EnableEntity((Actor) agent);
          HSceneManager.SleepStart = true;
          agent.InitiateHScene(HSceneManager.HEvent.GyakuYobai);
        });
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => Singleton<MapUIContainer>.Instance.OpenADV(openData, (IPack) packData)));
      }));
    }

    public void StartEatEvent(AgentActor agent)
    {
      Singleton<Manager.Map>.Instance.DisableEntity((Actor) agent);
      agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.PlayerController.ChangeState("Idle");
      this.SetActiveOnEquipedItem(false);
      this.ChaControl.setAllLayerWeight(0.0f);
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      int index1 = (int) AIProject.Definitions.Action.NameTable[EventType.Eat].Item1;
      DateActionPointInfo outInfo;
      this.CurrentPoint.TryGetPlayerDateActionPointInfo(this.ChaControl.sex, EventType.Eat, out outInfo);
      int poseIda = outInfo.poseIDA;
      this.PoseID = poseIda;
      int index2 = poseIda;
      Transform t1 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      Transform t2 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameB)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      this.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA)?.get_transform();
      this.Partner.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameB)?.get_transform();
      PlayState playState1 = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) this.ChaControl.sex][index1][index2];
      this.Animation.LoadEventKeyTable(index1, outInfo.poseIDA);
      this.LoadEventItems(playState1);
      this.LoadEventParticles(index1, outInfo.poseIDA);
      this.Animation.InitializeStates(playState1);
      Actor partner = this.Partner;
      partner.Animation.LoadEventKeyTable(index1, outInfo.poseIDB);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[index1][outInfo.poseIDB];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(index1, outInfo.poseIDB);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState1.Layer,
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop
      };
      this.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo3 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState2.DirectionType,
        isLoop = playState2.MainStateInfo.IsLoop,
        endEnableBlend = false,
        endBlendSec = 0.0f,
        loopMinTime = playState1.MainStateInfo.LoopMin,
        loopMaxTime = playState1.MainStateInfo.LoopMax,
        hasAction = playState1.ActionInfo.hasAction
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
        GameObject gameObject1 = this.LoadEventItem(num, rootParentName, false, eventItemInfo);
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
      this.DeactivateNavMeshAgent();
      this.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState1.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      this.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, playState1.DirectionType);
      partner.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      partner.SetStand(t2, actorAnimInfo4.inEnableBlend, actorAnimInfo4.inBlendSec, actorAnimInfo2.layer);
      bool enabled = ((Behaviour) this.HandsHolder).get_enabled();
      this.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) this.HandsHolder).set_enabled(false);
        if (this.HandsHolder.EnabledHolding)
          this.HandsHolder.EnabledHolding = false;
      }
      this.CameraControl.SetShotTypeForce(ShotType.Near);
      Dictionary<int, ActAnimFlagData> dictionary;
      ActAnimFlagData talkData;
      if (Singleton<Resources>.Instance.Action.AgentActionFlagTable.TryGetValue(index1, out dictionary) && dictionary.TryGetValue(outInfo.poseIDB, out talkData))
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => Manager.ADV.ChangeADVFixedAngleCamera(partner, talkData.attitudeID)));
      else
        Manager.ADV.ChangeADVCameraDiagonal(partner);
      this._eatEventEnumerator = this.EatEventCoroutine();
      this._eatEventDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._eatEventEnumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator EatEventCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CEatEventCoroutine\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void InitiateExitEatEventCommand()
    {
      this.PlayerController.ChangeState("ExitEatEvent");
    }

    private void ExitEatEventADV()
    {
      this._exitEatEventEnumerator = this.ExitEatEventCoroutine();
      this._exitEatEventDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._exitEatEventEnumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator ExitEatEventCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CExitEatEventCoroutine\u003Ec__Iterator4()
      {
        \u0024this = this
      };
    }

    public void StartHizamakuraEvent(AgentActor agent)
    {
      Singleton<Manager.Map>.Instance.DisableEntity((Actor) agent);
      agent.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.PlayerController.ChangeState("Idle");
      this.SetActiveOnEquipedItem(false);
      this.ChaControl.setAllLayerWeight(0.0f);
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      int eventID = (int) AIProject.Definitions.Action.NameTable[EventType.Break].Item1;
      DateActionPointInfo outInfo;
      this.CurrentPoint.FindPlayerDateActionPointInfo(this.ChaControl.sex, Singleton<Resources>.Instance.PlayerProfile.HizamakuraPTID, out outInfo);
      int poseIda = outInfo.poseIDA;
      this.PoseID = poseIda;
      int index = poseIda;
      Transform t1 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameA)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      Transform t2 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullNameB)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      this.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameA)?.get_transform();
      this.Partner.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullNameB)?.get_transform();
      PlayState playState1 = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) this.ChaControl.sex][eventID][index];
      this.Animation.LoadEventKeyTable(eventID, outInfo.poseIDA);
      this.LoadEventItems(playState1);
      this.LoadEventParticles(eventID, outInfo.poseIDA);
      this.Animation.InitializeStates(playState1);
      Actor partner = this.Partner;
      partner.Animation.LoadEventKeyTable(eventID, outInfo.poseIDB);
      PlayState playState2 = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eventID][outInfo.poseIDB];
      partner.LoadEventItems(playState2);
      partner.LoadEventParticles(eventID, outInfo.poseIDB);
      partner.Animation.InitializeStates(playState2);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState1.Layer,
        inEnableBlend = playState1.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState1.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState1.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState1.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState1.DirectionType,
        isLoop = playState1.MainStateInfo.IsLoop
      };
      this.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo3 = new ActorAnimInfo()
      {
        layer = playState2.Layer,
        inEnableBlend = playState2.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState2.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = playState2.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState2.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState2.DirectionType,
        isLoop = playState2.MainStateInfo.IsLoop,
        endEnableBlend = false,
        endBlendSec = 0.0f,
        loopMinTime = playState1.MainStateInfo.LoopMin,
        loopMaxTime = playState1.MainStateInfo.LoopMax,
        hasAction = playState1.ActionInfo.hasAction
      };
      partner.Animation.AnimInfo = actorAnimInfo3;
      ActorAnimInfo actorAnimInfo4 = actorAnimInfo3;
      this.DeactivateNavMeshAgent();
      this.IsKinematic = true;
      partner.SetActiveOnEquipedItem(false);
      partner.ChaControl.setAllLayerWeight(0.0f);
      partner.DeactivateNavMeshAgent();
      partner.IsKinematic = true;
      this.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState1.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      this.SetStand(t1, playState1.MainStateInfo.InStateInfo.EnableFade, playState1.MainStateInfo.InStateInfo.FadeSecond, playState1.DirectionType);
      partner.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState2.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      partner.SetStand(t2, actorAnimInfo4.inEnableBlend, actorAnimInfo4.inBlendSec, actorAnimInfo2.layer);
      bool enabled = ((Behaviour) this.HandsHolder).get_enabled();
      this.OldEnabledHoldingHand = enabled;
      if (enabled)
      {
        ((Behaviour) this.HandsHolder).set_enabled(false);
        if (this.HandsHolder.EnabledHolding)
          this.HandsHolder.EnabledHolding = false;
      }
      this.CameraControl.SetShotTypeForce(ShotType.Near);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => Manager.ADV.ChangeADVFixedAngleCamera(partner, 6)));
      this._hizamakuraEventEnumerator = this.HizamakuraEventCoroutine();
      this._hizamakuraEventDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this._hizamakuraEventEnumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator HizamakuraEventCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CHizamakuraEventCoroutine\u003Ec__Iterator5()
      {
        \u0024this = this
      };
    }

    public override string CharaName
    {
      get
      {
        return this.ChaControl.fileParam.fullname;
      }
    }

    public override ICharacterInfo TiedInfo
    {
      get
      {
        return (ICharacterInfo) this.PlayerData;
      }
    }

    public PlayerData PlayerData { get; set; }

    public override ActorAnimation Animation
    {
      get
      {
        return (ActorAnimation) this._animation;
      }
    }

    public ActorCameraControl CameraControl
    {
      get
      {
        return this._cameraCtrl;
      }
    }

    public CameraConfig CameraConfig
    {
      get
      {
        return this._cameraCtrl.CameraConfig;
      }
    }

    public Vector3 NormalOffset
    {
      get
      {
        return this._normalOffset;
      }
    }

    public Vector3 BehaviourOffset
    {
      get
      {
        return this._behaviourOffset;
      }
    }

    public GameObject CameraTarget
    {
      get
      {
        return this._cameraTarget;
      }
    }

    public override ActorLocomotion Locomotor
    {
      get
      {
        return (ActorLocomotion) this._character;
      }
    }

    public ActorLocomotionThirdPerson CharacterTPS
    {
      get
      {
        return this._character;
      }
    }

    public override ActorController Controller
    {
      get
      {
        return (ActorController) this._controller;
      }
    }

    public PlayerController PlayerController
    {
      get
      {
        return this._controller;
      }
    }

    public HandsHolder HandsHolder { get; protected set; }

    public bool OldEnabledHoldingHand { get; set; }

    public DevicePoint CurrentDevicePoint { get; set; }

    public EventPoint CurrentEventPoint { get; set; }

    public FarmPoint CurrentFarmPoint { get; set; }

    public PetHomePoint CurrentPetHomePoint { get; set; }

    public JukePoint CurrentjukePoint { get; set; }

    public CraftPoint CurrentCraftPoint { get; set; }

    public List<ValueTuple<Popup.Tutorial.Type, bool>> TutorialIndexList { get; set; } = new List<ValueTuple<Popup.Tutorial.Type, bool>>();

    public void SetScheduledInteractionState(bool isEnabled)
    {
      this._scheduledInteractionState = new bool?(isEnabled);
    }

    public void ReleaseInteraction()
    {
      if (!this._scheduledInteractionState.HasValue)
      {
        Debug.Log((object) "ScheduledInteraction was Empty");
      }
      else
      {
        this.CurrentInteractionState = this._scheduledInteractionState.Value;
        this._cameraCtrl.EnabledInput = this._scheduledInteractionState.Value;
        this._scheduledInteractionState = new bool?();
      }
    }

    public bool CurrentInteractionState { get; private set; }

    public Actor CommCompanion { get; set; }

    public AgentActor AgentPartner
    {
      get
      {
        return this.Partner as AgentActor;
      }
      set
      {
        this.Partner = (Actor) value;
      }
    }

    public AnimalBase Animal { get; set; }

    public bool IsRunning { get; set; }

    protected override void OnStart()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnLateUpdate()));
    }

    private void OnElapsedTimeFromDateSleep()
    {
      AgentActor agentPartner = this.AgentPartner;
      ActorAnimInfo animInfo = agentPartner.Animation.AnimInfo;
      if (this.OldEnabledHoldingHand)
      {
        ((Behaviour) this.HandsHolder).set_enabled(false);
        this.HandsHolder.EnabledHolding = false;
      }
      agentPartner.CurrentPoint = this.CurrentPoint;
      agentPartner.CurrentPoint.SetSlot((Actor) agentPartner);
      agentPartner.SetCurrentSchedule(animInfo.isLoop, "睡眠", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, true);
      agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.EndTaskSleepAfterDate);
      agentPartner.Mode = Desire.ActionType.EndTaskSleepAfterDate;
      agentPartner.DisableActionFlag();
      agentPartner.Partner = (Actor) null;
      this.Partner = (Actor) null;
    }

    public void StashData()
    {
      Singleton<MapScene>.Instance.SaveProfile(true);
      Game.PrevPlayerStateFromCharaCreate = this.PlayerController.State.GetType().Name;
      Game.PrevAccessDeviceID = this.CurrentDevicePoint.ID;
      Singleton<Game>.Instance.WorldData.Copy(Singleton<Game>.Instance.Data.AutoData);
      Singleton<Game>.Instance.IsAuto = true;
    }

    public void PlayActionMotion(EventType eventType)
    {
      ActionPointInfo outInfo;
      this.CurrentPoint.TryGetPlayerActionPointInfo(eventType, out outInfo);
      int poseId = outInfo.poseID;
      this.PoseID = poseId;
      int index = poseId;
      Transform t = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) this.CurrentPoint).get_transform();
      this.Animation.RecoveryPoint = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName)?.get_transform();
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) this.ChaControl.sex][outInfo.eventID][index];
      this.Animation.LoadEventKeyTable(outInfo.eventID, outInfo.poseID);
      this.LoadEventItems(playState);
      this.LoadEventParticles(outInfo.eventID, outInfo.poseID);
      this.Animation.InitializeStates(playState);
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
      this.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      this.DeactivateNavMeshAgent();
      this.IsKinematic = true;
      this.Animation.StopAllAnimCoroutine();
      this.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, playState.MainStateInfo.FadeOutTime, actorAnimInfo2.layer);
      this.SetStand(t, actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.directionType);
    }

    [DebuggerHidden]
    public override IEnumerator LoadAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CLoadAsync\u003Ec__Iterator6()
      {
        \u0024this = this
      };
    }

    public void ReloadChara()
    {
      ActorAnimationPlayer actorAnimationPlayer1 = this._animation.CloneComponent(((Component) this.Controller).get_gameObject());
      actorAnimationPlayer1.Actor = (Actor) this;
      actorAnimationPlayer1.Character = (ActorLocomotion) this._character;
      this._animation = actorAnimationPlayer1;
      Singleton<Character>.Instance.DeleteChara(this.ChaControl, false);
      this.LoadChara(this.PlayerData.CharaFileNames[(int) this.PlayerData.Sex]);
      this.LoadEquipments();
      this.ReleaseEquipedEventItem();
      this._chaFovTargets = (Transform[]) null;
      if (this._chaBodyParts == null)
        this._chaBodyParts = new Dictionary<Actor.BodyPart, Transform>();
      this._chaBodyParts.Clear();
      this._chaBodyParts[Actor.BodyPart.Body] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Hips")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.Bust] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Mune00")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.Head] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("N_Head")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.LeftFoot] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Foot01_L")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.RightFoot] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Foot01_R")?.get_transform();
      Animator animBody = this.ChaControl.animBody;
      FullBodyBipedIK componentInChildren = (FullBodyBipedIK) ((Component) animBody).GetComponentInChildren<FullBodyBipedIK>(true);
      ActorAnimationPlayer animation = this._animation;
      ActorAnimationPlayer actorAnimationPlayer2 = this._animation.CloneComponent(((Component) animBody).get_gameObject());
      actorAnimationPlayer2.IK = componentInChildren;
      actorAnimationPlayer2.Actor = (Actor) this;
      actorAnimationPlayer2.Character = (ActorLocomotion) this._character;
      actorAnimationPlayer2.Animator = animBody;
      this._animation = actorAnimationPlayer2;
      Object.Destroy((Object) animation);
      AssetBundleInfo outInfo = (AssetBundleInfo) null;
      RuntimeAnimatorController playerAnimator = Singleton<Resources>.Instance.Animation.GetPlayerAnimator((int) this.ChaControl.sex, ref outInfo);
      this.Animation.SetDefaultAnimatorController(playerAnimator);
      this.Animation.SetAnimatorController(playerAnimator);
      this.Animation.AnimABInfo = outInfo;
      animBody.Play("Locomotion", 0, 0.0f);
      this._character.CharacterAnimation = (ActorAnimationThirdPerson) this._animation;
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), (System.Action<M0>) (_ => ((Behaviour) this._animation.IK).set_enabled(true)));
      this.InitializeIK();
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    public override void LoadEquipments()
    {
      this.LoadEquipmentItem(this.PlayerData.EquipedHeadItem, ChaControlDefine.ExtraAccessoryParts.Head);
      this.LoadEquipmentItem(this.PlayerData.EquipedBackItem, ChaControlDefine.ExtraAccessoryParts.Back);
      this.LoadEquipmentItem(this.PlayerData.EquipedNeckItem, ChaControlDefine.ExtraAccessoryParts.Neck);
    }

    [DebuggerHidden]
    public IEnumerator LoadTrialAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CLoadTrialAsync\u003Ec__Iterator7()
      {
        \u0024this = this
      };
    }

    public void LoadCamera()
    {
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      this.LoadCamera(definePack.ABPaths.CameraAdd05, Singleton<Resources>.Instance.CommonDefine.FileNames.MainCameraName, definePack.ABManifests.Add05);
    }

    public void LoadTrialCamera()
    {
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      this.LoadCamera(definePack.ABPaths.Camera, Singleton<Resources>.Instance.CommonDefine.FileNames.TrialCamera, definePack.ABManifests.Default);
    }

    private void LoadCamera(string assetBundle, string asset, string manifest = null)
    {
      GameObject gameObject1 = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(assetBundle, asset, false, manifest));
      gameObject1.get_transform().SetParent(((Component) this).get_transform(), false);
      gameObject1.get_transform().set_localPosition(Vector3.get_zero());
      gameObject1.get_transform().set_localRotation(Quaternion.get_identity());
      this._cameraCtrl = (ActorCameraControl) gameObject1.GetComponentInChildren<ActorCameraControl>();
      this._cameraCtrl.LocomotionSetting = this._locomotionSetting;
      CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(assetBundle, commonDefine.FileNames.NormalCameraName, false, manifest));
      gameObject2.get_transform().SetParent(this._cameraCtrl.VirtualCameraRoot, false);
      gameObject2.get_transform().set_localPosition(Vector3.get_zero());
      gameObject2.get_transform().set_localRotation(Quaternion.get_identity());
      GameObject gameObject3 = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(assetBundle, commonDefine.FileNames.ActionCameraFreeLookName, false, manifest));
      gameObject3.get_transform().SetParent(this._cameraCtrl.VirtualCameraRoot, false);
      gameObject3.get_transform().set_localPosition(Vector3.get_zero());
      gameObject3.get_transform().set_localRotation(Quaternion.get_identity());
      this._cameraCtrl.AssignCameraTable((CameraTable) gameObject2.GetComponent<CameraTable>(), (CameraTable) gameObject3.GetComponent<CameraTable>());
      GameObject gameObject4 = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(assetBundle, commonDefine.FileNames.ActionCameraNotMoveName, false, manifest));
      gameObject4.get_transform().SetParent(this._cameraCtrl.VirtualCameraRoot, false);
      this._cameraCtrl.ActionCameraNotMove = (CinemachineVirtualCameraBase) gameObject4.GetComponentInChildren<CinemachineVirtualCamera>();
      this._cameraCtrl.ActionCameraNotMove.set_Follow(this._locomotionSetting.ActionFollow);
      GameObject gameObject5 = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(assetBundle, commonDefine.FileNames.FishingCamera, false, manifest));
      gameObject5.get_transform().SetParent(((Component) this._controller).get_transform(), false);
      this._cameraCtrl.FishingCamera = (CinemachineVirtualCameraBase) gameObject5.GetComponent<CinemachineVirtualCamera>();
      this._cameraCtrl.SetLensSetting(Singleton<Resources>.Instance.LocomotionProfile.DefaultLensSetting);
      ActorCameraControl cameraCtrl = this._cameraCtrl;
      Quaternion rotation = this.Rotation;
      // ISSUE: variable of the null type
      __Null y = ((Quaternion) ref rotation).get_eulerAngles().y;
      cameraCtrl.XAxisValue = (float) y;
      this._cameraCtrl.YAxisValue = 0.6f;
      ((Behaviour) this._cameraCtrl).set_enabled(true);
    }

    protected override void InitializeIK()
    {
      base.InitializeIK();
      HandsHolder component = (HandsHolder) ((GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(Singleton<Resources>.Instance.DefinePack.ABPaths.ActorPrefab, this.ChaControl.fileParam.sex != (byte) 0 ? "HandsTarget_F" : "HandsTarget_M", false, "abdata"), ((Component) this).get_transform())).GetComponent<HandsHolder>();
      this.HandsHolder = component;
      HandsHolder handsHolder = component;
      ((Behaviour) handsHolder).set_enabled(false);
      handsHolder.EnabledHolding = false;
      handsHolder.LeftHandAnimator = this.ChaControl.animBody;
      handsHolder.LeftHandIK = this.ChaControl.fullBodyIK;
      GameObject loop1 = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Kosi02");
      handsHolder.LeftLookTarget = loop1.get_transform();
      if (Singleton<Resources>.Instance.LocomotionProfile.HoldingHandTarget.IsNullOrEmpty())
        return;
      GameObject loop2 = ((Component) this.ChaControl.animBody).get_transform().FindLoop(Singleton<Resources>.Instance.LocomotionProfile.HoldingHandTarget);
      if (!Object.op_Inequality((Object) loop2, (Object) null))
        return;
      handsHolder.RightHandTarget = loop2.get_transform();
    }

    [DebuggerHidden]
    protected override IEnumerator LoadCharAsync(string fileName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlayerActor.\u003CLoadCharAsync\u003Ec__Iterator8()
      {
        fileName = fileName,
        \u0024this = this
      };
    }

    private void LoadChara(string fileName)
    {
      ChaFileControl _chaFile;
      if (!fileName.IsNullOrEmpty())
      {
        _chaFile = new ChaFileControl();
        if (!_chaFile.LoadCharaFile(fileName, this.PlayerData.Sex, false, true))
          _chaFile = (ChaFileControl) null;
      }
      else
      {
        _chaFile = (ChaFileControl) null;
        string assetName = this.PlayerData.Sex != (byte) 0 ? "ill_Default_Female" : "ill_Default_Male";
        foreach (KeyValuePair<int, ListInfoBase> keyValuePair in this.PlayerData.Sex != (byte) 0 ? Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.cha_sample_f) : Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.cha_sample_m))
        {
          if (keyValuePair.Value.GetInfo(ChaListDefine.KeyType.MainData) == assetName)
          {
            _chaFile = new ChaFileControl();
            _chaFile.LoadFromAssetBundle(keyValuePair.Value.GetInfo(ChaListDefine.KeyType.MainAB), assetName, false, true);
            break;
          }
        }
      }
      if (this.PlayerData.Sex == (byte) 0)
        this.ChaControl = Singleton<Character>.Instance.CreateChara((byte) 0, ((Component) this).get_gameObject(), 0, _chaFile);
      else
        this.ChaControl = Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this).get_gameObject(), 0, _chaFile);
      this.ChaControl.isPlayer = true;
      this.ChaControl.Load(false);
      this.ChaControl.ChangeLookEyesPtn(3);
      this.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.Controller.InitializeFaceLight(((Component) this.ChaControl).get_gameObject());
    }

    private void OnEnable()
    {
      ((Behaviour) this.Animation).set_enabled(true);
      ((Behaviour) this.Locomotor).set_enabled(true);
      ((Behaviour) this.Controller).set_enabled(true);
    }

    private void OnDisable()
    {
      ((Behaviour) this.Animation).set_enabled(false);
      ((Behaviour) this.Locomotor).set_enabled(false);
      ((Behaviour) this.Controller).set_enabled(false);
    }

    private void OnDestroy()
    {
    }

    public override void EnableEntity()
    {
      ((Behaviour) this.CameraControl).set_enabled(true);
      ((Behaviour) this.Animation).set_enabled(true);
      ((Behaviour) this.Controller).set_enabled(true);
      ((Behaviour) this.Locomotor).set_enabled(true);
      this.ChaControl.visibleAll = true;
    }

    public override void DisableEntity()
    {
      ((Behaviour) this.CameraControl).set_enabled(false);
      ((Behaviour) this.Animation).set_enabled(false);
      ((Behaviour) this.Controller).set_enabled(false);
      ((Behaviour) this.Locomotor).set_enabled(false);
      this.ChaControl.visibleAll = false;
    }

    private void OnUpdate()
    {
      ((Component) this._navMeshObstacle).get_transform().set_position(this.Position);
      this._coolTime += Time.get_deltaTime();
      PlayerData playerData = this.PlayerData;
      if (this._called)
      {
        this._callElapsedTime += Time.get_unscaledDeltaTime();
        if ((double) this._callElapsedTime > 2.0)
        {
          this._called = false;
          this._callElapsedTime = 0.0f;
        }
      }
      if (this._schedule.enabled && !this._schedule.useGameTime && this._schedule.progress)
      {
        this._schedule.elapsedTime += Time.get_deltaTime();
        if ((double) this._schedule.elapsedTime > (double) this._schedule.duration)
          this._schedule.enabled = false;
      }
      if (!this._scaleCtrlInfos.IsNullOrEmpty<Actor.ItemScaleInfo>())
      {
        foreach (Actor.ItemScaleInfo scaleCtrlInfo in this._scaleCtrlInfos)
        {
          if (scaleCtrlInfo.ScaleMode == 0)
          {
            float shapeBodyValue = this.ChaControl.GetShapeBodyValue(0);
            float num = scaleCtrlInfo.Evaluate(shapeBodyValue);
            scaleCtrlInfo.TargetItem.get_transform().set_localScale(new Vector3(num, num, num));
          }
        }
      }
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
      if (simulator.EnabledTimeProgression)
      {
        Weather weather = simulator.Weather;
        if (this.AreaType == MapArea.AreaType.Indoor)
        {
          playerData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
        }
        else
        {
          switch (weather)
          {
            case Weather.Rain:
              playerData.Wetness += statusProfile.WetRateInRain * Time.get_deltaTime();
              break;
            case Weather.Storm:
              playerData.Wetness += statusProfile.WetRateInStorm * Time.get_deltaTime();
              break;
            default:
              playerData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
              break;
          }
          playerData.Wetness = Mathf.Clamp(playerData.Wetness, 0.0f, 100f);
        }
      }
      if (Object.op_Inequality((Object) this.ChaControl, (Object) null))
        this.ChaControl.wetRate = Mathf.InverseLerp(0.0f, 100f, playerData.Wetness);
      playerData.Position = this.Position;
      playerData.Rotation = this.Rotation;
      playerData.ChunkID = this.ChunkID;
      if (this._mapAreaID != null && Object.op_Inequality((Object) this.MapArea, (Object) null))
      {
        ((ReactiveProperty<int>) this._mapAreaID).set_Value(this.MapArea.AreaID);
        this.PlayerData.AreaID = this.MapArea.AreaID;
      }
      if (!Object.op_Inequality((Object) this.HandsHolder, (Object) null))
        return;
      this.HandsHolder.BaseTransform.set_position(this.Position);
      this.HandsHolder.BaseTransform.set_rotation(this.Rotation);
    }

    private void OnLateUpdate()
    {
      if (!Object.op_Inequality((Object) this._cameraCtrl, (Object) null) || !((Behaviour) this._cameraCtrl).get_enabled())
        return;
      if (!(this.Controller.State is Communication) && !(this.Controller.State is Sex) && !(this.Controller.State is Fishing))
      {
        if (this._cameraCtrl.Mode == CameraMode.ADV || this._cameraCtrl.Mode == CameraMode.Fishing || this._cameraCtrl.Mode == CameraMode.Event)
          return;
        bool flag = true & this._cameraCtrl.ShotType != ShotType.PointOfView;
        if (flag)
        {
          if (!(this.Controller.State is AIProject.Player.Move))
            flag &= this.IsVisibleDistanceAll(((Component) this._cameraCtrl).get_transform());
          else
            flag &= this.IsVisibleDistance(((Component) this._cameraCtrl).get_transform(), Actor.BodyPart.Head, Singleton<Resources>.Instance.LocomotionProfile.CharaVisibleDistance);
        }
        this.ChaControl.visibleAll = this.IsVisible && flag;
        if (this.Mode != Desire.ActionType.Onbu || !Object.op_Inequality((Object) this.Partner, (Object) null))
          return;
        this.Partner.IsVisible = this.IsVisible && flag;
      }
      else
      {
        if (!(this.Controller.State is Fishing))
          return;
        bool flag = this.IsVisibleDistanceAll(((Component) this._cameraCtrl).get_transform());
        this.ChaControl.visibleAll = this.IsVisible && flag;
      }
    }

    public void ActivateTransfer()
    {
      EquipEventItemInfo itemInfo = (EquipEventItemInfo) null;
      PlayState info;
      this.LoadLocomotionAnimation(out info, ref itemInfo);
      this.ResetEquipEventItem(itemInfo);
      ActorAnimation animation = this.Animation;
      AnimatorStateInfo animatorStateInfo = this.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      if (info != null)
      {
        animation.InitializeStates(info);
        bool flag = false;
        foreach (PlayState.Info stateInfo in info.MainStateInfo.InStateInfo.StateInfos)
        {
          if (((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash() == stateInfo.ShortNameStateHash)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          if (info.MaskStateInfo.layer > 0)
          {
            if ((double) animation.Animator.GetLayerWeight(info.MaskStateInfo.layer) == 0.0)
              flag = false;
          }
          else
          {
            for (int index = 1; index < animation.Animator.get_layerCount(); ++index)
            {
              if ((double) animation.Animator.GetLayerWeight(index) > 0.0)
              {
                flag = false;
                break;
              }
            }
          }
        }
        if (flag)
        {
          this.Animation.InStates.Clear();
          this.Animation.OutStates.Clear();
          this.Animation.ActionStates.Clear();
        }
        else
        {
          int layer = info.Layer;
          if (animation.RefsActAnimInfo)
          {
            animation.StopAllAnimCoroutine();
            animation.PlayInLocoAnimation(animation.AnimInfo.endEnableBlend, animation.AnimInfo.endBlendSec, layer);
            animation.RefsActAnimInfo = false;
          }
          else
          {
            bool enableFade = info.MainStateInfo.InStateInfo.EnableFade;
            float fadeSecond = info.MainStateInfo.InStateInfo.FadeSecond;
            animation.StopAllAnimCoroutine();
            animation.PlayInLocoAnimation(enableFade, fadeSecond, layer);
          }
        }
      }
      else
      {
        for (int index = 1; index < animation.Animator.get_layerCount(); ++index)
          animation.Animator.SetLayerWeight(index, 0.0f);
        string normalLocoStateName = Singleton<Resources>.Instance.PlayerProfile.PoseIDData.NormalLocoStateName;
        int hash = Animator.StringToHash(normalLocoStateName);
        if (((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash() == hash)
          return;
        animation.StopAllAnimCoroutine();
        if (animation.AnimInfo.endEnableBlend)
        {
          this.Animation.CrossFadeAnimation(normalLocoStateName, animation.AnimInfo.outBlendSec, 0, 0.0f);
        }
        else
        {
          this.CameraControl.CrossFade.FadeStart(-1f);
          this.Animation.PlayAnimation(normalLocoStateName, 0, 0.0f);
        }
      }
    }

    private void LoadLocomotionAnimation(out PlayState info, ref EquipEventItemInfo itemInfo)
    {
      Resources instance = Singleton<Resources>.Instance;
      LocomotionProfile locomotionProfile = instance.LocomotionProfile;
      PlayerProfile playerProfile = instance.PlayerProfile;
      StuffItem equipedLampItem = this.PlayerData.EquipedLampItem;
      CommonDefine.ItemIDDefines itemIdDefine = instance.CommonDefine.ItemIDDefine;
      if (equipedLampItem != null)
      {
        ItemIDKeyPair torchId = itemIdDefine.TorchID;
        ItemIDKeyPair flashlightId = itemIdDefine.FlashlightID;
        ItemIDKeyPair maleLampId = itemIdDefine.MaleLampID;
        if (equipedLampItem.CategoryID == torchId.categoryID && equipedLampItem.ID == torchId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) this.ChaControl.sex][playerProfile.PoseIDData.TorchLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[torchId.categoryID][torchId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
        if (equipedLampItem.CategoryID == flashlightId.categoryID && equipedLampItem.ID == flashlightId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) this.ChaControl.sex][playerProfile.PoseIDData.TorchLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[flashlightId.categoryID][flashlightId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
        if (equipedLampItem.CategoryID == maleLampId.categoryID && equipedLampItem.ID == maleLampId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) this.ChaControl.sex][playerProfile.PoseIDData.LampLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[maleLampId.categoryID][maleLampId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
      }
      info = (PlayState) null;
    }

    public override void OnMinuteUpdated(TimeSpan deltaTime)
    {
      this._elapsedTimeInSleep += deltaTime;
      if (!this._schedule.enabled || !this._schedule.useGameTime || !this._schedule.progress)
        return;
      this._schedule.elapsedTime += (float) deltaTime.TotalMinutes;
      if ((double) this._schedule.elapsedTime <= (double) this._schedule.duration)
        return;
      this._schedule.enabled = false;
    }

    public void ReleaseCurrentPoint()
    {
      if (!Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        return;
      this.SetDefaultStateHousingItem();
      ActionPoint currentPoint = this.CurrentPoint;
      this.CurrentPoint = (ActionPoint) null;
      CommandArea commandArea = this.PlayerController.CommandArea;
      commandArea.RemoveConsiderationObject((ICommandable) currentPoint);
      commandArea.RefreshCommands();
    }

    public override bool CanAddItem(StuffItem sourceItem)
    {
      if (this.PlayerData.ItemList.Count < this.PlayerData.InventorySlotMax)
        return true;
      foreach (StuffItem stuffItem in this.PlayerData.ItemList)
      {
        if (stuffItem.CategoryID == sourceItem.CategoryID && stuffItem.ID == sourceItem.ID && stuffItem.Count + sourceItem.Count < Singleton<Resources>.Instance.DefinePack.MapDefines.ItemStackUpperLimit)
          return true;
      }
      return false;
    }

    public override bool CanAddItem(StuffItemInfo item)
    {
      if (this.PlayerData.ItemList.Count < this.PlayerData.InventorySlotMax)
        return true;
      foreach (StuffItem stuffItem in this.PlayerData.ItemList)
      {
        if (stuffItem.CategoryID == item.CategoryID && stuffItem.ID == item.ID && stuffItem.Count < Singleton<Resources>.Instance.DefinePack.MapDefines.ItemStackUpperLimit)
          return true;
      }
      return false;
    }

    protected override void LoadEquipedEventItem(EquipEventItemInfo eventItemInfo)
    {
      AssetBundleInfo assetbundleInfo = eventItemInfo.ActionItemInfo.assetbundleInfo;
      if (((string) assetbundleInfo.assetbundle).IsNullOrEmpty() || ((string) assetbundleInfo.asset).IsNullOrEmpty() || ((string) assetbundleInfo.manifest).IsNullOrEmpty())
        return;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>((string) assetbundleInfo.assetbundle, (string) assetbundleInfo.asset, false, (string) assetbundleInfo.manifest);
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
      {
        GameObject loop = ((Component) this.ChaControl.animBody).get_transform().FindLoop(Singleton<Resources>.Instance.LocomotionProfile.PlayerLocoItemParentName);
        if (!Object.op_Inequality((Object) loop, (Object) null))
          return;
        GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, loop.get_transform(), false);
        ((Object) gameObject2.get_gameObject()).set_name(((Object) gameObject1.get_gameObject()).get_name());
        gameObject2.get_transform().set_localPosition(Vector3.get_zero());
        gameObject2.get_transform().set_localRotation(Quaternion.get_identity());
        gameObject2.get_transform().set_localScale(Vector3.get_one());
        gameObject2.SetActive(true);
        this.EquipedItem = new ItemCache()
        {
          EventItemID = eventItemInfo.EventItemID,
          KeyInfo = eventItemInfo.ActionItemInfo,
          AsGameObject = gameObject2
        };
        if (!eventItemInfo.ActionItemInfo.existsAnimation)
          return;
        Animator component = (Animator) gameObject2.GetComponent<Animator>();
        RuntimeAnimatorController animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(eventItemInfo.ActionItemInfo.animeAssetBundle);
        component.set_runtimeAnimatorController(animatorController);
        this.Animation.ItemAnimatorTable[((Object) gameObject2).GetInstanceID()] = new ItemAnimInfo()
        {
          Animator = component,
          Parameters = component.get_parameters(),
          Sync = true
        };
      }
      else
        Debug.LogError((object) string.Format("イベントアイテム読み込み失敗： バンドルパス[{0}] プレハブ[{1}] マニフェスト[{2}]", (object) assetbundleInfo.assetbundle, (object) assetbundleInfo.asset, (object) assetbundleInfo.manifest));
    }

    public override void LoadEventItems(PlayState playState)
    {
      if (playState.ItemInfoCount <= 0)
        return;
      for (int index = 0; index < playState.ItemInfoCount; ++index)
      {
        PlayState.ItemInfo itemInfo = playState.GetItemInfo(index);
        Resources instance = Singleton<Resources>.Instance;
        if (itemInfo.fromEquipedItem)
        {
          ActionPointInfo outInfo;
          Dictionary<int, EquipEventItemInfo> dictionary;
          EquipEventItemInfo equipEventItemInfo;
          if (this.CurrentPoint.TryGetPlayerActionPointInfo(this.EventKey, out outInfo) && instance.GameInfo.SearchEquipEventItemTable.TryGetValue(outInfo.searchAreaID, out dictionary) && dictionary.TryGetValue(outInfo.gradeValue, out equipEventItemInfo))
            this.LoadEventItem(equipEventItemInfo.EventItemID, itemInfo, equipEventItemInfo.ActionItemInfo);
        }
        else
        {
          ActionItemInfo eventItemInfo;
          if (instance.Map.EventItemList.TryGetValue(itemInfo.itemID, out eventItemInfo))
            this.LoadEventItem(itemInfo.itemID, itemInfo, eventItemInfo);
        }
      }
    }

    public override void LoadEventParticles(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> dictionary1;
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary2;
      Dictionary<int, List<AnimeParticleEventInfo>> eventTable;
      if (!Singleton<Resources>.Instance.Animation.PlayerActParticleEventKeyTable.TryGetValue((int) this.ChaControl.sex, out dictionary1) || !dictionary1.TryGetValue(eventID, out dictionary2) || (!dictionary2.TryGetValue(poseID, out eventTable) || eventTable == null))
        return;
      this.LoadEventParticle(eventTable);
    }

    public void AddTutorialUI(Popup.Tutorial.Type type, bool groupDisplay)
    {
      int key = (int) type;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<int, bool> dictionary = worldData == null ? (Dictionary<int, bool>) null : worldData.TutorialOpenStateTable;
      if (dictionary != null)
      {
        bool flag = false;
        if (!dictionary.TryGetValue(key, out flag))
          dictionary[key] = flag = false;
        if (flag)
          return;
        if (!flag)
          dictionary[key] = true;
      }
      this.TutorialIndexList.Add(new ValueTuple<Popup.Tutorial.Type, bool>(type, groupDisplay));
    }
  }
}
