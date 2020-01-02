// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.Smooth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.UI.Viewer
{
  public static class Smooth
  {
    public static float Damp(
      float current,
      float target,
      ref float currentVelocity,
      float smoothTime)
    {
      return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.get_unscaledDeltaTime());
    }

    public static Vector2 Damp(
      Vector2 current,
      Vector2 target,
      ref Vector2 currentVelocity,
      float smoothTime)
    {
      return Vector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.get_unscaledDeltaTime());
    }

    public static Vector3 Damp(
      Vector3 current,
      Vector3 target,
      ref Vector3 currentVelocity,
      float smoothTime)
    {
      return Vector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.get_unscaledDeltaTime());
    }
  }
}
