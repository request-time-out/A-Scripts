// Decompiled with JetBrains decompiler
// Type: IllusionUtility.GetUtility.EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace IllusionUtility.GetUtility
{
  public static class EnumExtensions
  {
    public static bool HasFlag(this Enum self, Enum flag)
    {
      return (long) self.AND(flag) == (long) Convert.ToUInt64((object) flag);
    }

    public static ulong Add(this Enum self, Enum flag)
    {
      return self.OR(flag);
    }

    public static ulong Sub(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) & flag.NOT();
    }

    public static ulong Get(this Enum self, Enum flag)
    {
      return self.AND(flag);
    }

    public static ulong Reverse(this Enum self, Enum flag)
    {
      return self.XOR(flag);
    }

    public static ulong NOT(this Enum self)
    {
      return ~Convert.ToUInt64((object) self);
    }

    public static ulong AND(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) & Convert.ToUInt64((object) flag);
    }

    public static ulong OR(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) | Convert.ToUInt64((object) flag);
    }

    public static ulong XOR(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) ^ Convert.ToUInt64((object) flag);
    }
  }
}
