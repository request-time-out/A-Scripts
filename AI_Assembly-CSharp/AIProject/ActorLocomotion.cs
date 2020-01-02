// Decompiled with JetBrains decompiler
// Type: AIProject.ActorLocomotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (Rigidbody))]
  public abstract class ActorLocomotion : MonoBehaviour
  {
    [SerializeField]
    protected float _airbornThreshold;
    [SerializeField]
    private float _slopeStartAngle;
    [SerializeField]
    private float _slopeEndAngle;
    [SerializeField]
    private float _spherecastRadius;
    [SerializeField]
    private LayerMask _groundLayers;
    [SerializeField]
    protected Actor _actor;
    [SerializeField]
    protected float _slopeLimit;
    protected const float _half = 0.5f;
    protected float _originalHeight;
    protected Vector3 _originalCenter;

    protected ActorLocomotion()
    {
      base.\u002Ector();
    }

    public abstract void Move(Vector3 deltaPosition);

    protected virtual void Start()
    {
      RaycastHit raycastHit;
      Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.get_up()), Vector3.get_down(), ref raycastHit, (float) LayerMask.op_Implicit(this._groundLayers));
      ((Component) this).get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.get_up()));
    }

    protected virtual RaycastHit GetSpherecastHit()
    {
      Ray ray;
      ((Ray) ref ray).\u002Ector(Vector3.op_Addition(((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).get_position(), Vector3.op_Multiply(Vector3.get_up(), this._airbornThreshold)), Vector3.get_down());
      RaycastHit raycastHit;
      Physics.SphereCast(ray, this._spherecastRadius, ref raycastHit, this._airbornThreshold * 2f, LayerMask.op_Implicit(this._groundLayers));
      return raycastHit;
    }

    public float GetAngleFromForward(Vector3 worldDirection)
    {
      Vector3 vector3 = ((Component) this).get_transform().InverseTransformDirection(worldDirection);
      return Mathf.Atan2((float) vector3.x, (float) vector3.z) * 57.29578f;
    }

    protected void RigidbodyRotateAround(Vector3 point, Vector3 axis, float angle)
    {
      this._actor.Rotation = Quaternion.op_Multiply(Quaternion.AngleAxis(angle, axis), this._actor.Rotation);
    }

    protected float GetSlopeDamper(Vector3 velocity, Vector3 groundNormal)
    {
      return 1f - Mathf.Clamp((90f - Vector3.Angle(velocity, groundNormal) - this._slopeStartAngle) / (this._slopeEndAngle - this._slopeStartAngle), 0.0f, 1f);
    }

    public struct AnimationState
    {
      public float medVelocity;
      public float maxVelocity;
      public bool setMediumOnWalk;
      public Vector3 moveDirection;
      public bool onGround;
      public float yVelocity;

      public void Init()
      {
        this.moveDirection = Vector3.get_zero();
        this.onGround = true;
        this.yVelocity = 0.0f;
      }
    }

    public enum UpdateType
    {
      Update,
      LateUpdate,
      FixedUpdate,
    }
  }
}
