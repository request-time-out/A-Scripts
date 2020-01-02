// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_EyeHL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_EyeHL : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscEyeHLType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csEyeHLColor;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomSliderSet ssHLW;
    [SerializeField]
    private CustomSliderSet ssHLH;
    [SerializeField]
    private CustomSliderSet ssHLX;
    [SerializeField]
    private CustomSliderSet ssHLY;
    [SerializeField]
    private CustomSliderSet ssHLTilt;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssHLW.SetSliderValue((float) this.face.hlLayout.x);
      this.ssHLH.SetSliderValue((float) this.face.hlLayout.y);
      this.ssHLX.SetSliderValue((float) this.face.hlLayout.z);
      this.ssHLY.SetSliderValue((float) this.face.hlLayout.w);
      this.ssHLTilt.SetSliderValue(this.face.hlTilt);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscEyeHLType.SetToggleID(this.face.hlId);
      this.csEyeHLColor.SetColor(this.face.hlColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_EyeHL.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyeHL += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscEyeHLType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eye_hl, ChaListDefine.KeyType.Unknown));
      this.sscEyeHLType.SetToggleID(this.face.hlId);
      this.sscEyeHLType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.hlId == info.id)
          return;
        this.face.hlId = info.id;
        this.chaCtrl.ChangeEyesHighlightKind();
      });
      this.csEyeHLColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.hlColor = color;
        this.chaCtrl.ChangeEyesHighlightColor();
      });
      this.ssHLW.onChange = (Action<float>) (value =>
      {
        this.face.hlLayout = new Vector4(value, (float) this.face.hlLayout.y, (float) this.face.hlLayout.z, (float) this.face.hlLayout.w);
        this.chaCtrl.ChangeEyesHighlighLayout();
      });
      this.ssHLW.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.hlLayout.x);
      this.ssHLH.onChange = (Action<float>) (value =>
      {
        this.face.hlLayout = new Vector4((float) this.face.hlLayout.x, value, (float) this.face.hlLayout.z, (float) this.face.hlLayout.w);
        this.chaCtrl.ChangeEyesHighlighLayout();
      });
      this.ssHLH.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.hlLayout.y);
      this.ssHLX.onChange = (Action<float>) (value =>
      {
        this.face.hlLayout = new Vector4((float) this.face.hlLayout.x, (float) this.face.hlLayout.y, value, (float) this.face.hlLayout.w);
        this.chaCtrl.ChangeEyesHighlighLayout();
      });
      this.ssHLX.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.hlLayout.z);
      this.ssHLY.onChange = (Action<float>) (value =>
      {
        this.face.hlLayout = new Vector4((float) this.face.hlLayout.x, (float) this.face.hlLayout.y, (float) this.face.hlLayout.z, value);
        this.chaCtrl.ChangeEyesHighlighLayout();
      });
      this.ssHLY.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.hlLayout.w);
      this.ssHLTilt.onChange = (Action<float>) (value =>
      {
        this.face.hlTilt = value;
        this.chaCtrl.ChangeEyesHighlighTilt();
      });
      this.ssHLTilt.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.hlTilt);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
