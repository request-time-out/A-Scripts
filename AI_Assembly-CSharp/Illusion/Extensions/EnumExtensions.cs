// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Diagnostics;
using System.Text;

namespace Illusion.Extensions
{
  public static class EnumExtensions
  {
    [Conditional("UNITY_ASSERTIONS")]
    private static void Check(Enum self, Enum flag)
    {
    }

    public static bool HasFlag(this Enum self, Enum flag)
    {
      ulong uint64 = Convert.ToUInt64((object) flag);
      return (long) self.AND(uint64) == (long) uint64;
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

    public static ulong AND(this Enum self, ulong flag)
    {
      return Convert.ToUInt64((object) self) & flag;
    }

    public static ulong OR(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) | Convert.ToUInt64((object) flag);
    }

    public static ulong OR(this Enum self, ulong flag)
    {
      return Convert.ToUInt64((object) self) | flag;
    }

    public static ulong XOR(this Enum self, Enum flag)
    {
      return Convert.ToUInt64((object) self) ^ Convert.ToUInt64((object) flag);
    }

    public static ulong XOR(this Enum self, ulong flag)
    {
      return Convert.ToUInt64((object) self) ^ flag;
    }

    public static string LabelFormat(this Enum self, string label)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in Enum.GetValues(self.GetType()))
      {
        if (self.HasFlag((Enum) obj))
          stringBuilder.AppendFormat("{0} | ", obj);
      }
      return stringBuilder.Length == 0 ? string.Empty : label + (object) stringBuilder;
    }

    public static bool All(this Enum self)
    {
      return self.Reverse(self.Everything()) == 0UL;
    }

    public static bool Any(this Enum self)
    {
      return (long) Convert.ToUInt64((object) self) != (long) self.Nothing();
    }

    public static Enum Everything(this Enum self)
    {
      ulong num = 0;
      foreach (object obj in Enum.GetValues(self.GetType()))
        num += Convert.ToUInt64(obj);
      return (Enum) Enum.ToObject(self.GetType(), num);
    }

    public static ulong Nothing(this Enum self)
    {
      return 0;
    }

    public static ulong Normalize(this Enum self)
    {
      return (ulong) Enum.ToObject(self.GetType(), Convert.ToInt64((object) self) & Convert.ToInt64((object) self.Everything()));
    }
  }
}
