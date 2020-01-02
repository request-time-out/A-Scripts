// Decompiled with JetBrains decompiler
// Type: EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

public static class EnumExtensions
{
  public static bool HasFlag(this Enum self, Enum flag)
  {
    if (self.GetType() != flag.GetType())
      throw new ArgumentException("flag の型が、現在のインスタンスの型と異なっています。");
    ulong uint64_1 = Convert.ToUInt64((object) self);
    ulong uint64_2 = Convert.ToUInt64((object) flag);
    return ((long) uint64_1 & (long) uint64_2) == (long) uint64_2;
  }
}
