// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ArrayExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public static class ArrayExtensions
  {
    public static T Rand<T>(this T[] source)
    {
      return ((IReadOnlyList<T>) source).IsNullOrEmpty<T>() ? default (T) : source[Random.Range(0, source.Length)];
    }
  }
}
