// Decompiled with JetBrains decompiler
// Type: MeshBrush.VectorClampingUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace MeshBrush
{
  public static class VectorClampingUtility
  {
    public static void ClampVector(
      ref Vector2 vector,
      float minX,
      float maxX,
      float minY,
      float maxY)
    {
      vector.x = (__Null) (double) Mathf.Clamp((float) vector.x, minX, maxX);
      vector.y = (__Null) (double) Mathf.Clamp((float) vector.y, minY, maxY);
    }

    public static void ClampVector(
      ref Vector3 vector,
      float minX,
      float maxX,
      float minY,
      float maxY,
      float minZ,
      float maxZ)
    {
      vector.x = (__Null) (double) Mathf.Clamp((float) vector.x, minX, maxX);
      vector.y = (__Null) (double) Mathf.Clamp((float) vector.y, minY, maxY);
      vector.z = (__Null) (double) Mathf.Clamp((float) vector.z, minZ, maxZ);
    }

    public static void ClampVector(
      ref Vector4 vector,
      float minX,
      float maxX,
      float minY,
      float maxY,
      float minZ,
      float maxZ,
      float minW,
      float maxW)
    {
      vector.x = (__Null) (double) Mathf.Clamp((float) vector.x, minX, maxX);
      vector.y = (__Null) (double) Mathf.Clamp((float) vector.y, minY, maxY);
      vector.z = (__Null) (double) Mathf.Clamp((float) vector.z, minZ, maxZ);
      vector.w = (__Null) (double) Mathf.Clamp((float) vector.w, minW, maxW);
    }
  }
}
