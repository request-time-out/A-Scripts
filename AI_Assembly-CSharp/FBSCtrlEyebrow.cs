// Decompiled with JetBrains decompiler
// Type: FBSCtrlEyebrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

[Serializable]
public class FBSCtrlEyebrow : FBSBase
{
  public bool SyncBlink = true;

  public void CalcBlend(float blinkRate)
  {
    if (0.0 <= (double) blinkRate && this.SyncBlink)
      this.openRate = blinkRate;
    this.CalculateBlendShape();
  }
}
