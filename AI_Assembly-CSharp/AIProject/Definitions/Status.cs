// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Status
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.Definitions
{
  public static class Status
  {
    public static Dictionary<int, string> Names { get; } = new Dictionary<int, string>()
    {
      [0] = "体温",
      [1] = "機嫌",
      [2] = "満腹",
      [3] = "体調",
      [4] = "生命維持[存在しない]",
      [5] = "やる気",
      [6] = "Hな気分",
      [7] = "善悪"
    };

    public enum Type
    {
      Temperature,
      Mood,
      Hunger,
      Physical,
      Life,
      Motivation,
      Immoral,
      Morality,
    }
  }
}
