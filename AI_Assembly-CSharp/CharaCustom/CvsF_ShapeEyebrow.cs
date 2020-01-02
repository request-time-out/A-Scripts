// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_ShapeEyebrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_ShapeEyebrow : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssEyebrowW;
    [SerializeField]
    private CustomSliderSet ssEyebrowH;
    [SerializeField]
    private CustomSliderSet ssEyebrowX;
    [SerializeField]
    private CustomSliderSet ssEyebrowY;
    [SerializeField]
    private CustomSliderSet ssEyebrowTilt;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssEyebrowW.SetSliderValue((float) this.face.eyebrowLayout.z);
      this.ssEyebrowH.SetSliderValue((float) this.face.eyebrowLayout.w);
      this.ssEyebrowX.SetSliderValue((float) this.face.eyebrowLayout.x);
      this.ssEyebrowY.SetSliderValue((float) this.face.eyebrowLayout.y);
      this.ssEyebrowTilt.SetSliderValue(this.face.eyebrowTilt);
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
      return (IEnumerator) new CvsF_ShapeEyebrow.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFaceShapeEyebrow += new Action(((CvsBase) this).UpdateCustomUI);
      this.ssEyebrowW.onChange = (Action<float>) (value =>
      {
        this.face.eyebrowLayout = new Vector4((float) this.face.eyebrowLayout.x, (float) this.face.eyebrowLayout.y, value, (float) this.face.eyebrowLayout.w);
        this.chaCtrl.ChangeEyebrowLayout();
      });
      this.ssEyebrowW.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.eyebrowLayout.z);
      this.ssEyebrowH.onChange = (Action<float>) (value =>
      {
        this.face.eyebrowLayout = new Vector4((float) this.face.eyebrowLayout.x, (float) this.face.eyebrowLayout.y, (float) this.face.eyebrowLayout.z, value);
        this.chaCtrl.ChangeEyebrowLayout();
      });
      this.ssEyebrowH.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.eyebrowLayout.w);
      this.ssEyebrowX.onChange = (Action<float>) (value =>
      {
        this.face.eyebrowLayout = new Vector4(value, (float) this.face.eyebrowLayout.y, (float) this.face.eyebrowLayout.z, (float) this.face.eyebrowLayout.w);
        this.chaCtrl.ChangeEyebrowLayout();
      });
      this.ssEyebrowX.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.eyebrowLayout.x);
      this.ssEyebrowY.onChange = (Action<float>) (value =>
      {
        this.face.eyebrowLayout = new Vector4((float) this.face.eyebrowLayout.x, value, (float) this.face.eyebrowLayout.z, (float) this.face.eyebrowLayout.w);
        this.chaCtrl.ChangeEyebrowLayout();
      });
      this.ssEyebrowY.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.eyebrowLayout.y);
      this.ssEyebrowTilt.onChange = (Action<float>) (value =>
      {
        this.face.eyebrowTilt = value;
        this.chaCtrl.ChangeEyebrowTilt();
      });
      this.ssEyebrowTilt.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.eyebrowTilt);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
