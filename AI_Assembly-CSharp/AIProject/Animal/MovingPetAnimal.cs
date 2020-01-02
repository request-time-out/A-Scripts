// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.MovingPetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using AIProject.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  [RequireComponent(typeof (NavMeshAgent))]
  [RequireComponent(typeof (NavMeshObstacle))]
  public abstract class MovingPetAnimal : AnimalBase, IGroundPet, INicknameObject, INavMeshActor, IPetAnimal
  {
    [SerializeField]
    private float _walkSpeed = 5f;
    [SerializeField]
    private float _runSpeed = 20f;
    [SerializeField]
    private int _waypointRetentionNum = 5;
    [SerializeField]
    private float _normalStoppingDistance = 1f;
    [SerializeField]
    private float _followTargetStoppingDistance = 3f;
    [SerializeField]
    private float _reFollowTargetDistance = 10f;
    [SerializeField]
    private float _runningChangeDistance = 20f;
    [SerializeField]
    private Vector2 _narrowNextDistance = Vector2.get_zero();
    [SerializeField]
    private Vector2Int _narrowArrivalLimit = Vector2Int.get_zero();
    [SerializeField]
    private Vector2 _largeNextDistance = Vector2.get_zero();
    [SerializeField]
    private Vector2Int _largeArrivalLimit = Vector2Int.get_zero();
    [SerializeField]
    private float _nicknameHeightOffset = 1f;
    [SerializeField]
    private float _playerFollowWalkSpeed = 1f;
    [SerializeField]
    private float _otherFollowWalkSpeed = 1f;
    protected List<Waypoint> _movePointList = new List<Waypoint>();
    protected string _locomotionParamName = string.Empty;
    protected int _originPriority = 50;
    protected float _animLerpValue = 1f;
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private NavMeshObstacle _obstacle;
    [SerializeField]
    private float _playerFollowRunSpeed;
    [SerializeField]
    private float _otherFollowRunSpeed;
    private bool _chaseActor;
    protected IReadOnlyList<Waypoint> _restrictedPointList;
    protected IReadOnlyList<Waypoint> _largeAreaPointList;
    protected Waypoint _destination;
    protected int _arrivalCount;
    protected int _arrivalLimit;
    protected float _lovelyTimeCounter;
    protected Transform _nicknameRoot;
    private bool _nicknameEnabled;
    private bool _enableAgent;
    protected float _animParam;
    private IDisposable _waypointRetentionDisposable;
    private NavMeshPath _calcPath;

    public NavMeshAgent Agent
    {
      get
      {
        return this._agent;
      }
    }

    public NavMeshObstacle Obstacle
    {
      get
      {
        return this._obstacle;
      }
    }

    public float WalkSpeed
    {
      get
      {
        return this._walkSpeed;
      }
    }

    public float RunSpeed
    {
      get
      {
        return this._runSpeed;
      }
    }

    public int WaypointRetentionNum
    {
      get
      {
        return this._waypointRetentionNum;
      }
    }

    public float NormalStoppingDistance
    {
      get
      {
        return this._normalStoppingDistance;
      }
    }

    public float FollowTargetStoppingDistance
    {
      get
      {
        return this._followTargetStoppingDistance;
      }
    }

    public float ReFollowTargetDistance
    {
      get
      {
        return this._reFollowTargetDistance;
      }
    }

    public float RunningChangeDistance
    {
      get
      {
        return this._runningChangeDistance;
      }
    }

    public Vector2 NarrowNextDistance
    {
      get
      {
        return this._narrowNextDistance;
      }
    }

    public Vector2Int NarrowArrivalLimit
    {
      get
      {
        return this._narrowArrivalLimit;
      }
    }

    public Vector2 LargeNextDistance
    {
      get
      {
        return this._largeNextDistance;
      }
    }

    public Vector2Int LargeArrivalLimit
    {
      get
      {
        return this._largeArrivalLimit;
      }
    }

    public float NicknameHeightOffset
    {
      get
      {
        return this._nicknameHeightOffset;
      }
    }

    public AIProject.SaveData.AnimalData AnimalData { get; set; }

    public bool IsChasePlayer { get; set; } = true;

    public string Nickname
    {
      get
      {
        return this.AnimalData?.Nickname ?? this.Name;
      }
      set
      {
        if (this.AnimalData != null)
          this.AnimalData.Nickname = value;
        System.Action changeNickNameEvent = this.ChangeNickNameEvent;
        if (changeNickNameEvent == null)
          return;
        changeNickNameEvent();
      }
    }

    public bool ChaseActor
    {
      get
      {
        return this._chaseActor;
      }
      set
      {
        this.ChangeChaseActor(value);
      }
    }

    protected virtual void ChangeChaseActor(bool chaseFlag)
    {
      if (this._chaseActor == chaseFlag)
        return;
      this._chaseActor = chaseFlag;
      this.SetState(AnimalState.Start, (System.Action) null);
    }

    public PetHomePoint HomePoint { get; protected set; }

    public override bool IsWithAgentFree(AgentActor _actor)
    {
      if (!this.AgentInsight || this.IsWithActor || Object.op_Inequality((Object) this.CommandPartner, (Object) null) && this.CommandPartner is AgentActor && Object.op_Inequality((Object) this.CommandPartner, (Object) _actor))
        return false;
      AnimalState currentState = this.CurrentState;
      switch (currentState)
      {
        case AnimalState.Idle:
        case AnimalState.Locomotion:
        case AnimalState.LovelyIdle:
        case AnimalState.LovelyFollow:
        case AnimalState.Sleep:
        case AnimalState.Eat:
          return true;
        default:
          if (currentState != AnimalState.Action9)
            return false;
          goto case AnimalState.Idle;
      }
    }

    public bool AgentIsStopped
    {
      get
      {
        return this.Agent.get_isStopped();
      }
      set
      {
        if (!this.Agent.get_isOnNavMesh())
          return;
        this.Agent.set_isStopped(value);
      }
    }

    public override Vector3 Position
    {
      get
      {
        return base.Position;
      }
      set
      {
        if (((Behaviour) this.Agent).get_isActiveAndEnabled())
          this.Agent.Warp(value);
        else
          base.Position = value;
      }
    }

    protected static List<int> PriorityList { get; set; } = new List<int>();

    public bool RunMode { get; protected set; }

    public Transform NicknameRoot
    {
      get
      {
        return this._nicknameRoot;
      }
    }

    public bool NicknameEnabled
    {
      get
      {
        return this._nicknameEnabled && this.BodyEnabled;
      }
      set
      {
        this._nicknameEnabled = value;
      }
    }

    public System.Action ChangeNickNameEvent { get; set; }

    public bool IsActiveNavMeshAgent
    {
      get
      {
        if (!Object.op_Inequality((Object) this._agent, (Object) null) || !((Behaviour) this._agent).get_isActiveAndEnabled())
          return false;
        return this._agent.get_isOnNavMesh() || this._agent.get_isOnOffMeshLink();
      }
    }

    public virtual void Initialize(PetHomePoint homePoint)
    {
      this.Clear();
      PetHomePoint petHomePoint = homePoint;
      this.HomePoint = petHomePoint;
      if (Object.op_Equality((Object) petHomePoint, (Object) null) || this.AnimalData == null)
        return;
      if (0 <= this.AnimalData.ModelID && Singleton<Manager.Resources>.IsInstance() && Singleton<Manager.Resources>.Instance.AnimalTable != null)
      {
        Dictionary<int, Dictionary<int, AnimalModelInfo>> modelInfoTable = Singleton<Manager.Resources>.Instance.AnimalTable.ModelInfoTable;
        Dictionary<int, AnimalModelInfo> dictionary;
        if (!((IReadOnlyDictionary<int, Dictionary<int, AnimalModelInfo>>) modelInfoTable).IsNullOrEmpty<int, Dictionary<int, AnimalModelInfo>>() && modelInfoTable.TryGetValue(this.AnimalData.AnimalTypeID, out dictionary) && !((IReadOnlyDictionary<int, AnimalModelInfo>) dictionary).IsNullOrEmpty<int, AnimalModelInfo>())
        {
          AnimalModelInfo _modelInfo;
          if (!dictionary.TryGetValue(this.AnimalData.ModelID, out _modelInfo))
          {
            this.AnimalData.ModelID = dictionary.Keys.ToList<int>().Rand<int>();
            _modelInfo = dictionary[this.AnimalData.ModelID];
          }
          this.SetModelInfo(_modelInfo);
          this.LoadBody();
          if (Object.op_Inequality((Object) this.bodyObject, (Object) null) && Object.op_Equality((Object) this._nicknameRoot, (Object) null))
          {
            this._nicknameRoot = new GameObject("Nickname Root").get_transform();
            this._nicknameRoot.SetParent(this.bodyObject.get_transform(), false);
            this._nicknameRoot.set_localPosition(new Vector3(0.0f, this._nicknameHeightOffset, 0.0f));
          }
        }
      }
      if (Object.op_Equality((Object) this._nicknameRoot, (Object) null))
      {
        this._nicknameRoot = new GameObject("Nickname Root").get_transform();
        this._nicknameRoot.SetParent(((Component) this).get_transform(), false);
        this._nicknameRoot.set_localPosition(new Vector3(0.0f, this._nicknameHeightOffset, 0.0f));
      }
      this.SetStateData();
      this.InitializeCommandLabels();
      this._originPriority = Singleton<Manager.Resources>.Instance.AnimalDefinePack.AgentInfo.GroundAnimalStartPriority;
      MovingPetAnimal.PriorityList.Sort();
      while (MovingPetAnimal.PriorityList.Contains(this._originPriority))
      {
        ++this._originPriority;
        if (99 <= this._originPriority)
        {
          this._originPriority = 99;
          break;
        }
      }
      MovingPetAnimal.PriorityList.Add(this._originPriority);
      this.Agent.set_avoidancePriority(this._originPriority);
      this.Agent.set_stoppingDistance(0.5f);
      this.Initialize();
      this.BodyEnabled = false;
      this.AnimalData.First = false;
      this.SetState(AnimalState.Start, (System.Action) null);
    }

    public virtual void Initialize(AIProject.SaveData.AnimalData animalData)
    {
      if (animalData != null)
        return;
      this.SetState(AnimalState.Destroyed, (System.Action) null);
    }

    protected virtual void Initialize()
    {
    }

    public virtual void ReturnHome()
    {
      this.SetState(AnimalState.Idle, (System.Action) null);
      if (!Object.op_Inequality((Object) this.HomePoint, (Object) null))
        return;
      this.HomePoint.SetRootPoint(0, (IPetAnimal) this);
    }

    public virtual void Release()
    {
      this.SetState(AnimalState.Destroyed, (System.Action) null);
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._agent, (Object) null))
        this._agent = ((Component) ((Component) this).get_transform()).GetOrAddComponent<NavMeshAgent>();
      ((Behaviour) this._agent).set_enabled(false);
    }

    protected override void Start()
    {
      base.Start();
      this._locomotionParamName = !Singleton<Manager.Resources>.IsInstance() ? "Locomotion" : Singleton<Manager.Resources>.Instance.AnimalDefinePack.AnimatorInfo.LocomotionParamName;
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (!Object.op_Inequality((Object) this.Agent, (Object) null) || ((Behaviour) this.Agent).get_enabled() == this._enableAgent)
        return;
      ((Behaviour) this.Agent).set_enabled(this._enableAgent);
    }

    protected override void OnDisable()
    {
      if (Object.op_Inequality((Object) this.Agent, (Object) null))
      {
        this._enableAgent = ((Behaviour) this.Agent).get_enabled();
        if (this._enableAgent)
          ((Behaviour) this.Agent).set_enabled(false);
      }
      base.OnDisable();
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      this.StopWaypointRetention();
      base.OnDestroy();
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.IsLovely)
        this._lovelyTimeCounter += Time.get_deltaTime();
      else
        this._lovelyTimeCounter = 0.0f;
      if (this.AnimalData == null)
        return;
      this.AnimalData.Position = this.Position;
      this.AnimalData.Rotation = this.Rotation;
    }

    protected override void EnterStart()
    {
      AnimalState _nextState;
      if (this.ChaseActor)
      {
        _nextState = AnimalState.LovelyFollow;
        this.ActivateNavMeshAgent();
      }
      else
      {
        _nextState = AnimalState.Action9;
        this.ActivateNavMeshObstacle();
      }
      this.SetState(_nextState, (System.Action) null);
    }

    protected override void ExitStart()
    {
      this.BodyEnabled = true;
      this.Active = true;
    }

    protected override void EnterLovelyIdle()
    {
      this.ActivateNavMeshAgent();
      this.AnimationEndUpdate = false;
      this.PlayInAnim(AnimationCategoryID.Idle, 1, (System.Action) null);
      this.StateTimeLimit = 0.5f;
    }

    protected override void OnLovelyIdle()
    {
      this.StateCounter += Time.get_deltaTime();
      if ((double) this.StateCounter < (double) this.StateTimeLimit)
        return;
      this.StateCounter = 0.0f;
      if (Object.op_Inequality((Object) this.FollowActor, (Object) null))
      {
        if (this.CheckReachableActor(this.FollowActor))
        {
          if ((double) this._reFollowTargetDistance < (double) this.GetRemainingDistance(this.FollowActor, this.CalcPath))
          {
            this.SetState(AnimalState.LovelyFollow, (System.Action) null);
            return;
          }
        }
        else
          this.FollowActor = (Actor) null;
      }
      if (!Object.op_Equality((Object) this.FollowActor, (Object) null))
        return;
      this.FollowActor = this.GetChaseActor();
    }

    protected override void ExitLovelyIdle()
    {
      this.AnimationEndUpdate = true;
    }

    protected override void EnterLovelyFollow()
    {
      this.AnimationEndUpdate = false;
      if (Object.op_Equality((Object) this.FollowActor, (Object) null))
        this.FollowActor = this.GetChaseActor();
      this.ActivateNavMeshAgent();
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (System.Action) null);
      this.Agent.set_avoidancePriority(this._originPriority);
      this.RunMode = false;
      this.Agent.set_speed(this.WalkSpeed);
      this.AgentIsStopped = false;
      this.StateTimeLimit = 0.5f;
      this._animParam = 0.0f;
      this._animLerpValue = Singleton<Manager.Resources>.Instance.AnimalDefinePack.GroundAnimalInfo.ForwardAnimationLerpValue;
    }

    protected override void OnLovelyFollow()
    {
      if (Object.op_Equality((Object) this.FollowActor, (Object) null) || !this.IsActiveNavMeshAgent)
      {
        this.SetState(AnimalState.LovelyIdle, (System.Action) null);
      }
      else
      {
        if (!this.Agent.get_pathPending())
          this.Agent.SetDestination(this.FollowActor.Position);
        float distance;
        if (this.TryGetRemainingDistance(this.FollowActor, out distance))
        {
          if ((double) distance <= (double) this._followTargetStoppingDistance)
          {
            this.SetState(AnimalState.LovelyIdle, (System.Action) null);
          }
          else
          {
            this.RunMode = (double) this._runningChangeDistance < (double) distance;
            bool flag = this.FollowActor is PlayerActor;
            if (this.RunMode)
              this.Agent.set_speed(!flag ? this._otherFollowRunSpeed : this._playerFollowRunSpeed);
            else
              this.Agent.set_speed(!flag ? this._otherFollowWalkSpeed : this._playerFollowWalkSpeed);
          }
        }
        else
        {
          this.FollowActor = (Actor) null;
          this.SetState(AnimalState.LovelyIdle, (System.Action) null);
        }
      }
    }

    protected override void ExitLovelyFollow()
    {
      this.ReleaseAgentPath();
      this.SetFloat(this._locomotionParamName, 0.0f);
      this.AnimationEndUpdate = true;
    }

    protected override void AnimationLovelyFollow()
    {
      Vector3 velocity = this.Agent.get_velocity();
      float num1 = !Mathf.Approximately(((Vector3) ref velocity).get_magnitude(), 0.0f) ? (!this.RunMode ? 0.5f : 1f) : 0.0f;
      if (!Mathf.Approximately(num1, this._animParam))
      {
        float num2 = Mathf.Sign(num1 - this._animParam);
        float num3 = this._animParam + this._animLerpValue * Time.get_deltaTime() * num2;
        if (0.0 < (double) num2)
        {
          if ((double) num1 < (double) num3)
            num3 = num1;
        }
        else if ((double) num3 < (double) num1)
          num3 = num1;
        this._animParam = num3;
      }
      this.SetFloat(this._locomotionParamName, this._animParam);
    }

    protected override void EnterAction9()
    {
      this.AnimationEndUpdate = false;
      this.PlayInAnim(AnimationCategoryID.Etc, 999, (System.Action) null);
      this.ActivateNavMeshObstacle();
    }

    protected override void OnAction9()
    {
      Transform transform = !Object.op_Inequality((Object) this.HomePoint, (Object) null) ? (Transform) null : this.HomePoint.GetRootPoint(0);
      if (!Object.op_Inequality((Object) transform, (Object) null))
        return;
      this.Position = transform.get_position();
      this.Rotation = transform.get_rotation();
    }

    protected override void ExitAction9()
    {
      this.AnimationEndUpdate = true;
    }

    protected override void EnterWithAgent()
    {
      base.EnterWithAgent();
      this.ActivateNavMeshObstacle();
    }

    protected override void OnWithAgent()
    {
      AgentActor commandPartner = this.CommandPartner as AgentActor;
      bool flag = Object.op_Equality((Object) commandPartner, (Object) null);
      if (!flag)
        flag = commandPartner.Mode != Desire.ActionType.EndTaskPetAnimal;
      if (!flag)
        flag = Object.op_Inequality((Object) commandPartner.TargetInSightAnimal, (Object) this);
      if (!flag)
        flag = !commandPartner.LivesWithAnimalSequence;
      if (!flag)
        return;
      this.IsImpossible = false;
      this.CommandPartner = (Actor) null;
      this.SetState(this.BackupState, (System.Action) null);
    }

    protected override void ExitWithAgent()
    {
      base.ExitWithAgent();
    }

    public Actor GetMostFavorabilityActor()
    {
      ReadOnlyDictionary<int, Actor> readOnlyDictionary = !Singleton<Manager.Map>.IsInstance() ? (ReadOnlyDictionary<int, Actor>) null : Singleton<Manager.Map>.Instance.ActorTable;
      if (((IReadOnlyDictionary<int, Actor>) readOnlyDictionary).IsNullOrEmpty<int, Actor>())
        return (Actor) null;
      List<KeyValuePair<int, float>> list = ListPool<KeyValuePair<int, float>>.Get();
      this.AnimalData.GetFavorabilityKeyPairs(ref list);
      if (((IReadOnlyList<KeyValuePair<int, float>>) list).IsNullOrEmpty<KeyValuePair<int, float>>())
      {
        ListPool<KeyValuePair<int, float>>.Release(list);
        return (Actor) null;
      }
      Actor actor1 = (Actor) null;
      list.Sort((Comparison<KeyValuePair<int, float>>) ((x1, x2) => (int) ((double) x2.Value - (double) x1.Value)));
      foreach (KeyValuePair<int, float> keyValuePair in list)
      {
        Actor actor2;
        if (readOnlyDictionary.TryGetValue(keyValuePair.Key, ref actor2) && Object.op_Inequality((Object) actor2, (Object) null))
        {
          actor1 = actor2;
          break;
        }
      }
      ListPool<KeyValuePair<int, float>>.Release(list);
      return actor1;
    }

    public Actor GetChaseActor()
    {
      Actor actor1 = this.GetMostFavorabilityActor();
      if (Object.op_Equality((Object) actor1, (Object) null))
        actor1 = !Singleton<Manager.Map>.IsInstance() ? (Actor) null : (Actor) Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Inequality((Object) actor1, (Object) null))
      {
        bool flag = NavMesh.CalculatePath(this.Position, actor1.Position, this.Agent.get_areaMask(), this.CalcPath);
        if (flag)
          flag = this.CalcPath.get_status() == 0;
        if (flag)
          return actor1;
      }
      ReadOnlyDictionary<int, Actor> readOnlyDictionary = !Singleton<Manager.Map>.IsInstance() ? (ReadOnlyDictionary<int, Actor>) null : Singleton<Manager.Map>.Instance.ActorTable;
      Actor actor2 = (Actor) null;
      if (!((IReadOnlyDictionary<int, Actor>) readOnlyDictionary).IsNullOrEmpty<int, Actor>())
      {
        List<Actor> actorList = ListPool<Actor>.Get();
        actorList.AddRange((IEnumerable<Actor>) readOnlyDictionary.get_Values());
        while (!((IReadOnlyList<Actor>) actorList).IsNullOrEmpty<Actor>())
        {
          Actor rand = actorList.GetRand<Actor>();
          if (!Object.op_Equality((Object) rand, (Object) null) && !(rand is MerchantActor))
          {
            bool flag = NavMesh.CalculatePath(this.Position, rand.Position, this.Agent.get_areaMask(), this.CalcPath);
            if (flag)
              flag = this.CalcPath.get_status() == 0;
            if (flag)
            {
              actor2 = rand;
              break;
            }
          }
        }
        ListPool<Actor>.Release(actorList);
      }
      return actor2;
    }

    public bool CheckReachableActor(Actor actor)
    {
      if (Object.op_Equality((Object) actor, (Object) null))
        return false;
      bool flag = NavMesh.CalculatePath(this.Position, actor.Position, this.Agent.get_areaMask(), this.CalcPath);
      if (flag)
        flag = this.CalcPath.get_status() == 0;
      return flag;
    }

    public float GetRemainingDistance(Actor actor)
    {
      float num = float.MaxValue;
      if (Object.op_Equality((Object) actor, (Object) null))
        return num;
      bool flag = NavMesh.CalculatePath(this.Position, actor.Position, this.Agent.get_areaMask(), this.CalcPath);
      if (flag)
        flag = this.CalcPath.get_status() == 0;
      if (flag)
      {
        num = 0.0f;
        for (int index = 0; index < this.CalcPath.get_corners().Length - 1; ++index)
          num += Vector3.Distance(this.CalcPath.get_corners()[index], this.CalcPath.get_corners()[index + 1]);
      }
      return num;
    }

    public bool TryGetRemainingDistance(
      NavMeshAgent agent,
      Actor actor,
      out float remainingDistance)
    {
      remainingDistance = float.MaxValue;
      if (Object.op_Equality((Object) agent, (Object) null) || !((Behaviour) agent).get_isActiveAndEnabled() || !agent.get_isOnNavMesh())
        return false;
      if (!agent.get_hasPath())
        return agent.get_pathPending();
      Vector3[] corners = agent.get_path().get_corners();
      remainingDistance = 0.0f;
      for (int index = 0; index < corners.Length - 1; ++index)
        remainingDistance += Vector3.Distance(corners[index], corners[index + 1]);
      return true;
    }

    public bool TryGetRemainingDistance(Actor actor, out float distance)
    {
      distance = float.MaxValue;
      if (Object.op_Equality((Object) actor, (Object) null))
        return false;
      bool flag = NavMesh.CalculatePath(this.Position, actor.Position, this.Agent.get_areaMask(), this.CalcPath);
      if (flag)
        flag = this.CalcPath.get_status() == 0;
      if (flag)
      {
        distance = 0.0f;
        for (int index = 0; index < this.CalcPath.get_corners().Length - 1; ++index)
          distance += Vector3.Distance(this.CalcPath.get_corners()[index], this.CalcPath.get_corners()[index + 1]);
      }
      return flag;
    }

    public float GetRemainingDistance(Actor actor, NavMeshPath path)
    {
      float num = float.MaxValue;
      if (Object.op_Equality((Object) actor, (Object) null) || path == null || path.get_status() != null)
        return num;
      num = 0.0f;
      for (int index = 0; index < this.CalcPath.get_corners().Length - 1; ++index)
        num += Vector3.Distance(this.CalcPath.get_corners()[index], this.CalcPath.get_corners()[index + 1]);
      return num;
    }

    protected virtual void StartWaypointRetention()
    {
      IEnumerator coroutine = this.WaypointRetentionCoroutine();
      if (this._waypointRetentionDisposable != null)
        this._waypointRetentionDisposable.Dispose();
      this._waypointRetentionDisposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (Component) this);
    }

    protected virtual Vector3 LastPosition()
    {
      if (((IReadOnlyList<Waypoint>) this._movePointList).IsNullOrEmpty<Waypoint>())
        return Object.op_Inequality((Object) this._destination, (Object) null) ? ((Component) this._destination).get_transform().get_position() : this.Position;
      Waypoint waypoint = this._movePointList.Last<Waypoint>();
      if (Object.op_Inequality((Object) waypoint, (Object) null))
        return ((Component) waypoint).get_transform().get_position();
      return Object.op_Inequality((Object) this._destination, (Object) null) ? ((Component) this._destination).get_transform().get_position() : this.Position;
    }

    [DebuggerHidden]
    protected virtual IEnumerator WaypointRetentionCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MovingPetAnimal.\u003CWaypointRetentionCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public NavMeshPath CalcPath
    {
      get
      {
        return this._calcPath ?? (this._calcPath = new NavMeshPath());
      }
    }

    [DebuggerHidden]
    protected virtual IEnumerator CalcWaypointRetentionCoroutine(
      IReadOnlyList<Waypoint> pointList,
      Vector2 limitDistance)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MovingPetAnimal.\u003CCalcWaypointRetentionCoroutine\u003Ec__Iterator1()
      {
        pointList = pointList,
        limitDistance = limitDistance,
        \u0024this = this
      };
    }

    protected bool SetNextMovePoint()
    {
      this.ReleaseDestination();
      Waypoint waypoint = (Waypoint) null;
      do
        ;
      while (!((IReadOnlyList<Waypoint>) this._movePointList).IsNullOrEmpty<Waypoint>() && !Object.op_Inequality((Object) (waypoint = this._movePointList.PopFront<Waypoint>()), (Object) null));
      if (Object.op_Equality((Object) waypoint, (Object) null))
        return false;
      this._destination = waypoint;
      this.Agent.SetDestination(waypoint.Position);
      return true;
    }

    protected bool ClosedDestination()
    {
      return !Object.op_Equality((Object) this._destination, (Object) null) && (double) Vector3.Distance(this._destination.Position, this.Position) <= (double) this.Agent.get_stoppingDistance();
    }

    protected virtual void StopWaypointRetention()
    {
      if (this._waypointRetentionDisposable == null)
        return;
      this._waypointRetentionDisposable.Dispose();
      this._waypointRetentionDisposable = (IDisposable) null;
    }

    protected virtual void ActivateNavMeshAgent()
    {
      ((Behaviour) this._obstacle).set_enabled(false);
      ((Behaviour) this._agent).set_enabled(false);
      ((Behaviour) this._agent).set_enabled(true);
    }

    protected virtual void ActivateNavMeshObstacle()
    {
      ((Behaviour) this._agent).set_enabled(false);
      ((Behaviour) this._obstacle).set_enabled(false);
      ((Behaviour) this._obstacle).set_enabled(true);
    }

    protected virtual void ReleaseAgentPath()
    {
      if (!((Behaviour) this.Agent).get_isActiveAndEnabled() || !this.Agent.get_isOnNavMesh())
        return;
      this.Agent.ResetPath();
    }

    protected virtual void ReleaseDestination()
    {
      if (Object.op_Equality((Object) this._destination, (Object) null))
        return;
      if (this._destination.Reserver == this)
        this._destination.Reserver = (INavMeshActor) null;
      this._destination = (Waypoint) null;
    }

    protected virtual void ReleaseMovePointList()
    {
      if (((IReadOnlyList<Waypoint>) this._movePointList).IsNullOrEmpty<Waypoint>())
        return;
      foreach (Waypoint movePoint in this._movePointList)
      {
        if (!Object.op_Equality((Object) movePoint, (Object) null) && movePoint.Reserver == this)
          movePoint.Reserver = (INavMeshActor) null;
      }
      this._movePointList.Clear();
    }
  }
}
