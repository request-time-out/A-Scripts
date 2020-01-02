// Decompiled with JetBrains decompiler
// Type: RandomTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class RandomTimer
{
  private float randMin = 1000f;
  private float randMax = 1000f;
  private float next;
  private float cnt;

  public void Init(float min, float max)
  {
    this.randMin = min;
    this.randMax = max;
    this.next = Random.Range(this.randMin, this.randMax);
  }

  public bool Check()
  {
    this.cnt += Time.get_deltaTime();
    if ((double) this.cnt < (double) this.next)
      return false;
    this.next = Random.Range(this.randMin, this.randMax);
    this.cnt = 0.0f;
    return true;
  }
}
