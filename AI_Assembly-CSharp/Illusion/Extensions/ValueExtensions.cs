// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.ValueExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace Illusion.Extensions
{
  public static class ValueExtensions
  {
    public static int Check<T>(this IList<T> list, Func<T, bool> func)
    {
      return Utils.Value.Check(list.Count, (Func<int, bool>) (index => func(list[index])));
    }

    public static int Check<T>(this List<T> list, Func<T, bool> func)
    {
      return Utils.Value.Check(list.Count, (Func<int, bool>) (index => func(list[index])));
    }

    public static int Check<T>(this T[] array, Func<T, bool> func)
    {
      return Utils.Value.Check(array.Length, (Func<int, bool>) (index => func(array[index])));
    }

    public static int Check<T>(this IList<T> list, T value)
    {
      return Utils.Value.Check(list.Count, (Func<int, bool>) (index => list[index].Equals((object) (T) value)));
    }

    public static int Check<T>(this List<T> list, T value)
    {
      return Utils.Value.Check(list.Count, (Func<int, bool>) (index => list[index].Equals((object) (T) value)));
    }

    public static int Check<T>(this T[] array, T value)
    {
      return Utils.Value.Check(array.Length, (Func<int, bool>) (index => array[index].Equals((object) (T) value)));
    }
  }
}
