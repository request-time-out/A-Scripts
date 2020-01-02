// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_MakeupCheek
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
  public class CvsF_MakeupCheek : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscCheekType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csCheekColor;
    [SerializeField]
    private CustomSliderSet ssCheekGloss;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssCheekGloss.SetSliderValue(this.makeup.cheekGloss);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscCheekType.SetToggleID(this.makeup.cheekId);
      this.csCheekColor.SetColor(this.makeup.cheekColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_MakeupCheek.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsCheek += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscCheekType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_cheek, ChaListDefine.KeyType.Unknown));
      this.sscCheekType.SetToggleID(this.makeup.cheekId);
      this.sscCheekType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.makeup.cheekId == info.id)
          return;
        this.makeup.cheekId = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(false, false, false, false, true, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.csCheekColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.makeup.cheekColor = color;
        this.chaCtrl.AddUpdateCMFaceColorFlags(false, false, false, false, true, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssCheekGloss.onChange = (Action<float>) (value =>
      {
        this.makeup.cheekGloss = value;
        this.chaCtrl.AddUpdateCMFaceGlossFlags(false, false, false, true, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssCheekGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.cheekGloss);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
