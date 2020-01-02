// Decompiled with JetBrains decompiler
// Type: AIProject.UnityExtensions.PhysicsUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

namespace AIProject.UnityExtensions
{
  public static class PhysicsUtility
  {
    public static bool CheckPointInPolygon(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(b, a);
      Vector3 vector3_2 = Vector3.op_Subtraction(p, b);
      Vector3 vector3_3 = Vector3.op_Subtraction(c, b);
      Vector3 vector3_4 = Vector3.op_Subtraction(p, c);
      Vector3 vector3_5 = Vector3.op_Subtraction(a, c);
      Vector3 vector3_6 = Vector3.op_Subtraction(p, a);
      Vector3 vector3_7 = Vector3.Cross(vector3_1, vector3_2);
      Vector3 vector3_8 = Vector3.Cross(vector3_3, vector3_4);
      Vector3 vector3_9 = Vector3.Cross(vector3_5, vector3_6);
      float num1 = Vector3.Dot(vector3_7, vector3_8);
      float num2 = Vector3.Dot(vector3_7, vector3_9);
      return (double) num1 > 0.0 && (double) num2 > 0.0;
    }

    public static bool CheckFOV(
      LayerMask layer,
      Vector2 angle,
      Transform transform,
      Collider target,
      float viewDistance)
    {
      if (UnityEx.Misc.UnityExtensions.Contains(layer, ((Component) target).get_gameObject().get_layer()))
        return false;
      Vector3 vector3 = Vector3.get_zero();
      float num = 0.0f;
      switch (target)
      {
        case SphereCollider _:
        case CapsuleCollider _:
          switch (target)
          {
            case SphereCollider _:
              vector3 = (target as SphereCollider).get_center();
              num = (target as SphereCollider).get_radius();
              break;
            case CapsuleCollider _:
              vector3 = (target as CapsuleCollider).get_center();
              num = (target as CapsuleCollider).get_radius();
              break;
          }
          return PhysicsUtility.CheckInsideFOV(angle, transform, Vector3.op_Addition(((Component) target).get_transform().get_position(), Quaternion.op_Multiply(((Component) target).get_transform().get_rotation(), vector3)), viewDistance + num);
        default:
          return false;
      }
    }

    public static bool CheckFOVPoint(
      LayerMask layer,
      Vector2 angle,
      Transform transform,
      Transform target,
      float viewDistance)
    {
      return !UnityEx.Misc.UnityExtensions.Contains(layer, ((Component) target).get_gameObject().get_layer()) && PhysicsUtility.CheckInsideFOV(angle, transform, target, viewDistance);
    }

    public static bool CheckInsideFOV(
      Vector2 angle,
      Transform transform,
      Transform target,
      float viewDistance)
    {
      return PhysicsUtility.CheckInsideFOV(angle, transform, target.get_position(), viewDistance);
    }

    public static bool CheckInsideFOV(
      Vector2 angle,
      Transform transform,
      Vector3 targetPosition,
      float viewDistance)
    {
      if ((double) Vector3.Distance(transform.get_position(), targetPosition) > (double) viewDistance)
        return false;
      Vector2 vector2 = Vector2.op_Division(angle, 2f);
      Vector3 vector3_1 = Vector3.op_Subtraction(targetPosition, transform.get_position());
      Vector3 vector3_2 = Vector3.Normalize(new Vector3((float) vector3_1.x, 0.0f, (float) vector3_1.y));
      float num = Vector3.Angle(transform.get_forward(), vector3_2);
      if ((double) num > 180.0)
        num = Mathf.Abs(360f - num);
      return (double) num <= vector2.x;
    }

    [Conditional("UNITY_EDITOR")]
    public static void DrawWireFOV(
      float hAngle,
      float vAngle,
      Vector3 position,
      Quaternion rotation,
      float viewDistance)
    {
    }
  }
}
