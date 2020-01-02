// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Tuple`8
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject.Animal
{
  [Serializable]
  public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8>
  {
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;
    public T7 Item7;
    public T8 Item8;

    public Tuple(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
    {
      this.Item1 = i1;
      this.Item2 = i2;
      this.Item3 = i3;
      this.Item4 = i4;
      this.Item5 = i5;
      this.Item6 = i6;
      this.Item7 = i7;
      this.Item8 = i8;
    }
  }
}
