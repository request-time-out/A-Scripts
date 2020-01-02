// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.FlavorSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.Definitions
{
  public static class FlavorSkill
  {
    public static Dictionary<int, string> NameTable { get; } = new Dictionary<int, string>()
    {
      [0] = "女子力",
      [1] = "信頼",
      [2] = "人間性",
      [3] = "本能",
      [4] = "変態",
      [5] = "危機管理",
      [6] = "闇",
      [7] = "社交性"
    };

    public enum Type
    {
      Pheromone,
      Reliability,
      Reason,
      Instinct,
      Dirty,
      Wariness,
      Darkness,
      Sociability,
    }
  }
}
