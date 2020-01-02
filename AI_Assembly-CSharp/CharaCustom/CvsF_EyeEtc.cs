// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_EyeEtc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_EyeEtc : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssPupilY;
    [SerializeField]
    private CustomSliderSet ssShadowScale;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssPupilY.SetSliderValue(this.face.pupilY);
      this.ssShadowScale.SetSliderValue(this.face.whiteShadowScale);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_EyeEtc.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyeEtc += new Action(((CvsBase) this).UpdateCustomUI);
      this.ssPupilY.onChange = (Action<float>) (value =>
      {
        this.face.pupilY = value;
        this.chaCtrl.ChangeEyesBasePosY();
      });
      this.ssPupilY.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupilY);
      this.ssShadowScale.onChange = (Action<float>) (value =>
      {
        this.face.whiteShadowScale = value;
        this.chaCtrl.ChangeEyesShadowRange();
      });
      this.ssShadowScale.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.whiteShadowScale);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
