// Decompiled with JetBrains decompiler
// Type: Exploder.Polygon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PrimitivesPro.ThirdParty.P2T;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  public class Polygon
  {
    public Vector2[] Points;
    public readonly float Area;
    public Vector2 Min;
    public Vector2 Max;
    private readonly List<Polygon> holes;

    public Polygon(Vector2[] pnts)
    {
      this.Points = pnts;
      this.Area = this.GetArea();
      this.holes = new List<Polygon>();
    }

    public float GetArea()
    {
      this.Min.x = (__Null) 3.40282346638529E+38;
      this.Min.y = (__Null) 3.40282346638529E+38;
      this.Max.x = (__Null) -3.40282346638529E+38;
      this.Max.y = (__Null) -3.40282346638529E+38;
      int length = this.Points.Length;
      float num = 0.0f;
      int index1 = length - 1;
      for (int index2 = 0; index2 < length; index1 = index2++)
      {
        Vector2 point1 = this.Points[index1];
        Vector2 point2 = this.Points[index2];
        num += (float) (point1.x * point2.y - point2.x * point1.y);
        if (point1.x < this.Min.x)
          this.Min.x = point1.x;
        if (point1.y < this.Min.y)
          this.Min.y = point1.y;
        if (point1.x > this.Max.x)
          this.Max.x = point1.x;
        if (point1.y > this.Max.y)
          this.Max.y = point1.y;
      }
      return num * 0.5f;
    }

    public bool IsPointInside(Vector3 p)
    {
      int length = this.Points.Length;
      Vector2 vector2 = this.Points[length - 1];
      bool flag1 = vector2.y >= p.y;
      Vector2.get_zero();
      bool flag2 = false;
      for (int index = 0; index < length; ++index)
      {
        Vector2 point = this.Points[index];
        bool flag3 = point.y >= p.y;
        if (flag1 != flag3 && (point.y - p.y) * (vector2.x - point.x) >= (point.x - p.x) * (vector2.y - point.y) == flag3)
          flag2 = !flag2;
        flag1 = flag3;
        vector2 = point;
      }
      return flag2;
    }

    public bool IsPolygonInside(Polygon polygon)
    {
      return (double) this.Area > (double) polygon.Area && this.IsPointInside(Vector2.op_Implicit(polygon.Points[0]));
    }

    public void AddHole(Polygon polygon)
    {
      this.holes.Add(polygon);
    }

    public bool Triangulate(Array<int> indicesArray)
    {
      if (this.holes.Count == 0)
      {
        indicesArray.Initialize(this.Points.Length * 3);
        int length = this.Points.Length;
        if (length < 3)
          return true;
        int[] V = new int[length];
        if ((double) this.Area > 0.0)
        {
          for (int index = 0; index < length; ++index)
            V[index] = index;
        }
        else
        {
          for (int index = 0; index < length; ++index)
            V[index] = length - 1 - index;
        }
        int n = length;
        int num1 = 2 * n;
        int num2 = 0;
        int v = n - 1;
        while (n > 2)
        {
          if (num1-- <= 0)
            return true;
          int u = v;
          if (n <= u)
            u = 0;
          v = u + 1;
          if (n <= v)
            v = 0;
          int w = v + 1;
          if (n <= w)
            w = 0;
          if (this.Snip(u, v, w, n, V))
          {
            int data1 = V[u];
            int data2 = V[v];
            int data3 = V[w];
            indicesArray.Add(data1);
            indicesArray.Add(data2);
            indicesArray.Add(data3);
            ++num2;
            int index1 = v;
            for (int index2 = v + 1; index2 < n; ++index2)
            {
              V[index1] = V[index2];
              ++index1;
            }
            --n;
            num1 = 2 * n;
          }
        }
        indicesArray.Reverse();
        return true;
      }
      List<PolygonPoint> polygonPointList1 = new List<PolygonPoint>(this.Points.Length);
      foreach (Vector2 point in this.Points)
        polygonPointList1.Add(new PolygonPoint((double) point.x, (double) point.y));
      Polygon polygon = new Polygon((IList<PolygonPoint>) polygonPointList1);
      foreach (Polygon hole in this.holes)
      {
        List<PolygonPoint> polygonPointList2 = new List<PolygonPoint>(hole.Points.Length);
        foreach (Vector2 point in hole.Points)
          polygonPointList2.Add(new PolygonPoint((double) point.x, (double) point.y));
        polygon.AddHole(new Polygon((IList<PolygonPoint>) polygonPointList2));
      }
      try
      {
        PrimitivesPro.ThirdParty.P2T.P2T.Triangulate(polygon);
      }
      catch (Exception ex)
      {
        return false;
      }
      int count = ((ICollection<DelaunayTriangle>) polygon.get_Triangles()).Count;
      indicesArray.Initialize(count * 3);
      this.Points = new Vector2[count * 3];
      int data = 0;
      this.Min.x = (__Null) 3.40282346638529E+38;
      this.Min.y = (__Null) 3.40282346638529E+38;
      this.Max.x = (__Null) -3.40282346638529E+38;
      this.Max.y = (__Null) -3.40282346638529E+38;
      for (int index1 = 0; index1 < count; ++index1)
      {
        indicesArray.Add(data);
        indicesArray.Add(data + 1);
        indicesArray.Add(data + 2);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data + 2].x = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._0).get_X();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data + 2].y = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._0).get_Y();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data + 1].x = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._1).get_X();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data + 1].y = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._1).get_Y();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data].x = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._2).get_X();
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.Points[data].y = (__Null) ((Point2D) (^(FixedArray3<TriangulationPoint>&) ref polygon.get_Triangles()[index1].Points)._2).get_Y();
        for (int index2 = 0; index2 < 3; ++index2)
        {
          if (this.Points[data + index2].x < this.Min.x)
            this.Min.x = this.Points[data + index2].x;
          if (this.Points[data + index2].y < this.Min.y)
            this.Min.y = this.Points[data + index2].y;
          if (this.Points[data + index2].x > this.Max.x)
            this.Max.x = this.Points[data + index2].x;
          if (this.Points[data + index2].y > this.Max.y)
            this.Max.y = this.Points[data + index2].y;
        }
        data += 3;
      }
      return true;
    }

    private bool Snip(int u, int v, int w, int n, int[] V)
    {
      Vector2 point1 = this.Points[V[u]];
      Vector2 point2 = this.Points[V[v]];
      Vector2 point3 = this.Points[V[w]];
      if (Mathf.Epsilon > (point2.x - point1.x) * (point3.y - point1.y) - (point2.y - point1.y) * (point3.x - point1.x))
        return false;
      for (int index = 0; index < n; ++index)
      {
        if (index != u && index != v && index != w)
        {
          Vector2 point4 = this.Points[V[index]];
          float num1 = (float) (point3.x - point2.x);
          float num2 = (float) (point3.y - point2.y);
          float num3 = (float) (point1.x - point3.x);
          float num4 = (float) (point1.y - point3.y);
          float num5 = (float) (point2.x - point1.x);
          float num6 = (float) (point2.y - point1.y);
          float num7 = (float) (point4.x - point1.x);
          float num8 = (float) (point4.y - point1.y);
          float num9 = (float) (point4.x - point2.x);
          float num10 = (float) (point4.y - point2.y);
          float num11 = (float) (point4.x - point3.x);
          float num12 = (float) (point4.y - point3.y);
          float num13 = (float) ((double) num1 * (double) num10 - (double) num2 * (double) num9);
          float num14 = (float) ((double) num5 * (double) num8 - (double) num6 * (double) num7);
          float num15 = (float) ((double) num3 * (double) num12 - (double) num4 * (double) num11);
          if ((double) num13 >= 0.0 && (double) num15 >= 0.0 && (double) num14 >= 0.0)
            return false;
        }
      }
      return true;
    }

    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
      float num1 = (float) (C.x - B.x);
      float num2 = (float) (C.y - B.y);
      float num3 = (float) (A.x - C.x);
      float num4 = (float) (A.y - C.y);
      float num5 = (float) (B.x - A.x);
      float num6 = (float) (B.y - A.y);
      float num7 = (float) (P.x - A.x);
      float num8 = (float) (P.y - A.y);
      float num9 = (float) (P.x - B.x);
      float num10 = (float) (P.y - B.y);
      float num11 = (float) (P.x - C.x);
      float num12 = (float) (P.y - C.y);
      float num13 = (float) ((double) num1 * (double) num10 - (double) num2 * (double) num9);
      float num14 = (float) ((double) num5 * (double) num8 - (double) num6 * (double) num7);
      float num15 = (float) ((double) num3 * (double) num12 - (double) num4 * (double) num11);
      return (double) num13 >= 0.0 && (double) num15 >= 0.0 && (double) num14 >= 0.0;
    }
  }
}
