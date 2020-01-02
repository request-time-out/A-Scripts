// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.BitArrayExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;

namespace Illusion.Extensions
{
  public static class BitArrayExtensions
  {
    public static bool Any(this BitArray array)
    {
      foreach (bool flag in array)
      {
        if (flag)
          return true;
      }
      return false;
    }

    public static bool All(this BitArray array)
    {
      foreach (bool flag in array)
      {
        if (!flag)
          return false;
      }
      return true;
    }

    public static bool None(this BitArray array)
    {
      return !array.Any();
    }

    public static void Flip(this BitArray array, int index)
    {
      array.Set(index, !array.Get(index));
    }
  }
}
