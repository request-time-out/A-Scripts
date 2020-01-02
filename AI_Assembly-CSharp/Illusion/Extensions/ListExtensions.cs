// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace Illusion.Extensions
{
  public static class ListExtensions
  {
    public static T Peek<T>(this IList<T> self)
    {
      return self[0];
    }

    public static T Pop<T>(this IList<T> self)
    {
      T obj = self[0];
      self.RemoveAt(0);
      return obj;
    }

    public static void Push<T>(this IList<T> self, T item)
    {
      self.Insert(0, item);
    }

    public static T Dequeue<T>(this IList<T> self)
    {
      T obj = self[0];
      self.RemoveAt(0);
      return obj;
    }

    public static void Enqueue<T>(this IList<T> self, T item)
    {
      self.Add(item);
    }
  }
}
