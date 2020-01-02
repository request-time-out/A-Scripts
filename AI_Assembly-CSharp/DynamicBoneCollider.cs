// Decompiled with JetBrains decompiler
// Type: DynamicBoneCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : DynamicBoneColliderBase
{
  public float m_Radius = 0.5f;
  public float m_Height;

  private void OnValidate()
  {
    this.m_Radius = Mathf.Max(this.m_Radius, 0.0f);
    this.m_Height = Mathf.Max(this.m_Height, 0.0f);
  }

  public override void Collide(ref Vector3 particlePosition, float particleRadius)
  {
    float num1 = this.m_Radius * Mathf.Abs((float) ((Component) this).get_transform().get_lossyScale().x);
    float num2 = this.m_Height * 0.5f - this.m_Radius;
    if ((double) num2 <= 0.0)
    {
      if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
        DynamicBoneCollider.OutsideSphere(ref particlePosition, particleRadius, ((Component) this).get_transform().TransformPoint(this.m_Center), num1);
      else
        DynamicBoneCollider.InsideSphere(ref particlePosition, particleRadius, ((Component) this).get_transform().TransformPoint(this.m_Center), num1);
    }
    else
    {
      Vector3 center1 = this.m_Center;
      Vector3 center2 = this.m_Center;
      switch (this.m_Direction)
      {
        case DynamicBoneColliderBase.Direction.X:
          ref Vector3 local1 = ref center1;
          local1.x = (__Null) (local1.x - (double) num2);
          ref Vector3 local2 = ref center2;
          local2.x = (__Null) (local2.x + (double) num2);
          break;
        case DynamicBoneColliderBase.Direction.Y:
          ref Vector3 local3 = ref center1;
          local3.y = (__Null) (local3.y - (double) num2);
          ref Vector3 local4 = ref center2;
          local4.y = (__Null) (local4.y + (double) num2);
          break;
        case DynamicBoneColliderBase.Direction.Z:
          ref Vector3 local5 = ref center1;
          local5.z = (__Null) (local5.z - (double) num2);
          ref Vector3 local6 = ref center2;
          local6.z = (__Null) (local6.z + (double) num2);
          break;
      }
      if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
        DynamicBoneCollider.OutsideCapsule(ref particlePosition, particleRadius, ((Component) this).get_transform().TransformPoint(center1), ((Component) this).get_transform().TransformPoint(center2), num1);
      else
        DynamicBoneCollider.InsideCapsule(ref particlePosition, particleRadius, ((Component) this).get_transform().TransformPoint(center1), ((Component) this).get_transform().TransformPoint(center2), num1);
    }
  }

  private static void OutsideSphere(
    ref Vector3 particlePosition,
    float particleRadius,
    Vector3 sphereCenter,
    float sphereRadius)
  {
    float num1 = sphereRadius + particleRadius;
    float num2 = num1 * num1;
    Vector3 vector3 = Vector3.op_Subtraction(particlePosition, sphereCenter);
    float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
    if ((double) sqrMagnitude <= 0.0 || (double) sqrMagnitude >= (double) num2)
      return;
    float num3 = Mathf.Sqrt(sqrMagnitude);
    particlePosition = Vector3.op_Addition(sphereCenter, Vector3.op_Multiply(vector3, num1 / num3));
  }

  private static void InsideSphere(
    ref Vector3 particlePosition,
    float particleRadius,
    Vector3 sphereCenter,
    float sphereRadius)
  {
    float num1 = sphereRadius - particleRadius;
    float num2 = num1 * num1;
    Vector3 vector3 = Vector3.op_Subtraction(particlePosition, sphereCenter);
    float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
    if ((double) sqrMagnitude <= (double) num2)
      return;
    float num3 = Mathf.Sqrt(sqrMagnitude);
    particlePosition = Vector3.op_Addition(sphereCenter, Vector3.op_Multiply(vector3, num1 / num3));
  }

  private static void OutsideCapsule(
    ref Vector3 particlePosition,
    float particleRadius,
    Vector3 capsuleP0,
    Vector3 capsuleP1,
    float capsuleRadius)
  {
    float num1 = capsuleRadius + particleRadius;
    float num2 = num1 * num1;
    Vector3 vector3_1 = Vector3.op_Subtraction(capsuleP1, capsuleP0);
    Vector3 vector3_2 = Vector3.op_Subtraction(particlePosition, capsuleP0);
    float num3 = Vector3.Dot(vector3_2, vector3_1);
    if ((double) num3 <= 0.0)
    {
      float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
      if ((double) sqrMagnitude <= 0.0 || (double) sqrMagnitude >= (double) num2)
        return;
      float num4 = Mathf.Sqrt(sqrMagnitude);
      particlePosition = Vector3.op_Addition(capsuleP0, Vector3.op_Multiply(vector3_2, num1 / num4));
    }
    else
    {
      float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
      if ((double) num3 >= (double) sqrMagnitude1)
      {
        Vector3 vector3_3 = Vector3.op_Subtraction(particlePosition, capsuleP1);
        float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
        if ((double) sqrMagnitude2 <= 0.0 || (double) sqrMagnitude2 >= (double) num2)
          return;
        float num4 = Mathf.Sqrt(sqrMagnitude2);
        particlePosition = Vector3.op_Addition(capsuleP1, Vector3.op_Multiply(vector3_3, num1 / num4));
      }
      else
      {
        if ((double) sqrMagnitude1 <= 0.0)
          return;
        float num4 = num3 / sqrMagnitude1;
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, Vector3.op_Multiply(vector3_1, num4));
        float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
        if ((double) sqrMagnitude2 <= 0.0 || (double) sqrMagnitude2 >= (double) num2)
          return;
        float num5 = Mathf.Sqrt(sqrMagnitude2);
        particlePosition = Vector3.op_Addition(particlePosition, Vector3.op_Multiply(vector3_3, (num1 - num5) / num5));
      }
    }
  }

  private static void InsideCapsule(
    ref Vector3 particlePosition,
    float particleRadius,
    Vector3 capsuleP0,
    Vector3 capsuleP1,
    float capsuleRadius)
  {
    float num1 = capsuleRadius - particleRadius;
    float num2 = num1 * num1;
    Vector3 vector3_1 = Vector3.op_Subtraction(capsuleP1, capsuleP0);
    Vector3 vector3_2 = Vector3.op_Subtraction(particlePosition, capsuleP0);
    float num3 = Vector3.Dot(vector3_2, vector3_1);
    if ((double) num3 <= 0.0)
    {
      float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
      if ((double) sqrMagnitude <= (double) num2)
        return;
      float num4 = Mathf.Sqrt(sqrMagnitude);
      particlePosition = Vector3.op_Addition(capsuleP0, Vector3.op_Multiply(vector3_2, num1 / num4));
    }
    else
    {
      float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
      if ((double) num3 >= (double) sqrMagnitude1)
      {
        Vector3 vector3_3 = Vector3.op_Subtraction(particlePosition, capsuleP1);
        float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
        if ((double) sqrMagnitude2 <= (double) num2)
          return;
        float num4 = Mathf.Sqrt(sqrMagnitude2);
        particlePosition = Vector3.op_Addition(capsuleP1, Vector3.op_Multiply(vector3_3, num1 / num4));
      }
      else
      {
        if ((double) sqrMagnitude1 <= 0.0)
          return;
        float num4 = num3 / sqrMagnitude1;
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, Vector3.op_Multiply(vector3_1, num4));
        float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
        if ((double) sqrMagnitude2 <= (double) num2)
          return;
        float num5 = Mathf.Sqrt(sqrMagnitude2);
        particlePosition = Vector3.op_Addition(particlePosition, Vector3.op_Multiply(vector3_3, (num1 - num5) / num5));
      }
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
    float num1 = this.m_Radius * Mathf.Abs((float) ((Component) this).get_transform().get_lossyScale().x);
    float num2 = this.m_Height * 0.5f - this.m_Radius;
    if ((double) num2 <= 0.0)
    {
      Gizmos.DrawWireSphere(((Component) this).get_transform().TransformPoint(this.m_Center), num1);
    }
    else
    {
      Vector3 center1 = this.m_Center;
      Vector3 center2 = this.m_Center;
      switch (this.m_Direction)
      {
        case DynamicBoneColliderBase.Direction.X:
          ref Vector3 local1 = ref center1;
          local1.x = (__Null) (local1.x - (double) num2);
          ref Vector3 local2 = ref center2;
          local2.x = (__Null) (local2.x + (double) num2);
          break;
        case DynamicBoneColliderBase.Direction.Y:
          ref Vector3 local3 = ref center1;
          local3.y = (__Null) (local3.y - (double) num2);
          ref Vector3 local4 = ref center2;
          local4.y = (__Null) (local4.y + (double) num2);
          break;
        case DynamicBoneColliderBase.Direction.Z:
          ref Vector3 local5 = ref center1;
          local5.z = (__Null) (local5.z - (double) num2);
          ref Vector3 local6 = ref center2;
          local6.z = (__Null) (local6.z + (double) num2);
          break;
      }
      Gizmos.DrawWireSphere(((Component) this).get_transform().TransformPoint(center1), num1);
      Gizmos.DrawWireSphere(((Component) this).get_transform().TransformPoint(center2), num1);
    }
  }
}
