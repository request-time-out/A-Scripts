// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.CollectionExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.Animal
{
  public static class CollectionExtensions
  {
    public static bool IsNullOrEmpty<T>(this IReadOnlyList<T> source)
    {
      return source == null || ((IReadOnlyCollection<T>) source).get_Count() == 0;
    }

    public static bool IsNullOrEmpty<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
    {
      return source == null || ((IReadOnlyCollection<KeyValuePair<TKey, TValue>>) source).get_Count() == 0;
    }
  }
}
