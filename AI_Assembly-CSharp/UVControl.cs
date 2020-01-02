// Decompiled with JetBrains decompiler
// Type: UVControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UVControl
{
  public static void GetUVData(GameObject obj, List<Vector2> uv, int index)
  {
    if (Object.op_Equality((Object) null, (Object) obj))
      return;
    MeshFilter meshFilter = new MeshFilter();
    MeshFilter component1 = obj.GetComponent(typeof (MeshFilter)) as MeshFilter;
    if (Object.op_Inequality((Object) null, (Object) component1))
    {
      switch (index)
      {
        case 0:
          foreach (Vector2 vector2 in component1.get_sharedMesh().get_uv())
            uv.Add(vector2);
          break;
        case 1:
          foreach (Vector2 vector2 in component1.get_sharedMesh().get_uv2())
            uv.Add(vector2);
          break;
        case 2:
          foreach (Vector2 vector2 in component1.get_sharedMesh().get_uv3())
            uv.Add(vector2);
          break;
        case 3:
          foreach (Vector2 vector2 in component1.get_sharedMesh().get_uv4())
            uv.Add(vector2);
          break;
      }
    }
    else
    {
      SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
      SkinnedMeshRenderer component2 = obj.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if (!Object.op_Implicit((Object) component2))
        return;
      switch (index)
      {
        case 0:
          foreach (Vector2 vector2 in component2.get_sharedMesh().get_uv())
            uv.Add(vector2);
          break;
        case 1:
          foreach (Vector2 vector2 in component2.get_sharedMesh().get_uv2())
            uv.Add(vector2);
          break;
        case 2:
          foreach (Vector2 vector2 in component2.get_sharedMesh().get_uv3())
            uv.Add(vector2);
          break;
        case 3:
          foreach (Vector2 vector2 in component2.get_sharedMesh().get_uv4())
            uv.Add(vector2);
          break;
      }
    }
  }

  public static void GetRangeIndex(GameObject obj, out int[] rangeIndex)
  {
    rangeIndex = (int[]) null;
    if (Object.op_Equality((Object) null, (Object) obj))
      return;
    List<int> intList = new List<int>();
    MeshFilter meshFilter = new MeshFilter();
    MeshFilter component1 = obj.GetComponent(typeof (MeshFilter)) as MeshFilter;
    if (Object.op_Inequality((Object) null, (Object) component1))
    {
      for (int index = 0; index < component1.get_sharedMesh().get_colors().Length; ++index)
      {
        if (component1.get_sharedMesh().get_colors()[index].r == 1.0)
          intList.Add(index);
      }
    }
    else
    {
      SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
      SkinnedMeshRenderer component2 = obj.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if (Object.op_Implicit((Object) component2))
      {
        for (int index = 0; index < component2.get_sharedMesh().get_colors().Length; ++index)
        {
          if (component2.get_sharedMesh().get_colors()[index].r == 1.0)
            intList.Add(index);
        }
      }
    }
    if (intList.Count != 0)
    {
      rangeIndex = new int[intList.Count];
      for (int index = 0; index < intList.Count; ++index)
        rangeIndex[index] = intList[index];
    }
    else
      rangeIndex = (int[]) null;
  }
}
