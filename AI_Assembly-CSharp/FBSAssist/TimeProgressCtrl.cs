// Decompiled with JetBrains decompiler
// Type: FBSAssist.TimeProgressCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace FBSAssist
{
  public class TimeProgressCtrl
  {
    private float rate = 1f;
    private float progressTime = 0.15f;
    private float count;

    public TimeProgressCtrl(float ptime = 0.15f)
    {
      this.progressTime = ptime;
    }

    public void End()
    {
      this.count = this.progressTime;
      this.rate = 1f;
    }

    public void Start()
    {
      this.count = 0.0f;
      this.rate = 0.0f;
    }

    public float Calculate()
    {
      this.count += Time.get_deltaTime();
      if ((double) this.count < (double) this.progressTime)
        this.rate = Mathf.InverseLerp(0.0f, this.progressTime, this.count);
      else
        this.End();
      return this.rate;
    }

    public void SetProgressTime(float time)
    {
      this.progressTime = time;
    }

    public float GetProgressRate()
    {
      return this.rate;
    }
  }
}
