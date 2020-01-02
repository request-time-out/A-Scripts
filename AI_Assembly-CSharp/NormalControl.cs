// Decompiled with JetBrains decompiler
// Type: NormalControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class NormalControl
{
  public static void GetNormalData(GameObject obj, List<Vector3> normal)
  {
    if (Object.op_Equality((Object) null, (Object) obj))
      return;
    MeshFilter meshFilter = new MeshFilter();
    MeshFilter component1 = obj.GetComponent(typeof (MeshFilter)) as MeshFilter;
    if (Object.op_Inequality((Object) null, (Object) component1))
    {
      foreach (Vector3 normal1 in component1.get_sharedMesh().get_normals())
        normal.Add(normal1);
    }
    else
    {
      SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
      SkinnedMeshRenderer component2 = obj.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if (!Object.op_Implicit((Object) component2))
        return;
      foreach (Vector3 normal1 in component2.get_sharedMesh().get_normals())
        normal.Add(normal1);
    }
  }
}
