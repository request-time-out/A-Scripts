// Decompiled with JetBrains decompiler
// Type: Exploder.MeshCutter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
  public class MeshCutter
  {
    private List<int>[] triangles;
    private List<Vector3>[] vertices;
    private List<Vector3>[] normals;
    private List<Vector2>[] uvs;
    private List<Vector4>[] tangents;
    private List<Color32>[] vertexColors;
    private List<int> cutTris;
    private int[] triCache;
    private Vector3[] centroid;
    private int[] triCounter;
    private Array<int> polygonIndicesArray;
    private Contour contour;
    private Dictionary<long, int>[] cutVertCache;
    private Dictionary<int, int>[] cornerVertCache;
    private int contourBufferSize;
    private Color crossSectionVertexColour;
    private Vector4 crossSectionUV;

    public void Init(int trianglesNum, int verticesNum)
    {
      this.AllocateBuffers(trianglesNum, verticesNum, false, false);
      this.AllocateContours(trianglesNum / 2);
    }

    private void AllocateBuffers(
      int trianglesNum,
      int verticesNum,
      bool useMeshTangents,
      bool useVertexColors)
    {
      if (this.triangles == null || this.triangles[0].Capacity < trianglesNum)
      {
        this.triangles = new List<int>[2]
        {
          new List<int>(trianglesNum),
          new List<int>(trianglesNum)
        };
      }
      else
      {
        this.triangles[0].Clear();
        this.triangles[1].Clear();
      }
      if (this.vertices == null || this.vertices[0].Capacity < verticesNum || this.triCache.Length < verticesNum || useMeshTangents && (this.tangents == null || this.tangents[0].Capacity < verticesNum) || useVertexColors && (this.vertexColors == null || this.vertexColors[0].Capacity < verticesNum))
      {
        this.vertices = new List<Vector3>[2]
        {
          new List<Vector3>(verticesNum),
          new List<Vector3>(verticesNum)
        };
        this.normals = new List<Vector3>[2]
        {
          new List<Vector3>(verticesNum),
          new List<Vector3>(verticesNum)
        };
        this.uvs = new List<Vector2>[2]
        {
          new List<Vector2>(verticesNum),
          new List<Vector2>(verticesNum)
        };
        if (useMeshTangents)
          this.tangents = new List<Vector4>[2]
          {
            new List<Vector4>(verticesNum),
            new List<Vector4>(verticesNum)
          };
        else
          this.tangents = new List<Vector4>[2]
          {
            new List<Vector4>(0),
            new List<Vector4>(0)
          };
        if (useVertexColors)
          this.vertexColors = new List<Color32>[2]
          {
            new List<Color32>(verticesNum),
            new List<Color32>(verticesNum)
          };
        else
          this.vertexColors = new List<Color32>[2]
          {
            new List<Color32>(0),
            new List<Color32>(0)
          };
        this.centroid = new Vector3[2];
        this.triCache = new int[verticesNum + 1];
        this.triCounter = new int[2];
        this.cutTris = new List<int>(verticesNum / 3);
      }
      else
      {
        for (int index = 0; index < 2; ++index)
        {
          this.vertices[index].Clear();
          this.normals[index].Clear();
          this.uvs[index].Clear();
          this.tangents[index].Clear();
          this.vertexColors[index].Clear();
          this.centroid[index] = Vector3.get_zero();
          this.triCounter[index] = 0;
        }
        this.cutTris.Clear();
        for (int index = 0; index < this.triCache.Length; ++index)
          this.triCache[index] = 0;
      }
      if (this.polygonIndicesArray != null)
        return;
      this.polygonIndicesArray = new Array<int>(1024);
    }

    private void AllocateContours(int cutTrianglesNum)
    {
      if (this.contour == null)
      {
        this.contour = new Contour(cutTrianglesNum);
        this.cutVertCache = new Dictionary<long, int>[2]
        {
          new Dictionary<long, int>(cutTrianglesNum * 2),
          new Dictionary<long, int>(cutTrianglesNum * 2)
        };
        this.cornerVertCache = new Dictionary<int, int>[2]
        {
          new Dictionary<int, int>(cutTrianglesNum),
          new Dictionary<int, int>(cutTrianglesNum)
        };
        this.contourBufferSize = cutTrianglesNum;
      }
      else
      {
        if (this.contourBufferSize < cutTrianglesNum)
        {
          this.cutVertCache = new Dictionary<long, int>[2]
          {
            new Dictionary<long, int>(cutTrianglesNum * 2),
            new Dictionary<long, int>(cutTrianglesNum * 2)
          };
          this.cornerVertCache = new Dictionary<int, int>[2]
          {
            new Dictionary<int, int>(cutTrianglesNum),
            new Dictionary<int, int>(cutTrianglesNum)
          };
          this.contourBufferSize = cutTrianglesNum;
        }
        else
        {
          for (int index = 0; index < 2; ++index)
          {
            this.cutVertCache[index].Clear();
            this.cornerVertCache[index].Clear();
          }
        }
        this.contour.AllocateBuffers(cutTrianglesNum);
      }
    }

    public float Cut(
      ExploderMesh mesh,
      ExploderTransform meshTransform,
      Plane plane,
      bool triangulateHoles,
      bool allowOpenMesh,
      ref List<ExploderMesh> meshes,
      Color crossSectionVertexColor,
      Vector4 crossUV)
    {
      this.crossSectionVertexColour = crossSectionVertexColor;
      this.crossSectionUV = crossUV;
      return this.Cut(mesh, meshTransform, plane, triangulateHoles, allowOpenMesh, ref meshes);
    }

    private float Cut(
      ExploderMesh mesh,
      ExploderTransform meshTransform,
      Plane plane,
      bool triangulateHoles,
      bool allowOpenMesh,
      ref List<ExploderMesh> meshes)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      int length1 = mesh.triangles.Length;
      int length2 = mesh.vertices.Length;
      int[] triangles1 = mesh.triangles;
      Vector4[] tangents = mesh.tangents;
      Color32[] colors32 = mesh.colors32;
      Vector3[] vertices = mesh.vertices;
      Vector3[] normals = mesh.normals;
      Vector2[] uv = mesh.uv;
      bool flag1 = tangents != null && tangents.Length > 0;
      bool flag2 = colors32 != null && colors32.Length > 0;
      bool useNormals = normals != null && normals.Length > 0;
      this.AllocateBuffers(length1, length2, flag1, flag2);
      for (int index1 = 0; index1 < length1; index1 += 3)
      {
        Vector3 n1 = vertices[triangles1[index1]];
        Vector3 n2 = vertices[triangles1[index1 + 1]];
        Vector3 n3 = vertices[triangles1[index1 + 2]];
        bool sideFix1 = plane.GetSideFix(ref n1);
        bool sideFix2 = plane.GetSideFix(ref n2);
        bool sideFix3 = plane.GetSideFix(ref n3);
        vertices[triangles1[index1]] = n1;
        vertices[triangles1[index1 + 1]] = n2;
        vertices[triangles1[index1 + 2]] = n3;
        if (sideFix1 == sideFix2 && sideFix2 == sideFix3)
        {
          int index2 = !sideFix1 ? 1 : 0;
          if (triangles1[index1] < this.triCache.Length)
            ;
          if (this.triCache[triangles1[index1]] == 0)
          {
            this.triangles[index2].Add(this.triCounter[index2]);
            this.vertices[index2].Add(vertices[triangles1[index1]]);
            this.uvs[index2].Add(uv[triangles1[index1]]);
            if (useNormals)
              this.normals[index2].Add(normals[triangles1[index1]]);
            if (flag1)
              this.tangents[index2].Add(tangents[triangles1[index1]]);
            if (flag2)
              this.vertexColors[index2].Add(colors32[triangles1[index1]]);
            ref Vector3 local = ref this.centroid[index2];
            local = Vector3.op_Addition(local, vertices[triangles1[index1]]);
            this.triCache[triangles1[index1]] = this.triCounter[index2] + 1;
            ++this.triCounter[index2];
          }
          else
            this.triangles[index2].Add(this.triCache[triangles1[index1]] - 1);
          if (this.triCache[triangles1[index1 + 1]] == 0)
          {
            this.triangles[index2].Add(this.triCounter[index2]);
            this.vertices[index2].Add(vertices[triangles1[index1 + 1]]);
            this.uvs[index2].Add(uv[triangles1[index1 + 1]]);
            if (useNormals)
              this.normals[index2].Add(normals[triangles1[index1 + 1]]);
            if (flag1)
              this.tangents[index2].Add(tangents[triangles1[index1 + 1]]);
            if (flag2)
              this.vertexColors[index2].Add(colors32[triangles1[index1 + 1]]);
            ref Vector3 local = ref this.centroid[index2];
            local = Vector3.op_Addition(local, vertices[triangles1[index1 + 1]]);
            this.triCache[triangles1[index1 + 1]] = this.triCounter[index2] + 1;
            ++this.triCounter[index2];
          }
          else
            this.triangles[index2].Add(this.triCache[triangles1[index1 + 1]] - 1);
          if (this.triCache[triangles1[index1 + 2]] == 0)
          {
            this.triangles[index2].Add(this.triCounter[index2]);
            this.vertices[index2].Add(vertices[triangles1[index1 + 2]]);
            this.uvs[index2].Add(uv[triangles1[index1 + 2]]);
            if (useNormals)
              this.normals[index2].Add(normals[triangles1[index1 + 2]]);
            if (flag1)
              this.tangents[index2].Add(tangents[triangles1[index1 + 2]]);
            if (flag2)
              this.vertexColors[index2].Add(colors32[triangles1[index1 + 2]]);
            ref Vector3 local = ref this.centroid[index2];
            local = Vector3.op_Addition(local, vertices[triangles1[index1 + 2]]);
            this.triCache[triangles1[index1 + 2]] = this.triCounter[index2] + 1;
            ++this.triCounter[index2];
          }
          else
            this.triangles[index2].Add(this.triCache[triangles1[index1 + 2]] - 1);
        }
        else
          this.cutTris.Add(index1);
      }
      if (this.vertices[0].Count == 0)
      {
        this.centroid[0] = vertices[0];
      }
      else
      {
        ref Vector3 local = ref this.centroid[0];
        local = Vector3.op_Division(local, (float) this.vertices[0].Count);
      }
      if (this.vertices[1].Count == 0)
      {
        this.centroid[1] = vertices[1];
      }
      else
      {
        ref Vector3 local = ref this.centroid[1];
        local = Vector3.op_Division(local, (float) this.vertices[1].Count);
      }
      if (this.cutTris.Count < 1)
      {
        stopwatch.Stop();
        return (float) stopwatch.ElapsedMilliseconds;
      }
      this.AllocateContours(this.cutTris.Count);
      foreach (int cutTri in this.cutTris)
      {
        MeshCutter.Triangle triangle = new MeshCutter.Triangle();
        triangle.ids = new int[3]
        {
          triangles1[cutTri],
          triangles1[cutTri + 1],
          triangles1[cutTri + 2]
        };
        triangle.pos = new Vector3[3]
        {
          vertices[triangles1[cutTri]],
          vertices[triangles1[cutTri + 1]],
          vertices[triangles1[cutTri + 2]]
        };
        ref MeshCutter.Triangle local1 = ref triangle;
        Vector3[] vector3Array;
        if (useNormals)
          vector3Array = new Vector3[3]
          {
            normals[triangles1[cutTri]],
            normals[triangles1[cutTri + 1]],
            normals[triangles1[cutTri + 2]]
          };
        else
          vector3Array = new Vector3[3]
          {
            Vector3.get_zero(),
            Vector3.get_zero(),
            Vector3.get_zero()
          };
        local1.normal = vector3Array;
        triangle.uvs = new Vector2[3]
        {
          uv[triangles1[cutTri]],
          uv[triangles1[cutTri + 1]],
          uv[triangles1[cutTri + 2]]
        };
        ref MeshCutter.Triangle local2 = ref triangle;
        Vector4[] vector4Array;
        if (flag1)
          vector4Array = new Vector4[3]
          {
            tangents[triangles1[cutTri]],
            tangents[triangles1[cutTri + 1]],
            tangents[triangles1[cutTri + 2]]
          };
        else
          vector4Array = new Vector4[3]
          {
            Vector4.get_zero(),
            Vector4.get_zero(),
            Vector4.get_zero()
          };
        local2.tangents = vector4Array;
        ref MeshCutter.Triangle local3 = ref triangle;
        Color32[] color32Array;
        if (flag2)
          color32Array = new Color32[3]
          {
            colors32[triangles1[cutTri]],
            colors32[triangles1[cutTri + 1]],
            colors32[triangles1[cutTri + 2]]
          };
        else
          color32Array = new Color32[3]
          {
            Color32.op_Implicit(Color.get_white()),
            Color32.op_Implicit(Color.get_white()),
            Color32.op_Implicit(Color.get_white())
          };
        local3.colors = color32Array;
        MeshCutter.Triangle tri = triangle;
        bool side1 = plane.GetSide(tri.pos[0]);
        bool side2 = plane.GetSide(tri.pos[1]);
        bool side3 = plane.GetSide(tri.pos[2]);
        Vector3 zero1 = Vector3.get_zero();
        Vector3 zero2 = Vector3.get_zero();
        int index1 = !side1 ? 1 : 0;
        int index2 = 1 - index1;
        float t1;
        float t2;
        if (side1 == side2)
        {
          plane.IntersectSegment(tri.pos[2], tri.pos[0], out t1, ref zero1);
          plane.IntersectSegment(tri.pos[2], tri.pos[1], out t2, ref zero2);
          int id0_1 = this.AddIntersectionPoint(zero1, tri, tri.ids[2], tri.ids[0], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int id1_1 = this.AddIntersectionPoint(zero2, tri, tri.ids[2], tri.ids[1], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int num1 = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.tangents[0], tri.colors[0], tri.ids[0], this.triCache, this.cornerVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int num2 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.tangents[1], tri.colors[1], tri.ids[1], this.triCache, this.cornerVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          this.triangles[index1].Add(id0_1);
          this.triangles[index1].Add(num1);
          this.triangles[index1].Add(id1_1);
          this.triangles[index1].Add(id1_1);
          this.triangles[index1].Add(num1);
          this.triangles[index1].Add(num2);
          int id0_2 = this.AddIntersectionPoint(zero1, tri, tri.ids[2], tri.ids[0], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int id1_2 = this.AddIntersectionPoint(zero2, tri, tri.ids[2], tri.ids[1], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int num3 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.tangents[2], tri.colors[2], tri.ids[2], this.triCache, this.cornerVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          this.triangles[index2].Add(num3);
          this.triangles[index2].Add(id0_2);
          this.triangles[index2].Add(id1_2);
          if (triangulateHoles)
          {
            if (index1 == 0)
              this.contour.AddTriangle(cutTri, id0_1, id1_1, zero1, zero2);
            else
              this.contour.AddTriangle(cutTri, id0_2, id1_2, zero1, zero2);
          }
        }
        else if (side1 == side3)
        {
          plane.IntersectSegment(tri.pos[1], tri.pos[0], out t1, ref zero2);
          plane.IntersectSegment(tri.pos[1], tri.pos[2], out t2, ref zero1);
          int id0_1 = this.AddIntersectionPoint(zero1, tri, tri.ids[1], tri.ids[2], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int id1_1 = this.AddIntersectionPoint(zero2, tri, tri.ids[1], tri.ids[0], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int num1 = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.tangents[0], tri.colors[0], tri.ids[0], this.triCache, this.cornerVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int num2 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.tangents[2], tri.colors[2], tri.ids[2], this.triCache, this.cornerVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          this.triangles[index1].Add(num2);
          this.triangles[index1].Add(id1_1);
          this.triangles[index1].Add(id0_1);
          this.triangles[index1].Add(num2);
          this.triangles[index1].Add(num1);
          this.triangles[index1].Add(id1_1);
          int id0_2 = this.AddIntersectionPoint(zero1, tri, tri.ids[1], tri.ids[2], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int id1_2 = this.AddIntersectionPoint(zero2, tri, tri.ids[1], tri.ids[0], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int num3 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.tangents[1], tri.colors[1], tri.ids[1], this.triCache, this.cornerVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          this.triangles[index2].Add(id0_2);
          this.triangles[index2].Add(id1_2);
          this.triangles[index2].Add(num3);
          if (triangulateHoles)
          {
            if (index1 == 0)
              this.contour.AddTriangle(cutTri, id0_1, id1_1, zero1, zero2);
            else
              this.contour.AddTriangle(cutTri, id0_2, id1_2, zero1, zero2);
          }
        }
        else
        {
          plane.IntersectSegment(tri.pos[0], tri.pos[1], out t1, ref zero1);
          plane.IntersectSegment(tri.pos[0], tri.pos[2], out t2, ref zero2);
          int id0_1 = this.AddIntersectionPoint(zero1, tri, tri.ids[0], tri.ids[1], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int id1_1 = this.AddIntersectionPoint(zero2, tri, tri.ids[0], tri.ids[2], this.cutVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int num1 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.tangents[1], tri.colors[1], tri.ids[1], this.triCache, this.cornerVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          int num2 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.tangents[2], tri.colors[2], tri.ids[2], this.triCache, this.cornerVertCache[index2], this.vertices[index2], this.normals[index2], this.uvs[index2], this.tangents[index2], this.vertexColors[index2], flag1, flag2, useNormals);
          this.triangles[index2].Add(num2);
          this.triangles[index2].Add(id1_1);
          this.triangles[index2].Add(num1);
          this.triangles[index2].Add(id1_1);
          this.triangles[index2].Add(id0_1);
          this.triangles[index2].Add(num1);
          int id0_2 = this.AddIntersectionPoint(zero1, tri, tri.ids[0], tri.ids[1], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int id1_2 = this.AddIntersectionPoint(zero2, tri, tri.ids[0], tri.ids[2], this.cutVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          int num3 = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.tangents[0], tri.colors[0], tri.ids[0], this.triCache, this.cornerVertCache[index1], this.vertices[index1], this.normals[index1], this.uvs[index1], this.tangents[index1], this.vertexColors[index1], flag1, flag2, useNormals);
          this.triangles[index1].Add(id1_2);
          this.triangles[index1].Add(num3);
          this.triangles[index1].Add(id0_2);
          if (triangulateHoles)
          {
            if (index1 == 0)
              this.contour.AddTriangle(cutTri, id0_2, id1_2, zero1, zero2);
            else
              this.contour.AddTriangle(cutTri, id0_1, id1_1, zero1, zero2);
          }
        }
      }
      if (triangulateHoles)
      {
        this.contour.FindContours();
        if (this.contour.contour.Count == 0 || this.contour.contour[0].Count < 3)
        {
          if (allowOpenMesh)
          {
            triangulateHoles = false;
          }
          else
          {
            stopwatch.Stop();
            return (float) stopwatch.ElapsedMilliseconds;
          }
        }
      }
      List<int>[] triangles2 = (List<int>[]) null;
      Vector3 zero3 = Vector3.get_zero();
      Vector3 zero4 = Vector3.get_zero();
      Vector3 zero5 = Vector3.get_zero();
      Vector3 zero6 = Vector3.get_zero();
      Vector3 zero7 = Vector3.get_zero();
      Vector3 zero8 = Vector3.get_zero();
      ExploderMesh.CalculateCentroid(this.vertices[0], ref zero3, ref zero5, ref zero6);
      ExploderMesh.CalculateCentroid(this.vertices[1], ref zero4, ref zero7, ref zero8);
      if (triangulateHoles)
      {
        triangles2 = new List<int>[2]
        {
          new List<int>(this.contour.MidPointsCount),
          new List<int>(this.contour.MidPointsCount)
        };
        this.Triangulate(this.contour.contour, plane, this.vertices, this.normals, this.uvs, this.tangents, this.vertexColors, triangles2, true, flag1, flag2, useNormals);
      }
      if (this.vertices[0].Count > 3 && this.vertices[1].Count > 3)
      {
        ExploderMesh exploderMesh1 = new ExploderMesh();
        ExploderMesh exploderMesh2 = new ExploderMesh();
        Vector3[] array1 = this.vertices[0].ToArray();
        Vector3[] array2 = this.vertices[1].ToArray();
        exploderMesh1.vertices = array1;
        exploderMesh1.uv = this.uvs[0].ToArray();
        exploderMesh2.vertices = array2;
        exploderMesh2.uv = this.uvs[1].ToArray();
        if (useNormals)
        {
          exploderMesh1.normals = this.normals[0].ToArray();
          exploderMesh2.normals = this.normals[1].ToArray();
        }
        if (flag1)
        {
          exploderMesh1.tangents = this.tangents[0].ToArray();
          exploderMesh2.tangents = this.tangents[1].ToArray();
        }
        if (flag2)
        {
          exploderMesh1.colors32 = this.vertexColors[0].ToArray();
          exploderMesh2.colors32 = this.vertexColors[1].ToArray();
        }
        if (triangles2 != null && triangles2[0].Count > 3)
        {
          this.triangles[0].AddRange((IEnumerable<int>) triangles2[0]);
          this.triangles[1].AddRange((IEnumerable<int>) triangles2[1]);
        }
        exploderMesh1.triangles = this.triangles[0].ToArray();
        exploderMesh2.triangles = this.triangles[1].ToArray();
        exploderMesh1.centroid = zero3;
        exploderMesh1.min = zero5;
        exploderMesh1.max = zero6;
        exploderMesh2.centroid = zero4;
        exploderMesh2.min = zero7;
        exploderMesh2.max = zero8;
        meshes = new List<ExploderMesh>()
        {
          exploderMesh1,
          exploderMesh2
        };
        stopwatch.Stop();
        return (float) stopwatch.ElapsedMilliseconds;
      }
      stopwatch.Stop();
      return (float) stopwatch.ElapsedMilliseconds;
    }

    private int AddIntersectionPoint(
      Vector3 pos,
      MeshCutter.Triangle tri,
      int edge0,
      int edge1,
      Dictionary<long, int> cache,
      List<Vector3> vertices,
      List<Vector3> normals,
      List<Vector2> uvs,
      List<Vector4> tangents,
      List<Color32> colors32,
      bool useTangents,
      bool useColors,
      bool useNormals)
    {
      int num1 = edge0 >= edge1 ? (edge1 << 16) + edge0 : (edge0 << 16) + edge1;
      int num2;
      if (cache.TryGetValue((long) num1, out num2))
        return num2;
      Vector3 barycentricCoordinates = MeshUtils.ComputeBarycentricCoordinates(tri.pos[0], tri.pos[1], tri.pos[2], pos);
      vertices.Add(pos);
      if (useNormals)
        normals.Add(new Vector3((float) (barycentricCoordinates.x * tri.normal[0].x + barycentricCoordinates.y * tri.normal[1].x + barycentricCoordinates.z * tri.normal[2].x), (float) (barycentricCoordinates.x * tri.normal[0].y + barycentricCoordinates.y * tri.normal[1].y + barycentricCoordinates.z * tri.normal[2].y), (float) (barycentricCoordinates.x * tri.normal[0].z + barycentricCoordinates.y * tri.normal[1].z + barycentricCoordinates.z * tri.normal[2].z)));
      uvs.Add(new Vector2((float) (barycentricCoordinates.x * tri.uvs[0].x + barycentricCoordinates.y * tri.uvs[1].x + barycentricCoordinates.z * tri.uvs[2].x), (float) (barycentricCoordinates.x * tri.uvs[0].y + barycentricCoordinates.y * tri.uvs[1].y + barycentricCoordinates.z * tri.uvs[2].y)));
      if (useTangents)
        tangents.Add(new Vector4((float) (barycentricCoordinates.x * tri.tangents[0].x + barycentricCoordinates.y * tri.tangents[1].x + barycentricCoordinates.z * tri.tangents[2].x), (float) (barycentricCoordinates.x * tri.tangents[0].y + barycentricCoordinates.y * tri.tangents[1].y + barycentricCoordinates.z * tri.tangents[2].y), (float) (barycentricCoordinates.x * tri.tangents[0].z + barycentricCoordinates.y * tri.tangents[1].z + barycentricCoordinates.z * tri.tangents[2].z), (float) (barycentricCoordinates.x * tri.tangents[0].w + barycentricCoordinates.y * tri.tangents[1].w + barycentricCoordinates.z * tri.tangents[2].z)));
      if (useColors)
        colors32.Add(tri.colors[0]);
      int num3 = vertices.Count - 1;
      cache.Add((long) num1, num3);
      return num3;
    }

    private int AddTrianglePoint(
      Vector3 pos,
      Vector3 normal,
      Vector2 uv,
      Vector4 tangent,
      Color32 color,
      int idx,
      int[] triCache,
      Dictionary<int, int> cache,
      List<Vector3> vertices,
      List<Vector3> normals,
      List<Vector2> uvs,
      List<Vector4> tangents,
      List<Color32> colors,
      bool useTangents,
      bool useColors,
      bool useNormals)
    {
      if (triCache[idx] != 0)
        return triCache[idx] - 1;
      int num1;
      if (cache.TryGetValue(idx, out num1))
        return num1;
      vertices.Add(pos);
      if (useNormals)
        normals.Add(normal);
      uvs.Add(uv);
      if (useTangents)
        tangents.Add(tangent);
      if (useColors)
        colors.Add(color);
      int num2 = vertices.Count - 1;
      cache.Add(idx, num2);
      return num2;
    }

    private void Triangulate(
      List<Dictionary<int, int>> contours,
      Plane plane,
      List<Vector3>[] vertices,
      List<Vector3>[] normals,
      List<Vector2>[] uvs,
      List<Vector4>[] tangents,
      List<Color32>[] colors,
      List<int>[] triangles,
      bool uvCutMesh,
      bool useTangents,
      bool useColors,
      bool useNormals)
    {
      if (contours.Count == 0 || contours[0].Count < 3)
        return;
      Matrix4x4 planeMatrix = plane.GetPlaneMatrix();
      Matrix4x4 inverse = ((Matrix4x4) ref planeMatrix).get_inverse();
      float num1 = 0.0f;
      List<Polygon> polygonList1 = new List<Polygon>(contours.Count);
      Polygon polygon1 = (Polygon) null;
      foreach (Dictionary<int, int> contour in contours)
      {
        Vector2[] pnts = new Vector2[contour.Count];
        int num2 = 0;
        foreach (int index in contour.Values)
        {
          Vector4 vector4 = Matrix4x4.op_Multiply(inverse, Vector4.op_Implicit(vertices[0][index]));
          pnts[num2++] = Vector4.op_Implicit(vector4);
          num1 = (float) vector4.z;
        }
        Polygon polygon2 = new Polygon(pnts);
        polygonList1.Add(polygon2);
        if (polygon1 == null || (double) Mathf.Abs(polygon1.Area) < (double) Mathf.Abs(polygon2.Area))
          polygon1 = polygon2;
      }
      if (polygonList1.Count > 0)
      {
        List<Polygon> polygonList2 = new List<Polygon>();
        foreach (Polygon polygon2 in polygonList1)
        {
          if (polygon2 != polygon1 && polygon1.IsPointInside(Vector2.op_Implicit(polygon2.Points[0])))
          {
            polygon1.AddHole(polygon2);
            polygonList2.Add(polygon2);
          }
        }
        foreach (Polygon polygon2 in polygonList2)
          polygonList1.Remove(polygon2);
      }
      int count1 = vertices[0].Count;
      int count2 = vertices[1].Count;
      foreach (Polygon polygon2 in polygonList1)
      {
        if (polygon2.Triangulate(this.polygonIndicesArray))
        {
          float num2 = Mathf.Min((float) polygon2.Min.x, (float) polygon2.Min.y);
          float num3 = Mathf.Max((float) polygon2.Max.x, (float) polygon2.Max.y) - num2;
          foreach (Vector2 point in polygon2.Points)
          {
            Vector4 vector4_1 = Matrix4x4.op_Multiply(planeMatrix, Vector4.op_Implicit(new Vector3((float) point.x, (float) point.y, num1)));
            vertices[0].Add(Vector4.op_Implicit(vector4_1));
            vertices[1].Add(Vector4.op_Implicit(vector4_1));
            if (useNormals)
            {
              normals[0].Add(Vector3.op_UnaryNegation(plane.Normal));
              normals[1].Add(plane.Normal);
            }
            if (uvCutMesh)
            {
              Vector2 vector2_1;
              ((Vector2) ref vector2_1).\u002Ector(((float) point.x - num2) / num3, ((float) point.y - num2) / num3);
              Vector2 vector2_2;
              ((Vector2) ref vector2_2).\u002Ector(((float) point.x - num2) / num3, ((float) point.y - num2) / num3);
              float num4 = (float) (this.crossSectionUV.z - this.crossSectionUV.x);
              float num5 = (float) (this.crossSectionUV.w - this.crossSectionUV.y);
              vector2_1.x = (__Null) (this.crossSectionUV.x + vector2_1.x * (double) num4);
              vector2_1.y = (__Null) (this.crossSectionUV.y + vector2_1.y * (double) num5);
              vector2_2.x = (__Null) (this.crossSectionUV.x + vector2_2.x * (double) num4);
              vector2_2.y = (__Null) (this.crossSectionUV.y + vector2_2.y * (double) num5);
              uvs[0].Add(vector2_1);
              uvs[1].Add(vector2_2);
            }
            else
            {
              uvs[0].Add(Vector2.get_zero());
              uvs[1].Add(Vector2.get_zero());
            }
            if (useTangents)
            {
              Vector3 normal = plane.Normal;
              // ISSUE: cast to a reference type
              // ISSUE: cast to a reference type
              MeshUtils.Swap<float>((float&) ref normal.x, (float&) ref normal.y);
              // ISSUE: cast to a reference type
              // ISSUE: cast to a reference type
              MeshUtils.Swap<float>((float&) ref normal.y, (float&) ref normal.z);
              Vector4 vector4_2 = Vector4.op_Implicit(Vector3.Cross(plane.Normal, normal));
              vector4_2.w = (__Null) 1.0;
              tangents[0].Add(vector4_2);
              vector4_2.w = (__Null) -1.0;
              tangents[1].Add(vector4_2);
            }
            if (useColors)
            {
              colors[0].Add(Color32.op_Implicit(this.crossSectionVertexColour));
              colors[1].Add(Color32.op_Implicit(this.crossSectionVertexColour));
            }
          }
          int count3 = this.polygonIndicesArray.Count;
          int index1 = count3 - 1;
          for (int index2 = 0; index2 < count3; ++index2)
          {
            triangles[0].Add(count1 + this.polygonIndicesArray[index2]);
            triangles[1].Add(count2 + this.polygonIndicesArray[index1]);
            --index1;
          }
          count1 += polygon2.Points.Length;
          count2 += polygon2.Points.Length;
        }
      }
    }

    private struct Triangle
    {
      public int[] ids;
      public Vector3[] pos;
      public Vector3[] normal;
      public Vector2[] uvs;
      public Vector4[] tangents;
      public Color32[] colors;
    }
  }
}
