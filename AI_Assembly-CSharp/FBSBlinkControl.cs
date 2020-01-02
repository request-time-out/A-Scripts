// Decompiled with JetBrains decompiler
// Type: FBSBlinkControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class FBSBlinkControl
{
  [Range(0.0f, 255f)]
  public byte BlinkFrequency = 30;
  [Range(0.0f, 0.5f)]
  public float BaseSpeed = 0.15f;
  private float openRate = 1f;
  private byte fixedFlags;
  private sbyte blinkMode;
  private float calcSpeed;
  private float blinkTime;
  private int count;

  public void SetFixedFlags(byte flags)
  {
    this.fixedFlags = flags;
  }

  public byte GetFixedFlags()
  {
    return this.fixedFlags;
  }

  public void SetFrequency(byte value)
  {
    this.BlinkFrequency = value;
    if (this.blinkMode != (sbyte) 0)
      return;
    this.blinkTime = Time.get_time() + 0.2f * Mathf.Lerp(0.0f, (float) this.BlinkFrequency, Mathf.InverseLerp(0.0f, (float) this.BlinkFrequency, (float) Random.Range(0, (int) this.BlinkFrequency)));
  }

  public void SetSpeed(float value)
  {
    this.BaseSpeed = Mathf.Max(1f, value);
  }

  public void SetForceOpen()
  {
    this.calcSpeed = this.BaseSpeed + Random.Range(0.0f, 0.05f);
    this.blinkTime = Time.get_time() + this.calcSpeed;
    this.blinkMode = (sbyte) -1;
  }

  public void SetForceClose()
  {
    this.calcSpeed = this.BaseSpeed + Random.Range(0.0f, 0.05f);
    this.blinkTime = Time.get_time() + this.calcSpeed;
    this.count = Random.Range(0, 3) + 1;
    this.blinkMode = (sbyte) 1;
  }

  public void CalcBlink()
  {
    float num1 = Mathf.Max(0.0f, this.blinkTime - Time.get_time());
    float num2;
    switch ((int) this.blinkMode + 1)
    {
      case 1:
        num2 = 1f;
        break;
      case 2:
        num2 = Mathf.Clamp(num1 / this.calcSpeed, 0.0f, 1f);
        break;
      default:
        num2 = Mathf.Clamp((float) (1.0 - (double) num1 / (double) this.calcSpeed), 0.0f, 1f);
        break;
    }
    if (this.fixedFlags == (byte) 0)
      this.openRate = num2;
    if (this.fixedFlags != (byte) 0 || (double) Time.get_time() <= (double) this.blinkTime)
      return;
    switch ((int) this.blinkMode + 1)
    {
      case 0:
        this.blinkTime = Time.get_time() + 0.2f * Mathf.Lerp(0.0f, (float) this.BlinkFrequency, Mathf.InverseLerp(0.0f, (float) this.BlinkFrequency, (float) Random.Range(0, (int) this.BlinkFrequency)));
        this.blinkMode = (sbyte) 0;
        break;
      case 1:
        this.SetForceClose();
        break;
      case 2:
        --this.count;
        if (0 < this.count)
          break;
        this.SetForceOpen();
        break;
    }
  }

  public float GetOpenRate()
  {
    return this.openRate;
  }
}
