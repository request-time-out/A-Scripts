// Decompiled with JetBrains decompiler
// Type: AIProject.ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace AIProject
{
  public static class ListExtensions
  {
    public static void PushFront<T>(this IList<T> self, T item)
    {
      self.Insert(0, item);
    }

    public static T PopFront<T>(this IList<T> self)
    {
      if (self.IsNullOrEmpty<T>())
        return default (T);
      T obj = self[0];
      self.RemoveAt(0);
      return obj;
    }

    public static void PushBack<T>(this IList<T> self, T item)
    {
      self.Add(item);
    }

    public static T PopBack<T>(this IList<T> self)
    {
      if (self.IsNullOrEmpty<T>())
        return default (T);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      \u003C\u003E__AnonType10<T, int> anonType10 = self.Select<T, \u003C\u003E__AnonType10<T, int>>((Func<T, int, \u003C\u003E__AnonType10<T, int>>) ((value, index) => new \u003C\u003E__AnonType10<T, int>(value, index))).Last<\u003C\u003E__AnonType10<T, int>>();
      self.RemoveAt(anonType10.index);
      return anonType10.value;
    }
  }
}
