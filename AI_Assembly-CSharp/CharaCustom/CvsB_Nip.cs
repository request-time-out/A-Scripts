// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Nip
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
  public class CvsB_Nip : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscNipType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csNipColor;
    [SerializeField]
    private CustomSliderSet ssNipGloss;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssNipGloss.SetSliderValue(this.body.nipGlossPower);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscNipType.SetToggleID(this.body.nipId);
      this.csNipColor.SetColor(this.body.nipColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Nip.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsNip += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscNipType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_nip, ChaListDefine.KeyType.Unknown));
      this.sscNipType.SetToggleID(this.body.nipId);
      this.sscNipType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.nipId == info.id)
          return;
        this.body.nipId = info.id;
        this.chaCtrl.ChangeNipKind();
      });
      this.csNipColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.nipColor = color;
        this.chaCtrl.ChangeNipColor();
      });
      this.ssNipGloss.onChange = (Action<float>) (value =>
      {
        this.body.nipGlossPower = value;
        this.chaCtrl.ChangeNipGloss();
      });
      this.ssNipGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.nipGlossPower);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
