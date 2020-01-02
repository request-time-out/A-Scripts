// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Sickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AIProject.Definitions
{
  public static class Sickness
  {
    public const int GoodHealthID = -1;
    public const int ColdID = 0;
    public const int StomachacheID = 1;
    public const int OverworkID = 2;
    public const int HeatStrokeID = 3;
    public const int HurtID = 4;

    public static ReadOnlyDictionary<int, string> NameTable { get; } = new ReadOnlyDictionary<int, string>((IDictionary<int, string>) new Dictionary<int, string>()
    {
      [0] = "風邪",
      [1] = "腹痛",
      [2] = "過労",
      [3] = "熱中",
      [4] = "ケガ"
    });

    public static Dictionary<string, int> TagTable { get; } = new Dictionary<string, int>()
    {
      ["cold"] = 0,
      ["stomach"] = 1,
      ["overwork"] = 2,
      ["heatstroke"] = 3,
      ["drunkenness"] = 4
    };
  }
}
