// Decompiled with JetBrains decompiler
// Type: DynamicBonePlaneCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Plane Collider")]
public class DynamicBonePlaneCollider : DynamicBoneColliderBase
{
  private void OnValidate()
  {
  }

  public override void Collide(ref Vector3 particlePosition, float particleRadius)
  {
    Vector3 vector3_1 = Vector3.get_up();
    switch (this.m_Direction)
    {
      case DynamicBoneColliderBase.Direction.X:
        vector3_1 = ((Component) this).get_transform().get_right();
        break;
      case DynamicBoneColliderBase.Direction.Y:
        vector3_1 = ((Component) this).get_transform().get_up();
        break;
      case DynamicBoneColliderBase.Direction.Z:
        vector3_1 = ((Component) this).get_transform().get_forward();
        break;
    }
    Vector3 vector3_2 = ((Component) this).get_transform().TransformPoint(this.m_Center);
    Plane plane;
    ((Plane) ref plane).\u002Ector(vector3_1, vector3_2);
    float distanceToPoint = ((Plane) ref plane).GetDistanceToPoint(particlePosition);
    if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
    {
      if ((double) distanceToPoint >= 0.0)
        return;
      particlePosition = Vector3.op_Subtraction(particlePosition, Vector3.op_Multiply(vector3_1, distanceToPoint));
    }
    else
    {
      if ((double) distanceToPoint <= 0.0)
        return;
      particlePosition = Vector3.op_Subtraction(particlePosition, Vector3.op_Multiply(vector3_1, distanceToPoint));
    }
  }

  private void OnDrawGizmosSelected()
  {
    if (!((Behaviour) this).get_enabled())
      return;
    if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
      Gizmos.set_color(Color.get_yellow());
    else
      Gizmos.set_color(Color.get_magenta());
    Vector3 vector3_1 = Vector3.get_up();
    switch (this.m_Direction)
    {
      case DynamicBoneColliderBase.Direction.X:
        vector3_1 = ((Component) this).get_transform().get_right();
        break;
      case DynamicBoneColliderBase.Direction.Y:
        vector3_1 = ((Component) this).get_transform().get_up();
        break;
      case DynamicBoneColliderBase.Direction.Z:
        vector3_1 = ((Component) this).get_transform().get_forward();
        break;
    }
    Vector3 vector3_2 = ((Component) this).get_transform().TransformPoint(this.m_Center);
    Gizmos.DrawLine(vector3_2, Vector3.op_Addition(vector3_2, vector3_1));
  }
}
