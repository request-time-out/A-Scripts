// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using AIProject.Definitions;
using AIProject.Scene;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject.Animal
{
  public abstract class AnimalBase : MonoBehaviour, IAnimalActionPointUser, ICommandable
  {
    protected static RaycastHit[] raycastHits = new RaycastHit[4];
    protected static CommandLabel.CommandInfo[] emptyLabels = new CommandLabel.CommandInfo[0];
    [SerializeField]
    [DisableInPlayMode]
    private AnimalTypes _animalType;
    [SerializeField]
    [DisableInPlayMode]
    private int _animalTypeID;
    [SerializeField]
    [DisableInPlayMode]
    private BreedingTypes _breedingType;
    [SerializeField]
    private string _name;
    [SerializeField]
    private ItemIDKeyPair _itemID;
    protected bool active_;
    [NonSerialized]
    public AnimalModelInfo ModelInfo;
    private int? _instanceID;
    protected EyeLookControllerVer2 eyeController;
    protected NeckLookControllerVer3 neckController;
    protected Vector3? destination;
    [ShowInInspector]
    [ReadOnly]
    private AnimalState currentState_;
    protected AnimalSchedule schedule;
    [NonSerialized]
    public AnimalActionPoint actionPoint;
    protected Animator animator;
    private IDisposable inAnimDisposable;
    private IDisposable outAnimDisposable;
    protected AnimalPlayState CurrentAnimState;
    private ValueTuple<RuntimeAnimatorController, int, int, string> LastAnimState;
    protected GameObject bodyObject;
    protected Renderer[] bodyRenderers;
    protected GameObject[] bodyRendererObjects;
    protected ParticleSystemRenderer[] bodyParticleRenderers;
    protected GameObject[] bodyParticleObjects;
    private AnimalMarker _marker;
    private LabelTypes _labelType;
    private NavMeshPath pathForCalc;
    [SerializeField]
    private ObjectLayer _layer;
    [SerializeField]
    private CommandType _commandType;
    [SerializeField]
    private AnimalSearchActor _searchActor;
    [SerializeField]
    private bool _agentInsight;
    [SerializeField]
    private bool _onGroundCheck;
    [SerializeField]
    private float _labelOffsetY;
    [SerializeField]
    private bool _isCommandable;
    [SerializeField]
    protected Transform _labelPoint;
    private IDisposable everyUpdateDisposable;
    private IDisposable everyLateUpdateDisposable;
    private IDisposable everyFixedUpdateDisposable;
    [NonSerialized]
    public AIProject.TimeZone timeZone;
    [NonSerialized]
    public Weather weather;
    public System.Action OnDestroyEvent;
    private Dictionary<string, SkinnedMeshRenderer> _skinnedMeshRendererTable;

    protected AnimalBase()
    {
      base.\u002Ector();
    }

    public AnimalTypes AnimalType
    {
      get
      {
        return this._animalType;
      }
    }

    public BreedingTypes BreedingType
    {
      get
      {
        return this._breedingType;
      }
    }

    public ActionTypes ActionType { get; protected set; }

    public string Name
    {
      get
      {
        return this._name;
      }
    }

    public string IdentifierName { get; private set; }

    public virtual ItemIDKeyPair ItemID
    {
      get
      {
        return this._itemID;
      }
    }

    public StuffItemInfo ItemInfo
    {
      get
      {
        return !Singleton<Manager.Resources>.IsInstance() ? (StuffItemInfo) null : Singleton<Manager.Resources>.Instance.GameInfo.GetItem(this.ItemID.categoryID, this.ItemID.itemID);
      }
    }

    public virtual int NavMeshAreaMask
    {
      get
      {
        return -1;
      }
    }

    public string ObjName
    {
      get
      {
        return ((Object) ((Component) this).get_gameObject()).get_name();
      }
      set
      {
        ((Object) ((Component) this).get_gameObject()).set_name(value);
      }
    }

    public int AnimalID { get; protected set; }

    public int AnimalTypeID
    {
      get
      {
        return this._animalTypeID;
      }
    }

    public int ChunkID { get; protected set; }

    public int AreaID { get; protected set; }

    public MapArea CurrentMapArea { get; protected set; }

    public MapArea TargetMapArea { get; protected set; }

    public int MapAreaID { get; protected set; }

    public static string DefaultLocomotionParamName { get; } = "locomotion";

    public virtual bool Active
    {
      get
      {
        return this.active_ && ((Behaviour) this).get_isActiveAndEnabled();
      }
      set
      {
        this.active_ = value;
      }
    }

    public int InstanceID
    {
      get
      {
        return (!this._instanceID.HasValue ? (this._instanceID = new int?(((Object) this).GetInstanceID())) : this._instanceID).Value;
      }
    }

    public Transform Target { get; set; }

    public bool HasTarget
    {
      get
      {
        return Object.op_Inequality((Object) this.Target, (Object) null);
      }
    }

    public Transform FollowTarget { get; set; }

    public bool HasFollowTarget
    {
      get
      {
        return Object.op_Inequality((Object) this.FollowTarget, (Object) null);
      }
    }

    public Actor TargetActor { get; set; }

    public Actor FollowActor { get; set; }

    public bool HasFollowActor
    {
      get
      {
        return Object.op_Inequality((Object) this.FollowActor, (Object) null);
      }
    }

    public Transform LookTarget
    {
      set
      {
        if (Object.op_Implicit((Object) this.eyeController))
          this.eyeController.target = value;
        if (!Object.op_Implicit((Object) this.neckController))
          return;
        this.neckController.target = value;
      }
    }

    public bool LookWaitMode { get; protected set; }

    public void ClearTargetObject()
    {
      this.Target = (Transform) null;
      this.FollowTarget = (Transform) null;
      this.LookTarget = (Transform) null;
      this.TargetActor = (Actor) null;
      this.FollowActor = (Actor) null;
    }

    public virtual bool BadMood { get; set; }

    public virtual bool WaitMode { get; set; }

    public virtual bool Wait()
    {
      return this.WaitMode || this.LookWaitMode;
    }

    public bool HasDestination
    {
      get
      {
        return this.destination.HasValue;
      }
    }

    public AnimalState CurrentState
    {
      get
      {
        return this.currentState_;
      }
      set
      {
        if (this.currentState_ == value)
          return;
        this.PrevState = this.currentState_;
        this.currentState_ = value;
        float num = 0.0f;
        this.StateTimeLimit = num;
        this.StateCounter = num;
        this.StateChanged();
        if (value != AnimalState.Destroyed)
          return;
        this.Destroy();
      }
    }

    protected void SetDestroyState()
    {
      this.currentState_ = AnimalState.Destroyed;
    }

    public AnimalState PrevState { get; private set; }

    public AnimalState NextState { get; set; }

    public AnimalState BackupState { get; set; }

    public bool IsLovely
    {
      get
      {
        return this.CurrentState == AnimalState.LovelyFollow || this.CurrentState == AnimalState.LovelyIdle;
      }
    }

    public bool IsPrevLovely
    {
      get
      {
        return this.PrevState == AnimalState.LovelyFollow || this.PrevState == AnimalState.LovelyIdle;
      }
    }

    public bool EnabledStateUpdate { get; set; }

    public bool AutoChangeAnimation { get; set; }

    public bool AnimationEndUpdate { get; set; }

    public bool StateUpdatePossible
    {
      get
      {
        return this.AnimationEndUpdate && this.IsRunningAnimation == 1 || !this.AnimationEndUpdate;
      }
    }

    public float StateCounter { get; protected set; }

    public float StateTimeLimit { get; protected set; }

    public bool HasActionPoint
    {
      get
      {
        return Object.op_Inequality((Object) this.actionPoint, (Object) null);
      }
    }

    public bool AnimatorEmtpy
    {
      get
      {
        return Object.op_Equality((Object) this.animator, (Object) null);
      }
    }

    public bool AnimatorEnabled
    {
      get
      {
        return Object.op_Inequality((Object) this.animator, (Object) null) && ((Behaviour) this.animator).get_isActiveAndEnabled();
      }
    }

    public bool AnimatorControllerEnabled
    {
      get
      {
        return Object.op_Inequality((Object) this.animator, (Object) null) && ((Behaviour) this.animator).get_isActiveAndEnabled() && this.animator.get_isInitialized() && Object.op_Inequality((Object) this.animator.get_runtimeAnimatorController(), (Object) null);
      }
    }

    protected Dictionary<int, Dictionary<int, AnimalPlayState>> AnimCommonTable { get; private set; }

    protected ReadOnlyDictionary<AnimalState, LookState> LookStateTable { get; private set; }

    protected Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>>> ExpressionTable { get; private set; }

    protected Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>> CurrentExpressionTable { get; private set; }

    public bool PlayingInAnimation
    {
      get
      {
        return this.inAnimDisposable != null;
      }
    }

    public bool PlayingOutAnimation
    {
      get
      {
        return this.outAnimDisposable != null;
      }
    }

    public int IsRunningAnimation
    {
      get
      {
        if (this.PlayingInAnimation)
          return 0;
        return this.PlayingOutAnimation ? 2 : 1;
      }
    }

    private Queue<AnimalPlayState.StateInfo> InAnimStates { get; }

    private Queue<AnimalPlayState.StateInfo> OutAnimStates { get; }

    public virtual Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
      set
      {
        ((Component) this).get_transform().set_position(value);
      }
    }

    public Vector3 EulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_eulerAngles();
      }
      set
      {
        ((Component) this).get_transform().set_eulerAngles(value);
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this).get_transform().get_rotation();
      }
      set
      {
        ((Component) this).get_transform().set_rotation(value);
      }
    }

    public Vector3 LocalPosition
    {
      get
      {
        return ((Component) this).get_transform().get_localPosition();
      }
      set
      {
        ((Component) this).get_transform().set_localPosition(value);
      }
    }

    public Vector3 LocalEulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_localEulerAngles();
      }
      set
      {
        ((Component) this).get_transform().set_localEulerAngles(value);
      }
    }

    public Quaternion LocalRotation
    {
      get
      {
        return ((Component) this).get_transform().get_localRotation();
      }
      set
      {
        ((Component) this).get_transform().set_localRotation(value);
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this).get_transform().get_forward();
      }
    }

    public Vector3 Right
    {
      get
      {
        return ((Component) this).get_transform().get_right();
      }
    }

    public Vector3 Up
    {
      get
      {
        return ((Component) this).get_transform().get_up();
      }
    }

    public static bool CreateDisplay { get; set; } = true;

    public bool IsForcedBodyEnabled { get; private set; }

    public bool IsNormalBodyEnabled { get; private set; }

    public void SetForcedBodyEnabled(bool _enabled)
    {
      this.IsForcedBodyEnabled = _enabled;
      this.RefreshBodyEnabled();
    }

    public virtual bool BodyEnabled
    {
      get
      {
        bool flag = false;
        if (!((IReadOnlyList<Renderer>) this.bodyRenderers).IsNullOrEmpty<Renderer>())
        {
          foreach (Renderer bodyRenderer in this.bodyRenderers)
          {
            if (!Object.op_Equality((Object) bodyRenderer, (Object) null))
              flag |= bodyRenderer.get_enabled();
          }
        }
        if (!((IReadOnlyList<ParticleSystemRenderer>) this.bodyParticleRenderers).IsNullOrEmpty<ParticleSystemRenderer>())
        {
          foreach (ParticleSystemRenderer particleRenderer in this.bodyParticleRenderers)
          {
            if (!Object.op_Equality((Object) particleRenderer, (Object) null))
              flag |= ((Renderer) particleRenderer).get_enabled();
          }
        }
        return flag;
      }
      set
      {
        this.IsNormalBodyEnabled = value;
        this.RefreshBodyEnabled();
      }
    }

    private void RefreshBodyEnabled()
    {
      bool flag = this.IsNormalBodyEnabled && this.IsForcedBodyEnabled;
      if (!((IReadOnlyList<Renderer>) this.bodyRenderers).IsNullOrEmpty<Renderer>())
      {
        foreach (Renderer bodyRenderer in this.bodyRenderers)
        {
          if (Object.op_Inequality((Object) bodyRenderer, (Object) null) && bodyRenderer.get_enabled() != flag)
            bodyRenderer.set_enabled(flag);
        }
      }
      if (((IReadOnlyList<ParticleSystemRenderer>) this.bodyParticleRenderers).IsNullOrEmpty<ParticleSystemRenderer>())
        return;
      foreach (ParticleSystemRenderer particleRenderer in this.bodyParticleRenderers)
      {
        if (Object.op_Inequality((Object) particleRenderer, (Object) null) && ((Renderer) particleRenderer).get_enabled() != flag)
          ((Renderer) particleRenderer).set_enabled(flag);
      }
    }

    public bool BodyActive
    {
      get
      {
        bool flag = true;
        if (!((IReadOnlyList<GameObject>) this.bodyRendererObjects).IsNullOrEmpty<GameObject>())
        {
          foreach (GameObject bodyRendererObject in this.bodyRendererObjects)
          {
            if (!Object.op_Equality((Object) bodyRendererObject, (Object) null))
              flag &= bodyRendererObject.get_activeSelf();
          }
        }
        if (!((IReadOnlyList<GameObject>) this.bodyParticleObjects).IsNullOrEmpty<GameObject>())
        {
          foreach (GameObject bodyParticleObject in this.bodyParticleObjects)
          {
            if (!Object.op_Equality((Object) bodyParticleObject, (Object) null))
              flag &= bodyParticleObject.get_activeSelf();
          }
        }
        return flag;
      }
      set
      {
        bool flag = value;
        if (!((IReadOnlyList<GameObject>) this.bodyRendererObjects).IsNullOrEmpty<GameObject>())
        {
          foreach (GameObject bodyRendererObject in this.bodyRendererObjects)
          {
            if (Object.op_Inequality((Object) bodyRendererObject, (Object) null) && bodyRendererObject.get_activeSelf() != flag)
              bodyRendererObject.SetActive(flag);
          }
        }
        if (((IReadOnlyList<GameObject>) this.bodyParticleObjects).IsNullOrEmpty<GameObject>())
          return;
        foreach (GameObject bodyParticleObject in this.bodyParticleObjects)
        {
          if (Object.op_Inequality((Object) bodyParticleObject, (Object) null) && bodyParticleObject.get_activeSelf() != flag)
            bodyParticleObject.SetActive(flag);
        }
      }
    }

    public bool MarkerEnabled
    {
      get
      {
        return !Object.op_Equality((Object) this._marker, (Object) null) && ((Behaviour) this._marker).get_enabled();
      }
      set
      {
        if (!Object.op_Inequality((Object) this._marker, (Object) null) || ((Behaviour) this._marker).get_enabled() == value)
          return;
        ((Behaviour) this._marker).set_enabled(value);
      }
    }

    public void AttachMarker()
    {
      this._marker = ((Component) this).get_gameObject().GetOrAddComponent<AnimalMarker>();
    }

    public void DetachMarker()
    {
      this._marker = (AnimalMarker) ((Component) this).GetComponentInChildren<AnimalMarker>(true);
      if (!Object.op_Inequality((Object) this._marker, (Object) null))
        return;
      Object.Destroy((Object) this._marker);
      this._marker = (AnimalMarker) null;
    }

    public virtual void DesireMax(DesireType _desireType)
    {
    }

    public LabelTypes PrevLabelType { get; private set; }

    public LabelTypes LabelType
    {
      get
      {
        return this._labelType;
      }
      set
      {
        if (this._labelType != value)
        {
          this.PrevLabelType = this._labelType;
          this._labelType = value;
        }
        this.RefreshCommands(true);
      }
    }

    public virtual bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (!this._isCommandable || this.BadMood)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      if (Object.op_Inequality((Object) player, (Object) null) && player.Mode == Desire.ActionType.Onbu)
        return false;
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num1 = Vector3.Distance(basePosition, commandCenter);
      if (this._commandType == CommandType.Forward)
      {
        if ((double) radiusA < (double) num1)
          return false;
        Vector3 vector3 = commandCenter;
        vector3.y = (__Null) 0.0;
        float num2 = angle / 2f;
        return (double) Vector3.Angle(Vector3.op_Subtraction(vector3, basePosition), forward) <= (double) num2;
      }
      float num3 = radiusB;
      return (double) distance <= (double) num3;
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

    public bool IsImpossible { get; protected set; }

    public Actor CommandPartner { get; set; }

    public virtual bool SetImpossible(bool _value, Actor _actor)
    {
      if (this.IsImpossible == _value)
        return false;
      if (_value)
      {
        if (Object.op_Inequality((Object) this.CommandPartner, (Object) null) && Object.op_Inequality((Object) this.CommandPartner, (Object) _actor))
          return false;
      }
      else if (Object.op_Inequality((Object) this.CommandPartner, (Object) _actor))
        return false;
      this.IsImpossible = _value;
      this.CommandPartner = !_value ? (Actor) null : _actor;
      this.RefreshCommands(true);
      return true;
    }

    public virtual bool IsNeutralCommand
    {
      get
      {
        return this._isCommandable;
      }
    }

    public virtual CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return AnimalBase.emptyLabels;
      }
    }

    public virtual CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return AnimalBase.emptyLabels;
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
        return this._commandType;
      }
    }

    public virtual Vector3 CommandCenter
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    protected virtual void InitializeCommandLabels()
    {
    }

    public AnimalSearchActor SearchActor
    {
      get
      {
        return this._searchActor;
      }
    }

    public bool AgentInsight
    {
      get
      {
        return this._agentInsight;
      }
    }

    public bool OnGroundCheck
    {
      get
      {
        return this._onGroundCheck;
      }
    }

    public float LabelOffsetY
    {
      get
      {
        return this._labelOffsetY;
      }
    }

    public bool IsCommandable
    {
      get
      {
        return this._isCommandable;
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._labelPoint, (Object) null) ? this._labelPoint : ((Component) this).get_transform();
      }
    }

    private void InitializeActorHitEvent()
    {
      if (Object.op_Equality((Object) this.SearchActor, (Object) null))
        return;
      this.SearchActor.OnFarPlayerActorEnterEvent = (System.Action<PlayerActor>) (x => this.OnFarPlayerActorEnter(x));
      this.SearchActor.OnFarPlayerActorStayEvent = (System.Action<PlayerActor>) (x => this.OnFarPlayerActorStay(x));
      this.SearchActor.OnFarPlayerActorExitEvent = (System.Action<PlayerActor>) (x => this.OnFarPlayerActorExit(x));
      this.SearchActor.OnFarAgentActorEnterEvent = (System.Action<AgentActor>) (x => this.OnFarAgentActorEnter(x));
      this.SearchActor.OnFarAgentActorStayEvent = (System.Action<AgentActor>) (x => this.OnFarAgentActorStay(x));
      this.SearchActor.OnFarAgentActorExitEvent = (System.Action<AgentActor>) (x => this.OnFarAgentActorExit(x));
      this.SearchActor.OnFarActorEnterEvent = (System.Action<Actor>) (x => this.OnFarActorEnter(x));
      this.SearchActor.OnFarActorStayEvent = (System.Action<Actor>) (x => this.OnFarActorStay(x));
      this.SearchActor.OnFarActorExitEvent = (System.Action<Actor>) (x => this.OnFarActorExit(x));
      this.SearchActor.OnNearPlayerActorEnterEvent = (System.Action<PlayerActor>) (x => this.OnNearPlayerActorEnter(x));
      this.SearchActor.OnNearPlayerActorStayEvent = (System.Action<PlayerActor>) (x => this.OnNearPlayerActorStay(x));
      this.SearchActor.OnNearPlayerActorExitEvent = (System.Action<PlayerActor>) (x => this.OnNearPlayerActorExit(x));
      this.SearchActor.OnNearAgentActorEnterEvent = (System.Action<AgentActor>) (x => this.OnNearAgentActorEnter(x));
      this.SearchActor.OnNearAgentActorStayEvent = (System.Action<AgentActor>) (x => this.OnNearAgentActorStay(x));
      this.SearchActor.OnNearAgentActorExitEvent = (System.Action<AgentActor>) (x => this.OnNearAgentActorExit(x));
      this.SearchActor.OnNearActorEnterEvent = (System.Action<Actor>) (x => this.OnNearActorEnter(x));
      this.SearchActor.OnNearActorStayEvent = (System.Action<Actor>) (x => this.OnNearActorStay(x));
      this.SearchActor.OnNearActorExitEvent = (System.Action<Actor>) (x => this.OnNearActorExit(x));
    }

    protected virtual void OnFarPlayerActorEnter(PlayerActor _player)
    {
    }

    protected virtual void OnFarPlayerActorStay(PlayerActor _player)
    {
    }

    protected virtual void OnFarPlayerActorExit(PlayerActor _player)
    {
    }

    protected virtual void OnFarAgentActorEnter(AgentActor _agent)
    {
    }

    protected virtual void OnFarAgentActorStay(AgentActor _agent)
    {
    }

    protected virtual void OnFarAgentActorExit(AgentActor _agent)
    {
    }

    protected virtual void OnFarActorEnter(Actor _actor)
    {
    }

    protected virtual void OnFarActorStay(Actor _actor)
    {
    }

    protected virtual void OnFarActorExit(Actor _actor)
    {
    }

    protected virtual void OnNearPlayerActorEnter(PlayerActor _player)
    {
    }

    protected virtual void OnNearPlayerActorStay(PlayerActor _player)
    {
    }

    protected virtual void OnNearPlayerActorExit(PlayerActor _player)
    {
    }

    protected virtual void OnNearAgentActorEnter(AgentActor _agent)
    {
    }

    protected virtual void OnNearAgentActorStay(AgentActor _agent)
    {
    }

    protected virtual void OnNearAgentActorExit(AgentActor _agent)
    {
    }

    protected virtual void OnNearActorEnter(Actor _actor)
    {
    }

    protected virtual void OnNearActorStay(Actor _actor)
    {
    }

    protected virtual void OnNearActorExit(Actor _actor)
    {
    }

    protected virtual void Awake()
    {
      this.IdentifierName = this.AnimalType.ToString();
      if (this._animalTypeID < 0)
        this._animalTypeID = AnimalData.GetAnimalTypeID(this._animalType);
      else if (this._animalType != (AnimalTypes) (1 << this._animalTypeID))
        this._animalType = (AnimalTypes) (1 << this._animalTypeID);
      this._marker = (AnimalMarker) ((Component) this).GetComponentInChildren<AnimalMarker>(true);
      this.MarkerEnabled = false;
      this.InitializeCommandLabels();
      this.InitializeActorHitEvent();
      StuffItemInfo itemInfo = this.ItemInfo;
      if (itemInfo != null)
        this._name = itemInfo.Name;
      EnvironmentSimulator environmentSimulator = !Singleton<Manager.Map>.IsInstance() ? (EnvironmentSimulator) null : Singleton<Manager.Map>.Instance.Simulator;
      this.timeZone = !Object.op_Inequality((Object) environmentSimulator, (Object) null) ? AIProject.TimeZone.Morning : environmentSimulator.TimeZone;
      this.weather = !Object.op_Inequality((Object) environmentSimulator, (Object) null) ? Weather.Clear : environmentSimulator.Weather;
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void Start()
    {
      this.SetUpdateObservable();
    }

    private void SetUpdateObservable()
    {
      if (this.everyUpdateDisposable != null)
        this.everyUpdateDisposable.Dispose();
      if (this.everyLateUpdateDisposable != null)
        this.everyLateUpdateDisposable.Dispose();
      if (this.everyFixedUpdateDisposable != null)
        this.everyFixedUpdateDisposable.Dispose();
      this.everyUpdateDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ =>
      {
        this.OnUpdateFirst();
        this.OnUpdate();
        this.OnUpdateEnd();
      }));
      this.everyLateUpdateDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ =>
      {
        this.OnLateUpdateFirst();
        this.OnLateUpdate();
        this.OnLateUpdateEnd();
      }));
      this.everyFixedUpdateDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryFixedUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ =>
      {
        this.OnFixedUpdateFirst();
        this.OnFixedUpdate();
        this.OnFixedUpdateEnd();
      }));
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void OnUpdate()
    {
      if (this.StateUpdatePossible)
        this.StateUpdate();
      if (!this.AnimatorControllerEnabled)
        return;
      this.StateAnimationUpdate();
    }

    protected virtual void OnUpdateFirst()
    {
    }

    protected virtual void OnUpdateEnd()
    {
    }

    protected virtual void OnLateUpdateFirst()
    {
    }

    protected virtual void OnLateUpdate()
    {
    }

    protected virtual void OnLateUpdateEnd()
    {
    }

    protected virtual void OnFixedUpdateFirst()
    {
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void OnFixedUpdateEnd()
    {
    }

    protected virtual void ChangedStateEvent()
    {
    }

    private void StateChanged()
    {
      this.StateExit();
      if (this.CurrentState != AnimalState.Destroyed)
        this.ChangedStateEvent();
      this.StateEnter();
    }

    private void StateEnter()
    {
      switch (this.currentState_)
      {
        case AnimalState.Start:
          this.EnterStart();
          break;
        case AnimalState.Repop:
          this.EnterRepop();
          break;
        case AnimalState.Depop:
          this.EnterDepop();
          break;
        case AnimalState.Idle:
          this.EnterIdle();
          break;
        case AnimalState.Wait:
          this.EnterWait();
          break;
        case AnimalState.SitWait:
          this.EnterSitWait();
          break;
        case AnimalState.Locomotion:
          this.EnterLocomotion();
          break;
        case AnimalState.LovelyIdle:
          this.EnterLovelyIdle();
          break;
        case AnimalState.LovelyFollow:
          this.EnterLovelyFollow();
          break;
        case AnimalState.Escape:
          this.EnterEscape();
          break;
        case AnimalState.Swim:
          this.EnterSwim();
          break;
        case AnimalState.Sleep:
          this.EnterSleep();
          break;
        case AnimalState.Toilet:
          this.EnterToilet();
          break;
        case AnimalState.Rest:
          this.EnterRest();
          break;
        case AnimalState.Eat:
          this.EnterEat();
          break;
        case AnimalState.Drink:
          this.EnterDrink();
          break;
        case AnimalState.Actinidia:
          this.EnterActinidia();
          break;
        case AnimalState.Grooming:
          this.EnterGrooming();
          break;
        case AnimalState.MoveEars:
          this.EnterMoveEars();
          break;
        case AnimalState.Roar:
          this.EnterRoar();
          break;
        case AnimalState.Peck:
          this.EnterPeck();
          break;
        case AnimalState.ToDepop:
          this.EnterToDepop();
          break;
        case AnimalState.ToIndoor:
          this.EnterToIndoor();
          break;
        case AnimalState.Action0:
          this.EnterAction0();
          break;
        case AnimalState.Action1:
          this.EnterAction1();
          break;
        case AnimalState.Action2:
          this.EnterAction2();
          break;
        case AnimalState.Action3:
          this.EnterAction3();
          break;
        case AnimalState.Action4:
          this.EnterAction4();
          break;
        case AnimalState.Action5:
          this.EnterAction5();
          break;
        case AnimalState.Action6:
          this.EnterAction6();
          break;
        case AnimalState.Action7:
          this.EnterAction7();
          break;
        case AnimalState.Action8:
          this.EnterAction8();
          break;
        case AnimalState.Action9:
          this.EnterAction9();
          break;
        case AnimalState.WithPlayer:
          this.EnterWithPlayer();
          break;
        case AnimalState.WithAgent:
          this.EnterWithAgent();
          break;
        case AnimalState.WithMerchant:
          this.EnterWithMerchant();
          break;
      }
      if (!this.AutoChangeAnimation)
        ;
    }

    private void StateUpdate()
    {
      if (!this.EnabledStateUpdate)
        return;
      switch (this.currentState_)
      {
        case AnimalState.Start:
          this.OnStart();
          break;
        case AnimalState.Repop:
          this.OnRepop();
          break;
        case AnimalState.Depop:
          this.OnDepop();
          break;
        case AnimalState.Idle:
          this.OnIdle();
          break;
        case AnimalState.Wait:
          this.OnWait();
          break;
        case AnimalState.SitWait:
          this.OnSitWait();
          break;
        case AnimalState.Locomotion:
          this.OnLocomotion();
          break;
        case AnimalState.LovelyIdle:
          this.OnLovelyIdle();
          break;
        case AnimalState.LovelyFollow:
          this.OnLovelyFollow();
          break;
        case AnimalState.Escape:
          this.OnEscape();
          break;
        case AnimalState.Swim:
          this.OnSwim();
          break;
        case AnimalState.Sleep:
          this.OnSleep();
          break;
        case AnimalState.Toilet:
          this.OnToilet();
          break;
        case AnimalState.Rest:
          this.OnRest();
          break;
        case AnimalState.Eat:
          this.OnEat();
          break;
        case AnimalState.Drink:
          this.OnDrink();
          break;
        case AnimalState.Actinidia:
          this.OnActinidia();
          break;
        case AnimalState.Grooming:
          this.OnGrooming();
          break;
        case AnimalState.MoveEars:
          this.OnMoveEars();
          break;
        case AnimalState.Roar:
          this.OnRoar();
          break;
        case AnimalState.Peck:
          this.OnPeck();
          break;
        case AnimalState.ToDepop:
          this.OnToDepop();
          break;
        case AnimalState.ToIndoor:
          this.OnToIndoor();
          break;
        case AnimalState.Action0:
          this.OnAction0();
          break;
        case AnimalState.Action1:
          this.OnAction1();
          break;
        case AnimalState.Action2:
          this.OnAction2();
          break;
        case AnimalState.Action3:
          this.OnAction3();
          break;
        case AnimalState.Action4:
          this.OnAction4();
          break;
        case AnimalState.Action5:
          this.OnAction5();
          break;
        case AnimalState.Action6:
          this.OnAction6();
          break;
        case AnimalState.Action7:
          this.OnAction7();
          break;
        case AnimalState.Action8:
          this.OnAction8();
          break;
        case AnimalState.Action9:
          this.OnAction9();
          break;
        case AnimalState.WithPlayer:
          this.OnWithPlayer();
          break;
        case AnimalState.WithAgent:
          this.OnWithAgent();
          break;
        case AnimalState.WithMerchant:
          this.OnWithMerchant();
          break;
      }
    }

    private void StateExit()
    {
      switch (this.PrevState)
      {
        case AnimalState.Start:
          this.ExitStart();
          break;
        case AnimalState.Repop:
          this.ExitRepop();
          break;
        case AnimalState.Depop:
          this.ExitDepop();
          break;
        case AnimalState.Idle:
          this.ExitIdle();
          break;
        case AnimalState.Wait:
          this.ExitWait();
          break;
        case AnimalState.SitWait:
          this.ExitSitWait();
          break;
        case AnimalState.Locomotion:
          this.ExitLocomotion();
          break;
        case AnimalState.LovelyIdle:
          this.ExitLovelyIdle();
          break;
        case AnimalState.LovelyFollow:
          this.ExitLovelyFollow();
          break;
        case AnimalState.Escape:
          this.ExitEscape();
          break;
        case AnimalState.Swim:
          this.ExitSwim();
          break;
        case AnimalState.Sleep:
          this.ExitSleep();
          break;
        case AnimalState.Toilet:
          this.ExitToilet();
          break;
        case AnimalState.Rest:
          this.ExitRest();
          break;
        case AnimalState.Eat:
          this.ExitEat();
          break;
        case AnimalState.Drink:
          this.ExitDrink();
          break;
        case AnimalState.Actinidia:
          this.ExitActinidia();
          break;
        case AnimalState.Grooming:
          this.ExitGrooming();
          break;
        case AnimalState.MoveEars:
          this.ExitMoveEars();
          break;
        case AnimalState.Roar:
          this.ExitRoar();
          break;
        case AnimalState.Peck:
          this.ExitPeck();
          break;
        case AnimalState.ToDepop:
          this.ExitToDepop();
          break;
        case AnimalState.ToIndoor:
          this.ExitToIndoor();
          break;
        case AnimalState.Action0:
          this.ExitAction0();
          break;
        case AnimalState.Action1:
          this.ExitAction1();
          break;
        case AnimalState.Action2:
          this.ExitAction2();
          break;
        case AnimalState.Action3:
          this.ExitAction3();
          break;
        case AnimalState.Action4:
          this.ExitAction4();
          break;
        case AnimalState.Action5:
          this.ExitAction5();
          break;
        case AnimalState.Action6:
          this.ExitAction6();
          break;
        case AnimalState.Action7:
          this.ExitAction7();
          break;
        case AnimalState.Action8:
          this.ExitAction8();
          break;
        case AnimalState.Action9:
          this.ExitAction9();
          break;
        case AnimalState.WithPlayer:
          this.ExitWithPlayer();
          break;
        case AnimalState.WithAgent:
          this.ExitWithAgent();
          break;
        case AnimalState.WithMerchant:
          this.ExitWithMerchant();
          break;
      }
    }

    private void StateAnimationUpdate()
    {
      switch (this.currentState_)
      {
        case AnimalState.Start:
          this.AnimationStart();
          break;
        case AnimalState.Repop:
          this.AnimationRepop();
          break;
        case AnimalState.Depop:
          this.AnimationDepop();
          break;
        case AnimalState.Idle:
          this.AnimationIdle();
          break;
        case AnimalState.Wait:
          this.AnimationWait();
          break;
        case AnimalState.SitWait:
          this.AnimationSitWait();
          break;
        case AnimalState.Locomotion:
          this.AnimationLocomotion();
          break;
        case AnimalState.LovelyIdle:
          this.AnimationLovelyIdle();
          break;
        case AnimalState.LovelyFollow:
          this.AnimationLovelyFollow();
          break;
        case AnimalState.Escape:
          this.AnimationEscape();
          break;
        case AnimalState.Swim:
          this.AnimationSwim();
          break;
        case AnimalState.Sleep:
          this.AnimationSleep();
          break;
        case AnimalState.Toilet:
          this.AnimationToilet();
          break;
        case AnimalState.Rest:
          this.AnimationRest();
          break;
        case AnimalState.Eat:
          this.AnimationEat();
          break;
        case AnimalState.Drink:
          this.AnimationDrink();
          break;
        case AnimalState.Actinidia:
          this.AnimationActinidia();
          break;
        case AnimalState.Grooming:
          this.AnimationGrooming();
          break;
        case AnimalState.MoveEars:
          this.AnimationMoveEars();
          break;
        case AnimalState.Roar:
          this.AnimationRoar();
          break;
        case AnimalState.Peck:
          this.AnimationPeck();
          break;
        case AnimalState.ToDepop:
          this.AnimationToDepop();
          break;
        case AnimalState.ToIndoor:
          this.AnimationToIndoor();
          break;
        case AnimalState.Action0:
          this.AnimationAction0();
          break;
        case AnimalState.Action1:
          this.AnimationAction1();
          break;
        case AnimalState.Action2:
          this.AnimationAction2();
          break;
        case AnimalState.Action3:
          this.AnimationAction3();
          break;
        case AnimalState.Action4:
          this.AnimationAction4();
          break;
        case AnimalState.Action5:
          this.AnimationAction5();
          break;
        case AnimalState.Action6:
          this.AnimationAction6();
          break;
        case AnimalState.Action7:
          this.AnimationAction7();
          break;
        case AnimalState.Action8:
          this.AnimationAction8();
          break;
        case AnimalState.Action9:
          this.AnimationAction9();
          break;
        case AnimalState.WithPlayer:
          this.AnimationWithPlayer();
          break;
        case AnimalState.WithAgent:
          this.AnimationWithAgent();
          break;
        case AnimalState.WithMerchant:
          this.AnimationWithMerchant();
          break;
      }
    }

    [DebuggerHidden]
    protected virtual IEnumerator PrepareStart()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimalBase.\u003CPrepareStart\u003Ec__Iterator0 prepareStartCIterator0 = new AnimalBase.\u003CPrepareStart\u003Ec__Iterator0();
      return (IEnumerator) prepareStartCIterator0;
    }

    protected virtual void EnterStart()
    {
    }

    protected virtual void EnterRepop()
    {
    }

    protected virtual void EnterDepop()
    {
    }

    protected virtual void EnterIdle()
    {
    }

    protected virtual void EnterWait()
    {
    }

    protected virtual void EnterSitWait()
    {
    }

    protected virtual void EnterLocomotion()
    {
    }

    protected virtual void EnterLovelyIdle()
    {
    }

    protected virtual void EnterLovelyFollow()
    {
    }

    protected virtual void EnterEscape()
    {
    }

    protected virtual void EnterSwim()
    {
    }

    protected virtual void EnterSleep()
    {
    }

    protected virtual void EnterToilet()
    {
    }

    protected virtual void EnterRest()
    {
    }

    protected virtual void EnterEat()
    {
    }

    protected virtual void EnterDrink()
    {
    }

    protected virtual void EnterActinidia()
    {
    }

    protected virtual void EnterGrooming()
    {
    }

    protected virtual void EnterMoveEars()
    {
    }

    protected virtual void EnterRoar()
    {
    }

    protected virtual void EnterPeck()
    {
    }

    protected virtual void EnterToDepop()
    {
    }

    protected virtual void EnterToIndoor()
    {
    }

    protected virtual void EnterAction0()
    {
    }

    protected virtual void EnterAction1()
    {
    }

    protected virtual void EnterAction2()
    {
    }

    protected virtual void EnterAction3()
    {
    }

    protected virtual void EnterAction4()
    {
    }

    protected virtual void EnterAction5()
    {
    }

    protected virtual void EnterAction6()
    {
    }

    protected virtual void EnterAction7()
    {
    }

    protected virtual void EnterAction8()
    {
    }

    protected virtual void EnterAction9()
    {
    }

    protected virtual void EnterWithPlayer()
    {
      this.AutoChangeAnimation = false;
      this.AnimationEndUpdate = false;
    }

    protected virtual void EnterWithAgent()
    {
      this.AutoChangeAnimation = false;
      this.AnimationEndUpdate = false;
    }

    protected virtual void EnterWithMerchant()
    {
      this.AutoChangeAnimation = false;
      this.AnimationEndUpdate = false;
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnRepop()
    {
    }

    protected virtual void OnDepop()
    {
    }

    protected virtual void OnIdle()
    {
    }

    protected virtual void OnWait()
    {
    }

    protected virtual void OnSitWait()
    {
    }

    protected virtual void OnLocomotion()
    {
    }

    protected virtual void OnLovelyIdle()
    {
    }

    protected virtual void OnLovelyFollow()
    {
    }

    protected virtual void OnEscape()
    {
    }

    protected virtual void OnSwim()
    {
    }

    protected virtual void OnSleep()
    {
    }

    protected virtual void OnToilet()
    {
    }

    protected virtual void OnRest()
    {
    }

    protected virtual void OnEat()
    {
    }

    protected virtual void OnDrink()
    {
    }

    protected virtual void OnActinidia()
    {
    }

    protected virtual void OnGrooming()
    {
    }

    protected virtual void OnMoveEars()
    {
    }

    protected virtual void OnRoar()
    {
    }

    protected virtual void OnPeck()
    {
    }

    protected virtual void OnToDepop()
    {
    }

    protected virtual void OnToIndoor()
    {
    }

    protected virtual void OnAction0()
    {
    }

    protected virtual void OnAction1()
    {
    }

    protected virtual void OnAction2()
    {
    }

    protected virtual void OnAction3()
    {
    }

    protected virtual void OnAction4()
    {
    }

    protected virtual void OnAction5()
    {
    }

    protected virtual void OnAction6()
    {
    }

    protected virtual void OnAction7()
    {
    }

    protected virtual void OnAction8()
    {
    }

    protected virtual void OnAction9()
    {
    }

    protected virtual void OnWithPlayer()
    {
    }

    protected virtual void OnWithAgent()
    {
    }

    protected virtual void OnWithMerchant()
    {
    }

    protected virtual void ExitStart()
    {
    }

    protected virtual void ExitRepop()
    {
    }

    protected virtual void ExitDepop()
    {
    }

    protected virtual void ExitIdle()
    {
    }

    protected virtual void ExitWait()
    {
    }

    protected virtual void ExitSitWait()
    {
    }

    protected virtual void ExitLocomotion()
    {
    }

    protected virtual void ExitLovelyIdle()
    {
    }

    protected virtual void ExitLovelyFollow()
    {
    }

    protected virtual void ExitEscape()
    {
    }

    protected virtual void ExitSwim()
    {
    }

    protected virtual void ExitSleep()
    {
    }

    protected virtual void ExitToilet()
    {
    }

    protected virtual void ExitRest()
    {
    }

    protected virtual void ExitEat()
    {
    }

    protected virtual void ExitDrink()
    {
    }

    protected virtual void ExitActinidia()
    {
    }

    protected virtual void ExitGrooming()
    {
    }

    protected virtual void ExitMoveEars()
    {
    }

    protected virtual void ExitRoar()
    {
    }

    protected virtual void ExitPeck()
    {
    }

    protected virtual void ExitToDepop()
    {
    }

    protected virtual void ExitToIndoor()
    {
    }

    protected virtual void ExitAction0()
    {
    }

    protected virtual void ExitAction1()
    {
    }

    protected virtual void ExitAction2()
    {
    }

    protected virtual void ExitAction3()
    {
    }

    protected virtual void ExitAction4()
    {
    }

    protected virtual void ExitAction5()
    {
    }

    protected virtual void ExitAction6()
    {
    }

    protected virtual void ExitAction7()
    {
    }

    protected virtual void ExitAction8()
    {
    }

    protected virtual void ExitAction9()
    {
    }

    protected virtual void ExitWithPlayer()
    {
      this.AutoChangeAnimation = true;
      this.AnimationEndUpdate = true;
    }

    protected virtual void ExitWithAgent()
    {
      this.AutoChangeAnimation = true;
      this.AnimationEndUpdate = true;
    }

    protected virtual void ExitWithMerchant()
    {
      this.AutoChangeAnimation = true;
      this.AnimationEndUpdate = true;
    }

    protected virtual void AnimationStart()
    {
    }

    protected virtual void AnimationRepop()
    {
    }

    protected virtual void AnimationDepop()
    {
    }

    protected virtual void AnimationIdle()
    {
    }

    protected virtual void AnimationWait()
    {
    }

    protected virtual void AnimationSitWait()
    {
    }

    protected virtual void AnimationLocomotion()
    {
    }

    protected virtual void AnimationLovelyIdle()
    {
    }

    protected virtual void AnimationLovelyFollow()
    {
    }

    protected virtual void AnimationEscape()
    {
    }

    protected virtual void AnimationSwim()
    {
    }

    protected virtual void AnimationSleep()
    {
    }

    protected virtual void AnimationToilet()
    {
    }

    protected virtual void AnimationRest()
    {
    }

    protected virtual void AnimationEat()
    {
    }

    protected virtual void AnimationDrink()
    {
    }

    protected virtual void AnimationActinidia()
    {
    }

    protected virtual void AnimationGrooming()
    {
    }

    protected virtual void AnimationMoveEars()
    {
    }

    protected virtual void AnimationRoar()
    {
    }

    protected virtual void AnimationPeck()
    {
    }

    protected virtual void AnimationToDepop()
    {
    }

    protected virtual void AnimationToIndoor()
    {
    }

    protected virtual void AnimationAction0()
    {
    }

    protected virtual void AnimationAction1()
    {
    }

    protected virtual void AnimationAction2()
    {
    }

    protected virtual void AnimationAction3()
    {
    }

    protected virtual void AnimationAction4()
    {
    }

    protected virtual void AnimationAction5()
    {
    }

    protected virtual void AnimationAction6()
    {
    }

    protected virtual void AnimationAction7()
    {
    }

    protected virtual void AnimationAction8()
    {
    }

    protected virtual void AnimationAction9()
    {
    }

    protected virtual void AnimationWithPlayer()
    {
    }

    protected virtual void AnimationWithAgent()
    {
    }

    protected virtual void AnimationWithMerchant()
    {
    }

    public bool IsNight
    {
      get
      {
        return this.timeZone == AIProject.TimeZone.Night;
      }
    }

    public bool IsRain
    {
      get
      {
        return this.weather == Weather.Rain || this.weather == Weather.Storm;
      }
    }

    public bool CheckNight(AIProject.TimeZone _timeZone)
    {
      return _timeZone == AIProject.TimeZone.Night;
    }

    public bool CheckRain(Weather _weather)
    {
      return _weather == Weather.Rain || _weather == Weather.Storm;
    }

    public bool IndoorMode
    {
      get
      {
        return this.IsNight || this.IsRain;
      }
    }

    public virtual void OnSecondUpdate(TimeSpan _deltaTime)
    {
    }

    public virtual void OnMinuteUpdate(TimeSpan _deltaTime)
    {
    }

    public virtual void OnTimeZoneChanged(EnvironmentSimulator _simulator)
    {
      this.timeZone = _simulator.TimeZone;
    }

    public virtual void OnEnvironmentChanged(EnvironmentSimulator _simulator)
    {
    }

    public virtual void OnWeatherChanged(EnvironmentSimulator _simulator)
    {
      this.weather = _simulator.Weather;
    }

    public virtual void Clear()
    {
      this.Active = false;
      this.SetLookPattern(0);
      this.ClearTargetObject();
      bool flag1 = false;
      this.BadMood = flag1;
      bool flag2 = flag1;
      this.WaitMode = flag2;
      this.LookWaitMode = flag2;
      this.destination = new Vector3?();
      this.CurrentState = AnimalState.None;
      AnimalState animalState1 = AnimalState.None;
      this.BackupState = animalState1;
      AnimalState animalState2 = animalState1;
      this.NextState = animalState2;
      AnimalState animalState3 = animalState2;
      this.PrevState = animalState3;
      this.currentState_ = animalState3;
      bool flag3 = true;
      this.AnimationEndUpdate = flag3;
      bool flag4 = flag3;
      this.AutoChangeAnimation = flag4;
      this.EnabledStateUpdate = flag4;
      float num = 0.0f;
      this.StateTimeLimit = num;
      this.StateCounter = num;
      this.ReleaseActionPoint();
      this.StopPlayAnimChange();
      this.animator = (Animator) null;
      this.AnimCommonTable = (Dictionary<int, Dictionary<int, AnimalPlayState>>) null;
      this.LookStateTable = (ReadOnlyDictionary<AnimalState, LookState>) null;
      this.ReleaseBody();
      this.InAnimStates.Clear();
      this.OutAnimStates.Clear();
      this.CommandPartner = (Actor) null;
      this.IsImpossible = false;
      this.LabelType = LabelTypes.None;
    }

    public virtual void Destroy()
    {
      if (this.currentState_ != AnimalState.Destroyed)
        this.currentState_ = AnimalState.Destroyed;
      if (!Object.op_Inequality((Object) ((Component) this).get_gameObject(), (Object) null))
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    protected virtual void OnDestroy()
    {
      this.Active = false;
      this.currentState_ = AnimalState.Destroyed;
      if (this.OnDestroyEvent != null)
        this.OnDestroyEvent();
      this.StopPlayAnimChange();
      if (this.everyUpdateDisposable != null)
        this.everyUpdateDisposable.Dispose();
      if (this.everyLateUpdateDisposable != null)
        this.everyLateUpdateDisposable.Dispose();
      if (this.everyFixedUpdateDisposable == null)
        return;
      this.everyFixedUpdateDisposable.Dispose();
    }

    public void SetID(int _animalID, int _chunkID)
    {
      this.AnimalID = _animalID;
      this.ChunkID = _chunkID;
    }

    public void SetAnimalName(string _name_)
    {
      this._name = _name_;
    }

    public AnimalInfo GetAnimalInfo()
    {
      return new AnimalInfo(this.AnimalType, this.BreedingType, this.Name, this.IdentifierName, this.AnimalID, this.ChunkID, this.ModelInfo);
    }

    public void SetModelInfo(AnimalModelInfo _modelInfo)
    {
      this.ModelInfo = _modelInfo;
    }

    protected void SetStateData()
    {
      this.SetAnimStateTable();
      this.SetLookStateTable();
      this.SetExpressionTable();
    }

    protected void SetAnimStateTable()
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, Dictionary<int, AnimalPlayState>>> commonAnimeTable = Singleton<Manager.Resources>.Instance.AnimalTable.CommonAnimeTable;
      this.AnimCommonTable = !commonAnimeTable.ContainsKey(this.AnimalTypeID) ? new Dictionary<int, Dictionary<int, AnimalPlayState>>() : new Dictionary<int, Dictionary<int, AnimalPlayState>>((IDictionary<int, Dictionary<int, AnimalPlayState>>) commonAnimeTable[this.AnimalTypeID]);
    }

    protected void SetLookStateTable()
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, LookState>>> lookStateTable = Singleton<Manager.Resources>.Instance.AnimalTable.LookStateTable;
      this.LookStateTable = !lookStateTable.ContainsKey(this.AnimalType) || !lookStateTable[this.AnimalType].ContainsKey(this.BreedingType) ? (ReadOnlyDictionary<AnimalState, LookState>) null : new ReadOnlyDictionary<AnimalState, LookState>((IDictionary<AnimalState, LookState>) lookStateTable[this.AnimalType][this.BreedingType]);
    }

    protected void SetExpressionTable()
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      this._skinnedMeshRendererTable.Clear();
      this.ExpressionTable.Clear();
      this.CurrentExpressionTable = (Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>) null;
      Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>> dictionary1;
      if (!Singleton<Manager.Resources>.Instance.AnimalTable.ExpressionTable.TryGetValue(this.AnimalTypeID, out dictionary1) || ((IReadOnlyDictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>) dictionary1).IsNullOrEmpty<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>())
        return;
      using (Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>.Enumerator enumerator1 = dictionary1.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>> current1 = enumerator1.Current;
          if (!((IReadOnlyDictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>) current1.Value).IsNullOrEmpty<int, Dictionary<string, List<ValueTuple<string, int, int>>>>())
          {
            int key1 = current1.Key;
            using (Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, Dictionary<string, List<ValueTuple<string, int, int>>>> current2 = enumerator2.Current;
                if (!((IReadOnlyDictionary<string, List<ValueTuple<string, int, int>>>) current2.Value).IsNullOrEmpty<string, List<ValueTuple<string, int, int>>>())
                {
                  int key2 = current2.Key;
                  using (Dictionary<string, List<ValueTuple<string, int, int>>>.Enumerator enumerator3 = current2.Value.GetEnumerator())
                  {
                    while (enumerator3.MoveNext())
                    {
                      KeyValuePair<string, List<ValueTuple<string, int, int>>> current3 = enumerator3.Current;
                      if (!((IReadOnlyList<ValueTuple<string, int, int>>) current3.Value).IsNullOrEmpty<ValueTuple<string, int, int>>())
                      {
                        string key3 = current3.Key;
                        List<ValueTuple<SkinnedMeshRenderer, int, int>> toRelease1 = ListPool<ValueTuple<SkinnedMeshRenderer, int, int>>.Get();
                        using (List<ValueTuple<string, int, int>>.Enumerator enumerator4 = current3.Value.GetEnumerator())
                        {
                          while (enumerator4.MoveNext())
                          {
                            ValueTuple<string, int, int> current4 = enumerator4.Current;
                            SkinnedMeshRenderer skinnedMeshRenderer;
                            if (!this._skinnedMeshRendererTable.TryGetValue((string) current4.Item1, out skinnedMeshRenderer))
                            {
                              GameObject loop = (!Object.op_Inequality((Object) this.bodyObject, (Object) null) ? ((Component) this).get_transform() : this.bodyObject.get_transform()).FindLoop((string) current4.Item1);
                              Transform transform = !Object.op_Inequality((Object) loop, (Object) null) ? (Transform) null : loop.get_transform();
                              skinnedMeshRenderer = !Object.op_Inequality((Object) transform, (Object) null) ? (SkinnedMeshRenderer) null : (SkinnedMeshRenderer) ((Component) transform).GetComponent<SkinnedMeshRenderer>();
                              this._skinnedMeshRendererTable[(string) current4.Item1] = skinnedMeshRenderer;
                            }
                            if (!Object.op_Equality((Object) skinnedMeshRenderer, (Object) null))
                              toRelease1.Add(new ValueTuple<SkinnedMeshRenderer, int, int>(skinnedMeshRenderer, (int) current4.Item2, (int) current4.Item3));
                          }
                        }
                        if (((IReadOnlyList<ValueTuple<SkinnedMeshRenderer, int, int>>) toRelease1).IsNullOrEmpty<ValueTuple<SkinnedMeshRenderer, int, int>>())
                        {
                          ListPool<ValueTuple<SkinnedMeshRenderer, int, int>>.Release(toRelease1);
                        }
                        else
                        {
                          Dictionary<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>> dictionary2;
                          if (!this.ExpressionTable.TryGetValue(key1, out dictionary2) || dictionary2 == null)
                            this.ExpressionTable[key1] = dictionary2 = new Dictionary<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>>();
                          Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>> dictionary3;
                          if (!dictionary2.TryGetValue(key2, out dictionary3) || dictionary3 == null)
                            dictionary2[key2] = dictionary3 = new Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>();
                          List<ValueTuple<SkinnedMeshRenderer, int, int>> toRelease2;
                          if (dictionary3.TryGetValue(key3, out toRelease2) && !((IReadOnlyList<ValueTuple<SkinnedMeshRenderer, int, int>>) toRelease2).IsNullOrEmpty<ValueTuple<SkinnedMeshRenderer, int, int>>())
                            ListPool<ValueTuple<SkinnedMeshRenderer, int, int>>.Release(toRelease2);
                          dictionary3[key3] = toRelease1;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    public virtual void ChangeState(AnimalState _nextState, System.Action _changeEvent = null)
    {
      if (this.AutoChangeAnimation)
      {
        this.PlayOutAnim((System.Action) (() =>
        {
          this.CurrentState = _nextState;
          System.Action action = _changeEvent;
          if (action == null)
            return;
          action();
        }));
      }
      else
      {
        this.CurrentState = _nextState;
        System.Action action = _changeEvent;
        if (action == null)
          return;
        action();
      }
    }

    public virtual void SetState(AnimalState _nextState, System.Action _changeEvent = null)
    {
      if (this.AutoChangeAnimation)
      {
        this.PlayOutAnim((System.Action) (() =>
        {
          this.CurrentState = _nextState;
          System.Action action = _changeEvent;
          if (action == null)
            return;
          action();
        }));
      }
      else
      {
        this.CurrentState = _nextState;
        System.Action action = _changeEvent;
        if (action == null)
          return;
        action();
      }
    }

    public virtual void ResetState()
    {
      this.ChangedStateEvent();
      this.StateEnter();
    }

    public bool SetNextState()
    {
      if (this.NextState == AnimalState.None)
        return false;
      this.CurrentState = this.NextState;
      this.NextState = AnimalState.None;
      return true;
    }

    public virtual bool ActiveState
    {
      get
      {
        return this.CurrentState != AnimalState.None && this.CurrentState != AnimalState.Destroyed;
      }
    }

    public virtual bool ParamRisePossible
    {
      get
      {
        return true;
      }
    }

    public virtual bool WaitPossible
    {
      get
      {
        return true;
      }
    }

    public virtual bool DepopPossible
    {
      get
      {
        return true;
      }
    }

    public virtual bool NormalState
    {
      get
      {
        return this.CurrentState == AnimalState.Idle || this.CurrentState == AnimalState.Locomotion;
      }
    }

    public bool IsWithActor
    {
      get
      {
        return this.CurrentState == AnimalState.WithPlayer || this.CurrentState == AnimalState.WithAgent || this.CurrentState == AnimalState.WithMerchant;
      }
    }

    public virtual bool IsWithAgentFree(AgentActor _actor)
    {
      return false;
    }

    public virtual bool PrioritizeState()
    {
      return this.PrioritizeState(this.CurrentState);
    }

    public virtual bool PrioritizeState(AnimalState _state)
    {
      return false;
    }

    public void SetFloat(int _id, float _value)
    {
      if (!this.AnimatorControllerEnabled)
        return;
      this.animator.SetFloat(_id, _value);
    }

    public void SetFloat(string _paramName, float _value)
    {
      if (!this.AnimatorControllerEnabled)
        return;
      foreach (AnimatorControllerParameter parameter in this.animator.get_parameters())
      {
        if (parameter.get_name() == _paramName)
        {
          this.animator.SetFloat(_paramName, _value);
          break;
        }
      }
    }

    public Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>> GetExpressionTable(
      AnimationCategoryID _category,
      int _poseID)
    {
      Dictionary<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>> dictionary1;
      Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>> dictionary2;
      return this.ExpressionTable.TryGetValue((int) _category, out dictionary1) && !((IReadOnlyDictionary<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>>) dictionary1).IsNullOrEmpty<int, Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>>() && dictionary1.TryGetValue(_poseID, out dictionary2) ? dictionary2 : (Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>) null;
    }

    private void ChangeExpression(string stateName)
    {
      List<ValueTuple<SkinnedMeshRenderer, int, int>> valueTupleList;
      if (((IReadOnlyDictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>) this.CurrentExpressionTable).IsNullOrEmpty<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>() || stateName.IsNullOrEmpty() || (!this.CurrentExpressionTable.TryGetValue(stateName, out valueTupleList) || ((IReadOnlyList<ValueTuple<SkinnedMeshRenderer, int, int>>) valueTupleList).IsNullOrEmpty<ValueTuple<SkinnedMeshRenderer, int, int>>()))
        return;
      using (List<ValueTuple<SkinnedMeshRenderer, int, int>>.Enumerator enumerator = valueTupleList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<SkinnedMeshRenderer, int, int> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Item1, (Object) null))
            ((SkinnedMeshRenderer) current.Item1).SetBlendShapeWeight((int) current.Item2, (float) current.Item3);
        }
      }
    }

    public bool SetPlayAnimState(AnimationCategoryID _category)
    {
      this.InAnimStates.Clear();
      this.OutAnimStates.Clear();
      this.CurrentExpressionTable = (Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>) null;
      Dictionary<int, AnimalPlayState> source;
      if (this.AnimatorEmtpy || !this.AnimCommonTable.TryGetValue((int) _category, out source))
        return false;
      KeyValuePair<int, AnimalPlayState> keyValuePair = source.Rand<int, AnimalPlayState>();
      if (keyValuePair.Value == null)
        return false;
      this.CurrentExpressionTable = this.GetExpressionTable(_category, keyValuePair.Key);
      if (this.CurrentAnimState == null || Object.op_Inequality((Object) this.CurrentAnimState.MainStateInfo.Controller, (Object) keyValuePair.Value.MainStateInfo.Controller))
        this.animator.set_runtimeAnimatorController(keyValuePair.Value.MainStateInfo.Controller);
      this.CurrentAnimState = keyValuePair.Value;
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) keyValuePair.Value.MainStateInfo.InStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo inStateInfo in keyValuePair.Value.MainStateInfo.InStateInfos)
          this.InAnimStates.Enqueue(inStateInfo);
      }
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) keyValuePair.Value.MainStateInfo.OutStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo outStateInfo in keyValuePair.Value.MainStateInfo.OutStateInfos)
          this.OutAnimStates.Enqueue(outStateInfo);
      }
      return true;
    }

    public bool SetPlayAnimState(AnimationCategoryID _category, int _poseID)
    {
      this.InAnimStates.Clear();
      this.OutAnimStates.Clear();
      this.CurrentExpressionTable = (Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int, int>>>) null;
      Dictionary<int, AnimalPlayState> dictionary;
      AnimalPlayState animalPlayState;
      if (this.AnimatorEmtpy || !this.AnimCommonTable.TryGetValue((int) _category, out dictionary) || !dictionary.TryGetValue(_poseID, out animalPlayState))
        return false;
      this.CurrentExpressionTable = this.GetExpressionTable(_category, _poseID);
      if (this.CurrentAnimState == null || Object.op_Inequality((Object) this.CurrentAnimState.MainStateInfo.Controller, (Object) animalPlayState.MainStateInfo.Controller))
        this.animator.set_runtimeAnimatorController(animalPlayState.MainStateInfo.Controller);
      this.CurrentAnimState = animalPlayState;
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) animalPlayState.MainStateInfo.InStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo inStateInfo in animalPlayState.MainStateInfo.InStateInfos)
          this.InAnimStates.Enqueue(inStateInfo);
      }
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) animalPlayState.MainStateInfo.OutStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo outStateInfo in animalPlayState.MainStateInfo.OutStateInfos)
          this.OutAnimStates.Enqueue(outStateInfo);
      }
      return true;
    }

    public void SetPlayAnimState(AnimalPlayState _playState)
    {
      this.InAnimStates.Clear();
      this.OutAnimStates.Clear();
      if (_playState == null)
        return;
      AnimalPlayState.PlayStateInfo mainStateInfo = _playState.MainStateInfo;
      if (!this.AnimatorEmtpy && (this.CurrentAnimState == null || Object.op_Inequality((Object) this.CurrentAnimState.MainStateInfo.Controller, (Object) mainStateInfo.Controller)))
        this.animator.set_runtimeAnimatorController(mainStateInfo.Controller);
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) mainStateInfo.InStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo inStateInfo in mainStateInfo.InStateInfos)
          this.InAnimStates.Enqueue(inStateInfo);
      }
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) mainStateInfo.OutStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo outStateInfo in mainStateInfo.OutStateInfos)
          this.OutAnimStates.Enqueue(outStateInfo);
      }
      this.CurrentAnimState = _playState;
    }

    public void StopPlayAnimChange()
    {
      if (this.inAnimDisposable != null)
        this.inAnimDisposable.Dispose();
      if (this.outAnimDisposable != null)
        this.outAnimDisposable.Dispose();
      this.inAnimDisposable = (IDisposable) null;
      this.outAnimDisposable = (IDisposable) null;
    }

    public void PlayInAnim(AnimationCategoryID _category, System.Action _endEvent = null)
    {
      this.StopPlayAnimChange();
      if (!this.SetPlayAnimState(_category))
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartInAnimation(_endEvent);
        this.inAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    public void PlayInAnim(AnimationCategoryID _category, int _poseID, System.Action _endEvent = null)
    {
      this.StopPlayAnimChange();
      if (!this.SetPlayAnimState(_category, _poseID))
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartInAnimation(_endEvent);
        this.inAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    [DebuggerHidden]
    private IEnumerator StartInAnimation(System.Action _endEvent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalBase.\u003CStartInAnimation\u003Ec__Iterator1()
      {
        _endEvent = _endEvent,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator StartInAnimation(
      bool _fadeEnable,
      float _fadeTime,
      System.Action _endEvent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalBase.\u003CStartInAnimation\u003Ec__Iterator2()
      {
        _fadeEnable = _fadeEnable,
        _fadeTime = _fadeTime,
        _endEvent = _endEvent,
        \u0024this = this
      };
    }

    public void PlayOutAnim()
    {
      this.StopPlayAnimChange();
      if (!this.AnimatorControllerEnabled)
        return;
      IEnumerator _coroutine = this.StartOutAnimation((System.Action) null);
      this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
    }

    public void PlayOutAnim(AnimalPlayState.StateInfo[] _stateInfos, System.Action _endEvent = null)
    {
      this.OutAnimStates.Clear();
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) _stateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo stateInfo in _stateInfos)
          this.OutAnimStates.Enqueue(stateInfo);
      }
      this.StopPlayAnimChange();
      if (!this.AnimatorControllerEnabled)
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartInAnimation(_endEvent);
        this.inAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    public void PlayOutAnim(
      AnimalPlayState.StateInfo[] _stateInfos,
      bool _fadeEnable,
      float _fadeSecond,
      System.Action _endEvent = null)
    {
      this.OutAnimStates.Clear();
      if (!((IReadOnlyList<AnimalPlayState.StateInfo>) _stateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        foreach (AnimalPlayState.StateInfo stateInfo in _stateInfos)
          this.OutAnimStates.Enqueue(stateInfo);
      }
      this.StopPlayAnimChange();
      if (!this.AnimatorControllerEnabled)
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartOutAnimation(_fadeEnable, _fadeSecond, _endEvent);
        this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    public void PlayOutAnim(System.Action _endEvent)
    {
      this.StopPlayAnimChange();
      if (!this.AnimatorControllerEnabled || this.OutAnimStates.IsNullOrEmpty<AnimalPlayState.StateInfo>())
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartOutAnimation(_endEvent);
        this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    public void PlayOutAnim(AnimationCategoryID _category, System.Action _endEvent = null)
    {
      this.StopPlayAnimChange();
      if (!this.SetPlayAnimState(_category))
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartOutAnimation(_endEvent);
        this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    public void PlayOutAnim(AnimationCategoryID _category, int _poseID, System.Action _endEvent = null)
    {
      this.StopPlayAnimChange();
      if (!this.SetPlayAnimState(_category, _poseID))
      {
        if (_endEvent == null)
          return;
        _endEvent();
      }
      else
      {
        IEnumerator _coroutine = this.StartOutAnimation(_endEvent);
        this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
      }
    }

    [DebuggerHidden]
    private IEnumerator StartOutAnimation(System.Action _endEvent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalBase.\u003CStartOutAnimation\u003Ec__Iterator3()
      {
        _endEvent = _endEvent,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator StartOutAnimation(
      bool _fadeEnable,
      float _fadeTime,
      System.Action _endEvent = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalBase.\u003CStartOutAnimation\u003Ec__Iterator4()
      {
        _fadeEnable = _fadeEnable,
        _fadeTime = _fadeTime,
        _endEvent = _endEvent,
        \u0024this = this
      };
    }

    public void StartInAnimationWithActor(int _poseID)
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, AnimalPlayState>> withAgentAnimeTable = Singleton<Manager.Resources>.Instance.AnimalTable.WithAgentAnimeTable;
      AnimalPlayState _playState;
      if (!withAgentAnimeTable.ContainsKey(this.AnimalTypeID) || !withAgentAnimeTable[this.AnimalTypeID].TryGetValue(_poseID, out _playState))
        return;
      this.StopPlayAnimChange();
      this.SetPlayAnimState(_playState);
      this.CurrentAnimState = _playState;
      IEnumerator _coroutine = this.StartInAnimation(_playState.MainStateInfo.InFadeEnable, _playState.MainStateInfo.InFadeSecond, (System.Action) null);
      this.inAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
    }

    public void StartOutAnimationWithActor()
    {
      if (this.CurrentAnimState == null)
        return;
      this.StopPlayAnimChange();
      AnimalPlayState currentAnimState = this.CurrentAnimState;
      IEnumerator _coroutine = this.StartOutAnimation(currentAnimState.MainStateInfo.OutFadeEnable, currentAnimState.MainStateInfo.OutFadeSecond, (System.Action) null);
      this.outAnimDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
    }

    public bool IsCurrentAnimationCheck()
    {
      if (Object.op_Equality((Object) this.LastAnimState.Item1, (Object) null) || !this.AnimatorControllerEnabled)
        return false;
      int num = (int) this.LastAnimState.Item3;
      if (this.animator.IsInTransition(num))
        return true;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(num);
      return ((AnimatorStateInfo) ref animatorStateInfo).IsName((string) this.LastAnimState.Item4);
    }

    public bool IsCurrentAnimationCheck(int _animHash, int _layer = 0)
    {
      if (!this.AnimatorControllerEnabled)
        return false;
      int num = _animHash;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(_layer);
      int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
      return num == shortNameHash;
    }

    public bool AnimationKeepWaiting()
    {
      if (Object.op_Equality((Object) this.LastAnimState.Item1, (Object) null) || !this.AnimatorControllerEnabled)
        return false;
      int num = (int) this.LastAnimState.Item3;
      if (this.animator.IsInTransition(num))
        return true;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(num);
      return ((AnimatorStateInfo) ref animatorStateInfo).IsName((string) this.LastAnimState.Item4) && (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() < 1.0;
    }

    public bool IsCurrentAnimationEnd(int _layer = 0)
    {
      if (!this.AnimatorControllerEnabled)
        return false;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(_layer);
      return (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() >= 1.0;
    }

    public bool UseActionPoint
    {
      get
      {
        return !Object.op_Equality((Object) this.actionPoint, (Object) null) && this.actionPoint.MyUse((IAnimalActionPointUser) this);
      }
    }

    public virtual void CancelActionPoint()
    {
      if (!this.HasActionPoint)
        return;
      if (!this.actionPoint.MyUse((IAnimalActionPointUser) this))
        this.actionPoint.RemoveBooking((IAnimalActionPointUser) this);
      this.NextState = AnimalState.None;
      this.ActionType = ActionTypes.None;
      this.actionPoint = (AnimalActionPoint) null;
    }

    public virtual void RemoveActionPoint()
    {
      if (!this.HasActionPoint)
        return;
      if (this.actionPoint.MyUse((IAnimalActionPointUser) this))
      {
        this.actionPoint.StopUsing();
        this.NextState = AnimalState.None;
        this.ActionType = ActionTypes.None;
        this.actionPoint = (AnimalActionPoint) null;
      }
      else
        this.CancelActionPoint();
    }

    public virtual void MissingActionPoint()
    {
      if (this.HasActionPoint && this.actionPoint.MyUse((IAnimalActionPointUser) this))
        return;
      this.NextState = AnimalState.None;
      this.ActionType = ActionTypes.None;
      this.actionPoint = (AnimalActionPoint) null;
    }

    public virtual void ReleaseActionPoint()
    {
      if (!this.HasActionPoint)
        return;
      this.actionPoint.StopUsing((IAnimalActionPointUser) this);
      this.actionPoint.RemoveBooking((IAnimalActionPointUser) this);
      this.actionPoint = (AnimalActionPoint) null;
    }

    public virtual bool SetActionPoint(
      AnimalActionPoint _actionPoint,
      AnimalState _nextState,
      ActionTypes _actionType)
    {
      if (Object.op_Equality((Object) _actionPoint, (Object) null) || Object.op_Equality((Object) this.actionPoint, (Object) _actionPoint))
        return false;
      this.NextState = _nextState;
      if (Object.op_Inequality((Object) this.actionPoint, (Object) null) && this.actionPoint != null)
        this.actionPoint.RemoveBooking((IAnimalActionPointUser) this);
      this.actionPoint = _actionPoint;
      this.actionPoint.AddBooking((IAnimalActionPointUser) this);
      this.ActionType = _actionType;
      return true;
    }

    protected AudioSource Play3DSound(int id)
    {
      SoundPack soundPack = !Singleton<Manager.Resources>.IsInstance() ? (SoundPack) null : Singleton<Manager.Resources>.Instance.SoundPack;
      return Object.op_Equality((Object) soundPack, (Object) null) ? (AudioSource) null : soundPack.Play(id, Sound.Type.GameSE3D, 0.0f);
    }

    public virtual void ReleaseBody()
    {
      if (Object.op_Implicit((Object) this.bodyObject))
        Object.Destroy((Object) this.bodyObject);
      this.bodyRenderers = (Renderer[]) null;
      this.bodyRendererObjects = (GameObject[]) null;
      this.bodyParticleObjects = (GameObject[]) null;
      this.ModelInfo.ClearShapeState();
    }

    public virtual void CreateBody()
    {
      AssetBundleInfo assetInfo = this.ModelInfo.AssetInfo;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>((string) assetInfo.assetbundle, (string) assetInfo.asset, false, (string) assetInfo.manifest);
      if (Object.op_Equality((Object) gameObject1, (Object) null))
        return;
      MapScene.AddAssetBundlePath((string) assetInfo.assetbundle, (string) assetInfo.manifest);
      this.bodyObject = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
      if (Object.op_Inequality((Object) this.bodyObject, (Object) null))
      {
        GameObject gameObject2 = new GameObject("BodyRoot");
        gameObject2.get_transform().SetParent(((Component) this).get_transform(), false);
        this.bodyObject.get_transform().SetParent(gameObject2.get_transform(), false);
        this.bodyObject = gameObject2;
      }
      this.ModelInfo.SetShapeState(!Object.op_Inequality((Object) this.bodyObject, (Object) null) ? (Transform) null : this.bodyObject.get_transform());
      this.bodyRenderers = (Renderer[]) this.bodyObject.GetComponentsInChildren<Renderer>(true);
      if (this.bodyRenderers != null)
      {
        this.bodyRendererObjects = new GameObject[this.bodyRenderers.Length];
        for (int index = 0; index < this.bodyRenderers.Length; ++index)
          this.bodyRendererObjects[index] = ((Component) this.bodyRenderers[index]).get_gameObject();
      }
      this.bodyParticleRenderers = (ParticleSystemRenderer[]) this.bodyObject.GetComponentsInChildren<ParticleSystemRenderer>(true);
      if (this.bodyParticleRenderers != null)
      {
        this.bodyParticleObjects = new GameObject[this.bodyParticleRenderers.Length];
        for (int index = 0; index < this.bodyParticleRenderers.Length; ++index)
          this.bodyParticleObjects[index] = ((Component) this.bodyParticleRenderers[index]).get_gameObject();
      }
      this.animator = (Animator) ((Component) this).GetComponentInChildren<Animator>(true);
    }

    public virtual void CreateBody(GameObject prefab)
    {
      if (Object.op_Equality((Object) prefab, (Object) null))
        return;
      this.bodyObject = (GameObject) Object.Instantiate<GameObject>((M0) prefab, ((Component) this).get_transform(), false);
      if (Object.op_Inequality((Object) this.bodyObject, (Object) null))
      {
        GameObject gameObject = new GameObject("BodyRoot");
        gameObject.get_transform().SetParent(((Component) this).get_transform(), false);
        this.bodyObject.get_transform().SetParent(gameObject.get_transform(), false);
        this.bodyObject = gameObject;
      }
      this.ModelInfo.SetShapeState(!Object.op_Inequality((Object) this.bodyObject, (Object) null) ? (Transform) null : this.bodyObject.get_transform());
      this.bodyRenderers = (Renderer[]) this.bodyObject.GetComponentsInChildren<Renderer>(true);
      if (this.bodyRenderers != null)
      {
        this.bodyRendererObjects = new GameObject[this.bodyRenderers.Length];
        for (int index = 0; index < this.bodyRenderers.Length; ++index)
          this.bodyRendererObjects[index] = ((Component) this.bodyRenderers[index]).get_gameObject();
      }
      this.bodyParticleRenderers = (ParticleSystemRenderer[]) this.bodyObject.GetComponentsInChildren<ParticleSystemRenderer>(true);
      if (this.bodyParticleRenderers != null)
      {
        this.bodyParticleObjects = new GameObject[this.bodyParticleRenderers.Length];
        for (int index = 0; index < this.bodyParticleRenderers.Length; ++index)
          this.bodyParticleObjects[index] = ((Component) this.bodyParticleRenderers[index]).get_gameObject();
      }
      this.animator = (Animator) ((Component) this).GetComponentInChildren<Animator>(true);
    }

    public void LoadBody()
    {
      this.ReleaseBody();
      this.CreateBody();
    }

    public void LoadBody(GameObject prefab)
    {
      this.ReleaseBody();
      this.CreateBody(prefab);
    }

    public void SetLookPattern(int _ptnNo)
    {
      if (Object.op_Implicit((Object) this.neckController))
        this.neckController.ptnNo = _ptnNo;
      if (!Object.op_Implicit((Object) this.eyeController))
        return;
      this.eyeController.ptnNo = _ptnNo;
    }

    protected virtual void LookTargetUpdate()
    {
      this.LookWaitMode = false;
      if (Object.op_Equality((Object) this.Target, (Object) null) || this.LookStateTable == null || !this.LookStateTable.ContainsKey(this.CurrentState))
      {
        this.SetLookPattern(0);
      }
      else
      {
        LookState lookState = this.LookStateTable.get_Item(this.CurrentState);
        int ptnNo = lookState.ptnNo;
        bool flag = false;
        this.LookWaitMode = lookState.waitFlag && flag;
        switch (ptnNo)
        {
          case 0:
            this.SetLookPattern(ptnNo);
            break;
          case 1:
          case 2:
            this.SetLookPattern(!flag ? 0 : ptnNo);
            break;
        }
      }
    }

    protected void RefreshCommands(bool _conditionCheck)
    {
      CommandArea commandArea = Manager.Map.GetCommandArea();
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      if (_conditionCheck)
      {
        if (!commandArea.ContainsConsiderationObject((ICommandable) this))
          return;
        commandArea.RefreshCommands();
      }
      else
        commandArea.RefreshCommands();
    }

    public virtual void SetSearchTargetEnabled(bool _enabled, bool _clearCollision)
    {
      if (!Object.op_Inequality((Object) this.SearchActor, (Object) null))
        return;
      this.SearchActor.SetSearchEnabled(_enabled, _clearCollision);
    }

    public virtual void RefreshSearchTarget()
    {
      if (!Object.op_Inequality((Object) this.SearchActor, (Object) null))
        return;
      this.SearchActor?.RefreshSearchActorTable();
    }

    public virtual void Refresh()
    {
      this.RefreshCommands(true);
      this.RefreshSearchTarget();
    }

    private static float RaycastDistance { get; } = 1000f;

    public static Vector3 RaycastStartPoint(Vector3 _position)
    {
      return Vector3.op_Addition(_position, Vector3.op_Multiply(Vector3.get_up(), 5f));
    }

    public void UpdateCurrentMapArea(LayerMask layer)
    {
      if (!Singleton<Manager.Map>.IsInstance())
      {
        this.CurrentMapArea = (MapArea) null;
      }
      else
      {
        Dictionary<int, Chunk> chunkTable = Singleton<Manager.Map>.Instance.ChunkTable;
        if (((IReadOnlyDictionary<int, Chunk>) chunkTable).IsNullOrEmpty<int, Chunk>())
        {
          this.CurrentMapArea = (MapArea) null;
        }
        else
        {
          RaycastHit raycastHit;
          if (Physics.Raycast(Vector3.op_Addition(this.Position, Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(layer)))
          {
            bool flag = false;
            foreach (KeyValuePair<int, Chunk> keyValuePair in chunkTable)
            {
              foreach (MapArea mapArea in keyValuePair.Value.MapAreas)
              {
                if (flag = mapArea.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
                {
                  this.CurrentMapArea = mapArea;
                  this.ChunkID = mapArea.ChunkID;
                  this.AreaID = mapArea.AreaID;
                  break;
                }
              }
              if (flag)
                break;
            }
            if (flag)
              return;
            this.CurrentMapArea = (MapArea) null;
          }
          else
            this.CurrentMapArea = (MapArea) null;
        }
      }
    }

    public bool GetCurrentMapAreaType(MapArea _mapArea, out MapArea.AreaType _type)
    {
      _type = MapArea.AreaType.Normal;
      if (!Singleton<Manager.Resources>.IsInstance() || Object.op_Equality((Object) _mapArea, (Object) null))
        return false;
      LayerMask areaDetectionLayer = Singleton<Manager.Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      int num = Physics.RaycastNonAlloc(AnimalBase.RaycastStartPoint(this.Position), Vector3.get_up(), AnimalBase.raycastHits, AnimalBase.RaycastDistance, LayerMask.op_Implicit(areaDetectionLayer));
      _type = 0 >= num ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
      return true;
    }

    public static bool GetMapAreaType(
      Vector3 _position,
      MapArea _mapArea,
      out MapArea.AreaType _type)
    {
      _type = MapArea.AreaType.Normal;
      if (Object.op_Equality((Object) _mapArea, (Object) null) || !Singleton<Manager.Resources>.IsInstance())
        return false;
      LayerMask areaDetectionLayer = Singleton<Manager.Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      int num = Physics.RaycastNonAlloc(AnimalBase.RaycastStartPoint(_position), Vector3.get_up(), AnimalBase.raycastHits, AnimalBase.RaycastDistance, LayerMask.op_Implicit(areaDetectionLayer));
      _type = 0 >= num ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
      return true;
    }

    public static bool GetMapArea(
      Vector3 _position,
      out MapArea _mapArea,
      out MapArea.AreaType _type)
    {
      _type = MapArea.AreaType.Normal;
      _mapArea = (MapArea) null;
      if (!Singleton<Manager.Resources>.IsInstance())
        return false;
      LayerMask areaDetectionLayer = Singleton<Manager.Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      int num1 = Physics.RaycastNonAlloc(AnimalBase.RaycastStartPoint(_position), Vector3.get_down(), AnimalBase.raycastHits, AnimalBase.RaycastDistance, LayerMask.op_Implicit(areaDetectionLayer));
      if (num1 <= 0)
        return false;
      List<RaycastHit> toRelease = ListPool<RaycastHit>.Get();
      for (int index = 0; index < num1; ++index)
      {
        RaycastHit raycastHit = AnimalBase.raycastHits[index];
        if (Object.op_Inequality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) null))
          toRelease.Add(raycastHit);
      }
      if (((IReadOnlyList<RaycastHit>) toRelease).IsNullOrEmpty<RaycastHit>())
      {
        ListPool<RaycastHit>.Release(toRelease);
        return false;
      }
      toRelease.Sort((Comparison<RaycastHit>) ((a, b) =>
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(((RaycastHit) ref a).get_point(), _position);
        int sqrMagnitude1 = (int) ((Vector3) ref vector3_1).get_sqrMagnitude();
        Vector3 vector3_2 = Vector3.op_Subtraction(((RaycastHit) ref b).get_point(), _position);
        int sqrMagnitude2 = (int) ((Vector3) ref vector3_2).get_sqrMagnitude();
        return sqrMagnitude1 - sqrMagnitude2;
      }));
      for (int index = 0; index < toRelease.Count && Object.op_Equality((Object) _mapArea, (Object) null); ++index)
      {
        RaycastHit raycastHit = toRelease[index];
        _mapArea = (MapArea) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().GetComponent<MapArea>();
      }
      for (int index = 0; index < toRelease.Count && Object.op_Equality((Object) _mapArea, (Object) null); ++index)
      {
        RaycastHit raycastHit = toRelease[index];
        Transform parent = ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_parent();
        if (!Object.op_Equality((Object) parent, (Object) null))
          _mapArea = (MapArea) ((Component) parent).GetComponent<MapArea>();
      }
      ListPool<RaycastHit>.Release(toRelease);
      if (Object.op_Equality((Object) _mapArea, (Object) null))
        return false;
      int num2 = Physics.RaycastNonAlloc(AnimalBase.RaycastStartPoint(_position), Vector3.get_up(), AnimalBase.raycastHits, AnimalBase.RaycastDistance, LayerMask.op_Implicit(areaDetectionLayer));
      _type = 0 >= num2 ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
      return true;
    }

    public bool CheckTargetOnDistance(Vector3 _targetPosition, float _distance)
    {
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector((float) (_targetPosition.x - this.Position.x), (float) (_targetPosition.z - this.Position.z));
      return (double) ((Vector2) ref vector2).get_sqrMagnitude() <= (double) Mathf.Pow(_distance, 2f);
    }

    public bool CheckTargetOnHeight(Vector3 _targetPosition, float _height)
    {
      return (double) Mathf.Abs((float) (_targetPosition.y - this.Position.y)) <= (double) _height;
    }

    public bool CheckTargetOnAngle(Vector3 _targetPosition, float _angle)
    {
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector((float) (_targetPosition.x - this.Position.x), (float) (_targetPosition.z - this.Position.z));
      Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector((float) this.Forward.x, (float) this.Forward.z);
      Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
      return (double) (Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f) * 2.0 <= (double) _angle;
    }

    public bool CheckTargetOnArea(
      Vector3 _targetPosition,
      float _distance,
      float _height,
      float _angle)
    {
      return this.CheckTargetOnDistance(_targetPosition, _distance) && this.CheckTargetOnHeight(_targetPosition, _height) && this.CheckTargetOnAngle(_targetPosition, _angle);
    }

    protected AnimalState RandState(List<AnimalState> _stateList, AnimalState _prevState)
    {
      if (((IReadOnlyList<AnimalState>) _stateList).IsNullOrEmpty<AnimalState>())
        return AnimalState.None;
      List<AnimalState> toRelease = ListPool<AnimalState>.Get();
      toRelease.AddRange((IEnumerable<AnimalState>) _stateList);
      toRelease.RemoveAll((Predicate<AnimalState>) (x => x == _prevState));
      if (((IReadOnlyList<AnimalState>) toRelease).IsNullOrEmpty<AnimalState>())
      {
        ListPool<AnimalState>.Release(toRelease);
        return AnimalState.None;
      }
      AnimalState _state = toRelease[Random.Range(0, toRelease.Count)];
      _stateList.RemoveAll((Predicate<AnimalState>) (x => x == _state));
      return _state;
    }

    protected float AngleAbs(float _angle)
    {
      if ((double) _angle < 0.0)
        _angle = (float) ((double) _angle % 360.0 + 360.0);
      else if (360.0 <= (double) _angle)
        _angle %= 360f;
      return _angle;
    }

    protected float Angle360To180(float _angle)
    {
      _angle %= 360f;
      if ((double) _angle <= -180.0)
        _angle += 360f;
      else if (360.0 < (double) _angle)
        _angle -= 360f;
      return _angle;
    }

    protected float Angle180To360(float _angle)
    {
      _angle %= 360f;
      if ((double) _angle < 0.0)
        _angle += 360f;
      else if (360.0 <= (double) _angle)
        _angle -= _angle;
      return _angle;
    }

    protected Vector3 GetRandomPosOnCircle(float _radius)
    {
      float num1 = (float) ((double) Random.get_value() * 3.14159274101257 * 2.0);
      float num2 = _radius * Mathf.Sqrt(Random.get_value());
      return new Vector3(num2 * Mathf.Cos(num1), 0.0f, num2 * Mathf.Sin(num1));
    }

    public virtual Vector3 GetWithActorPoint(Actor _targetActor, int _poseID)
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return this.Position;
      Manager.Resources.AnimalTables animalTable = Singleton<Manager.Resources>.Instance.AnimalTable;
      if (animalTable == null)
        return this.Position;
      Dictionary<int, AnimalPlayState> dictionary = (Dictionary<int, AnimalPlayState>) null;
      AnimalPlayState animalPlayState = (AnimalPlayState) null;
      if (!animalTable.WithAgentAnimeTable.TryGetValue(this.AnimalTypeID, out dictionary) || !dictionary.TryGetValue(_poseID, out animalPlayState))
        return this.Position;
      float shapeBodyValue = _targetActor.ChaControl.GetShapeBodyValue(0);
      float num1 = 0.0f;
      float num2 = 0.5f;
      if ((double) shapeBodyValue == (double) num2)
        num1 = animalPlayState.FloatList.GetElement<float>(1);
      else if ((double) shapeBodyValue < (double) num2)
      {
        float num3 = Mathf.InverseLerp(0.0f, 0.5f, shapeBodyValue);
        num1 = Mathf.Lerp(animalPlayState.FloatList.GetElement<float>(0), animalPlayState.FloatList.GetElement<float>(1), num3);
      }
      else if ((double) num2 < (double) shapeBodyValue)
      {
        float num3 = Mathf.InverseLerp(0.5f, 1f, shapeBodyValue);
        num1 = Mathf.Lerp(animalPlayState.FloatList.GetElement<float>(1), animalPlayState.FloatList.GetElement<float>(2), num3);
      }
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, num1);
      return Vector3.op_Addition(_targetActor.Position, Quaternion.op_Multiply(_targetActor.Rotation, vector3));
    }

    public void SetWithActorPoint(Actor _targetActor, int _poseID)
    {
      this.Position = this.GetWithActorPoint(_targetActor, _poseID);
      Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(_targetActor.Position, this.Position));
      Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
      eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
      this.EulerAngles = eulerAngles;
    }

    public virtual Vector3 GetWithActorGetPoint(Actor _targetActor)
    {
      if (Singleton<Manager.Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Resources>.Instance.AnimalDefinePack, (Object) null))
      {
        AnimalDefinePack.WithActorInfoGroup withActorInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.WithActorInfo;
        if (withActorInfo != null)
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, withActorInfo.GetPointDistance);
          return Vector3.op_Addition(_targetActor.Position, Quaternion.op_Multiply(_targetActor.Rotation, vector3));
        }
      }
      return Vector3.get_zero();
    }

    public void SetWithActorGetPoint(Actor _targetActor)
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      this.Position = this.GetWithActorGetPoint(_targetActor);
      Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(_targetActor.Position, this.Position));
      Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
      eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
      this.EulerAngles = eulerAngles;
    }

    public void Relocate(LocateTypes _locateType = LocateTypes.NavMesh)
    {
      switch (_locateType)
      {
        case LocateTypes.Collider:
          if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Manager.Resources>.IsInstance())
            break;
          Manager.Resources instance1 = Singleton<Manager.Resources>.Instance;
          Manager.Map instance2 = Singleton<Manager.Map>.Instance;
          RaycastHit raycastHit;
          if (!Physics.Raycast(Vector3.op_Addition(this.Position, Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit, 100f, LayerMask.op_Implicit(instance1.DefinePack.MapDefines.AreaDetectionLayer)))
            break;
          bool flag = false;
          using (Dictionary<int, Chunk>.Enumerator enumerator = instance2.ChunkTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              foreach (MapArea mapArea in enumerator.Current.Value.MapAreas)
              {
                if (flag = mapArea.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
                {
                  this.Position = ((RaycastHit) ref raycastHit).get_point();
                  break;
                }
              }
              if (flag)
                break;
            }
            break;
          }
        case LocateTypes.NavMesh:
          NavMeshHit navMeshHit;
          if (!NavMesh.SamplePosition(this.Position, ref navMeshHit, 15f, this.NavMeshAreaMask))
            break;
          this.Position = ((NavMeshHit) ref navMeshHit).get_position();
          break;
      }
    }

    public static bool RandomBool
    {
      get
      {
        return Random.Range(0, 100) < 50;
      }
    }

    public void CrossFade(float _fadeTime = -1f)
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      ActorCameraControl cameraControl = Manager.Map.GetCameraControl();
      if (Object.op_Equality((Object) cameraControl, (Object) null))
        return;
      Camera cameraComponent = cameraControl.CameraComponent;
      if (Object.op_Equality((Object) cameraComponent, (Object) null))
        return;
      AnimalDefinePack.GroundWildInfoGroup groundWildInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.GroundWildInfo;
      Vector3 vector3 = Vector3.op_Subtraction(this.Position, ((Component) cameraComponent).get_transform().get_position());
      float num1 = Vector3.SqrMagnitude(vector3);
      if ((double) groundWildInfo.DepopCrossFadeDistance * (double) groundWildInfo.DepopCrossFadeDistance < (double) num1)
        return;
      float num2 = Vector3.Angle(vector3, ((Component) cameraComponent).get_transform().get_forward());
      if ((double) groundWildInfo.DepopCrossFadeAngle < (double) num2)
        return;
      cameraControl.CrossFade.FadeStart(_fadeTime);
    }
  }
}
