// Decompiled with JetBrains decompiler
// Type: FBSAssist.TimeProgressCtrlRandom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace FBSAssist
{
  public class TimeProgressCtrlRandom : TimeProgressCtrl
  {
    private float minTime = 0.1f;
    private float maxTime = 0.2f;

    public TimeProgressCtrlRandom()
      : base(0.15f)
    {
    }

    public void Init(float min, float max)
    {
      this.minTime = min;
      this.maxTime = max;
      this.SetProgressTime(Random.Range(this.minTime, this.maxTime));
      this.Start();
    }

    public new float Calculate()
    {
      float num = base.Calculate();
      if ((double) num == 1.0)
      {
        this.SetProgressTime(Random.Range(this.minTime, this.maxTime));
        this.Start();
      }
      return num;
    }

    public float Calculate(float _minTime, float _maxTime)
    {
      this.minTime = _minTime;
      this.maxTime = _maxTime;
      return this.Calculate();
    }
  }
}
