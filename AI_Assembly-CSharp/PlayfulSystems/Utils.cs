// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PlayfulSystems
{
  public static class Utils
  {
    public static float EaseSinInOut(float lerp, float start, float change)
    {
      return (float) (-(double) change / 2.0 * ((double) Mathf.Cos(3.141593f * lerp) - 1.0)) + start;
    }
  }
}
