// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Nail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_Nail : CvsBase
  {
    [SerializeField]
    private CustomColorSet csNailColor;
    [SerializeField]
    private CustomSliderSet ssNailGloss;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssNailGloss.SetSliderValue(this.body.nailGlossPower);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.csNailColor.SetColor(this.body.nailColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Nail.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsNail += new Action(((CvsBase) this).UpdateCustomUI);
      this.csNailColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.nailColor = color;
        this.chaCtrl.ChangeNailColor();
      });
      this.ssNailGloss.onChange = (Action<float>) (value =>
      {
        this.body.nailGlossPower = value;
        this.chaCtrl.ChangeNailGloss();
      });
      this.ssNailGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.nailGlossPower);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
