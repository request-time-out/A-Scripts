// Decompiled with JetBrains decompiler
// Type: MeshBrush.CombineUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace MeshBrush
{
  public static class CombineUtility
  {
    private static int vertexCount;
    private static int triangleCount;
    private static int stripCount;
    private static int curStripCount;
    private static Vector3[] vertices;
    private static Vector3[] normals;
    private static Vector4[] tangents;
    private static Vector2[] uv;
    private static Vector2[] uv1;
    private static Color[] colors;
    private static int[] triangles;
    private static int[] strip;
    private static int offset;
    private static int triangleOffset;
    private static int stripOffset;
    private static int vertexOffset;
    private static Matrix4x4 invTranspose;
    public const string combinedMeshName = "Combined Mesh";
    private static Vector4 p4;
    private static Vector3 p;

    public static Mesh Combine(CombineUtility.MeshInstance[] combines, bool generateStrips)
    {
      CombineUtility.vertexCount = 0;
      CombineUtility.triangleCount = 0;
      CombineUtility.stripCount = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
        {
          CombineUtility.vertexCount += combine.mesh.get_vertexCount();
          if (generateStrips)
          {
            CombineUtility.curStripCount = combine.mesh.GetTriangles(combine.subMeshIndex).Length;
            if (CombineUtility.curStripCount != 0)
            {
              if (CombineUtility.stripCount != 0)
              {
                if ((CombineUtility.stripCount & 1) == 1)
                  CombineUtility.stripCount += 3;
                else
                  CombineUtility.stripCount += 2;
              }
              CombineUtility.stripCount += CombineUtility.curStripCount;
            }
            else
              generateStrips = false;
          }
        }
      }
      if (!generateStrips)
      {
        foreach (CombineUtility.MeshInstance combine in combines)
        {
          if (Object.op_Inequality((Object) combine.mesh, (Object) null))
            CombineUtility.triangleCount += combine.mesh.GetTriangles(combine.subMeshIndex).Length;
        }
      }
      CombineUtility.vertices = new Vector3[CombineUtility.vertexCount];
      CombineUtility.normals = new Vector3[CombineUtility.vertexCount];
      CombineUtility.tangents = new Vector4[CombineUtility.vertexCount];
      CombineUtility.uv = new Vector2[CombineUtility.vertexCount];
      CombineUtility.uv1 = new Vector2[CombineUtility.vertexCount];
      CombineUtility.colors = new Color[CombineUtility.vertexCount];
      CombineUtility.triangles = new int[CombineUtility.triangleCount];
      CombineUtility.strip = new int[CombineUtility.stripCount];
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
          CombineUtility.Copy(combine.mesh.get_vertexCount(), combine.mesh.get_vertices(), CombineUtility.vertices, ref CombineUtility.offset, combine.transform);
      }
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
        {
          CombineUtility.invTranspose = combine.transform;
          Matrix4x4 inverse = ((Matrix4x4) ref CombineUtility.invTranspose).get_inverse();
          CombineUtility.invTranspose = ((Matrix4x4) ref inverse).get_transpose();
          CombineUtility.CopyNormal(combine.mesh.get_vertexCount(), combine.mesh.get_normals(), CombineUtility.normals, ref CombineUtility.offset, CombineUtility.invTranspose);
        }
      }
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
        {
          CombineUtility.invTranspose = combine.transform;
          Matrix4x4 inverse = ((Matrix4x4) ref CombineUtility.invTranspose).get_inverse();
          CombineUtility.invTranspose = ((Matrix4x4) ref inverse).get_transpose();
          CombineUtility.CopyTangents(combine.mesh.get_vertexCount(), combine.mesh.get_tangents(), CombineUtility.tangents, ref CombineUtility.offset, CombineUtility.invTranspose);
        }
      }
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
          CombineUtility.Copy(combine.mesh.get_vertexCount(), combine.mesh.get_uv(), CombineUtility.uv, ref CombineUtility.offset);
      }
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
          CombineUtility.Copy(combine.mesh.get_vertexCount(), combine.mesh.get_uv2(), CombineUtility.uv1, ref CombineUtility.offset);
      }
      CombineUtility.offset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
          CombineUtility.CopyColors(combine.mesh.get_vertexCount(), combine.mesh.get_colors(), CombineUtility.colors, ref CombineUtility.offset);
      }
      CombineUtility.triangleOffset = 0;
      CombineUtility.stripOffset = 0;
      CombineUtility.vertexOffset = 0;
      foreach (CombineUtility.MeshInstance combine in combines)
      {
        if (Object.op_Inequality((Object) combine.mesh, (Object) null))
        {
          if (generateStrips)
          {
            int[] triangles = combine.mesh.GetTriangles(combine.subMeshIndex);
            if (CombineUtility.stripOffset != 0)
            {
              if ((CombineUtility.stripOffset & 1) == 1)
              {
                CombineUtility.strip[CombineUtility.stripOffset] = CombineUtility.strip[CombineUtility.stripOffset - 1];
                CombineUtility.strip[CombineUtility.stripOffset + 1] = triangles[0] + CombineUtility.vertexOffset;
                CombineUtility.strip[CombineUtility.stripOffset + 2] = triangles[0] + CombineUtility.vertexOffset;
                CombineUtility.stripOffset += 3;
              }
              else
              {
                CombineUtility.strip[CombineUtility.stripOffset] = CombineUtility.strip[CombineUtility.stripOffset - 1];
                CombineUtility.strip[CombineUtility.stripOffset + 1] = triangles[0] + CombineUtility.vertexOffset;
                CombineUtility.stripOffset += 2;
              }
            }
            for (int index = 0; index < triangles.Length; ++index)
              CombineUtility.strip[index + CombineUtility.stripOffset] = triangles[index] + CombineUtility.vertexOffset;
            CombineUtility.stripOffset += triangles.Length;
          }
          else
          {
            int[] triangles = combine.mesh.GetTriangles(combine.subMeshIndex);
            for (int index = 0; index < triangles.Length; ++index)
              CombineUtility.triangles[index + CombineUtility.triangleOffset] = triangles[index] + CombineUtility.vertexOffset;
            CombineUtility.triangleOffset += triangles.Length;
          }
          CombineUtility.vertexOffset += combine.mesh.get_vertexCount();
        }
      }
      Mesh mesh = new Mesh();
      ((Object) mesh).set_name("Combined Mesh");
      mesh.set_vertices(CombineUtility.vertices);
      mesh.set_normals(CombineUtility.normals);
      mesh.set_colors(CombineUtility.colors);
      mesh.set_uv(CombineUtility.uv);
      mesh.set_uv2(CombineUtility.uv1);
      mesh.set_tangents(CombineUtility.tangents);
      if (generateStrips)
        mesh.SetTriangles(CombineUtility.strip, 0);
      else
        mesh.set_triangles(CombineUtility.triangles);
      return mesh;
    }

    private static void Copy(
      int vertexcount,
      Vector3[] src,
      Vector3[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
      for (int index = 0; index < src.Length; ++index)
        dst[index + offset] = ((Matrix4x4) ref transform).MultiplyPoint(src[index]);
      offset += vertexcount;
    }

    private static void CopyNormal(
      int vertexcount,
      Vector3[] src,
      Vector3[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
      for (int index = 0; index < src.Length; ++index)
      {
        ref Vector3 local = ref dst[index + offset];
        Vector3 vector3 = ((Matrix4x4) ref transform).MultiplyVector(src[index]);
        Vector3 normalized = ((Vector3) ref vector3).get_normalized();
        local = normalized;
      }
      offset += vertexcount;
    }

    private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
    {
      for (int index = 0; index < src.Length; ++index)
        dst[index + offset] = src[index];
      offset += vertexcount;
    }

    private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
    {
      for (int index = 0; index < src.Length; ++index)
        dst[index + offset] = src[index];
      offset += vertexcount;
    }

    private static void CopyTangents(
      int vertexcount,
      Vector4[] src,
      Vector4[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
      for (int index = 0; index < src.Length; ++index)
      {
        CombineUtility.p4 = src[index];
        CombineUtility.p = new Vector3((float) CombineUtility.p4.x, (float) CombineUtility.p4.y, (float) CombineUtility.p4.z);
        Vector3 vector3 = ((Matrix4x4) ref transform).MultiplyVector(CombineUtility.p);
        CombineUtility.p = ((Vector3) ref vector3).get_normalized();
        dst[index + offset] = new Vector4((float) CombineUtility.p.x, (float) CombineUtility.p.y, (float) CombineUtility.p.z, (float) CombineUtility.p4.w);
      }
      offset += vertexcount;
    }

    public struct MeshInstance
    {
      public Mesh mesh;
      public int subMeshIndex;
      public Matrix4x4 transform;
    }
  }
}
