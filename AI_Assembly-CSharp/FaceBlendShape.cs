// Decompiled with JetBrains decompiler
// Type: FaceBlendShape
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

public class FaceBlendShape : MonoBehaviour
{
  private FBSBlinkControl BlinkCtrlEx;
  public FBSBlinkControl BlinkCtrl;
  public FBSCtrlEyebrow EyebrowCtrl;
  public FBSCtrlEyes EyesCtrl;
  public FBSCtrlMouth MouthCtrl;
  private float voiceValue;
  public EyeLookController EyeLookController;
  [Range(0.0f, 1f)]
  public float EyeLookUpCorrect;
  [Range(0.0f, 1f)]
  public float EyeLookDownCorrect;
  [Range(0.0f, 1f)]
  public float EyeLookSideCorrect;

  public FaceBlendShape()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.EyebrowCtrl.Init();
    this.EyesCtrl.Init();
    this.MouthCtrl.Init();
  }

  public void SetBlinkControlEx(FBSBlinkControl ctrl)
  {
    this.BlinkCtrlEx = ctrl;
  }

  private void Start()
  {
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
  }

  private void OnLateUpdate()
  {
    this.BlinkCtrl.CalcBlink();
    FBSBlinkControl fbsBlinkControl = this.BlinkCtrl;
    if (this.BlinkCtrlEx != null)
      fbsBlinkControl = this.BlinkCtrlEx;
    float blinkRate;
    if (fbsBlinkControl.GetFixedFlags() == (byte) 0)
    {
      blinkRate = fbsBlinkControl.GetOpenRate();
      if (Object.op_Implicit((Object) this.EyeLookController))
      {
        float angleHrate = this.EyeLookController.eyeLookScript.GetAngleHRate(EYE_LR.EYE_L);
        float angleVrate = this.EyeLookController.eyeLookScript.GetAngleVRate();
        float num1 = -Mathf.Max(this.EyeLookDownCorrect, this.EyeLookSideCorrect);
        float num2 = 1f - this.EyeLookUpCorrect;
        if ((double) num2 > (double) this.EyesCtrl.OpenMax)
          num2 = this.EyesCtrl.OpenMax;
        float num3 = (double) angleVrate <= 0.0 ? -MathfEx.LerpAccel(0.0f, this.EyeLookDownCorrect, -angleVrate) : MathfEx.LerpAccel(0.0f, this.EyeLookUpCorrect, angleVrate);
        float num4 = Mathf.Clamp((double) angleHrate <= 0.0 ? num3 - MathfEx.LerpAccel(0.0f, this.EyeLookSideCorrect, -angleHrate) : num3 - MathfEx.LerpAccel(0.0f, this.EyeLookSideCorrect, angleHrate), num1, this.EyeLookUpCorrect) * (float) (1.0 - (1.0 - (double) this.EyesCtrl.OpenMax));
        this.EyesCtrl.SetCorrectOpenMax(num2 + num4);
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
