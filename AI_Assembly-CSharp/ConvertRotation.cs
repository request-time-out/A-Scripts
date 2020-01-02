// Decompiled with JetBrains decompiler
// Type: ConvertRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ConvertRotation
{
  public static void CheckNaN(ref float val, float correct = 0.0f)
  {
    if (!float.IsNaN(val))
      return;
    Debug.LogWarning((object) "NaN");
    val = correct;
  }

  public static Quaternion ConvertDegreeToQuaternion(
    ConvertRotation.RotationOrder order,
    float x,
    float y,
    float z)
  {
    Quaternion quaternion1 = Quaternion.AngleAxis(x, Vector3.get_right());
    Quaternion quaternion2 = Quaternion.AngleAxis(y, Vector3.get_up());
    Quaternion quaternion3 = Quaternion.AngleAxis(z, Vector3.get_forward());
    switch (order)
    {
      case ConvertRotation.RotationOrder.xyz:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion1, quaternion2), quaternion3);
      case ConvertRotation.RotationOrder.xzy:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion1, quaternion3), quaternion2);
      case ConvertRotation.RotationOrder.yxz:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion2, quaternion1), quaternion3);
      case ConvertRotation.RotationOrder.yzx:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion2, quaternion3), quaternion1);
      case ConvertRotation.RotationOrder.zxy:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion3, quaternion1), quaternion2);
      case ConvertRotation.RotationOrder.zyx:
        return Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion3, quaternion2), quaternion1);
      default:
        return Quaternion.get_identity();
    }
  }

  public static Quaternion ConvertRadianToQuaternion(
    ConvertRotation.RotationOrder order,
    float x,
    float y,
    float z)
  {
    return ConvertRotation.ConvertDegreeToQuaternion(order, x * 57.29578f, y * 57.29578f, z * 57.29578f);
  }

  public static Vector3 ConvertDegreeFromQuaternion(
    ConvertRotation.RotationOrder order,
    Quaternion q)
  {
    Vector3 vector3 = ConvertRotation.ConvertRadianFromQuaternion(order, q);
    return new Vector3((float) (vector3.x * 57.2957801818848), (float) (vector3.y * 57.2957801818848), (float) (vector3.z * 57.2957801818848));
  }

  public static Vector3 ConvertRadianFromQuaternion(
    ConvertRotation.RotationOrder order,
    Quaternion q)
  {
    Matrix4x4 m = Matrix4x4.TRS(Vector3.get_zero(), q, Vector3.get_one());
    return ConvertRotation.ConvertRadianFromMatrix(order, m);
  }

  private static float[] threeaxisrot(float r11, float r12, float r21, float r31, float r32)
  {
    float[] numArray = new float[3];
    if ((double) Mathf.Abs(r21) > 0.999899983406067)
    {
      numArray[0] = Mathf.Atan2(r31, r32);
      numArray[1] = Mathf.Asin(Mathf.Clamp(r21, -1f, 1f));
      numArray[2] = Mathf.Atan2(r11, r12);
    }
    else
    {
      numArray[0] = Mathf.Atan2(r31, r32);
      numArray[1] = Mathf.Asin(Mathf.Clamp(r21, -1f, 1f));
      numArray[2] = Mathf.Atan2(r11, r12);
    }
    return numArray;
  }

  public static Vector3 ConvertDegreeFromQuaternionEx(
    ConvertRotation.RotationOrder order,
    Quaternion q)
  {
    Vector3 vector3 = ConvertRotation.ConvertRadianFromQuaternionEx(order, q);
    return new Vector3((float) (vector3.x * 57.2957801818848), (float) (vector3.y * 57.2957801818848), (float) (vector3.z * 57.2957801818848));
  }

  public static Vector3 ConvertRadianFromQuaternionEx(
    ConvertRotation.RotationOrder order,
    Quaternion q)
  {
    switch (order)
    {
      case ConvertRotation.RotationOrder.xyz:
        float[] numArray1 = ConvertRotation.threeaxisrot((float) (-2.0 * (q.y * q.z - q.w * q.x)), (float) (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z), (float) (2.0 * (q.x * q.z + q.w * q.y)), (float) (-2.0 * (q.x * q.y - q.w * q.z)), (float) (q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z));
        return new Vector3(numArray1[2], numArray1[1], numArray1[0]);
      case ConvertRotation.RotationOrder.xzy:
        float[] numArray2 = ConvertRotation.threeaxisrot((float) (2.0 * (q.y * q.z + q.w * q.x)), (float) (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z), (float) (-2.0 * (q.x * q.y - q.w * q.z)), (float) (2.0 * (q.x * q.z + q.w * q.y)), (float) (q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z));
        return new Vector3(numArray2[2], numArray2[0], numArray2[1]);
      case ConvertRotation.RotationOrder.yxz:
        float[] numArray3 = ConvertRotation.threeaxisrot((float) (2.0 * (q.x * q.z + q.w * q.y)), (float) (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z), (float) (-2.0 * (q.y * q.z - q.w * q.x)), (float) (2.0 * (q.x * q.y + q.w * q.z)), (float) (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z));
        return new Vector3(numArray3[1], numArray3[2], numArray3[0]);
      case ConvertRotation.RotationOrder.yzx:
        float[] numArray4 = ConvertRotation.threeaxisrot((float) (-2.0 * (q.x * q.z - q.w * q.y)), (float) (q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z), (float) (2.0 * (q.x * q.y + q.w * q.z)), (float) (-2.0 * (q.y * q.z - q.w * q.x)), (float) (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z));
        return new Vector3(numArray4[0], numArray4[2], numArray4[1]);
      case ConvertRotation.RotationOrder.zxy:
        float[] numArray5 = ConvertRotation.threeaxisrot((float) (-2.0 * (q.x * q.y - q.w * q.z)), (float) (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z), (float) (2.0 * (q.y * q.z + q.w * q.x)), (float) (-2.0 * (q.x * q.z - q.w * q.y)), (float) (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z));
        return new Vector3(numArray5[1], numArray5[0], numArray5[2]);
      case ConvertRotation.RotationOrder.zyx:
        float[] numArray6 = ConvertRotation.threeaxisrot((float) (2.0 * (q.x * q.y + q.w * q.z)), (float) (q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z), (float) (-2.0 * (q.x * q.z - q.w * q.y)), (float) (2.0 * (q.y * q.z + q.w * q.x)), (float) (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z));
        return new Vector3(numArray6[0], numArray6[1], numArray6[2]);
      default:
        return Vector3.get_zero();
    }
  }

  public static Vector3 ConvertDegreeFromMatrix(
    ConvertRotation.RotationOrder order,
    Matrix4x4 m)
  {
    Vector3 vector3 = ConvertRotation.ConvertRadianFromMatrix(order, m);
    return new Vector3((float) (vector3.x * 57.2957801818848), (float) (vector3.y * 57.2957801818848), (float) (vector3.z * 57.2957801818848));
  }

  public static Vector3 ConvertRadianFromMatrix(
    ConvertRotation.RotationOrder order,
    Matrix4x4 m)
  {
    switch (order)
    {
      case ConvertRotation.RotationOrder.xyz:
        float m02 = (float) m.m02;
        float num1 = Mathf.Asin(Mathf.Clamp(m02, -1f, 1f));
        float val1;
        float val2;
        if ((double) m02 > 0.999899983406067)
        {
          val1 = 0.0f;
          val2 = Mathf.Atan2((float) m.m21, (float) m.m11);
          ConvertRotation.CheckNaN(ref val2, 0.0f);
        }
        else
        {
          val2 = Mathf.Atan2((float) -m.m12, (float) m.m22);
          ConvertRotation.CheckNaN(ref val2, 0.0f);
          val1 = Mathf.Atan2((float) -m.m01, (float) m.m00);
          ConvertRotation.CheckNaN(ref val1, 0.0f);
        }
        return new Vector3(val2, num1, val1);
      case ConvertRotation.RotationOrder.xzy:
        float num2 = (float) -m.m01;
        float num3 = Mathf.Asin(Mathf.Clamp(num2, -1f, 1f));
        float val3;
        float val4;
        if ((double) num2 > 0.999899983406067)
        {
          val3 = 0.0f;
          val4 = Mathf.Atan2((float) -m.m12, (float) m.m22);
          ConvertRotation.CheckNaN(ref val4, 0.0f);
        }
        else
        {
          val4 = Mathf.Atan2((float) m.m21, (float) m.m11);
          ConvertRotation.CheckNaN(ref val4, 0.0f);
          val3 = Mathf.Atan2((float) m.m02, (float) m.m00);
          ConvertRotation.CheckNaN(ref val3, 0.0f);
        }
        return new Vector3(val4, val3, num3);
      case ConvertRotation.RotationOrder.yxz:
        float num4 = (float) -m.m12;
        float num5 = Mathf.Asin(Mathf.Clamp(num4, -1f, 1f));
        float val5;
        float val6;
        if ((double) num4 > 0.999899983406067)
        {
          val5 = 0.0f;
          val6 = Mathf.Atan2((float) -m.m20, (float) m.m00);
          ConvertRotation.CheckNaN(ref val6, 0.0f);
        }
        else
        {
          val6 = Mathf.Atan2((float) m.m02, (float) m.m22);
          ConvertRotation.CheckNaN(ref val6, 0.0f);
          val5 = Mathf.Atan2((float) m.m10, (float) m.m11);
          ConvertRotation.CheckNaN(ref val5, 0.0f);
        }
        return new Vector3(num5, val6, val5);
      case ConvertRotation.RotationOrder.yzx:
        float m10 = (float) m.m10;
        float num6 = Mathf.Asin(Mathf.Clamp(m10, -1f, 1f));
        float val7;
        float val8;
        if ((double) m10 > 0.999899983406067)
        {
          val7 = 0.0f;
          val8 = Mathf.Atan2((float) m.m02, (float) m.m22);
          ConvertRotation.CheckNaN(ref val8, 0.0f);
        }
        else
        {
          val7 = Mathf.Atan2((float) -m.m12, (float) m.m11);
          ConvertRotation.CheckNaN(ref val7, 0.0f);
          val8 = Mathf.Atan2((float) -m.m20, (float) m.m00);
          ConvertRotation.CheckNaN(ref val8, 0.0f);
        }
        return new Vector3(val7, val8, num6);
      case ConvertRotation.RotationOrder.zxy:
        float m21 = (float) m.m21;
        float num7 = Mathf.Asin(Mathf.Clamp(m21, -1f, 1f));
        float val9;
        float val10;
        if ((double) m21 > 0.999899983406067)
        {
          val9 = 0.0f;
          val10 = Mathf.Atan2((float) m.m10, (float) m.m00);
          ConvertRotation.CheckNaN(ref val10, 0.0f);
        }
        else
        {
          val9 = Mathf.Atan2((float) -m.m20, (float) m.m22);
          ConvertRotation.CheckNaN(ref val9, 0.0f);
          val10 = Mathf.Atan2((float) -m.m01, (float) m.m11);
          ConvertRotation.CheckNaN(ref val10, 0.0f);
        }
        return new Vector3(num7, val9, val10);
      case ConvertRotation.RotationOrder.zyx:
        float num8 = (float) -m.m20;
        float num9 = Mathf.Asin(Mathf.Clamp(num8, -1f, 1f));
        float val11;
        float val12;
        if ((double) num8 > 0.999899983406067)
        {
          val11 = 0.0f;
          val12 = Mathf.Atan2((float) -m.m01, (float) m.m11);
          ConvertRotation.CheckNaN(ref val12, 0.0f);
        }
        else
        {
          val11 = Mathf.Atan2((float) m.m21, (float) m.m22);
          ConvertRotation.CheckNaN(ref val11, 0.0f);
          val12 = Mathf.Atan2((float) m.m10, (float) m.m00);
          ConvertRotation.CheckNaN(ref val12, 0.0f);
        }
        return new Vector3(val11, num9, val12);
      default:
        return Vector3.get_zero();
    }
  }

  public enum RotationOrder
  {
    xyz,
    xzy,
    yxz,
    yzx,
    zxy,
    zyx,
  }
}
