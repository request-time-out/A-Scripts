// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_MakeupLip
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
  public class CvsF_MakeupLip : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscLipType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csLipColor;
    [SerializeField]
    private CustomSliderSet ssLipGloss;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssLipGloss.SetSliderValue(this.makeup.lipGloss);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscLipType.SetToggleID(this.makeup.lipId);
      this.csLipColor.SetColor(this.makeup.lipColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_MakeupLip.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsLip += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscLipType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_lip, ChaListDefine.KeyType.Unknown));
      this.sscLipType.SetToggleID(this.makeup.lipId);
      this.sscLipType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.makeup.lipId == info.id)
          return;
        this.makeup.lipId = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(false, false, false, false, false, true, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.csLipColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.makeup.lipColor = color;
        this.chaCtrl.AddUpdateCMFaceColorFlags(false, false, false, false, false, true, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssLipGloss.onChange = (Action<float>) (value =>
      {
        this.makeup.lipGloss = value;
        this.chaCtrl.AddUpdateCMFaceGlossFlags(false, false, false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssLipGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.lipGloss);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
