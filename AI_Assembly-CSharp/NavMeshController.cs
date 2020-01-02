// Decompiled with JetBrains decompiler
// Type: NavMeshController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using Sirenix.OdinInspector;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class NavMeshController : MonoBehaviour
{
  [SerializeField]
  private NavMeshAgent _agent;
  [Tooltip("最大移動速度")]
  [MinValue(0.100000001490116)]
  public float Speed;
  [Tooltip("加速度")]
  [MinValue(0.100000001490116)]
  public float Acceleration;
  [Tooltip("通常の旋回速度")]
  [MinValue(1.0)]
  public float AngularSpeed;
  [Tooltip("ターンする時の角度差")]
  [MinValue(1.0)]
  [MaxValue(360.0)]
  public float TurnAngle;
  [Tooltip("ターン時の旋回速度")]
  public float TurnAngularSpeed;
  [Tooltip("減速距離")]
  public float SpeedDownDistance;
  [Tooltip("停止距離")]
  public float StopDistance;
  [Header("アニメーション")]
  public Animator Animator;
  [SerializeField]
  [Tooltip("変動させるアニメーションのパラメーター名")]
  private string _animParamName;
  [SerializeField]
  [ReadOnly]
  private int _animParamNameHashCode;
  [Tooltip("移動速度とアニメーション速度の変換率")]
  public float Speed2Anim;
  [Tooltip("アニメを停止とみなす速度")]
  public float StopSpeed;
  private IDisposable changeSpeedLinearDisposable;
  private IDisposable changeAccelerationLinearDisposable;
  private float speed;

  public NavMeshController()
  {
    base.\u002Ector();
  }

  public NavMeshAgent Agent
  {
    get
    {
      return this._agent;
    }
  }

  public bool AgentActive
  {
    get
    {
      return Object.op_Inequality((Object) this._agent, (Object) null) && ((Behaviour) this._agent).get_isActiveAndEnabled();
    }
  }

  public string AnimParamName
  {
    get
    {
      return this._animParamName;
    }
    set
    {
      this._animParamName = value;
      this._animParamNameHashCode = Animator.StringToHash(this._animParamName);
    }
  }

  public int AnimParamNameHashCode
  {
    get
    {
      return this._animParamNameHashCode;
    }
  }

  public Vector3 Position
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

  public Vector3 Velocity { get; private set; }

  public Vector3 Forward
  {
    get
    {
      return ((Component) this).get_transform().get_forward();
    }
  }

  public Vector3 Up
  {
    get
    {
      return ((Component) this).get_transform().get_up();
    }
  }

  public Vector3 Right
  {
    get
    {
      return ((Component) this).get_transform().get_right();
    }
  }

  public Vector3 Destination
  {
    get
    {
      return this.Agent.get_destination();
    }
  }

  public int AreaMask
  {
    get
    {
      return this.Agent.get_areaMask();
    }
    set
    {
      this.Agent.set_areaMask(value);
    }
  }

  public bool ConsiderYAxis { get; set; }

  public bool MoveUpdateEnabled { get; set; }

  public bool AnimationUpdateEnabled { get; set; }

  public bool AnimatorEnabled
  {
    get
    {
      return Object.op_Inequality((Object) this.Animator, (Object) null) && ((Behaviour) this.Animator).get_isActiveAndEnabled();
    }
  }

  private void Awake()
  {
    if (Object.op_Equality((Object) this._agent, (Object) null))
      this._agent = ((Component) this).GetOrAddComponent<NavMeshAgent>();
    this._animParamNameHashCode = Animator.StringToHash(this._animParamName);
    NavMeshAgent agent = this.Agent;
    float num1 = 0.0f;
    this.Agent.set_acceleration(num1);
    float num2 = num1;
    this.Agent.set_angularSpeed(num2);
    double num3 = (double) num2;
    agent.set_speed((float) num3);
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.MoveUpdateEnabled)), (Action<M0>) (_ => this.OnUpdate()));
  }

  public Vector3 SteeringTarget
  {
    get
    {
      return this.Agent.get_steeringTarget();
    }
  }

  public bool IsReached
  {
    get
    {
      return (double) this.RemainingDistance <= (double) this.StopDistance;
    }
  }

  public float RemainingDistance
  {
    get
    {
      return this.Agent.get_remainingDistance();
    }
  }

  public bool HasDestination
  {
    get
    {
      return this.Agent.get_hasPath() || 0.0 < (double) this.RemainingDistance;
    }
  }

  public bool IsMoving
  {
    get
    {
      return this.HasDestination && !this.Agent.get_isStopped();
    }
  }

  public bool SetDestination(Vector3 _point, bool _isStopped = false)
  {
    if (!this.AgentActive)
      return false;
    this.Agent.set_isStopped(_isStopped);
    return this.Agent.SetDestination(_point);
  }

  public bool SetDestinationWithPathCheck(Vector3 _point, float _distance, bool _isStopped = false)
  {
    NavMeshPath _path = new NavMeshPath();
    NavMeshHit _hit = (NavMeshHit) null;
    return this.CalculatePath(_point, _path) && _path.get_status() == null && (this.FindClosestEdge(_point, out _hit) && ((NavMeshHit) ref _hit).get_hit()) && (double) _distance <= (double) ((NavMeshHit) ref _hit).get_distance() && this.SetDestination(_point, _isStopped);
  }

  public void ResetPath()
  {
    if (!this.AgentActive || !this.Agent.get_hasPath())
      return;
    this.Agent.ResetPath();
  }

  public void Resume()
  {
    if (!this.AgentActive || !this.Agent.get_isStopped())
      return;
    this.Agent.set_isStopped(false);
  }

  public void Stop()
  {
    if (!this.AgentActive || this.Agent.get_isStopped())
      return;
    this.Agent.set_isStopped(true);
  }

  public void Refresh()
  {
    if (!this.AgentActive)
      return;
    this.Agent.set_isStopped(true);
    this.Agent.ResetPath();
  }

  public bool CalculatePath(Vector3 _targetPosition, NavMeshPath _path)
  {
    return !Object.op_Equality((Object) this.Agent, (Object) null) && this.Agent.CalculatePath(_targetPosition, _path);
  }

  public bool FindClosestEdge(Vector3 _point, out NavMeshHit _hit)
  {
    return NavMesh.FindClosestEdge(_point, ref _hit, this.Agent.get_areaMask());
  }

  public void ChangeSpeedLinear(float _speed, float _second)
  {
    if (this.changeSpeedLinearDisposable != null)
      this.changeSpeedLinearDisposable.Dispose();
    float _prevSpeed = this.Speed;
    float _startTime = Time.get_realtimeSinceStartup();
    this.changeSpeedLinearDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear(_second, false), ((Component) this).get_gameObject()), false), (Action<M0>) (x =>
    {
      this.Speed = Mathf.Lerp(_prevSpeed, _speed, ((TimeInterval<float>) ref x).get_Value());
      Debug.Log((object) string.Format("Linear x[{0}] PrevSpeed[{1}] SetSpeed[{2}] Speed[{3}]", (object) ((TimeInterval<float>) ref x).get_Value(), (object) _prevSpeed, (object) _speed, (object) this.Speed));
    }), (Action) (() => Debug.Log((object) string.Format("使用時間[{0}]", (object) (float) ((double) Time.get_realtimeSinceStartup() - (double) _startTime)))));
  }

  public void ChangeAccelerationLinear(float _acceleration, float _second)
  {
    if (this.changeAccelerationLinearDisposable != null)
      this.changeAccelerationLinearDisposable.Dispose();
    float _prevAcceleration = this.Acceleration;
    float _startTime = Time.get_realtimeSinceStartup();
    this.changeAccelerationLinearDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear(_second, false), ((Component) this).get_gameObject()), false), (Action<M0>) (x =>
    {
      this.Acceleration = Mathf.Lerp(_prevAcceleration, _acceleration, ((TimeInterval<float>) ref x).get_Value());
      Debug.Log((object) string.Format("Linear x[{0}] PrevAcceleration[{1}] SetAcceleration[{2}] Acceleration[{3}]", (object) ((TimeInterval<float>) ref x).get_Value(), (object) _prevAcceleration, (object) _acceleration, (object) this.Acceleration));
    }), (Action) (() => Debug.Log((object) string.Format("使用時間[{0}]", (object) (float) ((double) Time.get_realtimeSinceStartup() - (double) _startTime)))));
  }

  public void SetEnabled(bool _enabled)
  {
    if (_enabled)
    {
      if (((Behaviour) this).get_enabled() != _enabled)
        ((Behaviour) this).set_enabled(_enabled);
      if (((Behaviour) this.Agent).get_enabled() == _enabled)
        return;
      ((Behaviour) this.Agent).set_enabled(_enabled);
    }
    else
    {
      if (((Behaviour) this.Agent).get_enabled() != _enabled)
        ((Behaviour) this.Agent).set_enabled(_enabled);
      if (((Behaviour) this).get_enabled() == _enabled)
        return;
      ((Behaviour) this).set_enabled(_enabled);
    }
  }

  public void Move(Vector3 _velocity)
  {
    if (this.AgentActive)
    {
      NavMeshAgent agent = this.Agent;
      Vector3 vector3_1 = _velocity;
      this.Velocity = vector3_1;
      Vector3 vector3_2 = vector3_1;
      agent.Move(vector3_2);
    }
    else
    {
      Vector3 position = this.Position;
      Vector3 vector3_1 = _velocity;
      this.Velocity = vector3_1;
      Vector3 vector3_2 = vector3_1;
      this.Position = Vector3.op_Addition(position, vector3_2);
    }
  }

  private void OnUpdate()
  {
    Vector3 zero = Vector3.get_zero();
    this.Velocity = zero;
    Vector3 vector3 = zero;
    float _speed = 0.0f;
    float deltaTime = Time.get_deltaTime();
    Vector3 destination = this.Destination;
    if (!this.AgentActive || this.Agent.get_pathPending() || this.Agent.get_isStopped())
    {
      vector3.x = (__Null) (double) (vector3.y = vector3.z = (__Null) 0.0f);
      this.speed = 0.0f;
    }
    else
    {
      this.speed = Mathf.Clamp(this.speed + this.Acceleration * deltaTime, 0.0f, this.Speed);
      _speed = this.speed * deltaTime;
      vector3 = Vector3.op_Subtraction(this.Agent.get_steeringTarget(), this.Position);
      vector3.y = (__Null) 0.0;
      float num1 = this.AngularSpeed * deltaTime;
      if (!this.IsReached)
      {
        float num2 = Vector3.SignedAngle(this.Forward, vector3, Vector3.get_up());
        if ((double) this.TurnAngle < (double) Mathf.Abs(num2))
        {
          float num3 = this.TurnAngularSpeed * deltaTime;
          ((Component) this).get_transform().Rotate(0.0f, Mathf.Min(Mathf.Abs(num2), num3) * Mathf.Sign(num2), 0.0f);
          vector3 = Vector3.get_zero();
          _speed = this.speed = 0.0f;
        }
        else
        {
          if ((double) this.RemainingDistance < (double) this.SpeedDownDistance)
            _speed *= (float) (1.0 - (double) Mathf.Abs(num2) / (double) this.TurnAngle);
          if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < (double) _speed * (double) _speed)
          {
            _speed = ((Vector3) ref vector3).get_magnitude();
            ((Component) this).get_transform().Rotate(0.0f, num2, 0.0f);
          }
          else
            ((Component) this).get_transform().Rotate(0.0f, Mathf.Min(Mathf.Abs(num2), num1) * Mathf.Sign(num2), 0.0f);
          vector3 = Vector3.op_Multiply(this.Forward, _speed);
        }
      }
      else
      {
        _speed = this.speed = 0.0f;
        vector3 = Vector3.get_zero();
        this.Agent.ResetPath();
        this.Agent.set_isStopped(true);
      }
    }
    this.Velocity = vector3;
    this.Agent.Move(vector3);
    if (!this.AnimatorEnabled || !this.AnimationUpdateEnabled)
      return;
    this.AnimationUpdate(_speed);
  }

  private void SetAnimator(Animator _animator)
  {
    this.Animator = _animator;
  }

  private void AnimationUpdate(float _speed)
  {
    float num1 = this.Speed * Time.get_deltaTime();
    if ((double) num1 == 0.0)
    {
      this.Animator.SetFloat(this.AnimParamNameHashCode, 0.0f);
    }
    else
    {
      float num2 = _speed / num1 * this.Speed2Anim;
      if ((double) num2 <= (double) this.StopSpeed)
        this.Animator.SetFloat(this.AnimParamNameHashCode, 0.0f);
      else
        this.Animator.SetFloat(this.AnimParamNameHashCode, num2);
    }
  }

  public enum DistanceTypes
  {
    XZ,
    XYZ,
    CutHeight,
  }
}
