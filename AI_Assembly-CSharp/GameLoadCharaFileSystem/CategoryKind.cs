// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.CategoryKind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace GameLoadCharaFileSystem
{
  [Flags]
  public enum CategoryKind
  {
    Male = 1,
    Female = 2,
    MyData = 4,
    Download = 8,
    Preset = 16, // 0x00000010
  }
}
