// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.IntExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Illusion.Extensions
{
  public static class IntExtensions
  {
    public static string MinusThroughToString(this int self, string format)
    {
      return self >= 0 ? self.ToString(format) : self.ToString();
    }

    public static int ValueRound(this int self, int add)
    {
      if (add == 0)
        return self;
      int num = self;
      self += add;
      if (add > 0 && self < num)
        self = int.MaxValue;
      else if (add < 0 && self > num)
        self = int.MinValue;
      return self;
    }
  }
}
