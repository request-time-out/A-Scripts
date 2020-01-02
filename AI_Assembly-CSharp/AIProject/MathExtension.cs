// Decompiled with JetBrains decompiler
// Type: AIProject.MathExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public static class MathExtension
  {
    public static bool IsInsideRange(this int source, int min, int max)
    {
      return source >= min && source <= max;
    }

    public static bool GetInsideRange(this float source, float min, float max)
    {
      return (double) source >= (double) min && (double) source <= (double) max;
    }

    public static Vector3 NearestVertexTo(this MeshFilter meshFilter, Vector3 point)
    {
      return MathExtension.NearestVertexTo(((Component) meshFilter).get_transform(), meshFilter.get_mesh(), point);
    }

    public static Vector3 NearestVertexTo(this MeshCollider collider, Vector3 point)
    {
      return MathExtension.NearestVertexTo(((Component) collider).get_transform(), collider.get_sharedMesh(), point);
    }

    public static Vector3 NearestVertexTo(Transform transform, Mesh mesh, Vector3 point)
    {
      point = transform.InverseTransformPoint(point);
      float num = float.PositiveInfinity;
      Vector3 vector3_1 = Vector3.get_zero();
      foreach (Vector3 vertex in mesh.get_vertices())
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(point, vertex);
        float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          vector3_1 = vertex;
        }
      }
      return transform.TransformPoint(vector3_1);
    }
  }
}
