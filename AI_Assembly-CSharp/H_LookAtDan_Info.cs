// Decompiled with JetBrains decompiler
// Type: H_LookAtDan_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class H_LookAtDan_Info
{
  public string lookAtName = string.Empty;
  public string targetName = string.Empty;
  public H_LookAtDan_Info.AxisType targetAxisType = H_LookAtDan_Info.AxisType.Z;
  public string upAxisName = string.Empty;
  public H_LookAtDan_Info.AxisType upAxisType = H_LookAtDan_Info.AxisType.Y;
  public H_LookAtDan_Info.AxisType sourceAxisType = H_LookAtDan_Info.AxisType.Y;
  public H_LookAtDan_Info.AxisType limitAxisType = H_LookAtDan_Info.AxisType.None;
  public H_LookAtDan_Info.RotationOrder rotOrder = H_LookAtDan_Info.RotationOrder.ZXY;
  private Quaternion oldRotation = Quaternion.get_identity();
  [Range(-180f, 180f)]
  public float limitMin;
  [Range(-180f, 180f)]
  public float limitMax;

  public H_LookAtDan_Info()
  {
    this.trfLookAt = (Transform) null;
    this.trfTarget = (Transform) null;
    this.trfUpAxis = (Transform) null;
  }

  public Transform trfLookAt { get; private set; }

  public Transform trfTarget { get; private set; }

  public Transform trfUpAxis { get; private set; }

  public void SetLookAtTransform(Transform trf)
  {
    this.trfLookAt = trf;
  }

  public void SetTargetTransform(Transform trf)
  {
    this.trfTarget = trf;
  }

  public void SetUpAxisTransform(Transform trf)
  {
    this.trfUpAxis = trf;
  }

  public void SetOldRotation(Quaternion q)
  {
    this.oldRotation = q;
  }

  public void ManualCalc()
  {
    if (Object.op_Equality((Object) null, (Object) this.trfTarget) || Object.op_Equality((Object) null, (Object) this.trfLookAt))
      return;
    Vector3 upVector = this.GetUpVector();
    Vector3 vector3_1 = Vector3.Normalize(Vector3.op_Subtraction(this.trfTarget.get_position(), this.trfLookAt.get_position()));
    Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(upVector, vector3_1));
    Vector3 vector3_3 = Vector3.Cross(vector3_1, vector3_2);
    if (this.targetAxisType == H_LookAtDan_Info.AxisType.RevX || this.targetAxisType == H_LookAtDan_Info.AxisType.RevY || this.targetAxisType == H_LookAtDan_Info.AxisType.RevZ)
    {
      vector3_1 = Vector3.op_UnaryNegation(vector3_1);
      vector3_2 = Vector3.op_UnaryNegation(vector3_2);
    }
    Vector3 xvec = Vector3.get_zero();
    Vector3 yvec = Vector3.get_zero();
    Vector3 zvec = Vector3.get_zero();
    switch (this.targetAxisType)
    {
      case H_LookAtDan_Info.AxisType.X:
      case H_LookAtDan_Info.AxisType.RevX:
        xvec = vector3_1;
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.Y)
        {
          yvec = vector3_3;
          zvec = Vector3.op_UnaryNegation(vector3_2);
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevY)
        {
          yvec = Vector3.op_UnaryNegation(vector3_3);
          zvec = vector3_2;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.Z)
        {
          yvec = vector3_2;
          zvec = vector3_3;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevZ)
        {
          yvec = Vector3.op_UnaryNegation(vector3_2);
          zvec = Vector3.op_UnaryNegation(vector3_3);
          break;
        }
        break;
      case H_LookAtDan_Info.AxisType.Y:
      case H_LookAtDan_Info.AxisType.RevY:
        yvec = vector3_1;
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.X)
        {
          xvec = vector3_3;
          zvec = vector3_2;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevX)
        {
          xvec = Vector3.op_UnaryNegation(vector3_3);
          zvec = Vector3.op_UnaryNegation(vector3_2);
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.Z)
        {
          xvec = Vector3.op_UnaryNegation(vector3_2);
          zvec = vector3_3;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevZ)
        {
          xvec = vector3_2;
          zvec = Vector3.op_UnaryNegation(vector3_3);
          break;
        }
        break;
      case H_LookAtDan_Info.AxisType.Z:
      case H_LookAtDan_Info.AxisType.RevZ:
        zvec = vector3_1;
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.X)
        {
          xvec = vector3_3;
          yvec = Vector3.op_UnaryNegation(vector3_2);
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevX)
        {
          xvec = Vector3.op_UnaryNegation(vector3_3);
          yvec = vector3_2;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.Y)
        {
          xvec = vector3_2;
          yvec = vector3_3;
          break;
        }
        if (this.sourceAxisType == H_LookAtDan_Info.AxisType.RevY)
        {
          xvec = Vector3.op_UnaryNegation(vector3_2);
          yvec = Vector3.op_UnaryNegation(vector3_3);
          break;
        }
        break;
    }
    if (this.limitAxisType == H_LookAtDan_Info.AxisType.None)
    {
      Quaternion q = (Quaternion) null;
      if (this.LookAtQuat(xvec, yvec, zvec, ref q))
        this.trfLookAt.set_rotation(q);
      else
        this.trfLookAt.set_rotation(this.oldRotation);
      this.oldRotation = this.trfLookAt.get_rotation();
    }
    else
    {
      Quaternion q1 = (Quaternion) null;
      if (this.LookAtQuat(xvec, yvec, zvec, ref q1))
        this.trfLookAt.set_rotation(q1);
      else
        this.trfLookAt.set_rotation(this.oldRotation);
      ConvertRotation.RotationOrder rotOrder = (ConvertRotation.RotationOrder) this.rotOrder;
      Quaternion localRotation = this.trfLookAt.get_localRotation();
      Vector3 vector3_4 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, localRotation);
      Quaternion q2 = Quaternion.Slerp(localRotation, Quaternion.get_identity(), 0.5f);
      Vector3 vector3_5 = ConvertRotation.ConvertDegreeFromQuaternion(rotOrder, q2);
      if (this.limitAxisType == H_LookAtDan_Info.AxisType.X)
      {
        if (vector3_4.x < 0.0 && vector3_5.x > 0.0 || vector3_4.x > 0.0 && vector3_5.x < 0.0)
        {
          ref Vector3 local = ref vector3_4;
          local.x = (__Null) (local.x * -1.0);
        }
        vector3_4.x = (__Null) (double) Mathf.Clamp((float) vector3_4.x, this.limitMin, this.limitMax);
      }
      else if (this.limitAxisType == H_LookAtDan_Info.AxisType.Y)
      {
        if (vector3_4.y < 0.0 && vector3_5.y > 0.0 || vector3_4.y > 0.0 && vector3_5.y < 0.0)
        {
          ref Vector3 local = ref vector3_4;
          local.y = (__Null) (local.y * -1.0);
        }
        vector3_4.y = (__Null) (double) Mathf.Clamp((float) vector3_4.y, this.limitMin, this.limitMax);
      }
      else if (this.limitAxisType == H_LookAtDan_Info.AxisType.Z)
      {
        if (vector3_4.z < 0.0 && vector3_5.z > 0.0 || vector3_4.z > 0.0 && vector3_5.z < 0.0)
        {
          ref Vector3 local = ref vector3_4;
          local.z = (__Null) (local.z * -1.0);
        }
        vector3_4.z = (__Null) (double) Mathf.Clamp((float) vector3_4.z, this.limitMin, this.limitMax);
      }
      this.trfLookAt.set_localRotation(ConvertRotation.ConvertDegreeToQuaternion(rotOrder, (float) vector3_4.x, (float) vector3_4.y, (float) vector3_4.z));
      this.oldRotation = this.trfLookAt.get_rotation();
    }
  }

  private Vector3 GetUpVector()
  {
    Vector3 vector3 = Vector3.get_up();
    if (Object.op_Implicit((Object) this.trfUpAxis))
    {
      switch (this.upAxisType)
      {
        case H_LookAtDan_Info.AxisType.X:
          vector3 = this.trfUpAxis.get_right();
          break;
        case H_LookAtDan_Info.AxisType.Y:
          vector3 = this.trfUpAxis.get_up();
          break;
        case H_LookAtDan_Info.AxisType.Z:
          vector3 = this.trfUpAxis.get_forward();
          break;
      }
    }
    return vector3;
  }

  private bool LookAtQuat(Vector3 xvec, Vector3 yvec, Vector3 zvec, ref Quaternion q)
  {
    float num1 = (float) (1.0 + xvec.x + yvec.y + zvec.z);
    if ((double) num1 == 0.0)
    {
      GlobalMethod.DebugLog("LookAt 計算不可 値0", 1);
      return false;
    }
    float f = Mathf.Sqrt(num1) / 2f;
    if (float.IsNaN(f))
    {
      GlobalMethod.DebugLog("LookAt 計算不可 NaN", 1);
      return false;
    }
    float num2 = 4f * f;
    if ((double) num2 == 0.0)
    {
      GlobalMethod.DebugLog("LookAt 計算不可 w=0", 1);
      return false;
    }
    float num3 = (float) (yvec.z - zvec.y) / num2;
    float num4 = (float) (zvec.x - xvec.z) / num2;
    float num5 = (float) (xvec.y - yvec.x) / num2;
    ((Quaternion) ref q).\u002Ector(num3, num4, num5, f);
    return true;
  }

  public enum AxisType
  {
    X,
    Y,
    Z,
    RevX,
    RevY,
    RevZ,
    None,
  }

  public enum RotationOrder
  {
    XYZ,
    XZY,
    YXZ,
    YZX,
    ZXY,
    ZYX,
  }
}
