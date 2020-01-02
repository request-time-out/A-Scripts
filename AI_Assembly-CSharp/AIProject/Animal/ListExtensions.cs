// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public static class ListExtensions
  {
    public static T Rand<T>(this List<T> source)
    {
      return ((IReadOnlyList<T>) source).IsNullOrEmpty<T>() ? default (T) : source[Random.Range(0, source.Count)];
    }

    public static T GetRand<T>(this List<T> source)
    {
      if (((IReadOnlyList<T>) source).IsNullOrEmpty<T>())
        return default (T);
      int index = Random.Range(0, source.Count);
      T obj = source[index];
      source.RemoveAt(index);
      return obj;
    }

    public static T First<T>(this List<T> source)
    {
      return ((IReadOnlyList<T>) source).IsNullOrEmpty<T>() ? default (T) : source[0];
    }

    public static T Back<T>(this List<T> source)
    {
      return ((IReadOnlyList<T>) source).IsNullOrEmpty<T>() ? default (T) : source[source.Count - 1];
    }

    public static bool AddNonContains<T>(this List<T> source, T value)
    {
      if (source == null || source.Contains(value))
        return false;
      source.Add(value);
      return true;
    }

    public static bool InRange<T>(this List<T> source, int i)
    {
      return source != null && 0 <= i && i < source.Count;
    }
  }
}
