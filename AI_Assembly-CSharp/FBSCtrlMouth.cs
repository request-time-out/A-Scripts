// Decompiled with JetBrains decompiler
// Type: FBSCtrlMouth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using FBSAssist;
using System;
using UnityEngine;

[Serializable]
public class FBSCtrlMouth : FBSBase
{
  [Range(0.01f, 1f)]
  public float randTimeMin = 0.1f;
  [Range(0.01f, 1f)]
  public float randTimeMax = 0.2f;
  [Range(0.1f, 2f)]
  public float randScaleMin = 0.65f;
  [Range(0.1f, 2f)]
  public float randScaleMax = 1f;
  [Range(0.0f, 1f)]
  public float openRefValue = 0.2f;
  private float sclNow = 1f;
  private float sclStart = 1f;
  private float sclEnd = 1f;
  private float adjustWidthScale = 1f;
  public bool useAjustWidthScale;
  private TimeProgressCtrlRandom tpcRand;
  public GameObject objAdjustWidthScale;

  public float GetAdjustWidthScale()
  {
    return this.adjustWidthScale;
  }

  public void Init()
  {
    base.Init();
    this.tpcRand = new TimeProgressCtrlRandom();
    this.tpcRand.Init(this.randTimeMin, this.randTimeMax);
  }

  public void CalcBlend(float openValue)
  {
    this.openRate = openValue;
    this.CalculateBlendShape();
    if (!this.useAjustWidthScale)
      return;
    this.AdjustWidthScale();
  }

  public void UseAdjustWidthScale(bool useFlags)
  {
    this.useAjustWidthScale = useFlags;
  }

  public bool AdjustWidthScale()
  {
    this.adjustWidthScale = 1f;
    bool flag = false;
    float num = this.tpcRand.Calculate(this.randTimeMin, this.randTimeMax);
    if ((double) num == 1.0)
    {
      this.sclStart = this.sclNow = this.sclEnd;
      this.sclEnd = Random.Range(this.randScaleMin, this.randScaleMax);
      flag = true;
    }
    if (flag)
      num = 0.0f;
    this.sclNow = Mathf.Lerp(this.sclStart, this.sclEnd, num);
    this.sclNow = Mathf.Max(0.0f, this.sclNow - this.openRefValue * this.openRate);
    if (0.200000002980232 < (double) this.openRate)
      this.adjustWidthScale = this.sclNow;
    if (Object.op_Inequality((Object) null, (Object) this.objAdjustWidthScale))
      this.objAdjustWidthScale.get_transform().set_localScale(new Vector3(this.adjustWidthScale, 1f, 1f));
    return true;
  }
}
