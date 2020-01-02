// Decompiled with JetBrains decompiler
// Type: AIProject.AgentActor
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
using AIProject.UI.Viewer;
using Cinemachine;
using Illusion.Extensions;
using IllusionUtility.GetUtility;
using Manager;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEx;

namespace AIProject
{
  public class AgentActor : Actor, ICommandable, INavMeshActor
  {
    private static readonly Actor.FovBodyPart[] _checkParts = new Actor.FovBodyPart[2]
    {
      Actor.FovBodyPart.Head,
      Actor.FovBodyPart.Body
    };
    private static readonly YieldInstruction _waitMinute = (YieldInstruction) new WaitForSeconds(1f);
    private AgentActor.TouchDisposableInfo _disposableInfo = new AgentActor.TouchDisposableInfo();
    private RaycastHit[] _hits = new RaycastHit[10];
    public bool _canTalkCache = true;
    public bool _useNeckLookCache = true;
    public bool _canHCommandCache = true;
    private AIProject.Definitions.State.Type _stateType = AIProject.Definitions.State.Type.Normal;
    [SerializeField]
    private float _impossibleCooldownTime = 3f;
    private float _distanceTweenPlayer = float.MaxValue;
    private float _heightDistTweenPlayer = float.MaxValue;
    private float _angleDiffTweenPlayer = float.MaxValue;
    [SerializeField]
    private ObjectLayer _layer = ObjectLayer.Command;
    private Dictionary<Actor, bool> _actorConsideredTable = new Dictionary<Actor, bool>();
    private List<ActionPoint> _actionPoints = new List<ActionPoint>();
    private Dictionary<ActionPoint, bool> _actionPointCheckFlagTable = new Dictionary<ActionPoint, bool>();
    [SerializeField]
    private Threshold _coolTimeThreshold = new Threshold(0.0f, 1f);
    private ValueTuple<Desire.ActionType, Desire.ActionType, bool> _modeCache = (ValueTuple<Desire.ActionType, Desire.ActionType, bool>) null;
    private int yureID = -1;
    private int HLayerID = -1;
    private int HVoiceID = -1;
    private List<IDisposable> MasturbationDisposables = new List<IDisposable>();
    private float height = -1f;
    private float breast = -1f;
    private YureCtrl.YureCtrlMapH[] HYureCtrlLes = new YureCtrl.YureCtrlMapH[2];
    private int[] yureLesID = new int[2]{ -1, -1 };
    private int[] HLayerLesID = new int[2]{ -1, -1 };
    private int HVoiceLesID = -1;
    private ChaControl[] lesChars = new ChaControl[2];
    private ActorAnimation[] lesCharAnimes = new ActorAnimation[2];
    private List<IDisposable> lesbianDisposable = new List<IDisposable>();
    private float[] timeMotions = new float[2];
    private bool[] enableMotions = new bool[2];
    private bool[] allowMotions = new bool[2]{ true, true };
    private Vector2[] lerpMotions = new Vector2[2]
    {
      Vector2.get_zero(),
      Vector2.get_zero()
    };
    private float[] lerpTimes = new float[2];
    protected float[] timeChangeMotions = new float[2];
    protected float[] timeChangeMotionDeltaTimes = new float[2];
    private List<Waypoint> _reservedWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedNearActionWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedDateNearActionWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedLocationWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedActorWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedPlayerWaypoints = new List<Waypoint>();
    private List<Waypoint> _reservedAnimalWaypoints = new List<Waypoint>();
    private CommandLabel.CommandInfo[] _labels;
    private CommandLabel.CommandInfo[] _pEventLabels;
    private CommandLabel.CommandInfo[] _pEventFishingLabels;
    private CommandLabel.CommandInfo[] _eventLabels;
    private CommandLabel.CommandInfo[] _sleepLabels;
    private CommandLabel.CommandInfo[] _bathLabels;
    private CommandLabel.CommandInfo[] _isuToiletLabels;
    private CommandLabel.CommandInfo[] _shagamiLabels;
    private CommandLabel.CommandInfo[] _shagamiFoundLabels;
    private CommandLabel.CommandInfo[] _searchLabels;
    private CommandLabel.CommandInfo[] _cookLabels;
    private CommandLabel.CommandInfo[] _standLabels;
    private CommandLabel.CommandInfo[] _mapBathLabels;
    private CommandLabel.CommandInfo[] _holeLabels;
    private CommandLabel.CommandInfo[] _sleepOnanismLabels;
    private CommandLabel.CommandInfo[] _deskBackLabels;
    private CommandLabel.CommandInfo[] _collapseLabels;
    private CommandLabel.CommandInfo[] _coldLabels;
    private CommandLabel.CommandInfo[] _tutorialPassFishingRodLabels;
    private CommandLabel.CommandInfo[] _tutorialFoodRequestLabels;
    private CommandLabel.CommandInfo[] _tutorialWaitAtBaseLabels;
    private CommandLabel.CommandInfo[] _tutorialGrilledFishRequestLabels;
    private CommandLabel.CommandInfo[] _dateLabels;
    private List<AgentActor.TouchInfo> _touchList;
    private List<AgentActor.ColDisposableInfo> _colDisposableList;
    private ActionPoint _recoveryActionPointFromHScene;
    private Desire.ActionType _recoveryModeFromHScene;
    public int _attitudeIDCache;
    public bool _isSpecialCache;
    public int _hPositionIDCache;
    public int _hPositionSubIDCache;
    public float _obstacleSizeCache;
    private Dictionary<int, System.Action> _presentParameterTable;
    private AgentActor.LeaveAloneDisposableInfo _ladInfo;
    [SerializeField]
    private ActorAnimationAgent _animation;
    [SerializeField]
    private ActorLocomotionAgent _character;
    [SerializeField]
    private AgentController _controller;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private Vector3[] _positions;
    private NavMeshPath _pathForCalc;
    private float _elapsedTimeFromLastImpossible;
    private bool _isStandby;
    private bool _prevCloseToPlayer;
    public int SearchAreaID;
    private Desire.ActionType _modeType;
    private Desire.Type _requestedDesire;
    private ReadOnlyCollection<ActionPoint> _readonlyActionPoints;
    private Dictionary<int, DateTimeThreshold[]> _desireDailyRhythm;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private ActionPoint _targetInSightActionPoint;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private Actor _targetInSightActor;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private AnimalBase _targetInSightAnimal;
    private float _maxMotivationInMasturbation;
    private float _runtimeMotivationInMasturbation;
    private List<string> abName;
    private HParticleCtrl HParticle;
    private HSeCtrl Hse;
    private HItemCtrl HItem;
    private GameObject HItemPlace;
    private HSceneManager hSceneManager;
    private HScene hScene;
    private HSceneFlagCtrl hFlagCtrl;
    private HVoiceCtrl HVoiceCtrl;
    private YureCtrl.YureCtrlMapH HYureCtrl;
    private HLayerCtrl HLayerCtrl;
    private IEnumerator _masturbationEnumerator;
    private IDisposable _masturbationDisposable;
    private float _maxMotivationInLesbian;
    private float _runtimeMotivationInLesbian;
    private IEnumerator _lesbianHEnumerator;
    private IDisposable _lesbianHDisposable;
    private IDisposable advEventResetDisposable;
    private NavMeshPath _calcElementPath;
    private IEnumerator _patrolCoroutine;
    private IEnumerator _calcEnumerator;
    private IEnumerator _additiveCalcProcess;
    private IEnumerator _actionPatrolCoroutine;
    private IEnumerator _actionCalcEnumerator;
    private IEnumerator _additiveActionCalcProcess;
    private IEnumerator _dateActionPatrolCoroutine;
    private IEnumerator _dateActionCalcEnumerator;
    private IEnumerator _additiveDateActionCalcProcess;
    private IEnumerator _locationPatrolCoroutine;
    private IEnumerator _locationCalcEnumerator;
    private IEnumerator _additiveLocationCalcProcess;
    private IEnumerator _actorPatrolCoroutine;
    private IEnumerator _actorCalcEnumerator;
    private IEnumerator _additiveActorCalcProcess;
    private IEnumerator _playerPatrolCoroutine;
    private IEnumerator _playerCalcEnumerator;
    private IEnumerator _additivePlayerCalcProcess;
    private IEnumerator _animalPatrolCoroutine;
    private IEnumerator _animalCalcEnumerator;
    private IEnumerator _additiveAnimalCalcProcess;

    public bool IsBadMood()
    {
      if (this.ChaControl.fileGameInfo.phase < 2)
        this.AgentData.StatsTable[1] = (float) (((double) this.ChaControl.fileGameInfo.moodBound.lower + (double) this.ChaControl.fileGameInfo.moodBound.upper) / 2.0);
      return (double) this.AgentData.StatsTable[1] <= (double) this.ChaControl.fileGameInfo.moodBound.lower;
    }

    public bool IsGoodMood()
    {
      if (this.ChaControl.fileGameInfo.phase < 2)
        this.AgentData.StatsTable[1] = (float) (((double) this.ChaControl.fileGameInfo.moodBound.lower + (double) this.ChaControl.fileGameInfo.moodBound.upper) / 2.0);
      return (double) this.AgentData.StatsTable[1] > (double) this.ChaControl.fileGameInfo.moodBound.upper;
    }

    public int charaID
    {
      get
      {
        return this.AgentData.param.charaID;
      }
    }

    public bool IsAdvEvent
    {
      get
      {
        return !this.advEventName.IsNullOrEmpty() || this.CheckFishingEvent();
      }
    }

    public bool CheckEventADV(int eventType)
    {
      this.advEventParam = this.GetAdvEvent(eventType);
      this.advEventName = this.advEventParam?.FileName;
      return !this.advEventName.IsNullOrEmpty();
    }

    private string advEventName { get; set; }

    private AgentAdvEventInfo.Param advEventParam { get; set; }

    public OpenData openData { get; } = new OpenData();

