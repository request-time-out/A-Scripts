// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using AIProject.Definitions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  [RequireComponent(typeof (NavMeshAgent))]
  public abstract class AnimalGround : AnimalBase, INavMeshActor
  {
    protected static List<int> priorityList = new List<int>();
    [SerializeField]
    [DisableInPlayMode]
    [Tooltip("Agentの優先度を自動で設定するか")]
    private bool agentPriorityAutoSetting = true;
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    [Tooltip("次のポイントへの最短距離")]
    protected float nextPointDistanceMin = 75f;
    [SerializeField]
    [Tooltip("次のポイントへの最長距離")]
    protected float nextPointDistanceMax = 1000f;
    [SerializeField]
    [Tooltip("自動移動から手動移動に切り替える距離")]
    protected float disableUpdateDistance = 15f;
    [SerializeField]
    protected float addAngle = 120f;
    protected bool prevAgentEnabled = true;
    private Vector3 toTargetVelocity = Vector3.get_zero();
    protected float CutDistance = 5f;
    protected bool CutDistanceFlag = true;
    protected bool correctFlag = true;
    protected float correctHeight = 5f;
    private List<Waypoint> dummyWaypointList = new List<Waypoint>();
    [NonSerialized]
    public float breakTime = -1f;
    [SerializeField]
    private NavMeshAgent agent_;
    [SerializeField]
    private NavMeshObstacle obstacle_;
    protected LocomotionArea MoveArea;
    protected Waypoint currentWaypoint;
    private int locomotionCount_;
    protected AnimalStateController stateController;
    protected bool prevSetAgentState;
    protected IDisposable checkWaypointListDisposable;
    protected System.Action ScheduleEndEvent;
    protected NavMeshPath calculatePath;

    public float WalkSpeed
    {
      get
      {
        return this.walkSpeed;
      }
    }

    public float RunSpeed
    {
      get
      {
        return this.runSpeed;
      }
    }

    protected AnimalState StateIorL
    {
      get
      {
        return (double) Random.get_value() < 0.5 ? AnimalState.Idle : AnimalState.Locomotion;
      }
    }

    public NavMeshAgent Agent
    {
      get
      {
        return this.agent_;
      }
    }

    public NavMeshObstacle Obstacle
    {
      get
      {
        return this.obstacle_;
      }
    }

    public bool IsActiveAgent
    {
      get
      {
        return Object.op_Inequality((Object) this.agent_, (Object) null) && ((Behaviour) this.agent_).get_isActiveAndEnabled() && this.agent_.get_isOnNavMesh();
      }
    }

    protected float FirstStoppingDistance { get; private set; } = 1f;

    public List<Waypoint> WaypointList { get; } = new List<Waypoint>();

    public int WaypointCount
    {
      get
      {
        return this.WaypointList.Count;
      }
    }

    public int LocomotionCount
    {
      get
      {
        return this.locomotionCount_;
      }
      set
      {
        this.locomotionCount_ = Mathf.Max(0, value);
        this.SetAgentAutoBraking();
      }
    }

    public override int NavMeshAreaMask
    {
      get
      {
        return Object.op_Inequality((Object) this.Agent, (Object) null) ? this.Agent.get_areaMask() : 0;
      }
    }

    public int Priority { get; protected set; } = 51;

    public virtual float NextPointDistanceMin
    {
      get
      {
        return Mathf.Min(this.nextPointDistanceMin, this.nextPointDistanceMax);
      }
    }

    public virtual float NextPointDistanceMax
    {
      get
      {
        return Mathf.Max(this.nextPointDistanceMin, this.nextPointDistanceMax);
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this.agent_, (Object) null))
        this.agent_ = (NavMeshAgent) ((Component) this).GetComponent<NavMeshAgent>();
      if (Object.op_Equality((Object) this.agent_, (Object) null))
      {
        this.Destroy();
      }
      else
      {
        if (Object.op_Equality((Object) this.obstacle_, (Object) null))
          this.obstacle_ = (NavMeshObstacle) ((Component) this).GetComponent<NavMeshObstacle>();
        this.FirstStoppingDistance = this.agent_.get_stoppingDistance();
        this.prevAgentEnabled = ((Behaviour) this.agent_).get_enabled();
        if (this.stateController == null)
          this.stateController = new AnimalStateController();
        if (this.agentPriorityAutoSetting)
        {
          int num1 = 51;
          int num2 = 5;
          if (Singleton<Manager.Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Resources>.Instance.AnimalDefinePack, (Object) null))
          {
            AnimalDefinePack.NavMeshAgentInfoGroup agentInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.AgentInfo;
            if (agentInfo != null)
            {
              num1 = agentInfo.GroundAnimalStartPriority;
              num2 = agentInfo.PriorityMargin;
            }
          }
          this.Priority = num1;
          for (int index = 0; index < AnimalGround.priorityList.Count && this.Priority >= AnimalGround.priorityList[index]; ++index)
            this.Priority += num2;
          AnimalGround.priorityList.Add(this.Priority);
          AnimalGround.priorityList.Sort();
          this.Agent.set_avoidancePriority(this.Priority);
        }
        ((Behaviour) this.Agent).set_enabled(false);
      }
    }

    protected bool Off { get; private set; }

    protected bool prevAgentStopped { get; private set; }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (this.Off && this.prevSetAgentState)
      {
        this.ResetState();
        if (Object.op_Inequality((Object) this.Agent, (Object) null) && ((Behaviour) this.Agent).get_isActiveAndEnabled() && (this.Agent.get_isOnNavMesh() && this.Agent.get_isStopped() != this.prevAgentStopped))
          this.Agent.set_isStopped(this.prevAgentStopped);
      }
      this.Off = false;
    }

    protected override void OnDisable()
    {
      if (Object.op_Inequality((Object) this.Agent, (Object) null) && ((Behaviour) this.Agent).get_isActiveAndEnabled() && this.Agent.get_isOnNavMesh())
      {
        this.prevAgentStopped = this.Agent.get_isStopped();
        if (!this.prevAgentStopped)
          this.Agent.set_isStopped(true);
        this.prevSetAgentState = true;
      }
      else
        this.prevSetAgentState = false;
      base.OnDisable();
      this.Off = true;
    }

    protected void StartCheckCoroutine()
    {
      IEnumerator _coroutine = this.CheckWaypointList();
      if (this.checkWaypointListDisposable != null)
        this.checkWaypointListDisposable.Dispose();
      this.checkWaypointListDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.TakeUntilDestroy<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Component) this), (Component) this.Agent), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())));
    }

    protected void StopCheckCoroutine()
    {
      if (this.checkWaypointListDisposable == null)
        return;
      this.checkWaypointListDisposable.Dispose();
      this.checkWaypointListDisposable = (IDisposable) null;
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      this.SetDestroyState();
      this.StopCheckCoroutine();
      this.RemoveActionPoint();
      this.ClearWaypoint();
      if (this.agentPriorityAutoSetting && AnimalGround.priorityList.Contains(this.Priority))
        AnimalGround.priorityList.Remove(this.Priority);
      base.OnDestroy();
    }

    public void SetWaypoints(int _chunkID)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Dictionary<int, Chunk> chunkTable = Singleton<Manager.Map>.Instance.ChunkTable;
      Chunk chunk = (Chunk) null;
      if (!chunkTable.TryGetValue(_chunkID, out chunk))
        return;
      if (this.MoveArea == null)
        this.MoveArea = new LocomotionArea();
      this.MoveArea.SetWaypoint(chunk.Waypoints);
    }

    public override void Clear()
    {
      this.StopCheckCoroutine();
      this.MoveArea = (LocomotionArea) null;
      this.ClearWaypoint();
      this.stateController.Clear();
      base.Clear();
    }

    public override void ReleaseBody()
    {
      this.eyeController = (EyeLookControllerVer2) null;
      this.neckController = (NeckLookControllerVer3) null;
      base.ReleaseBody();
    }

    public override void CreateBody()
    {
      base.CreateBody();
      this.eyeController = (EyeLookControllerVer2) ((Component) ((Component) this).get_transform()).GetComponentInChildren<EyeLookControllerVer2>(true);
      this.neckController = (NeckLookControllerVer3) ((Component) this).GetComponentInChildren<NeckLookControllerVer3>(true);
      this.BodyEnabled = false;
    }

    protected override void ChangedStateEvent()
    {
      this.ResetAgent();
      this.destination = new Vector3?();
      this.LocomotionCount = 0;
      this.ClearSchedule();
    }

    protected void ClearSchedule()
    {
      this.schedule = new AnimalSchedule(false, DateTime.MinValue, TimeSpan.Zero, false);
    }

    protected bool SetSchedule(AnimalPlayState.PlayStateInfo _stateInfo)
    {
      if (_stateInfo == null)
      {
        this.schedule = new AnimalSchedule(false, DateTime.MinValue, TimeSpan.Zero, false);
        return false;
      }
      int num1 = Mathf.Min(_stateInfo.LoopMin, _stateInfo.LoopMax);
      int num2 = Mathf.Max(_stateInfo.LoopMin, _stateInfo.LoopMax);
      int num3 = num1 == num2 ? num1 : Random.Range(num1, num2 + 1);
      this.schedule = new AnimalSchedule(_stateInfo.IsLoop, Singleton<Manager.Map>.Instance.Simulator.Now, TimeSpan.FromMinutes((double) num3), true);
      return true;
    }

    protected bool SetSchedule(AnimalPlayState _playState)
    {
      return this.SetSchedule(_playState?.MainStateInfo);
    }

    public void ActivateNavMeshAgent()
    {
      ((Behaviour) this.obstacle_).SetEnableSelf(false);
      ((Behaviour) this.agent_).SetEnableSelf(true);
    }

    public void ActivateNavMeshObstacle()
    {
      ((Behaviour) this.agent_).SetEnableSelf(false);
      ((Behaviour) this.obstacle_).SetEnableSelf(true);
    }

    public void DeactivateNavMeshElements()
    {
      ((Behaviour) this.agent_).SetEnableSelf(false);
      ((Behaviour) this.obstacle_).SetEnableSelf(false);
    }

    protected override void OnUpdate()
    {
      if (this.schedule.managing && this.StateUpdatePossible)
        this.ScheduleUpdate();
      base.OnUpdate();
    }

    public bool CheckBothTheIndoor(Vector3 _position)
    {
      MapArea.AreaType _type;
      return this.GetCurrentMapAreaType(this.CurrentMapArea, out _type) && _type == MapArea.AreaType.Indoor && AnimalBase.GetMapArea(_position, out MapArea _, out _type) && _type == MapArea.AreaType.Indoor;
    }

    public bool CheckBothTheIndoor(Actor _actor)
    {
      MapArea.AreaType _type;
      return this.GetCurrentMapAreaType(this.CurrentMapArea, out _type) && _type == MapArea.AreaType.Indoor && AnimalBase.GetMapAreaType(_actor.Position, _actor.MapArea, out _type) && _type == MapArea.AreaType.Indoor;
    }

    protected override void OnUpdateFirst()
    {
      Vector3 velocity = this.Agent.get_velocity();
      int num = 0.0 >= (double) ((Vector3) ref velocity).get_sqrMagnitude() ? 0 : this.Priority;
      if (this.Agent.get_avoidancePriority() != num)
        this.Agent.set_avoidancePriority(num);
      this.LocomotionOnClosePosition();
      this.LookTargetUpdate();
    }

    public override void OnMinuteUpdate(TimeSpan _deltaTime)
    {
      if (!this.schedule.managing || !this.schedule.enable)
        return;
      this.schedule.elapsedTime = this.schedule.elapsedTime + _deltaTime;
      if (!(this.schedule.duration < this.schedule.elapsedTime))
        return;
      this.schedule.enable = false;
    }

    private Vector3 ToTarget
    {
      get
      {
        return this.destination.HasValue ? Vector3.op_Subtraction(this.destination.Value, this.Position) : Vector3.get_zero();
      }
    }

    protected void LocomotionOnClosePosition()
    {
      if (this.LocomotionCount == 0 || !((Behaviour) this.Agent).get_enabled())
        return;
      if (1 < this.LocomotionCount)
      {
        if (this.HasDestination)
          this.destination = new Vector3?();
        if (this.Agent.get_updateRotation())
          return;
        this.Agent.set_updateRotation(true);
      }
      else
      {
        if (!this.AgentHasPath && !this.HasDestination || (this.HasTwoCorners || this.Wait()) || this.Agent.get_isStopped())
          return;
        if (!this.HasDestination && this.IsNearPointHasPath(this.disableUpdateDistance))
        {
          this.destination = new Vector3?(this.Agent.get_destination());
          this.Agent.set_updateRotation(false);
          Vector3 velocity = this.Agent.get_velocity();
          this.toTargetVelocity = new Vector3(0.0f, 0.0f, ((Vector3) ref velocity).get_magnitude());
          this.Agent.ResetPath();
        }
        else if (this.HasDestination && !this.IsNearPoint(this.disableUpdateDistance))
        {
          this.Agent.SetDestination(this.destination.Value);
          this.Agent.set_updateRotation(true);
          this.destination = new Vector3?();
        }
        if (!this.HasDestination)
          return;
        Vector3 vector3_1 = this.destination.Value;
        Vector2 vector2_1;
        ((Vector2) ref vector2_1).\u002Ector((float) this.Forward.x, (float) this.Forward.z);
        Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
        Vector2 vector2_2;
        ((Vector2) ref vector2_2).\u002Ector((float) (vector3_1.x - this.Position.x), (float) (vector3_1.z - this.Position.z));
        Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
        float num1 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
        if (0.0 < (double) num1)
        {
          Vector3 vector3_2 = Vector3.Cross(new Vector3((float) normalized1.x, 0.0f, (float) normalized1.y), new Vector3((float) normalized2.x, 0.0f, (float) normalized2.y));
          Vector3 eulerAngles = this.EulerAngles;
          float num2 = this.addAngle * Time.get_deltaTime() * Mathf.Sign((float) vector3_2.y);
          if ((double) num1 <= (double) Mathf.Abs(num2))
          {
            ref Vector3 local = ref eulerAngles;
            local.y = (__Null) (local.y + (double) num1 * (double) Mathf.Sign((float) vector3_2.y));
          }
          else
          {
            ref Vector3 local = ref eulerAngles;
            local.y = (__Null) (local.y + (double) num2);
          }
          eulerAngles.y = (__Null) (double) this.AngleAbs((float) eulerAngles.y);
          this.EulerAngles = eulerAngles;
        }
        float num3 = 45f;
        if ((double) num1 < (double) num3)
        {
          ref Vector3 local = ref this.toTargetVelocity;
          local.z = (__Null) (local.z + (double) this.Agent.get_acceleration() * (double) Time.get_deltaTime());
          this.toTargetVelocity.z = (__Null) (double) Mathf.Min((float) this.toTargetVelocity.z, this.Agent.get_speed());
          this.Agent.set_velocity(Vector3.op_Multiply(Vector3.op_Multiply(Quaternion.op_Multiply(this.Rotation, this.toTargetVelocity), Time.get_timeScale()), (float) (1.0 - (double) num1 / (double) num3)));
        }
        else
        {
          this.Agent.set_velocity(Vector3.get_zero());
          this.toTargetVelocity = Vector3.get_zero();
        }
      }
    }

    public bool AgentPathPending
    {
      get
      {
        return this.Agent.get_pathPending();
      }
    }

    public bool AgentHasPath
    {
      get
      {
        return !this.Agent.get_pathPending() && this.Agent.get_hasPath();
      }
    }

    public bool AgentActive
    {
      get
      {
        return ((Behaviour) this.Agent).get_isActiveAndEnabled() && this.Agent.get_isOnNavMesh();
      }
    }

    public bool HasTwoCorners
    {
      get
      {
        return this.Agent.get_hasPath() && 2 < this.Agent.get_path().get_corners().Length;
      }
    }

    protected void ResetAgent()
    {
      if (this.CurrentState == AnimalState.Destroyed)
        return;
      bool flag1 = this.AgentMove();
      bool flag2 = this.AgentStop();
      if (flag1)
        this.ActivateNavMeshAgent();
      else if (flag2)
        this.ActivateNavMeshObstacle();
      if (!((Behaviour) this.Agent).get_isActiveAndEnabled() || !this.Agent.get_isOnNavMesh())
        return;
      this.Agent.ResetPath();
      this.Agent.set_autoBraking(true);
      if (flag1)
        return;
      this.Agent.set_velocity(Vector3.get_zero());
    }

    public virtual bool AgentMove()
    {
      return this.CurrentState == AnimalState.Locomotion || this.CurrentState == AnimalState.LovelyFollow || (this.CurrentState == AnimalState.Escape || this.CurrentState == AnimalState.ToDepop) || this.CurrentState == AnimalState.ToIndoor;
    }

    public virtual bool AgentStop()
    {
      return this.CurrentState == AnimalState.WithPlayer || this.CurrentState == AnimalState.WithAgent;
    }

    protected void SetAgentSpeed(AnimalGround.LocomotionTypes _type)
    {
      if (_type != AnimalGround.LocomotionTypes.Walk)
      {
        if (_type != AnimalGround.LocomotionTypes.Run)
          return;
        this.Agent.set_speed(this.runSpeed);
      }
      else
        this.Agent.set_speed(this.walkSpeed);
    }

    protected void SetAutoBraking()
    {
      if (!Object.op_Implicit((Object) this.Agent))
        return;
      this.Agent.set_autoBraking(this.LocomotionCount <= 1);
    }

    public void SetAgentAutoBraking()
    {
      if (!Object.op_Implicit((Object) this.Agent))
        return;
      this.Agent.set_autoBraking(this.LocomotionCount <= 1);
    }

    public Vector3 CurrentPosition(Vector3 _position)
    {
      if (this.CutDistanceFlag && (double) Mathf.Abs((float) _position.y) <= (double) this.CutDistance)
        _position.y = (__Null) 0.0;
      return _position;
    }

    public bool HasAgentPath
    {
      get
      {
        return ((Behaviour) this.Agent).get_isActiveAndEnabled() && this.Agent.get_hasPath();
      }
    }

    public bool HasNotAgentPath
    {
      get
      {
        if (!((Behaviour) this.Agent).get_isActiveAndEnabled())
          return true;
        return !this.Agent.get_pathPending() && !this.Agent.get_hasPath();
      }
    }

    public bool isNeutralCommand { get; protected set; } = true;

    public override void ChangeState(AnimalState _nextState, System.Action _changeEvent = null)
    {
      this.isNeutralCommand = false;
      if (this.AutoChangeAnimation)
      {
        this.AutoChangeAnimation = false;
        this.PlayOutAnim((System.Action) (() =>
        {
          this.AutoChangeAnimation = true;
          this.stateController.ChangeState(_nextState);
          this.isNeutralCommand = true;
          System.Action action = _changeEvent;
          if (action == null)
            return;
          action();
        }));
      }
      else
      {
        this.stateController.ChangeState(_nextState);
        this.isNeutralCommand = true;
        System.Action action = _changeEvent;
        if (action == null)
          return;
        action();
      }
    }

    public override void SetState(AnimalState _nextState, System.Action _changeEvent = null)
    {
      this.isNeutralCommand = false;
      if (this.AutoChangeAnimation)
      {
        this.AutoChangeAnimation = false;
        this.PlayOutAnim((System.Action) (() =>
        {
          this.AutoChangeAnimation = true;
          this.stateController.SetState(_nextState);
          this.isNeutralCommand = true;
          System.Action action = _changeEvent;
          if (action == null)
            return;
          action();
        }));
      }
      else
      {
        this.stateController.SetState(_nextState);
        this.isNeutralCommand = true;
        System.Action action = _changeEvent;
        if (action == null)
          return;
        action();
      }
    }

    public bool TargetHasPath
    {
      get
      {
        return Object.op_Inequality((Object) this.Target, (Object) null) && this.Agent.get_hasPath();
      }
    }

    protected Vector3 CorrectPosition(Vector3 _position)
    {
      if (this.correctFlag && (double) Mathf.Abs((float) _position.y) <= (double) this.correctHeight)
        _position.y = (__Null) 0.0;
      return _position;
    }

    protected Vector3 CorrectPosition(Vector3 _position, bool _correctFlag)
    {
      if (_correctFlag && (double) Mathf.Abs((float) _position.y) <= (double) this.correctHeight)
        _position.y = (__Null) 0.0;
      return _position;
    }

    protected bool IsNearPoint(Vector3 _targetPoint, float _distance)
    {
      Vector3 vector3 = this.CorrectPosition(Vector3.op_Subtraction(_targetPoint, this.Position));
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) Mathf.Pow(_distance, 2f);
    }

    protected bool IsNearPointHasPath()
    {
      return this.AgentHasPath && (double) this.Agent.get_remainingDistance() <= (double) this.Agent.get_stoppingDistance();
    }

    protected bool IsNearPointHasPath(float _stoppingDistance)
    {
      return this.AgentHasPath && (double) this.Agent.get_remainingDistance() <= (double) _stoppingDistance;
    }

    protected bool IsNearPointNotHasPath(Vector3 _point, float _stoppingDistance)
    {
      if (!this.HasNotAgentPath)
        return false;
      Vector3 vector3 = this.CorrectPosition(Vector3.op_Subtraction(_point, this.Position));
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) Mathf.Pow(_stoppingDistance, 2f);
    }

    protected bool IsNearPoint(Vector3 _point)
    {
      Vector3 vector3 = this.CorrectPosition(Vector3.op_Subtraction(_point, this.Position));
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) Mathf.Pow(this.Agent.get_stoppingDistance(), 2f);
    }

    public bool IsNearPoint()
    {
      if (this.HasActionPoint)
        return this.IsNearPoint(this.actionPoint.Destination);
      if (this.AgentHasPath)
        return this.IsNearPointHasPath();
      if (this.HasDestination)
        return this.IsNearPoint(this.destination.Value);
      return Object.op_Inequality((Object) this.currentWaypoint, (Object) null) && this.IsNearPoint(((Component) this.currentWaypoint).get_transform().get_position());
    }

    public bool IsNearPoint(float _stoppingDistance)
    {
      if (this.AgentHasPath)
        return this.IsNearPointHasPath(_stoppingDistance);
      if (this.HasDestination)
        return this.IsNearPointNotHasPath(this.destination.Value, _stoppingDistance);
      return Object.op_Inequality((Object) this.currentWaypoint, (Object) null) && this.IsNearPoint(((Component) this.currentWaypoint).get_transform().get_position(), _stoppingDistance);
    }

    public bool IsNearTargetPoint()
    {
      if (!this.TargetHasPath)
        return false;
      Vector3 vector3 = this.CorrectPosition(Vector3.op_Subtraction(this.Target.get_position(), this.Position));
      return (double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) Mathf.Pow(this.Agent.get_stoppingDistance(), 2f);
    }

    protected void StateEndEvent()
    {
      if (this.ScheduleEndEvent == null)
        return;
      System.Action scheduleEndEvent = this.ScheduleEndEvent;
      this.ScheduleEndEvent = (System.Action) null;
      scheduleEndEvent();
    }

    protected bool ScheduleUpdate()
    {
      if (this.schedule.managing)
      {
        if (!this.schedule.enable)
        {
          this.StateEndEvent();
          return true;
        }
      }
      else if (!this.AnimationKeepWaiting())
      {
        this.StateEndEvent();
        return true;
      }
      return false;
    }

    protected bool UseWaypoint(Waypoint _waypoint)
    {
      return !Object.op_Equality((Object) _waypoint, (Object) null) && _waypoint.Reserver != null;
    }

    protected void ClearWaypoint()
    {
      this.ClearCurrentWaypoint();
      if (((IReadOnlyList<Waypoint>) this.WaypointList).IsNullOrEmpty<Waypoint>())
        return;
      foreach (Waypoint waypoint in this.WaypointList)
      {
        if (!Object.op_Equality((Object) waypoint, (Object) null) && waypoint.Reserver == this)
          waypoint.Reserver = (INavMeshActor) null;
      }
      this.WaypointList.Clear();
    }

    protected void ClearCurrentWaypoint()
    {
      if (!Object.op_Inequality((Object) this.currentWaypoint, (Object) null))
        return;
      if (this.currentWaypoint.Reserver == this)
        this.currentWaypoint.Reserver = (INavMeshActor) null;
      this.currentWaypoint = (Waypoint) null;
    }

    protected bool SetNextWaypoint(Waypoint _waypoint)
    {
      if (Object.op_Equality((Object) _waypoint, (Object) null) || _waypoint.Reserver != null && _waypoint.Reserver != this)
        return false;
      bool enabled = ((Behaviour) this.Agent).get_enabled();
      this.ActivateNavMeshAgent();
      if (this.calculatePath == null)
        this.calculatePath = new NavMeshPath();
      if (this.Agent.get_pathStatus() == 2 || !this.Agent.CalculatePath(((Component) _waypoint).get_transform().get_position(), this.calculatePath) || (this.calculatePath.get_status() != null || !this.Agent.SetPath(this.calculatePath)))
      {
        if (enabled)
          this.ActivateNavMeshAgent();
        else
          this.ActivateNavMeshObstacle();
        return false;
      }
      this.calculatePath = (NavMeshPath) null;
      this.ClearCurrentWaypoint();
      this.destination = new Vector3?();
      _waypoint.Reserver = (INavMeshActor) this;
      this.currentWaypoint = _waypoint;
      this.Agent.set_isStopped(false);
      return true;
    }

    protected bool ChangeNextWaypoint()
    {
      if (((Behaviour) this.Agent).get_isActiveAndEnabled())
        this.Agent.ResetPath();
      this.destination = new Vector3?();
      if (!((Behaviour) this.Agent).get_isActiveAndEnabled())
      {
        this.ClearCurrentWaypoint();
        return false;
      }
      if (Object.op_Inequality((Object) this.currentWaypoint, (Object) null))
      {
        if (this.IsNearPoint(((Component) this.currentWaypoint).get_transform().get_position(), this.NextPointDistanceMin))
        {
          this.ClearCurrentWaypoint();
        }
        else
        {
          if (this.Agent.SetDestination(((Component) this.currentWaypoint).get_transform().get_position()))
          {
            this.SetAutoBraking();
            return true;
          }
          this.ClearCurrentWaypoint();
        }
      }
      bool flag = this.WaypointCount <= 0;
      this.Agent.set_isStopped(flag);
      if (flag)
        return false;
      while (0 < this.WaypointCount)
      {
        this.currentWaypoint = this.WaypointList.PopFront<Waypoint>();
        if (this.currentWaypoint.Reserver == this && this.Agent.SetDestination(((Component) this.currentWaypoint).get_transform().get_position()))
        {
          this.SetAutoBraking();
          return true;
        }
        this.ClearCurrentWaypoint();
      }
      return false;
    }

    protected virtual List<Waypoint> GetWaypointList(
      Vector3 _startPosition,
      float _minDistance,
      float _maxDistance)
    {
      LocomotionArea.AreaType _areaType = LocomotionArea.AreaType.Normal;
      switch (this.BreedingType)
      {
        case BreedingTypes.Wild:
          _areaType = LocomotionArea.AreaType.Normal;
          break;
        case BreedingTypes.Pet:
          _areaType = !this.IndoorMode ? LocomotionArea.AreaType.Normal | LocomotionArea.AreaType.Indoor : LocomotionArea.AreaType.Indoor;
          break;
      }
      return Object.op_Equality((Object) this.TargetMapArea, (Object) null) ? this.MoveArea.GetPointList(_startPosition, _minDistance, _maxDistance, _areaType) : this.MoveArea.GetPointList(_startPosition, _minDistance, _maxDistance, this.TargetMapArea, _areaType);
    }

    [DebuggerHidden]
    protected IEnumerator IAddWaypoint(float _minDistance, float _maxDistance)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalGround.\u003CIAddWaypoint\u003Ec__Iterator0()
      {
        _minDistance = _minDistance,
        _maxDistance = _maxDistance,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    protected IEnumerator CheckWaypointList()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalGround.\u003CCheckWaypointList\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    protected override void EnterWait()
    {
      this.RefreshCommands(true);
      this.PlayInAnim(AnimationCategoryID.Idle, 0, (System.Action) null);
    }

    protected override void ExitWait()
    {
      this.RefreshCommands(true);
    }

    protected override void EnterIdle()
    {
      this.ActivateNavMeshObstacle();
      this.PlayInAnim(AnimationCategoryID.Idle, 0, (System.Action) null);
      this.SetSchedule(this.CurrentAnimState);
    }

    protected override void OnIdle()
    {
      if (this.schedule.managing)
      {
        if (this.schedule.enable)
          return;
        this.ClearSchedule();
        this.SetState(AnimalState.Locomotion, (System.Action) null);
      }
      else
      {
        if (this.AnimationKeepWaiting())
          return;
        this.SetState(AnimalState.Locomotion, (System.Action) null);
      }
    }

    protected override void EnterLocomotion()
    {
      this.ActivateNavMeshAgent();
      this.SetAgentSpeed(AnimalGround.LocomotionTypes.Walk);
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (System.Action) null);
      this.ChangeNextWaypoint();
      this.LocomotionCount = 999;
    }

    protected override void OnLocomotion()
    {
      if (this.Wait() || this.AgentPathPending)
        return;
      if (this.IsNearPoint())
      {
        if (!this.HasActionPoint)
          this.ChangeNextWaypoint();
        else if (!this.SetNextState())
        {
          this.StateEndEvent();
          return;
        }
      }
      else if (!this.AgentHasPath)
      {
        if (this.HasActionPoint)
          this.Agent.SetDestination(this.actionPoint.Destination);
        else
          this.ChangeNextWaypoint();
      }
      if (!((Behaviour) this.Agent).get_isActiveAndEnabled() || !this.Agent.get_hasPath())
        return;
      this.Agent.SetDestination(this.Agent.get_destination());
    }

    protected override void AnimationLocomotion()
    {
      this.WalkAnimationUpdate();
    }

    protected override void ExitLocomotion()
    {
      this.ClearCurrentWaypoint();
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
        flag = commandPartner.Mode != Desire.ActionType.EndTaskWildAnimal;
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

    protected void WalkAnimationUpdate()
    {
      double num1;
      if ((double) this.Agent.get_speed() != 0.0)
      {
        Vector3 velocity = this.Agent.get_velocity();
        num1 = (double) ((Vector3) ref velocity).get_magnitude() / (double) this.Agent.get_speed();
      }
      else
        num1 = 0.0;
      float num2 = Mathf.Clamp((float) num1, 0.0f, 0.5f);
      string locomotionParamName = AnimalBase.DefaultLocomotionParamName;
      if (Singleton<Manager.Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Resources>.Instance.AnimalDefinePack, (Object) null))
      {
        AnimalDefinePack.AnimatorInfoGroup animatorInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.AnimatorInfo;
        if (animatorInfo != null)
          locomotionParamName = animatorInfo.LocomotionParamName;
      }
      this.SetFloat(locomotionParamName, num2);
    }

    protected void RunAnimationUpdate()
    {
      double num1;
      if ((double) this.Agent.get_speed() != 0.0)
      {
        Vector3 velocity = this.Agent.get_velocity();
        num1 = (double) ((Vector3) ref velocity).get_magnitude() / (double) this.Agent.get_speed() * 1.25;
      }
      else
        num1 = 0.0;
      float num2 = Mathf.Clamp((float) num1, 0.0f, 1f);
      string locomotionParamName = AnimalBase.DefaultLocomotionParamName;
      if (Singleton<Manager.Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Resources>.Instance.AnimalDefinePack, (Object) null))
      {
        AnimalDefinePack.AnimatorInfoGroup animatorInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.AnimatorInfo;
        if (animatorInfo != null)
          locomotionParamName = animatorInfo.LocomotionParamName;
      }
      this.SetFloat(locomotionParamName, num2);
    }

    public override bool IsWithAgentFree(AgentActor _actor)
    {
      return !this.IsWithActor && (!Object.op_Inequality((Object) this.CommandPartner, (Object) null) || !(this.CommandPartner is AgentActor) || !Object.op_Inequality((Object) this.CommandPartner, (Object) _actor)) && (this.CurrentState != AnimalState.Repop && this.CurrentState != AnimalState.Depop && this.CurrentState != AnimalState.ToDepop);
    }

    protected bool AvailablePoint(Waypoint _point)
    {
      return !Object.op_Equality((Object) _point, (Object) null) && ((Component) _point).get_gameObject().get_activeSelf() && !Object.op_Equality((Object) _point.OwnerArea, (Object) null) && (_point.Reserver == null || _point.Reserver == this);
    }

    protected enum LocomotionTypes
    {
      Walk,
      Run,
    }
  }
}
