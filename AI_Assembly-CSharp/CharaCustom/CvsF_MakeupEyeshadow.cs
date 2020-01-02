// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_MakeupEyeshadow
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
  public class CvsF_MakeupEyeshadow : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscEyeshadowType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csEyeshadowColor;
    [SerializeField]
    private CustomSliderSet ssEyeshadowGloss;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssEyeshadowGloss.SetSliderValue(this.makeup.eyeshadowGloss);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscEyeshadowType.SetToggleID(this.makeup.eyeshadowId);
      this.csEyeshadowColor.SetColor(this.makeup.eyeshadowColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_MakeupEyeshadow.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyeshadow += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscEyeshadowType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eyeshadow, ChaListDefine.KeyType.Unknown));
      this.sscEyeshadowType.SetToggleID(this.makeup.eyeshadowId);
      this.sscEyeshadowType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.makeup.eyeshadowId == info.id)
          return;
        this.makeup.eyeshadowId = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(false, true, false, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.csEyeshadowColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.makeup.eyeshadowColor = color;
        this.chaCtrl.AddUpdateCMFaceColorFlags(false, true, false, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssEyeshadowGloss.onChange = (Action<float>) (value =>
      {
        this.makeup.eyeshadowGloss = value;
        this.chaCtrl.AddUpdateCMFaceGlossFlags(true, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssEyeshadowGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.eyeshadowGloss);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
