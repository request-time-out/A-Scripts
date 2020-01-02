// Decompiled with JetBrains decompiler
// Type: ShuffleRand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

public class ShuffleRand
{
  private int[] no;
  private int cnt;

  public ShuffleRand(int num = -1)
  {
    if (num == -1)
      return;
    this.Init(num);
  }

  public void Init(int num)
  {
    this.no = new int[num];
    this.Shuffle();
  }

  private void Shuffle()
  {
    if (this.no.Length == 0)
      return;
    int[] numArray = new int[this.no.Length];
    for (int index = 0; index < this.no.Length; ++index)
      numArray[index] = index;
    this.no = ((IEnumerable<int>) numArray).OrderBy<int, Guid>((Func<int, Guid>) (i => Guid.NewGuid())).ToArray<int>();
    this.cnt = 0;
  }

  public int Get()
  {
    if (this.no.Length == 0)
      return -1;
    if (this.cnt >= this.no.Length)
      this.Shuffle();
    return this.no[this.cnt++];
  }
}
