// Decompiled with JetBrains decompiler
// Type: Morphing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Morphing : MonoBehaviour
{
  public MorphBlinkControl BlinkCtrl;
  public MorphCtrlEyebrow EyebrowCtrl;
  public MorphCtrlEyes EyesCtrl;
  public MorphCtrlMouth MouthCtrl;
  private float voiceValue;
  public EyeLookController EyeLookController;
  [Range(0.0f, 1f)]
  public float EyeLookUpCorrect;
  [Range(0.0f, 1f)]
  public float EyeLookDownCorrect;
  [Range(0.0f, 1f)]
  public float EyeLookSideCorrect;

  public Morphing()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    List<MorphingTargetInfo> MorphTargetList = new List<MorphingTargetInfo>();
    MorphTargetList.Clear();
    this.EyebrowCtrl.Init(MorphTargetList);
    this.EyesCtrl.Init(MorphTargetList);
    this.MouthCtrl.Init(MorphTargetList);
  }

  private void Start()
  {
  }

  private void LateUpdate()
  {
    this.BlinkCtrl.CalcBlink();
    float blinkRate;
    if (this.BlinkCtrl.GetFixedFlags() == (byte) 0)
    {
      blinkRate = this.BlinkCtrl.GetOpenRate();
      if (Object.op_Implicit((Object) this.EyeLookController))
      {
        float angleHrate = this.EyeLookController.eyeLookScript.GetAngleHRate(EYE_LR.EYE_L);
        float angleVrate = this.EyeLookController.eyeLookScript.GetAngleVRate();
        float num1 = -Mathf.Max(this.EyeLookDownCorrect, this.EyeLookSideCorrect);
        float num2 = 1f - this.EyeLookUpCorrect;
        if ((double) num2 > (double) this.EyesCtrl.OpenMax)
          num2 = this.EyesCtrl.OpenMax;
        float num3 = Mathf.Clamp(((double) angleVrate <= 0.0 ? -MathfEx.LerpAccel(0.0f, this.EyeLookDownCorrect, -angleVrate) : MathfEx.LerpAccel(0.0f, this.EyeLookUpCorrect, angleVrate)) - MathfEx.LerpAccel(0.0f, this.EyeLookSideCorrect, angleHrate), num1, this.EyeLookUpCorrect) * (float) (1.0 - (1.0 - (double) this.EyesCtrl.OpenMax));
        this.EyesCtrl.SetCorrectOpenMax(num2 + num3);
      }
    }
    else
      blinkRate = -1f;
    this.EyebrowCtrl.CalcBlend(blinkRate);
    this.EyesCtrl.CalcBlend(blinkRate);
    this.MouthCtrl.CalcBlend(this.voiceValue);
  }

  public void SetVoiceVaule(float value)
  {
    this.voiceValue = value;
  }
}
