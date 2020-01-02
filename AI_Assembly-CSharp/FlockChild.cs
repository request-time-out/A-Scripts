// Decompiled with JetBrains decompiler
// Type: FlockChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class FlockChild : MonoBehaviour
{
  [HideInInspector]
  public FlockController _spawner;
  [HideInInspector]
  public Vector3 _wayPoint;
  public float _speed;
  [HideInInspector]
  public bool _dived;
  [HideInInspector]
  public float _stuckCounter;
  [HideInInspector]
  public float _damping;
  [HideInInspector]
  public bool _soar;
  [HideInInspector]
  public bool _landing;
  [HideInInspector]
  public float _targetSpeed;
  [HideInInspector]
  public bool _move;
  public GameObject _model;
  public Transform _modelT;
  [HideInInspector]
  public float _avoidValue;
  [HideInInspector]
  public float _avoidDistance;
  private float _soarTimer;
  private bool _instantiated;
  private static int _updateNextSeed;
  private int _updateSeed;
  [HideInInspector]
  public bool _avoid;
  public Transform _thisT;
  public Vector3 _landingPosOffset;

  public FlockChild()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    this.FindRequiredComponents();
    this.Wander(0.0f);
    this.SetRandomScale();
    this._thisT.set_position(this.findWaypoint());
    this.RandomizeStartAnimationFrame();
    this.InitAvoidanceValues();
    this._speed = this._spawner._minSpeed;
    ++this._spawner._activeChildren;
    this._instantiated = true;
    if (this._spawner._updateDivisor <= 1)
      return;
    int num = this._spawner._updateDivisor - 1;
    ++FlockChild._updateNextSeed;
    this._updateSeed = FlockChild._updateNextSeed;
    FlockChild._updateNextSeed %= num;
  }

  public void Update()
  {
    if (this._spawner._updateDivisor > 1 && this._spawner._updateCounter != this._updateSeed)
      return;
    this.SoarTimeLimit();
    this.CheckForDistanceToWaypoint();
    this.RotationBasedOnWaypointOrAvoidance();
    this.LimitRotationOfModel();
  }

  public void OnDisable()
  {
    this.CancelInvoke();
    --this._spawner._activeChildren;
  }

  public void OnEnable()
  {
    if (!this._instantiated)
      return;
    ++this._spawner._activeChildren;
    if (this._landing)
      ((Animation) this._model.GetComponent<Animation>()).Play(this._spawner._idleAnimation);
    else
      ((Animation) this._model.GetComponent<Animation>()).Play(this._spawner._flapAnimation);
  }

  public void FindRequiredComponents()
  {
    if (Object.op_Equality((Object) this._thisT, (Object) null))
      this._thisT = ((Component) this).get_transform();
    if (Object.op_Equality((Object) this._model, (Object) null))
      this._model = ((Component) this._thisT.Find("Model")).get_gameObject();
    if (!Object.op_Equality((Object) this._modelT, (Object) null))
      return;
    this._modelT = this._model.get_transform();
  }

  public void RandomizeStartAnimationFrame()
  {
    IEnumerator enumerator = ((Animation) this._model.GetComponent<Animation>()).GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        AnimationState current = (AnimationState) enumerator.Current;
        current.set_time(Random.get_value() * current.get_length());
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void InitAvoidanceValues()
  {
    this._avoidValue = Random.Range(0.3f, 0.1f);
    if ((double) this._spawner._birdAvoidDistanceMax != (double) this._spawner._birdAvoidDistanceMin)
      this._avoidDistance = Random.Range(this._spawner._birdAvoidDistanceMax, this._spawner._birdAvoidDistanceMin);
    else
      this._avoidDistance = this._spawner._birdAvoidDistanceMin;
  }

  public void SetRandomScale()
  {
    float num = Random.Range(this._spawner._minScale, this._spawner._maxScale);
    this._thisT.set_localScale(new Vector3(num, num, num));
  }

  public void SoarTimeLimit()
  {
    if (!this._soar || (double) this._spawner._soarMaxTime <= 0.0)
      return;
    if ((double) this._soarTimer > (double) this._spawner._soarMaxTime)
    {
      this.Flap();
      this._soarTimer = 0.0f;
    }
    else
      this._soarTimer += this._spawner._newDelta;
  }

  public void CheckForDistanceToWaypoint()
  {
    if (!this._landing)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this._thisT.get_position(), this._wayPoint);
      if ((double) ((Vector3) ref vector3).get_magnitude() < (double) this._spawner._waypointDistance + (double) this._stuckCounter)
      {
        this.Wander(0.0f);
        this._stuckCounter = 0.0f;
        return;
      }
    }
    if (!this._landing)
      this._stuckCounter += this._spawner._newDelta;
    else
      this._stuckCounter = 0.0f;
  }

  public void RotationBasedOnWaypointOrAvoidance()
  {
    Vector3 vector3_1 = Vector3.op_Subtraction(this._wayPoint, this._thisT.get_position());
    if ((double) this._targetSpeed > -1.0 && Vector3.op_Inequality(vector3_1, Vector3.get_zero()))
      this._thisT.set_rotation(Quaternion.Slerp(this._thisT.get_rotation(), Quaternion.LookRotation(vector3_1), this._spawner._newDelta * this._damping));
    if (this._spawner._childTriggerPos)
    {
      Vector3 vector3_2 = Vector3.op_Subtraction(this._thisT.get_position(), this._spawner._posBuffer);
      if ((double) ((Vector3) ref vector3_2).get_magnitude() < 1.0)
        this._spawner.SetFlockRandomPosition();
    }
    this._speed = Mathf.Lerp(this._speed, this._targetSpeed, this._spawner._newDelta * 2.5f);
    if (!this._move)
      return;
    Transform thisT = this._thisT;
    thisT.set_position(Vector3.op_Addition(thisT.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(this._thisT.get_forward(), this._speed), this._spawner._newDelta)));
    if (!this._avoid || !this._spawner._birdAvoid)
      return;
    this.Avoidance();
  }

  public bool Avoidance()
  {
    RaycastHit raycastHit = (RaycastHit) null;
    Vector3 forward = this._modelT.get_forward();
    bool flag = false;
    Quaternion.get_identity();
    Vector3.get_zero();
    Vector3.get_zero();
    Vector3 position = this._thisT.get_position();
    Quaternion rotation1 = this._thisT.get_rotation();
    Quaternion rotation2 = this._thisT.get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation2).get_eulerAngles();
    if (Physics.Raycast(this._thisT.get_position(), Vector3.op_Addition(forward, Vector3.op_Multiply(this._modelT.get_right(), this._avoidValue)), ref raycastHit, this._avoidDistance, LayerMask.op_Implicit(this._spawner._avoidanceMask)))
    {
      ref Vector3 local = ref eulerAngles;
      local.y = (__Null) (local.y - (double) this._spawner._birdAvoidHorizontalForce * (double) this._spawner._newDelta * (double) this._damping);
      ((Quaternion) ref rotation1).set_eulerAngles(eulerAngles);
      this._thisT.set_rotation(rotation1);
      flag = true;
    }
    else if (Physics.Raycast(this._thisT.get_position(), Vector3.op_Addition(forward, Vector3.op_Multiply(this._modelT.get_right(), -this._avoidValue)), ref raycastHit, this._avoidDistance, LayerMask.op_Implicit(this._spawner._avoidanceMask)))
    {
      ref Vector3 local = ref eulerAngles;
      local.y = (__Null) (local.y + (double) this._spawner._birdAvoidHorizontalForce * (double) this._spawner._newDelta * (double) this._damping);
      ((Quaternion) ref rotation1).set_eulerAngles(eulerAngles);
      this._thisT.set_rotation(rotation1);
      flag = true;
    }
    if (this._spawner._birdAvoidDown && !this._landing && Physics.Raycast(this._thisT.get_position(), Vector3.op_UnaryNegation(Vector3.get_up()), ref raycastHit, this._avoidDistance, LayerMask.op_Implicit(this._spawner._avoidanceMask)))
    {
      ref Vector3 local1 = ref eulerAngles;
      local1.x = (__Null) (local1.x - (double) this._spawner._birdAvoidVerticalForce * (double) this._spawner._newDelta * (double) this._damping);
      ((Quaternion) ref rotation1).set_eulerAngles(eulerAngles);
      this._thisT.set_rotation(rotation1);
      ref Vector3 local2 = ref position;
      local2.y = (__Null) (local2.y + (double) this._spawner._birdAvoidVerticalForce * (double) this._spawner._newDelta * 0.00999999977648258);
      this._thisT.set_position(position);
      flag = true;
    }
    else if (this._spawner._birdAvoidUp && !this._landing && Physics.Raycast(this._thisT.get_position(), Vector3.get_up(), ref raycastHit, this._avoidDistance, LayerMask.op_Implicit(this._spawner._avoidanceMask)))
    {
      ref Vector3 local1 = ref eulerAngles;
      local1.x = (__Null) (local1.x + (double) this._spawner._birdAvoidVerticalForce * (double) this._spawner._newDelta * (double) this._damping);
      ((Quaternion) ref rotation1).set_eulerAngles(eulerAngles);
      this._thisT.set_rotation(rotation1);
      ref Vector3 local2 = ref position;
      local2.y = (__Null) (local2.y - (double) this._spawner._birdAvoidVerticalForce * (double) this._spawner._newDelta * 0.00999999977648258);
      this._thisT.set_position(position);
      flag = true;
    }
    return flag;
  }

  public void LimitRotationOfModel()
  {
    Quaternion.get_identity();
    Vector3.get_zero();
    Quaternion localRotation = this._modelT.get_localRotation();
    Vector3 eulerAngles = ((Quaternion) ref localRotation).get_eulerAngles();
    if ((this._soar && this._spawner._flatSoar || this._spawner._flatFly && !this._soar) && this._wayPoint.y > this._thisT.get_position().y || this._landing)
    {
      eulerAngles.x = (__Null) (double) Mathf.LerpAngle((float) this._modelT.get_localEulerAngles().x, (float) -this._thisT.get_localEulerAngles().x, this._spawner._newDelta * 1.75f);
      ((Quaternion) ref localRotation).set_eulerAngles(eulerAngles);
      this._modelT.set_localRotation(localRotation);
    }
    else
    {
      eulerAngles.x = (__Null) (double) Mathf.LerpAngle((float) this._modelT.get_localEulerAngles().x, 0.0f, this._spawner._newDelta * 1.75f);
      ((Quaternion) ref localRotation).set_eulerAngles(eulerAngles);
      this._modelT.set_localRotation(localRotation);
    }
  }

  public void Wander(float delay)
  {
    if (this._landing)
      return;
    this._damping = Random.Range(this._spawner._minDamping, this._spawner._maxDamping);
    this._targetSpeed = Random.Range(this._spawner._minSpeed, this._spawner._maxSpeed);
    this.Invoke("SetRandomMode", delay);
  }

  public void SetRandomMode()
  {
    this.CancelInvoke(nameof (SetRandomMode));
    if (!this._dived && (double) Random.get_value() < (double) this._spawner._soarFrequency)
      this.Soar();
    else if (!this._dived && (double) Random.get_value() < (double) this._spawner._diveFrequency)
      this.Dive();
    else
      this.Flap();
  }

  public void Flap()
  {
    if (!this._move)
      return;
    if (Object.op_Inequality((Object) this._model, (Object) null))
      ((Animation) this._model.GetComponent<Animation>()).CrossFade(this._spawner._flapAnimation, 0.5f);
    this._soar = false;
    this.animationSpeed();
    this._wayPoint = this.findWaypoint();
    this._dived = false;
  }

  public Vector3 findWaypoint()
  {
    Vector3 zero = Vector3.get_zero();
    zero.x = (__Null) ((double) Random.Range(-this._spawner._spawnSphere, this._spawner._spawnSphere) + this._spawner._posBuffer.x);
    zero.z = (__Null) ((double) Random.Range(-this._spawner._spawnSphereDepth, this._spawner._spawnSphereDepth) + this._spawner._posBuffer.z);
    zero.y = (__Null) ((double) Random.Range(-this._spawner._spawnSphereHeight, this._spawner._spawnSphereHeight) + this._spawner._posBuffer.y);
    return zero;
  }

  public void Soar()
  {
    if (!this._move)
      return;
    ((Animation) this._model.GetComponent<Animation>()).CrossFade(this._spawner._soarAnimation, 1.5f);
    this._wayPoint = this.findWaypoint();
    this._soar = true;
  }

  public void Dive()
  {
    if (this._spawner._soarAnimation != null)
    {
      ((Animation) this._model.GetComponent<Animation>()).CrossFade(this._spawner._soarAnimation, 1.5f);
    }
    else
    {
      IEnumerator enumerator = ((Animation) this._model.GetComponent<Animation>()).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          AnimationState current = (AnimationState) enumerator.Current;
          if (this._thisT.get_position().y < this._wayPoint.y + 25.0)
            current.set_speed(0.1f);
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
    this._wayPoint = this.findWaypoint();
    ref Vector3 local = ref this._wayPoint;
    local.y = (__Null) (local.y - (double) this._spawner._diveValue);
    this._dived = true;
  }

  public void animationSpeed()
  {
    IEnumerator enumerator = ((Animation) this._model.GetComponent<Animation>()).GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        AnimationState current = (AnimationState) enumerator.Current;
        if (!this._dived && !this._landing)
          current.set_speed(Random.Range(this._spawner._minAnimationSpeed, this._spawner._maxAnimationSpeed));
        else
          current.set_speed(this._spawner._maxAnimationSpeed);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }
}
