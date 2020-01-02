// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_EyeLR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_EyeLR : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomColorSet csWhiteColor;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscPupilType;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomColorSet csPupilColor;
    [SerializeField]
    private CustomSliderSet ssPupilEmission;
    [SerializeField]
    private CustomSliderSet ssPupilW;
    [SerializeField]
    private CustomSliderSet ssPupilH;
    [Header("【設定04】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscBlackType;
    [Header("【設定05】----------------------")]
    [SerializeField]
    private CustomColorSet csBlackColor;
    [SerializeField]
    private CustomSliderSet ssBlackW;
    [SerializeField]
    private CustomSliderSet ssBlackH;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssPupilEmission.SetSliderValue(this.face.pupil[this.SNo].pupilEmission);
      this.ssPupilW.SetSliderValue(this.face.pupil[this.SNo].pupilW);
      this.ssPupilH.SetSliderValue(this.face.pupil[this.SNo].pupilH);
      this.ssBlackW.SetSliderValue(this.face.pupil[this.SNo].blackW);
      this.ssBlackH.SetSliderValue(this.face.pupil[this.SNo].blackH);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscPupilType.SetToggleID(this.face.pupil[this.SNo].pupilId);
      this.sscBlackType.SetToggleID(this.face.pupil[this.SNo].blackId);
      this.csWhiteColor.SetColor(this.face.pupil[this.SNo].whiteColor);
      this.csPupilColor.SetColor(this.face.pupil[this.SNo].pupilColor);
      this.csBlackColor.SetColor(this.face.pupil[this.SNo].blackColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_EyeLR.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyeLR += new Action(((CvsBase) this).UpdateCustomUI);
      this.csWhiteColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.pupil[this.SNo].whiteColor = color;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].whiteColor = color;
        this.chaCtrl.ChangeWhiteEyesColor(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.sscPupilType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eye, ChaListDefine.KeyType.Unknown));
      this.sscPupilType.SetToggleID(this.face.pupil[this.SNo].pupilId);
      this.sscPupilType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.pupil[this.SNo].pupilId == info.id)
          return;
        this.face.pupil[this.SNo].pupilId = info.id;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].pupilId = info.id;
        this.chaCtrl.ChangeEyesKind(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.csPupilColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.pupil[this.SNo].pupilColor = color;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].pupilColor = color;
        this.chaCtrl.ChangeEyesColor(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssPupilEmission.onChange = (Action<float>) (value =>
      {
        this.face.pupil[this.SNo].pupilEmission = value;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].pupilEmission = value;
        this.chaCtrl.ChangeEyesEmission(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssPupilEmission.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupil[this.SNo].pupilEmission);
      this.ssPupilW.onChange = (Action<float>) (value =>
      {
        this.face.pupil[this.SNo].pupilW = value;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].pupilW = value;
        this.chaCtrl.ChangeEyesWH(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssPupilW.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupil[this.SNo].pupilW);
      this.ssPupilH.onChange = (Action<float>) (value =>
      {
        this.face.pupil[this.SNo].pupilH = value;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].pupilH = value;
        this.chaCtrl.ChangeEyesWH(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssPupilH.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupil[this.SNo].pupilH);
      this.sscBlackType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eyeblack, ChaListDefine.KeyType.Unknown));
      this.sscBlackType.SetToggleID(this.face.pupil[this.SNo].blackId);
      this.sscBlackType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.pupil[this.SNo].blackId == info.id)
          return;
        this.face.pupil[this.SNo].blackId = info.id;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].blackId = info.id;
        this.chaCtrl.ChangeBlackEyesKind(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.csBlackColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.pupil[this.SNo].blackColor = color;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].blackColor = color;
        this.chaCtrl.ChangeBlackEyesColor(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssBlackW.onChange = (Action<float>) (value =>
      {
        this.face.pupil[this.SNo].blackW = value;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].blackW = value;
        this.chaCtrl.ChangeBlackEyesWH(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssBlackW.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupil[this.SNo].blackW);
      this.ssBlackH.onChange = (Action<float>) (value =>
      {
        this.face.pupil[this.SNo].blackH = value;
        if (this.face.pupilSameSetting)
          this.face.pupil[this.SNo ^ 1].blackH = value;
        this.chaCtrl.ChangeBlackEyesWH(!this.face.pupilSameSetting ? this.SNo : 2);
      });
      this.ssBlackH.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.pupil[this.SNo].blackH);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
