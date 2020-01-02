// Decompiled with JetBrains decompiler
// Type: Exploder.MeshUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Exploder.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  public static class MeshUtils
  {
    public static Vector3 ComputeBarycentricCoordinates(
      Vector3 a,
      Vector3 b,
      Vector3 c,
      Vector3 p)
    {
      float num1 = (float) (b.x - a.x);
      float num2 = (float) (b.y - a.y);
      float num3 = (float) (b.z - a.z);
      float num4 = (float) (c.x - a.x);
      float num5 = (float) (c.y - a.y);
      float num6 = (float) (c.z - a.z);
      float num7 = (float) (p.x - a.x);
      float num8 = (float) (p.y - a.y);
      float num9 = (float) (p.z - a.z);
      float num10 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
      float num11 = (float) ((double) num1 * (double) num4 + (double) num2 * (double) num5 + (double) num3 * (double) num6);
      float num12 = (float) ((double) num4 * (double) num4 + (double) num5 * (double) num5 + (double) num6 * (double) num6);
      float num13 = (float) ((double) num7 * (double) num1 + (double) num8 * (double) num2 + (double) num9 * (double) num3);
      float num14 = (float) ((double) num7 * (double) num4 + (double) num8 * (double) num5 + (double) num9 * (double) num6);
      float num15 = (float) ((double) num10 * (double) num12 - (double) num11 * (double) num11);
      float num16 = (float) ((double) num12 * (double) num13 - (double) num11 * (double) num14) / num15;
      float num17 = (float) ((double) num10 * (double) num14 - (double) num11 * (double) num13) / num15;
      return new Vector3(1f - num16 - num17, num16, num17);
    }

    public static void Swap<T>(ref T a, ref T b)
    {
      T obj = a;
      a = b;
      b = obj;
    }

    public static void CenterPivot(Vector3[] vertices, Vector3 centroid)
    {
      int length = vertices.Length;
      for (int index = 0; index < length; ++index)
      {
        Vector3 vertex = vertices[index];
        ref Vector3 local1 = ref vertex;
        local1.x = local1.x - centroid.x;
        ref Vector3 local2 = ref vertex;
        local2.y = local2.y - centroid.y;
        ref Vector3 local3 = ref vertex;
        local3.z = local3.z - centroid.z;
        vertices[index] = vertex;
      }
    }

    public static List<ExploderMesh> IsolateMeshIslands(ExploderMesh mesh)
    {
      int[] triangles = mesh.triangles;
      int length1 = mesh.vertices.Length;
      int length2 = mesh.triangles.Length;
      Vector4[] tangents = mesh.tangents;
      Color32[] colors32 = mesh.colors32;
      Vector3[] vertices = mesh.vertices;
      Vector3[] normals = mesh.normals;
      Vector2[] uv = mesh.uv;
      bool flag1 = tangents != null && tangents.Length > 0;
      bool flag2 = colors32 != null && colors32.Length > 0;
      bool flag3 = normals != null && normals.Length > 0;
      if (length2 <= 3)
        return (List<ExploderMesh>) null;
      LSHash lsHash = new LSHash(0.1f, length1);
      int[] numArray = new int[length2];
      for (int index = 0; index < length2; ++index)
        numArray[index] = lsHash.Hash(vertices[triangles[index]]);
      List<HashSet<int>> intSetList = new List<HashSet<int>>()
      {
        new HashSet<int>() { numArray[0], numArray[1], numArray[2] }
      };
      List<List<int>> intListList = new List<List<int>>()
      {
        new List<int>(length2) { 0, 1, 2 }
      };
      bool[] flagArray = new bool[length2];
      flagArray[0] = true;
      flagArray[1] = true;
      flagArray[2] = true;
      HashSet<int> intSet = intSetList[0];
      List<int> intList1 = intListList[0];
      int num1 = 3;
      int index1 = -1;
      int num2 = 0;
      do
      {
        bool flag4 = false;
        for (int index2 = 3; index2 < length2; index2 += 3)
        {
          if (!flagArray[index2])
          {
            if (intSet.Contains(numArray[index2]) || intSet.Contains(numArray[index2 + 1]) || intSet.Contains(numArray[index2 + 2]))
            {
              intSet.Add(numArray[index2]);
              intSet.Add(numArray[index2 + 1]);
              intSet.Add(numArray[index2 + 2]);
              intList1.Add(index2);
              intList1.Add(index2 + 1);
              intList1.Add(index2 + 2);
              flagArray[index2] = true;
              flagArray[index2 + 1] = true;
              flagArray[index2 + 2] = true;
              num1 += 3;
              flag4 = true;
            }
            else
              index1 = index2;
          }
        }
        if (num1 != length2)
        {
          if (!flag4)
          {
            intSet = new HashSet<int>()
            {
              numArray[index1],
              numArray[index1 + 1],
              numArray[index1 + 2]
            };
            intList1 = new List<int>(length2 / 2)
            {
              index1,
              index1 + 1,
              index1 + 2
            };
            intSetList.Add(intSet);
            intListList.Add(intList1);
          }
          ++num2;
        }
        else
          break;
      }
      while (num2 <= 100);
      if (intSetList.Count == 1)
        return (List<ExploderMesh>) null;
      List<ExploderMesh> exploderMeshList = new List<ExploderMesh>(intSetList.Count);
      foreach (List<int> intList2 in intListList)
      {
        ExploderMesh exploderMesh1 = new ExploderMesh();
        int count = intList2.Count;
        ExploderMesh exploderMesh2 = exploderMesh1;
        List<int> intList3 = new List<int>(count);
        List<Vector3> vector3List1 = new List<Vector3>(count);
        List<Vector3> vector3List2 = new List<Vector3>(count);
        List<Vector2> vector2List = new List<Vector2>(count);
        List<Color32> color32List = new List<Color32>(count);
        List<Vector4> vector4List = new List<Vector4>(count);
        Dictionary<int, int> dictionary = new Dictionary<int, int>(length2);
        Vector3 vector3 = Vector3.get_zero();
        int num3 = 0;
        int num4 = 0;
        foreach (int index2 in intList2)
        {
          int key = triangles[index2];
          int num5 = 0;
          if (dictionary.TryGetValue(key, out num5))
          {
            intList3.Add(num5);
          }
          else
          {
            intList3.Add(num4);
            dictionary.Add(key, num4);
            ++num4;
            vector3 = Vector3.op_Addition(vector3, vertices[key]);
            ++num3;
            vector3List1.Add(vertices[key]);
            vector2List.Add(uv[key]);
            if (flag3)
              vector3List2.Add(normals[key]);
            if (flag2)
              color32List.Add(colors32[key]);
            if (flag1)
              vector4List.Add(tangents[key]);
          }
        }
        exploderMesh2.vertices = vector3List1.ToArray();
        exploderMesh2.uv = vector2List.ToArray();
        if (flag3)
          exploderMesh2.normals = vector3List2.ToArray();
        if (flag2)
          exploderMesh2.colors32 = color32List.ToArray();
        if (flag1)
          exploderMesh2.tangents = vector4List.ToArray();
        exploderMesh2.triangles = intList3.ToArray();
        exploderMesh1.centroid = Vector3.op_Division(vector3, (float) num3);
        exploderMeshList.Add(exploderMesh1);
      }
      return exploderMeshList;
    }

    public static void GeneratePolygonCollider(PolygonCollider2D collider, Mesh mesh)
    {
      if (!Object.op_Implicit((Object) mesh) || !Object.op_Implicit((Object) collider))
        return;
      Vector3[] vertices = mesh.get_vertices();
      Vector2[] Pnts = new Vector2[vertices.Length];
      for (int index = 0; index < vertices.Length; ++index)
        Pnts[index] = Vector2.op_Implicit(vertices[index]);
      Vector2[] vector2Array = Hull2D.ChainHull2D(Pnts);
      collider.SetPath(0, vector2Array);
    }
  }
}
