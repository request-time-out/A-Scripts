// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Tuple`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject.Animal
{
  [Serializable]
  public class Tuple<T1, T2>
  {
    public T1 Item1;
    public T2 Item2;

    public Tuple(T1 i1, T2 i2)
    {
      this.Item1 = i1;
      this.Item2 = i2;
    }
  }
}
