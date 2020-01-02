// Decompiled with JetBrains decompiler
// Type: LoopRateTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class LoopRateTimer
{
  private float loop;
  private float cnt;

  public void Init(float looptime)
  {
    this.loop = looptime;
  }

  public float Check()
  {
    if ((double) this.loop <= 0.0)
      return 0.0f;
    this.cnt += Time.get_deltaTime() * (180f / this.loop);
    while ((double) this.cnt >= 180.0)
      this.cnt -= 180f;
    return Mathf.Sin(this.cnt * ((float) Math.PI / 180f));
  }
}
