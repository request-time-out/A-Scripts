// Decompiled with JetBrains decompiler
// Type: YS_Timer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class YS_Timer : MonoBehaviour
{
  public float time;
  public float rate;
  private float cnt;

  public YS_Timer()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
    this.cnt += Time.get_deltaTime();
    while ((double) this.cnt > (double) this.time)
      this.cnt -= this.time;
    this.rate = this.cnt / this.time;
  }
}