    public AgentActor.PackData packData { get; private set; }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        if (this.TutorialMode)
        {
          switch (this.TutorialType)
          {
            case AIProject.Definitions.Tutorial.ActionType.PassFishingRod:
              return this._tutorialPassFishingRodLabels;
            case AIProject.Definitions.Tutorial.ActionType.FoodRequest:
              return this._tutorialFoodRequestLabels;
            case AIProject.Definitions.Tutorial.ActionType.WaitAtBase:
              return this._tutorialWaitAtBaseLabels;
            case AIProject.Definitions.Tutorial.ActionType.GrilledFishRequest:
              return this._tutorialGrilledFishRequestLabels;
            default:
              return (CommandLabel.CommandInfo[]) null;
          }
        }
        else
        {
          if (!Singleton<Manager.Map>.IsInstance() || !(Singleton<Manager.Map>.Instance.Player.PlayerController.State is Normal))
            return (CommandLabel.CommandInfo[]) null;
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          switch (this.Mode)
          {
            case Desire.ActionType.Date:
              return this.DateLabels;
            case Desire.ActionType.Onbu:
              return (CommandLabel.CommandInfo[]) null;
            default:
              if (this.CheckFishingEvent())
                return this._pEventFishingLabels;
              if (this.CheckEventADV(1))
                return this._pEventLabels;
              switch (this.StateType)
              {
                case AIProject.Definitions.State.Type.Immobility:
                  return (CommandLabel.CommandInfo[]) null;
                case AIProject.Definitions.State.Type.Collapse:
                  return this._collapseLabels;
                case AIProject.Definitions.State.Type.Cold:
                  return !this.AgentData.SickState.UsedMedicine && !this.AgentData.SickState.Enabled ? this._coldLabels : (CommandLabel.CommandInfo[]) null;
                default:
                  if (this.IsSpecial)
                  {
                    if (player.ChaControl.sex == (byte) 1 && !player.ChaControl.fileParam.futanari)
                    {
                      if (this.HPositionID != 1)
                        return (CommandLabel.CommandInfo[]) null;
                      return this.Mode == Desire.ActionType.EndTaskSecondSleep || this.Mode == Desire.ActionType.ShallowSleep ? (CommandLabel.CommandInfo[]) null : this._sleepLabels;
                    }
                    switch (this.HPositionID)
                    {
                      case 1:
                        return this.Mode == Desire.ActionType.EndTaskSecondSleep || this.Mode == Desire.ActionType.ShallowSleep ? (CommandLabel.CommandInfo[]) null : this._sleepLabels;
                      case 2:
                        return this._bathLabels;
                      case 3:
                        return this._isuToiletLabels;
                      case 4:
                        return this._shagamiLabels;
                      case 5:
                        return this._shagamiFoundLabels;
                      case 6:
                        return this._searchLabels;
                      case 7:
                        return this._cookLabels;
                      case 8:
                        return this._standLabels;
                      case 13:
                        return this._mapBathLabels;
                      case 14:
                        return this._holeLabels;
                      case 16:
                        return this._sleepOnanismLabels;
                      case 17:
                        return this._deskBackLabels;
                    }
                  }
                  else
                  {
                    if (this.CheckEventADV(0))
                      return this._pEventLabels;
                    if (this.IsStandby)
                      return this._eventLabels;
                    if (this.CanTalk)
                      return this._labels;
                  }
                  return (CommandLabel.CommandInfo[]) null;
              }
          }
        }
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        switch (this.StateType)
        {
          case AIProject.Definitions.State.Type.Immobility:
            return (CommandLabel.CommandInfo[]) null;
          case AIProject.Definitions.State.Type.Collapse:
            return (CommandLabel.CommandInfo[]) null;
          case AIProject.Definitions.State.Type.Cold:
            return !this.AgentData.SickState.UsedMedicine && !this.AgentData.SickState.Enabled ? this._coldLabels : (CommandLabel.CommandInfo[]) null;
          default:
            if (this.IsSpecial)
              return (CommandLabel.CommandInfo[]) null;
            return this.CanTalk ? this._dateLabels : (CommandLabel.CommandInfo[]) null;
        }
      }
    }

    private CommCommandList.CommandInfo[] NormalCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] TalkCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] PraiseCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] InstructionCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] DateCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] DateCommandOptionInfosTP { get; set; }

    private CommCommandList.CommandInfo[] SpecialCommandOptionInfos { get; set; }

    private CommCommandList.CommandInfo[] ColdCommandOptionInfos { get; set; }

    public int TouchCount { get; set; }

    public bool CanTalk { get; set; } = true;

    public int AttitudeID { get; set; }

    public bool UseNeckLook { get; set; } = true;

    public bool CanHCommand { get; set; } = true;

    public bool IsSpecial { get; set; }

    public int HPositionID { get; set; }

    public int HPositionSubID { get; set; }

    public void ResetActionFlag()
    {
      this.UseNeckLook = true;
      this.CanTalk = true;
      this.CanHCommand = true;
      this.AttitudeID = 0;
      this.IsSpecial = false;
      this.HPositionID = 0;
      this.HPositionSubID = 0;
      if (!Object.op_Inequality((Object) this._navMeshObstacle, (Object) null))
        return;
      this._navMeshObstacle.set_radius(2f);
    }

    public void LoadActionFlag(int actionID, int poseID)
    {
      Dictionary<int, ActAnimFlagData> dictionary;
      ActAnimFlagData actAnimFlagData;
      if (Singleton<Resources>.Instance.Action.AgentActionFlagTable.TryGetValue(actionID, out dictionary) && dictionary.TryGetValue(poseID, out actAnimFlagData))
      {
        this.UseNeckLook = actAnimFlagData.useNeckLook;
        this.CanTalk = actAnimFlagData.canTalk;
        this.AttitudeID = actAnimFlagData.attitudeID;
        this.CanHCommand = actAnimFlagData.canHCommand;
        this.IsSpecial = actAnimFlagData.isSpecial;
        this.HPositionID = actAnimFlagData.hPositionID1;
        this.HPositionSubID = actAnimFlagData.hPositionID2;
        if (!Object.op_Inequality((Object) this._navMeshObstacle, (Object) null))
          return;
        this._navMeshObstacle.set_radius(actAnimFlagData.obstacleRadius);
      }
      else
      {
        this.UseNeckLook = false;
        this.CanTalk = false;
        this.AttitudeID = -1;
        this.CanHCommand = false;
        this.IsSpecial = false;
        this.HPositionID = -1;
        this.HPositionSubID = -1;
        if (!Object.op_Inequality((Object) this._navMeshObstacle, (Object) null))
          return;
        this._navMeshObstacle.set_radius(2f);
      }
    }

    public void DisableActionFlag()
    {
      this.UseNeckLook = false;
      this.CanTalk = false;
      this.AttitudeID = 0;
      this.CanHCommand = false;
      this.IsSpecial = false;
      this.HPositionID = -1;
      this.HPositionSubID = -1;
      if (!Object.op_Inequality((Object) this._navMeshObstacle, (Object) null))
        return;
      this._navMeshObstacle.set_radius(2f);
    }

    public bool WaitTutorialUIDisplay()
    {
      bool? isBlending = (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.CameraControl?.CinemachineBrain?.get_IsBlending();
      if ((!isBlending.HasValue ? 0 : (isBlending.Value ? 1 : 0)) == 0)
      {
        bool? playingTurnAnimation = this.Animation?.PlayingTurnAnimation;
        if ((!playingTurnAnimation.HasValue ? 0 : (playingTurnAnimation.Value ? 1 : 0)) == 0)
        {
          AudioSource asVoice = this.ChaControl?.asVoice;
          return !Object.op_Equality((Object) asVoice, (Object) null) && !asVoice.get_loop();
        }
      }
      return true;
    }

    private void InitCommands()
    {
      Transform transform = ((Component) this.ChaControl).get_transform().FindLoop("cf_J_Mune00")?.get_transform();
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActorIconTable.TryGetValue(this.ID, out sprite);
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          CoolTimeFillRate = (Func<float>) (() =>
          {
            bool lockTalk = this.AgentData.LockTalk;
            float talkElapsedTime = this.AgentData.TalkElapsedTime;
            float talkLockDuration = Singleton<Resources>.Instance.AgentProfile.TalkLockDuration;
            return lockTalk ? 1f - Mathf.Clamp(talkElapsedTime / talkLockDuration, 0.0f, 1f) : 0.0f;
          }),
          Event = (System.Action) (() =>
          {
            if (this.CheckStealEvent())
              this.StartStealEvent();
            else if (this.CanProgressPhase())
              this.StartPhaseEvent();
            else if (this.CheckCatEvent())
              this.StartCatEvent();
            else if (this.AgentData.LockTalk)
            {
              this.StartCommunication();
              ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => this.Animation.PlayingTurnAnimation)), 1), (System.Action<M0>) (_ =>
              {
                this.openData.FindLoad("10", this.charaID, 0);
                this.packData.onComplete = (System.Action) (() =>
                {
                  this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                  this.EndCommunication();
                });
                Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
              }));
            }
            else
            {
              this.StartCommunication();
              if (this.AgentData.SickState.ID == 0)
                this.openData.FindLoad("9", this.charaID, 4);
              else if (this.AgentData.SickState.ID == 4)
                this.openData.FindLoad("11", this.charaID, 4);
              else
                this.openData.FindLoad("0", this.charaID, 4);
              this.packData.onComplete = (System.Action) (() =>
              {
                int tutorialID = 4;
                bool flag = Singleton<MapUIContainer>.IsInstance() && !MapUIContainer.GetTutorialOpenState(tutorialID);
                this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                if (!flag)
                  this.PopupCommands(this.NormalCommandOptionInfos, (System.Action) (() =>
                  {
                    this.StartCommonSelection();
                    MapUIContainer.CommandList.OnOpened = (System.Action) null;
                  }));
                else
                  ObservableExtensions.Subscribe<long>(Observable.Delay<long>(Observable.Take<long>(Observable.SkipWhile<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Func<M0, bool>) (_ => this.WaitTutorialUIDisplay())), 1), TimeSpan.FromSeconds(0.5)), (System.Action<M0>) (_ =>
                  {
                    MapUIContainer.TutorialUI.ClosedEvent = (System.Action) (() => this.PopupCommands(this.NormalCommandOptionInfos, (System.Action) (() =>
                    {
                      this.StartCommonSelection();
                      MapUIContainer.CommandList.OnOpened = (System.Action) null;
                    })));
                    MapUIContainer.OpenTutorialUI(tutorialID, false);
                  }));
              });
              Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
            }
          })
        }
      };
      this._pEventLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          CoolTimeFillRate = (Func<float>) (() =>
          {
            bool lockTalk = this.AgentData.LockTalk;
            float talkElapsedTime = this.AgentData.TalkElapsedTime;
            float talkLockDuration = Singleton<Resources>.Instance.AgentProfile.TalkLockDuration;
            return lockTalk ? 1f - Mathf.Clamp(talkElapsedTime / talkLockDuration, 0.0f, 1f) : 0.0f;
          }),
          Event = (System.Action) (() => this.AdvEventStart(this.advEventName))
        }
      };
      this._pEventFishingLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          CoolTimeFillRate = (Func<float>) (() =>
          {
            bool lockTalk = this.AgentData.LockTalk;
            float talkElapsedTime = this.AgentData.TalkElapsedTime;
            float talkLockDuration = Singleton<Resources>.Instance.AgentProfile.TalkLockDuration;
            return lockTalk ? 1f - Mathf.Clamp(talkElapsedTime / talkLockDuration, 0.0f, 1f) : 0.0f;
          }),
          Event = (System.Action) (() => this.FishingEvent())
        }
      };
      this._eventLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() => this.StartEvent())
        }
      };
      this._sleepLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("1", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._bathLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("2", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._isuToiletLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("3", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._shagamiLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("4", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._shagamiFoundLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("5", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._searchLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("6", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._cookLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("7", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._standLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("8", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._mapBathLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("2", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._holeLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("6", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._sleepOnanismLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("6", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._deskBackLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}にアクション", (object) this.CharaName)),
          Text = string.Format("{0}にアクション", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("6", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.SpecialCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._collapseLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}を担ぐ", (object) this.CharaName)),
          Text = string.Format("{0}を担ぐ", (object) this.CharaName),
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            Singleton<Manager.Map>.Instance.Player.Partner = (Actor) this;
            this.PrevMode = this.Mode;
            this.Mode = Desire.ActionType.Onbu;
            Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Onbu");
          })
        }
      };
      this._coldLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}に話しかける", (object) this.CharaName)),
          Text = string.Format("{0}に話しかける", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartCommunication();
            this.openData.FindLoad("10", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupCommands(this.ColdCommandOptionInfos, (System.Action) null);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._dateLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() =>
          {
            this.StartDateCommunication();
            this.openData.FindLoad("12", this.charaID, 4);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.PopupDateCommands(Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Player.Partner, (Object) this));
            });
            this.packData.isThisPartner = Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Player.Partner, (Object) this);
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        }
      };
      this._tutorialPassFishingRodLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() => this.StartTutorialADV(1))
        }
      };
      this._tutorialFoodRequestLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() => this.StartTutorialADV(2))
        }
      };
      this._tutorialWaitAtBaseLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() => this.StartTutorialADV(3))
        }
      };
      this._tutorialGrilledFishRequestLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          OnText = (Func<string>) (() => string.Format("{0}と話す", (object) this.CharaName)),
          Text = string.Format("{0}と話す", (object) this.CharaName),
          Icon = sprite,
          TargetSpriteInfo = icon.CharaSpriteInfo,
          Transform = transform,
          Event = (System.Action) (() => this.StartTutorialADV(4))
        }
      };
      this.NormalCommandOptionInfos = new CommCommandList.CommandInfo[10]
      {
        new CommCommandList.CommandInfo("トーク")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.TalkCommandOptionInfos, "トーク", (System.Action) (() => this.BackCommandList(this.NormalCommandOptionInfos, this.CharaName, (System.Action) null))))
        },
        new CommCommandList.CommandInfo("アドバイスする")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.InstructionCommandOptionInfos, "アドバイス", (System.Action) (() => this.BackCommandList(this.NormalCommandOptionInfos, this.CharaName, (System.Action) null))))
        },
        new CommCommandList.CommandInfo("アイテムを渡す")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.PresentADV(this.NormalCommandOptionInfos))
        },
        new CommCommandList.CommandInfo("頼まれもの")
        {
          Condition = (Func<bool>) (() => this.AgentData.ItemScrounge.isEvent),
          IsSpecial = true,
          Timer = (Func<float>) (() => this.AgentData.ItemScrounge.remainingTime),
          Event = (System.Action<int>) (x => this.StartScroungeEvent(this.NormalCommandOptionInfos))
        },
        new CommCommandList.CommandInfo("薬をあげる")
        {
          Condition = (Func<bool>) (() => this.AgentData.SickState.ID != 0),
          Event = (System.Action<int>) (x => this.StartSicknessEvent(0))
        },
        new CommCommandList.CommandInfo("薬をあげる")
        {
          Condition = (Func<bool>) (() => this.AgentData.SickState.ID == 0),
          Event = (System.Action<int>) (x => this.StartSicknessEvent(0))
        },
        new CommCommandList.CommandInfo("ついてきて")
        {
          Condition = (Func<bool>) (() => !this.IsBadMood()),
          Event = (System.Action<int>) (x =>
          {
            this.EndCommonSelection();
            this.Animation.StopAllAnimCoroutine();
            this.openData.FindLoad("5", this.charaID, 0);
            this.packData.onComplete = (System.Action) (() =>
            {
              if (this.packData.isSuccessFollow)
              {
                this.ResetActionFlag();
                if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
                {
                  this.Animation.CrossFadeScreen(-1f);
                  this.CurrentPoint.SetActiveMapItemObjs(true);
                  this.CurrentPoint.CreateByproduct(this.ActionID, this.PoseID);
                  this.CurrentPoint.ReleaseSlot((Actor) this);
                  this.CurrentPoint = (ActionPoint) null;
                  this.ActivateNavMeshAgent();
                }
                this.ActivatePairing(x, true);
                bool flag1 = this.ChaControl.fileGameInfo.phase >= 2;
                bool flag2 = this.ChaControl.fileGameInfo.flavorState[1] >= Singleton<Resources>.Instance.StatusProfile.HoldingHandBorderReliability;
                if (flag1 && flag2)
                  this.ActivateHoldingHands(x, true);
                this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                this.EndCommunication();
              }
              else
                this.CheckTalkForceEnd(this.NormalCommandOptionInfos);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        },
        new CommCommandList.CommandInfo("エッチがしたい")
        {
          Condition = (Func<bool>) (() =>
          {
            ChaControl chaControl = Singleton<Manager.Map>.Instance.Player.ChaControl;
            return chaControl.sex == (byte) 1 && !chaControl.fileParam.futanari ? this.CanSelectHCommand(Singleton<Resources>.Instance.DefinePack.MapDefines.LesTypeHMeshTag) && !this.IsBadMood() : this.CanSelectHCommand() && !this.IsBadMood();
          }),
          Event = (System.Action<int>) (x => this.InviteH(this.NormalCommandOptionInfos))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            ChaControl chaControl = Singleton<Manager.Map>.Instance.Player.ChaControl;
            return (chaControl.sex != (byte) 1 || chaControl.fileParam.futanari) && this.CanSelectHCommand() && this.IsBadMood();
          }),
          Event = (System.Action<int>) (x =>
          {
            Singleton<HSceneManager>.Instance.isForce = true;
            this.InviteH(this.NormalCommandOptionInfos);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.Left("0"))
        }
      };
      CommCommandList.CommandInfo[] commandInfoArray1 = new CommCommandList.CommandInfo[9]
      {
        new CommCommandList.CommandInfo("印象を聞く")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartTalk("0", string.Empty))
        },
        new CommCommandList.CommandInfo("調子どう？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartTalk("1", string.Empty))
        },
        new CommCommandList.CommandInfo("誰と仲良し？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            this.EndCommonSelection();
            this.Animation.StopAllAnimCoroutine();
            this.openData.FindLoad("2", this.charaID, 0);
            this.packData.onComplete = (System.Action) (() => this.CheckTalkForceEnd(this.TalkCommandOptionInfos));
            this.SearchFavoriteTargetForADV();
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        },
        new CommCommandList.CommandInfo("お気に入りの場所は？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartTalk("3", string.Empty))
        },
        new CommCommandList.CommandInfo("おだてる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.PraiseCommandOptionInfos, "おだてる", (System.Action) (() => this.BackCommandList(this.TalkCommandOptionInfos, "トーク", (System.Action) (() =>
          {
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            this.BackCommandList(player.Mode != Desire.ActionType.Date ? this.NormalCommandOptionInfos : (!Object.op_Equality((Object) player.Partner, (Object) this) ? this.DateCommandOptionInfosTP : this.DateCommandOptionInfos), this.CharaName, (System.Action) null);
          })))))
        },
        null,
        null,
        null,
        null
      };
      CommCommandList.CommandInfo commandInfo1 = new CommCommandList.CommandInfo("エッチな会話");
      CommCommandList.CommandInfo commandInfo2 = commandInfo1;
      // ISSUE: reference to a compiler-generated field
      if (AgentActor.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AgentActor.\u003C\u003Ef__mg\u0024cache0 = new Func<bool>(Game.get_isAdd01);
      }
      // ISSUE: reference to a compiler-generated field
      Func<bool> fMgCache0 = AgentActor.\u003C\u003Ef__mg\u0024cache0;
      commandInfo2.Condition = fMgCache0;
      commandInfo1.Event = (System.Action<int>) (x =>
      {
        this.EndCommonSelection();
        this.Animation.StopAllAnimCoroutine();
        this.openData.FindLoad("0", this.charaID, 9);
        this.packData.onComplete = (System.Action) (() => this.CheckTalkForceEnd(this.TalkCommandOptionInfos));
        this.packData.restoreCommands = this.TalkCommandOptionInfos;
        Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
      });
      commandInfoArray1[5] = commandInfo1;
      commandInfoArray1[6] = new CommCommandList.CommandInfo("戻る")
      {
        Condition = (Func<bool>) (() => Singleton<Manager.Map>.Instance.Player.Mode != Desire.ActionType.Date),
        Event = (System.Action<int>) (x => this.BackCommandList(this.NormalCommandOptionInfos, this.CharaName, (System.Action) null))
      };
      commandInfoArray1[7] = new CommCommandList.CommandInfo("戻る")
      {
        Condition = (Func<bool>) (() =>
        {
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          bool flag1 = player.Mode == Desire.ActionType.Date;
          bool flag2 = Object.op_Equality((Object) player.Partner, (Object) this);
          return flag1 && flag2;
        }),
        Event = (System.Action<int>) (x => this.BackCommandList(this.DateCommandOptionInfos, this.CharaName, (System.Action) null))
      };
      commandInfoArray1[8] = new CommCommandList.CommandInfo("戻る")
      {
        Condition = (Func<bool>) (() =>
        {
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          bool flag1 = player.Mode == Desire.ActionType.Date;
          bool flag2 = Object.op_Inequality((Object) player.Partner, (Object) this);
          return flag1 && flag2;
        }),
        Event = (System.Action<int>) (x => this.BackCommandList(this.DateCommandOptionInfosTP, this.CharaName, (System.Action) null))
      };
      this.TalkCommandOptionInfos = commandInfoArray1;
      this.PraiseCommandOptionInfos = new CommCommandList.CommandInfo[3]
      {
        new CommCommandList.CommandInfo("容姿についてほめる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            this.StartTalk("4", "トーク");
            MapUIContainer.CommandList.CancelEvent = (System.Action) (() =>
            {
              PlayerActor player = Singleton<Manager.Map>.Instance.Player;
              this.BackCommandList(player.Mode != Desire.ActionType.Date ? this.NormalCommandOptionInfos : (!Object.op_Equality((Object) player.Partner, (Object) this) ? this.DateCommandOptionInfosTP : this.DateCommandOptionInfos), this.CharaName, (System.Action) null);
            });
          })
        },
        new CommCommandList.CommandInfo("内面についてほめる")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            this.StartTalk("6", "トーク");
            MapUIContainer.CommandList.CancelEvent = (System.Action) (() =>
            {
              PlayerActor player = Singleton<Manager.Map>.Instance.Player;
              this.BackCommandList(player.Mode != Desire.ActionType.Date ? this.NormalCommandOptionInfos : (!Object.op_Equality((Object) player.Partner, (Object) this) ? this.DateCommandOptionInfosTP : this.DateCommandOptionInfos), this.CharaName, (System.Action) null);
            });
          })
        },
        new CommCommandList.CommandInfo("戻る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.BackCommandList(this.TalkCommandOptionInfos, "トーク", (System.Action) (() =>
          {
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            this.BackCommandList(player.Mode != Desire.ActionType.Date ? this.NormalCommandOptionInfos : (!Object.op_Equality((Object) player.Partner, (Object) this) ? this.DateCommandOptionInfosTP : this.DateCommandOptionInfos), this.CharaName, (System.Action) null);
          })))
        }
      };
      CommCommandList.CommandInfo[] commandInfoArray2 = new CommCommandList.CommandInfo[12]
      {
        new CommCommandList.CommandInfo("寝に行ったら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("7"))
        },
        new CommCommandList.CommandInfo("少し休んだら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("0"))
        },
        new CommCommandList.CommandInfo("採取手伝って")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("1"))
        },
        new CommCommandList.CommandInfo("なにか食べたら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("2"))
        },
        new CommCommandList.CommandInfo("なにか飲んだら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("6"))
        },
        new CommCommandList.CommandInfo("料理作って")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("5"))
        },
        new CommCommandList.CommandInfo("たまには気分転換を")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("3"))
        },
        new CommCommandList.CommandInfo("トイレ行ったら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("8"))
        },
        new CommCommandList.CommandInfo("風呂入ったら？")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.StartInstruction("9"))
        },
        null,
        null,
        null
      };
      CommCommandList.CommandInfo commandInfo3 = new CommCommandList.CommandInfo("エッチな行動を要求");
      CommCommandList.CommandInfo commandInfo4 = commandInfo3;
      // ISSUE: reference to a compiler-generated field
      if (AgentActor.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AgentActor.\u003C\u003Ef__mg\u0024cache1 = new Func<bool>(Game.get_isAdd01);
      }
      // ISSUE: reference to a compiler-generated field
      Func<bool> fMgCache1 = AgentActor.\u003C\u003Ef__mg\u0024cache1;
      commandInfo4.Condition = fMgCache1;
      commandInfo3.Event = (System.Action<int>) (x => this.StartInstruction("4"));
      commandInfoArray2[9] = commandInfo3;
      commandInfoArray2[10] = new CommCommandList.CommandInfo("戻る")
      {
        Condition = (Func<bool>) (() => Singleton<Manager.Map>.Instance.Player.Mode != Desire.ActionType.Date),
        Event = (System.Action<int>) (x => this.BackCommandList(this.NormalCommandOptionInfos, this.CharaName, (System.Action) null))
      };
      commandInfoArray2[11] = new CommCommandList.CommandInfo("戻る")
      {
        Condition = (Func<bool>) (() =>
        {
          PlayerActor player = Singleton<Manager.Map>.Instance.Player;
          bool flag1 = player.Mode == Desire.ActionType.Date;
          bool flag2 = Object.op_Inequality((Object) player.Partner, (Object) this);
          return flag1 && flag2;
        }),
        Event = (System.Action<int>) (x => this.BackCommandList(this.DateCommandOptionInfosTP, this.CharaName, (System.Action) null))
      };
      this.InstructionCommandOptionInfos = commandInfoArray2;
      this.DateCommandOptionInfos = new CommCommandList.CommandInfo[6]
      {
        new CommCommandList.CommandInfo("トーク")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.TalkCommandOptionInfos, "トーク", (System.Action) (() => this.BackCommandList(this.DateCommandOptionInfos, this.CharaName, (System.Action) (() => this.CancelCommunication())))))
        },
        new CommCommandList.CommandInfo("アイテムを渡す")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.PresentADV(this.DateCommandOptionInfos))
        },
        new CommCommandList.CommandInfo("解散する")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x =>
          {
            if (Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
              return;
            this.EndCommonSelection();
            this.openData.FindLoad("1", this.charaID, 5);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.EndCommunication();
              this.DeactivatePairing(x);
              this.ActivateHoldingHands(x, false);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        },
        new CommCommandList.CommandInfo("エッチがしたい")
        {
          Condition = (Func<bool>) (() =>
          {
            ChaControl chaControl = Singleton<Manager.Map>.Instance.Player.ChaControl;
            return chaControl.sex == (byte) 1 && !chaControl.fileParam.futanari ? this.CanSelectHCommand(Singleton<Resources>.Instance.DefinePack.MapDefines.LesTypeHMeshTag) && !this.IsBadMood() : this.CanSelectHCommand() && !this.IsBadMood();
          }),
          Event = (System.Action<int>) (x => this.InviteH(this.DateCommandOptionInfos))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            ChaControl chaControl = Singleton<Manager.Map>.Instance.Player.ChaControl;
            return (chaControl.sex != (byte) 1 || chaControl.fileParam.futanari) && this.CanSelectHCommand() && this.IsBadMood();
          }),
          Event = (System.Action<int>) (x =>
          {
            Singleton<HSceneManager>.Instance.isForce = true;
            this.InviteH(this.DateCommandOptionInfos);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.Left("2"))
        }
      };
      this.DateCommandOptionInfosTP = new CommCommandList.CommandInfo[8]
      {
        new CommCommandList.CommandInfo("トーク")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.TalkCommandOptionInfos, "トーク", (System.Action) (() => this.BackCommandList(this.DateCommandOptionInfosTP, this.CharaName, (System.Action) (() => this.CancelCommunication())))))
        },
        new CommCommandList.CommandInfo("アドバイスする")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.ChangeCommandList(this.InstructionCommandOptionInfos, "アドバイス", (System.Action) (() => this.BackCommandList(this.DateCommandOptionInfosTP, this.CharaName, (System.Action) (() => this.CancelCommunication())))))
        },
        new CommCommandList.CommandInfo("アイテムを渡す")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.PresentADV(this.DateCommandOptionInfosTP))
        },
        new CommCommandList.CommandInfo("頼まれもの")
        {
          Condition = (Func<bool>) (() => this.AgentData.ItemScrounge.isEvent),
          IsSpecial = true,
          Timer = (Func<float>) (() => this.AgentData.ItemScrounge.remainingTime),
          Event = (System.Action<int>) (x => this.StartScroungeEvent(this.DateCommandOptionInfosTP))
        },
        new CommCommandList.CommandInfo("薬をあげる")
        {
          Condition = (Func<bool>) (() => this.AgentData.SickState.ID != 0),
          Event = (System.Action<int>) (x => this.StartSicknessEvent(0))
        },
        new CommCommandList.CommandInfo("薬をあげる")
        {
          Condition = (Func<bool>) (() => this.AgentData.SickState.ID == 0),
          Event = (System.Action<int>) (x => this.StartSicknessEvent(0))
        },
        new CommCommandList.CommandInfo("3人でエッチがしたい")
        {
          Condition = (Func<bool>) (() =>
          {
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            ChaControl chaControl = player.ChaControl;
            return (chaControl.sex != (byte) 1 || chaControl.fileParam.futanari) && !Object.op_Equality((Object) player.AgentPartner, (Object) null) && this.CanSelectHCommand(Singleton<Resources>.Instance.DefinePack.MapDefines.FloorTypeHMeshTag) && !this.IsBadMood();
          }),
          Event = (System.Action<int>) (x =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
            this.EndCommonSelection();
            this.Animation.StopAllAnimCoroutine();
            this.openData.FindLoad("2", this.charaID, 9);
            this.packData.onComplete = (System.Action) (() =>
            {
              if (this.packData.isSuccessH)
              {
                this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                this.InitiateHScene3P(Singleton<Manager.Map>.Instance.Player.AgentPartner, this);
              }
              else
                this.CheckTalkForceEnd(this.DateCommandOptionInfosTP);
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          })
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.Left("3"))
        }
      };
      this.SpecialCommandOptionInfos = new CommCommandList.CommandInfo[16]
      {
        new CommCommandList.CommandInfo("起こす")
        {
          Condition = (Func<bool>) (() => (this.Mode == Desire.ActionType.EndTaskSleep || this.Mode == Desire.ActionType.EndTaskSleepAfterDate || this.Mode == Desire.ActionType.EndTaskSleepTogether) && !this.SleepTrigger),
          Event = (System.Action<int>) (x =>
          {
            if (Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
              return;
            this.EndCommonSelection();
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
            this.SleepTrigger = true;
            this.EndCommunication();
            this.ChangeBehavior(Desire.ActionType.WokenUp);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            if (player.ChaControl.sex == (byte) 1 && !player.ChaControl.fileParam.futanari)
              return false;
            bool flag1 = this.Mode == Desire.ActionType.EndTaskSleep || this.Mode == Desire.ActionType.EndTaskSleepAfterDate || this.Mode == Desire.ActionType.EndTaskSleepTogether;
            bool flag2 = this.HPositionID == 1;
            return flag1 && flag2 && !this.AgentData.YobaiTrigger;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Sleep);
            this.AgentData.YobaiTrigger = true;
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 2;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Bath);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 3;
          }),
          Event = (System.Action<int>) (x =>
          {
            Singleton<HSceneManager>.Instance.ReserveToilet = this.GetDesire(Desire.GetDesireKey(Desire.Type.Toilet));
            this.ClearDesire(Desire.Type.Toilet);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 4;
          }),
          Event = (System.Action<int>) (x =>
          {
            Singleton<HSceneManager>.Instance.ReserveToilet = this.GetDesire(Desire.GetDesireKey(Desire.Type.Toilet));
            this.ClearDesire(Desire.Type.Toilet);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 5;
          }),
          Event = (System.Action<int>) (x =>
          {
            Desire.Type key;
            if (Desire.ModeTable.TryGetValue(this.PrevMode, out key))
              this.SetDesire(Desire.GetDesireKey(key), 0.0f);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 6;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Hunt);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 7;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Cook);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 8;
          }),
          Event = (System.Action<int>) (x => this.GotoHScene(this.HPositionID))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 9;
          }),
          Event = (System.Action<int>) (x => this.GotoHScene(this.HPositionID))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 10;
          }),
          Event = (System.Action<int>) (x => this.GotoHScene(this.HPositionID))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 13;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Bath);
            this.GotoHScene(this.HPositionID);
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 14;
          }),
          Event = (System.Action<int>) (x =>
          {
            this.ClearDesire(Desire.Type.Game);
            float num = -this.Animation.GetAngleFromForward(((Component) this.Controller).get_transform().get_forward());
            Vector3 position1 = this.Position;
            position1.y = (__Null) 0.0;
            Vector3 position2 = Singleton<Manager.Map>.Instance.Player.Position;
            position2.y = (__Null) 0.0;
            if ((double) Mathf.Abs(this.Animation.GetAngleFromForward(Vector3.Normalize(Vector3.op_Subtraction(position2, position1))) - num) > 90.0)
            {
              Debug.Log((object) "壁穴後ろから");
              this.GotoHScene(this.HPositionID);
            }
            else
            {
              Debug.Log((object) "壁穴前から");
              this.GotoHScene(this.HPositionSubID);
            }
          })
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 16;
          }),
          Event = (System.Action<int>) (x => this.GotoHScene(this.HPositionID))
        },
        new CommCommandList.CommandInfo("エッチなことをする")
        {
          Condition = (Func<bool>) (() =>
          {
            if (!Singleton<Manager.Map>.IsInstance())
              return false;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            return (player.ChaControl.sex != (byte) 1 || player.ChaControl.fileParam.futanari) && this.HPositionID == 17;
          }),
          Event = (System.Action<int>) (x => this.GotoHScene(this.HPositionID))
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.Left("4"))
        }
      };
      this.ColdCommandOptionInfos = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("薬をあげる")
        {
          Condition = (Func<bool>) null,
          IsSpecial = true,
          Event = (System.Action<int>) (x => this.StartSicknessEvent(1))
        },
        new CommCommandList.CommandInfo("立ち去る")
        {
          Condition = (Func<bool>) null,
          Event = (System.Action<int>) (x => this.Left("10"))
        }
      };
    }

    public void StartCommunication()
    {
      this.StopNavMeshAgent();
      if (this.Mode != Desire.ActionType.Encounter)
      {
        this._originAvoidancePriority = this._navMeshAgent.get_avoidancePriority();
        this.ChangeStaticNavMeshAgentAvoidance();
      }
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.NavMeshAgent.set_velocity(Vector3.get_zero());
      player.CommCompanion = (Actor) this;
      player.Controller.ChangeState("Communication");
      this.packData.AttitudeID = this.AttitudeID;
      switch (this.AttitudeID)
      {
        case 0:
          if (this.UseNeckLook)
          {
            this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
            this.ChaControl.ChangeLookEyesPtn(1);
            this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            this.ChaControl.ChangeLookNeckPtn(3, 1f);
          }
          PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
          PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
          AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
          this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
          this.Animation.PlayTurnAnimation(player.Position, 1f, playState.MainStateInfo.InStateInfo, false);
          this.SetActiveOnEquipedItem(false);
          break;
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
          if (this.UseNeckLook)
          {
            this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
            this.ChaControl.ChangeLookEyesPtn(1);
            this.ChaControl.ChangeLookNeckTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 0.8f);
            this.ChaControl.ChangeLookNeckPtn(1, 1f);
          }
          this._schedule.progress = false;
          break;
      }
      this.DisableBehavior();
      Debug.Log((object) string.Format("StartCommunication.DisableBehavior proc frame: {0}", (object) Time.get_frameCount()));
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryUpdate(), 5), (System.Action<M0>) (_ =>
      {
        switch (this.AttitudeID)
        {
          case 0:
            if (this.UseNeckLook)
            {
              this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
              this.ChaControl.ChangeLookEyesPtn(1);
              this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
              this.ChaControl.ChangeLookNeckPtn(3, 1f);
              break;
            }
            break;
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
            if (this.UseNeckLook)
            {
              this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
              this.ChaControl.ChangeLookEyesPtn(1);
              this.ChaControl.ChangeLookNeckTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 0.8f);
              this.ChaControl.ChangeLookNeckPtn(1, 1f);
            }
            this._schedule.progress = false;
            break;
        }
        this.BehaviorResources.Current.DisableBehavior(true);
      }));
      if (Object.op_Inequality((Object) this.Partner, (Object) null) && this.Partner is AgentActor)
      {
        AgentActor partner = this.Partner as AgentActor;
        if (this.AttitudeID != 0)
          partner.DisableBehavior();
      }
      switch (this.AttitudeID)
      {
        case 0:
          Manager.ADV.ChangeADVCamera((Actor) this);
          break;
        case 1:
          Manager.ADV.ChangeADVCameraDiagonal((Actor) this);
          break;
        default:
          Manager.ADV.ChangeADVFixedAngleCamera((Actor) this, this.AttitudeID);
          break;
      }
      player.CameraControl.VanishControl.LoadHousingVanish(player);
      player.CameraControl.VanishControl.Load();
      MapUIContainer.SetVisibleHUD(false);
      Singleton<Manager.ADV>.Instance.TargetCharacter = this;
    }

    private void StartDateCommunication()
    {
      this.StopNavMeshAgent();
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CommCompanion = (Actor) this;
      player.Controller.ChangeState("Communication");
      if (this.UseNeckLook)
      {
        this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
        this.ChaControl.ChangeLookEyesPtn(1);
        this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
        this.ChaControl.ChangeLookNeckPtn(3, 1f);
      }
      this.packData.AttitudeID = this.AttitudeID;
      switch (this.AttitudeID)
      {
        case 0:
          PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
          PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
          AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
          this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
          this.Animation.PlayTurnAnimation(player.Position, 1f, playState.MainStateInfo.InStateInfo, false);
          bool enabled = ((Behaviour) player.HandsHolder).get_enabled();
          player.OldEnabledHoldingHand = enabled;
          if (enabled)
          {
            ((Behaviour) player.HandsHolder).set_enabled(false);
            if (player.HandsHolder.EnabledHolding)
              player.HandsHolder.EnabledHolding = false;
          }
          this.SetActiveOnEquipedItem(false);
          break;
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
          this._schedule.progress = false;
          break;
      }
      this.DisableBehavior();
      Manager.ADV instance = Singleton<Manager.ADV>.Instance;
      Manager.ADV.ChangeADVCamera((Actor) this);
      MapUIContainer.SetVisibleHUD(false);
      instance.TargetCharacter = this;
    }

    private void EndCommunication()
    {
      MapUIContainer.SetActiveCommandList(false);
      this.VanishCommands();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (player.OldEnabledHoldingHand)
      {
        ((Behaviour) player.HandsHolder).set_enabled(true);
        player.OldEnabledHoldingHand = false;
      }
      Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RefreshCommands();
      this.packData.Release();
    }

    private void CancelCommunication()
    {
      this.EndCommonSelection();
      MapUIContainer.SetActiveCommandList(false);
      this.VanishCommands();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (player.OldEnabledHoldingHand)
      {
        ((Behaviour) player.HandsHolder).set_enabled(true);
        player.OldEnabledHoldingHand = false;
      }
      Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RefreshCommands();
      this.packData.Release();
    }

    private void StartTalk(string asset, string titleOnBack = "")
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      this.EndCommonSelection();
      this.Animation.StopAllAnimCoroutine();
      this.openData.FindLoad(asset, this.charaID, 0);
      this.packData.onComplete = (System.Action) (() =>
      {
        if (!titleOnBack.IsNullOrEmpty())
          MapUIContainer.CommandList.Label.set_text(titleOnBack);
        this.CheckTalkForceEnd(this.TalkCommandOptionInfos);
      });
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    private void StartInstruction(string asset)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      this.EndCommonSelection();
      this.Animation.StopAllAnimCoroutine();
      this.openData.FindLoad(asset, this.charaID, 1);
      this.packData.onComplete = (System.Action) (() => this.CheckTalkForceEnd(this.InstructionCommandOptionInfos));
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    private void InviteH(CommCommandList.CommandInfo[] restoreCommands)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      this.EndCommonSelection();
      this.Animation.StopAllAnimCoroutine();
      this.openData.FindLoad("1", this.charaID, 9);
      this.packData.onComplete = (System.Action) (() =>
      {
        if (this.packData.isSuccessH)
        {
          if (this.Mode == Desire.ActionType.Encounter)
          {
            Desire.Type type;
            if (Desire.ModeTable.TryGetValue(this.PrevMode, out type))
              this.ClearDesire(type);
          }
          else
          {
            Desire.Type type;
            if (Desire.ModeTable.TryGetValue(this.Mode, out type))
              this.ClearDesire(type);
          }
          this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
          this.InitiateHScene(HSceneManager.HEvent.Normal);
        }
        else
          this.CheckTalkForceEnd(restoreCommands);
      });
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    private void GotoHScene(int hPosID)
    {
      this.EndCommonSelection();
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
      MapUIContainer.SetActiveCommandList(false);
      Singleton<HSceneManager>.Instance.isForce = true;
      this.InitiateHScene((HSceneManager.HEvent) hPosID);
    }

    private void Left(string asset)
    {
      if (Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
        return;
      this.EndCommonSelection();
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      this.openData.FindLoad(asset, this.charaID, 5);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.EndCommunication();
      });
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    private void ChangeCommandList(
      CommCommandList.CommandInfo[] commands,
      string title,
      System.Action onCancel)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      MapUIContainer.CommandList.Refresh(commands, (System.Action) null);
      MapUIContainer.CommandList.Label.set_text(title);
      MapUIContainer.CommandList.CancelEvent = onCancel;
    }

    private void BackCommandList(
      CommCommandList.CommandInfo[] commands,
      string title,
      System.Action onCancel)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      MapUIContainer.CommandList.Refresh(commands, (System.Action) null);
      MapUIContainer.CommandList.Label.set_text(title);
      MapUIContainer.CommandList.CancelEvent = onCancel;
    }

    private void CheckTalkForceEnd(CommCommandList.CommandInfo[] restoreCommands)
    {
      if ((double) this.AgentData.TalkMotivation <= 0.0)
      {
        Debug.Log((object) "会話のやる気が0になった");
        this.AgentData.LockTalk = true;
        this.AgentData.TalkElapsedTime = 0.0f;
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.EndCommunication();
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        if (player.Mode != Desire.ActionType.Date || !Object.op_Equality((Object) player.Partner, (Object) this))
          return;
        this.DeactivatePairing(0);
        this.ActivateHoldingHands(0, false);
      }
      else
      {
        this.packData.restoreCommands = restoreCommands;
        this.packData.OnEndRefreshCommand = (System.Action) (() =>
        {
          this.StartCommonSelection();
          this.packData.OnEndRefreshCommand = (System.Action) null;
        });
      }
    }

    private void StartEvent()
    {
      this.IsEvent = true;
    }

    private bool CheckStealEvent()
    {
      PoseKeyPair snitchFoodId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SnitchFoodID;
      return this.ActionID == snitchFoodId.postureID && this.PoseID == snitchFoodId.poseID;
    }

    private void StartStealEvent()
    {
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      Desire.ActionType mode = this.BehaviorResources.Mode;
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 20), (System.Action<M0>) (_ => this.StopNavMeshAgent()));
      this.StopNavMeshAgent();
      this.Animation.CrossFadeScreen(-1f);
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CommCompanion = (Actor) this;
      player.PlayerController.ChangeState("Idle");
      PoseKeyPair foundID = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.StealFoundID;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.StealAnimCoroutine(foundID.postureID, foundID.poseID)), false), (System.Action<M0>) (_ =>
      {
        player.PlayerController.ChangeState("Communication");
        this.Animation.CrossFadeScreen(-1f);
        this.Animation.StopAllAnimCoroutine();
        if (this.UseNeckLook)
        {
          this.ChaControl.ChangeLookEyesTarget(1, ((Component) player.CameraControl.CameraComponent).get_transform(), 0.5f, 0.0f, 1f, 2f);
          this.ChaControl.ChangeLookEyesPtn(1);
          this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
          this.ChaControl.ChangeLookNeckPtn(3, 1f);
        }
        PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
        PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
        AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
        this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        this.Animation.PlayTurnAnimation(player.Position, 1f, playState.MainStateInfo.InStateInfo, false);
        Manager.ADV.ChangeADVCamera((Actor) this);
        player.CameraControl.VanishControl.LoadHousingVanish(player);
        player.CameraControl.VanishControl.Load();
        this.openData.FindLoad("2", this.charaID, 7);
        this.packData.onComplete = (System.Action) (() =>
        {
          this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
          this.SetLookPtn(0, 3);
          this.SetLookTarget(0, 0, (Transform) null);
          this.Animation.StopAllAnimCoroutine();
          this.Animation.ResetDefaultAnimatorController();
          if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
          {
            this.CurrentPoint.SetActiveMapItemObjs(true);
            this.CurrentPoint.ReleaseSlot((Actor) this);
            this.CurrentPoint = (ActionPoint) null;
          }
          if (this.AgentData.CarryingItem != null)
          {
            this.ApplyFoodParameter(this.AgentData.CarryingItem);
            this.AgentData.CarryingItem = (StuffItem) null;
          }
          this.ChangeBehavior(Desire.ActionType.Normal);
          player.CameraControl.Mode = CameraMode.Normal;
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
          this.packData.Release();
        });
        Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
      }));
    }

    private bool CheckCatEvent()
    {
      if (this.AttitudeID != 0 || this.AgentData.CheckCatEvent || this.PrevActionMode != Desire.ActionType.EndTaskWildAnimal)
        return false;
      int num1 = Random.Range(0, 100);
      float num2 = Singleton<Resources>.Instance.AgentProfile.CatEventBaseProb + (float) Mathf.RoundToInt(AgentActor.FlavorVariation(Singleton<Resources>.Instance.StatusProfile.FlavorCatCaptureMinMax, Singleton<Resources>.Instance.StatusProfile.FlavorCatCaptureRate, this.ChaControl.fileGameInfo.flavorState[3]));
      if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(19))
        num2 += (float) Singleton<Resources>.Instance.StatusProfile.CatCaptureProbBuff;
      this.AgentData.CheckCatEvent = true;
      if ((double) num1 < (double) num2)
      {
        Debug.Log((object) "猫イベント発生");
        return true;
      }
      Debug.Log((object) "猫イベント失敗");
      return false;
    }

    private void StartCatEvent()
    {
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      Desire.ActionType prevMode = this.BehaviorResources.Mode;
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      player.PlayerController.ChangeState("Idle");
      this.openData.FindLoad("1", this.charaID, 7);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.SetLookPtn(0, 3);
        this.SetLookTarget(0, 0, (Transform) null);
        if (this.AttitudeID == 0)
        {
          this.Animation.StopAllAnimCoroutine();
          this.Animation.ResetDefaultAnimatorController();
        }
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        player.CameraControl.Mode = CameraMode.Normal;
        player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Singleton<Manager.Map>.Instance.EnableEntity((Actor) this);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          this.BehaviorResources.ChangeMode(prevMode);
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
          this.packData.Release();
        }));
      });
      this.packData.Init();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (MapUIContainer.FadeCanvas.IsFading)
        MapUIContainer.FadeCanvas.SkipFade();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.DisableEntity((Actor) this);
        Vector3 position1 = this.Position;
        position1.y = (__Null) 0.0;
        Vector3 position2 = player.Position;
        position2.y = (__Null) 0.0;
        this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1));
        player.CommCompanion = (Actor) this;
        player.PlayerController.ChangeState("Communication");
        PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
        AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID].MainStateInfo.AssetBundleInfo;
        this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        player.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2));
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        Manager.ADV.ChangeADVCamera((Actor) this);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Transform transform = ((Component) player.CameraControl.CameraComponent).get_transform();
        this.SetLookPtn(1, 3);
        this.SetLookTarget(1, 0, transform);
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))));
      }));
    }

    private bool CheckFishingEvent()
    {
      if (this.IsBadMood() || this.ActionID != 6)
        return false;
      return ((IEnumerable<int>) new int[3]
      {
        6,
        7,
        8
      }).Contains<int>(this.PoseID);
    }

    private void FishingEvent()
    {
      this.packData.Init();
      this.SetDesire(Desire.GetDesireKey(this.RequestedDesire), 0.0f);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.PlayerController.ChangeState("Idle");
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.DisableBehavior();
      this.DeactivateNavMeshAgent();
      this.openData.FindLoad("0", this.charaID, 7);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.SetLookPtn(0, 3);
        this.SetLookTarget(0, 0, (Transform) null);
        this.ResetActionFlag();
        this.Animation.StopAllAnimCoroutine();
        this.Animation.ResetDefaultAnimatorController();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        player.CameraControl.Mode = CameraMode.Normal;
        player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        if (this.AgentData.CarryingItem != null)
          this.AgentData.CarryingItem = (StuffItem) null;
        this.EnableBehavior();
        this.ActivateNavMeshAgent();
        Singleton<Manager.Map>.Instance.EnableEntity((Actor) this);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          this.ChangeBehavior(Desire.ActionType.Normal);
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
          this.packData.Release();
        }));
      });
      this.packData.Init();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (MapUIContainer.FadeCanvas.IsFading)
        MapUIContainer.FadeCanvas.SkipFade();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.DisableEntity((Actor) this);
        Vector3 position1 = this.Position;
        position1.y = (__Null) 0.0;
        Vector3 position2 = player.Position;
        position2.y = (__Null) 0.0;
        this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1));
        player.CommCompanion = (Actor) this;
        player.PlayerController.ChangeState("Communication");
        PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
        AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID].MainStateInfo.AssetBundleInfo;
        this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        player.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2));
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        Manager.ADV.ChangeADVCamera((Actor) this);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Transform transform = ((Component) player.CameraControl.CameraComponent).get_transform();
        this.SetLookPtn(1, 3);
        this.SetLookTarget(1, 0, transform);
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))));
      }));
    }

    private void OpenDataFindLoadEvent(string fileName)
    {
      int category = 6;
      this.AgentData.advEventLimitation.Add(category);
      this.AgentData.GetAdvEventCheck(category).Add(fileName);
      this.openData.FindLoad(fileName, this.charaID, category);
    }

    public void AdvEventStart()
    {
      this.AdvEventStart(this.advEventName);
    }

    private void AdvEventStart(string fileName)
    {
      this.packData.Init();
      this.SetDesire(Desire.GetDesireKey(this.RequestedDesire), 0.0f);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.PlayerController.ChangeState("Idle");
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.DisableBehavior();
      this.DeactivateNavMeshAgent();
      this.OpenDataFindLoadEvent(fileName);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.SetLookPtn(0, 3);
        this.SetLookTarget(0, 0, (Transform) null);
        this.ResetActionFlag();
        this.Animation.StopAllAnimCoroutine();
        this.Animation.ResetDefaultAnimatorController();
        if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          this.CurrentPoint.SetActiveMapItemObjs(true);
          this.CurrentPoint.ReleaseSlot((Actor) this);
          this.CurrentPoint = (ActionPoint) null;
        }
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        player.CameraControl.Mode = CameraMode.Normal;
        player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        if (this.AgentData.CarryingItem != null)
          this.AgentData.CarryingItem = (StuffItem) null;
        this.EnableBehavior();
        this.ActivateNavMeshAgent();
        Singleton<Manager.Map>.Instance.EnableEntity((Actor) this);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          this.ChangeBehavior(Desire.ActionType.Normal);
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
          this.packData.Release();
        }));
      });
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (MapUIContainer.FadeCanvas.IsFading)
        MapUIContainer.FadeCanvas.SkipFade();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.DisableEntity((Actor) this);
        int? expansionId = this.advEventParam?.ExpansionID;
        if (expansionId.HasValue && expansionId.Value == 2 && Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          EventPivot component = (EventPivot) ((Component) this.CurrentPoint).GetComponent<EventPivot>();
          if (Object.op_Inequality((Object) component, (Object) null) && Object.op_Inequality((Object) component.PivotTransform, (Object) null))
          {
            Transform pivotTransform = component.PivotTransform;
            this.Position = pivotTransform.get_position();
            this.Rotation = pivotTransform.get_rotation();
          }
        }
        AgentAdvEventInfo.EventPosition eventPos = this.advEventParam?.EventPos;
        Dictionary<int, Transform> dictionary;
        Transform transform1;
        if (eventPos != null && eventPos.isOrder && (Singleton<Manager.Map>.Instance.EventStartPointDic.TryGetValue(eventPos.mapID, out dictionary) && dictionary.TryGetValue(eventPos.ID, out transform1)))
        {
          this.Position = transform1.get_position();
          this.Rotation = transform1.get_rotation();
        }
        bool? lookPlayer = this.advEventParam?.LookPlayer;
        if ((!lookPlayer.HasValue ? 1 : (lookPlayer.Value ? 1 : 0)) != 0)
        {
          Vector3 position1 = this.Position;
          position1.y = (__Null) 0.0;
          Vector3 position2 = player.Position;
          position2.y = (__Null) 0.0;
          this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1));
          player.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2));
        }
        player.CommCompanion = (Actor) this;
        player.PlayerController.ChangeState("Communication");
        this.SetActiveOnEquipedItem(false);
        PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
        AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID].MainStateInfo.AssetBundleInfo;
        this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        Manager.ADV.ChangeADVCamera((Actor) this);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Transform transform2 = ((Component) player.CameraControl.CameraComponent).get_transform();
        this.SetLookPtn(1, 3);
        this.SetLookTarget(1, 0, transform2);
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))));
      }));
    }

    public void AdvEventStart_SleepingPlayer(PlayerActor player)
    {
      this.packData.Init();
      this.SetDesire(Desire.GetDesireKey(this.RequestedDesire), 0.0f);
      this.Animation.StopAllAnimCoroutine();
      this.SetActiveOnEquipedItem(false);
      this.ChaControl.setAllLayerWeight(0.0f);
      player.PlayerController.ChangeState("Idle");
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.DisableBehavior();
      this.DeactivateNavMeshAgent();
      MapUIContainer.SetVisibleHUD(false);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      \u003C\u003E__AnonType17<Vector3, Quaternion> bkData = new \u003C\u003E__AnonType17<Vector3, Quaternion>(this.Position, this.Rotation);
      this.Position = player.Position;
      this.Rotation = player.Rotation;
      PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
      AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID].MainStateInfo.AssetBundleInfo;
      this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
      Transform transform = ((Component) player.CameraControl.CameraComponent).get_transform();
      this.SetLookPtn(1, 3);
      this.SetLookTarget(1, 0, transform);
      this.OpenDataFindLoadEvent(this.advEventName);
      System.Action cameraBlendStyleChange = (System.Action) (() =>
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
      });
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.SetLookPtn(0, 3);
        this.SetLookTarget(0, 0, (Transform) null);
        this.ResetActionFlag();
        this.Animation.StopAllAnimCoroutine();
        this.Animation.ResetDefaultAnimatorController();
        player.CameraControl.Mode = CameraMode.Normal;
        player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView;
        cameraBlendStyleChange();
        this.Position = bkData.Position;
        this.Rotation = bkData.Rotation;
        if (this.AgentData.CarryingItem != null)
          this.AgentData.CarryingItem = (StuffItem) null;
        this.EnableBehavior();
        this.ActivateNavMeshAgent();
        this.packData.Release();
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          Singleton<Manager.Map>.Instance.EnableEntity((Actor) this);
          this.ChangeBehavior(Desire.ActionType.Normal);
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
        }));
      });
      Manager.ADV.ChangeADVCamera((Actor) this);
      cameraBlendStyleChange();
      Singleton<Manager.Map>.Instance.DisableEntity((Actor) this);
      ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))));
    }

    private void StartPhaseEvent()
    {
      this.StopNavMeshAgent();
      if (this.Mode != Desire.ActionType.Encounter)
      {
        this._originAvoidancePriority = this._navMeshAgent.get_avoidancePriority();
        this.ChangeStaticNavMeshAgentAvoidance();
      }
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      Desire.ActionType prevMode = this.BehaviorResources.Mode;
      this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      player.CommCompanion = (Actor) this;
      player.PlayerController.ChangeState("Idle");
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      this.openData.FindLoad(string.Format("{0}", (object) (this.ChaControl.fileGameInfo.phase + 1)), this.AgentData.param.charaID, 6);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        this.packData.Release();
        this.SetLookPtn(0, 3);
        this.SetLookTarget(0, 0, (Transform) null);
        if (this.AttitudeID == 0)
        {
          this.Animation.StopAllAnimCoroutine();
          this.Animation.ResetDefaultAnimatorController();
        }
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        player.CameraControl.Mode = CameraMode.Normal;
        player.ChaControl.visibleAll = player.CameraControl.ShotType != ShotType.PointOfView;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        this.SetPhase(this.ChaControl.fileGameInfo.phase + 1);
        Singleton<Manager.Map>.Instance.EnableEntity((Actor) this);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          if (this.Mode != Desire.ActionType.Encounter)
            this.RecoverNavMeshAgentAvoidance();
          this.BehaviorResources.ChangeMode(prevMode);
          player.PlayerController.ChangeState("Normal");
          MapUIContainer.SetVisibleHUD(true);
          MapUIContainer.StorySupportUI.Open();
          this.packData.Release();
        }));
      });
      if (MapUIContainer.FadeCanvas.IsFading)
        MapUIContainer.FadeCanvas.SkipFade();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.DisableEntity((Actor) this);
        player.CommCompanion = (Actor) this;
        player.PlayerController.ChangeState("Communication");
        this.SetActiveOnEquipedItem(false);
        Vector3 position1 = this.Position;
        position1.y = (__Null) 0.0;
        Vector3 position2 = player.Position;
        position2.y = (__Null) 0.0;
        this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1));
        PoseKeyPair poseKeyPair = Singleton<Resources>.Instance.AgentProfile.ADVIdleTable[this.ChaControl.fileParam.personality];
        AssetBundleInfo assetBundleInfo = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID].MainStateInfo.AssetBundleInfo;
        this.ChangeAnimator((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset);
        player.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2));
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        Manager.ADV.ChangeADVCamera((Actor) this);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Transform transform = ((Component) player.CameraControl.CameraComponent).get_transform();
        this.SetLookPtn(1, 3);
        this.SetLookTarget(1, 0, transform);
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))));
      }));
    }

    private void StartSicknessEvent(int id)
    {
      this.EndCommonSelection();
      MapUIContainer.CommandList.Visibled = false;
      MapUIContainer.ReserveSystemMenuMode(SystemMenuUI.MenuMode.InventoryEnter);
      SystemMenuUI systemUI = MapUIContainer.SystemMenuUI;
      InventoryUIController inventoryUI = systemUI.InventoryEnterUI;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      Dictionary<int, List<int>> dictionary = DictionaryPool<int, List<int>>.Get();
      int id1 = this.AgentData.SickState.ID;
      foreach (ItemIDKeyPair itemIdKeyPair in id1 != 0 || this.AgentData.SickState.UsedMedicine || this.AgentData.SickState.Enabled ? (id1 != 4 || this.AgentData.SickState.Enabled ? (id1 != 1 ? (id1 != 3 ? agentProfile.MedicineNormalItemList : agentProfile.MedicineHeatStrokeItemList) : agentProfile.MedicineStomachacheItemList) : agentProfile.MedicineHurtItemList) : agentProfile.MedicineColdItemList)
      {
        List<int> intList1;
        if (!dictionary.TryGetValue(itemIdKeyPair.categoryID, out intList1))
        {
          List<int> intList2 = ListPool<int>.Get();
          dictionary[itemIdKeyPair.categoryID] = intList2;
          intList1 = intList2;
        }
        intList1.Add(itemIdKeyPair.itemID);
      }
      inventoryUI.isConfirm = true;
      inventoryUI.CountViewerVisible(false);
      inventoryUI.EmptyTextAutoVisible(true);
      InventoryFacadeViewer.ItemFilter[] array = dictionary.Select<KeyValuePair<int, List<int>>, InventoryFacadeViewer.ItemFilter>((Func<KeyValuePair<int, List<int>>, InventoryFacadeViewer.ItemFilter>) (x => new InventoryFacadeViewer.ItemFilter(x.Key, x.Value.ToArray()))).ToArray<InventoryFacadeViewer.ItemFilter>();
      inventoryUI.SetItemFilter(array);
      foreach (int index in dictionary.Keys.ToArray<int>())
        ListPool<int>.Release(dictionary[index]);
      DictionaryPool<int, List<int>>.Release(dictionary);
      inventoryUI.itemList = (Func<List<StuffItem>>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList);
      inventoryUI.itemList_System = (Func<List<StuffItem>>) null;
      inventoryUI.DoubleClickAction((System.Action<InventoryFacadeViewer.DoubleClickData>) null);
      bool isVisibleCommand = true;
      inventoryUI.OnSubmit = (System.Action<StuffItem>) (item =>
      {
        if (Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null))
          Singleton<Game>.Instance.Dialog.TimeScale = 1f;
        isVisibleCommand = false;
        InventoryUIController inventoryUiController = inventoryUI;
        if (inventoryUiController != null)
        {
          System.Action onClose = inventoryUiController.OnClose;
          if (onClose != null)
            onClose();
        }
        MapUIContainer.SetActiveCommandList(false);
        Desire.ActionType prevMode = this.BehaviorResources.Mode;
        this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 20), (System.Action<M0>) (_ => this.StopNavMeshAgent()));
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        player.PlayerController.ChangeState("Idle");
        PoseKeyPair medicID = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.MedicID;
        if (item.MatchItem(agentProfile.ColdMedicineID) || item.MatchItem(agentProfile.FeverReducerID))
        {
          if (id == 0)
          {
            this.openData.FindLoad("0", this.charaID, 11);
            this.packData.onComplete = (System.Action) (() =>
            {
              this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
              this.SetLookPtn(0, 3);
              this.SetLookTarget(0, 0, (Transform) null);
              if (this.AttitudeID == 0)
              {
                this.Animation.StopAllAnimCoroutine();
                this.Animation.ResetDefaultAnimatorController();
              }
              this.ClearItems();
              this.ClearParticles();
              if (item.MatchItem(agentProfile.ColdMedicineID))
              {
                this.AgentData.SickState.ID = -1;
                this.SetStatus(0, 50f);
                this.AgentData.ColdLockInfo.Lock = true;
                this.ApplySituationResultParameter(0);
              }
              else if (item.MatchItem(agentProfile.FeverReducerID))
              {
                this.AgentData.SickState.UsedMedicine = true;
                if (Random.Range(0, 100) < 30)
                {
                  this.AgentData.SickState.ID = -1;
                  this.SetStatus(0, 50f);
                  this.AgentData.ColdLockInfo.Lock = true;
                  this.ApplySituationResultParameter(0);
                }
              }
              this.EnableBehavior();
              this.BehaviorResources.ChangeMode(prevMode);
              if ((double) this.AgentData.TalkMotivation <= 0.0)
              {
                Debug.Log((object) "会話のやる気が0になった");
                this.AgentData.LockTalk = true;
                this.AgentData.TalkElapsedTime = 0.0f;
                if (player.Mode == Desire.ActionType.Date && Object.op_Equality((Object) player.Partner, (Object) this))
                {
                  this.DeactivatePairing(0);
                  this.ActivateHoldingHands(0, false);
                }
              }
              player.CameraControl.Mode = CameraMode.Normal;
              player.CameraControl.VanishControl.VisibleForceVanish(true);
              player.CameraControl.VanishControl.ResetVanish();
              if (Singleton<Manager.Housing>.IsInstance())
                Singleton<Manager.Housing>.Instance.EndShield();
              player.PlayerController.ChangeState("Normal");
              MapUIContainer.SetVisibleHUD(true);
              this.packData.Release();
            });
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          }
          else
          {
            if (id != 1)
              return;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
            Manager.ADV.ChangeADVFixedAngleCamera((Actor) this, 2);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.AnimCoroutine(medicID.postureID, medicID.poseID)), false), (System.Action<M0>) (_ =>
            {
              if (item.MatchItem(agentProfile.ColdMedicineID))
              {
                this.AgentData.SickState.Enabled = true;
                this.AgentData.SickState.UsedMedicine = true;
                if (this.Mode == Desire.ActionType.Cold2B)
                  this.openData.FindLoad("1", this.charaID, 11);
                else if (this.Mode == Desire.ActionType.Cold3B)
                  this.openData.FindLoad("2", this.charaID, 11);
              }
              else if (item.MatchItem(agentProfile.FeverReducerID))
              {
                if (Random.Range(0, 100) < 30)
                  this.AgentData.SickState.Enabled = true;
                this.AgentData.SickState.UsedMedicine = true;
                if (this.Mode == Desire.ActionType.Cold2B)
                  this.openData.FindLoad("1", this.charaID, 11);
                else if (this.Mode == Desire.ActionType.Cold3B)
                  this.openData.FindLoad("2", this.charaID, 11);
              }
              this.packData.onComplete = (System.Action) (() =>
              {
                this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
                this.SetLookPtn(0, 3);
                this.SetLookTarget(0, 0, (Transform) null);
                this.ClearItems();
                this.ClearParticles();
                this.EnableBehavior();
                if (this.Mode == Desire.ActionType.Cold2B)
                {
                  this.BehaviorResources.ChangeMode(this.Mode);
                  if (this.AgentData.SickState.Enabled)
                  {
                    AIProject.SaveData.Sickness sickState = this.AgentData.SickState;
                    sickState.Duration = sickState.ElapsedTime + TimeSpan.FromHours(12.0);
                  }
                }
                else if (this.Mode == Desire.ActionType.Cold3B)
                {
                  this.BehaviorResources.ChangeMode(this.Mode);
                  if (this.AgentData.SickState.Enabled)
                  {
                    AIProject.SaveData.Sickness sickState = this.AgentData.SickState;
                    if (sickState.ElapsedTime.Days >= 6)
                      sickState.Duration = sickState.ElapsedTime + TimeSpan.FromHours(1.0);
                  }
                }
                player.CameraControl.Mode = CameraMode.Normal;
                player.CameraControl.VanishControl.VisibleForceVanish(true);
                player.CameraControl.VanishControl.ResetVanish();
                if (Singleton<Manager.Housing>.IsInstance())
                  Singleton<Manager.Housing>.Instance.EndShield();
                player.PlayerController.ChangeState("Normal");
                MapUIContainer.SetVisibleHUD(true);
                this.packData.Release();
              });
              Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
            }));
          }
        }
        else if (id == 0)
        {
          this.openData.FindLoad("3", this.charaID, 11);
          this.packData.onComplete = (System.Action) (() =>
          {
            StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
            if (item.ID == 1)
            {
              this.AgentData.ItemList.Add(item);
              if (this.AgentData.SickState.ID == 4)
                this.AgentData.SickState.Enabled = true;
            }
            else if (item.ID == 2)
              this.AddStatus(6, statusProfile.PotionImmoralAdd);
            else if (item.ID == 3)
              this.AddDesire(Desire.GetDesireKey(Desire.Type.Toilet), statusProfile.DiureticToiletAdd);
            else if (item.ID == 4)
              this.AddDesire(Desire.GetDesireKey(Desire.Type.Sleep), statusProfile.PillSleepAdd);
            else if (item.ID == 8)
              this.AgentData.SickState.ID = -1;
            else if (item.ID == 9)
            {
              this.AgentData.SickState.ID = -1;
              this.AgentData.HeatStrokeLockInfo.Lock = true;
              this.SetStatus(0, 50f);
            }
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            this.SetLookPtn(0, 3);
            this.SetLookTarget(0, 0, (Transform) null);
            if (this.AttitudeID == 0)
            {
              this.Animation.StopAllAnimCoroutine();
              this.Animation.ResetDefaultAnimatorController();
            }
            this.ClearItems();
            this.ClearParticles();
            this.EnableBehavior();
            this.BehaviorResources.ChangeMode(prevMode);
            if ((double) this.AgentData.TalkMotivation <= 0.0)
            {
              Debug.Log((object) "会話のやる気が0になった");
              this.AgentData.LockTalk = true;
              this.AgentData.TalkElapsedTime = 0.0f;
              if (player.Mode == Desire.ActionType.Date && Object.op_Equality((Object) player.Partner, (Object) this))
              {
                this.DeactivatePairing(0);
                this.ActivateHoldingHands(0, false);
              }
            }
            player.CameraControl.Mode = CameraMode.Normal;
            player.CameraControl.VanishControl.VisibleForceVanish(true);
            player.CameraControl.VanishControl.ResetVanish();
            if (Singleton<Manager.Housing>.IsInstance())
              Singleton<Manager.Housing>.Instance.EndShield();
            player.PlayerController.ChangeState("Normal");
            MapUIContainer.SetVisibleHUD(true);
            this.packData.Release();
          });
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
        }
        else
        {
          if (id != 1)
            return;
          this.openData.FindLoad("4", this.charaID, 11);
          this.packData.onComplete = (System.Action) (() =>
          {
            StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
            if (item.ID == 2)
              this.AddStatus(6, statusProfile.PotionImmoralAdd);
            else if (item.ID == 3)
              this.AddDesire(Desire.GetDesireKey(Desire.Type.Toilet), statusProfile.DiureticToiletAdd);
            else if (item.ID == 4)
              this.AddDesire(Desire.GetDesireKey(Desire.Type.Sleep), statusProfile.PillSleepAdd);
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            this.SetLookPtn(0, 3);
            this.SetLookTarget(0, 0, (Transform) null);
            if (this.AttitudeID == 0)
            {
              this.Animation.StopAllAnimCoroutine();
              this.Animation.ResetDefaultAnimatorController();
            }
            this.ClearItems();
            this.ClearParticles();
            this.EnableBehavior();
            this.BehaviorResources.ChangeMode(prevMode);
            if ((double) this.AgentData.TalkMotivation <= 0.0)
            {
              Debug.Log((object) "会話のやる気が0になった");
              this.AgentData.LockTalk = true;
              this.AgentData.TalkElapsedTime = 0.0f;
              if (player.Mode == Desire.ActionType.Date && Object.op_Equality((Object) player.Partner, (Object) this))
              {
                this.DeactivatePairing(0);
                this.ActivateHoldingHands(0, false);
              }
            }
            player.CameraControl.Mode = CameraMode.Normal;
            player.CameraControl.VanishControl.VisibleForceVanish(true);
            player.CameraControl.VanishControl.ResetVanish();
            if (Singleton<Manager.Housing>.IsInstance())
              Singleton<Manager.Housing>.Instance.EndShield();
            player.PlayerController.ChangeState("Normal");
            MapUIContainer.SetVisibleHUD(true);
            this.packData.Release();
          });
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
        }
      });
      inventoryUI.OnClose = (System.Action) (() =>
      {
        inventoryUI.OnSubmit = (System.Action<StuffItem>) null;
        inventoryUI.IsActiveControl = false;
        systemUI.IsActiveControl = false;
        if (isVisibleCommand)
        {
          MapUIContainer.CommandList.Visibled = true;
          this.StartCommonSelection();
        }
        Singleton<Manager.Input>.Instance.FocusLevel = 0;
        Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
        inventoryUI.OnClose = (System.Action) null;
      });
      MapUIContainer.SetActiveSystemMenuUI(true);
    }

    private void StartScroungeEvent(CommCommandList.CommandInfo[] restoreCommands)
    {
      this.EndCommonSelection();
      this.openData.FindLoad("13", this.charaID, 4);
      this.packData.onComplete = (System.Action) (() =>
      {
        MapUIContainer.CommandList.Visibled = false;
        MapUIContainer.ScroungeUI.OnClose = (System.Action) (() =>
        {
          if (!this.AgentData.ItemScrounge.isEvent)
          {
            this.packData.onComplete = (System.Action) (() =>
            {
              MapUIContainer.CommandList.Visibled = true;
              this.StartCommonSelection();
            });
            this.packData.restoreCommands = restoreCommands;
            this.openData.FindLoad("7", this.charaID, 0);
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          }
          else
          {
            MapUIContainer.CommandList.Refresh(restoreCommands, MapUIContainer.CommandList.CanvasGroup, (System.Action) null);
            MapUIContainer.CommandList.Visibled = true;
            this.StartCommonSelection();
          }
        });
        this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
        MapUIContainer.ScroungeUI.agent = this;
        MapUIContainer.SetActiveScroungeUI(true);
      });
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    [DebuggerHidden]
    private IEnumerator AnimCoroutine(int eventID, int poseID)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CAnimCoroutine\u003Ec__Iterator0()
      {
        eventID = eventID,
        poseID = poseID,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator StealAnimCoroutine(int eventID, int poseID)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CStealAnimCoroutine\u003Ec__Iterator1()
      {
        eventID = eventID,
        poseID = poseID,
        \u0024this = this
      };
    }

    public void PopupCommands(CommCommandList.CommandInfo[] infos, System.Action onOpen = null)
    {
      Debug.Log((object) nameof (PopupCommands));
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.RefreshCommands(this.ID, infos);
      MapUIContainer.CommandList.CancelEvent = (System.Action) null;
      MapUIContainer.CommandList.OnOpened = onOpen;
      MapUIContainer.SetActiveCommandList(true, this.CharaName);
      this.InitCommon();
    }

    public void PopupDateCommands(bool isFirstPerson)
    {
      Debug.Log((object) nameof (PopupDateCommands));
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      if (isFirstPerson)
        MapUIContainer.RefreshCommands(this.ID, this.DateCommandOptionInfos);
      else
        MapUIContainer.RefreshCommands(this.ID, this.DateCommandOptionInfosTP);
      MapUIContainer.CommandList.CancelEvent = (System.Action) null;
      MapUIContainer.CommandList.OnOpened = (System.Action) (() =>
      {
        this.StartCommonSelection();
        MapUIContainer.CommandList.OnOpened = (System.Action) null;
      });
      MapUIContainer.SetActiveCommandList(true, this.CharaName);
      this.InitCommon();
    }

    public void VanishCommands()
    {
      this.EndCommonSelection();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CameraControl.VanishControl.VisibleForceVanish(true);
      player.CameraControl.VanishControl.ResetVanish();
      if (Singleton<Manager.Housing>.IsInstance())
        Singleton<Manager.Housing>.Instance.EndShield();
      if (this.IsEvent)
        this.IsEvent = false;
      MapUIContainer.SetVisibleHUD(true);
      switch (player.Mode)
      {
        case Desire.ActionType.Normal:
        case Desire.ActionType.Date:
          player.CameraControl.Mode = CameraMode.Normal;
          player.Controller.ChangeState("Normal");
          break;
      }
      Singleton<Manager.Map>.Instance.Player.ChaControl.visibleAll = true;
      switch (this.AttitudeID)
      {
        case 0:
          this.Animation.StopAllAnimCoroutine();
          this.Animation.ResetDefaultAnimatorController();
          this.SetActiveOnEquipedItem(true);
          this.ActivateTransfer(false);
          this.EnableBehavior();
          break;
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
          this.EnableBehavior();
          this._schedule.progress = true;
          break;
        default:
          this.EnableBehavior();
          break;
      }
      if (this.Mode != Desire.ActionType.Encounter)
        this.RecoverNavMeshAgentAvoidance();
      this.ChaControl.ChangeLookEyesPtn(0);
      this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      this.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
    }

    public bool CanSelectHCommand()
    {
      return this.IsOnHMesh((string[]) null) && this.CanHCommand && Game.isAdd01;
    }

    public bool CanSelectHCommand(string[] tagName)
    {
      return this.IsOnHMesh(tagName) && this.CanHCommand && Game.isAdd01;
    }

    private bool IsOnHMesh(string[] tagName = null)
    {
      LayerMask hlayer = Singleton<Resources>.Instance.DefinePack.MapDefines.HLayer;
      Vector3 position = this.Position;
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y + 15.0);
      int num = Physics.SphereCastNonAlloc(position, 7.5f, Vector3.get_down(), this._hits, 25f, LayerMask.op_Implicit(hlayer));
      if (num == 0)
        return false;
      bool flag1 = true;
      for (int index = 0; index < num; ++index)
      {
        RaycastHit hit = this._hits[index];
        string tag = ((Component) ((RaycastHit) ref hit).get_collider()).get_tag();
        flag1 = tag != "Untagged";
        if (flag1)
        {
          if (!tagName.IsNullOrEmpty<string>())
          {
            bool flag2 = false;
            foreach (string str in tagName)
            {
              if (tag == str)
              {
                flag2 = true;
                break;
              }
            }
            flag1 = flag2;
          }
          if (flag1)
            break;
        }
        else
          break;
      }
      return flag1;
    }

    public void InitiateHScene(HSceneManager.HEvent hEvent = HSceneManager.HEvent.Normal)
    {
      this._recoveryActionPointFromHScene = this.CurrentPoint;
      this._recoveryModeFromHScene = this.Mode;
      this._canTalkCache = this.CanTalk;
      this._attitudeIDCache = this.AttitudeID;
      this._useNeckLookCache = this.UseNeckLook;
      this._canHCommandCache = this.CanHCommand;
      this._isSpecialCache = this.IsSpecial;
      this._hPositionIDCache = this.HPositionID;
      this._hPositionSubIDCache = this.HPositionSubID;
      this._obstacleSizeCache = this._navMeshObstacle.get_radius();
      this.AgentData.ScheduleEnabled = this._schedule.enabled;
      this.AgentData.ScheduleElapsedTime = this._schedule.elapsedTime;
      this.AgentData.ScheduleDuration = this._schedule.duration;
      if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
      {
        this.CurrentPoint.SetActiveMapItemObjs(true);
        this.CurrentPoint.ReleaseSlot((Actor) this);
        this.CurrentPoint = (ActionPoint) null;
      }
      HSceneManager instance = Singleton<HSceneManager>.Instance;
      AgentActor agentActor1 = this;
      HSceneManager.HEvent hevent = hEvent;
      AgentActor agentActor2 = agentActor1;
      int num = (int) hevent;
      instance.HsceneEnter((Actor) agentActor2, -1, (AgentActor) null, (HSceneManager.HEvent) num);
    }

    public void RecoverAction()
    {
      Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RemoveConsiderationObject((ICommandable) this._recoveryActionPointFromHScene);
      this._recoveryActionPointFromHScene.SetForceImpossible(false);
      this.CanTalk = this._canTalkCache;
      this.AttitudeID = this._attitudeIDCache;
      this.UseNeckLook = this._useNeckLookCache;
      this.CanHCommand = this._canHCommandCache;
      this.IsSpecial = this._isSpecialCache;
      this.HPositionID = this._hPositionIDCache;
      this.HPositionSubID = this._hPositionSubIDCache;
      this._navMeshObstacle.set_radius(this._obstacleSizeCache);
      this.TargetInSightActionPoint = this._recoveryActionPointFromHScene;
      this.Mode = this._recoveryModeFromHScene;
      this._recoveryActionPointFromHScene.SetSlot((Actor) this);
      this.BehaviorResources.ChangeMode(this._recoveryModeFromHScene);
    }

    public void InitiateHScene3P(AgentActor main, AgentActor sub)
    {
      if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
      {
        this.CurrentPoint.SetActiveMapItemObjs(true);
        this.CurrentPoint.ReleaseSlot((Actor) this);
        this.CurrentPoint = (ActionPoint) null;
      }
      HSceneManager instance = Singleton<HSceneManager>.Instance;
      AgentActor agentActor1 = main;
      AgentActor agentActor2 = sub;
      AgentActor agentActor3 = agentActor1;
      AgentActor agent2 = agentActor2;
      instance.HsceneEnter((Actor) agentActor3, -1, agent2, HSceneManager.HEvent.Normal);
    }

    private void StartTutorialADV(int id)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      switch (id)
      {
        case 1:
          this.packData.onComplete = (System.Action) (() =>
          {
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            List<StuffItem> itemList = player.PlayerData.ItemList;
            FishingDefinePack.ItemIDPair brokenFishingRod = Singleton<Resources>.Instance.FishingDefinePack.IDInfo.BrokenFishingRod;
            itemList.AddItem(new StuffItem(brokenFishingRod.CategoryID, brokenFishingRod.ItemID, 1));
            player.AddTutorialUI(Popup.Tutorial.Type.Collection, false);
            Manager.Map.SetTutorialProgress(3);
            Singleton<Manager.Map>.Instance.CreateTutorialSearchPoint();
            this.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.FoodRequest);
            this.EndTutorialADV(true);
          });
          break;
        case 2:
          this.packData.onComplete = (System.Action) (() =>
          {
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            this.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.HeadToBase);
            this.EndTutorialADV(true);
          });
          break;
        case 3:
          this.packData.onComplete = (System.Action) (() =>
          {
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            Singleton<Manager.Map>.Instance.Player.AddTutorialUI(Popup.Tutorial.Type.BasePoint, false);
            Manager.Map.SetTutorialProgress(8);
            this.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.GrilledFishRequest);
            this.EndTutorialADV(true);
          });
          break;
        case 4:
          this.packData.onComplete = (System.Action) (() =>
          {
            this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
            PlayerActor player = Singleton<Manager.Map>.Instance.Player;
            List<StuffItem> itemList = player.PlayerData.ItemList;
            FishingDefinePack.ItemIDPair grilledFish = Singleton<Resources>.Instance.FishingDefinePack.IDInfo.GrilledFish;
            itemList.RemoveItem(new StuffItem(grilledFish.CategoryID, grilledFish.ItemID, 1));
            Manager.Map.SetTutorialProgress(14);
            Singleton<Manager.Map>.Instance.DestroyTutorialLockArea();
            this.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.HeadToAgit);
            ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) Singleton<Resources>.Instance.CommonDefine.Tutorial.FollowGirlWaitTime)), (System.Action<M0>) (_ =>
            {
              ActorCameraControl cameraControl = player.CameraControl;
              Quaternion rotation = player.Rotation;
              // ISSUE: variable of the null type
              __Null y = ((Quaternion) ref rotation).get_eulerAngles().y;
              cameraControl.XAxisValue = (float) y;
              player.CameraControl.YAxisValue = player.CameraControl.ShotType != ShotType.PointOfView ? 0.6f : 0.5f;
              this.EndTutorialADV(true);
            }));
          });
          break;
        default:
          Debug.LogWarning((object) string.Format("TutorialADV 範囲外のIDが渡された ID[{0}]", (object) id));
          return;
      }
      PlayerActor player1 = Singleton<Manager.Map>.Instance.Player;
      this.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.Idle);
      this.openData.FindLoad(string.Format("{0}", (object) id), this.charaID, 100);
      this.packData.Init();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (id != 4)
      {
        player1.CommCompanion = (Actor) this;
        player1.PlayerController.ChangeState("Communication");
        Manager.ADV.ChangeADVCamera((Actor) this);
        Vector3 position = player1.Position;
        if (Singleton<Resources>.IsInstance())
        {
          if ((double) Singleton<Resources>.Instance.LocomotionProfile.TurnEnableAngle <= (double) Vector3.Angle(Vector3.op_Subtraction(position, this.Position), this.Forward))
          {
            this.Animation.StopAllAnimCoroutine();
            PlayState.AnimStateInfo personalIdleState = this.GetTutorialPersonalIdleState();
            this.Animation.PlayTurnAnimation(position, 1f, personalIdleState, false);
          }
          else
          {
            Transform transform = ((Component) this.Locomotor).get_transform();
            transform.LookAt(position);
            Vector3 eulerAngles = transform.get_eulerAngles();
            eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
            transform.set_eulerAngles(eulerAngles);
          }
        }
        else
        {
          this.Animation.StopAllAnimCoroutine();
          PlayState.AnimStateInfo personalIdleState = this.GetTutorialPersonalIdleState();
          this.Animation.PlayTurnAnimation(position, 1f, personalIdleState, false);
        }
        Transform transform1 = ((Component) player1.CameraControl.CameraComponent).get_transform();
        this.SetLookPtn(1, 3);
        this.SetLookTarget(1, 0, transform1);
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player1.CameraControl.CinemachineBrain.get_IsBlending() || this.Animation.PlayingTurnAnimation)), 1), 30, (FrameCountType) 0), (System.Action<M0>) (_ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)));
      }
      else
      {
        player1.PlayerController.ChangeState("Idle");
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          ((Component) this.Locomotor).get_transform().set_localRotation(Quaternion.Euler(0.0f, 80f, 0.0f));
          Vector3 vector3 = Vector3.op_Addition(Vector3.op_Multiply(this.Forward, 10f), this.Position);
          if (((Behaviour) player1.NavMeshAgent).get_enabled())
            player1.NavMeshAgent.Warp(vector3);
          else
            player1.Position = vector3;
          player1.CommCompanion = (Actor) this;
          player1.PlayerController.ChangeState("Communication");
          Transform transform1 = ((Component) player1.Locomotor).get_transform();
          transform1.LookAt(this.Position);
          Vector3 eulerAngles = transform1.get_eulerAngles();
          eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
          transform1.set_eulerAngles(eulerAngles);
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player1.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(CinemachineBlendDefinition&) ref player1.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
          Manager.ADV.ChangeADVCamera((Actor) this);
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player1.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
          Transform transform2 = ((Component) player1.CameraControl.CameraComponent).get_transform();
          this.SetLookPtn(1, 3);
          this.SetLookTarget(1, 0, transform2);
          ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player1.CameraControl.CinemachineBrain.get_IsBlending())), 1), 5, (FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 2f, true), (System.Action<M0>) (__ => {}), (System.Action) (() => ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(30, (FrameCountType) 0), (System.Action<M0>) (__ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)))))));
        }));
      }
    }

    private void EndTutorialADV(bool changeNormalMode)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      this.SetLookPtn(0, 3);
      this.SetLookTarget(0, 0, (Transform) null);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CameraControl.Mode = CameraMode.Normal;
      if (changeNormalMode)
      {
        player.PlayerController.ChangeState("Normal");
        MapUIContainer.SetVisibleHUD(true);
      }
      this.packData.Release();
    }

    private Dictionary<int, System.Action> presentParameterTable
    {
      get
      {
        return ((object) this).GetCache<Dictionary<int, System.Action>>(ref this._presentParameterTable, (Func<Dictionary<int, System.Action>>) (() =>
        {
          System.Action<FlavorSkill.Type, int> action = (System.Action<FlavorSkill.Type, int>) ((type, add) =>
          {
            Debug.Log((object) string.Format("AddParameter:[{0}:{1}]", (object) type, (object) add));
            int id = (int) type;
            this.AgentData.SetFlavorSkill(id, this.ChaControl.fileGameInfo.flavorState[id] + add);
          });
          return new Dictionary<int, System.Action>()
          {
            [0] = (System.Action) (() => action(FlavorSkill.Type.Pheromone, 10)),
            [1] = (System.Action) (() => action(FlavorSkill.Type.Reliability, 10)),
            [2] = (System.Action) (() => action(FlavorSkill.Type.Reason, 10)),
            [3] = (System.Action) (() => action(FlavorSkill.Type.Instinct, 10)),
            [4] = (System.Action) (() => action(FlavorSkill.Type.Dirty, 10)),
            [5] = (System.Action) (() => action(FlavorSkill.Type.Wariness, 10)),
            [6] = (System.Action) (() => action(FlavorSkill.Type.Sociability, 10)),
            [7] = (System.Action) (() => action(FlavorSkill.Type.Darkness, 10)),
            [8] = (System.Action) (() => action(FlavorSkill.Type.Pheromone, 50)),
            [9] = (System.Action) (() => action(FlavorSkill.Type.Reliability, 50)),
            [10] = (System.Action) (() => action(FlavorSkill.Type.Reason, 50)),
            [11] = (System.Action) (() => action(FlavorSkill.Type.Instinct, 50)),
            [12] = (System.Action) (() => action(FlavorSkill.Type.Dirty, 50)),
            [13] = (System.Action) (() => action(FlavorSkill.Type.Wariness, 50)),
            [14] = (System.Action) (() => action(FlavorSkill.Type.Sociability, 50)),
            [15] = (System.Action) (() => action(FlavorSkill.Type.Darkness, 50)),
            [16] = (System.Action) (() => action(FlavorSkill.Type.Pheromone, 100)),
            [17] = (System.Action) (() => action(FlavorSkill.Type.Reliability, 100)),
            [18] = (System.Action) (() => action(FlavorSkill.Type.Reason, 100)),
            [19] = (System.Action) (() => action(FlavorSkill.Type.Instinct, 100)),
            [20] = (System.Action) (() => action(FlavorSkill.Type.Dirty, 100)),
            [21] = (System.Action) (() => action(FlavorSkill.Type.Wariness, 100)),
            [22] = (System.Action) (() => action(FlavorSkill.Type.Sociability, 100)),
            [23] = (System.Action) (() => action(FlavorSkill.Type.Darkness, 100)),
            [24] = (System.Action) (() => action(FlavorSkill.Type.Pheromone, -50)),
            [25] = (System.Action) (() => action(FlavorSkill.Type.Reliability, -50)),
            [26] = (System.Action) (() => action(FlavorSkill.Type.Reason, -50)),
            [27] = (System.Action) (() => action(FlavorSkill.Type.Instinct, -50)),
            [28] = (System.Action) (() => action(FlavorSkill.Type.Dirty, -50)),
            [29] = (System.Action) (() => action(FlavorSkill.Type.Wariness, -50)),
            [30] = (System.Action) (() => action(FlavorSkill.Type.Sociability, -50)),
            [31] = (System.Action) (() => action(FlavorSkill.Type.Darkness, -50))
          };
        }));
      }
    }

    private void PresentADV(CommCommandList.CommandInfo[] restoreCommands)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      this.EndCommonSelection();
      MapUIContainer.CommandList.Visibled = false;
      MapUIContainer.ReserveSystemMenuMode(SystemMenuUI.MenuMode.InventoryEnter);
      SystemMenuUI systemUI = MapUIContainer.SystemMenuUI;
      InventoryUIController inventoryUI = systemUI.InventoryEnterUI;
      inventoryUI.isConfirm = true;
      inventoryUI.CountViewerVisible(false);
      inventoryUI.EmptyTextAutoVisible(true);
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      inventoryUI.SetItemFilter(agentProfile.PresentItemFilter);
      inventoryUI.itemList = (Func<List<StuffItem>>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList);
      inventoryUI.itemList_System = (Func<List<StuffItem>>) null;
      inventoryUI.DoubleClickAction((System.Action<InventoryFacadeViewer.DoubleClickData>) null);
      bool isVisibleCommand = true;
      inventoryUI.OnSubmit = (System.Action<StuffItem>) (item =>
      {
        if (Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null))
          Singleton<Game>.Instance.Dialog.TimeScale = 1f;
        this.Animation.StopAllAnimCoroutine();
        isVisibleCommand = false;
        InventoryUIController inventoryUiController = inventoryUI;
        if (inventoryUiController != null)
          inventoryUiController.OnClose();
        if (item != null)
        {
          Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary1;
          Dictionary<int, FoodParameterPacket> dictionary2;
          if (Singleton<Resources>.Instance.GameInfo.FoodParameterTable.TryGetValue(item.CategoryID, out dictionary1) && dictionary1.TryGetValue(item.ID, out dictionary2))
            this.AgentData.ItemList.AddItem(item);
          else if (Singleton<Resources>.Instance.GameInfo.DrinkParameterTable.TryGetValue(item.CategoryID, out dictionary1) && dictionary1.TryGetValue(item.ID, out dictionary2))
          {
            this.AgentData.ItemList.AddItem(item);
          }
          else
          {
            System.Action action;
            if (this.presentParameterTable.TryGetValue(item.ID, out action))
            {
              if (action != null)
                action();
              else
                Debug.LogError((object) string.Format("{0}:{1}.{2}:{3}:action none", (object) nameof (PresentADV), (object) nameof (item), (object) "ID", (object) item.ID));
            }
          }
        }
        else
          Debug.LogError((object) string.Format("{0}:{1}[{2}]", (object) nameof (PresentADV), (object) nameof (item), item != null ? (object) item.ID.ToString() : (object) "null"));
        this.openData.FindLoad("0", this.charaID, 3);
        this.packData.onComplete = (System.Action) (() => this.CheckTalkForceEnd(restoreCommands));
        Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
      });
      inventoryUI.OnClose = (System.Action) (() =>
      {
        inventoryUI.OnSubmit = (System.Action<StuffItem>) null;
        inventoryUI.IsActiveControl = false;
        systemUI.IsActiveControl = false;
        if (isVisibleCommand)
        {
          MapUIContainer.CommandList.Visibled = true;
          this.StartCommonSelection();
        }
        Singleton<Manager.Input>.Instance.FocusLevel = 0;
        Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
        inventoryUI.OnClose = (System.Action) null;
      });
      MapUIContainer.SetActiveSystemMenuUI(true);
    }

    private void SearchFavoriteTargetForADV()
    {
      int? nullable = new int?();
      using (IEnumerator<KeyValuePair<int, int>> enumerator = this.AgentData.FriendlyRelationShipTable.Where<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, bool>) (v => v.Key != -99)).OrderByDescending<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (v => v.Value)).Where<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, bool>) (v => v.Value > 0)).GetEnumerator())
      {
        if (enumerator.MoveNext())
          nullable = new int?(enumerator.Current.Key);
      }
      this.packData.isFavoriteTarget = nullable.HasValue;
      this.packData.FavoriteTargetName = string.Empty;
      if (!nullable.HasValue)
        return;
      int num = nullable.Value;
      if (num == -90)
      {
        this.packData.FavoriteTargetName = Singleton<Manager.Map>.Instance.Merchant.CharaName;
      }
      else
      {
        AgentActor agentActor;
        if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(num, ref agentActor))
          return;
        this.packData.FavoriteTargetName = agentActor.CharaName;
      }
    }

    private AgentAdvEventInfo.Param GetAdvEvent(int eventType)
    {
      int category = 6;
      if (this.AgentData.advEventLimitation.Contains(category))
        return (AgentAdvEventInfo.Param) null;
      if (this.AgentData.SickState.ID != -1)
        return (AgentAdvEventInfo.Param) null;
      if (this.IsBadMood())
        return (AgentAdvEventInfo.Param) null;
      int id1 = -1;
      int[] numArray = (int[]) null;
      ActionPoint currentPoint1 = this.CurrentPoint;
      bool flag = Object.op_Inequality((Object) currentPoint1, (Object) null);
      if (flag)
      {
        if (!currentPoint1.IDList.IsNullOrEmpty<int>())
          numArray = currentPoint1.IDList;
        else
          id1 = currentPoint1.ID;
      }
      foreach (AgentAdvEventInfo.Param obj in (IEnumerable<AgentAdvEventInfo.Param>) Singleton<Resources>.Instance.GameInfo.GetAgentAdvEvents(this).get_Values().Shuffle<AgentAdvEventInfo.Param>().OrderByDescending<AgentAdvEventInfo.Param, int>((Func<AgentAdvEventInfo.Param, int>) (x => x.SortID)))
      {
        AgentAdvEventInfo.Param param = obj;
        AgentActor agentActor = this;
        if (param.EventType == eventType && (!flag || !param.IsStateEmpty) && (!this.AgentData.GetAdvEventCheck(category).Contains(param.FileName) && (!((IEnumerable<int>) param.PlaceIDs).Any<int>() || ((IEnumerable<int>) param.PlaceIDs).Contains<int>(this.AreaID))))
        {
          if (numArray == null)
          {
            if (!param.IsState(id1, this.ActionID, this.PoseID))
              continue;
          }
          else if (!((IEnumerable<int>) numArray).Any<int>((Func<int, bool>) (id => param.IsState(id, agentActor.ActionID, agentActor.PoseID))))
            continue;
          if ((!((IEnumerable<int>) param.Phases).Any<int>() || ((IEnumerable<int>) param.Phases).Contains<int>(this.ChaControl.fileGameInfo.phase + 1)) && (param.TimeRound.Check(Singleton<Manager.Map>.Instance.Simulator.Now.Hour) && (!((IEnumerable<int>) param.Weathers).Any<int>() || ((IEnumerable<int>) param.Weathers).Contains<int>((int) Singleton<Manager.Map>.Instance.Simulator.Weather))))
          {
            switch (param.ExpansionID)
            {
              case 0:
                if (this.Mode != Desire.ActionType.EndTaskPetAnimal || !this.LivesWithAnimalSequence)
                  continue;
                break;
              case 1:
                int num = 89;
                PlayerActor player = Singleton<Manager.Map>.Instance.Player;
                if (!Object.op_Equality((Object) player, (Object) null))
                {
                  ActionPoint currentPoint2 = player.CurrentPoint;
                  if (!Object.op_Equality((Object) currentPoint2, (Object) null))
                  {
                    if (!currentPoint2.IDList.IsNullOrEmpty<int>())
                    {
                      if (((IEnumerable<int>) currentPoint2.IDList).Contains<int>(num))
                        break;
                      continue;
                    }
                    if (currentPoint2.ID == num)
                      break;
                    continue;
                  }
                  continue;
                }
                continue;
              case 2:
                ActionPoint currentPoint3 = this.CurrentPoint;
                if (flag)
                {
                  EventPivot component = (EventPivot) ((Component) currentPoint3).GetComponent<EventPivot>();
                  if (Object.op_Equality((Object) component, (Object) null) || Object.op_Equality((Object) component.PivotTransform, (Object) null))
                    continue;
                  break;
                }
                continue;
            }
            return param;
          }
        }
      }
      return (AgentAdvEventInfo.Param) null;
    }

    private void InitCommon()
    {
      this._ladInfo = new AgentActor.LeaveAloneDisposableInfo();
      this._colDisposableList = new List<AgentActor.ColDisposableInfo>();
      string[,] strArray1 = new string[2, 2]
      {
        {
          "cf_hit_Mune02_s_L",
          "Chara"
        },
        {
          "cf_hit_Mune02_s_R",
          "Chara"
        }
      };
      Transform transform1 = this.ChaControl.objBodyBone.get_transform();
      int length = strArray1.GetLength(0);
      this._touchList = new List<AgentActor.TouchInfo>();
      for (int index1 = 0; index1 < length; ++index1)
      {
        int num1 = 0;
        Transform transform2 = transform1;
        string[,] strArray2 = strArray1;
        int index2 = index1;
        int index3 = num1;
        int num2 = index3 + 1;
        string name = strArray2[index2, index3];
        GameObject loop = transform2.FindLoop(name);
        if (!Object.op_Equality((Object) loop, (Object) null))
        {
          Collider component = (Collider) loop.GetComponent<Collider>();
          if (!Object.op_Equality((Object) component, (Object) null))
          {
            GameObject gameObject = loop;
            Collider col = component;
            string[,] strArray3 = strArray1;
            int index4 = index1;
            int index5 = num2;
            int num3 = index5 + 1;
            int layer = LayerMask.NameToLayer(strArray3[index4, index5]);
            this._touchList.Add(new AgentActor.TouchInfo(gameObject, col, layer));
            this._colDisposableList.Add(new AgentActor.ColDisposableInfo(component, new System.Action(this.OnTouch), new System.Action(this.OnEnter), new System.Action(this.OnExit)));
          }
        }
      }
    }

    private void StartCommonSelection()
    {
      this.StartLeaveAlone();
      foreach (AgentActor.ColDisposableInfo colDisposable in this._colDisposableList)
        colDisposable.Start();
    }

    private void EndCommonSelection()
    {
      if (this._ladInfo != null)
        this._ladInfo.End();
      if (!this._colDisposableList.IsNullOrEmpty<AgentActor.ColDisposableInfo>())
      {
        foreach (AgentActor.ColDisposableInfo colDisposable in this._colDisposableList)
          colDisposable?.End();
      }
      if (this._disposableInfo == null)
        return;
      this._disposableInfo.End();
    }

    private void StartLeaveAlone()
    {
      this._ladInfo.End();
      this._ladInfo.Timer = new SingleAssignmentDisposable();
      this._ladInfo.Timer.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>(Observable.Repeat<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(30.0))), (System.Action<M0>) (_ => this.OnLeaveAlone())), (Component) this));
    }

    private void OnLeaveAlone()
    {
      this._ladInfo.Timer.Dispose();
      this.openData.FindLoad("9", this.charaID, 0);
      this.packData.onComplete = (System.Action) (() => Debug.Log((object) "放置終了"));
      this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
      this.packData.CommandListVisibleEnabled(false);
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
      Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 1);
      this._ladInfo.Wait = new SingleAssignmentDisposable();
      this._ladInfo.Wait.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.CommonEventEnd), false)), (Component) this));
    }

    private void OnTouch()
    {
      ++this.TouchCount;
      this.Animation.StopAllAnimCoroutine();
      this.openData.FindLoad("8", this.charaID, 0);
      this.packData.onComplete = (System.Action) (() => Debug.Log((object) "お触り終了"));
      this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
      this.packData.CommandListVisibleEnabled(false);
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
      this._ladInfo.End();
      Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 1);
      foreach (AgentActor.ColDisposableInfo colDisposable in this._colDisposableList)
        colDisposable.End();
      this._disposableInfo.End();
      this._disposableInfo.Wait = new SingleAssignmentDisposable();
      this._disposableInfo.Wait.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.CommonEventEnd), false)), (Component) this));
    }

    private void OnEnter()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      Cursor.SetCursor(Singleton<Resources>.Instance.CommonDefine.Icon.TouchCursorTexture, Vector2.op_Multiply(((Texture) Singleton<Resources>.Instance.CommonDefine.Icon.TouchCursorTexture).get_texelSize(), 0.5f), (CursorMode) 1);
    }

    private void OnExit()
    {
      Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 1);
    }

    [DebuggerHidden]
    private IEnumerator CommonEventEnd()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCommonEventEnd\u003Ec__Iterator2()
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

    public override ActorAnimation Animation
    {
      get
      {
        return (ActorAnimation) this._animation;
      }
    }

    public ActorAnimationAgent AnimationAgent
    {
      get
      {
        return this._animation;
      }
    }

    public override ActorLocomotion Locomotor
    {
      get
      {
        return (ActorLocomotion) this._character;
      }
    }

    public ActorLocomotionAgent LocomotorAgent
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

    public AgentController AgentController
    {
      get
      {
        return this._controller;
      }
    }

    public bool ReleasableCommand
    {
      get
      {
        return !this.IsImpossible;
      }
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.Mode == Desire.ActionType.Onbu)
        return false;
      if (!this.TutorialMode)
      {
        switch (this.BehaviorResources.Mode)
        {
          case Desire.ActionType.Idle:
          case Desire.ActionType.ChaseLesbianH:
          case Desire.ActionType.ChaseToTalk:
            return false;
        }
      }
      if (this.Mode == Desire.ActionType.SearchActor || this.Mode == Desire.ActionType.SearchBirthdayGift || (this.Mode == Desire.ActionType.SearchGift || this.Mode == Desire.ActionType.GiftForceEncounter) || (this.Mode == Desire.ActionType.SearchH || this.Mode == Desire.ActionType.EndTaskH || (this.Mode == Desire.ActionType.SearchRevRape || this.Mode == Desire.ActionType.ReverseRape)) || (this.Mode == Desire.ActionType.SearchPlayerToTalk || this.Mode == Desire.ActionType.WalkWithAgent || (this.Mode == Desire.ActionType.WalkWithAgentFollow || this.Mode == Desire.ActionType.InviteSleep) || (this.Mode == Desire.ActionType.InviteSleepH || this.Mode == Desire.ActionType.InviteEat || (this.Mode == Desire.ActionType.InviteBreak || (double) distance > (double) radiusA))))
        return false;
      Vector3 position = this.Position;
      position.y = (__Null) 0.0;
      float num = angle / 2f;
      return (double) Vector3.Angle(Vector3.op_Subtraction(position, basePosition), forward) <= (double) num;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        float radius = this.CommandType != CommandType.Forward ? radiusB : radiusA;
        if (this.TutorialMode)
        {
          if (!this.TutorialCanTalk)
            return false;
          Vector3 dest;
          if (((Behaviour) this._navMeshAgent).get_isActiveAndEnabled())
          {
            dest = this.Position;
          }
          else
          {
            Vector3? position = (Vector3?) this.TargetStoryPoint?.Position;
            dest = !position.HasValue ? Vector3.get_zero() : position.Value;
          }
          flag1 &= this.IsReachable(nmAgent, dest, radius);
        }
        else if (((Behaviour) this._navMeshAgent).get_isActiveAndEnabled())
          flag1 &= this.IsReachable(nmAgent, this.Position, radius);
        else if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          bool flag2 = false;
          using (List<Transform>.Enumerator enumerator = this.CurrentPoint.NavMeshPoints.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Transform current = enumerator.Current;
              flag2 = this.IsReachable(nmAgent, current.get_position(), radius);
              if (flag2)
                break;
            }
          }
          if (!flag2)
            flag1 = false;
        }
        else
          flag1 = false;
      }
      else
        flag1 = false;
      return flag1;
    }

    private bool IsReachable(NavMeshAgent navMeshAgent, Vector3 dest, float radius)
    {
      navMeshAgent.CalculatePath(dest, this._pathForCalc);
      if (this._pathForCalc.get_status() != null)
        return false;
      float num1 = 0.0f;
      Vector3[] corners = this._pathForCalc.get_corners();
      for (int index = 0; index < corners.Length - 1; ++index)
      {
        float num2 = Vector3.Distance(corners[index], corners[index + 1]);
        num1 += num2;
        if ((double) num1 > (double) radius)
          return false;
      }
      return true;
    }

    public bool IsImpossible { get; private set; }

    public void SetForceImpossible(bool value, Actor actor)
    {
      this.IsImpossible = value;
      this.CommandPartner = actor;
    }

    public bool SetImpossible(bool value, Actor actor)
    {
      if (this.IsImpossible == value || value && Object.op_Inequality((Object) this.CommandPartner, (Object) null))
        return false;
      this.IsImpossible = value;
      if (value)
      {
        this._elapsedTimeFromLastImpossible = 0.0f;
        this.CommandPartner = actor;
      }
      else
        this.CommandPartner = (Actor) null;
      return true;
    }

    public override bool IsNeutralCommand
    {
      get
      {
        if (this.TutorialMode)
          return this.TutorialCanTalk;
        if (this.Mode == Desire.ActionType.SearchActor || this.Mode == Desire.ActionType.WithPlayer || this.Mode == Desire.ActionType.SearchPlayerToTalk || this.Mode == Desire.ActionType.EndTaskTalkToPlayer)
          return false;
        return this.IsAdvEvent || this.CanTalk;
      }
    }

    public bool IsEncounterable
    {
      get
      {
        return this.StateType == AIProject.Definitions.State.Type.Normal || this.StateType == AIProject.Definitions.State.Type.Greet;
      }
    }

    public AIProject.Definitions.State.Type StateType
    {
      get
      {
        return this._stateType;
      }
      set
      {
        if (this._stateType == value)
          return;
        this._stateType = value;
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        CommandArea commandArea = Singleton<Manager.Map>.Instance.Player?.PlayerController?.CommandArea;
        if (!Object.op_Inequality((Object) commandArea, (Object) null) || !commandArea.ContainsConsiderationObject((ICommandable) this))
          return;
        commandArea.RefreshCommands();
      }
    }

    public Transform TargetCommun { get; set; }

    public int SelectedActionID { get; set; }

    public bool IsEvent { get; set; }

    public bool IsStandby
    {
      get
      {
        return this._isStandby;
      }
      set
      {
        if (this._isStandby == value)
          return;
        this._isStandby = value;
        Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RefreshCommands();
      }
    }

    public bool IsCloseToPlayer
    {
      get
      {
        AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
        return (double) this._distanceTweenPlayer <= (double) rangeSetting.arrivedDistance && (double) this._heightDistTweenPlayer <= (double) rangeSetting.acceptableHeight;
      }
    }

    public bool IsFarPlayer
    {
      get
      {
        AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
        return (double) this._distanceTweenPlayer > (double) rangeSetting.leaveDistance || (double) this._heightDistTweenPlayer > (double) rangeSetting.acceptableHeight;
      }
    }

    public bool IsFarPlayerInSurprise
    {
      get
      {
        AgentProfile.RangeParameter rangeSetting = Singleton<Resources>.Instance.AgentProfile.RangeSetting;
        return (double) this._distanceTweenPlayer > (double) rangeSetting.leaveDistanceInSurprise || (double) this._heightDistTweenPlayer > (double) rangeSetting.acceptableHeight;
      }
    }

    public bool IsCloseToPlayerByPhotoMode
    {
      get
      {
        AgentProfile.PhotoShotRangeParameter shotRangeSetting = Singleton<Resources>.Instance.AgentProfile.PhotoShotRangeSetting;
        return (double) this._distanceTweenPlayer <= (double) shotRangeSetting.arriveDistance && (double) this._heightDistTweenPlayer <= (double) shotRangeSetting.acceptableHeight && (double) this._angleDiffTweenPlayer * 2.0 <= (double) shotRangeSetting.sightAngle;
      }
    }

    public bool IsFarPlayerByPhotoMode
    {
      get
      {
        AgentProfile.PhotoShotRangeParameter shotRangeSetting = Singleton<Resources>.Instance.AgentProfile.PhotoShotRangeSetting;
        return (double) shotRangeSetting.leaveDistance < (double) this._distanceTweenPlayer || (double) shotRangeSetting.acceptableHeight < (double) this._heightDistTweenPlayer || (double) shotRangeSetting.invisibleAngle < (double) this._angleDiffTweenPlayer * 2.0;
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return this._layer;
      }
    }

    public AgentBehaviorTree NormalBehaviorTree { get; private set; }

    public override Desire.ActionType Mode
    {
      get
      {
        return this._modeType;
      }
      set
      {
        if (this._modeType == value)
          return;
        this._modeType = value;
      }
    }

    public Desire.ActionType PrevMode { get; set; }

    public Desire.ActionType PrevActionMode { get; set; }

    public Desire.ActionType ReservedMode { get; set; }

    public AIProject.Definitions.Tutorial.ActionType TutorialType { get; set; }

    public bool TutorialMode { get; set; }

    public bool TutorialCanTalk { get; set; }

    public int TutorialLocomoCaseID { get; set; }

    public Actor CommandPartner { get; set; }

    public BehaviorTreeResources BehaviorResources { get; private set; }

    protected TutorialBehaviorTreeResources TutorialBehaviorResources { get; private set; }

    public void EnableBehavior()
    {
      if (Object.op_Inequality((Object) this.BehaviorResources, (Object) null) && !((Behaviour) this.BehaviorResources).get_enabled())
        ((Behaviour) this.BehaviorResources).set_enabled(true);
      if (!Object.op_Inequality((Object) this.TutorialBehaviorResources, (Object) null) || ((Behaviour) this.TutorialBehaviorResources).get_enabled())
        return;
      ((Behaviour) this.TutorialBehaviorResources).set_enabled(true);
    }

    public void DisableBehavior()
    {
      if (Object.op_Inequality((Object) this.BehaviorResources, (Object) null) && ((Behaviour) this.BehaviorResources).get_enabled())
        ((Behaviour) this.BehaviorResources).set_enabled(false);
      if (!Object.op_Inequality((Object) this.TutorialBehaviorResources, (Object) null) || !((Behaviour) this.TutorialBehaviorResources).get_enabled())
        return;
      ((Behaviour) this.TutorialBehaviorResources).set_enabled(false);
    }

    public CommandType CommandType { get; }

    public override ICharacterInfo TiedInfo
    {
      get
      {
        return (ICharacterInfo) this.AgentData;
      }
    }

    public AgentData AgentData { get; set; }

    public Desire.Type ScheduledDesire { get; private set; }

    public Desire.Type RuntimeDesire { get; set; }

    public Desire.Type RequestedDesire
    {
      get
      {
        if (!Desire.ModeTable.TryGetValue(this._modeType, out this._requestedDesire))
          this._requestedDesire = Desire.Type.None;
        return this._requestedDesire;
      }
    }

    public List<Actor> TargetActors { get; private set; } = new List<Actor>();

    public ReadOnlyCollection<ActionPoint> ActionPoints
    {
      get
      {
        if (this._readonlyActionPoints == null)
          this._readonlyActionPoints = new ReadOnlyCollection<ActionPoint>((IList<ActionPoint>) this._actionPoints);
        return this._readonlyActionPoints;
      }
    }

    public ActionPoint[] SearchTargets { get; private set; } = (ActionPoint[]) Array.Empty<ActionPoint>();

    public List<AnimalBase> TargetAnimals { get; private set; } = new List<AnimalBase>();

    public IVisible TargetVisual { get; set; }

    public ActionPoint TargetInSightActionPoint
    {
      get
      {
        return this._targetInSightActionPoint;
      }
      set
      {
        this._targetInSightActionPoint = value;
      }
    }

    public Vector3? DestPosition { get; set; }

    public ActionPoint BookingActionPoint { get; set; }

    public StoryPoint TargetStoryPoint { get; set; }

    public OffMeshLink TargetOffMeshLink { get; set; }

    public OffMeshLink NearOffMeshLink
    {
      get
      {
        OffMeshLink offMeshLink = (OffMeshLink) null;
        OffMeshLinkData currentOffMeshLinkData1 = this._navMeshAgent.get_currentOffMeshLinkData();
        if (((OffMeshLinkData) ref currentOffMeshLinkData1).get_activated())
        {
          OffMeshLinkData currentOffMeshLinkData2 = this._navMeshAgent.get_currentOffMeshLinkData();
          offMeshLink = ((OffMeshLinkData) ref currentOffMeshLinkData2).get_offMeshLink();
        }
        if (Object.op_Equality((Object) offMeshLink, (Object) null))
        {
          OffMeshLinkData nextOffMeshLinkData1 = this._navMeshAgent.get_nextOffMeshLinkData();
          if (((OffMeshLinkData) ref nextOffMeshLinkData1).get_activated())
          {
            OffMeshLinkData nextOffMeshLinkData2 = this._navMeshAgent.get_nextOffMeshLinkData();
            offMeshLink = ((OffMeshLinkData) ref nextOffMeshLinkData2).get_offMeshLink();
          }
        }
        return offMeshLink;
      }
    }

    public ActionPoint PrevActionPoint { get; set; }

    public bool UpdateMotivation { get; set; }

    public float MotivationInEncounter { get; set; }

    public float RuntimeMotivationInPhoto { get; set; }

    public Actor TargetInSightActor
    {
      get
      {
        return this._targetInSightActor;
      }
      set
      {
        this._targetInSightActor = value;
      }
    }

    public PlayerActor Pair { get; set; }

    public AnimalBase TargetInSightAnimal
    {
      get
      {
        return this._targetInSightAnimal;
      }
      set
      {
        this._targetInSightAnimal = value;
      }
    }

    public float AnimalFovAngleOffsetY { get; set; }

    public Dictionary<int, CollisionState> ActionPointCollisionStateTable { get; private set; } = new Dictionary<int, CollisionState>();

    public Dictionary<int, CollisionState> ActorCollisionStateTable { get; private set; } = new Dictionary<int, CollisionState>();

    public Dictionary<int, CollisionState> ActorFarCollisionStateTable { get; private set; } = new Dictionary<int, CollisionState>();

    public Dictionary<int, CollisionState> AnimalCollisionStateTable { get; private set; } = new Dictionary<int, CollisionState>();

    public PoseKeyPair? SurprisePoseID { get; set; }

    public bool IsMasturbating { get; set; }

    public bool SleepTrigger { get; set; }

    public bool SuccessCook { get; set; }

    private void Awake()
    {
      this.BehaviorResources = (BehaviorTreeResources) ((Component) this).GetComponentInChildren<BehaviorTreeResources>();
      this.NormalBehaviorTree = this.BehaviorResources.GetBehaviorTree(Desire.ActionType.Normal);
      this.BehaviorResources.Initialize();
    }

    protected override void OnStart()
    {
      ObservableExtensions.Subscribe<long>(Observable.OnErrorRetry<long, Exception>(Observable.Do<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()), (System.Action<Exception>) (ex => Debug.LogException(ex))), (System.Action<M1>) (ex => Debug.LogException(ex))), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    public override IEnumerator LoadAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CLoadAsync\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    public void Load()
    {
      AgentData agentData = this.AgentData;
      if (agentData != null)
        this.LoadChar(agentData.CharaFileName);
      else
        this.LoadChar(string.Empty);
      if (!this.ChaControl.fileGameInfo.gameRegistration)
      {
        this.ChaControl.chaFile.InitGameInfoParam();
        this.ChaControl.fileGameInfo.gameRegistration = true;
        if (!agentData.CharaFileName.IsNullOrEmpty())
          this.ChaControl.chaFile.SaveCharaFile(this.ChaControl.chaFile.charaFileName, byte.MaxValue, false);
      }
      this.LoadEquipments();
      if (this._chaBodyParts == null)
        this._chaBodyParts = new Dictionary<Actor.BodyPart, Transform>();
      this._chaBodyParts[Actor.BodyPart.Body] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Hips")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.Bust] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Mune00")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.Head] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("N_Head")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.LeftFoot] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Foot01_L")?.get_transform();
      this._chaBodyParts[Actor.BodyPart.RightFoot] = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Foot01_R")?.get_transform();
      Animator animBody = this.ChaControl.animBody;
      this.packData = new AgentActor.PackData();
      this.packData.SetCommandData((ADV.ICommandData) Singleton<Game>.Instance?.WorldData?.Environment);
      this.packData.SetParam((IParams) agentData, (IParams) Singleton<Game>.Instance?.WorldData?.PlayerData);
      this.InitCommands();
      FullBodyBipedIK componentInChildren = (FullBodyBipedIK) ((Component) animBody).GetComponentInChildren<FullBodyBipedIK>(true);
      GameObject gameObject = ((Component) this._animation).get_gameObject();
      ActorAnimationAgent actorAnimationAgent = this._animation.CloneComponent(((Component) animBody).get_gameObject());
      actorAnimationAgent.IK = componentInChildren;
      actorAnimationAgent.Actor = (Actor) this;
      actorAnimationAgent.Character = (ActorLocomotion) this._character;
      actorAnimationAgent.Animator = animBody;
      this._animation = actorAnimationAgent;
      Object.Destroy((Object) gameObject);
      AssetBundleInfo outInfo = (AssetBundleInfo) null;
      RuntimeAnimatorController charaAnimator = Singleton<Resources>.Instance.Animation.GetCharaAnimator(0, ref outInfo);
      this.Animation.SetDefaultAnimatorController(charaAnimator);
      this.Animation.SetAnimatorController(charaAnimator);
      this.Animation.AnimABInfo = outInfo;
      animBody.Play("Locomotion", 0, 0.0f);
      this._character.CharacterAnimation = this._animation;
      this.InitializeIK();
      this._controller.StartBehavior();
    }

    public override void LoadEquipments()
    {
      this.LoadEquipmentItem(this.AgentData.EquipedHeadItem, ChaControlDefine.ExtraAccessoryParts.Head);
      this.LoadEquipmentItem(this.AgentData.EquipedBackItem, ChaControlDefine.ExtraAccessoryParts.Back);
      this.LoadEquipmentItem(this.AgentData.EquipedNeckItem, ChaControlDefine.ExtraAccessoryParts.Neck);
      this.LoadEquipmentItem(this.AgentData.EquipedLampItem, ChaControlDefine.ExtraAccessoryParts.Waist);
    }

    public override void EnableEntity()
    {
      if (this._modeCache.Item3 != null)
      {
        this.ActivateNavMeshAgent();
        this.NavMeshAgent.Warp(this.Position);
      }
      else if (this.EventKey == EventType.Move)
      {
        this.ActivateNavMeshAgent();
        this.SetDefaultStateHousingItem();
        if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          OffMeshLink component = (OffMeshLink) ((Component) this.CurrentPoint).GetComponent<OffMeshLink>();
          if (Object.op_Inequality((Object) component, (Object) null))
          {
            Transform endTransform = component.get_endTransform();
            this.NavMeshAgent.Warp(endTransform.get_position());
            this.Rotation = endTransform.get_rotation();
          }
          this.CurrentPoint.RemoveBookingUser((Actor) this);
          this.CurrentPoint.SetActiveMapItemObjs(true);
          this.CurrentPoint.ReleaseSlot((Actor) this);
          this.CurrentPoint = (ActionPoint) null;
        }
        this.EventKey = (EventType) 0;
        this.TargetInSightActionPoint = (ActionPoint) null;
        this.Animation.ResetDefaultAnimatorController();
        this._modeCache.Item1 = (__Null) 0;
        this._modeCache.Item2 = (__Null) 0;
      }
      else if (this.EventKey == EventType.DoorOpen)
      {
        this.ActivateNavMeshAgent();
        this.SetDefaultStateHousingItem();
        if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          DoorPoint currentPoint = this.CurrentPoint as DoorPoint;
          if (Object.op_Inequality((Object) currentPoint, (Object) null))
          {
            if (currentPoint.OpenState == DoorPoint.OpenPattern.Close)
            {
              if (currentPoint.OpenType == DoorPoint.OpenTypeState.Right || currentPoint.OpenType == DoorPoint.OpenTypeState.Right90)
                currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenRight, true);
              else
                currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenLeft, true);
            }
            currentPoint.RemoveBookingUser((Actor) this);
          }
          this.CurrentPoint.SetActiveMapItemObjs(true);
          this.CurrentPoint.ReleaseSlot((Actor) this);
          this.CurrentPoint = (ActionPoint) null;
        }
        this.EventKey = (EventType) 0;
        this.TargetInSightActionPoint = (ActionPoint) null;
        this.Animation.ResetDefaultAnimatorController();
        this._modeCache.Item1 = (__Null) 0;
        this._modeCache.Item2 = (__Null) 0;
      }
      else if (this.EventKey == EventType.Toilet || this.EventKey == EventType.DressIn || (this.EventKey == EventType.Bath || this.EventKey == EventType.DressOut) || (this.AgentData.PlayedDressIn || this._modeCache.Item1 == 97))
      {
        this.ActivateNavMeshAgent();
        this.SetDefaultStateHousingItem();
        if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        {
          this.CurrentPoint.SetActiveMapItemObjs(true);
          this.CurrentPoint.ReleaseSlot((Actor) this);
          this.CurrentPoint = (ActionPoint) null;
        }
        this.ChaControl.ChangeNowCoordinate(true, true);
        this.AgentData.BathCoordinateFileName = (string) null;
        this.ChaControl.SetClothesState(0, (byte) 0, true);
        this.ChaControl.SetClothesState(1, (byte) 0, true);
        this.ChaControl.SetClothesState(2, (byte) 0, true);
        this.ChaControl.SetClothesState(3, (byte) 0, true);
        this.AgentData.PlayedDressIn = false;
        this.EventKey = (EventType) 0;
        this.TargetInSightActionPoint = (ActionPoint) null;
        this.Animation.ResetDefaultAnimatorController();
        this._modeCache.Item1 = (__Null) 0;
        this._modeCache.Item2 = (__Null) 0;
      }
      else if (this._modeCache.Item1 == 44)
      {
        this.ActivateNavMeshAgent();
        this.EventKey = (EventType) 0;
        this.TargetInSightActionPoint = (ActionPoint) null;
        this.Animation.ResetDefaultAnimatorController();
        this._modeCache.Item1 = (__Null) 0;
        this._modeCache.Item2 = (__Null) 0;
      }
      ((Behaviour) this.Controller).set_enabled(true);
      this.ChaControl.visibleAll = true;
      ((Behaviour) this.AnimationAgent).set_enabled(true);
      this.SetActiveOnEquipedItem(false);
      this.EnableBehavior();
      if (this._modeCache.Item1 == null && this._modeCache.Item2 == null)
      {
        this.ResetActionFlag();
        if (this._schedule.enabled)
          this._schedule.enabled = false;
      }
      this.Mode = (Desire.ActionType) this._modeCache.Item1;
      this.BehaviorResources.ChangeMode((Desire.ActionType) this._modeCache.Item2);
      this.AnimationAgent.EnableItems();
      this.AnimationAgent.EnableParticleRenderer();
    }

    public override void DisableEntity()
    {
      Desire.ActionType mode = this.BehaviorResources.Mode;
      bool enabled = ((Behaviour) this.NavMeshAgent).get_enabled();
      this._modeCache = new ValueTuple<Desire.ActionType, Desire.ActionType, bool>(this.Mode, mode, enabled);
      this.SetActiveOnEquipedItem(false);
      ((Behaviour) this.Controller).set_enabled(false);
      if (enabled)
        ((Behaviour) this.NavMeshAgent).set_enabled(false);
      ((Behaviour) this.AnimationAgent).set_enabled(false);
      this.ChaControl.visibleAll = false;
      if (mode == Desire.ActionType.EndTaskMasturbation || mode == Desire.ActionType.EndTaskLesbianH || mode == Desire.ActionType.EndTaskLesbianMerchantH)
        this.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
      this.DisableBehavior();
      this.AnimationAgent.DisableItems();
      this.AnimationAgent.DisableParticleRenderer();
    }

    private void OnUpdate()
    {
      ((Component) this._navMeshObstacle).get_transform().set_position(this.Position);
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
      AgentData agentData = this.AgentData;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      if (Singleton<Manager.Map>.Instance.Simulator.EnabledTimeProgression)
      {
        Weather weather = Singleton<Manager.Map>.Instance.Simulator.Weather;
        if (this.AreaType == MapArea.AreaType.Indoor)
        {
          agentData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
        }
        else
        {
          switch (weather)
          {
            case Weather.Rain:
              agentData.Wetness += statusProfile.WetRateInRain * Time.get_deltaTime();
              break;
            case Weather.Storm:
              agentData.Wetness += statusProfile.WetRateInStorm * Time.get_deltaTime();
              break;
            default:
              agentData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
              break;
          }
          agentData.Wetness = Mathf.Clamp(agentData.Wetness, 0.0f, 100f);
        }
      }
      if ((double) agentData.Wetness >= 100.0)
        agentData.IsWet = true;
      if ((double) agentData.Wetness <= 0.0)
        agentData.IsWet = false;
      if (Object.op_Inequality((Object) this.ChaControl, (Object) null))
        this.ChaControl.wetRate = Mathf.InverseLerp(0.0f, 100f, agentData.Wetness);
      if (((Behaviour) this.BehaviorResources).get_enabled() && this._schedule.enabled && (!this._schedule.useGameTime && this._schedule.progress))
      {
        this._schedule.elapsedTime += Time.get_deltaTime();
        if ((double) this._schedule.elapsedTime > (double) this._schedule.duration)
          this._schedule.enabled = false;
      }
      Vector3 position1 = this.Position;
      Vector3 position2 = Singleton<Manager.Map>.Instance.Player.Position;
      Vector3 forward = this.Forward;
      position1.y = (__Null) (double) (position2.y = forward.y = (__Null) 0.0f);
      this._distanceTweenPlayer = Vector3.Distance(position1, position2);
      this._heightDistTweenPlayer = Mathf.Abs((float) (Singleton<Manager.Map>.Instance.Player.Position.y - this.Position.y));
      ((Vector3) ref forward).Normalize();
      AgentProfile.PhotoShotRangeParameter shotRangeSetting = Singleton<Resources>.Instance.AgentProfile.PhotoShotRangeSetting;
      Vector3 vector3 = Vector3.op_Addition(position1, Vector3.op_Multiply(forward, shotRangeSetting.sightOffsetZ));
      this._angleDiffTweenPlayer = Vector3.Angle(forward, Vector3.op_Subtraction(position2, vector3));
      this.UpdateActionPointCollision();
      this.UpdateActorSightCollision();
      this.UpdateActorFarSightCollision();
      this.UpdateAnimalSightCollision();
      this._elapsedTimeFromLastImpossible += Time.get_deltaTime();
      if (((Behaviour) this.NavMeshAgent).get_isActiveAndEnabled() && this.NavMeshAgent.get_isOnNavMesh())
      {
        this.AgentData.Position = this.Position;
        this.AgentData.Rotation = this.Rotation;
      }
      this.AgentData.ChunkID = this.ChunkID;
      this.AgentData.ModeType = this.Mode;
      this.AgentData.PrevMode = this.PrevMode;
      this.AgentData.TutorialModeType = this.TutorialType;
      if (!this.TutorialMode)
        this.UpdateEncounter();
      if (this._mapAreaID == null || !Object.op_Inequality((Object) this.MapArea, (Object) null))
        return;
      ((ReactiveProperty<int>) this._mapAreaID).set_Value(this.MapArea.AreaID);
      this.AgentData.AreaID = this.MapArea.AreaID;
    }

    private void UpdateEncounter()
    {
      IState state = Singleton<Manager.Map>.Instance.Player.PlayerController.State;
      bool flag = this.ChaControl.fileGameInfo.flavorState[2] >= Singleton<Resources>.Instance.StatusProfile.SurpriseBorder;
      switch (state)
      {
        case Normal _:
          if (this._modeType == Desire.ActionType.EndTaskMasturbation || this._modeType == Desire.ActionType.EndTaskToilet || this._modeType == Desire.ActionType.EndTaskBath)
          {
            if (!flag || !this.ExistsInvader() || !Object.op_Inequality((Object) this.CurrentPoint, (Object) null) || (!this.SurprisePoseID.HasValue || !this.CanTalk))
              break;
            switch (this._modeType)
            {
              case Desire.ActionType.EndTaskMasturbation:
                this.ReservedMode = Desire.ActionType.Normal;
                break;
              case Desire.ActionType.EndTaskToilet:
                this.ReservedMode = Desire.ActionType.Normal;
                break;
              case Desire.ActionType.EndTaskBath:
                this.ReservedMode = Desire.ActionType.GotoDressOut;
                break;
            }
            this.ChangeBehavior(Desire.ActionType.FoundPeeping);
            break;
          }
          PlayerActor player1 = Singleton<Manager.Map>.Instance.Player;
          if (this.Mode != Desire.ActionType.Encounter)
          {
            if (this.UseNeckLook)
            {
              if ((!this.IsCloseToPlayer || !this._prevCloseToPlayer) && (this.ReleasableCommand && this.IsFarPlayer) && this._prevCloseToPlayer)
              {
                this.ChaControl.ChangeLookEyesPtn(0);
                this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
                this.ChaControl.ChangeLookNeckPtn(3, 1f);
                this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
              }
            }
            else
            {
              this.ChaControl.ChangeLookEyesPtn(0);
              this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
              this.ChaControl.ChangeLookNeckPtn(3, 1f);
              this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            }
          }
          if (this.IsCloseToPlayer && !this._prevCloseToPlayer && (this.CanTalk && Singleton<Resources>.Instance.AgentProfile.EncounterWhitelist.Contains(this._modeType)) && this.BehaviorResources.Mode != Desire.ActionType.Idle)
          {
            if (this.IsTraverseOffMeshLink())
              break;
            this._prevCloseToPlayer = true;
            this.ChangeBehavior(Desire.ActionType.Encounter);
            Debug.Log((object) string.Format("UpdateEncounter ChangeBehavior Encounter proc frame : {0}", (object) Time.get_frameCount()));
            break;
          }
          if (!this.ReleasableCommand || !this.IsFarPlayer || !this._prevCloseToPlayer)
            break;
          this._prevCloseToPlayer = false;
          break;
        case Onbu _:
          if (this._modeType != Desire.ActionType.EndTaskMasturbation && this._modeType != Desire.ActionType.EndTaskToilet && this._modeType != Desire.ActionType.EndTaskBath || (!flag || !this.ExistsInvader() || !Object.op_Inequality((Object) this.CurrentPoint, (Object) null)) || (!this.SurprisePoseID.HasValue || !this.CanTalk))
            break;
          switch (this._modeType)
          {
            case Desire.ActionType.EndTaskMasturbation:
              this.ReservedMode = Desire.ActionType.Normal;
              break;
            case Desire.ActionType.EndTaskToilet:
              this.ReservedMode = Desire.ActionType.Normal;
              break;
            case Desire.ActionType.EndTaskBath:
              this.ReservedMode = Desire.ActionType.GotoDressOut;
              break;
          }
          this.ChangeBehavior(Desire.ActionType.FoundPeeping);
          break;
        case Photo _:
          if (this._modeType == Desire.ActionType.EndTaskMasturbation || this._modeType == Desire.ActionType.EndTaskToilet || this._modeType == Desire.ActionType.EndTaskBath)
          {
            if (!flag || !this.ExistsInvader() || !Object.op_Inequality((Object) this.CurrentPoint, (Object) null) || (!this.SurprisePoseID.HasValue || !this.CanTalk))
              break;
            switch (this._modeType)
            {
              case Desire.ActionType.EndTaskMasturbation:
                this.ReservedMode = Desire.ActionType.Normal;
                break;
              case Desire.ActionType.EndTaskToilet:
                this.ReservedMode = Desire.ActionType.Normal;
                break;
              case Desire.ActionType.EndTaskBath:
                this.ReservedMode = Desire.ActionType.GotoDressOut;
                break;
            }
            this.ChangeBehavior(Desire.ActionType.FoundPeeping);
            break;
          }
          PlayerActor player2 = Singleton<Manager.Map>.Instance.Player;
          if (this.Mode != Desire.ActionType.Encounter)
          {
            if (this.UseNeckLook)
            {
              if (this.IsCloseToPlayer && this._prevCloseToPlayer)
              {
                Transform transform = ((Component) player2.ChaControl.cmpBoneBody.targetEtc.trfHeadParent).get_transform();
                this.ChaControl.ChangeLookEyesTarget(1, transform, 0.5f, 0.0f, 1f, 2f);
                this.ChaControl.ChangeLookEyesPtn(1);
                this.ChaControl.ChangeLookNeckTarget(1, transform, 0.5f, 0.0f, 1f, 0.8f);
                this.ChaControl.ChangeLookNeckPtn(1, 1f);
              }
              else if (this.ReleasableCommand && this.IsFarPlayer && this._prevCloseToPlayer)
              {
                this.ChaControl.ChangeLookEyesPtn(0);
                this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
                this.ChaControl.ChangeLookNeckPtn(3, 1f);
                this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
              }
            }
            else
            {
              this.ChaControl.ChangeLookEyesPtn(0);
              this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
              this.ChaControl.ChangeLookNeckPtn(3, 1f);
              this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
            }
          }
          if (this.IsCloseToPlayer && !this._prevCloseToPlayer && (this.CanTalk && Singleton<Resources>.Instance.AgentProfile.EncounterWhitelist.Contains(this._modeType)) && this.BehaviorResources.Mode != Desire.ActionType.Idle)
          {
            if (this.IsTraverseOffMeshLink())
              break;
            this._prevCloseToPlayer = true;
            this.ChangeBehavior(Desire.ActionType.Encounter);
            Debug.Log((object) string.Format("UpdateEncounter ChangeBehavior Encounter proc frame : {0}", (object) Time.get_frameCount()));
            break;
          }
          if (!this.ReleasableCommand || !this.IsFarPlayer || !this._prevCloseToPlayer)
            break;
          this._prevCloseToPlayer = false;
          break;
      }
    }

    [DebuggerHidden]
    protected override IEnumerator LoadCharAsync(string fileName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CLoadCharAsync\u003Ec__Iterator4()
      {
        fileName = fileName,
        \u0024this = this
      };
    }

    protected void LoadChar(string fileName)
    {
      ChaFileControl _chaFile;
      if (!fileName.IsNullOrEmpty())
      {
        _chaFile = new ChaFileControl();
        if (!_chaFile.LoadCharaFile(fileName, (byte) 1, false, true))
          _chaFile = (ChaFileControl) null;
      }
      else
        _chaFile = (ChaFileControl) null;
      this.ChaControl = Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this).get_gameObject(), 0, _chaFile);
      this.ChaControl.Load(false);
      this.ChaControl.ChangeLookEyesPtn(3);
      this.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.Controller.InitializeFaceLight(((Component) this.ChaControl).get_gameObject());
    }

    public void RefreshWalkStatus(Waypoint[] waypoints)
    {
      this.AgentController.SearchArea.RefreshQueryPoints();
    }

    public void UpdateActionPointCollision()
    {
      Dictionary<int, CollisionState> collisionStateTable = this.ActionPointCollisionStateTable;
      float viewDistance = Singleton<Resources>.Instance.AgentProfile.ActionPointSight.ViewDistance;
      foreach (ActionPoint actionPoint in this._actionPoints)
      {
        int instanceId = actionPoint.InstanceID;
        CollisionState collisionState1;
        if (!collisionStateTable.TryGetValue(instanceId, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          collisionStateTable[instanceId] = collisionState2;
          collisionState1 = collisionState2;
        }
        bool flag = this.FovCheck(actionPoint);
        if (flag)
          flag &= actionPoint.IsReachable(this.NavMeshAgent, viewDistance);
        if (flag)
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              if (actionPoint.IsNeutralCommand)
              {
                this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Enter);
                continue;
              }
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Stay);
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.None);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Exit);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private bool HasCommands(ActionPoint point)
    {
      Actor partner = this.Partner;
      Desire.Type requestedDesire = this.RequestedDesire;
      EventType eventType = (EventType) 0;
      foreach (ValueTuple<EventType, Desire.Type> valuePair in Desire.ValuePairs)
      {
        if ((Desire.Type) valuePair.Item2 == requestedDesire)
        {
          eventType = (EventType) valuePair.Item1;
          break;
        }
      }
      return Object.op_Equality((Object) partner, (Object) null) ? point.HasAgentActionPointInfo(eventType) : point.ContainsAgentDateActionPointInfo(eventType);
    }

    private bool FovCheck(ActionPoint actionPoint)
    {
      return Singleton<Resources>.Instance.AgentProfile.ActionPointSight.HasEntered(((Component) this.AgentController).get_transform(), actionPoint.CommandCenter, actionPoint.Radius);
    }

    public static ActorController GetCapturedInSight(
      AgentActor agent,
      List<Actor> actors)
    {
      Actor element = actors.GetElement<Actor>(Random.Range(0, actors.Count));
      if (Object.op_Equality((Object) element, (Object) null))
        return (ActorController) null;
      if (!AgentActor.IsCaptureInSight(agent, element))
        return (ActorController) null;
      ActorController controller = element.Controller;
      return Object.op_Equality((Object) controller, (Object) null) ? (ActorController) null : controller;
    }

    private static bool IsCaptureInSight(AgentActor agent, Actor target)
    {
      if (Object.op_Equality((Object) target, (Object) null))
        return false;
      Vector3 position = agent.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head).get_position();
      int layer = LayerMask.NameToLayer("Map");
      foreach (Actor.FovBodyPart checkPart in AgentActor._checkParts)
      {
        Transform transform = target.FovTargetPointTable.get_Item(checkPart);
        Vector3 vector3 = Vector3.op_Subtraction(position, transform.get_position());
        Ray ray;
        ((Ray) ref ray).\u002Ector(position, vector3);
        if (Physics.Raycast(ray, ((Vector3) ref vector3).get_magnitude(), 1 << layer))
          return false;
      }
      return true;
    }

    public void UpdateActorSightCollision()
    {
      Dictionary<int, CollisionState> collisionStateTable = this.ActorCollisionStateTable;
      foreach (Actor targetActor in this.TargetActors)
      {
        int instanceId = targetActor.InstanceID;
        CollisionState collisionState1;
        if (!collisionStateTable.TryGetValue(instanceId, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          collisionStateTable[instanceId] = collisionState2;
          collisionState1 = collisionState2;
        }
        if (this.FovCheck(targetActor))
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Enter);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Stay);
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.None);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Exit);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void SetCollisionState(
      Dictionary<int, CollisionState> table,
      int id,
      CollisionState state)
    {
      table[id] = state;
    }

    private bool FovCheck(Actor actor)
    {
      Transform transform = ((Component) this.AgentController).get_transform();
      AgentProfile.SightSetting characterFarSight = Singleton<Resources>.Instance.AgentProfile.CharacterFarSight;
      AgentProfile.SightSetting characterNearSight = Singleton<Resources>.Instance.AgentProfile.CharacterNearSight;
      return characterFarSight.HasEntered(transform, actor.Position) || characterNearSight.HasEntered(transform, actor.Position);
    }

    public void UpdateActorFarSightCollision()
    {
      Dictionary<int, CollisionState> collisionStateTable = this.ActorFarCollisionStateTable;
      foreach (Actor targetActor in this.TargetActors)
      {
        int instanceId = targetActor.InstanceID;
        CollisionState collisionState1;
        if (!collisionStateTable.TryGetValue(this.InstanceID, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          collisionStateTable[this.InstanceID] = collisionState2;
          collisionState1 = collisionState2;
        }
        if (this.FovFarCheck(targetActor))
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Enter);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Stay);
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.None);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Exit);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private bool FovFarCheck(Actor actor)
    {
      return Singleton<Resources>.Instance.AgentProfile.CharacterFarSight.HasEntered(((Component) this.AgentController).get_transform(), actor.Position);
    }

    public void UpdateAnimalSightCollision()
    {
      Dictionary<int, CollisionState> collisionStateTable = this.AnimalCollisionStateTable;
      foreach (AnimalBase targetAnimal in this.TargetAnimals)
      {
        int instanceId = targetAnimal.InstanceID;
        CollisionState collisionState1;
        if (!collisionStateTable.TryGetValue(this.InstanceID, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          collisionStateTable[this.InstanceID] = collisionState2;
          collisionState1 = collisionState2;
        }
        if (this.FovCheck(targetAnimal))
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Enter);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Stay);
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.None);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(collisionStateTable, instanceId, CollisionState.Exit);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private bool FovCheck(AnimalBase animal)
    {
      return !Object.op_Equality((Object) animal, (Object) null) && !Object.op_Equality((Object) ((Component) animal).get_transform(), (Object) null) && Singleton<Resources>.Instance.AgentProfile.AnimalSight.HasEntered(((Component) this.AgentController).get_transform(), animal.Position, this.AnimalFovAngleOffsetY);
    }

    public void AddActor(Actor actor)
    {
      if (this.TargetActors.Contains(actor))
        return;
      this.TargetActors.Add(actor);
    }

    public bool ExistsInvader()
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      CollisionState collisionState;
      return (double) Vector3.Distance(player.Position, this.Position) < (double) Singleton<Resources>.Instance.LocomotionProfile.AccessInvasionRange && this.ActorCollisionStateTable.TryGetValue(player.InstanceID, out collisionState) && (collisionState == CollisionState.Enter || collisionState == CollisionState.Stay);
    }

    public void RemoveActor(Actor actor)
    {
      if (!this.TargetActors.Contains(actor))
        return;
      this.TargetActors.Remove(actor);
    }

    public void AddActionPoint(ActionPoint point)
    {
      if (!this._actionPoints.Contains(point))
        this._actionPoints.Add(point);
      this._actionPointCheckFlagTable[point] = false;
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      foreach (ActionPoint actionPoint in this._actionPoints)
        toRelease.Add(actionPoint);
      this.SearchTargets = toRelease.ToArray();
      point.Actor = (Actor) this;
      ListPool<ActionPoint>.Release(toRelease);
    }

    public void RemoveActionPoint(ActionPoint point)
    {
      if (this._actionPoints.Contains(point))
        this._actionPoints.Remove(point);
      List<ActionPoint> toRelease = ListPool<ActionPoint>.Get();
      foreach (ActionPoint actionPoint in this._actionPoints)
        toRelease.Add(actionPoint);
      this.SearchTargets = toRelease.ToArray();
      point.Actor = (Actor) null;
      ListPool<ActionPoint>.Release(toRelease);
    }

    public void AddAnimal(AnimalBase animal)
    {
      if (Object.op_Equality((Object) animal, (Object) null) || this.TargetAnimals.Contains(animal))
        return;
      this.TargetAnimals.Add(animal);
    }

    public void RemoveAnimal(AnimalBase animal)
    {
      if (Object.op_Equality((Object) animal, (Object) null))
        return;
      if (this.TargetAnimals.Contains(animal))
        this.TargetAnimals.Remove(animal);
      if (!this.AnimalCollisionStateTable.ContainsKey(animal.InstanceID))
        return;
      this.AnimalCollisionStateTable.Remove(animal.InstanceID);
    }

    private void ActivatePairing(int id, bool isDate)
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.Partner = (Actor) this;
      this.BehaviorResources.ChangeMode(Desire.ActionType.Date);
      this.Mode = Desire.ActionType.Date;
      this.AgentData.CarryingItem = (StuffItem) null;
      this.Partner = (Actor) player;
      player.Mode = Desire.ActionType.Date;
      Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RefreshCommands();
    }

    public void DeactivatePairing(int id)
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.Partner = (Actor) null;
      AgentBehaviorTree current = this.BehaviorResources.Current;
      this.BehaviorResources.ChangeMode(Desire.ActionType.Normal);
      this.Mode = Desire.ActionType.Normal;
      this.Partner = (Actor) null;
      player.Mode = Desire.ActionType.Normal;
      player.PlayerData.DateEatTrigger = false;
      Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea.RefreshCommands();
    }

    private Vector3 LocalTargetPosition { get; set; }

    public Quaternion LocalTargetRotation { get; set; }

    public void ActivateHoldingHands(int id, bool enabled)
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      HandsHolder handsHolder = player.HandsHolder;
      handsHolder.RightHandAnimator = this.ChaControl.animBody;
      handsHolder.RightHandIK = this.ChaControl.fullBodyIK;
      if (Object.op_Inequality((Object) handsHolder.RightHandTarget, (Object) null))
      {
        if (enabled)
        {
          this.LocalTargetPosition = handsHolder.TargetTransform.get_localPosition();
          this.LocalTargetRotation = handsHolder.TargetTransform.get_localRotation();
        }
        else
        {
          handsHolder.TargetTransform.set_localPosition(this.LocalTargetPosition);
          handsHolder.TargetTransform.set_localRotation(this.LocalTargetRotation);
        }
      }
      GameObject loop1 = ((Component) this.ChaControl.animBody).get_transform().FindLoop("cf_J_Kosi02");
      handsHolder.RightLookTarget = loop1.get_transform();
      if (!Singleton<Resources>.Instance.LocomotionProfile.HoldingElboTarget.IsNullOrEmpty())
      {
        GameObject loop2 = ((Component) this.ChaControl.animBody).get_transform().FindLoop(Singleton<Resources>.Instance.LocomotionProfile.HoldingElboTarget);
        if (Object.op_Inequality((Object) loop2, (Object) null))
          handsHolder.RightElboTarget = loop2.get_transform();
      }
      Vector3 offsetInParty = Singleton<Resources>.Instance.AgentProfile.GetOffsetInParty(this.ChaControl.GetShapeBodyValue(0));
      handsHolder.MinDistance = ((Vector3) ref offsetInParty).get_magnitude();
      ((Behaviour) handsHolder).set_enabled(enabled);
      handsHolder.EnabledHolding = enabled;
      if (!enabled)
        player.OldEnabledHoldingHand = false;
      string height1ParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.Height1ParameterName;
      float shapeBodyValue = this.ChaControl.GetShapeBodyValue(0);
      foreach (AnimatorControllerParameter parameter in player.Animation.Animator.get_parameters())
      {
        if (parameter.get_type() == 1 && parameter.get_name() == height1ParameterName)
          player.Animation.Animator.SetFloat(height1ParameterName, shapeBodyValue);
      }
    }

    public void ActivateTransfer(bool move = true)
    {
      int id = this.AgentData.SickState.ID;
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Weather weather = Singleton<Manager.Map>.Instance.Simulator.Weather;
      EquipEventItemInfo itemInfo = (EquipEventItemInfo) null;
      PlayState info;
      this.LoadLocomotionAnimation(this, id, weather, out info, ref itemInfo);
      if (info == null)
        return;
      this.ResetEquipEventItem(itemInfo);
      ActorAnimation animation = this.Animation;
      animation.InitializeStates(info);
      if (Object.op_Equality((Object) animation.Animator, (Object) null))
        return;
      AnimatorStateInfo animatorStateInfo = animation.Animator.GetCurrentAnimatorStateInfo(0);
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
          if ((double) this.Animation.Animator.GetLayerWeight(info.MaskStateInfo.layer) == 0.0)
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
      if (!move)
        return;
      if (this.NavMeshAgent.get_isStopped())
        this.NavMeshAgent.set_isStopped(false);
      if (!this.IsKinematic)
        return;
      this.IsKinematic = false;
    }

    private void LoadLocomotionAnimation(
      AgentActor agent,
      int sickID,
      Weather weather,
      out PlayState info,
      ref EquipEventItemInfo itemInfo)
    {
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      StuffItem carryingItem = agent.AgentData.CarryingItem;
      if (carryingItem != null && !agentProfile.CanStandEatItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == carryingItem.CategoryID && pair.itemID == carryingItem.ID)))
      {
        int cookMoveLocoId = agentProfile.PoseIDTable.CookMoveLocoID;
        info = Singleton<Resources>.Instance.Animation.AgentLocomotionStateTable[cookMoveLocoId];
        int obonEventItemId = locomotionProfile.ObonEventItemID;
        ActionItemInfo eventItem = Singleton<Resources>.Instance.Map.EventItemList[obonEventItemId];
        string agentOtherParentName = this.LocomotionProfile.AgentOtherParentName;
        itemInfo = new EquipEventItemInfo()
        {
          EventItemID = obonEventItemId,
          ActionItemInfo = eventItem,
          ParentName = agentOtherParentName
        };
      }
      else if (sickID != 3)
      {
        if (sickID == 4)
        {
          int hurtLocoId = agentProfile.PoseIDTable.HurtLocoID;
          info = Singleton<Resources>.Instance.Animation.AgentLocomotionStateTable[hurtLocoId];
        }
        else
        {
          StuffItem equipedUmbrellaItem = agent.AgentData.EquipedUmbrellaItem;
          CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
          if (equipedUmbrellaItem != null && equipedUmbrellaItem.CategoryID == itemIdDefine.UmbrellaID.categoryID && equipedUmbrellaItem.ID == itemIdDefine.UmbrellaID.itemID)
          {
            if (weather == Weather.Rain || weather == Weather.Storm)
            {
              int umbrellaLocoId = agentProfile.PoseIDTable.UmbrellaLocoID;
              info = Singleton<Resources>.Instance.Animation.AgentLocomotionStateTable[umbrellaLocoId];
              ItemIDKeyPair umbrellaId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.UmbrellaID;
              itemInfo = Singleton<Resources>.Instance.GameInfo.CommonEquipEventItemTable[umbrellaId.categoryID][umbrellaId.itemID];
              itemInfo.ParentName = locomotionProfile.AgentLocoItemParentName;
            }
            else
              this.LoadLocoAnimationOtherCond(weather, out info, ref itemInfo);
          }
          else
            this.LoadLocoAnimationOtherCond(weather, out info, ref itemInfo);
        }
      }
      else
      {
        int locomotionId = agentProfile.PoseIDTable.LocomotionID;
        info = Singleton<Resources>.Instance.Animation.AgentLocomotionStateTable[locomotionId];
      }
    }

    private void LoadLocoAnimationOtherCond(
      Weather weather,
      out PlayState info,
      ref EquipEventItemInfo itemInfo)
    {
      Resources instance = Singleton<Resources>.Instance;
      switch (this.Mode)
      {
        case Desire.ActionType.EndTaskGift:
        case Desire.ActionType.EndTaskH:
          int mojimojiLocoId = instance.AgentProfile.PoseIDTable.MojimojiLocoID;
          info = instance.Animation.AgentLocomotionStateTable[mojimojiLocoId];
          break;
        default:
          this.LoadLocoAnimationDefault(out info);
          break;
      }
    }

    private void LoadLocoAnimationDefault(out PlayState info)
    {
      Resources instance = Singleton<Resources>.Instance;
      int key = !instance.AgentProfile.ScrollWhitelist.Contains(this.Mode) ? instance.AgentProfile.PoseIDTable.LocomotionID : instance.AgentProfile.PoseIDTable.WalkLocoID;
      instance.Animation.AgentLocomotionStateTable.TryGetValue(key, out info);
    }

    public void ResetLocomotionAnimation(bool move = true)
    {
      this.ActivateTransfer(move);
    }

    public void UpdateLocomotionSpeed(Waypoint destination)
    {
      int tutorialProgress;
      if (this.TutorialMode && ((tutorialProgress = Manager.Map.GetTutorialProgress()) == 14 || tutorialProgress == 15) && (this.TutorialLocomoCaseID == 100 || this.TutorialLocomoCaseID == 101))
      {
        LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
        this.NavMeshAgent.set_speed(Mathf.Lerp(this.NavMeshAgent.get_speed(), this.TutorialLocomoCaseID != 100 ? locomotionProfile.AgentSpeed.tutorialRunSpeed : locomotionProfile.AgentSpeed.tutorialWalkSpeed, locomotionProfile.TutorialLerpSpeed));
      }
      else
      {
        float speed1 = this.NavMeshAgent.get_speed();
        int id = this.AgentData.SickState.ID;
        Weather weather = Singleton<Manager.Map>.Instance.Simulator.Weather;
        float speed2;
        this.LoadLocomotionSpeed(destination, id, weather, out speed2);
        LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
        this.NavMeshAgent.set_speed(Mathf.Lerp(speed1, speed2, locomotionProfile.LerpSpeed));
      }
    }

    private void LoadLocomotionSpeed(
      Waypoint destination,
      int sickID,
      Weather weather,
      out float speed)
    {
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      StuffItem carryingItem = this.AgentData.CarryingItem;
      if (carryingItem != null && !agentProfile.CanStandEatItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == carryingItem.CategoryID && pair.itemID == carryingItem.ID)))
        speed = Singleton<Resources>.Instance.LocomotionProfile.AgentSpeed.walkSpeed;
      else if (sickID == 3 || sickID == 4)
      {
        speed = Singleton<Resources>.Instance.LocomotionProfile.AgentSpeed.walkSpeed;
      }
      else
      {
        StuffItem equipedUmbrellaItem = this.AgentData.EquipedUmbrellaItem;
        CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
        if (equipedUmbrellaItem != null && equipedUmbrellaItem.CategoryID == itemIdDefine.UmbrellaID.categoryID && equipedUmbrellaItem.ID == itemIdDefine.UmbrellaID.itemID)
        {
          if (weather == Weather.Rain || weather == Weather.Storm)
            speed = Singleton<Resources>.Instance.LocomotionProfile.AgentSpeed.walkSpeed;
          else
            this.LoadLocomotionSpeedOtherCondition(destination, sickID, weather, out speed);
        }
        else
          this.LoadLocomotionSpeedOtherCondition(destination, sickID, weather, out speed);
      }
    }

    private void LoadLocomotionSpeedOtherCondition(
      Waypoint destination,
      int sickID,
      Weather weather,
      out float speed)
    {
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      switch (this.Mode)
      {
        case Desire.ActionType.EndTaskGift:
        case Desire.ActionType.EndTaskH:
          speed = locomotionProfile.AgentSpeed.walkSpeed;
          break;
        default:
          if (weather == Weather.Rain || weather == Weather.Storm)
          {
            speed = locomotionProfile.AgentSpeed.runSpeed;
            break;
          }
          if (this.Mode == Desire.ActionType.Normal)
          {
            if (Object.op_Inequality((Object) this.MapArea, (Object) null) && Object.op_Inequality((Object) destination, (Object) null) && Object.op_Inequality((Object) destination.OwnerArea, (Object) null))
            {
              if (destination.OwnerArea.AreaID == this.MapArea.AreaID)
              {
                speed = locomotionProfile.AgentSpeed.walkSpeed;
                break;
              }
              speed = locomotionProfile.AgentSpeed.runSpeed;
              break;
            }
            speed = locomotionProfile.AgentSpeed.walkSpeed;
            break;
          }
          float num = this.AgentData.StatsTable[5] * Singleton<Resources>.Instance.AgentProfile.MustRunMotivationPercent;
          float? motivation = this.GetMotivation(Desire.GetDesireKey(this.RequestedDesire));
          if (motivation.HasValue && (double) motivation.Value < (double) num)
          {
            speed = locomotionProfile.AgentSpeed.runSpeed;
            break;
          }
          if (Object.op_Inequality((Object) this.MapArea, (Object) null))
          {
            int areaId = this.MapArea.AreaID;
            if (Object.op_Inequality((Object) this.TargetInSightActionPoint, (Object) null) && Object.op_Inequality((Object) this.TargetInSightActionPoint.OwnerArea, (Object) null))
            {
              if (this.TargetInSightActionPoint.OwnerArea.AreaID == areaId)
              {
                speed = locomotionProfile.AgentSpeed.walkSpeed;
                break;
              }
              speed = locomotionProfile.AgentSpeed.runSpeed;
              break;
            }
            if (Object.op_Inequality((Object) destination, (Object) null) && Object.op_Inequality((Object) destination.OwnerArea, (Object) null))
            {
              if (destination.OwnerArea.AreaID == areaId)
              {
                speed = locomotionProfile.AgentSpeed.walkSpeed;
                break;
              }
              speed = locomotionProfile.AgentSpeed.runSpeed;
              break;
            }
            speed = locomotionProfile.AgentSpeed.walkSpeed;
            break;
          }
          speed = locomotionProfile.AgentSpeed.walkSpeed;
          break;
      }
    }

    public void ChangeBehavior(Desire.ActionType type)
    {
      this.PrevMode = this.Mode;
      if (this.Mode != Desire.ActionType.Normal)
      {
        this.PrevActionMode = this.Mode;
        if (this.PrevActionMode != Desire.ActionType.EndTaskWildAnimal)
          this.AgentData.CheckCatEvent = false;
      }
      this.Mode = type;
      if (this.AgentData.CarryingItem != null && type != Desire.ActionType.Encounter && (type != Desire.ActionType.SearchEatSpot && type != Desire.ActionType.EndTaskEat) && type != Desire.ActionType.EndTaskEatThere)
        this.AgentData.CarryingItem = (StuffItem) null;
      if (type == Desire.ActionType.Normal)
      {
        int num = -1;
        this.PoseID = num;
        this.ActionID = num;
        this.StateType = AIProject.Definitions.State.Type.Normal;
        this.EventKey = (EventType) 0;
        this.TargetInSightActionPoint = (ActionPoint) null;
        this.TargetInSightActor = (Actor) null;
        this.Partner = (Actor) null;
        this.CommandPartner = (Actor) null;
        this.ResetActionFlag();
        if (this._schedule.enabled)
          this._schedule.enabled = false;
        if (this.IsHealthManager)
          this.SetDesire(Desire.GetDesireKey(Desire.Type.Break), 70f);
        this.ActivateNavMeshAgent();
        this.ActivateTransfer(true);
      }
      this.BehaviorResources.ChangeMode(type);
    }

    public void ChangeReservedBehavior()
    {
      this.ChangeBehavior(this.ReservedMode);
    }

    public void StartWeakness()
    {
      this.AgentData.WeaknessMotivation = this.AgentData.StatsTable[5];
      this.ChangeBehavior(Desire.ActionType.WeaknessA);
      this.CommandPartner = (Actor) null;
    }

    public void ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType type)
    {
      AIProject.Definitions.Tutorial.ActionType tutorialType = this.TutorialType;
      AIProject.Definitions.Tutorial.ActionType actionType = type;
      this.AgentData.TutorialModeType = actionType;
      this.TutorialType = actionType;
      if (Object.op_Equality((Object) this.TutorialBehaviorResources, (Object) null))
        return;
      if (this.TutorialType != tutorialType)
      {
        CommandArea commandArea = !Singleton<Manager.Map>.IsInstance() ? (CommandArea) null : Singleton<Manager.Map>.Instance.Player?.PlayerController?.CommandArea;
        if (Object.op_Inequality((Object) commandArea, (Object) null) && commandArea.ContainsConsiderationObject((ICommandable) this))
          commandArea.RefreshCommands();
      }
      this.TutorialBehaviorResources.ChangeMode(type);
    }

    public void CreateTutorialBehaviorResources()
    {
      if (!Object.op_Equality((Object) this.TutorialBehaviorResources, (Object) null))
        return;
      Transform transform = new GameObject("TutorialBehaviorResources").get_transform();
      transform.SetParent(((Component) this).get_transform(), false);
      this.TutorialBehaviorResources = ((Component) transform).GetOrAddComponent<TutorialBehaviorTreeResources>();
      this.TutorialBehaviorResources.Initialize(this);
    }

    public void ChangeFirstTutorialBehavior()
    {
      if (Object.op_Inequality((Object) this.BehaviorResources, (Object) null) && ((Behaviour) this.BehaviorResources).get_enabled())
        ((Behaviour) this.BehaviorResources).set_enabled(false);
      this.ChangeBehavior(Desire.ActionType.Idle);
      this.PrevMode = Desire.ActionType.Idle;
      this.TutorialMode = true;
      AgentProfile.TutorialSetting tutorial;
      if (Singleton<Resources>.IsInstance() && (tutorial = Singleton<Resources>.Instance.AgentProfile.Tutorial) != null)
      {
        AssetBundleInfo outInfo = (AssetBundleInfo) null;
        RuntimeAnimatorController charaAnimator = Singleton<Resources>.Instance.Animation.GetCharaAnimator(tutorial.AnimatorID, ref outInfo);
        this.Animation.AnimABInfo = outInfo;
        this.ChangeAnimator(charaAnimator);
        this.PlayTutorialDefaultStateAnim();
      }
      this.ChangeTutorialBehavior(this.AgentData.TutorialModeType);
      CommandArea commandArea = Manager.Map.GetCommandArea();
      if (!Object.op_Inequality((Object) commandArea, (Object) null) || !commandArea.ContainsConsiderationObject((ICommandable) this))
        return;
      commandArea.RefreshCommands();
    }

    public void ChangeFirstNormalBehavior()
    {
      if (Object.op_Inequality((Object) this.TutorialBehaviorResources, (Object) null) && Object.op_Inequality((Object) ((Component) this.TutorialBehaviorResources).get_gameObject(), (Object) null))
      {
        Object.Destroy((Object) ((Component) this.TutorialBehaviorResources).get_gameObject());
        this.TutorialBehaviorResources = (TutorialBehaviorTreeResources) null;
      }
      this.EnableBehavior();
      AssetBundleInfo outInfo = (AssetBundleInfo) null;
      RuntimeAnimatorController charaAnimator = Singleton<Resources>.Instance.Animation.GetCharaAnimator(0, ref outInfo);
      if (Object.op_Inequality((Object) charaAnimator, (Object) null))
      {
        this.Animation.AnimABInfo = outInfo;
        this.ChangeAnimator(charaAnimator);
      }
      this.TutorialMode = false;
      AIProject.Definitions.Tutorial.ActionType actionType = AIProject.Definitions.Tutorial.ActionType.End;
      this.AgentData.TutorialModeType = actionType;
      this.TutorialType = actionType;
      this.ActivateNavMeshAgent();
      this.ChangeBehavior(Desire.ActionType.Normal);
    }

    public void PlayTutorialDefaultStateAnim()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      PlayState.AnimStateInfo idleStateInfo = Singleton<Resources>.Instance.DefinePack?.AnimatorState?.IdleStateInfo;
      if (idleStateInfo == null)
        return;
      Animator animator = this.Animation.Animator;
      if (!((Behaviour) this.Animation).get_isActiveAndEnabled() || !((Behaviour) animator).get_isActiveAndEnabled())
        return;
      animator.CrossFadeInFixedTime(((IEnumerable<PlayState.Info>) idleStateInfo.StateInfos).LastOrDefault<PlayState.Info>().stateName ?? "Locomotion", idleStateInfo.FadeSecond, 0, 0.0f);
    }

    public PlayState.AnimStateInfo GetTutorialPersonalIdleState()
    {
      if (!Singleton<Resources>.IsInstance())
        return (PlayState.AnimStateInfo) null;
      Dictionary<int, PoseKeyPair> tutorialIdlePoseTable = Singleton<Resources>.Instance.AgentProfile.TutorialIdlePoseTable;
      if (tutorialIdlePoseTable.IsNullOrEmpty<int, PoseKeyPair>())
        return Singleton<Resources>.Instance.DefinePack.AnimatorState.IdleStateInfo;
      int charaId = this.charaID;
      PoseKeyPair poseKeyPair;
      Dictionary<int, PlayState> dictionary;
      PlayState playState;
      return !tutorialIdlePoseTable.TryGetValue(charaId, out poseKeyPair) && !tutorialIdlePoseTable.TryGetValue(0, out poseKeyPair) || (!Singleton<Resources>.Instance.Animation.AgentActionAnimTable.TryGetValue(poseKeyPair.postureID, out dictionary) || !dictionary.TryGetValue(poseKeyPair.poseID, out playState)) || playState == null ? Singleton<Resources>.Instance.DefinePack.AnimatorState.IdleStateInfo : playState.MainStateInfo.InStateInfo;
    }

    public void ElectNextPoint()
    {
      EventType ev;
      if (!Singleton<Resources>.Instance.AgentProfile.AfterActionTable.TryGetValue(this.EventKey, ref ev))
        return;
      Chunk chunk;
      Singleton<Manager.Map>.Instance.ChunkTable.TryGetValue(this.ChunkID, out chunk);
      if (!Object.op_Implicit((Object) chunk))
        return;
      ActionPoint actionPoint = (ActionPoint) null;
      float? distance = new float?();
      this.NearestActionPoint(chunk.ActionPoints, ev, ref actionPoint, ref distance);
      this.NearestActionPoint(chunk.AppendActionPoints, ev, ref actionPoint, ref distance);
      if (!Object.op_Inequality((Object) actionPoint, (Object) null))
        return;
      this.NextPoint = actionPoint;
    }

    private void NearestActionPoint(
      List<ActionPoint> actionPoints,
      EventType ev,
      ref ActionPoint actionPoint,
      ref float? distance)
    {
      foreach (ActionPoint actionPoint1 in actionPoints)
      {
        if (actionPoint1.HasAgentActionPointInfo(ev))
        {
          float num = Vector3.Distance(actionPoint1.Position, this.Position);
          if (!distance.HasValue)
          {
            actionPoint = actionPoint1;
            distance = new float?(num);
          }
          else if ((double) num < (double) distance.Value)
          {
            actionPoint = actionPoint1;
            distance = new float?(num);
          }
        }
      }
    }

    public override void OnMinuteUpdated(TimeSpan deltaTime)
    {
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      AgentData agentData = this.AgentData;
      AgentProfile.DiminuationRates diminuationRate = agentProfile.DiminuationRate;
      AgentProfile.DiminuationRates weaknessDiminuationRate = agentProfile.WeaknessDiminuationRate;
      AgentProfile.DiminuationRates motivationDimRate = agentProfile.TalkMotivationDimRate;
      Dictionary<int, AgentProfile.DiminuationRates> diminMotivationRate = agentProfile.DiminMotivationRate;
      if (this.Mode == Desire.ActionType.Normal || this.Mode == Desire.ActionType.CommonSearchBreak || (this.Mode == Desire.ActionType.CommonBreak || this.Mode == Desire.ActionType.CommonGameThere))
        this.UpdateDesire(deltaTime, diminMotivationRate);
      float num1 = agentData.StatsTable[5];
      if (((Behaviour) this.BehaviorResources).get_enabled())
      {
        float num2 = agentData.TalkMotivation + motivationDimRate.valueRecovery * (float) deltaTime.TotalMinutes;
        agentData.TalkMotivation = Mathf.Clamp(num2, 0.0f, num1);
      }
      if (this.UpdateMotivation)
      {
        Desire.Type key;
        if (Desire.ModeTable.TryGetValue(this.Mode, out key))
        {
          int desireKey = Desire.GetDesireKey(key);
          float? motivation = this.GetMotivation(desireKey);
          if (motivation.HasValue)
          {
            float num2 = diminMotivationRate[desireKey].value * (float) deltaTime.TotalMinutes;
            float num3 = Mathf.Clamp(motivation.Value - num2, 0.0f, num1);
            this.SetMotivation(desireKey, num3);
          }
        }
        this.MotivationInEncounter -= diminuationRate.value;
        this.MotivationInEncounter = Mathf.Clamp(this.MotivationInEncounter, 0.0f, num1);
        this.AgentData.WeaknessMotivation -= weaknessDiminuationRate.value;
        this.AgentData.WeaknessMotivation = Mathf.Clamp(this.AgentData.WeaknessMotivation, 0.0f, num1);
        this.RuntimeMotivationInPhoto -= diminuationRate.value;
        this.RuntimeMotivationInPhoto = Mathf.Clamp(this.RuntimeMotivationInPhoto, 0.0f, num1);
        Threshold diminuationInMasturbation = agentProfile.DiminuationInMasturbation;
        Threshold diminuationInLesbian = agentProfile.DiminuationInLesbian;
        this._runtimeMotivationInMasturbation -= diminuationInMasturbation.RandomValue;
        this._runtimeMotivationInLesbian -= diminuationInLesbian.RandomValue;
      }
      this._runtimeMotivationInMasturbation = Mathf.Clamp(this._runtimeMotivationInMasturbation, 0.0f, 100f);
      this._runtimeMotivationInLesbian = Mathf.Clamp(this._runtimeMotivationInLesbian, 0.0f, 100f);
      if (this.AgentController.SleepedSchedule || !((Behaviour) this.BehaviorResources).get_enabled() || (!this._schedule.enabled || !this._schedule.useGameTime) || !this._schedule.progress)
        return;
      this._schedule.elapsedTime += (float) deltaTime.TotalMinutes;
      if ((double) this._schedule.elapsedTime <= (double) this._schedule.duration)
        return;
      this._schedule.enabled = false;
    }

    public override void OnSecondUpdated(TimeSpan timeSpan)
    {
      this.OnSickUpdate(timeSpan);
    }

    public StuffItem SelectDrinkItem()
    {
      AgentData agentData = this.AgentData;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> drinkParameterTable = Singleton<Resources>.Instance.GameInfo.DrinkParameterTable;
      List<StuffItem> stuffItemList1 = ListPool<StuffItem>.Get();
      float num = agentData.StatsTable[0];
      foreach (StuffItem stuffItem in agentData.ItemList)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (drinkParameterTable.TryGetValue(stuffItem.CategoryID, out dictionary) && dictionary.TryGetValue(stuffItem.ID, out Dictionary<int, FoodParameterPacket> _))
          stuffItemList1.Add(stuffItem);
      }
      StuffItem stuffItem1 = (StuffItem) null;
      if ((double) num < (double) agentProfile.ColdTempBorder)
      {
        List<StuffItem> stuffItemList2 = ListPool<StuffItem>.Get();
        foreach (StuffItem stuffItem2 in stuffItemList1)
        {
          StuffItem stuffItem = stuffItem2;
          if (agentProfile.LowerTempDrinkItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == stuffItem.CategoryID && pair.itemID == stuffItem.ID)))
            stuffItemList2.Add(stuffItem);
        }
        if (!stuffItemList2.IsNullOrEmpty<StuffItem>())
          stuffItem1 = stuffItemList2.GetElement<StuffItem>(Random.Range(0, stuffItemList2.Count));
        ListPool<StuffItem>.Release(stuffItemList2);
      }
      else if ((double) num > (double) agentProfile.HotTempBorder)
      {
        List<StuffItem> stuffItemList2 = ListPool<StuffItem>.Get();
        foreach (StuffItem stuffItem2 in stuffItemList1)
        {
          StuffItem stuffItem = stuffItem2;
          if (agentProfile.RaiseTempDrinkItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == stuffItem.CategoryID && pair.itemID == stuffItem.ID)))
            stuffItemList2.Add(stuffItem);
        }
        if (!stuffItemList2.IsNullOrEmpty<StuffItem>())
          stuffItem1 = stuffItemList2.GetElement<StuffItem>(Random.Range(0, stuffItemList2.Count));
        ListPool<StuffItem>.Release(stuffItemList2);
      }
      if (stuffItem1 == null)
        stuffItem1 = stuffItemList1.GetElement<StuffItem>(Random.Range(0, stuffItemList1.Count));
      ListPool<StuffItem>.Release(stuffItemList1);
      return new StuffItem(stuffItem1.CategoryID, stuffItem1.ID, 1);
    }

    protected override void LoadEquipedEventItem(EquipEventItemInfo eventItemInfo)
    {
      AssetBundleInfo bundleInfo = eventItemInfo.ActionItemInfo.assetbundleInfo;
      if (((string) bundleInfo.assetbundle).IsNullOrEmpty() || ((string) bundleInfo.asset).IsNullOrEmpty() || ((string) bundleInfo.manifest).IsNullOrEmpty())
        return;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>((string) bundleInfo.assetbundle, (string) bundleInfo.asset, false, (string) bundleInfo.manifest);
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
      {
        if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == (string) bundleInfo.assetbundle && (string) x.Item2 == (string) bundleInfo.manifest)))
          MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>((string) bundleInfo.assetbundle, (string) bundleInfo.manifest));
        GameObject loop = ((Component) this.ChaControl.animBody).get_transform().FindLoop(eventItemInfo.ParentName);
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
        Debug.LogError((object) string.Format("イベントアイテム読み込み失敗： バンドルパス[{0}] プレハブ[{1}] マニフェスト[{2}]", (object) bundleInfo.assetbundle, (object) bundleInfo.asset, (object) bundleInfo.manifest));
    }

    public override void LoadEventItems(PlayState playState)
    {
      if (playState.ItemInfoCount <= 0)
        return;
      Resources instance = Singleton<Resources>.Instance;
      for (int index = 0; index < playState.ItemInfoCount; ++index)
      {
        PlayState.ItemInfo itemInfo = playState.GetItemInfo(index);
        if (itemInfo.fromEquipedItem)
        {
          ActionPointInfo actionPointInfo = this.Animation.ActionPointInfo;
          Dictionary<int, EquipEventItemInfo> dictionary;
          EquipEventItemInfo equipEventItemInfo;
          if (instance.GameInfo.SearchEquipEventItemTable.TryGetValue(actionPointInfo.searchAreaID, out dictionary) && dictionary.TryGetValue(actionPointInfo.gradeValue, out equipEventItemInfo))
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

    public void LoadEventItems(List<PlayState.ItemInfo> itemList)
    {
      Resources instance = Singleton<Resources>.Instance;
      foreach (PlayState.ItemInfo itemInfo in itemList)
      {
        if (itemInfo.fromEquipedItem)
        {
          ActionPointInfo actionPointInfo = this.Animation.ActionPointInfo;
          Dictionary<int, EquipEventItemInfo> dictionary;
          EquipEventItemInfo equipEventItemInfo;
          if (instance.GameInfo.SearchEquipEventItemTable.TryGetValue(actionPointInfo.searchAreaID, out dictionary) && dictionary.TryGetValue(actionPointInfo.gradeValue, out equipEventItemInfo))
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
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary;
      Dictionary<int, List<AnimeParticleEventInfo>> eventTable;
      if (!Singleton<Resources>.Instance.Animation.AgentActParticleEventKeyTable.TryGetValue(eventID, out dictionary) || !dictionary.TryGetValue(poseID, out eventTable) || eventTable == null)
        return;
      this.LoadEventParticle(eventTable);
    }

    public float MaxMotivationInMasturbation
    {
      get
      {
        return this._maxMotivationInMasturbation;
      }
    }

    public float RuntimeMotivationInMasturbation
    {
      get
      {
        return this._runtimeMotivationInMasturbation;
      }
    }

    public void StartMasturbationSequence(HScene.AnimationListInfo info)
    {
      IEnumerator enumerator = this._masturbationEnumerator = this.StartMasturbationSequenceCoroutine(info);
      this._masturbationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    public void StopMasturbationSequence()
    {
      if (this._masturbationDisposable == null)
        return;
      this.ChaControl.SetClothesStateAll((byte) 0);
      this.Animation.CanUseMapIK = true;
      this.Animation.MapIK.MapIK = true;
      if (this.HParticle != null)
      {
        this.HParticle.ReleaseObject();
        this.HParticle = (HParticleCtrl) null;
      }
      if (Object.op_Inequality((Object) this.hFlagCtrl, (Object) null))
      {
        if (Object.op_Inequality((Object) this.HVoiceCtrl, (Object) null))
        {
          this.hFlagCtrl.RemoveMapHvoices(this.HVoiceID);
          this.HVoiceCtrl = (HVoiceCtrl) null;
        }
        this.hFlagCtrl.EndProc();
        this.hFlagCtrl = (HSceneFlagCtrl) null;
      }
      if (this.HItem != null)
      {
        this.HItem.ReleaseItem();
        this.HItem = (HItemCtrl) null;
      }
      if (Object.op_Inequality((Object) this.HItemPlace, (Object) null))
      {
        Object.Destroy((Object) this.HItemPlace);
        this.HItemPlace = (GameObject) null;
      }
      if (Object.op_Inequality((Object) this.HLayerCtrl, (Object) null))
      {
        this.HLayerCtrl.MapHLayerRemove(this.HLayerID);
        this.HLayerCtrl = (HLayerCtrl) null;
      }
      if (this.HYureCtrl != null)
      {
        this.HYureCtrl.ResetShape(this.yureID);
        this.HYureCtrl.RemoveChaInit(this.yureID);
        this.HYureCtrl = (YureCtrl.YureCtrlMapH) null;
      }
      this._masturbationDisposable.Dispose();
      this._masturbationEnumerator = (IEnumerator) null;
      for (int index = 0; index < this.MasturbationDisposables.Count; ++index)
        this.MasturbationDisposables[index].Dispose();
      this.MasturbationDisposables.Clear();
      if (!Singleton<HSceneManager>.IsInstance())
        return;
      foreach (string assetBundleName in this.hSceneManager.hashUseAssetBundle)
        AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      this.hSceneManager.hashUseAssetBundle.Clear();
    }

    [DebuggerHidden]
    private IEnumerator StartMasturbationSequenceCoroutine(HScene.AnimationListInfo info)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CStartMasturbationSequenceCoroutine\u003Ec__Iterator5()
      {
        info = info,
        \u0024this = this
      };
    }

    public bool PlayingMasturbationSequence
    {
      get
      {
        return this._masturbationEnumerator != null;
      }
    }

    private void SetPlay(string stateName, bool isFade = true)
    {
      this.Animation.PlayAnimation(stateName, 0, 0.0f);
      if (this.Animation.MapIK.data != null)
        this.Animation.MapIK.Calc(stateName);
      this.ChaControl.setAnimatorParamFloat("height", this.height);
      this.ChaControl.setAnimatorParamFloat("breast", this.breast);
      if (!isFade)
        return;
      this.Animation.CrossFadeScreen(1f);
      if (!(stateName == "WLoop") && !(stateName == "SLoop") && !(stateName == "OLoop"))
        return;
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(1.0)), (System.Action<M0>) (_ => this.HvoiceFlagSet(0)));
    }

    private void HvoiceFlagSet(int mode = 0)
    {
      if (mode == 0)
      {
        this.hFlagCtrl.MapHvoices[this.HVoiceID].playVoices[0] = true;
        this.hFlagCtrl.MapHvoices[this.HVoiceID].playShorts[0] = 0;
      }
      else
      {
        if (mode != 1)
          return;
        this.hFlagCtrl.MapHvoices[this.HVoiceLesID].playVoices[0] = true;
        this.hFlagCtrl.MapHvoices[this.HVoiceLesID].playVoices[1] = true;
        this.hFlagCtrl.MapHvoices[this.HVoiceLesID].playShorts[0] = 0;
        this.hFlagCtrl.MapHvoices[this.HVoiceLesID].playShorts[1] = 0;
      }
    }

    private bool GotoNextLoop(float range, string nextState)
    {
      if ((double) (this._runtimeMotivationInMasturbation / this._maxMotivationInMasturbation) > (double) range)
        return false;
      this.SetPlay(nextState, true);
      return true;
    }

    public float MaxMotivationInLesbian
    {
      get
      {
        return this._maxMotivationInLesbian;
      }
    }

    public float RuntimeMotivationInLesbian
    {
      get
      {
        return this._runtimeMotivationInLesbian;
      }
    }

    public void StartLesbianSequence(Actor partner, HScene.AnimationListInfo info)
    {
      if (partner is AgentActor)
      {
        IEnumerator enumerator = this._lesbianHEnumerator = this.StartLesbianSequenceCoroutine(partner, info);
        this._lesbianHDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
        AgentActor agentActor = partner as AgentActor;
        agentActor.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
        agentActor.Mode = Desire.ActionType.Idle;
      }
      else
      {
        IEnumerator enumerator = this._lesbianHEnumerator = this.StartLesbianSequenceCoroutine(partner, info);
        this._lesbianHDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
        (partner as MerchantActor).ChangeBehavior(Merchant.ActionType.HWithAgent);
      }
    }

    public void StopLesbianSequence()
    {
      if (this._lesbianHDisposable == null)
        return;
      for (int index = 0; index < this.lesChars.Length; ++index)
        this.lesChars[index].SetClothesStateAll((byte) 0);
      for (int index = 0; index < this.lesCharAnimes.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.lesCharAnimes[index], (Object) null))
        {
          this.lesCharAnimes[index].CanUseMapIK = true;
          this.lesCharAnimes[index].MapIK.MapIK = true;
        }
      }
      if (this.HParticle != null)
      {
        this.HParticle.ReleaseObject();
        this.HParticle = (HParticleCtrl) null;
      }
      if (Object.op_Inequality((Object) this.hFlagCtrl, (Object) null))
      {
        if (Object.op_Inequality((Object) this.HVoiceCtrl, (Object) null))
        {
          this.hFlagCtrl.RemoveMapHvoices(this.HVoiceLesID);
          this.HVoiceCtrl = (HVoiceCtrl) null;
        }
        this.hFlagCtrl.EndProc();
        this.hFlagCtrl = (HSceneFlagCtrl) null;
      }
      if (Object.op_Inequality((Object) this.HLayerCtrl, (Object) null))
      {
        for (int index = 0; index < 2; ++index)
          this.HLayerCtrl.MapHLayerRemove(this.HLayerLesID[index]);
        this.HLayerCtrl = (HLayerCtrl) null;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (this.HYureCtrlLes[index] != null)
        {
          this.HYureCtrlLes[index].ResetShape(this.yureLesID[index]);
          this.HYureCtrlLes[index].RemoveChaInit(this.yureLesID[index]);
          this.HYureCtrlLes[index] = (YureCtrl.YureCtrlMapH) null;
        }
      }
      this._lesbianHDisposable.Dispose();
      this._lesbianHEnumerator = (IEnumerator) null;
      for (int index = 0; index < this.lesbianDisposable.Count; ++index)
        this.lesbianDisposable[index].Dispose();
      this.lesbianDisposable.Clear();
      if (!Singleton<HSceneManager>.IsInstance())
        return;
      foreach (string assetBundleName in this.hSceneManager.hashUseAssetBundle)
        AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      this.hSceneManager.hashUseAssetBundle.Clear();
    }

    [DebuggerHidden]
    private IEnumerator StartLesbianSequenceCoroutine(
      Actor receiver,
      HScene.AnimationListInfo info)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CStartLesbianSequenceCoroutine\u003Ec__Iterator6()
      {
        receiver = receiver,
        info = info,
        \u0024this = this
      };
    }

    public bool LivesLesbianHSequence
    {
      get
      {
        return this._lesbianHEnumerator != null;
      }
    }

    private void SetPlayLes(string stateName, bool isFade = true)
    {
      for (int index = 0; index < this.lesCharAnimes.Length; ++index)
      {
        this.lesCharAnimes[index].PlayAnimation(stateName, 0, 0.0f);
        if (this.lesCharAnimes[index].MapIK.data != null)
          this.lesCharAnimes[index].MapIK.Calc(stateName);
      }
      if (!isFade)
        return;
      this.Animation.CrossFadeScreen(1f);
      if (!(stateName == "WLoop") && !(stateName == "SLoop") && !(stateName == "OLoop"))
        return;
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(1.0)), (System.Action<M0>) (_ => this.HvoiceFlagSet(1)));
    }

    private bool GotoNextLoopLes(float range, string nextState)
    {
      if ((double) (this._runtimeMotivationInLesbian / this._maxMotivationInLesbian) > (double) range)
        return false;
      this.SetPlayLes(nextState, true);
      return true;
    }

    public override Actor.SearchInfo RandomAddItem(
      Dictionary<int, ItemTableElement> itemTable,
      bool containsNone)
    {
      Resources instance = Singleton<Resources>.Instance;
      if (itemTable == null)
        return new Actor.SearchInfo() { IsSuccess = false };
      StatusProfile statusProfile = instance.StatusProfile;
      int flavor = this.ChaControl.fileGameInfo.flavorState[3];
      Resources.GameInfoTables gameInfo = instance.GameInfo;
      int key = this.Lottery(itemTable);
      ItemTableElement itemTableElement;
      if (itemTable.TryGetValue(key, out itemTableElement))
      {
        Actor.SearchInfo searchInfo = new Actor.SearchInfo()
        {
          IsSuccess = true
        };
        foreach (ItemTableElement.GatherElement element in itemTableElement.Elements)
        {
          float num1 = Random.get_value();
          StuffItemInfo stuffItemInfo = gameInfo.GetItem(element.categoryID, element.itemID);
          float prob = element.prob;
          if (stuffItemInfo != null)
          {
            if (stuffItemInfo.Grade == Grade._3)
            {
              float num2 = AgentActor.FlavorVariation(statusProfile.DropBuffMinMax, statusProfile.DropBuff, flavor);
              prob += num2;
              if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(44))
                prob += 10f;
            }
            if ((double) num1 < (double) prob)
            {
              int num2 = Random.Range(element.minCount, element.maxCount + 1);
              if (num2 > 0)
                searchInfo.ItemList.Add(new Actor.ItemSearchInfo()
                {
                  name = stuffItemInfo.Name,
                  categoryID = element.categoryID,
                  id = element.itemID,
                  count = num2
                });
            }
          }
        }
        if (searchInfo.ItemList.IsNullOrEmpty<Actor.ItemSearchInfo>())
        {
          if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(49))
          {
            List<ItemTableElement.GatherElement> gatherElementList = ListPool<ItemTableElement.GatherElement>.Get();
            foreach (ItemTableElement.GatherElement element in itemTableElement.Elements)
            {
              if (element.categoryID == 16 || element.categoryID == 17)
                gatherElementList.Add(element);
            }
            if (!gatherElementList.IsNullOrEmpty<ItemTableElement.GatherElement>())
            {
              ItemTableElement.GatherElement gatherElement = this.Lottery(gatherElementList);
              StuffItemInfo stuffItemInfo = gameInfo.GetItem(gatherElement.categoryID, gatherElement.itemID);
              if (stuffItemInfo != null)
              {
                int num = Random.Range(Mathf.Max(gatherElement.minCount, 1), gatherElement.maxCount + 1);
                searchInfo.ItemList.Add(new Actor.ItemSearchInfo()
                {
                  name = stuffItemInfo.Name,
                  categoryID = gatherElement.categoryID,
                  id = gatherElement.itemID,
                  count = num
                });
              }
              else
                searchInfo.IsSuccess = false;
            }
            else
              searchInfo.IsSuccess = false;
            ListPool<ItemTableElement.GatherElement>.Release(gatherElementList);
          }
          else
            searchInfo.IsSuccess = false;
        }
        return searchInfo;
      }
      return new Actor.SearchInfo() { IsSuccess = false };
    }

    public override void OnDayUpdated(TimeSpan timeSpan)
    {
      this.SetAdvEventLimitationResetReserve();
    }

    public void SetAdvEventLimitationResetReserve()
    {
      if (this.advEventResetDisposable != null)
        this.advEventResetDisposable.Dispose();
      this.advEventResetDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (System.Action<M0>) (_ =>
      {
        if (!Singleton<Manager.Map>.Instance.IsHour7After)
          return;
        this.AgentData.advEventLimitation.Remove(6);
        if (this.advEventResetDisposable != null)
          this.advEventResetDisposable.Dispose();
        this.advEventResetDisposable = (IDisposable) null;
      }));
    }

    public Waypoint[] GetReservedWaypoints()
    {
      return this._reservedWaypoints.ToArray();
    }

    public Queue<Waypoint> WalkRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedNearActionWaypoints()
    {
      return this._reservedNearActionWaypoints.ToArray();
    }

    public void ClearReservedNearActionWaypoints()
    {
      this._reservedNearActionWaypoints.Clear();
    }

    public Queue<Waypoint> SearchActionRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedDateNearActionWaypoints()
    {
      return this._reservedDateNearActionWaypoints.ToArray();
    }

    public Queue<Waypoint> SearchDateActionRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedLocationWaypoints()
    {
      return this._reservedLocationWaypoints.ToArray();
    }

    public Queue<Waypoint> SearchLocationRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedActorWaypoints()
    {
      return this._reservedActorWaypoints.ToArray();
    }

    public Queue<Waypoint> SearchActorRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedPlayerWaypoints()
    {
      return this._reservedPlayerWaypoints.ToArray();
    }

    public Queue<Waypoint> SearchPlayerRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint[] GetReservedAnimalWaypoints()
    {
      return this._reservedAnimalWaypoints.ToArray();
    }

    public Queue<Waypoint> SearchAnimalRoute { get; private set; } = new Queue<Waypoint>();

    public Waypoint DestWaypoint { get; private set; }

    public bool IsRunning { get; set; }

    private bool IsTraverseOffMeshLink()
    {
      if (this.NavMeshAgent.get_isOnOffMeshLink())
        return true;
      OffMeshLinkData currentOffMeshLinkData1 = this.NavMeshAgent.get_currentOffMeshLinkData();
      if (((OffMeshLinkData) ref currentOffMeshLinkData1).get_valid())
        return true;
      OffMeshLinkData currentOffMeshLinkData2 = this.NavMeshAgent.get_currentOffMeshLinkData();
      if (Object.op_Inequality((Object) ((OffMeshLinkData) ref currentOffMeshLinkData2).get_offMeshLink(), (Object) null))
      {
        OffMeshLinkData currentOffMeshLinkData3 = this.NavMeshAgent.get_currentOffMeshLinkData();
        if (!((ActionPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData3).get_offMeshLink()).GetComponent<ActionPoint>()).IsNeutralCommand)
          return true;
      }
      return this.EventKey == EventType.Move;
    }

    public void SetOriginalDestination()
    {
      if (Object.op_Equality((Object) this.DestWaypoint, (Object) null))
        return;
      this.SetDestinationByDirectPathForce(this.DestWaypoint);
    }

    private bool AnyWaypoint(List<Waypoint> list, Waypoint pt)
    {
      foreach (Object @object in list)
      {
        if (Object.op_Equality(@object, (Object) pt))
          return true;
      }
      return false;
    }

    public void LoadWaypoints()
    {
      Chunk chunk = (Chunk) null;
      foreach (KeyValuePair<int, Chunk> keyValuePair in Singleton<Manager.Map>.Instance.ChunkTable)
      {
        if (keyValuePair.Value.ID == this.AgentData.ChunkID)
        {
          chunk = keyValuePair.Value;
          break;
        }
      }
      foreach (int reservedWaypointId in this.AgentData.ReservedWaypointIDList)
      {
        Waypoint waypoint1 = (Waypoint) null;
        foreach (MapArea mapArea in chunk.MapAreas)
        {
          foreach (Waypoint waypoint2 in mapArea.Waypoints)
          {
            if (waypoint2.ID == reservedWaypointId)
            {
              waypoint1 = waypoint2;
              break;
            }
          }
          if (Object.op_Inequality((Object) waypoint1, (Object) null))
            break;
        }
        if (Object.op_Inequality((Object) waypoint1, (Object) null))
          this._reservedWaypoints.Add(waypoint1);
      }
      foreach (int walkRouteWaypointId in this.AgentData.WalkRouteWaypointIDList)
      {
        Waypoint waypoint1 = (Waypoint) null;
        foreach (MapArea mapArea in chunk.MapAreas)
        {
          foreach (Waypoint waypoint2 in mapArea.Waypoints)
          {
            if (waypoint2.ID == walkRouteWaypointId)
            {
              waypoint1 = waypoint2;
              break;
            }
          }
          if (Object.op_Inequality((Object) waypoint1, (Object) null))
            break;
        }
        if (Object.op_Inequality((Object) waypoint1, (Object) null))
          this._reservedWaypoints.Add(waypoint1);
      }
    }

    public void AbortCalc()
    {
      if (this._calcEnumerator == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._calcEnumerator);
      this._calcEnumerator = (IEnumerator) null;
    }

    private bool ValidatePoints(
      Waypoint point,
      int chunkID,
      Weather weather,
      bool checkWeather = true,
      bool checkChunk = true)
    {
      return !Object.op_Equality((Object) point, (Object) null) && !Object.op_Equality((Object) point.OwnerArea, (Object) null) && (!checkWeather || this.WeatherCheck(weather, (Point) point)) && ((!checkChunk || point.OwnerArea.ChunkID == chunkID) && (point.Reserver == null || point.Reserver == this));
    }

    private bool AvailableAreaCheck(
      MapArea area,
      Dictionary<int, bool> availableArea,
      Manager.Map mapManager)
    {
      if (!Object.op_Inequality((Object) area, (Object) null) || area.ChunkID == this.ChunkID)
        return true;
      bool flag;
      if (!availableArea.TryGetValue(area.AreaID, out flag))
        availableArea[area.AreaID] = flag = mapManager.CheckAvailableMapArea(area.AreaID);
      return flag;
    }

    [DebuggerHidden]
    public IEnumerator CalculateWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateWaypoints\u003Ec__Iterator7()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculatePath(List<Waypoint> points, int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculatePath\u003Ec__Iterator8()
      {
        points = points,
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculatePath(List<Waypoint> points, List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculatePath\u003Ec__Iterator9()
      {
        points = points,
        list = list,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateNearActionWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateNearActionWaypoints\u003Ec__IteratorA()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateNearActionWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateNearActionWaypointsAdditive\u003Ec__IteratorB()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateNearActionPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateNearActionPath\u003Ec__IteratorC()
      {
        points = points,
        count = count,
        actionPoints = actionPoints,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateNearActionPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateNearActionPath\u003Ec__IteratorD()
      {
        actionPoints = actionPoints,
        points = points,
        list = list,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateDateActionWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateDateActionWaypoints\u003Ec__IteratorE()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateDateActionWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateDateActionWaypointsAdditive\u003Ec__IteratorF()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateDateActionPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateDateActionPath\u003Ec__Iterator10()
      {
        points = points,
        count = count,
        actionPoints = actionPoints,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateDateActionPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateDateActionPath\u003Ec__Iterator11()
      {
        actionPoints = actionPoints,
        points = points,
        list = list,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateLocationWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateLocationWaypoints\u003Ec__Iterator12()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateLocationWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateLocationWaypointsAdditive\u003Ec__Iterator13()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateLocationPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateLocationPath\u003Ec__Iterator14()
      {
        points = points,
        count = count,
        actionPoints = actionPoints,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateLocationPath(
      List<Waypoint> points,
      List<ActionPoint> actionPoints,
      List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateLocationPath\u003Ec__Iterator15()
      {
        actionPoints = actionPoints,
        points = points,
        list = list,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateActorWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateActorWaypoints\u003Ec__Iterator16()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateActorWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateActorWaypointsAdditive\u003Ec__Iterator17()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateActorPath(
      List<Waypoint> points,
      Actor actor,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateActorPath\u003Ec__Iterator18()
      {
        points = points,
        count = count,
        actor = actor,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateActorPath(
      List<Waypoint> points,
      Actor actor,
      List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateActorPath\u003Ec__Iterator19()
      {
        actor = actor,
        points = points,
        list = list,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculatePlayerWaypoins()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculatePlayerWaypoins\u003Ec__Iterator1A()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculatePlayerWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculatePlayerWaypointsAdditive\u003Ec__Iterator1B()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculatePlayerPath(
      List<Waypoint> points,
      Actor actor,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculatePlayerPath\u003Ec__Iterator1C()
      {
        points = points,
        count = count,
        actor = actor,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateAnimalWaypoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateAnimalWaypoints\u003Ec__Iterator1D()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CalculateAnimalWaypointsAdditive(int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateAnimalWaypointsAdditive\u003Ec__Iterator1E()
      {
        count = count,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateAnimalPath(
      List<Waypoint> points,
      AnimalBase target,
      int count)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateAnimalPath\u003Ec__Iterator1F()
      {
        points = points,
        count = count,
        target = target,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CalculateAnimalPath(
      List<Waypoint> points,
      AnimalBase animal,
      List<Waypoint> list)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CCalculateAnimalPath\u003Ec__Iterator20()
      {
        points = points,
        list = list,
        animal = animal,
        \u0024this = this
      };
    }

    private bool WeatherCheck(Weather weather, Point point)
    {
      return weather != Weather.Rain && weather != Weather.Storm || (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(16) || point.AreaType == MapArea.AreaType.Indoor);
    }

    public void StartPatrol()
    {
      this._patrolCoroutine = this.Patrol();
      ((MonoBehaviour) this).StartCoroutine(this._patrolCoroutine);
    }

    public void ResumePatrol()
    {
      if (!((Behaviour) this).get_isActiveAndEnabled())
        return;
      if (this._patrolCoroutine == null)
        this._patrolCoroutine = this.Patrol();
      ((MonoBehaviour) this).StartCoroutine(this._patrolCoroutine);
    }

    public void StopPatrol()
    {
      if (this._patrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._patrolCoroutine);
    }

    public void AbortPatrol()
    {
      if (this._patrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._patrolCoroutine);
      this._patrolCoroutine = (IEnumerator) null;
    }

    public void ClearWalkPath()
    {
      this._reservedWaypoints.Clear();
    }

    [DebuggerHidden]
    private IEnumerator Patrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CPatrol\u003Ec__Iterator21()
      {
        \u0024this = this
      };
    }

    public bool LivesPatrol
    {
      get
      {
        return this._patrolCoroutine != null;
      }
    }

    public bool LivesCalc
    {
      get
      {
        return this._calcEnumerator != null;
      }
    }

    private bool SetDestination(Waypoint destination)
    {
      this.DestWaypoint = destination;
      return this._navMeshAgent.SetDestination(((Component) destination).get_transform().get_position());
    }

    private bool SetDestinationByDirectPathForce(Waypoint destination)
    {
      this.DestWaypoint = destination;
      bool flag = false;
      if (Object.op_Equality((Object) this._navMeshAgent, (Object) null))
        return false;
      if (!this._navMeshAgent.get_isOnNavMesh())
        return flag;
      NavMeshPath navMeshPath = new NavMeshPath();
      if (!this._navMeshAgent.CalculatePath(((Component) destination).get_transform().get_position(), navMeshPath) || !this._navMeshAgent.SetPath(navMeshPath) || !this._navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>())
        ;
      return flag;
    }

    private bool SetDestinationByDirectPath(Waypoint destination)
    {
      this.DestWaypoint = destination;
      bool flag = false;
      if (!this._navMeshAgent.get_isOnNavMesh() || !this._navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>())
        return flag;
      NavMeshPath navMeshPath = new NavMeshPath();
      if (!this._navMeshAgent.CalculatePath(((Component) destination).get_transform().get_position(), navMeshPath) || !this._navMeshAgent.SetPath(navMeshPath) || !this._navMeshAgent.get_path().get_corners().IsNullOrEmpty<Vector3>())
        ;
      return flag;
    }

    public bool HasArrived()
    {
      if (!((Behaviour) this._navMeshAgent).get_enabled())
        return false;
      float arrivedDistance = Singleton<Resources>.Instance.AgentProfile.WalkSetting.arrivedDistance;
      return (!this._navMeshAgent.get_pathPending() ? (double) this._navMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) arrivedDistance;
    }

    public bool HasArrivedOffMeshLink(OffMeshLink offMeshLink)
    {
      if (Object.op_Equality((Object) offMeshLink, (Object) null) || !((Behaviour) this._navMeshAgent).get_isActiveAndEnabled())
        return false;
      float arrivedDistance = Singleton<Resources>.Instance.AgentProfile.WalkSetting.arrivedDistance;
      return (!this._navMeshAgent.get_pathPending() ? (double) this._navMeshAgent.get_remainingDistance() : double.PositiveInfinity) <= (double) arrivedDistance;
    }

    public bool IsInvalidMoveDestination(OffMeshLink checkLink)
    {
      if (Object.op_Equality((Object) checkLink, (Object) null))
        return false;
      ActionPoint component = (ActionPoint) ((Component) checkLink).GetComponent<ActionPoint>();
      if (Object.op_Equality((Object) component, (Object) null))
        return false;
      if (!component.IsNeutralCommand)
        return true;
      List<ActionPoint> connectedActionPoints = component.ConnectedActionPoints;
      if (connectedActionPoints != null)
      {
        foreach (ActionPoint actionPoint in connectedActionPoints)
        {
          if (!Object.op_Equality((Object) actionPoint, (Object) null) && !actionPoint.IsNeutralCommand)
            return true;
        }
      }
      return false;
    }

    public void StartActionPatrol()
    {
      if (this._actionPatrolCoroutine == null)
        this._actionPatrolCoroutine = this.ActionPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._actionPatrolCoroutine);
    }

    public void StopActionPatrol()
    {
      if (this._actionPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._actionPatrolCoroutine);
    }

    public void AbortActionPatrol()
    {
      if (this._actionPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._actionPatrolCoroutine);
      this._actionPatrolCoroutine = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator ActionPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CActionPatrol\u003Ec__Iterator22()
      {
        \u0024this = this
      };
    }

    public bool LivesActionPatrol
    {
      get
      {
        return this._actionPatrolCoroutine != null;
      }
    }

    public bool LivesActionCalc
    {
      get
      {
        return this._actionCalcEnumerator != null;
      }
    }

    public void StartDateActionPatrol()
    {
      if (this._dateActionPatrolCoroutine == null)
        this._dateActionPatrolCoroutine = this.DateActionPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._dateActionPatrolCoroutine);
    }

    public void StopDateActionPatrol()
    {
      if (this._dateActionPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._dateActionPatrolCoroutine);
    }

    [DebuggerHidden]
    private IEnumerator DateActionPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CDateActionPatrol\u003Ec__Iterator23()
      {
        \u0024this = this
      };
    }

    public bool LivesDateActionPatrol
    {
      get
      {
        return this._dateActionPatrolCoroutine != null;
      }
    }

    public bool LivesDateActionCalc
    {
      get
      {
        return this._dateActionCalcEnumerator != null;
      }
    }

    public void StartLocationPatrol()
    {
      if (this._locationPatrolCoroutine == null)
        this._locationPatrolCoroutine = this.LocationPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._locationPatrolCoroutine);
    }

    public void StopLocationPatrol()
    {
      if (this._locationPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._locationPatrolCoroutine);
    }

    [DebuggerHidden]
    private IEnumerator LocationPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CLocationPatrol\u003Ec__Iterator24()
      {
        \u0024this = this
      };
    }

    public bool LivesLocationPatrol
    {
      get
      {
        return this._locationPatrolCoroutine != null;
      }
    }

    public bool LivesLocationCalc
    {
      get
      {
        return this._locationCalcEnumerator != null;
      }
    }

    public void StartActorPatrol()
    {
      if (this._actorPatrolCoroutine == null)
        this._actorPatrolCoroutine = this.ActorPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._actorPatrolCoroutine);
    }

    public void StopActorPatrol()
    {
      if (this._actorPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._actorPatrolCoroutine);
    }

    [DebuggerHidden]
    private IEnumerator ActorPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CActorPatrol\u003Ec__Iterator25()
      {
        \u0024this = this
      };
    }

    public bool LivesActorPatrol
    {
      get
      {
        return this._actorPatrolCoroutine != null;
      }
    }

    public bool LivesActorCalc
    {
      get
      {
        return this._actorCalcEnumerator != null;
      }
    }

    public void StartPlayerPatrol()
    {
      if (this._playerPatrolCoroutine == null)
        this._playerPatrolCoroutine = this.PlayerPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._playerPatrolCoroutine);
    }

    public void StopPlayerPatrol()
    {
      if (this._playerPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._playerPatrolCoroutine);
    }

    [DebuggerHidden]
    private IEnumerator PlayerPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CPlayerPatrol\u003Ec__Iterator26()
      {
        \u0024this = this
      };
    }

    public bool LivesPlayerPatrol
    {
      get
      {
        return this._playerPatrolCoroutine != null;
      }
    }

    public bool LivesPlayerCalc
    {
      get
      {
        return this._playerCalcEnumerator != null;
      }
    }

    public void StartAnimalPatrol()
    {
      if (this._animalPatrolCoroutine == null)
        this._animalPatrolCoroutine = this.AnimalPatrol();
      ((MonoBehaviour) this).StartCoroutine(this._animalPatrolCoroutine);
    }

    public void StopAnimalPatrol()
    {
      if (this._animalPatrolCoroutine == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._animalPatrolCoroutine);
    }

    public void StopForcedAnimalPatrol()
    {
      if (this._animalPatrolCoroutine != null)
      {
        ((MonoBehaviour) this).StopCoroutine(this._animalPatrolCoroutine);
        this._animalPatrolCoroutine = (IEnumerator) null;
      }
      if (this._animalCalcEnumerator != null)
      {
        ((MonoBehaviour) this).StopCoroutine(this._animalCalcEnumerator);
        this._animalCalcEnumerator = (IEnumerator) null;
      }
      if (this._additiveAnimalCalcProcess == null)
        return;
      ((MonoBehaviour) this).StopCoroutine(this._additiveAnimalCalcProcess);
      this._additiveAnimalCalcProcess = (IEnumerator) null;
    }

    public void ClearAnimalRoutePoints()
    {
      if (!this._reservedAnimalWaypoints.IsNullOrEmpty<Waypoint>())
        this._reservedAnimalWaypoints.Clear();
      if (this.SearchAnimalRoute.IsNullOrEmpty<Waypoint>())
        return;
      this.SearchAnimalRoute.Clear();
    }

    [DebuggerHidden]
    private IEnumerator AnimalPatrol()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AgentActor.\u003CAnimalPatrol\u003Ec__Iterator27()
      {
        \u0024this = this
      };
    }

    public bool SearchAnimalEmpty { get; set; }

    public bool LivesAnimalPatrol
    {
      get
      {
        return this._animalPatrolCoroutine != null;
      }
    }

    public bool LivesAnimalCalc
    {
      get
      {
        return this._animalCalcEnumerator != null;
      }
    }

    public bool CanGreet
    {
      get
      {
        AgentData agentData = this.AgentData;
        return !agentData.Greeted && (double) this.ChaControl.fileGameInfo.flavorState[1] >= (double) Singleton<Resources>.Instance.StatusProfile.CanGreetBorder && (double) agentData.StatsTable[1] >= (double) this.ChaControl.fileGameInfo.moodBound.upper;
      }
    }

    public bool CanScrounge
    {
      get
      {
        return this.ChaControl.fileGameInfo.phase >= 2 && !this.AgentData.ItemScrounge.isEvent;
      }
    }

    public bool CanPresent
    {
      get
      {
        return this.ChaControl.fileGameInfo.phase >= 2;
      }
    }

    public bool CanBirthdayPresent
    {
      get
      {
        return !this.IsBadMood() && Singleton<Manager.Map>.Instance.Player.IsBirthday(this);
      }
    }

    public bool IsHealthManager
    {
      get
      {
        bool flag1 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(28);
        bool flag2 = (double) this.AgentData.StatsTable[3] < (double) Singleton<Resources>.Instance.StatusProfile.HealthyPhysicalBorder;
        return flag1 && flag2;
      }
    }

    public bool CanWashFace
    {
      get
      {
        return this.ChaControl.fileGameInfo.flavorState[2] >= Singleton<Resources>.Instance.StatusProfile.WashFaceBorder;
      }
    }

    public bool CanClothChange
    {
      get
      {
        return (double) this.ChaControl.fileGameInfo.flavorState[0] >= (double) Singleton<Resources>.Instance.StatusProfile.CanClothChangeBorder;
      }
    }

    public bool ShouldRestoreCloth
    {
      get
      {
        bool isOtherCoordinate = this.AgentData.IsOtherCoordinate;
        DateTime now = Singleton<Manager.Map>.Instance.Simulator.Now;
        DateTime time = Singleton<Resources>.Instance.StatusProfile.ShouldRestoreCoordTime.Time;
        float restoreRangeMinuteTime = Singleton<Resources>.Instance.StatusProfile.RestoreRangeMinuteTime;
        bool flag = (double) Mathf.Abs((float) (time - now).TotalMinutes) < (double) restoreRangeMinuteTime;
        return isOtherCoordinate && flag;
      }
    }

    public bool CanGirlsAction
    {
      get
      {
        return this.ChaControl.fileGameInfo.flavorState[7] >= Singleton<Resources>.Instance.StatusProfile.GirlsActionBorder;
      }
    }

    public bool CanCauseHurt
    {
      get
      {
        if (this.AgentData.SickState.ID != -1)
          return false;
        int flavor = this.ChaControl.fileGameInfo.flavorState[5];
        StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
        float num = AgentActor.FlavorVariation(statusProfile.HurtRateBuffMinMax, statusProfile.HurtRateBuff, flavor);
        bool flag1 = (double) Random.Range(0.0f, 100f) <= (double) (statusProfile.HurtDefaultIncidence + num);
        bool flag2 = this.AreaType == MapArea.AreaType.Normal;
        return flag1 && flag2;
      }
    }

    public bool CanInvitation
    {
      get
      {
        bool flag = this.ChaControl.fileGameInfo.flavorState[4] >= Singleton<Resources>.Instance.StatusProfile.InvitationBorder;
        return Game.isAdd01 && flag;
      }
    }

    public bool CanMasturbation
    {
      get
      {
        int num = this.ChaControl.fileGameInfo.flavorState[4];
        int masturbationBorder = Singleton<Resources>.Instance.StatusProfile.MasturbationBorder;
        return Game.isAdd01 && num >= masturbationBorder;
      }
    }

    public bool CanLesbian
    {
      get
      {
        if (!Game.isAdd01 || Object.op_Equality((Object) this.TargetInSightActor, (Object) null))
          return false;
        bool flag1 = this.ChaControl.fileGameInfo.hSkill.ContainsValue(12);
        int id = this.TargetInSightActor.ID;
        int num1;
        if (!this.AgentData.FriendlyRelationShipTable.TryGetValue(id, out num1))
        {
          int num2 = 50;
          this.AgentData.FriendlyRelationShipTable[id] = num2;
          num1 = num2;
        }
        StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
        bool flag2 = num1 >= statusProfile.LesbianFriendlyRelationBorder;
        return flag1 && flag2 && (double) Random.Range(0.0f, 100f) <= (double) statusProfile.LesbianRate;
      }
    }

    public bool IsEmptyWeaknessMotivation
    {
      get
      {
        return (double) this.AgentData.WeaknessMotivation <= 0.0;
      }
    }

    public bool FromFemale { get; set; }

    public void SetDefaultImmoral()
    {
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      ChaFileGameInfo fileGameInfo = this.ChaControl.fileGameInfo;
      int immoral = fileGameInfo.immoral;
      int num1 = fileGameInfo.flavorState[4];
      Mathf.InverseLerp(statusProfile.DirtyImmoralMinMax.min, statusProfile.DirtyImmoralMinMax.max, (float) num1);
      float num2 = Mathf.Lerp(statusProfile.DirtyImmoralBuff.min, statusProfile.DirtyImmoralBuff.max, (float) num1);
      int num3 = immoral + Mathf.RoundToInt(num2);
      int num4 = 0;
      foreach (KeyValuePair<int, int> keyValuePair in fileGameInfo.hSkill)
      {
        if (keyValuePair.Value != -1)
          ++num4;
      }
      int num5 = num3 + Mathf.RoundToInt(statusProfile.ImmoralBuff * (float) num4);
      if (fileGameInfo.hSkill.ContainsValue(22))
        num5 += statusProfile.LustImmoralBuff;
      else if (fileGameInfo.hSkill.ContainsValue(11))
        num5 += statusProfile.FiredBodyImmoralBuff;
      this.SetStatus(6, (float) num5);
    }

    public void UpdateStatus(int actionID, int poseID)
    {
      this.ApplyActionResultParameter(actionID, poseID);
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      switch (Singleton<Manager.Map>.Instance.Simulator.Temperature)
      {
        case Temperature.Hot:
          this.AddStatus(0, statusProfile.HotTemperatureValue);
          break;
        case Temperature.Cold:
          this.AddStatus(0, statusProfile.ColdTemperatureValue);
          break;
      }
      if (this.AgentData.IsWet && !this.ChaControl.fileGameInfo.normalSkill.ContainsValue(4))
        this.AddStatus(0, statusProfile.WetTemperatureRate);
      if ((double) this.AgentData.StatsTable[2] <= 0.0)
      {
        this.AddFlavorSkill(5, statusProfile.StarveWarinessValue);
        this.AddFlavorSkill(6, statusProfile.StarveDarknessValue);
      }
      if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(24))
      {
        this.AddStatus(3, statusProfile.CursedPhysicalBuff);
        this.AddStatus(6, statusProfile.CursedImmoralBuff);
        this.AddDesire(Desire.GetDesireKey(Desire.Type.H), statusProfile.CursedHBuff);
      }
      this.AddStatus(6, 1f);
      switch (this.AgentData.SickState.ID)
      {
        case 1:
          this.ApplySituationResultParameter(7);
          break;
        case 3:
          this.ApplySituationResultParameter(10);
          break;
      }
    }

    public void ApplyActionResultParameter(int actionID, int poseID)
    {
      Dictionary<int, Dictionary<int, ParameterPacket>> dictionary;
      if (!Singleton<Resources>.Instance.Action.ActionStatusResultTable.TryGetValue(actionID, out dictionary))
        return;
      Dictionary<int, ParameterPacket> parameters;
      if (!dictionary.TryGetValue(poseID, out parameters))
      {
        EventType key;
        ValueTuple<int, string> valueTuple;
        if (!Debug.get_isDebugBuild() || !AIProject.Definitions.Action.EventTypeTable.TryGetValue(actionID, out key) || !AIProject.Definitions.Action.NameTable.TryGetValue(key, out valueTuple))
          return;
        Debug.LogWarning((object) string.Format("リザルトパラメーターがない：行動={0} ポーズ={1}", (object) valueTuple.Item2, (object) poseID));
      }
      else
        this.ApplyParameters(parameters);
    }

    public void ApplySituationResultParameter(int id)
    {
      Dictionary<int, ParameterPacket> parameters;
      if (!Singleton<Resources>.Instance.Action.SituationStatusResultTable.TryGetValue(id, out parameters))
        return;
      this.ApplyParameters(parameters);
      if (this.AgentData.SickState.ID != 3 || (double) this.AgentData.StatsTable[0] >= (double) this.ChaControl.fileGameInfo.tempBound.upper)
        return;
      this.AgentData.SickState.ID = -1;
    }

    private void ApplyParameters(Dictionary<int, ParameterPacket> parameters)
    {
      float num1 = 0.0f;
      foreach (KeyValuePair<int, ParameterPacket> parameter in parameters)
        num1 += parameter.Value.Probability;
      float num2 = Random.Range(0.0f, num1);
      ParameterPacket parameterPacket = (ParameterPacket) null;
      foreach (KeyValuePair<int, ParameterPacket> parameter in parameters)
      {
        if ((double) num2 <= (double) parameter.Value.Probability)
        {
          parameterPacket = parameter.Value;
          break;
        }
        num2 -= parameter.Value.Probability;
      }
      float rate = 1f;
      if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(5))
        rate = Random.Range(0, 10) != 0 ? 1f : 2f;
      foreach (KeyValuePair<int, TriThreshold> parameter in parameterPacket.Parameters)
        this.ApplyParameter(parameter, rate);
    }

    private void ApplyParameter(KeyValuePair<int, TriThreshold> param, float rate)
    {
      int key1 = param.Key;
      if (key1 < 10)
      {
        float t = Random.get_value();
        float num = (float) param.Value.Evaluate(t) * rate;
        this.AddStatus(key1, num);
      }
      else if (key1 < 100)
      {
        int id = key1 - 10;
        float t = Random.get_value();
        float num = (float) param.Value.Evaluate(t) * rate;
        if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(9) && id == 1)
          num += (float) Singleton<Resources>.Instance.StatusProfile.ReliabilityGWife;
        if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(4) && id == 6)
          num += (float) Singleton<Resources>.Instance.StatusProfile.DarknessReduceMaiden;
        this.AddFlavorSkill(id, (int) num);
      }
      else
      {
        int key2 = key1 - 100;
        float t = Random.get_value();
        float num = (float) param.Value.Evaluate(t) * rate;
        if (!this.AgentData.DesireTable.ContainsKey(key2))
          return;
        float desireValue = this.AgentData.DesireTable[key2] + num;
        if ((double) desireValue < 0.0)
          desireValue = 0.0f;
        else if ((double) desireValue > 100.0)
          desireValue = 100f;
        this.SetDesire(key2, desireValue);
        if (!this.ChaControl.fileGameInfo.normalSkill.ContainsValue(36) || key2 != Desire.GetDesireKey(Desire.Type.Bath))
          return;
        this.AddStatus(1, Singleton<Resources>.Instance.StatusProfile.DebuffMoodInBathDesire);
      }
    }

    public void ApplyFoodParameter(StuffItem item)
    {
      Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
      Dictionary<int, FoodParameterPacket> parameters;
      if (item == null || !Singleton<Resources>.Instance.GameInfo.FoodParameterTable.TryGetValue(item.CategoryID, out dictionary) || !dictionary.TryGetValue(item.ID, out parameters))
        return;
      this.ApplyEatParameters(parameters);
      this.AgentData.ItemList.RemoveItem(item);
    }

    public void ApplyDrinkParameter(StuffItem item)
    {
      Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
      Dictionary<int, FoodParameterPacket> parameters;
      if (item == null || !Singleton<Resources>.Instance.GameInfo.DrinkParameterTable.TryGetValue(item.CategoryID, out dictionary) || !dictionary.TryGetValue(item.ID, out parameters))
        return;
      if (!this.ChaControl.fileGameInfo.normalSkill.ContainsValue(45))
        ;
      this.ApplyDrinkParameters(parameters);
      this.AgentData.ItemList.RemoveItem(item);
    }

    private void ApplyEatParameters(Dictionary<int, FoodParameterPacket> parameters)
    {
      float num1 = 0.0f;
      foreach (KeyValuePair<int, FoodParameterPacket> parameter in parameters)
        num1 += parameter.Value.Probability;
      float num2 = Random.Range(0.0f, num1);
      FoodParameterPacket foodParameterPacket = (FoodParameterPacket) null;
      foreach (KeyValuePair<int, FoodParameterPacket> parameter in parameters)
      {
        if ((double) num2 <= (double) parameter.Value.Probability)
        {
          foodParameterPacket = parameter.Value;
          break;
        }
        num2 -= parameter.Value.Probability;
      }
      float rate = 1f;
      if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(5))
        rate = Random.Range(0, 10) != 0 ? 1f : 2f;
      foreach (KeyValuePair<int, TriThreshold> parameter in foodParameterPacket.Parameters)
        this.ApplyParameter(parameter, rate);
      if (this.AgentData.SickState.ID != -1)
        return;
      int num3 = Random.Range(0, 100);
      float stomachacheRate = foodParameterPacket.StomachacheRate;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      int flavor = this.ChaControl.fileGameInfo.flavorState[3];
      float num4 = AgentActor.FlavorVariation(statusProfile.StomachacheRateDebuffMinMax, statusProfile.StomachacheRateBuff, flavor);
      float num5 = stomachacheRate + num4;
      if ((double) num3 >= (double) num5)
        return;
      this.CauseStomachache();
    }

    private void ApplyDrinkParameters(Dictionary<int, FoodParameterPacket> parameters)
    {
      float num1 = 0.0f;
      foreach (KeyValuePair<int, FoodParameterPacket> parameter in parameters)
        num1 += parameter.Value.Probability;
      float num2 = Random.Range(0.0f, num1);
      FoodParameterPacket foodParameterPacket = (FoodParameterPacket) null;
      foreach (KeyValuePair<int, FoodParameterPacket> parameter in parameters)
      {
        if ((double) num2 < (double) parameter.Value.Probability)
        {
          foodParameterPacket = parameter.Value;
          break;
        }
        num2 -= parameter.Value.Probability;
      }
      float rate = 1f;
      if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(5))
        rate = Random.Range(0, 10) != 0 ? 1f : 2f;
      foreach (KeyValuePair<int, TriThreshold> parameter in foodParameterPacket.Parameters)
        this.ApplyParameter(parameter, rate);
      if (this.AgentData.SickState.ID != -1)
        return;
      int num3 = Random.Range(0, 100);
      float stomachacheRate = foodParameterPacket.StomachacheRate;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      int flavor = this.ChaControl.fileGameInfo.flavorState[3];
      float num4 = AgentActor.FlavorVariation(statusProfile.StomachacheRateDebuffMinMax, statusProfile.StomachacheRateBuff, flavor);
      float num5 = stomachacheRate + num4;
      if ((double) num3 >= (double) foodParameterPacket.StomachacheRate)
        return;
      this.CauseStomachache();
    }

    private void CauseStomachache()
    {
      this.AgentData.SickState.ID = 1;
      this.SetDesire(Desire.GetDesireKey(Desire.Type.Toilet), 100f);
    }

    public void HealSickBySleep()
    {
      AIProject.SaveData.Sickness sickState = this.AgentData.SickState;
      switch (sickState.ID)
      {
        case 1:
          sickState.ID = -1;
          break;
        case 3:
          this.ApplySituationResultParameter(11);
          this.AgentData.HeatStrokeLockInfo.Lock = true;
          sickState.ID = -1;
          break;
        case 4:
          this.ApplySituationResultParameter(14);
          sickState.ID = -1;
          break;
      }
    }

    public float? GetMotivation(int key)
    {
      float num;
      return !this.AgentData.MotivationTable.TryGetValue(key, out num) ? new float?() : new float?(num);
    }

    public bool SetMotivation(int key, float value)
    {
      Dictionary<int, float> motivationTable = this.AgentData.MotivationTable;
      float num;
      if (motivationTable == null || !motivationTable.TryGetValue(key, out num) || (double) num == (double) value)
        return false;
      motivationTable[key] = value;
      return true;
    }

    public void AddMotivation(int key, float addValue)
    {
      float? motivation = this.GetMotivation(key);
      if (!motivation.HasValue)
        return;
      float num = motivation.Value + addValue;
      this.SetMotivation(key, num);
    }

    private float GetAddRate(int key)
    {
      Dictionary<int, float> desireAddRateTable = Singleton<Resources>.Instance.GetDesireAddRateTable(0, Singleton<Manager.Map>.Instance.Simulator.TimeZone);
      float num;
      return desireAddRateTable == null || !desireAddRateTable.TryGetValue(key, out num) ? 0.0f : num;
    }

    public float? GetDesire(int key)
    {
      float num;
      return !this.AgentData.DesireTable.TryGetValue(key, out num) ? new float?() : new float?(num);
    }

    public bool SetDesire(int key, float desireValue)
    {
      Dictionary<int, float> desireTable = this.AgentData.DesireTable;
      float num;
      if (desireTable == null || !desireTable.TryGetValue(key, out num) || (double) num == (double) desireValue)
        return false;
      desireTable[key] = desireValue;
      return true;
    }

    public void SetForceDesire(int key, float desireValue)
    {
      this.AgentData.DesireTable[key] = desireValue;
    }

    public void AddDesire(int key, float addValue)
    {
      float? desire = this.GetDesire(key);
      if (!desire.HasValue)
        return;
      float desireValue = desire.Value + addValue;
      this.SetDesire(key, desireValue);
    }

    public void MulDesire(int key, float mulValue)
    {
      float? desire = this.GetDesire(key);
      if (!desire.HasValue)
        return;
      float desireValue = Mathf.Max(0.0f, desire.Value * mulValue);
      this.SetDesire(key, desireValue);
    }

    public void ClearDesire(Desire.Type type)
    {
      this.SetDesire(Desire.GetDesireKey(type), 0.0f);
    }

    private void UpdateDesire(
      TimeSpan deltaTime,
      Dictionary<int, AgentProfile.DiminuationRates> rate)
    {
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      ChaFileGameInfo fileGameInfo = this.ChaControl.fileGameInfo;
      int flavor1 = fileGameInfo.flavorState[0];
      int flavor2 = fileGameInfo.flavorState[1];
      int flavor3 = fileGameInfo.flavorState[2];
      int flavor4 = fileGameInfo.flavorState[3];
      int flavor5 = fileGameInfo.flavorState[4];
      int flavor6 = fileGameInfo.flavorState[5];
      int flavor7 = fileGameInfo.flavorState[7];
      int flavor8 = fileGameInfo.flavorState[6];
      bool flag1 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(0);
      bool flag2 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(1);
      bool flag3 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(2);
      bool flag4 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(3);
      bool flag5 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(7);
      bool flag6 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(13);
      bool flag7 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(14);
      bool flag8 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(15);
      bool flag9 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(21);
      bool flag10 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(22);
      bool flag11 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(30);
      bool flag12 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(34);
      bool flag13 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(35);
      bool flag14 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(40);
      bool flag15 = this.ChaControl.fileGameInfo.normalSkill.ContainsValue(41);
      int desireKey1 = Desire.GetDesireKey(Desire.Type.Bath);
      int desireKey2 = Desire.GetDesireKey(Desire.Type.Sleep);
      int desireKey3 = Desire.GetDesireKey(Desire.Type.Break);
      int desireKey4 = Desire.GetDesireKey(Desire.Type.Gift);
      int desireKey5 = Desire.GetDesireKey(Desire.Type.Want);
      int desireKey6 = Desire.GetDesireKey(Desire.Type.Eat);
      int desireKey7 = Desire.GetDesireKey(Desire.Type.H);
      int desireKey8 = Desire.GetDesireKey(Desire.Type.Game);
      int desireKey9 = Desire.GetDesireKey(Desire.Type.Lonely);
      int desireKey10 = Desire.GetDesireKey(Desire.Type.Hunt);
      int desireKey11 = Desire.GetDesireKey(Desire.Type.Cook);
      int desireKey12 = Desire.GetDesireKey(Desire.Type.Animal);
      int desireKey13 = Desire.GetDesireKey(Desire.Type.Location);
      int desireKey14 = Desire.GetDesireKey(Desire.Type.Drink);
      float num1 = this.AgentData.StatsTable[5];
      int[] desireTableKeys = AgentData.DesireTableKeys;
      bool flag16 = false;
      foreach (int key in desireTableKeys)
      {
        float? desire = this.GetDesire(key);
        if (desire.HasValue)
        {
          float addRate = this.GetAddRate(key);
          float num2;
          this.ChaControl.fileGameInfo.desireBuffVal.TryGetValue(key, out num2);
          float num3 = addRate + num2;
          if (key == desireKey1 && flag2)
            num3 += statusProfile.BuffBath;
          if (key == desireKey2)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.SleepSociabilityBuffMinMax, statusProfile.SleepSociabilityBuff, flavor7);
            num3 += num4;
            if (flag4)
              num3 += statusProfile.BuffSleep;
          }
          if (key == desireKey6)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.EatPheromoneDebuffMinMax, statusProfile.EatPheromoneDebuff, flavor1);
            num3 = num3 + num4 + AgentActor.FlavorVariation(statusProfile.EatInstinctBuffMinMax, statusProfile.EatInstinctBuff, flavor4) + AgentActor.FlavorVariation(statusProfile.EatDarknessDebuffMinMax, statusProfile.EatDarknessDebuff, flavor8);
            if (flag8)
              num3 += statusProfile.BuffEat;
          }
          if (key == desireKey3)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.BreakReasonBuffMinMax, statusProfile.BreakReasonBuff, flavor3);
            num3 = num3 + num4 + AgentActor.FlavorVariation(statusProfile.BreakInstinctBuffMinMax, statusProfile.BreakInstinctBuff, flavor3);
            if (flag13)
              num3 += statusProfile.BuffBreak;
          }
          if (key == desireKey4)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.GiftReliabilityBuffMinMax, statusProfile.GiftReliabilityBuff, flavor2);
            num3 += num4;
            if (flag5)
              num3 += statusProfile.BuffGift;
          }
          if (key == desireKey5)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.GimmeWarinessBuffMinMax, statusProfile.GimmeWarinessBuff, flavor6);
            num3 = num3 + num4 + AgentActor.FlavorVariation(statusProfile.GimmeDarknessBuffMinMax, statusProfile.GimmeDarknessBuff, flavor8);
            if (flag6)
              num3 += statusProfile.BuffGimme;
          }
          if (key == desireKey9)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.LonelySociabilityBuffMinMax, statusProfile.LonelySociabilityBuff, flavor7);
            num3 += num4;
            if (flag11)
              num3 += statusProfile.BuffLonely;
            if (flag12)
              num3 += statusProfile.BuffLonelySuperSense;
          }
          if (key == desireKey7)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.HDirtyBuffMinMax, statusProfile.HDirtyBuff, flavor5);
            num3 += num4;
            if (flag10)
              num3 += statusProfile.BuffH;
          }
          if (key == desireKey10)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.SearchWarinessBuffMinMax, statusProfile.SearchWarinessBuff, flavor6);
            num3 = num3 + num4 + AgentActor.FlavorVariation(statusProfile.SearchDarknessDebuffMinMax, statusProfile.SearchDarknessDebuff, flavor8);
            if (flag7)
              num3 += statusProfile.BuffSearchTough;
            if (flag15)
              num3 += statusProfile.BuffSearch;
          }
          if (key == desireKey8)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.PlayReasonDebuffMinMax, statusProfile.PlayReasonDebuff, flavor3);
            num3 = num3 + num4 + AgentActor.FlavorVariation(statusProfile.PlayInstinctBuffMinMax, statusProfile.PlayInstinctBuff, flavor4);
            if (flag9)
              num3 += statusProfile.BuffPlay;
          }
          if (key == desireKey11)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.CookPheromoneBuffMinMax, statusProfile.CookPheromoneBuff, flavor1);
            num3 += num4;
            if (flag1)
              num3 += statusProfile.BuffCook;
          }
          if (key == desireKey12 && flag3)
            num3 += statusProfile.BuffAnimal;
          if (key == desireKey13 && flag14)
            num3 += statusProfile.BuffLocation;
          if (key == desireKey14)
          {
            float num4 = AgentActor.FlavorVariation(statusProfile.DrinkWarinessBuffMinMax, statusProfile.DrinkWarinessBuff, flavor6);
            num3 += num4;
          }
          float num5 = Mathf.Max(0.0f, num3) / 60f;
          float desireValue = Mathf.Clamp(desire.Value + num5 * (float) deltaTime.TotalMinutes, 0.0f, 100f);
          if (this.SetDesire(key, desireValue))
            flag16 = true;
        }
        float? motivation = this.GetMotivation(key);
        if (motivation.HasValue)
        {
          float valueRecovery = rate[key].valueRecovery;
          float num2 = Mathf.Clamp(motivation.Value + valueRecovery * (float) deltaTime.TotalMinutes, 0.0f, num1);
          this.SetMotivation(key, num2);
        }
      }
      this.AgentData.TalkMotivation = Mathf.Clamp(this.AgentData.TalkMotivation + Singleton<Resources>.Instance.AgentProfile.TalkMotivationDimRate.valueRecovery * (float) deltaTime.TotalMinutes, 0.0f, num1);
      if (!flag16)
        ;
    }

    public static float FlavorVariation(Threshold minmax, Threshold threshold, int flavor)
    {
      float t = Mathf.InverseLerp(minmax.min, minmax.max, (float) flavor);
      return threshold.Lerp(t);
    }

    public void SetStatus(int id, float value)
    {
      if (id == 1 && this.ChaControl.fileGameInfo.phase < 2)
      {
        this.AgentData.StatsTable[1] = (float) (((double) this.ChaControl.fileGameInfo.moodBound.lower + (double) this.ChaControl.fileGameInfo.moodBound.upper) / 2.0);
      }
      else
      {
        StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
        switch (id)
        {
          case 1:
            if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(25))
              value += statusProfile.DebuffMood;
            if (this.ChaControl.fileGameInfo.phase < 2)
            {
              this.AgentData.StatsTable[id] = Mathf.Clamp(value, this.ChaControl.fileGameInfo.moodBound.lower, this.ChaControl.fileGameInfo.moodBound.upper);
              break;
            }
            this.AgentData.StatsTable[id] = Mathf.Clamp(value, 0.0f, 100f);
            break;
          case 2:
            this.AgentData.StatsTable[id] = Mathf.Clamp(value, 0.0f, 100f);
            break;
          case 5:
            float num;
            value = !Singleton<Resources>.Instance.AgentProfile.MotivationMinValueTable.TryGetValue(this.ChaControl.fileParam.personality, out num) ? Mathf.Clamp(value, 0.0f, 150f) : Mathf.Clamp(value, num, 150f);
            this.AgentData.StatsTable[id] = value;
            break;
          case 7:
            this.ChaControl.fileGameInfo.morality = Mathf.Clamp((int) value, 0, 100);
            break;
          default:
            value = Mathf.Clamp(value, 0.0f, 100f);
            this.AgentData.StatsTable[id] = value;
            break;
        }
      }
    }

    public void AddStatus(int id, float value)
    {
      ChaFileGameInfo fileGameInfo = this.ChaControl.fileGameInfo;
      if (id == 7)
      {
        float num = (float) fileGameInfo.morality + value;
        this.SetStatus(id, num);
      }
      else
      {
        float num1 = this.AgentData.StatsTable[id];
        float num2 = num1 + value;
        bool flag1 = fileGameInfo.normalSkill.ContainsValue(29);
        bool flag2 = fileGameInfo.normalSkill.ContainsValue(20);
        if (id == 3)
        {
          StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
          int flavor = fileGameInfo.flavorState[6];
          float num3 = AgentActor.FlavorVariation(statusProfile.DarknessPhysicalBuffMinMax, statusProfile.DarknessPhysicalBuff, flavor);
          num2 += num3;
        }
        if (id == 6 && flag2 && (double) num2 > (double) num1)
          num2 += Singleton<Resources>.Instance.StatusProfile.BuffImmoral;
        if (flag1 && id == 2)
        {
          if ((double) num1 >= (double) num2)
            return;
          this.SetStatus(id, num2);
        }
        else
          this.SetStatus(id, num2);
      }
    }

    public bool CanRevRape()
    {
      bool flag1 = this.ChaControl.fileGameInfo.hSkill.ContainsValue(13);
      bool flag2 = this.ChaControl.fileGameInfo.flavorState[4] >= Singleton<Resources>.Instance.StatusProfile.RevRapeBorder;
      return flag1 && flag2;
    }

    public void AddFlavorSkill(int id, int value)
    {
      if (this.AgentData == null)
        return;
      ChaFileGameInfo fileGameInfo = this.ChaControl.fileGameInfo;
      if (!fileGameInfo.flavorState.ContainsKey(id) || this.AgentData.ParameterLock || Manager.Config.GameData.ParameterLock)
        return;
      int num1 = fileGameInfo.flavorState[id] + value;
      fileGameInfo.flavorState[id] = Mathf.Clamp(num1, 0, 99999);
      if (id == 4)
      {
        if (!fileGameInfo.isHAddTaii0 && fileGameInfo.flavorState[id] >= 100)
          fileGameInfo.isHAddTaii0 = true;
        if (!fileGameInfo.isHAddTaii1 && fileGameInfo.flavorState[id] >= 170)
          fileGameInfo.isHAddTaii1 = true;
      }
      int num2 = fileGameInfo.totalFlavor + value;
      fileGameInfo.totalFlavor = Mathf.Max(num2, 0);
      this.AgentData.AddFlavorSkill(id, value);
    }

    public void AddFlavorSkill(FlavorSkill.Type type, int value)
    {
      this.AddFlavorSkill((int) type, value);
    }

    public void SetFlavorSkill(int id, int value)
    {
      if (this.AgentData == null)
        return;
      ChaFileGameInfo fileGameInfo = this.ChaControl.fileGameInfo;
      if (!fileGameInfo.flavorState.ContainsKey(id) || this.AgentData.ParameterLock || Manager.Config.GameData.ParameterLock)
        return;
      int num1 = value - fileGameInfo.flavorState[id];
      fileGameInfo.flavorState[id] = Mathf.Clamp(value, 0, 99999);
      if (id == 4)
      {
        if (!fileGameInfo.isHAddTaii0 && fileGameInfo.flavorState[id] >= 100)
          fileGameInfo.isHAddTaii0 = true;
        if (!fileGameInfo.isHAddTaii1 && fileGameInfo.flavorState[id] >= 170)
          fileGameInfo.isHAddTaii1 = true;
      }
      int num2 = fileGameInfo.totalFlavor + num1;
      fileGameInfo.totalFlavor = Mathf.Max(num2, 0);
      this.AgentData.AddFlavorAdditionAmount(num1);
    }

    public void SetFlavorSkill(FlavorSkill.Type type, int value)
    {
      this.SetFlavorSkill((int) type, value);
    }

    public int GetFlavorSkill(int id)
    {
      return Object.op_Inequality((Object) this.ChaControl, (Object) null) && this.ChaControl.fileGameInfo != null ? this.ChaControl.fileGameInfo.flavorState[id] : 0;
    }

    public int GetFlavorSkill(FlavorSkill.Type type)
    {
      return this.GetFlavorSkill((int) type);
    }

    private bool CanProgressPhase()
    {
      Dictionary<int, int> dictionary;
      int num;
      return this.AttitudeID == 0 && Singleton<Resources>.Instance.Action.PhaseExp.TryGetValue(this.ChaControl.fileParam.personality, out dictionary) && dictionary.TryGetValue(this.ChaControl.fileGameInfo.phase, out num) && num <= this.ChaControl.fileGameInfo.totalFlavor;
    }

    public void SetPhase(int phase)
    {
      if (this.ChaControl.fileGameInfo.phase == phase)
        return;
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0} change phase: {1} -> {2}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) this.ChaControl.fileGameInfo.phase, (object) phase));
      this.ChaControl.fileGameInfo.phase = phase;
      switch (phase)
      {
        case 2:
          KeyValuePair<int, int>[] array1 = this.ChaControl.fileGameInfo.flavorState.OrderByDescending<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (pair => pair.Value)).Take<KeyValuePair<int, int>>(3).ToArray<KeyValuePair<int, int>>();
          int num1 = 0;
          int num2 = 0;
          foreach (KeyValuePair<int, int> keyValuePair in array1)
          {
            Dictionary<int, ObtainItemInfo> dic;
            if (Singleton<Resources>.Instance.Action.FlavorPickSkillTable.TryGetValue(keyValuePair.Key, out dic))
            {
              int key = this.LotterySkill(dic);
              ObtainItemInfo obtainItemInfo;
              if (dic.TryGetValue(key, out obtainItemInfo))
                this.ChaControl.fileGameInfo.normalSkill[num1++] = obtainItemInfo.Info.ItemID;
            }
            if (Game.isAdd01 && Singleton<Resources>.Instance.Action.FlavorPickHSkillTable.TryGetValue(keyValuePair.Key, out dic))
            {
              int key = this.LotterySkill(dic);
              ObtainItemInfo obtainItemInfo;
              if (dic.TryGetValue(key, out obtainItemInfo))
                this.ChaControl.fileGameInfo.hSkill[num2++] = obtainItemInfo.Info.ItemID;
            }
          }
          int num3;
          if (this.ChaControl.fileGameInfo.morality >= 50)
          {
            Dictionary<int, ObtainItemInfo> dic;
            if (!Singleton<Resources>.Instance.Action.FlavorPickSkillTable.TryGetValue(17, out dic))
              break;
            int key = this.LotterySkill(dic);
            ObtainItemInfo obtainItemInfo;
            if (!dic.TryGetValue(key, out obtainItemInfo))
              break;
            Dictionary<int, int> normalSkill = this.ChaControl.fileGameInfo.normalSkill;
            int index = num1;
            num3 = index + 1;
            int itemId = obtainItemInfo.Info.ItemID;
            normalSkill[index] = itemId;
            break;
          }
          Dictionary<int, ObtainItemInfo> dic1;
          if (!Singleton<Resources>.Instance.Action.FlavorPickSkillTable.TryGetValue(18, out dic1))
            break;
          int key1 = this.LotterySkill(dic1);
          ObtainItemInfo obtainItemInfo1;
          if (!dic1.TryGetValue(key1, out obtainItemInfo1))
            break;
          Dictionary<int, int> normalSkill1 = this.ChaControl.fileGameInfo.normalSkill;
          int index1 = num1;
          num3 = index1 + 1;
          int itemId1 = obtainItemInfo1.Info.ItemID;
          normalSkill1[index1] = itemId1;
          break;
        case 3:
          KeyValuePair<int, int>[] array2 = this.ChaControl.fileGameInfo.flavorState.OrderByDescending<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (pair => pair.Value)).Take<KeyValuePair<int, int>>(2).ToArray<KeyValuePair<int, int>>();
          int key2 = array2[0].Key;
          int key3 = array2[1].Key;
          Dictionary<int, int> dictionary;
          int num4;
          if (!Singleton<Resources>.Instance.Action.LifestyleTable.TryGetValue(key2, out dictionary) || !dictionary.TryGetValue(key3, out num4))
            break;
          this.ChaControl.fileGameInfo.lifestyle = num4;
          break;
      }
    }

    private int LotterySkill(Dictionary<int, ObtainItemInfo> dic)
    {
      float num1 = 0.0f;
      foreach (KeyValuePair<int, ObtainItemInfo> keyValuePair in dic)
        num1 += (float) keyValuePair.Value.Rate;
      float num2 = Random.Range(0.0f, num1);
      int num3 = -1;
      foreach (KeyValuePair<int, ObtainItemInfo> keyValuePair in dic)
      {
        int rate = keyValuePair.Value.Rate;
        if ((double) num2 <= (double) rate)
        {
          num3 = keyValuePair.Key;
          break;
        }
        num2 -= (float) rate;
      }
      return num3;
    }

    public void CauseSick()
    {
      AIProject.SaveData.Sickness sickState = this.AgentData.SickState;
      if (sickState.ID != -1)
        return;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      float num1 = this.AgentData.StatsTable[3];
      float num2 = this.AgentData.StatsTable[0];
      int flavor = this.ChaControl.fileGameInfo.flavorState[5];
      if ((double) num1 <= 0.0)
      {
        if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(14))
          return;
        sickState.ID = 2;
      }
      if ((double) num2 <= (double) agentProfile.ColdTempBorder)
      {
        if (this.AgentData.ColdLockInfo.Lock)
          return;
        float num3 = Random.Range(0.0f, 100f);
        float num4 = statusProfile.ColdDefaultIncidence + AgentActor.FlavorVariation(statusProfile.ColdRateBuffMinMax, statusProfile.ColdRateBuff, flavor);
        if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(38))
          num4 += statusProfile.ColdRateDebuffWeak;
        if ((double) num3 >= (double) num4)
          return;
        sickState.ID = 0;
      }
      else
      {
        if ((double) num2 < (double) agentProfile.HotTempBorder || this.AgentData.HeatStrokeLockInfo.Lock)
          return;
        float num3 = Random.Range(0.0f, 100f);
        float num4 = statusProfile.HeatStrokeDefaultIncidence + AgentActor.FlavorVariation(statusProfile.HeatStrokeRateBuffMinMax, statusProfile.HeatStrokeRateBuff, flavor);
        if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(43))
          num4 += statusProfile.HeatStrokeBuffGuts;
        if ((double) num3 >= (double) num4)
          return;
        sickState.ID = 3;
      }
    }

    private void OnSickUpdate(TimeSpan timeSpan)
    {
      AIProject.SaveData.Sickness sickState = this.AgentData.SickState;
      if (sickState.ID <= -1 || this.Mode == Desire.ActionType.Onbu)
        return;
      sickState.ElapsedTime += timeSpan;
      if (Desire.ContainsSickFilterTable(this.Mode))
        return;
      Resources instance = Singleton<Resources>.Instance;
      switch (sickState.ID)
      {
        case 0:
          if (sickState.ElapsedTime.Days >= 4)
          {
            if (!((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || !((Behaviour) this.BehaviorResources).get_enabled() || !instance.AgentProfile.EncounterWhitelist.Contains(this.BehaviorResources.Mode))
              break;
            this.CommandPartner = (Actor) null;
            this.AgentData.CarryingItem = (StuffItem) null;
            this.ActivateTransfer(false);
            this.ReservedMode = Desire.ActionType.Cold3A;
            this.ChangeBehavior(Desire.ActionType.Faint);
            MapUIContainer.AddSystemLog(string.Format("{0}が倒れました", (object) MapUIContainer.CharaNameColor((Actor) this)), true);
            sickState.Duration = TimeSpan.FromDays(4.0) + TimeSpan.FromDays(2.0);
            break;
          }
          if (sickState.ElapsedTime.Days < 2 || !((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || (!((Behaviour) this.BehaviorResources).get_enabled() || !instance.AgentProfile.EncounterWhitelist.Contains(this.BehaviorResources.Mode)))
            break;
          this.CommandPartner = (Actor) null;
          this.AgentData.CarryingItem = (StuffItem) null;
          this.ActivateTransfer(false);
          this.ReservedMode = Desire.ActionType.Cold2A;
          this.ChangeBehavior(Desire.ActionType.Faint);
          MapUIContainer.AddSystemLog(string.Format("{0}が倒れました", (object) MapUIContainer.CharaNameColor((Actor) this)), true);
          sickState.Duration = TimeSpan.FromDays(2.0) + TimeSpan.FromDays(2.0);
          break;
        case 2:
          if (this.ChaControl.fileGameInfo.normalSkill.ContainsValue(14))
          {
            sickState.ID = -1;
            break;
          }
          if (!((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() || !((Behaviour) this.BehaviorResources).get_enabled() || !instance.AgentProfile.EncounterWhitelist.Contains(this.BehaviorResources.Mode))
            break;
          float num = Random.Range(0.0f, (float) TimeSpan.FromDays(1.0).TotalHours);
          sickState.Duration = sickState.ElapsedTime + TimeSpan.FromDays(2.0) + TimeSpan.FromHours((double) num);
          this.CommandPartner = (Actor) null;
          this.AgentData.CarryingItem = (StuffItem) null;
          this.ActivateTransfer(false);
          this.ReservedMode = Desire.ActionType.OverworkA;
          this.ChangeBehavior(Desire.ActionType.Faint);
          MapUIContainer.AddSystemLog(string.Format("{0}が倒れました", (object) MapUIContainer.CharaNameColor((Actor) this)), true);
          break;
      }
    }

    public enum ADV_CATEGORY
    {
      TALK = 0,
      INSTRUCTION = 1,
      SPEAK = 2,
      PRESENT = 3,
      IN = 4,
      OUT = 5,
      Event = 6,
      Phase = 7,
      Together = 8,
      H = 9,
      HScene = 10, // 0x0000000A
      Sickness = 11, // 0x0000000B
      Food = 12, // 0x0000000C
      AddEV = 13, // 0x0000000D
      TUTORIAL = 100, // 0x00000064
    }

    public class PackData : CharaPackData
    {
      public CommCommandList.CommandInfo[] restoreCommands
      {
        set
        {
          this._restoreCommands = value;
        }
      }

      private CommCommandList.CommandInfo[] _restoreCommands { get; set; }

      public System.Action OnEndRefreshCommand { get; set; }

      public int AttitudeID { get; set; }

      public bool isFavoriteTarget { get; set; }

      public string FavoriteTargetName { get; set; } = string.Empty;

      public bool isThisPartner { get; set; } = true;

      public bool isSuccessFollow
      {
        get
        {
          ValData valData;
          return this.Vars != null && this.Vars.TryGetValue(nameof (isSuccessFollow), out valData) && (bool) valData.o;
        }
      }

      public bool isSuccessH
      {
        get
        {
          ValData valData;
          return this.Vars != null && this.Vars.TryGetValue(nameof (isSuccessH), out valData) && (bool) valData.o;
        }
      }

      public int hMode
      {
        get
        {
          ValData valData;
          return this.Vars == null || !this.Vars.TryGetValue(nameof (hMode), out valData) ? -1 : (int) valData.o;
        }
      }

      public override List<Program.Transfer> Create()
      {
        List<Program.Transfer> transferList = base.Create();
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "int", "AttitudeID", this.AttitudeID.ToString()));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isFavoriteTarget", this.isFavoriteTarget.ToString()));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "string", "FavoriteTargetName", this.FavoriteTargetName));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isThisPartner", this.isThisPartner.ToString()));
        return transferList;
      }

      public override void Receive(TextScenario scenario)
      {
        base.Receive(scenario);
        CommCommandList commandList = MapUIContainer.CommandList;
        if (this._restoreCommands == null)
          return;
        commandList.Refresh(this._restoreCommands, commandList.CanvasGroup, this.OnEndRefreshCommand);
      }
    }

    private class LeaveAloneDisposableInfo
    {
      private SingleAssignmentDisposable[] _arrayDisposable;

      public LeaveAloneDisposableInfo()
      {
        this._arrayDisposable = new SingleAssignmentDisposable[2];
      }

      public SingleAssignmentDisposable Timer
      {
        get
        {
          return this._arrayDisposable[0];
        }
        set
        {
          this._arrayDisposable[0] = value;
        }
      }

      public SingleAssignmentDisposable Wait
      {
        get
        {
          return this._arrayDisposable[1];
        }
        set
        {
          this._arrayDisposable[1] = value;
        }
      }

      public void End()
      {
        for (int index = 0; index < 2; ++index)
        {
          this._arrayDisposable[index]?.Dispose();
          this._arrayDisposable[index] = (SingleAssignmentDisposable) null;
        }
      }
    }

    private class TouchDisposableInfo
    {
      public SingleAssignmentDisposable Wait { get; set; }

      public bool Check
      {
        get
        {
          return this.Wait != null;
        }
      }

      public void End()
      {
        this.Wait?.Dispose();
        this.Wait = (SingleAssignmentDisposable) null;
      }
    }

    private class TouchInfo
    {
      private bool _enableCol;

      public TouchInfo(GameObject obj, Collider col, int layer)
      {
        this.Obj = obj;
        this.Col = col;
        this._enableCol = this.Col.get_enabled();
        this.Col.set_enabled(true);
        this.Layer = obj.get_layer();
        this.Obj.set_layer(layer);
      }

      public GameObject Obj { get; private set; }

      public Collider Col { get; private set; }

      public int Layer { get; private set; }

      private void Reset()
      {
        this.Col.set_enabled(this._enableCol);
        this.Obj.set_layer(this.Layer);
      }
    }

    private class ColDisposableInfo
    {
      private System.Action _onTouch;
      private System.Action _onEnter;
      private System.Action _onExit;
      private SingleAssignmentDisposable _disposableTouch;
      private SingleAssignmentDisposable _disposableEnter;
      private SingleAssignmentDisposable _disposableExit;

      public ColDisposableInfo(Collider col, System.Action onTouch, System.Action onEnter, System.Action onExit)
      {
        this.Col = col;
        this._onTouch = onTouch;
        this._onEnter = onEnter;
        this._onExit = onExit;
      }

      public Collider Col { get; private set; }

      public void Start()
      {
        this.End();
        this._disposableTouch = new SingleAssignmentDisposable();
        this._disposableTouch.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnMouseDownAsObservable((Component) this.Col), (Func<M0, bool>) (_ => !EventSystem.get_current().IsPointerOverGameObject())), (System.Action<M0>) (_ =>
        {
          if (this._onTouch == null)
            return;
          this._onTouch();
        })), (Component) this.Col));
        this._disposableEnter = new SingleAssignmentDisposable();
        this._disposableEnter.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnMouseEnterAsObservable((Component) this.Col), (Func<M0, bool>) (_ => !EventSystem.get_current().IsPointerOverGameObject())), (System.Action<M0>) (_ =>
        {
          if (this._onEnter == null)
            return;
          this._onEnter();
        })), (Component) this.Col));
        this._disposableExit = new SingleAssignmentDisposable();
        this._disposableExit.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnMouseExitAsObservable((Component) this.Col), (Func<M0, bool>) (_ => !EventSystem.get_current().IsPointerOverGameObject())), (System.Action<M0>) (_ =>
        {
          if (this._onExit == null)
            return;
          this._onExit();
        })), (Component) this.Col));
      }

      public void End()
      {
        if (this._disposableTouch != null)
          this._disposableTouch.Dispose();
        this._disposableTouch = (SingleAssignmentDisposable) null;
        if (this._disposableEnter != null)
          this._disposableEnter.Dispose();
        this._disposableEnter = (SingleAssignmentDisposable) null;
        if (this._disposableExit != null)
          this._disposableExit.Dispose();
        this._disposableExit = (SingleAssignmentDisposable) null;
      }
    }
  }
}
