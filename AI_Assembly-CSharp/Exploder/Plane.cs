// Decompiled with JetBrains decompiler
// Type: Exploder.Plane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder
{
  public class Plane
  {
    private const float epsylon = 0.0001f;
    public Vector3 Normal;
    public float Distance;

    public Plane(Vector3 a, Vector3 b, Vector3 c)
    {
      Vector3 vector3 = Vector3.Cross(Vector3.op_Subtraction(b, a), Vector3.op_Subtraction(c, a));
      this.Normal = ((Vector3) ref vector3).get_normalized();
      this.Distance = Vector3.Dot(this.Normal, a);
      this.Pnt = a;
    }

    public Plane(Vector3 normal, Vector3 p)
    {
      this.Set(normal, p);
    }

    public Plane(Plane instance)
    {
      this.Normal = instance.Normal;
      this.Distance = instance.Distance;
      this.Pnt = instance.Pnt;
    }

    public Vector3 Pnt { get; private set; }

    public void Set(Vector3 normal, Vector3 p)
    {
      this.Normal = ((Vector3) ref normal).get_normalized();
      this.Distance = Vector3.Dot(this.Normal, p);
      this.Pnt = p;
    }

    public Plane.PointClass ClassifyPoint(Vector3 p)
    {
      float num = Vector3.Dot(p, this.Normal) - this.Distance;
      if ((double) num < -9.99999974737875E-05)
        return Plane.PointClass.Back;
      return (double) num > 9.99999974737875E-05 ? Plane.PointClass.Front : Plane.PointClass.Coplanar;
    }

    public bool GetSide(Vector3 n)
    {
      return (double) Vector3.Dot(n, this.Normal) - (double) this.Distance > 9.99999974737875E-05;
    }

    public void Flip()
    {
      this.Normal = Vector3.op_UnaryNegation(this.Normal);
      this.Distance = -this.Distance;
    }

    public bool GetSideFix(ref Vector3 n)
    {
      float num1 = (float) (n.x * this.Normal.x + n.y * this.Normal.y + n.z * this.Normal.z) - this.Distance;
      float num2 = 1f;
      float num3 = num1;
      if ((double) num1 < 0.0)
      {
        num2 = -1f;
        num3 = -num1;
      }
      if ((double) num3 < 0.00109999999403954)
      {
        ref Vector3 local1 = ref n;
        local1.x = (__Null) (local1.x + this.Normal.x * (1.0 / 1000.0) * (double) num2);
        ref Vector3 local2 = ref n;
        local2.y = (__Null) (local2.y + this.Normal.y * (1.0 / 1000.0) * (double) num2);
        ref Vector3 local3 = ref n;
        local3.z = (__Null) (local3.z + this.Normal.z * (1.0 / 1000.0) * (double) num2);
        num1 = (float) (n.x * this.Normal.x + n.y * this.Normal.y + n.z * this.Normal.z) - this.Distance;
      }
      return (double) num1 > 9.99999974737875E-05;
    }

    public bool SameSide(Vector3 a, Vector3 b)
    {
      throw new NotImplementedException();
    }

    public bool IntersectSegment(Vector3 a, Vector3 b, out float t, ref Vector3 q)
    {
      float num1 = (float) (b.x - a.x);
      float num2 = (float) (b.y - a.y);
      float num3 = (float) (b.z - a.z);
      float num4 = (float) (this.Normal.x * a.x + this.Normal.y * a.y + this.Normal.z * a.z);
      float num5 = (float) (this.Normal.x * (double) num1 + this.Normal.y * (double) num2 + this.Normal.z * (double) num3);
      t = (this.Distance - num4) / num5;
      if ((double) t >= -9.99999974737875E-05 && (double) t <= 1.00010001659393)
      {
        q.x = (__Null) (a.x + (double) t * (double) num1);
        q.y = (__Null) (a.y + (double) t * (double) num2);
        q.z = (__Null) (a.z + (double) t * (double) num3);
        return true;
      }
      q = Vector3.get_zero();
      return false;
    }

    public void InverseTransform(ExploderTransform transform)
    {
      Vector3 vector3_1 = transform.InverseTransformDirection(this.Normal);
      Vector3 vector3_2 = transform.InverseTransformPoint(this.Pnt);
      this.Normal = vector3_1;
      this.Distance = Vector3.Dot(vector3_1, vector3_2);
    }

    public Matrix4x4 GetPlaneMatrix()
    {
      Matrix4x4 matrix4x4 = (Matrix4x4) null;
      Quaternion quaternion = Quaternion.LookRotation(this.Normal);
      ((Matrix4x4) ref matrix4x4).SetTRS(this.Pnt, quaternion, Vector3.get_one());
      return matrix4x4;
    }

    [Flags]
    public enum PointClass
    {
      Coplanar = 0,
      Front = 1,
      Back = 2,
      Intersection = Back | Front, // 0x00000003
    }
  }
}
