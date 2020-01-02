// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.IEnumerableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Illusion.Extensions
{
  public static class IEnumerableExtensions
  {
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
    {
      return (IEnumerable<T>) self.OrderBy<T, Guid>((Func<T, Guid>) (_ => Guid.NewGuid()));
    }

    public static IEnumerable<T> SymmetricExcept<T>(
      this IEnumerable<T> self,
      IEnumerable<T> target)
    {
      return self.Except<T>(target).Concat<T>(target.Except<T>(self));
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, T second)
    {
      return first.Concat<T>((IEnumerable<T>) new T[1]
      {
        second
      });
    }
  }
}
