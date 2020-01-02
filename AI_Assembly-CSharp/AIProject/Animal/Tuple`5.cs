// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Tuple`5
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject.Animal
{
  [Serializable]
  public class Tuple<T1, T2, T3, T4, T5>
  {
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;

    public Tuple(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5)
    {
      this.Item1 = i1;
      this.Item2 = i2;
      this.Item3 = i3;
      this.Item4 = i4;
      this.Item5 = i5;
    }
  }
}
