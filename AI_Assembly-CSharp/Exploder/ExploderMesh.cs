// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  public class ExploderMesh
  {
    public int[] triangles;
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] uv;
    public Vector4[] tangents;
    public Color32[] colors32;
    public Vector3 centroid;
    public Vector3 min;
    public Vector3 max;

    public ExploderMesh()
    {
    }

    public ExploderMesh(Mesh unityMesh)
    {
      this.triangles = unityMesh.get_triangles();
      this.vertices = unityMesh.get_vertices();
      this.normals = unityMesh.get_normals();
      this.uv = unityMesh.get_uv();
      this.tangents = unityMesh.get_tangents();
      this.colors32 = unityMesh.get_colors32();
      ExploderMesh.CalculateCentroid(new List<Vector3>((IEnumerable<Vector3>) this.vertices), ref this.centroid, ref this.min, ref this.max);
    }

    public static void CalculateCentroid(
      List<Vector3> vertices,
      ref Vector3 ctr,
      ref Vector3 min,
      ref Vector3 max)
    {
      ctr = Vector3.get_zero();
      int count = vertices.Count;
      ((Vector3) ref min).Set(float.MaxValue, float.MaxValue, float.MaxValue);
      ((Vector3) ref max).Set(float.MinValue, float.MinValue, float.MinValue);
      for (int index = 0; index < count; ++index)
      {
        if (min.x > vertices[index].x)
          min.x = vertices[index].x;
        if (min.y > vertices[index].y)
          min.y = vertices[index].y;
        if (min.z > vertices[index].z)
          min.z = vertices[index].z;
        if (max.x < vertices[index].x)
          max.x = vertices[index].x;
        if (max.y < vertices[index].y)
          max.y = vertices[index].y;
        if (max.z < vertices[index].z)
          max.z = vertices[index].z;
        ctr = Vector3.op_Addition(ctr, vertices[index]);
      }
      ctr = Vector3.op_Division(ctr, (float) count);
    }

    public Mesh ToUnityMesh()
    {
      Mesh mesh = new Mesh();
      mesh.set_vertices(this.vertices);
      mesh.set_normals(this.normals);
      mesh.set_uv(this.uv);
      mesh.set_tangents(this.tangents);
      mesh.set_colors32(this.colors32);
      mesh.set_triangles(this.triangles);
      return mesh;
    }
  }
}
