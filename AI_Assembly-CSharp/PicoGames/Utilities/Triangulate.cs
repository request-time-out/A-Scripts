// Decompiled with JetBrains decompiler
// Type: PicoGames.Utilities.Triangulate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace PicoGames.Utilities
{
  public class Triangulate
  {
    private static Vector3[] shape;

    public static int[] Edge(Vector3[] _shape)
    {
      Triangulate.shape = _shape;
      List<int> intList = new List<int>();
      int length = Triangulate.shape.Length;
      if (length < 3)
        return intList.ToArray();
      int[] V = new int[length];
      if ((double) Triangulate.Area() > 0.0)
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
          return intList.ToArray();
        int u = v;
        if (n <= u)
          u = 0;
        v = u + 1;
        if (n <= v)
          v = 0;
        int w = v + 1;
        if (n <= w)
          w = 0;
        if (Triangulate.Snip(u, v, w, n, V))
        {
          int num3 = V[u];
          int num4 = V[v];
          int num5 = V[w];
          intList.Add(num3);
          intList.Add(num4);
          intList.Add(num5);
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
      intList.Reverse();
      return intList.ToArray();
    }

    private static float Area()
    {
      int length = Triangulate.shape.Length;
      float num = 0.0f;
      int index1 = length - 1;
      for (int index2 = 0; index2 < length; index1 = index2++)
      {
        Vector3 vector3_1 = Triangulate.shape[index1];
        Vector3 vector3_2 = Triangulate.shape[index2];
        num += (float) (vector3_1.x * vector3_2.y - vector3_2.x * vector3_1.y);
      }
      return num * 0.5f;
    }

    private static bool OverlapsPoint(Vector3 _t1, Vector3 _t2, Vector3 _t3, Vector3 _p1)
    {
      float num1 = (float) (_t3.x - _t2.x);
      float num2 = (float) (_t3.y - _t2.y);
      float num3 = (float) (_t1.x - _t3.x);
      float num4 = (float) (_t1.y - _t3.y);
      float num5 = (float) (_t2.x - _t1.x);
      float num6 = (float) (_t2.y - _t1.y);
      float num7 = (float) (_p1.x - _t1.x);
      float num8 = (float) (_p1.y - _t1.y);
      float num9 = (float) (_p1.x - _t2.x);
      float num10 = (float) (_p1.y - _t2.y);
      float num11 = (float) (_p1.x - _t3.x);
      float num12 = (float) (_p1.y - _t3.y);
      float num13 = (float) ((double) num1 * (double) num10 - (double) num2 * (double) num9);
      float num14 = (float) ((double) num5 * (double) num8 - (double) num6 * (double) num7);
      float num15 = (float) ((double) num3 * (double) num12 - (double) num4 * (double) num11);
      return (double) num13 >= 0.0 && (double) num15 >= 0.0 && (double) num14 >= 0.0;
    }

    private static bool Snip(int u, int v, int w, int n, int[] V)
    {
      Vector3 _t1 = Triangulate.shape[V[u]];
      Vector3 _t2 = Triangulate.shape[V[v]];
      Vector3 _t3 = Triangulate.shape[V[w]];
      if (Mathf.Epsilon > (_t2.x - _t1.x) * (_t3.y - _t1.y) - (_t2.y - _t1.y) * (_t3.x - _t1.x))
        return false;
      for (int index = 0; index < n; ++index)
      {
        if (index != u && index != v && index != w)
        {
          Vector3 _p1 = Triangulate.shape[V[index]];
          if (Triangulate.OverlapsPoint(_t1, _t2, _t3, _p1))
            return false;
        }
      }
      return true;
    }
  }
}
