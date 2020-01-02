// Decompiled with JetBrains decompiler
// Type: AIProject.PoseType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  [Flags]
  public enum PoseType
  {
    Stand = 1,
    Floor = 2,
    Sit = 4,
    Recline = 8,
    PairF2F = 16, // 0x00000010
    PairSxS = 32, // 0x00000020
  }
}
