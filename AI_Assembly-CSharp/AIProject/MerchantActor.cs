// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using AIProject.Player;
using AIProject.SaveData;
using AIProject.UI;
using Manager;
using MessagePack;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class MerchantActor : Actor, ICommandable, INavMeshActor
  {
    public ValueTuple<CompositeDisposable, System.Action> SequenceAction = new ValueTuple<CompositeDisposable, System.Action>((CompositeDisposable) null, (System.Action) null);
    public Dictionary<int, Dictionary<int, List<MerchantPoint>>> MerchantPointTable = new Dictionary<int, Dictionary<int, List<MerchantPoint>>>();
    private CommandLabel.CommandInfo[] _emptyLabel = new CommandLabel.CommandInfo[0];
    [SerializeField]
    private ObjectLayer _layer = ObjectLayer.Command;
    private float _distanceTweenPlayer = float.MaxValue;
    private float _heightDistTweenPlayer = float.MaxValue;
    private bool _prevAnimEnable = true;
    private bool _prevLocomEnable = true;
    private bool _prevConEnable = true;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    private NavMeshPath pathForCalc;
    private CommandLabel.CommandInfo[] _talkLabel;
    [SerializeField]
    private ActorAnimationMerchant _animation;
    [SerializeField]
    private ActorLocomotionMerchant _locomotor;
    [SerializeField]
    private MerchantController _controller;
    private bool _prevCloseToPlayer;
    private IDisposable _locomotionChangeSpeedDisposable;
    private CommCommandList.CommandInfo[] _normalCommandOptionInfo;

    public override string CharaName
    {
      get
      {
        return "シャン";
      }
    }

    protected MerchantBehaviorTreeResources BehaviorResources { get; private set; }

    public Merchant.ActionType BehaviorMode
    {
      get
      {
        return Object.op_Inequality((Object) this.BehaviorResources, (Object) null) ? this.BehaviorResources.Mode : Merchant.ActionType.ToAbsent;
      }
    }

    public void EnableBehavior()
    {
      if (!Object.op_Inequality((Object) this.BehaviorResources, (Object) null) || ((Behaviour) this.BehaviorResources).get_enabled())
        return;
      ((Behaviour) this.BehaviorResources).set_enabled(true);
    }

    public void DisableBehavior()
    {
      if (!Object.op_Inequality((Object) this.BehaviorResources, (Object) null) || !((Behaviour) this.BehaviorResources).get_enabled())
        return;
      ((Behaviour) this.BehaviorResources).set_enabled(false);
    }

    public void StopBehavior()
    {
      if (!Object.op_Inequality((Object) this.BehaviorResources, (Object) null))
        return;
      this.BehaviorResources.StopBehaviorTree();
    }

    public Merchant.ActionType CurrentMode { get; private set; } = Merchant.ActionType.Wait;

    public Merchant.ActionType PrevMode { get; private set; } = Merchant.ActionType.Wait;

    public Merchant.ActionType NextMode { get; private set; } = Merchant.ActionType.Wait;

    public Merchant.ActionType LastNormalMode { get; private set; } = Merchant.ActionType.Wait;

    public int OpenAreaID { get; set; }

    public bool IsActionMoving { get; set; }

    public bool IsRunning { get; set; }

    public MerchantPoint CurrentMerchantPoint { get; set; }

    public MerchantPoint TargetInSightMerchantPoint { get; set; }

    public MerchantPoint MainTargetInSightMerchantPoint { get; set; }

    public MerchantPoint PrevMerchantPoint { get; set; }

    public MerchantPoint StartPoint { get; private set; }

    public MerchantPoint ExitPoint { get; private set; }

    public ActionPoint PrevActionPoint { get; set; }

    public ActionPoint BookingActionPoint { get; set; }

    public MerchantActor.MerchantSchedule CurrentSchedule
    {
      get
      {
        return this.MerchantData?.CurrentSchedule;
      }
      private set
      {
        if (this.MerchantData == null)
          return;
        this.MerchantData.CurrentSchedule = value;
      }
    }

    public List<MerchantActor.MerchantSchedule> ScheduleList
    {
      get
      {
        return this.MerchantData?.ScheduleList;
      }
    }

    public MerchantActor.MerchantSchedule PrevSchedule
    {
      get
      {
        return this.MerchantData?.PrevSchedule;
      }
      private set
      {
        if (this.MerchantData == null)
          return;
        this.MerchantData.PrevSchedule = value;
      }
    }

    public bool Talkable { get; set; } = true;

    public bool ActiveEncount { get; set; }

    public Vector3 ObstaclePosition
    {
      get
      {
        return ((Component) this.NavMeshObstacle).get_transform().get_position();
      }
      set
      {
        ((Component) this.NavMeshObstacle).get_transform().set_position(value);
      }
    }

    public Quaternion ObstacleRotation
    {
      get
      {
        return ((Component) this.NavMeshObstacle).get_transform().get_rotation();
      }
      set
      {
        ((Component) this.NavMeshObstacle).get_transform().set_rotation(value);
      }
    }

    public bool IsActiveAgnet
    {
      get
      {
        return ((Behaviour) this.NavMeshAgent).get_enabled();
      }
    }

    public bool IsActiveObstacle
    {
      get
      {
        return ((Behaviour) this.NavMeshObstacle).get_enabled();
      }
    }

    public bool IsActiveNavMeshElement
    {
      get
      {
        return this.IsActiveAgnet || this.IsActiveObstacle;
      }
    }

    public void AddSequenceActionDisposable(IDisposable disposable)
    {
      ((CompositeDisposable) this.SequenceAction.Item1 ?? (CompositeDisposable) (this.SequenceAction.Item1 = (__Null) new CompositeDisposable())).Add(disposable);
    }

    public void AddSequenceActionOnComplete(System.Action onComplete)
    {
      ref ValueTuple<CompositeDisposable, System.Action> local = ref this.SequenceAction;
      local.Item2 = (__Null) Delegate.Combine((Delegate) local.Item2, (Delegate) onComplete);
    }

    public void SetSequenceAction(IDisposable disposable, System.Action onComplete)
    {
      this.AddSequenceActionDisposable(disposable);
      this.AddSequenceActionOnComplete(onComplete);
    }

    public void ClearSequenceAction()
    {
      this.SequenceAction.Item1 = null;
      this.SequenceAction.Item2 = null;
    }

    public void StopSequenceAction()
    {
      // ISSUE: variable of the null type
      __Null local = this.SequenceAction.Item2;
      if (local != null)
        ((System.Action) local)();
      this.SequenceAction.Item2 = null;
      ((CompositeDisposable) this.SequenceAction.Item1)?.Clear();
    }

    public void DisposeSequenceAction()
    {
      this.SequenceAction.Item2 = null;
      ((CompositeDisposable) this.SequenceAction.Item1)?.Clear();
    }

    public bool ElapsedDay { get; set; }

    public int ElapsedDayCount { get; protected set; }

    public List<MerchantPoint> MerchantPoints { get; set; } = new List<MerchantPoint>();

    public bool IsImpossible { get; private set; }

    public Actor CommandPartner { get; set; }

    public bool SetImpossible(bool value, Actor actor)
    {
      if (this.IsImpossible == value || value && Object.op_Inequality((Object) this.CommandPartner, (Object) null))
        return false;
      this.IsImpossible = value;
      this.CommandPartner = !value ? (Actor) null : actor;
      return true;
    }

    public override bool IsNeutralCommand
    {
      get
      {
        if (this.IsImpossible || this.CurrentSchedule == null || (!this.Talkable || !this.MerchantData.Unlock))
          return false;
        return Merchant.NormalModeList.Contains(this.CurrentMode) || this.CurrentMode == Merchant.ActionType.Encounter;
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
      if ((double) radiusA < (double) distance)
        return false;
      Vector3 position = this.Position;
      position.y = (__Null) 0.0;
      float num = angle / 2f;
      return (double) Vector3.Angle(Vector3.op_Subtraction(position, basePosition), forward) <= (double) num;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this.pathForCalc == null)
        this.pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        nmAgent.CalculatePath(this.Position, this.pathForCalc);
        flag2 = flag1 & this.pathForCalc.get_status() == 0;
        float num1 = 0.0f;
        Vector3[] corners = this.pathForCalc.get_corners();
        for (int index = 0; index < corners.Length - 1; ++index)
        {
          float num2 = Vector3.Distance(corners[index], corners[index + 1]);
          num1 += num2;
          float num3 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
          if ((double) num1 > (double) num3)
          {
            flag2 = false;
            break;
          }
        }
      }
      else
        flag2 = false;
      return flag2;
    }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
        return Object.op_Inequality((Object) playerActor, (Object) null) && playerActor.PlayerController.State is Onbu ? (CommandLabel.CommandInfo[]) null : this._talkLabel;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return this._emptyLabel;
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return this._layer;
      }
    }

    public CommandType CommandType
    {
      get
      {
        return CommandType.Forward;
      }
    }

    public override ActorAnimation Animation
    {
      get
      {
        return (ActorAnimation) this._animation;
      }
    }

    public ActorAnimationMerchant AnimationMerchant
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
        return (ActorLocomotion) this._locomotor;
      }
    }

    public ActorLocomotionMerchant LocomotorMerchant
    {
      get
      {
        return this._locomotor;
      }
    }

    public override ActorController Controller
    {
      get
      {
        return (ActorController) this._controller;
      }
    }

    public MerchantController MerchantController
    {
      get
      {
        return this._controller;
      }
    }

    public override ICharacterInfo TiedInfo
    {
      get
      {
        return (ICharacterInfo) this.MerchantData;
      }
    }

    public MerchantData MerchantData { get; set; }

    public float DestinationSpeed { get; set; }

    public List<GameObject> ShipObjects { get; private set; }

    private int charaID
    {
      get
      {
        return this.MerchantData.param.charaID;
      }
    }

    private OpenData openData { get; } = new OpenData();

    private MerchantActor.PackData packData { get; set; }

    private void Awake()
    {
      this.BehaviorResources = (MerchantBehaviorTreeResources) ((Component) this).GetComponentInChildren<MerchantBehaviorTreeResources>(true);
      bool? enabled = ((Behaviour) this.Animation)?.get_enabled();
      this._prevAnimEnable = !enabled.HasValue || enabled.Value;
      bool? nullable1 = this._locomotor != null ? new bool?(((Behaviour) this._locomotor).get_enabled()) : new bool?();
      this._prevLocomEnable = !nullable1.HasValue || nullable1.Value;
      bool? nullable2 = this._controller != null ? new bool?(((Behaviour) this._controller).get_enabled()) : new bool?();
      this._prevConEnable = !nullable2.HasValue || nullable2.Value;
    }

    protected override void OnStart()
    {
      this.BehaviorResources.Initialize();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
    }

    [DebuggerHidden]
    public override IEnumerator LoadAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MerchantActor.\u003CLoadAsync\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    protected override IEnumerator LoadCharAsync(string fileName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MerchantActor.\u003CLoadCharAsync\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void ChangeMap(int mapID)
    {
      MerchantData merchantData = this.MerchantData;
      int num1 = mapID;
      this.MapID = num1;
      int num2 = num1;
      merchantData.MapID = num2;
      this.ResetState();
      if (this.MapID == 0)
      {
        this.DataUpdate(this.MerchantData);
      }
      else
      {
        this.ElapsedDay = false;
        if (Object.op_Inequality((Object) this.StartPoint, (Object) null))
        {
          Transform basePoint = this.StartPoint.GetBasePoint(Merchant.EventType.Wait);
          if (((Behaviour) this.NavMeshAgent).get_isActiveAndEnabled())
            this.NavMeshAgent.Warp(basePoint.get_position());
          else
            this.Position = basePoint.get_position();
          this.Rotation = basePoint.get_rotation();
          this.TargetInSightMerchantPoint = this.StartPoint;
          Merchant.ActionType actionType1 = Merchant.ActionType.Wait;
          this.LastNormalMode = actionType1;
          Merchant.ActionType actionType2 = actionType1;
          this.PrevMode = actionType2;
          this.CurrentMode = actionType2;
        }
        else
        {
          Merchant.ActionType actionType1 = Merchant.ActionType.Absent;
          this.LastNormalMode = actionType1;
          Merchant.ActionType actionType2 = actionType1;
          this.PrevMode = actionType2;
          this.CurrentMode = actionType2;
        }
      }
    }

    public void ResetState()
    {
      if (Singleton<Manager.Voice>.IsInstance())
        Singleton<Manager.Voice>.Instance.Stop(-90);
      this.Animation.RecoveryPoint = (Transform) null;
      this.Animation.EndIgnoreEvent();
      Singleton<Game>.Instance.GetExpression(-90, "標準")?.Change(this.ChaControl);
      this.Animation.ResetDefaultAnimatorController();
    }

    private void OnUpdate()
    {
      this.DataUpdate();
      this.UpdateEncounter();
      if (this._mapAreaID != null && Object.op_Inequality((Object) this.MapArea, (Object) null))
        ((ReactiveProperty<int>) this._mapAreaID).set_Value(this.MapArea.AreaID);
      if (Object.op_Inequality((Object) this.MapArea, (Object) null))
        this.MerchantData.AreaID = this.MapArea.AreaID;
      MerchantData merchantData = this.MerchantData;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
      if (simulator.EnabledTimeProgression)
      {
        Weather weather = simulator.Weather;
        if (this.AreaType == MapArea.AreaType.Indoor)
        {
          merchantData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
        }
        else
        {
          switch (weather)
          {
            case Weather.Rain:
              merchantData.Wetness += statusProfile.WetRateInRain * Time.get_deltaTime();
              break;
            case Weather.Storm:
              merchantData.Wetness += statusProfile.WetRateInStorm * Time.get_deltaTime();
              break;
            default:
              merchantData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
              break;
          }
          merchantData.Wetness = Mathf.Clamp(merchantData.Wetness, 0.0f, 100f);
        }
      }
      if (!Object.op_Inequality((Object) this.ChaControl, (Object) null))
        return;
      this.ChaControl.wetRate = Mathf.InverseLerp(0.0f, 100f, merchantData.Wetness);
    }

    private void DataUpdate()
    {
      this.MerchantData.MapID = this.MapID;
      if (this.MapID != 0)
        return;
      if (!this.IsActionMoving && ((Behaviour) this._navMeshAgent).get_isActiveAndEnabled() && this._navMeshAgent.get_isOnNavMesh())
      {
        this.MerchantData.Position = this.Position;
        this.MerchantData.Rotation = this.Rotation;
      }
      this.MerchantData.ChunkID = this.ChunkID;
      this.MerchantData.AreaID = this.AreaID;
      this.MerchantData.ModeType = this.CurrentMode;
      this.MerchantData.LastNormalModeType = this.LastNormalMode;
      MerchantData merchantData = this.MerchantData;
      int? id = this.CommandPartner?.ID;
      int num = !id.HasValue ? -1 : id.Value;
      merchantData.ActionTargetID = num;
      this.MerchantData.OpenAreaID = this.OpenAreaID;
      this.MerchantData.ElapsedDay = this.ElapsedDay;
    }

    public void DataUpdate(MerchantData data)
    {
      if (((Behaviour) this.NavMeshAgent).get_isActiveAndEnabled())
        this.NavMeshAgent.Warp(data.Position);
      else
        this.Position = data.Position;
      this.Rotation = data.Rotation;
      this.MapID = data.MapID;
      this.ChunkID = data.ChunkID;
      this.CurrentMode = data.ModeType;
      this.LastNormalMode = data.LastNormalModeType;
      this.OpenAreaID = data.OpenAreaID;
      this.ElapsedDay = data.MapID == 0 && data.ElapsedDay;
    }

    public void SetOpenAreaID(Manager.Map map)
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      IReadOnlyDictionary<int, List<ValueTuple<int, int>>> areaOpenState = Singleton<Resources>.Instance.MerchantProfile.AreaOpenState;
      List<ValueTuple<int, int>> source;
      if (areaOpenState == null || ((IReadOnlyCollection<KeyValuePair<int, List<ValueTuple<int, int>>>>) areaOpenState).get_Count() == 0 || (!areaOpenState.TryGetValue(map.MapID, ref source) || source.IsNullOrEmpty<ValueTuple<int, int>>()))
        return;
      this.OpenAreaID = 0;
      if (this.MerchantData != null)
        this.MerchantData.OpenAreaID = 0;
      source.Sort((Comparison<ValueTuple<int, int>>) ((x1, x2) => (int) (x1.Item1 - x2.Item1)));
      for (int index = 0; index < source.Count; ++index)
      {
        ValueTuple<int, int> valueTuple = source[index];
        if (!map.GetOpenAreaState((int) valueTuple.Item1))
          break;
        this.OpenAreaID = (int) valueTuple.Item2;
        if (this.MerchantData != null)
          this.MerchantData.OpenAreaID = (int) valueTuple.Item2;
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
        return (double) rangeSetting.leaveDistance < (double) this._distanceTweenPlayer || (double) rangeSetting.acceptableHeight < (double) this._heightDistTweenPlayer;
      }
    }

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
      return this.EventKey == EventType.Move || this.EventKey == EventType.DoorClose || this.EventKey == EventType.DoorOpen;
    }

    private void UpdateEncounter()
    {
      PlayerActor player = Manager.Map.GetPlayer();
      if (Object.op_Equality((Object) player, (Object) null))
        return;
      IState state = player.PlayerController.State;
      Vector3 position1 = this.Position;
      Vector3 position2 = player.Position;
      this._heightDistTweenPlayer = Mathf.Abs((float) (position2.y - position1.y));
      position1.y = (__Null) (double) (position2.y = (__Null) 0.0f);
      this._distanceTweenPlayer = Vector3.Distance(position1, position2);
      switch (state)
      {
        case Normal _:
        case Photo _:
          if (!this._prevCloseToPlayer && this.ActiveEncount && (this.Talkable && this.IsCloseToPlayer) && Merchant.EncountList.Contains(this.CurrentMode))
          {
            if (this.IsTraverseOffMeshLink())
              break;
            this._prevCloseToPlayer = true;
            this.ChangeBehavior(Merchant.ActionType.Encounter);
            break;
          }
          if (!this._prevCloseToPlayer || this.IsImpossible || !this.IsFarPlayer)
            break;
          this._prevCloseToPlayer = false;
          break;
      }
    }

    public void SetPointIDInfo(MerchantPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null) || this.MerchantData.MapID != 0)
        return;
      this.MerchantData.PointID = point.PointID;
      this.MerchantData.PointAreaID = point.AreaID;
      this.MerchantData.PointGroupID = point.GroupID;
      this.MerchantData.PointPosition = ((Component) point).get_transform().get_position();
      this.MerchantData.MainPointPosition = new Vector3(-999f, -999f, -999f);
    }

    public override void OnDayUpdated(TimeSpan timeSpan)
    {
      this.ElapsedDayCount = Mathf.Max(timeSpan.Days, 1);
      this.ElapsedDay = true;
      if (!this.MerchantData.Unlock || this.MapID != 0)
      {
        this.ElapsedDay = false;
      }
      else
      {
        if (this.IsActionMoving || Object.op_Inequality((Object) this.BehaviorResources, (Object) null) && !((Behaviour) this.BehaviorResources).get_enabled() || !Merchant.ChangeableModeList.Contains(this.CurrentMode))
          return;
        this.ChangeNextSchedule();
        this.SetCurrentSchedule();
      }
    }

    public override void OnMinuteUpdated(TimeSpan timeSpan)
    {
    }

    protected override void LoadEquipedEventItem(EquipEventItemInfo eventItemInfo)
    {
    }

    public override void LoadEventParticles(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary;
      Dictionary<int, List<AnimeParticleEventInfo>> eventTable;
      if (!Singleton<Resources>.Instance.Animation.MerchantCommonParticleEventKeyTable.TryGetValue(eventID, out dictionary) || !dictionary.TryGetValue(poseID, out eventTable) || eventTable == null)
        return;
      this.LoadEventParticle(eventTable);
    }

    public void LoadMerchantEventParticles(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary;
      Dictionary<int, List<AnimeParticleEventInfo>> eventTable;
      if (!Singleton<Resources>.Instance.Animation.MerchantOnlyParticleEventKeyTable.TryGetValue(eventID, out dictionary) || !dictionary.TryGetValue(poseID, out eventTable) || eventTable == null)
        return;
      this.LoadEventParticle(eventTable);
    }

    public void SetMerchantPoints(MerchantPoint[] merchantPoints)
    {
      this.StartPoint = (MerchantPoint) null;
      this.ExitPoint = (MerchantPoint) null;
      if (this.MerchantPoints == null)
        this.MerchantPoints = new List<MerchantPoint>();
      else
        this.MerchantPoints.Clear();
      if (this.MerchantPointTable == null)
        this.MerchantPointTable = new Dictionary<int, Dictionary<int, List<MerchantPoint>>>();
      else
        this.MerchantPointTable.Clear();
      if (merchantPoints.IsNullOrEmpty<MerchantPoint>())
        return;
      this.MerchantPoints.AddRange((IEnumerable<MerchantPoint>) merchantPoints);
      foreach (MerchantPoint merchantPoint in merchantPoints)
      {
        if (!this.MerchantPointTable.ContainsKey(merchantPoint.AreaID))
          this.MerchantPointTable[merchantPoint.AreaID] = new Dictionary<int, List<MerchantPoint>>();
        if (!this.MerchantPointTable[merchantPoint.AreaID].ContainsKey(merchantPoint.GroupID))
          this.MerchantPointTable[merchantPoint.AreaID][merchantPoint.GroupID] = new List<MerchantPoint>();
        this.MerchantPointTable[merchantPoint.AreaID][merchantPoint.GroupID].Add(merchantPoint);
        if (!Object.op_Equality((Object) merchantPoint, (Object) null))
        {
          if (merchantPoint.IsStartPoint)
            this.StartPoint = merchantPoint;
          if (merchantPoint.IsExitPoint)
            this.ExitPoint = merchantPoint;
        }
      }
      if (!Object.op_Inequality((Object) this.ExitPoint, (Object) null) || !Singleton<Resources>.IsInstance())
        return;
      this.ShipObjects = MapItemData.Get(Singleton<Resources>.Instance.MerchantProfile.MapShipItemID);
      List<ForcedHideObject> forcedHideObjectList = new List<ForcedHideObject>();
      if (!this.ShipObjects.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = this.ShipObjects.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (!Object.op_Equality((Object) current, (Object) null))
            {
              ForcedHideObject orAddComponent = current.GetOrAddComponent<ForcedHideObject>();
              if (!Object.op_Equality((Object) orAddComponent, (Object) null) && !forcedHideObjectList.Contains(orAddComponent))
              {
                orAddComponent.Init();
                forcedHideObjectList.Add(orAddComponent);
              }
            }
          }
        }
      }
      this.ExitPoint.ItemObjects = forcedHideObjectList;
    }

    public void ChangeBehavior(Merchant.ActionType mode)
    {
      if (mode != this.CurrentMode)
      {
        this.PrevMode = this.CurrentMode;
        this.CurrentMode = mode;
        if (Merchant.NormalModeList.Contains(this.CurrentMode))
          this.LastNormalMode = this.CurrentMode;
      }
      Merchant.StateType stateType;
      if (Merchant.StateTypeTable.TryGetValue(mode, out stateType))
        this.MerchantData.StateType = stateType;
      this.BehaviorResources.ChangeMode(mode);
    }

    private void OnEnable()
    {
      if (Object.op_Inequality((Object) this.Animation, (Object) null) && this._prevAnimEnable)
        ((Behaviour) this.Animation).set_enabled(true);
      if (Object.op_Inequality((Object) this.Locomotor, (Object) null) && this._prevLocomEnable)
        ((Behaviour) this.Locomotor).set_enabled(true);
      if (!Object.op_Inequality((Object) this.Controller, (Object) null) || !this._prevConEnable)
        return;
      ((Behaviour) this.Controller).set_enabled(true);
    }

    private void OnDisable()
    {
      if (Object.op_Inequality((Object) this.Animation, (Object) null) && (this._prevAnimEnable = ((Behaviour) this.Animation).get_enabled()))
        ((Behaviour) this.Animation).set_enabled(false);
      if (Object.op_Inequality((Object) this.Locomotor, (Object) null) && (this._prevLocomEnable = ((Behaviour) this.Locomotor).get_enabled()))
        ((Behaviour) this.Locomotor).set_enabled(false);
      if (!Object.op_Inequality((Object) this.Controller, (Object) null) || !(this._prevConEnable = ((Behaviour) this.Controller).get_enabled()))
        return;
      ((Behaviour) this.Controller).set_enabled(false);
    }

    public void Show()
    {
      ((Behaviour) this.AnimationMerchant).set_enabled(true);
      ((Behaviour) this.LocomotorMerchant).set_enabled(true);
      ((Behaviour) this.MerchantController).set_enabled(true);
      this.ChaControl.visibleAll = true;
      this.AnimationMerchant.EnableItems();
      this.AnimationMerchant.EnableParticleRenderer();
    }

    public void Hide()
    {
      ((Behaviour) this.AnimationMerchant).set_enabled(false);
      ((Behaviour) this.LocomotorMerchant).set_enabled(false);
      ((Behaviour) this.MerchantController).set_enabled(false);
      this.ChaControl.visibleAll = false;
      this.AnimationMerchant.DisableItems();
      this.AnimationMerchant.DisableParticleRenderer();
    }

    public override void EnableEntity()
    {
      if (this.CurrentMode != Merchant.ActionType.Absent)
        this.Show();
      this.EnableBehavior();
    }

    public override void DisableEntity()
    {
      this.Hide();
      this.DisableBehavior();
    }

    public void ActivateLocomotionMotion()
    {
      int key = 0;
      this._locomotor.MovePoseID = 0;
      PlayState info;
      Singleton<Resources>.Instance.Animation.MerchantLocomotionStateTable.TryGetValue(key, out info);
      if (info != null)
      {
        this.Animation.StopAllAnimCoroutine();
        Animator animator = this.Animation.Animator;
        PlayState.Info[] stateInfos = info.MainStateInfo.InStateInfo.StateInfos;
        if (!stateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
          if (!((AnimatorStateInfo) ref animatorStateInfo).IsName(((IEnumerable<PlayState.Info>) stateInfos).LastOrDefault<PlayState.Info>().stateName))
          {
            this.Animation.InitializeStates(info);
            int layer = info.Layer;
            this.Animation.PlayInLocoAnimation(info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, layer);
          }
        }
      }
      this.DestinationSpeed = this.GetLocomotionSpeed(this._locomotor.MovePoseID);
    }

    public void StartLocomotion(Vector3 destination)
    {
      float destinationSpeed = this.DestinationSpeed;
      float speed = this.NavMeshAgent.get_speed();
      this.ActivateNavMeshAgent();
      if (this.NavMeshAgent.get_isStopped())
        this.NavMeshAgent.set_isStopped(false);
      if (this.IsKinematic)
        this.IsKinematic = false;
      this.NavMeshAgent.SetDestination(destination);
      if (this._locomotionChangeSpeedDisposable != null)
        this._locomotionChangeSpeedDisposable.Dispose();
      this._locomotionChangeSpeedDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear(1f, false), ((Component) this).get_gameObject()), false), (System.Action<M0>) (x => this.NavMeshAgent.set_speed(Mathf.Lerp(speed, destinationSpeed, ((TimeInterval<float>) ref x).get_Value()))));
    }

    public float GetLocomotionSpeed(int movePoseID)
    {
      return Singleton<Resources>.Instance.LocomotionProfile.MerchantSpeed.walkSpeed;
    }

    public void DeactivateTransfer()
    {
      this.NavMeshAgent.set_isStopped(true);
      this.IsKinematic = true;
    }

    public new void ActivateNavMeshAgent()
    {
      if (((Behaviour) this.NavMeshObstacle).get_enabled())
        ((Behaviour) this.NavMeshObstacle).set_enabled(false);
      if (((Behaviour) this.NavMeshAgent).get_enabled())
        return;
      ((Behaviour) this.NavMeshAgent).set_enabled(true);
    }

    public new void DeactivateNavMeshAgent()
    {
      if (!((Behaviour) this.NavMeshAgent).get_enabled())
        return;
      ((Behaviour) this.NavMeshAgent).set_enabled(false);
    }

    public void ActivateNavMeshObstacle()
    {
      if (((Behaviour) this.NavMeshAgent).get_enabled())
        ((Behaviour) this.NavMeshAgent).set_enabled(false);
      if (((Behaviour) this.NavMeshObstacle).get_enabled())
        return;
      ((Behaviour) this.NavMeshObstacle).set_enabled(true);
    }

    public void ActivateNavMeshObstacle(Vector3 position)
    {
      ((Component) this.NavMeshObstacle).get_transform().set_position(position);
      if (((Behaviour) this.NavMeshAgent).get_enabled())
        ((Behaviour) this.NavMeshAgent).set_enabled(false);
      if (((Behaviour) this.NavMeshObstacle).get_enabled())
        return;
      ((Behaviour) this.NavMeshObstacle).set_enabled(true);
    }

    public void DeactivateNavMeshObjstacle()
    {
      if (!((Behaviour) this.NavMeshObstacle).get_enabled())
        return;
      ((Behaviour) this.NavMeshObstacle).set_enabled(false);
    }

    public void DeactivateNavMeshElement()
    {
      if (((Behaviour) this.NavMeshObstacle).get_enabled())
        ((Behaviour) this.NavMeshObstacle).set_enabled(false);
      if (!((Behaviour) this.NavMeshAgent).get_enabled())
        return;
      ((Behaviour) this.NavMeshAgent).set_enabled(false);
    }

    public bool IsInvalidMoveDestination()
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
        if (Object.op_Implicit((Object) ((OffMeshLinkData) ref nextOffMeshLinkData1).get_offMeshLink()))
        {
          OffMeshLinkData nextOffMeshLinkData2 = this._navMeshAgent.get_nextOffMeshLinkData();
          offMeshLink = ((OffMeshLinkData) ref nextOffMeshLinkData2).get_offMeshLink();
        }
      }
      if (Object.op_Equality((Object) offMeshLink, (Object) null))
        return false;
      ActionPoint component = (ActionPoint) ((Component) offMeshLink).GetComponent<ActionPoint>();
      if (Object.op_Equality((Object) component, (Object) null))
        return false;
      if (!component.IsNeutralCommand)
        return true;
      List<ActionPoint> connectedActionPoints = component.ConnectedActionPoints;
      if (connectedActionPoints != null)
      {
        foreach (ActionPoint actionPoint in connectedActionPoints)
        {
          if (Object.op_Inequality((Object) actionPoint, (Object) null) && !actionPoint.IsNeutralCommand)
            return true;
        }
      }
      return false;
    }

    public bool IsArrived()
    {
      NavMeshAgent navMeshAgent = this.NavMeshAgent;
      float distanceActionPoint = Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPoint;
      bool flag = false;
      if (((Behaviour) navMeshAgent).get_isActiveAndEnabled() && (navMeshAgent.get_hasPath() || navMeshAgent.get_pathPending()))
      {
        if (navMeshAgent.get_hasPath())
        {
          flag = (double) navMeshAgent.get_remainingDistance() <= (double) distanceActionPoint;
          if (flag && Object.op_Inequality((Object) this.TargetInSightMerchantPoint, (Object) null))
            flag = (double) Vector3.Distance(this.Position, this.TargetInSightMerchantPoint.Destination) <= (double) distanceActionPoint;
        }
      }
      else if (Object.op_Inequality((Object) this.TargetInSightMerchantPoint, (Object) null))
        flag = (double) Vector3.Distance(this.Position, this.TargetInSightMerchantPoint.Destination) <= (double) distanceActionPoint;
      return flag;
    }

    public bool IsArrivedToOffMesh()
    {
      OffMeshLinkData nextOffMeshLinkData = this.NavMeshAgent.get_nextOffMeshLinkData();
      OffMeshLink offMeshLink = ((OffMeshLinkData) ref nextOffMeshLinkData).get_offMeshLink();
      if (Object.op_Equality((Object) offMeshLink, (Object) null))
        return false;
      Transform startTransform = offMeshLink.get_startTransform();
      return !Object.op_Equality((Object) startTransform, (Object) null) && (double) Vector3.Distance(this.Position, startTransform.get_position()) <= (double) Singleton<Resources>.Instance.LocomotionProfile.ApproachDistanceActionPoint;
    }

    public void FirstAppear()
    {
      if (this.MapID != 0)
        this.ChangeBehavior(this.CurrentMode);
      else if (this.ScheduleList.IsNullOrEmpty<MerchantActor.MerchantSchedule>())
      {
        MerchantActor.MerchantSchedule merchantSchedule = new MerchantActor.MerchantSchedule()
        {
          AbsenceDay = false,
          Event = Merchant.ScheduleTaskTable[Merchant.StateType.Wait]
        };
        if (Object.op_Inequality((Object) this.StartPoint, (Object) null))
        {
          Tuple<MerchantPointInfo, Transform, Transform> eventInfo = this.StartPoint.GetEventInfo(Merchant.EventType.Wait);
          this.Position = eventInfo.Item2.get_position();
          this.Rotation = eventInfo.Item2.get_rotation();
        }
        this.ScheduleList.Clear();
        this.ScheduleList.Add(merchantSchedule);
        this.SetCurrentSchedule();
        MerchantPoint startPoint = this.StartPoint;
        this.TargetInSightMerchantPoint = startPoint;
        this.SetMerchantPoint(startPoint);
        this.ChangeBehavior(Merchant.ActionType.Wait);
      }
      else
      {
        if (0 <= this.MerchantData.PointAreaID && 0 <= this.MerchantData.PointGroupID)
        {
          int pointAreaId = this.MerchantData.PointAreaID;
          int pointGroupId = this.MerchantData.PointGroupID;
          Vector3 pointPosition = this.MerchantData.PointPosition;
          Vector3 mainPointPosition = this.MerchantData.MainPointPosition;
          List<MerchantPoint> merchantPointList = ListPool<MerchantPoint>.Get();
          this.TargetInSightMerchantPoint = (MerchantPoint) null;
          this.MainTargetInSightMerchantPoint = (MerchantPoint) null;
          foreach (MerchantPoint merchantPoint in this.MerchantPoints)
          {
            if (!Object.op_Equality((Object) merchantPoint, (Object) null))
            {
              if (Object.op_Equality((Object) this.TargetInSightMerchantPoint, (Object) null) && this.SamePosition(((Component) merchantPoint).get_transform().get_position(), pointPosition))
                this.TargetInSightMerchantPoint = merchantPoint;
              if (Object.op_Equality((Object) this.MainTargetInSightMerchantPoint, (Object) null) && this.SamePosition(((Component) merchantPoint).get_transform().get_position(), mainPointPosition))
                this.MainTargetInSightMerchantPoint = merchantPoint;
              if (merchantPoint.AreaID == pointAreaId && merchantPoint.GroupID == pointGroupId)
                merchantPointList.Add(merchantPoint);
            }
          }
          if (Object.op_Equality((Object) this.TargetInSightMerchantPoint, (Object) null) && !merchantPointList.IsNullOrEmpty<MerchantPoint>())
            this.TargetInSightMerchantPoint = merchantPointList.GetElement<MerchantPoint>(Random.Range(0, merchantPointList.Count));
          ListPool<MerchantPoint>.Release(merchantPointList);
        }
        bool elapsedDay = this.ElapsedDay;
        this.SetCurrentSchedule();
        this.ElapsedDay = elapsedDay;
      }
    }

    private bool SamePosition(Vector3 p0, Vector3 p1)
    {
      return Mathf.Approximately((float) p0.x, (float) p1.x) && Mathf.Approximately((float) p0.y, (float) p1.y) && Mathf.Approximately((float) p0.z, (float) p1.z);
    }

    public void SetMerchantPoint(MerchantPoint merchantPoint)
    {
      if (Object.op_Equality((Object) merchantPoint, (Object) null) || Object.op_Equality((Object) merchantPoint, (Object) this.CurrentMerchantPoint))
        return;
      if (Object.op_Inequality((Object) this.CurrentMerchantPoint, (Object) null))
        this.PrevMerchantPoint = this.CurrentMerchantPoint;
      this.CurrentMerchantPoint = merchantPoint;
      this.SetPointIDInfo(merchantPoint);
    }

    public void AddSchedule()
    {
      int? count = this.ScheduleList?.Count;
      int num1 = !count.HasValue ? 0 : count.Value;
      bool flag1 = false;
      bool flag2 = false;
      int oneCycle = Singleton<Resources>.Instance.MerchantProfile.OneCycle;
      int num2;
      int num3;
      if (this.ScheduleList.IsNullOrEmpty<MerchantActor.MerchantSchedule>())
      {
        num2 = Random.Range(2, oneCycle);
        num3 = num2 - 1;
      }
      else if (this.ScheduleList[num1 - 1].AbsenceDay)
      {
        num2 = Random.Range(2, oneCycle);
        num3 = num2 - 1;
        flag2 = true;
      }
      else
      {
        num2 = Random.Range(1, oneCycle);
        num3 = num2 - 1;
        flag1 = this.ScheduleList[num1 - 1].IsSearch;
      }
      for (int index1 = 0; index1 < oneCycle; ++index1)
      {
        MerchantActor.MerchantSchedule merchantSchedule = new MerchantActor.MerchantSchedule()
        {
          AbsenceDay = false,
          Event = (MerchantActor.MerchantSchedule.MerchantEvent) null
        };
        if (num3 == index1 || num2 == index1)
        {
          merchantSchedule.Event = Merchant.ScheduleTaskTable[Merchant.StateType.Absent];
          merchantSchedule.AbsenceDay = num2 == index1;
        }
        else if (flag1 || flag2)
        {
          merchantSchedule.Event = Merchant.ScheduleTaskTable[Merchant.StateType.Wait];
        }
        else
        {
          float searchSelectedRange = Singleton<Resources>.Instance.MerchantProfile.ToSearchSelectedRange;
          float num4 = Random.Range(0.0f, 100f);
          Merchant.StateType index2 = !Mathf.Approximately(searchSelectedRange, 0.0f) ? (!Mathf.Approximately(searchSelectedRange, 100f) ? ((double) num4 > (double) searchSelectedRange ? Merchant.StateType.Wait : Merchant.StateType.Search) : Merchant.StateType.Search) : Merchant.StateType.Wait;
          merchantSchedule.Event = Merchant.ScheduleTaskTable[index2];
        }
        flag1 = merchantSchedule.IsSearch;
        flag2 = merchantSchedule.AbsenceDay;
        this.ScheduleList.Add(merchantSchedule);
      }
    }

    public void ChangeNextSchedule()
    {
      int oneCycle = Singleton<Resources>.Instance.MerchantProfile.OneCycle;
      if (this.ScheduleList.Count < oneCycle)
        this.AddSchedule();
      if (this.CurrentSchedule != null)
        this.PrevSchedule = this.CurrentSchedule;
      this.CurrentSchedule = this.ScheduleList.Pop<MerchantActor.MerchantSchedule>();
      if (this.ScheduleList.Count >= oneCycle)
        return;
      this.AddSchedule();
    }

    public void SetCurrentSchedule()
    {
      int oneCycle = Singleton<Resources>.Instance.MerchantProfile.OneCycle;
      if (this.ScheduleList.Count < oneCycle)
        this.AddSchedule();
      if (this.CurrentSchedule == null)
        this.CurrentSchedule = this.ScheduleList.Pop<MerchantActor.MerchantSchedule>();
      if (this.ScheduleList.Count < oneCycle)
        this.AddSchedule();
      this.ElapsedDay = false;
      if (this.CurrentSchedule.AbsenceDay)
      {
        if (this.BehaviorResources.IsMatchCurrentTree(Merchant.ActionType.Absent))
          return;
        this.ChangeBehavior(Merchant.ActionType.Absent);
      }
      else
      {
        bool? absenceDay = this.PrevSchedule?.AbsenceDay;
        if ((!absenceDay.HasValue ? 0 : (absenceDay.Value ? 1 : 0)) != 0)
          this.ChangeBehavior(this.CurrentSchedule.Event.Purpose);
        else
          this.ChangeBehavior(this.CurrentSchedule.Event.Process);
      }
    }

    public void CrossFade()
    {
      Renderer componentInChildren1 = (Renderer) this.ChaControl.objBody.GetComponentInChildren<Renderer>();
      Renderer componentInChildren2 = (Renderer) this.ChaControl.objHead.GetComponentInChildren<Renderer>();
      if (((!Object.op_Inequality((Object) componentInChildren1, (Object) null) ? (false ? 1 : 0) : (componentInChildren1.get_isVisible() ? 1 : 0)) | (!Object.op_Inequality((Object) componentInChildren2, (Object) null) ? 0 : (componentInChildren2.get_isVisible() ? 1 : 0))) == 0)
        return;
      ActorCameraControl cameraControl = Singleton<Manager.Map>.Instance.Player.CameraControl;
      if ((double) Vector3.Distance(this.Position, ((Component) cameraControl).get_transform().get_position()) > (double) Singleton<Resources>.Instance.LocomotionProfile.CrossFadeEnableDistance)
        return;
      cameraControl.CrossFade.FadeStart(-1f);
    }

    private void StartCommunication()
    {
      this.packData.Init();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CommCompanion = (Actor) this;
      player.Controller.ChangeState("Communication");
      this.ChangeBehavior(Merchant.ActionType.TalkWithPlayer);
      this.TurnToActorCrossFade((Actor) player);
      this.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      this.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      this.ChaControl.ChangeLookEyesPtn(1);
      this.ChaControl.ChangeLookNeckPtn(1, 1f);
      ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.TimerFrame(1, (FrameCountType) 0), (System.Action<M0>) (_ => Manager.ADV.ChangeADVCamera((Actor) this)));
      MapUIContainer.SetVisibleHUD(false);
      Singleton<Manager.ADV>.Instance.TargetMerchant = this;
      this.PopupCommands();
    }

    private void TurnToActorCrossFade(Actor actor)
    {
      this.DisposeSequenceAction();
      Singleton<Manager.Map>.Instance.Player.CameraControl.CrossFade.FadeStart(-1f);
      this.Animation.StopAllAnimCoroutine();
      this.Animation.Animator.Play(Singleton<Resources>.Instance.DefinePack.AnimatorState.MerchantIdleState, 0);
      Vector3 vector3 = Vector3.op_Subtraction(actor.Position, this.Position);
      vector3.y = (__Null) 0.0;
      this.Rotation = Quaternion.LookRotation(vector3, Vector3.get_up());
    }

    private void EndCommunication()
    {
      MapUIContainer.SetVisibledSpendMoneyUI(false);
      MapUIContainer.SetActiveCommandList(false);
      this.VanishCommands();
      this.packData.Release();
    }

    private void LeaveADV()
    {
      MapUIContainer.SetVisibledSpendMoneyUI(false);
      this.openData.FindLoad("1", this.charaID, 0);
      this.packData.onComplete = (System.Action) (() => this.EndCommunication());
      this.packData.OpenAreaID = this.OpenAreaID;
      this.packData.restoreCommands = (CommCommandList.CommandInfo[]) null;
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }

    public void PopupCommands()
    {
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.UI);
      Singleton<Manager.Input>.Instance.SetupState();
      MapUIContainer.CommandList.CancelEvent = (System.Action) (() => this.LeaveADV());
    }

    public void OpenShopUI()
    {
      this.PopupCommands();
    }

    public void InitiateHScene()
    {
      this.PopupCommands();
    }

    private void HSceneEnter(int hMode)
    {
      this.ChangeBehavior(Merchant.ActionType.HWithPlayer);
      Singleton<HSceneManager>.Instance.HsceneEnter((Actor) this, hMode, (AgentActor) null, HSceneManager.HEvent.Normal);
    }

    public void VanishCommands()
    {
      Singleton<Manager.ADV>.Instance.Captions.EndADV(new System.Action(this.EnableBehavior));
      if (Object.op_Equality((Object) this.TargetInSightMerchantPoint, (Object) null))
        this.TargetInSightMerchantPoint = this.PrevMerchantPoint;
      MapUIContainer.SetVisibleHUD(true);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      player.CameraControl.Mode = CameraMode.Normal;
      player.Controller.ChangeState("Normal");
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      instance.ReserveState(Manager.Input.ValidType.Action);
      instance.SetupState();
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      player.ChaControl.visibleAll = true;
      this.ChaControl.ChangeLookEyesPtn(0);
      this.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.ChangeBehavior(this.LastNormalMode);
    }

    public bool CanSelectHCommand(string[] tagName)
    {
      return Game.isAdd01 && this.IsOnHMesh(tagName);
    }

    private bool IsOnHMesh(string[] tagName = null)
    {
      if (tagName.IsNullOrEmpty<string>())
        return false;
      LayerMask hlayer = Singleton<Resources>.Instance.DefinePack.MapDefines.HLayer;
      Vector3 position = this.Position;
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y + 15.0);
      int num = Mathf.Min(Physics.SphereCastNonAlloc(position, 7.5f, Vector3.get_down(), this._raycastHits, 25f, LayerMask.op_Implicit(hlayer)), this._raycastHits.Length);
      if (num == 0)
        return false;
      bool flag = false;
      for (int index1 = 0; index1 < num; ++index1)
      {
        RaycastHit raycastHit = this._raycastHits[index1];
        string tag = ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_tag();
        if (!tag.IsNullOrEmpty() && !(tag == "Untagged"))
        {
          string[] strArray = tagName;
          int index2 = 0;
          while (index2 < strArray.Length && !(flag = strArray[index2] == tag))
            ++index2;
          if (flag)
            break;
        }
      }
      return flag;
    }

    public enum ADV_CATEGORY
    {
      TALK = 0,
      EVENT = 1,
      H = 9,
      HScene = 10, // 0x0000000A
      TUTORIAL = 100, // 0x00000064
    }

    private class PackData : CharaPackData
    {
      public CommCommandList.CommandInfo[] restoreCommands { get; set; }

      public bool isHSuccess
      {
        get
        {
          return this.hMode > 0;
        }
      }

      public int hMode
      {
        get
        {
          ValData valData;
          return this.Vars == null || !this.Vars.TryGetValue(nameof (hMode), out valData) ? 0 : (int) valData.o;
        }
      }

      public string JumpTag { get; set; } = string.Empty;

      public int OpenAreaID { get; set; }

      public bool isClearFlag
      {
        get
        {
          if (!Singleton<Game>.IsInstance())
            return false;
          bool? cleared = Singleton<Game>.Instance.WorldData?.Cleared;
          return cleared.HasValue && cleared.Value;
        }
      }

      public bool isLesbian { get; set; }

      public bool isWeaknessH { get; set; }

      public override List<Program.Transfer> Create()
      {
        List<Program.Transfer> transferList = base.Create();
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "int", "OpenAreaID", string.Format("{0}", (object) this.OpenAreaID)));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isClearFlag", string.Format("{0}", (object) this.isClearFlag)));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isLesbian", string.Format("{0}", (object) this.isLesbian)));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isWeaknessH", string.Format("{0}", (object) this.isWeaknessH)));
        transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "string", "JumpTag", this.JumpTag));
        return transferList;
      }

      public override void Receive(TextScenario scenario)
      {
        base.Receive(scenario);
        if (this.restoreCommands == null)
          return;
        CommCommandList commandList = MapUIContainer.CommandList;
        commandList.Refresh(this.restoreCommands, commandList.CanvasGroup, (System.Action) null);
      }
    }

    [MessagePackObject(false)]
    public class MerchantSchedule
    {
      [Key(1)]
      public MerchantActor.MerchantSchedule.MerchantEvent Event;

      public MerchantSchedule()
      {
      }

      public MerchantSchedule(MerchantActor.MerchantSchedule _source)
      {
        if (_source == null)
          return;
        this.AbsenceDay = _source.AbsenceDay;
        this.SetEvent(_source.Event);
      }

      [Key(0)]
      public bool AbsenceDay { get; set; }

      public bool IsMatchState(Merchant.StateType stateType)
      {
        Merchant.StateType stateType1;
        return !this.AbsenceDay && this.Event != null && Merchant.StateTypeTable.TryGetValue(this.Event.Process, out stateType1) && stateType1 == stateType;
      }

      [IgnoreMember]
      public bool IsAbsent
      {
        get
        {
          return this.AbsenceDay || this.IsMatchState(Merchant.StateType.Absent);
        }
      }

      [IgnoreMember]
      public bool IsSearch
      {
        get
        {
          return this.IsMatchState(Merchant.StateType.Search);
        }
      }

      [IgnoreMember]
      public bool IsWait
      {
        get
        {
          return this.IsMatchState(Merchant.StateType.Wait);
        }
      }

      public void SetEvent(
        MerchantActor.MerchantSchedule.MerchantEvent @event)
      {
        if (@event == null)
          return;
        if (this.Event == null)
          this.Event = new MerchantActor.MerchantSchedule.MerchantEvent(@event.Process, @event.Purpose);
        else
          this.Event.SetEvent(@event);
      }

      [MessagePackObject(false)]
      public class MerchantEvent
      {
        public MerchantEvent(
          MerchantActor.MerchantSchedule.MerchantEvent @event)
        {
          this.SetEvent(@event);
        }

        public MerchantEvent(
          Tuple<Merchant.ActionType, Merchant.ActionType> @event)
        {
          this.SetEvent(@event);
        }

        public MerchantEvent(Merchant.ActionType process, Merchant.ActionType purpose)
        {
          this.SetEvent(process, purpose);
        }

        [Key(0)]
        public Merchant.ActionType Process { get; set; }

        [Key(1)]
        public Merchant.ActionType Purpose { get; set; }

        public void SetEvent(
          Tuple<Merchant.ActionType, Merchant.ActionType> @event)
        {
          this.Process = @event.Item1;
          this.Purpose = @event.Item2;
        }

        public void SetEvent(Merchant.ActionType process, Merchant.ActionType purpose)
        {
          this.Process = process;
          this.Purpose = purpose;
        }

        public void SetEvent(
          MerchantActor.MerchantSchedule.MerchantEvent @event)
        {
          this.Process = @event.Process;
          this.Purpose = @event.Purpose;
        }
      }
    }
  }
}
