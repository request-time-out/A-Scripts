// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Vector2Extensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public static class Vector2Extensions
  {
    public static float Min(this Vector2 vec)
    {
      return Mathf.Min((float) vec.x, (float) vec.y);
    }

    public static float Max(this Vector2 vec)
    {
      return Mathf.Max((float) vec.x, (float) vec.y);
    }

    public static float RandomRange(this Vector2 vec)
    {
      return Random.Range((float) vec.x, (float) vec.y);
    }

    public static bool Range(this Vector2 vec, float value)
    {
      return vec.x <= (double) value && (double) value <= vec.y;
    }
  }
}
