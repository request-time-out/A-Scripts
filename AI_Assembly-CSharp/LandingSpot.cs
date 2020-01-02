// Decompiled with JetBrains decompiler
// Type: LandingSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LandingSpot : MonoBehaviour
{
  [HideInInspector]
  public FlockChild landingChild;
  [HideInInspector]
  public bool landing;
  private int lerpCounter;
  [HideInInspector]
  public LandingSpotController _controller;
  private bool _idle;
  public Transform _thisT;
  public bool _gotcha;

  public LandingSpot()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (Object.op_Equality((Object) this._thisT, (Object) null))
      this._thisT = ((Component) this).get_transform();
    if (Object.op_Equality((Object) this._controller, (Object) null))
      this._controller = (LandingSpotController) ((Component) this._thisT.get_parent()).GetComponent<LandingSpotController>();
    if (this._controller._autoCatchDelay.x <= 0.0)
      return;
    this.StartCoroutine(this.GetFlockChild((float) this._controller._autoCatchDelay.x, (float) this._controller._autoCatchDelay.y));
  }

  public void OnDrawGizmos()
  {
    if (Object.op_Equality((Object) this._thisT, (Object) null))
      this._thisT = ((Component) this).get_transform();
    if (Object.op_Equality((Object) this._controller, (Object) null))
      this._controller = (LandingSpotController) ((Component) this._thisT.get_parent()).GetComponent<LandingSpotController>();
    Gizmos.set_color(Color.get_yellow());
    if (Object.op_Inequality((Object) this.landingChild, (Object) null) && this.landing)
      Gizmos.DrawLine(this._thisT.get_position(), this.landingChild._thisT.get_position());
    Quaternion rotation1 = this._thisT.get_rotation();
    if (((Quaternion) ref rotation1).get_eulerAngles().x == 0.0)
    {
      Quaternion rotation2 = this._thisT.get_rotation();
      if (((Quaternion) ref rotation2).get_eulerAngles().z == 0.0)
        goto label_9;
    }
    this._thisT.set_eulerAngles(new Vector3(0.0f, (float) this._thisT.get_eulerAngles().y, 0.0f));
label_9:
    Gizmos.DrawCube(new Vector3((float) this._thisT.get_position().x, (float) this._thisT.get_position().y, (float) this._thisT.get_position().z), Vector3.op_Multiply(Vector3.get_one(), this._controller._gizmoSize));
    Gizmos.DrawCube(Vector3.op_Addition(this._thisT.get_position(), Vector3.op_Multiply(this._thisT.get_forward(), this._controller._gizmoSize)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this._controller._gizmoSize), 0.5f));
    Gizmos.set_color(new Color(1f, 1f, 0.0f, 0.05f));
    Gizmos.DrawWireSphere(this._thisT.get_position(), this._controller._maxBirdDistance);
  }

  public void LateUpdate()
  {
    if (Object.op_Equality((Object) this.landingChild, (Object) null))
    {
      this._gotcha = false;
      this._idle = false;
      this.lerpCounter = 0;
    }
    else if (this._gotcha)
    {
      ((Component) this.landingChild).get_transform().set_position(Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset));
      this.RotateBird();
    }
    else
    {
      if (((Component) this._controller._flock).get_gameObject().get_activeInHierarchy() && this.landing && Object.op_Inequality((Object) this.landingChild, (Object) null))
      {
        if (!((Component) this.landingChild).get_gameObject().get_activeInHierarchy())
          this.Invoke("ReleaseFlockChild", 0.0f);
        float num = Vector3.Distance(this.landingChild._thisT.get_position(), Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset));
        if ((double) num < 5.0 && (double) num > 0.5)
        {
          if (this._controller._soarLand)
          {
            ((Animation) this.landingChild._model.GetComponent<Animation>()).CrossFade(this.landingChild._spawner._soarAnimation, 0.5f);
            if ((double) num < 2.0)
              ((Animation) this.landingChild._model.GetComponent<Animation>()).CrossFade(this.landingChild._spawner._flapAnimation, 0.5f);
          }
          this.landingChild._targetSpeed = this.landingChild._spawner._maxSpeed * this._controller._landingSpeedModifier;
          this.landingChild._wayPoint = Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset);
          this.landingChild._damping = this._controller._landingTurnSpeedModifier;
          this.landingChild._avoid = false;
        }
        else if ((double) num <= 0.5)
        {
          this.landingChild._wayPoint = Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset);
          if ((double) num < (double) this._controller._snapLandDistance && !this._idle)
          {
            this._idle = true;
            ((Animation) this.landingChild._model.GetComponent<Animation>()).CrossFade(this.landingChild._spawner._idleAnimation, 0.55f);
          }
          if ((double) num > (double) this._controller._snapLandDistance)
          {
            this.landingChild._targetSpeed = this.landingChild._spawner._minSpeed * this._controller._landingSpeedModifier;
            Transform thisT = this.landingChild._thisT;
            thisT.set_position(Vector3.op_Addition(thisT.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset), this.landingChild._thisT.get_position()), Time.get_deltaTime()), this.landingChild._speed), this._controller._landingSpeedModifier), 2f)));
          }
          else
            this._gotcha = true;
          this.landingChild._move = false;
          this.RotateBird();
        }
        else
          this.landingChild._wayPoint = Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset);
        this.landingChild._damping += 0.01f;
      }
      this.StraightenBird();
    }
  }

  public void StraightenBird()
  {
    if (this.landingChild._thisT.get_eulerAngles().x == 0.0)
      return;
    Vector3 eulerAngles = this.landingChild._thisT.get_eulerAngles();
    eulerAngles.z = (__Null) 0.0;
    this.landingChild._thisT.set_eulerAngles(eulerAngles);
  }

  public void RotateBird()
  {
    if (this._controller._randomRotate && this._idle)
      return;
    ++this.lerpCounter;
    Quaternion rotation1 = this.landingChild._thisT.get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation1).get_eulerAngles();
    ref Vector3 local = ref eulerAngles;
    Quaternion rotation2 = this.landingChild._thisT.get_rotation();
    // ISSUE: variable of the null type
    __Null y1 = ((Quaternion) ref rotation2).get_eulerAngles().y;
    Quaternion rotation3 = this._thisT.get_rotation();
    // ISSUE: variable of the null type
    __Null y2 = ((Quaternion) ref rotation3).get_eulerAngles().y;
    double num1 = (double) this.lerpCounter * (double) Time.get_deltaTime() * (double) this._controller._landedRotateSpeed;
    double num2 = (double) Mathf.LerpAngle((float) y1, (float) y2, (float) num1);
    local.y = (__Null) num2;
    ((Quaternion) ref rotation1).set_eulerAngles(eulerAngles);
    this.landingChild._thisT.set_rotation(rotation1);
  }

  [DebuggerHidden]
  public IEnumerator GetFlockChild(float minDelay, float maxDelay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LandingSpot.\u003CGetFlockChild\u003Ec__Iterator0()
    {
      minDelay = minDelay,
      maxDelay = maxDelay,
      \u0024this = this
    };
  }

  public void InstantLand()
  {
    if (!((Component) this._controller._flock).get_gameObject().get_activeInHierarchy() || !Object.op_Equality((Object) this.landingChild, (Object) null))
      return;
    FlockChild flockChild = (FlockChild) null;
    for (int index = 0; index < this._controller._flock._roamers.Count; ++index)
    {
      FlockChild roamer = this._controller._flock._roamers[index];
      if (!roamer._landing && !roamer._dived)
        flockChild = roamer;
    }
    if (Object.op_Inequality((Object) flockChild, (Object) null))
    {
      this.landingChild = flockChild;
      this.landing = true;
      ++this._controller._activeLandingSpots;
      this.landingChild._landing = true;
      this.landingChild._thisT.set_position(Vector3.op_Addition(this._thisT.get_position(), this.landingChild._landingPosOffset));
      ((Animation) this.landingChild._model.GetComponent<Animation>()).Play(this.landingChild._spawner._idleAnimation);
      this.landingChild._thisT.Rotate(Vector3.get_up(), Random.Range(0.0f, 360f));
      if (this._controller._autoDismountDelay.x <= 0.0)
        return;
      this.Invoke("ReleaseFlockChild", Random.Range((float) this._controller._autoDismountDelay.x, (float) this._controller._autoDismountDelay.y));
    }
    else
    {
      if (this._controller._autoCatchDelay.x <= 0.0)
        return;
      this.StartCoroutine(this.GetFlockChild((float) this._controller._autoCatchDelay.x, (float) this._controller._autoCatchDelay.y));
    }
  }

  public void ReleaseFlockChild()
  {
    if (!((Component) this._controller._flock).get_gameObject().get_activeInHierarchy() || !Object.op_Inequality((Object) this.landingChild, (Object) null))
      return;
    this._gotcha = false;
    this.lerpCounter = 0;
    if (Object.op_Inequality((Object) this._controller._featherPS, (Object) null))
    {
      this._controller._featherPS.set_position(this.landingChild._thisT.get_position());
      ((ParticleSystem) ((Component) this._controller._featherPS).GetComponent<ParticleSystem>()).Emit(Random.Range(0, 3));
    }
    this.landing = false;
    this._idle = false;
    this.landingChild._avoid = true;
    this.landingChild._damping = this.landingChild._spawner._maxDamping;
    ((Animation) this.landingChild._model.GetComponent<Animation>()).CrossFade(this.landingChild._spawner._flapAnimation, 0.2f);
    this.landingChild._dived = true;
    this.landingChild._speed = 0.0f;
    this.landingChild._move = true;
    this.landingChild._landing = false;
    this.landingChild.Flap();
    this.landingChild._wayPoint = new Vector3((float) this.landingChild._wayPoint.x, (float) (this._thisT.get_position().y + 10.0), (float) this.landingChild._wayPoint.z);
    if (this._controller._autoCatchDelay.x > 0.0)
      this.StartCoroutine(this.GetFlockChild((float) (this._controller._autoCatchDelay.x + 0.100000001490116), (float) (this._controller._autoCatchDelay.y + 0.100000001490116)));
    this.landingChild = (FlockChild) null;
    --this._controller._activeLandingSpots;
  }
}
