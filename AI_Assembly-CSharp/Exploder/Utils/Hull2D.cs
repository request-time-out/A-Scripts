// Decompiled with JetBrains decompiler
// Type: Exploder.Utils.Hull2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder.Utils
{
  internal class Hull2D
  {
    public static void Sort(Vector2[] array)
    {
      Array.Sort<Vector2>(array, (Comparison<Vector2>) ((value0, value1) =>
      {
        // ISSUE: explicit non-virtual call
        int num = __nonvirtual (value0.x.CompareTo((float) value1.x));
        // ISSUE: explicit non-virtual call
        return num != 0 ? num : __nonvirtual (value0.y.CompareTo((float) value1.y));
      }));
    }

    public static void DumpArray(Vector2[] array)
    {
      foreach (Vector2 vector2 in array)
        Debug.Log((object) ("V: " + (object) vector2));
    }

    public static Vector2[] ChainHull2D(Vector2[] Pnts)
    {
      int length1 = Pnts.Length;
      int length2 = 0;
      Hull2D.Sort(Pnts);
      Vector2[] vector2Array1 = new Vector2[2 * length1];
      for (int index = 0; index < length1; ++index)
      {
        while (length2 >= 2 && (double) Hull2D.Hull2DCross(ref vector2Array1[length2 - 2], ref vector2Array1[length2 - 1], ref Pnts[index]) <= 0.0)
          --length2;
        vector2Array1[length2++] = Pnts[index];
      }
      int index1 = length1 - 2;
      int num = length2 + 1;
      for (; index1 >= 0; --index1)
      {
        while (length2 >= num && (double) Hull2D.Hull2DCross(ref vector2Array1[length2 - 2], ref vector2Array1[length2 - 1], ref Pnts[index1]) <= 0.0)
          --length2;
        vector2Array1[length2++] = Pnts[index1];
      }
      Vector2[] vector2Array2 = new Vector2[length2];
      for (int index2 = 0; index2 < length2; ++index2)
        vector2Array2[index2] = vector2Array1[index2];
      return vector2Array2;
    }

    private static float Hull2DCross(ref Vector2 O, ref Vector2 A, ref Vector2 B)
    {
      return (float) ((A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x));
    }
  }
}
