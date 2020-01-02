// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Rand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace AIProject.Animal
{
  public static class Rand
  {
    public static T Get<T>(List<T> source)
    {
      return source.Rand<T>();
    }

    public static KeyValuePair<T1, T2> Get<T1, T2>(Dictionary<T1, T2> source)
    {
      return source.Rand<T1, T2>();
    }

    public static T1 GetKey<T1, T2>(Dictionary<T1, T2> source)
    {
      return source.RandKey<T1, T2>();
    }

    public static T2 GetValue<T1, T2>(Dictionary<T1, T2> source)
    {
      return source.RandValue<T1, T2>();
    }
  }
}
