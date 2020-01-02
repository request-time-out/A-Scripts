// Decompiled with JetBrains decompiler
// Type: WaitForSecondsRealtime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public sealed class WaitForSecondsRealtime : CustomYieldInstruction
{
  private float waitTime;

  public WaitForSecondsRealtime(float time)
  {
    this.\u002Ector();
    this.waitTime = Time.get_realtimeSinceStartup() + time;
  }

  public virtual bool keepWaiting
  {
    get
    {
      return (double) Time.get_realtimeSinceStartup() < (double) this.waitTime;
    }
  }
}
