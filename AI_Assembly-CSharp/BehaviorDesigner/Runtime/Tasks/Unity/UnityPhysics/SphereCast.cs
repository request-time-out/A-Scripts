// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics.SphereCast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics
{
  [TaskCategory("Unity/Physics")]
  [TaskDescription("Casts a sphere against all colliders in the scene. Returns success if a collider was hit.")]
  public class SphereCast : Action
  {
    [Tooltip("Starts the spherecast at the GameObject's position. If null the originPosition will be used")]
    public SharedGameObject originGameObject;
    [Tooltip("Starts the sherecast at the position. Only used if originGameObject is null")]
    public SharedVector3 originPosition;
    [Tooltip("The radius of the spherecast")]
    public SharedFloat radius;
    [Tooltip("The direction of the spherecast")]
    public SharedVector3 direction;
    [Tooltip("The length of the spherecast. Set to -1 for infinity")]
    public SharedFloat distance;
    [Tooltip("Selectively ignore colliders")]
    public LayerMask layerMask;
    [Tooltip("Use world or local space. The direction is in world space if no GameObject is specified")]
    public Space space;
    [SharedRequired]
    [Tooltip("Stores the hit object of the spherecast")]
    public SharedGameObject storeHitObject;
    [SharedRequired]
    [Tooltip("Stores the hit point of the spherecast")]
    public SharedVector3 storeHitPoint;
    [SharedRequired]
    [Tooltip("Stores the hit normal of the spherecast")]
    public SharedVector3 storeHitNormal;
    [SharedRequired]
    [Tooltip("Stores the hit distance of the spherecast")]
    public SharedFloat storeHitDistance;

    public SphereCast()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector3 vector3 = this.direction.get_Value();
      Vector3 position;
      if (Object.op_Inequality((Object) this.originGameObject.get_Value(), (Object) null))
      {
        position = this.originGameObject.get_Value().get_transform().get_position();
        if (this.space == 1)
          vector3 = this.originGameObject.get_Value().get_transform().TransformDirection(this.direction.get_Value());
      }
      else
        position = this.originPosition.get_Value();
      RaycastHit raycastHit;
      if (!Physics.SphereCast(position, this.radius.get_Value(), vector3, ref raycastHit, (double) this.distance.get_Value() != -1.0 ? this.distance.get_Value() : float.PositiveInfinity, LayerMask.op_Implicit(this.layerMask)))
        return (TaskStatus) 1;
      this.storeHitObject.set_Value(((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject());
      this.storeHitPoint.set_Value(((RaycastHit) ref raycastHit).get_point());
      this.storeHitNormal.set_Value(((RaycastHit) ref raycastHit).get_normal());
      this.storeHitDistance.set_Value(((RaycastHit) ref raycastHit).get_distance());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.originGameObject = (SharedGameObject) null;
      this.originPosition = (SharedVector3) Vector3.get_zero();
      this.radius = (SharedFloat) 0.0f;
      this.direction = (SharedVector3) Vector3.get_zero();
      this.distance = (SharedFloat) -1f;
      this.layerMask = LayerMask.op_Implicit(-1);
      this.space = (Space) 1;
    }
  }
}
